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

        public static string ValueToString(object value, bool attributetypes, bool convertqueries, bool expandcollections, IOrganizationService service, int indent = 1)
        {
            var indentstring = new string(' ', indent * 2);
            if (value == null)
            {
                return $"{indentstring}<null>";
            }
            else if (value is EntityCollection collection)
            {
                var result = $"{collection.EntityName} collection\n  Records: {collection.Entities.Count}\n  TotalRecordCount: {collection.TotalRecordCount}\n  MoreRecords: {collection.MoreRecords}\n  PagingCookie: {collection.PagingCookie}";
                if (expandcollections)
                {
                    result += $"\n{indentstring}  {string.Join($"\n{indentstring}", collection.Entities.Select(e => ValueToString(e, attributetypes, convertqueries, expandcollections, service, indent + 1)))}";
                }
                return result;
            }
            else if (value is Entity entity)
            {
                var keylen = entity.Attributes.Count > 0 ? entity.Attributes.Max(p => p.Key.Length) : 50;
                return $"{entity.LogicalName} {entity.Id}\n{indentstring}" + string.Join($"\n{indentstring}", entity.Attributes.OrderBy(a => a.Key).Select(a => $"{a.Key}{new string(' ', keylen - a.Key.Length)} = {ValueToString(a.Value, attributetypes, convertqueries, expandcollections, service, indent + 1)}"));
            }
            else if (value is ColumnSet columnset)
            {
                var columnlist = new List<string>(columnset.Columns);
                columnlist.Sort();
                return $"\n{indentstring}" + string.Join($"\n{indentstring}", columnlist);
            }
            else if (value is FetchExpression fetchexpression)
            {
                return $"{value}\n{indentstring}{fetchexpression.Query}";
            }
            else
            {
                var result = string.Empty;
                if (value is EntityReference entityreference)
                {
                    result = $"{entityreference.LogicalName} {entityreference.Id} {entityreference.Name}";
                }
                else if (value is OptionSetValue optionsetvalue)
                {
                    result = optionsetvalue.Value.ToString();
                }
                else if (value is Money money)
                {
                    result = money.Value.ToString();
                }
                else
                {
                    result = value.ToString().Replace("\n", $"\n  {indentstring}");
                }
                return result + (attributetypes ? $" \t({value.GetType()})" : "");
            }
        }

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
                        var outputparams = response.Item1.Results;
                        PopulateOutputParamValues(outputparams);
                    }
                    SaveHistory(catreq);
                }
            });
        }

        private void SaveHistory(CATRequest catreq)
        {
            if (!SettingsManager.Instance.TryLoad(typeof(CustomActionTester), out List<CATRequest> catreqshistory, catTool.Target + " History"))
            {
                catreqshistory = new List<CATRequest>();
            }
            catreqshistory.Add(catreq);
            catreqshistory = catreqshistory.OrderBy(req => req.Execution?.RunTime).Reverse().ToList();
            SettingsManager.Instance.Save(typeof(CustomActionTester), catreqshistory, catTool.Target + " History");
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

        private void GetCustomActions(Entity solution)
        {
            cmbCustomActions.DataSource = null;
            var qx = catTool.GetActionQuery(solution?.Id ?? Guid.Empty);
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

        private void GetInputParams(Entity ca)
        {
            if (ca == null)
            {
                gridInputParams.DataSource = null;
                gridOutputParams.DataSource = null;
                return;
            }
            var qx = catTool.GetInputQuery(ca.Id);

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
                        GetOutputParams(ca);
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

        private void GetOutputParams(Entity ca)
        {
            if (ca == null)
            {
                gridOutputParams.DataSource = null;
                return;
            }
            var qx = catTool.GetOutputQuery(ca.Id);

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
            lblResultDetailType.Text = result.GetType().ToString();
            if (result is string)
            {
                panTextFormat.Visible = true;
                return result;
            }
            return ValueToString(result, false, false, true, Service);
        }

        private void GetSolutions(bool managed)
        {
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
                if (rawvalue is Money money)
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
            GetInputParams(ca);
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

        #endregion Private Methods
    }
}