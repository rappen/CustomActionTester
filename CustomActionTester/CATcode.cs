using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Rappen.XTB.CAT
{
    public partial class CustomActionTester : IGitHubPlugin, IAboutPlugin, IShortcutReceiver
    {
        #region Private Fields

        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";
        private const string aiKey = "eed73022-2444-45fd-928b-5eebd8fa46a6";    // jonas@rappen.net tenant, XrmToolBox
        private AppInsights ai = new AppInsights(aiEndpoint, aiKey, Assembly.GetExecutingAssembly(), "Custom Action Tester");
        public EntityMetadataProxy[] entities;

        #endregion Private Fields

        #region Public Properties

        public string RepositoryName => "CustomActionTester";

        public string UserName => "rappen";

        #endregion Public Properties

        #region Public Methods

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

        private void ExecuteCA()
        {
            var request = new OrganizationRequest(txtMessageName.Text);
            foreach (var input in gridInputParams.DataSource as IEnumerable<Entity>)
            {
                if (input.TryGetAttributeValue("name", out string name))
                {
                    if (input.TryGetAttributeValue("rawvalue", out object value))
                    {
                        request[name] = value;
                    }
                    else if (input.TryGetAttributeValue("optional", out bool optional) && !optional)
                    {
                        MessageBox.Show($"Missing value for required parameter: {name}", "Execute Custom Action", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Executing Custom Action",
                Work = (worker, args) =>
                {
                    args.Result = Service.Execute(request);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (args.Result is OrganizationResponse response)
                    {
                        foreach (var result in response.Results)
                        {
                            var outputs = gridOutputParams.DataSource as IEnumerable<Entity>;
                            var output = outputs.FirstOrDefault(o => o["name"].ToString().Equals(result.Key));
                            if (output != null)
                            {
                                output["value"] = result.Value;
                            }
                        }
                        gridOutputParams.Refresh();
                        gridOutputParams.AutoResizeColumns();
                        FormatResultDetailDefault();
                    }
                }
            });
        }

        private void ExtractTypeInfo(EntityCollection records)
        {
            foreach (var record in records.Entities)
            {
                var attribute = record.Contains("parser") ? "parser" : "formatter";
                if (record.TryGetAttributeValue(attribute, out string parser))
                {
                    parser = parser.Split(',')[0];
                    while (parser.Contains("."))
                    {
                        parser = parser.Substring(parser.IndexOf('.') + 1);
                    }
                    record["type"] = parser;
                }
            }
            var otcrecords = records.Entities.Where(r => r.Contains("parameterbindinginformation"));
            var siblingrecords = new List<Entity>();
            foreach (var otcrecord in otcrecords)
            {
                var siblingrecord = records.Entities.FirstOrDefault(r => r["name"].ToString() == otcrecord["name"].ToString() && !r.Contains("parameterbindinginformation"));
                if (siblingrecord == null)
                {
                    continue;
                }
                var binding = otcrecord["parameterbindinginformation"].ToString();
                var otcstr = binding.Replace("OTC:", "").Trim();
                if (int.TryParse(otcstr, out int otc))
                {
                    if (entities.FirstOrDefault(e => e.Metadata.ObjectTypeCode == otc) is EntityMetadataProxy entity)
                    {
                        otcrecord["type"] = otcrecord["type"].ToString() + " " +
                            (entity.Metadata?.DisplayName?.UserLocalizedLabel?.Label ?? entity.Metadata.LogicalName);
                        otcrecord["entity"] = entity;
                    }
                }
                siblingrecords.Add(siblingrecord);
            }
            siblingrecords.ForEach(s => records.Entities.Remove(s));
        }

        private void FormatResultDetail()
        {
            var value = GetResultDetailValue();
            if (!string.IsNullOrEmpty(value))
            {
                if (rbFormatJSON.Checked)
                {
                    var parsedJson = JToken.Parse(value);
                    value = parsedJson.ToString(Newtonsoft.Json.Formatting.Indented);
                }
                else if (rbFormatXML.Checked)
                {
                    var string_writer = new StringWriter();
                    var xml_text_writer = new XmlTextWriter(string_writer);
                    xml_text_writer.Formatting = System.Xml.Formatting.Indented;
                    value = string_writer.ToString();
                }
            }
            txtResultDetail.Text = value;
        }

        private void FormatResultDetailDefault()
        {
            var value = GetResultDetailValue();
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (value.StartsWith("{"))
            {
                rbFormatJSON.Checked = true;
            }
            else if (value.StartsWith("<"))
            {
                rbFormatXML.Checked = true;
            }
        }

        private void GetCustomActions(Entity solution)
        {
            var qx = new QueryExpression("workflow");
            qx.ColumnSet.AddColumns("name", "uniquename", "createdby", "primaryentity", "scope", "mode", "ismanaged", "iscustomizable", "istransacted", "iscustomprocessingstepallowedforotherpublishers", "inputparameters", "description");
            qx.AddOrder("ismanaged", OrderType.Descending);
            qx.AddOrder("name", OrderType.Ascending);
            qx.Criteria.AddCondition("category", ConditionOperator.Equal, 3);
            qx.Criteria.AddCondition("type", ConditionOperator.Equal, 1);
            qx.Criteria.AddCondition("componentstate", ConditionOperator.Equal, 0);
            qx.Criteria.AddCondition("statuscode", ConditionOperator.Equal, 2);
            var qxsdk = qx.AddLink("sdkmessage", "sdkmessageid", "sdkmessageid", JoinOperator.LeftOuter);
            qxsdk.EntityAlias = "M";
            qxsdk.Columns.AddColumns("name", "workflowsdkstepenabled");
            if (solution != null)
            {
                var solcomp = qx.AddLink("solutioncomponent", "workflowid", "objectid");
                solcomp.LinkCriteria.AddCondition("solutionid", ConditionOperator.Equal, solution.Id);
            }

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting Custom Actions",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(qx);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (args.Result is EntityCollection actions)
                    {
                        cmbCustomActions.DataSource = actions;
                    }
                }
            });
        }

        private void GetInputParams(Entity ca)
        {
            if (ca == null)
            {
                gridInputParams.DataSource = null;
                gridOutputParams.DataSource = null;
                return;
            }
            var qx = new QueryExpression("sdkmessagerequestfield");
            qx.Distinct = true;
            qx.ColumnSet.AddColumns("name", "position", "parameterbindinginformation", "optional", "parser", "fieldmask");
            qx.AddOrder("position", OrderType.Ascending);
            var req = qx.AddLink("sdkmessagerequest", "sdkmessagerequestid", "sdkmessagerequestid");
            var pair = req.AddLink("sdkmessagepair", "sdkmessagepairid", "sdkmessagepairid");
            var msg = pair.AddLink("sdkmessage", "sdkmessageid", "sdkmessageid");
            var wf = msg.AddLink("workflow", "sdkmessageid", "sdkmessageid");
            wf.LinkCriteria.AddCondition("workflowid", ConditionOperator.Equal, ca.Id);
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Input Parameters",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(qx);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (args.Result is EntityCollection inputs)
                    {
                        ExtractTypeInfo(inputs);
                        gridInputParams.DataSource = inputs;
                        gridInputParams.AutoResizeColumns();
                        GetOutputParams(ca);
                    }
                }
            });
        }

        private void GetOutputParams(Entity ca)
        {
            if (ca == null)
            {
                gridOutputParams.DataSource = null;
                return;
            }
            var qx = new QueryExpression("sdkmessageresponsefield");
            qx.Distinct = true;
            qx.ColumnSet.AddColumns("name", "position", "parameterbindinginformation", "formatter", "publicname");
            qx.AddOrder("position", OrderType.Ascending);
            var resp = qx.AddLink("sdkmessageresponse", "sdkmessageresponseid", "sdkmessageresponseid");
            var req = resp.AddLink("sdkmessagerequest", "sdkmessagerequestid", "sdkmessagerequestid");
            var pair = req.AddLink("sdkmessagepair", "sdkmessagepairid", "sdkmessagepairid");
            var msg = pair.AddLink("sdkmessage", "sdkmessageid", "sdkmessageid");
            var wf = msg.AddLink("workflow", "sdkmessageid", "sdkmessageid");
            wf.LinkCriteria.AddCondition("workflowid", ConditionOperator.Equal, ca.Id);
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading Output Parameters",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(qx);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (args.Result is EntityCollection outputs)
                    {
                        ExtractTypeInfo(outputs);
                        gridOutputParams.DataSource = outputs;
                        gridOutputParams.AutoResizeColumns();
                    }
                }
            });
        }

        private string GetResultDetailValue()
        {
            var record = gridOutputParams.SelectedCellRecords.FirstOrDefault();
            if (record?.Contains("value") != true || record["value"] == null)
            {
                txtResultDetail.Text = string.Empty;
                return null;
            }
            return record["value"].ToString();
        }

        private void GetSolutions(bool managed, bool invisible)
        {
            var qx = new QueryExpression("solution");
            qx.Distinct = true;
            qx.ColumnSet.AddColumns("uniquename", "friendlyname", "version", "solutionid");
            qx.AddOrder("friendlyname", OrderType.Ascending);
            qx.Criteria.AddCondition("ismanaged", ConditionOperator.Equal, managed);
            qx.Criteria.AddCondition("isvisible", ConditionOperator.Equal, !invisible);
            var solcomp = qx.AddLink("solutioncomponent", "solutionid", "solutionid");
            solcomp.Columns.AddColumns("componenttype");
            var wf = solcomp.AddLink("workflow", "objectid", "workflowid");
            wf.LinkCriteria.AddCondition("category", ConditionOperator.Equal, 3);
            wf.LinkCriteria.AddCondition("type", ConditionOperator.Equal, 1);
            wf.LinkCriteria.AddCondition("componentstate", ConditionOperator.Equal, 0);
            wf.LinkCriteria.AddCondition("statuscode", ConditionOperator.Equal, 2);
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Getting Solutions",
                Work = (worker, args) =>
                {
                    args.Result = Service.RetrieveMultiple(qx);
                },
                PostWorkCallBack = (args) =>
                {
                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (args.Result is EntityCollection solutions)
                    {
                        cmbSolution.DataSource = solutions;
                    }
                }
            });
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
                PostWorkCallBack = (completedargs) =>
                {
                    if (completedargs.Error != null)
                    {
                        MessageBox.Show(completedargs.Error.Message);
                        return;
                    }
                    if (completedargs.Result is RetrieveMetadataChangesResponse resp)
                    {
                        entities = resp.EntityMetadata
                            .Where(e => e.IsCustomizable.Value == true && e.IsIntersect.Value != true)
                            .Select(m => new EntityMetadataProxy(m))
                            .OrderBy(e => e.ToString()).ToArray();
                    }
                }
            });
        }

        private void SetCustomAction(Entity ca)
        {
            txtUniqueName.Entity = ca;
            txtMessageName.Entity = ca;
            txtCreatedBy.Entity = ca;
            GetInputParams(ca);
        }

        #endregion Private Methods
    }
}
