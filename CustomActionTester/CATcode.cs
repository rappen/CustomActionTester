using Microsoft.Xrm.Sdk;
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
    public partial class CustomActionTester : IGitHubPlugin, IAboutPlugin
    {
        #region Private Fields

        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";
        private const string aiKey = "eed73022-2444-45fd-928b-5eebd8fa46a6";    // jonas@rappen.net tenant, XrmToolBox
        private AppInsights ai = new AppInsights(aiEndpoint, aiKey, Assembly.GetExecutingAssembly(), "Custom Action Tester");

        #endregion Private Fields

        #region Public Properties

        public string RepositoryName => "CustomActionTester";

        public string UserName => "rappen";

        #endregion Public Properties

        #region Public Methods

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

        private void ExtractTypeInfo(EntityCollection records, string attribute)
        {
            foreach (var record in records.Entities)
            {
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

        private void GetCustomActions()
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

        private void GetInputParams()
        {
            if (!(cmbCustomActions.SelectedEntity is Entity ca))
            {
                gridInputParams.DataSource = null;
                return;
            }
            var qx = new QueryExpression("sdkmessagerequestfield");
            qx.Distinct = true;
            qx.ColumnSet.AddColumns("name", "position", "optional", "parser", "fieldmask");
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
                    }
                    else if (args.Result is EntityCollection inputs)
                    {
                        ExtractTypeInfo(inputs, "parser");
                        gridInputParams.DataSource = inputs;
                        gridInputParams.AutoResizeColumns();
                        GetOutputParams();
                    }
                }
            });
        }

        private void GetOutputParams()
        {
            if (!(cmbCustomActions.SelectedEntity is Entity ca))
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
                        ExtractTypeInfo(outputs, "formatter");
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

        private void HandleAIResult(string result)
        {
            if (!string.IsNullOrEmpty(result))
            {
                LogError("Failed to write to Application Insights:\n{0}", result);
            }
        }

        #endregion Private Methods
    }
}
