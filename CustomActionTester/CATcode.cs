﻿using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json.Linq;
using Rappen.XRM.Helpers.Extensions;
using Rappen.XTB.Helpers.ControlItems;
using Rappen.XTB.Helpers.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Rappen.XTB.CAT
{
    using ParamType = Rappen.XTB.CAT.Customapirequestparameter.Type_OptionSet;

    public partial class CustomActionTester : IGitHubPlugin, IAboutPlugin, IShortcutReceiver, IMessageBusHost
    {
        #region Public Events

        public event EventHandler<MessageBusEventArgs> OnOutgoingMessage;

        #endregion Public Events

        #region Private Fields

        private EntityMetadataProxy[] entities;
        private Guid InArgumentId = Guid.Empty;
        private CATRequest historyrequest;
        private bool _uiupdated;

        #endregion Private Fields

        #region Public Properties

        public string RepositoryName => "CustomActionTester";

        public string UserName => "rappen";

        #endregion Public Properties

        #region Public Methods

        public void OnIncomingMessage(MessageBusEventArgs message)
        {
            if (message.TargetArgument is string arg && Guid.TryParse(arg, out Guid argid))
            {
                InArgumentId = argid;
                if (cmbSolution.DataSource != null)
                {
                    SelectDefaultSolution();
                    GetCustomActions(cmbSolution.SelectedRecord);
                }
            }
        }

        public void ReceiveKeyDownShortcut(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5 && btnExecute.Enabled)
            {
                btnExecute_Click(null, null);
            }
        }

        public void ReceiveKeyPressShortcut(KeyPressEventArgs e)
        {
            // Noop
        }

        public void ReceiveKeyUpShortcut(KeyEventArgs e)
        {
            // Noop
        }

        public void ReceivePreviewKeyDownShortcut(PreviewKeyDownEventArgs e)
        {
            // Noop
        }

        public void ShowAboutDialog()
        {
            var about = new About(this)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            about.imgLogo.Image = catTool.LogoAbout;
            about.lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            about.ShowDialog();
        }

        #endregion Public Methods

        #region Internal Methods

        internal void LogUse(string action, double? count = null, double? duration = null)
        {
            ai.WriteEvent(action, count, duration, HandleAIResult);
        }

        #endregion Internal Methods

        #region Private Methods

        private void ClearOutputParamValues()
        {
            var outputs = gridOutputParams.DataSource as IEnumerable<Entity>;
            outputs.ToList().ForEach(o => o.Attributes.Remove("value"));
            outputs.ToList().ForEach(o => o.Attributes.Remove("rawvalue"));
        }

        private void ExecuteCA()
        {
            txtExecution.Text = string.Empty;
            ClearOutputParamValues();
            var request = GetRequest();
            var catreq = new CATRequest(request);
            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Executing {catTool.Target}",
                Work = (worker, args) =>
                {
                    var sw = Stopwatch.StartNew();
                    var result = Service.Execute(request);
                    sw.Stop();
                    catreq.Execution.Duration = sw.ElapsedMilliseconds;
                    args.Result = new Tuple<OrganizationResponse, long>(result, sw.ElapsedMilliseconds);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        catreq.Execution.ErrorMessage = args.Error.Message;
                        ShowErrorDialog(args.Error);
                    }
                    else if (args.Result is Tuple<OrganizationResponse, long> response)
                    {
                        txtExecution.Text = $"{response.Item2} ms";
                        btnPTV.Enabled = true;
                        catreq.Response = response.Item1;
                        var outputparams = response.Item1.Results;
                        PopulateOutputParamValues(outputparams);
                    }
                    AddHistoryToList(catreq);
                }
            });
        }

        private OrganizationRequest GetRequest()
        {
            var request = new OrganizationRequest(GetApiMessage());
            if (rhScopeRecord.Record?.ToEntityReference() != null)
            {
                request.Parameters["Target"] = rhScopeRecord.Record?.ToEntityReference();
            }
            foreach (var input in gridInputParams.DataSource as IEnumerable<Entity>)
            {
                if (!input.TryGetAttributeValue(catTool.Columns.ParamUniqueName, out string name))
                {
                    continue;
                }
                if (input.TryGetAttributeValue("rawvalue", out object value))
                {
                    request[name] = value;
                }
                else if (input.TryGetAttributeValue("isoptional", out bool optional) && !optional)
                {
                    MessageBox.Show($"Missing value for required parameter: {name}", $"Execute {catTool.Target}", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return null;
                }
            }
            return request;
        }

        private void FormatResultDetail()
        {
            var value = GetResultDetailValue();
            if (value is string strvalue)
            {
                if (!string.IsNullOrEmpty(strvalue))
                {
                    try
                    {
                        if (rbFormatJSON.Checked)
                        {
                            var parsedJson = JToken.Parse(strvalue);
                            strvalue = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
                        }
                        else if (rbFormatXML.Checked)
                        {
                            var string_writer = new StringWriter();
                            var xml_text_writer = new XmlTextWriter(string_writer);
                            xml_text_writer.Formatting = Formatting.Indented;
                            strvalue = string_writer.ToString();
                            if (string.IsNullOrWhiteSpace(strvalue))
                            {
                                throw new XmlException("Unable to parse XML");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        rbFormatText.Checked = true;
                    }
                }
                txtResultDetail.Text = strvalue;
            }
        }

        private void FormatResultDetailDefault()
        {
            var value = GetResultDetailValue();
            if (value is string strvalue)
            {
                if (string.IsNullOrEmpty(strvalue))
                {
                    return;
                }
                if (strvalue.StartsWith("{"))
                {
                    rbFormatJSON.Checked = true;
                }
                else if (strvalue.StartsWith("<"))
                {
                    rbFormatXML.Checked = true;
                }
            }
            FormatResultDetail();
        }

        private string GetApiMessage()
        {
            return cmbCustomActions.SelectedRecord.PropertyAsBaseType(catTool.Columns.APIMessageName, string.Empty, true) as string;
        }

        private string GetBoundEntity()
        {
            if (rhCustomAction.Record == null || !rhCustomAction.Record.TryGetAttributeValue(catTool.Columns.APIBoundEntity, out string entity))
            {
                return string.Empty;
            }
            return entity.Trim();
        }

        private void GetCustomActions(Entity solution) => GetCustomActions(solution?.Id ?? Guid.Empty);

        private void GetCustomActions(Guid solutionid, Action<int> nextstep = null, int nextstepnum = 0)
        {
            if (_uiupdated)
            {
                return;
            }
            _uiupdated = true;
            cmbSolution.SetSelected(solutionid);
            _uiupdated = false;
            cmbCustomActions.DataSource = null;
            var qx = catTool.GetActionQuery(solutionid);
            if (qx == null)
            {
                return;
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = $"Getting {catTool.Target}s",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(qx);
                },
                PostWorkCallBack = (args) =>
                {
                    ShowErrorDialog(args.Error);
                    if (args.Error == null && args.Result is EntityCollection actions)
                    {
                        cmbCustomActions.DataSource = actions;
                        if (!InArgumentId.Equals(Guid.Empty))
                        {
                            SelectActionByInArgumentId();
                        }
                        SetCustomAction(cmbCustomActions.SelectedRecord);
                        nextstep?.Invoke(nextstepnum);
                    }
                }
            });
        }

        private void SelectActionByInArgumentId()
        {
            cmbCustomActions.SelectedItem = cmbCustomActions.Items
                .OfType<EntityItem>()
                .FirstOrDefault(e =>
                    e.Entity.Id.Equals(InArgumentId));
            if (cmbCustomActions.SelectedItem != null)
            {
                InArgumentId = Guid.Empty;
            }
        }

        private void GetInputParams(Guid caid, Action<int> nextstep = null, int nextstepnum = 0)
        {
            if (caid.Equals(Guid.Empty))
            {
                gridInputParams.DataSource = null;
                gridOutputParams.DataSource = null;
                return;
            }
            var qx = catTool.GetInputQuery(caid);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Input Parameters",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(qx);
                },
                PostWorkCallBack = (args) =>
                {
                    ShowErrorDialog(args.Error);
                    if (args.Error == null && args.Result is EntityCollection inputs)
                    {
                        PreProcessParams(inputs);
                        gridInputParams.DataSource = inputs;
                        gridInputParams.AutoResizeColumns();
                        btnExecute.Enabled = ReadyToExecute();
                        GetOutputParams(caid);
                        nextstep?.Invoke(nextstepnum);
                    }
                }
            });
        }

        private void PopulateInputParamValue(Entity input)
        {
            panCAValue.Tag = input;
            panCAValue.Visible = ParseInput(input);
        }

        private void SetInputParamValue(bool clear)
        {
            if (!(panCAValue.Tag is Entity input))
            {
                return;
            }
            if (clear)
            {
                input.RemoveProperty("rawvalue");
                input.RemoveProperty("value");
            }
            else
            {
                if (!HandleInput(input))
                {
                    return;
                }
            }
            RefreshInputParameters(input);
        }

        private void RefreshInputParameters(Entity input)
        {
            SuspendLayout();
            gridInputParams.Refresh();
            gridInputParams.AutoResizeColumns();
            gridInputParams.Rows.OfType<DataGridViewRow>()
                .Where(r => r.Cells["#entity"].Value == input)
                .ToList()
                .ForEach(r => r.Selected = true);
            PopulateInputParamValue(input);
            ResumeLayout();
            btnExecute.Enabled = ReadyToExecute();
        }

        private void GetOutputParams(Guid caid)
        {
            if (caid.Equals(Guid.Empty))
            {
                gridOutputParams.DataSource = null;
                return;
            }
            var qx = catTool.GetOutputQuery(caid);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Output Parameters",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(qx);
                },
                PostWorkCallBack = (args) =>
                {
                    ShowErrorDialog(args.Error);
                    if (args.Error == null && args.Result is EntityCollection outputs)
                    {
                        PreProcessParams(outputs);
                        gridOutputParams.DataSource = outputs;
                        gridOutputParams.AutoResizeColumns();
                    }
                }
            });
        }

        private object GetResultDetailValue()
        {
            panTextFormat.Visible = false;
            var record = gridOutputParams.SelectedCellRecords.FirstOrDefault();
            if (record == null || !record.TryGetAttributeValue("rawvalue", out object result))
            {
                txtResultDetail.Text = string.Empty;
                return null;
            }
            lblResultDetailType.Text = CATRequest.ValueToParamType(result)?.ToString();
            if (result is string)
            {
                panTextFormat.Visible = true;
                return result;
            }
            return CATRequest.ValueToString(result);
        }

        private void GetSolutions(bool managed, Action<int> nextstep = null, int nextstepnum = 0)
        {
            if (_uiupdated)
            {
                return;
            }
            if (managed != rbSolManaged.Checked)
            {
                _uiupdated = true;
                rbSolManaged.Checked = managed;
                rbSolUnmanaged.Checked = !managed;
                _uiupdated = false;
            }
            var qx = new QueryExpression(Solution.EntityName);
            qx.Distinct = true;
            qx.ColumnSet.AddColumns(Solution.PrimaryKey, Solution.PrimaryName, Solution.UniqueName, Solution.Version);
            qx.AddOrder(Solution.PrimaryName, OrderType.Ascending);
            qx.Criteria.AddCondition(Solution.Ismanaged, ConditionOperator.Equal, managed);
            qx.Criteria.AddCondition(Solution.Isvisible, ConditionOperator.Equal, true);
            catTool.AddSolutionFilter(qx);
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting Solutions",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(qx);
                },
                PostWorkCallBack = (args) =>
                {
                    ShowErrorDialog(args.Error);
                    if (args.Error == null && args.Result is EntityCollection solutions)
                    {
                        cmbSolution.DataSource = solutions;
                        if (!InArgumentId.Equals(Guid.Empty))
                        {
                            SelectDefaultSolution();
                        }
                        GetCustomActions(cmbSolution.SelectedRecord);
                        nextstep?.Invoke(nextstepnum);
                    }
                }
            });
        }

        private void SelectDefaultSolution()
        {
            cmbSolution.SelectedItem = cmbSolution.Items
                .OfType<EntityItem>()
                .FirstOrDefault(e =>
                    e.Entity.TryGetAttributeValue<string>(Solution.UniqueName, out string uniquename) &&
                    uniquename.Equals("Default"));
        }

        private void HandleAIResult(string result)
        {
            if (!string.IsNullOrEmpty(result))
            {
                LogError("Failed to write to Application Insights:\n{0}", result);
            }
        }

        private void LoadEntities()
        {
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading entities...",
                Work = (worker, eventargs) =>
                {
                    eventargs.Result = MetadataHelper.LoadEntities(Service);
                },
                PostWorkCallBack = (args) =>
                {
                    ShowErrorDialog(args.Error);
                    if (args.Error == null && args.Result is RetrieveMetadataChangesResponse resp)
                    {
                        entities = resp.EntityMetadata
                            .Where(e => e.IsCustomizable.Value == true && e.IsIntersect.Value != true)
                            .Select(m => new EntityMetadataProxy(m))
                            .OrderBy(e => e.ToString()).ToArray();
                        cmbEntity.Items.AddRange(entities);
                    }
                }
            });
        }

        private void LookupBoundRecord()
        {
            var entity = GetBoundEntity();
            rhScopeRecord.Record = LookupRecord(entity);
            btnExecute.Enabled = ReadyToExecute();
        }

        private void LookupInputParamRecord()
        {
            if (!(cmbEntity.SelectedItem is EntityMetadataProxy entity))
            {
                return;
            }
            var entityname = entity.Metadata.LogicalName;
            rhRecord.Record = LookupRecord(entityname);
        }

        private Entity LookupRecord(string entityname)
        {
            if (string.IsNullOrWhiteSpace(entityname))
            {
                return null;
            }
            var lkp = new XRMLookupDialog
            {
                Service = Service,
                LogicalName = entityname
            };
            if (lkp.ShowDialog(this) == DialogResult.OK)
            {
                return lkp.Record;
            }
            return null;
        }

        private void OpenAction(Entity ca)
        {
            var url = GetFullWebApplicationUrl();
            url = string.Concat(url,
                url.EndsWith("/") ? "" : "/",
                catTool.GetActionUrlPath(ca.Id));
            Process.Start(url);
        }

        private void ManageAction(Entity ca)
        {
            OnOutgoingMessage(this, new MessageBusEventArgs(catTool.ManagerTool, true) { TargetArgument = ca.Id.ToString() });
        }

        private string GetFullWebApplicationUrl()
        {
            var url = ConnectionDetail.WebApplicationUrl;
            if (string.IsNullOrEmpty(url))
            {
                url = ConnectionDetail.ServerName;
            }
            if (!url.ToLower().StartsWith("http"))
            {
                url = string.Concat("http://", url);
            }
            var uri = new Uri(url);
            if (!uri.Host.EndsWith(".dynamics.com"))
            {
                if (string.IsNullOrEmpty(uri.AbsolutePath.Trim('/')))
                {
                    uri = new Uri(uri, ConnectionDetail.Organization);
                }
            }
            return uri.ToString();
        }

        private void PopulateOutputParamValues(ParameterCollection outputparams)
        {
            var outputs = gridOutputParams.DataSource as IEnumerable<Entity>;
            foreach (var result in outputparams)
            {
                var output = outputs
                    .FirstOrDefault(o => o.Contains(catTool.Columns.ParamUniqueName) &&
                        o[catTool.Columns.ParamUniqueName].ToString().Equals(result.Key));
                if (output == null)
                {
                    continue;
                }
                var rawvalue = result.Value;
                var value = rawvalue;
                output["rawvalue"] = rawvalue;
                if (rawvalue is string[] strings)
                {
                    value = string.Join(", ", strings).Replace("\n", " ");
                }
                else if (rawvalue is Money money)
                {
                    value = money.Value;
                }
                else if (rawvalue is OptionSetValue osv)
                {
                    value = osv.Value;
                }
                else if (rawvalue is Entity entity)
                {
                    rhResult.Record = entity;
                    value = txtCDSDataHelper.Text;
                }
                else if (rawvalue is EntityReference entref)
                {
                    rhResult.LogicalName = entref.LogicalName;
                    rhResult.Id = entref.Id;
                    value = txtCDSDataHelper.Text;
                }
                output["value"] = value;
            }
            gridOutputParams.Refresh();
            gridOutputParams.AutoResizeColumns();
            FormatResultDetailDefault();
        }

        private void PreProcessParams(EntityCollection records)
        {
            catTool.PreProcessParams(records, entities);
            records.Entities.Where(e => !e.Contains("position")).ToList().ForEach(e => e["postition"] =
                records.Entities.Any(e2 => e2.Contains("position")) ?
                    records.Entities.Where(e3 => e3.Contains("position")).Max(e3 => (int)e3["position"]) + 1 : 0);
            records.Entities
                .Where(e => e.TryGetAttributeValue("entity", out EntityMetadataProxy meta))
                .ToList().ForEach(e => e["subtype"] = e.GetAttributeValue<EntityMetadataProxy>("entity").DisplayName);
        }

        private bool ReadyToExecute()
        {
            if (cmbCustomActions.SelectedRecord == null || !(gridInputParams.DataSource is IEnumerable<Entity> inputparams))
            {
                return false;
            }
            if (!string.IsNullOrWhiteSpace(GetBoundEntity()) && rhScopeRecord.Record == null)
            {
                return false;
            }
            foreach (var inputparam in inputparams)
            {
                if (inputparam.TryGetAttributeValue("isoptional", out bool optional) && !optional &&
                    !inputparam.TryGetAttributeValue("rawvalue", out object _))
                {
                    return false;
                }
            }
            return true;
        }

        private void SetCustomAction(Entity ca)
        {
            btnOpenAction.Enabled = ca != null;
            btnManage.Enabled = ca != null;
            if (ca == null)
            {
                return;
            }
            rhCustomAction.Record = ca;
            rhScopeRecord.Record = null;
            var bindingtype = catTool.BindingType(ca);
            panCARecord.Visible = bindingtype != Customapi.BindingType_OptionSet.Global;
            txtExecution.Text = string.Empty;
            GetInputParams(ca.Id);
        }

        private void TraceLastExecution()
        {
            OnOutgoingMessage(this, new MessageBusEventArgs("Plugin Trace Viewer", true) { TargetArgument = $"Message={GetApiMessage()}" });
        }

        private void RefreshLayout()
        {
            var unique = mnuShowUnique.Checked;
            cmbSolution.DisplayFormat = "{" + (unique ? Solution.UniqueName : Solution.PrimaryName) + "} {" + Solution.Version + "}";
            cmbCustomActions.DisplayFormat = unique ? catTool.Columns.APIUniqueName : catTool.Columns.APIName;
            txtScope.DisplayFormat = "{" + catTool.Columns.APIScope + "} {" + catTool.Columns.APIBoundEntity + "}";
            dgInputsName.DataPropertyName = unique ? catTool.Columns.ParamUniqueName : catTool.Columns.ParamName;
            dgOutputsName.DataPropertyName = unique ? catTool.Columns.ParamUniqueName : catTool.Columns.ParamName;
            gridInputParams.Refresh();
            gridOutputParams.Refresh();
        }

        private static ParamType? GetType(Entity input)
        {
            if (input.TryGetAttributeValue("type", out string typestr))
            {
                try
                {
                    return CATRequest.StringToParamType(typestr);
                }
                catch { }
            }
            if (input.TryGetAttributeValue("type", out OptionSetValue typeosv))
            {
                return (ParamType)typeosv.Value;
            }
            return null;
        }

        private bool ParseInput(Entity input)
        {
            panString.Visible = false;
            panEntity.Visible = false;
            panBoolean.Visible = false;
            panDateTime.Visible = false;
            txtString.Text = string.Empty;
            chkBoolean.Checked = false;
            dtDateTime.Value = DateTime.Today;
            cmbEntity.SelectedIndex = -1;
            rhRecord.Record = null;
            if (input == null)
            {
                return false;
            }
            var currentvalue = input.Contains("rawvalue") ? input["rawvalue"] : null;
            var type = GetType(input);
            switch (type)
            {
                case ParamType.String:
                case ParamType.Integer:
                case ParamType.Decimal:
                case ParamType.Float:
                    panString.Visible = true;
                    txtString.Text = currentvalue?.ToString();
                    break;

                case ParamType.Money:
                    panString.Visible = true;
                    if (currentvalue is Money money)
                    {
                        txtString.Text = money.Value.ToString();
                    }
                    break;

                case ParamType.GuId:
                    panString.Visible = true;
                    txtString.Text = currentvalue?.ToString() ?? Guid.Empty.ToString();
                    break;

                case ParamType.Picklist:
                    panString.Visible = true;
                    if (currentvalue is OptionSetValue osv)
                    {
                        txtString.Text = osv.Value.ToString();
                    }
                    break;

                case ParamType.Entity:
                case ParamType.EntityReference:
                    panEntity.Visible = true;
                    if (input.TryGetAttributeValue("entity", out EntityMetadataProxy entitytype) &&
                        cmbEntity.Items.Cast<EntityMetadataProxy>().FirstOrDefault(e => e.Metadata.LogicalName == entitytype.Metadata.LogicalName) is EntityMetadataProxy selentity)
                    {
                        cmbEntity.SelectedItem = selentity;
                        cmbEntity.Enabled = false;
                    }
                    else
                    {
                        cmbEntity.Enabled = true;
                    }
                    if (currentvalue is Entity entity)
                    {
                        cmbEntity.SelectedItem = cmbEntity.Items.Cast<EntityMetadataProxy>().FirstOrDefault(e => e.Metadata.LogicalName == entity.LogicalName);
                        rhRecord.Record = entity;
                    }
                    else if (currentvalue is EntityReference entref)
                    {
                        cmbEntity.SelectedItem = cmbEntity.Items.Cast<EntityMetadataProxy>().FirstOrDefault(e => e.Metadata.LogicalName == entref.LogicalName);
                        rhRecord.LogicalName = entref.LogicalName;
                        rhRecord.Id = entref.Id;
                    }
                    break;

                case ParamType.Boolean:
                    panBoolean.Visible = true;
                    chkBoolean.Checked = currentvalue is bool boolval && boolval;
                    break;

                case ParamType.DateTime:
                    panDateTime.Visible = true;
                    if (currentvalue is DateTime dtvalue)
                    {
                        dtDateTime.Value = dtvalue;
                    }
                    break;

                default:
                    return false;
            }
            return true;
        }

        private bool HandleInput(Entity input)
        {
            var invalidstr = false;
            object result = null;
            object formattedresult = null;
            var type = GetType(input);
            switch (type)
            {
                case ParamType.String:
                    result = txtString.Text;
                    break;

                case ParamType.Integer:
                    if (!int.TryParse(txtString.Text, out int intvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    result = intvalue;
                    break;

                case ParamType.Decimal:
                case ParamType.Money:
                    if (!decimal.TryParse(txtString.Text, out decimal decvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    if (type == ParamType.Money)
                    {
                        result = new Money(decvalue);
                        formattedresult = decvalue;
                    }
                    else
                    {
                        result = decvalue;
                    }
                    break;

                case ParamType.Float:
                    if (!double.TryParse(txtString.Text, out double douvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    result = douvalue;
                    break;

                case ParamType.Picklist:
                    if (!int.TryParse(txtString.Text, out int osvvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    result = new OptionSetValue(osvvalue);
                    formattedresult = osvvalue.ToString();
                    break;

                case ParamType.GuId:
                    if (!Guid.TryParse(txtString.Text, out Guid guidvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    result = guidvalue;
                    break;

                case ParamType.Entity:
                case ParamType.EntityReference:
                    if (rhRecord.Record == null)
                    {
                        MessageBox.Show($"No record selected", "Input Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (type == ParamType.Entity)
                    {
                        result = rhRecord.Record;
                    }
                    else if (type == ParamType.EntityReference)
                    {
                        result = rhRecord.Record.ToEntityReference();
                    }
                    formattedresult = txtRecord.Text;
                    break;

                case ParamType.Boolean:
                    result = chkBoolean.Checked;
                    break;

                case ParamType.DateTime:
                    result = dtDateTime.Value;
                    break;
            }
            if (invalidstr)
            {
                MessageBox.Show($"Not a valid {type}: {txtString.Text}", "Input Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            formattedresult = formattedresult ?? result;
            input["rawvalue"] = result;
            input["value"] = formattedresult;
            return true;
        }

        private void LoadAndShowHistoryIfNeeded()
        {
            picHistoryOpen.Visible = splitToolHistory.Panel2Collapsed;
            picHistoryClose.Visible = !splitToolHistory.Panel2Collapsed;
            panHistoryOptions.Visible = !splitToolHistory.Panel2Collapsed;
            if (!splitToolHistory.Panel2Collapsed)
            {
                ShowHistory(GetHistoryFromFile());
            }
        }

        private void AddHistoryToList(CATRequest catreq)
        {
            if (catreq.Execution == null)
            {
                catreq.Execution = new ExecutionInfo();
            }
            catreq.Execution.Environment = ConnectionDetail.ConnectionName;
            if (cmbSolution.SelectedRecord?.TryGetAttributeValue<string>("friendlyname", out string solution) == true)
            {
                catreq.Execution.SolutionId = cmbSolution.SelectedRecord.Id;
                catreq.Execution.Solution = solution;
            }
            catreq.CustomRequestId = cmbCustomActions.SelectedRecord?.Id ?? Guid.Empty;
            var catreqshistory = GetHistoryFromFile();
            catreqshistory.Insert(0, catreq);
            SaveHistoryToFile(catreqshistory);
            ShowHistory(catreqshistory);
        }

        private void SaveHistoryToFile(List<CATRequest> history)
        {
            SettingsManager.Instance.Save(typeof(CustomActionTester), history, "History");
        }

        private void ShowHistory(List<CATRequest> history)
        {
            listHistory.Items.Clear();
            listHistory.Groups.Clear();
            listHistory.ShowGroups = !rbHistGroupNone.Checked;
            var timeformat = rbHistGroupDate.Checked ? "T" : "G";
            colTime.Text = rbHistGroupDate.Checked ? "Time" : "Date";
            if (rbHistGroupDate.Checked)
            {
                listHistory.Groups.AddRange(history
                    .OrderBy(h => h.Execution.RunTime)
                    .Reverse()
                    .Select(h => h.Execution.RunTime.Date)
                    .Distinct()
                    .Select(d => new ListViewGroup(d.ToString(), d.ToString("d"))).ToArray());
                listHistory.Groups.Cast<ListViewGroup>().ToList()
                    .ForEach(g => listHistory.Items.AddRange(history
                       .Where(h => h.Execution.RunTime.Date.Equals(DateTime.Parse(g.Name)))
                       .Select(h => h.GetListItem(g, timeformat)).ToArray()));
            }
            else if (rbHistGroupSolution.Checked)
            {
                listHistory.Groups.AddRange(history
                    .OrderBy(h => h.Execution.Solution)
                    .Select(h => new Tuple<Guid, string>(h.Execution.SolutionId, h.Execution.Solution))
                    .Distinct()
                    .Select(s => new ListViewGroup(s.Item1.ToString(), s.Item2)).ToArray());
                listHistory.Groups.Cast<ListViewGroup>().ToList()
                    .ForEach(g => listHistory.Items.AddRange(history
                       .Where(h => h.Execution.SolutionId.Equals(Guid.Parse(g.Name)))
                       .Select(h => h.GetListItem(g, timeformat)).ToArray()));
            }
            else if (rbHistGroupAPI.Checked)
            {
                listHistory.Groups.AddRange(history
                    .OrderBy(h => h.Name)
                    .Select(h => new Tuple<Guid, string>(h.CustomRequestId, h.Name))
                    .Distinct()
                    .Select(s => new ListViewGroup(s.Item1.ToString(), s.Item2)).ToArray());
                listHistory.Groups.Cast<ListViewGroup>().ToList()
                    .ForEach(g => listHistory.Items.AddRange(history
                       .Where(h => h.CustomRequestId.Equals(Guid.Parse(g.Name)))
                       .Select(h => h.GetListItem(g, timeformat)).ToArray()));
            }
            else
            {
                listHistory.Items.AddRange(history
                    .Select(h => h.GetListItem(null, timeformat)).ToArray());
            }
            listHistory.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            if (rbHistGroupSolution.Checked)
            {
                listHistory.Columns[4].Width = 0;
            }
            if (rbHistGroupAPI.Checked)
            {
                listHistory.Columns[0].Width = 0;
            }
            EnableHistButtons();
        }

        private List<CATRequest> GetHistoryFromFile()
        {
            if (!SettingsManager.Instance.TryLoad(typeof(CustomActionTester), out List<CATRequest> catreqshistory, "History"))
            {
                catreqshistory = new List<CATRequest>();
            }
            catreqshistory = catreqshistory.OrderBy(req => req.Execution?.RunTime).Reverse().ToList();

            return catreqshistory;
        }

        private List<CATRequest> GetHistoryFromList()
        {
            return listHistory.Items.Cast<ListViewItem>()
                .Where(l => l.Tag is CATRequest)
                .Select(l => l.Tag as CATRequest)
                .ToList();
        }

        private void DeleteHistories(List<CATRequest> deletehistories)
        {
            var history = GetHistoryFromList();
            deletehistories.ForEach(d => history.Remove(d));
            SaveHistoryToFile(history);
            ShowHistory(history);
        }

        private void DeleteAllHistory()
        {
            if (MessageBox.Show("Confirm full deletion of the histories.", "Delete All", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) != DialogResult.OK)
            {
                return;
            }
            var history = new List<CATRequest>();
            ShowHistory(history);
            SaveHistoryToFile(history);
        }

        private void EnableHistButtons()
        {
            btnHistReload.Enabled = listHistory.SelectedItems.Count == 1;
            btnHistDelete.Enabled = listHistory.SelectedItems.Count > 0;
            btnHistDeleteAll.Enabled = listHistory.Items.Count > 0;
        }

        private void ReloadHistoryItem(CATRequest history)
        {
            historyrequest = history;
            ReloadHistoryItem(0);
        }

        private void ReloadHistoryItem(int nextstepnum)
        {
            switch (nextstepnum)
            {
                case 0:
                    if (rbSolManaged.Checked == historyrequest.Execution.ManagedSolutions)
                    {
                        ReloadHistoryItem(++nextstepnum);
                    }
                    else
                    {
                        GetSolutions(historyrequest.Execution.ManagedSolutions, ReloadHistoryItem, ++nextstepnum);
                    }
                    break;

                case 1:
                    if (cmbSolution.SelectedRecord?.Id == historyrequest.Execution.SolutionId)
                    {
                        ReloadHistoryItem(++nextstepnum);
                    }
                    else
                    {
                        GetCustomActions(historyrequest.Execution.SolutionId, ReloadHistoryItem, ++nextstepnum);
                    }
                    break;

                case 2:
                    if (cmbCustomActions.SelectedRecord?.Id != historyrequest.CustomRequestId)
                    {
                        cmbCustomActions.SetSelected(historyrequest.CustomRequestId);
                    }
                    ReloadHistoryItem(++nextstepnum);
                    break;

                case 3:
                    GetInputParams(historyrequest.CustomRequestId, ReloadHistoryItem, ++nextstepnum);
                    break;

                case 4:
                    SetInputParametersValues(historyrequest.Parameters);
                    break;
            }
        }

        private void SetInputParametersValues(List<CATParameter> parameters)
        {
            var inputs = gridInputParams.GetDataSource<IEnumerable<Entity>>();
            if (inputs == null)
            {
                return;
            }
            foreach (var param in parameters)
            {
                var input = inputs.FirstOrDefault(i => i.TryGetAttributeValue(catTool.Columns.ParamUniqueName, out string name) && name == param.Name);
                if (input == null)
                {
                    continue;
                }
                switch (param.Type)
                {
                    case ParamType.Entity:
                        var entity = param.RawValue as Entity;
                        entity = Service.Retrieve(entity.LogicalName, entity.Id, new ColumnSet(true));
                        input["rawvalue"] = entity;
                        input["value"] = GetEntityPrimaryName(entity);
                        break;

                    case ParamType.EntityReference:
                        var entityref = param.RawValue as EntityReference;
                        entity = Service.Retrieve(entityref.LogicalName, entityref.Id, new ColumnSet(true));
                        input["value"] = GetEntityPrimaryName(entity);
                        break;

                    default:
                        input["rawvalue"] = param.RawValue;
                        input["value"] = param.Value;
                        break;
                }
                RefreshInputParameters(input);
            }
        }

        private string GetEntityPrimaryName(Entity entity)
        {
            if (entities.FirstOrDefault(e => e.Metadata.LogicalName == entity.LogicalName) is EntityMetadataProxy proxy &&
                         entity.TryGetAttributeValue(proxy.Metadata.PrimaryNameAttribute, out string entityname))
            {
                return entityname;
            }
            else
            {
                return entity.Id.ToString();
            }
        }

        #endregion Private Methods
    }
}