using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json.Linq;
using System;
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
    public partial class CustomActionTester : PluginControlBase, IGitHubPlugin, IAboutPlugin
    {
        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";
        private const string aiKey = "eed73022-2444-45fd-928b-5eebd8fa46a6";    // jonas@rappen.net tenant, XrmToolBox
        private AppInsights ai = new AppInsights(aiEndpoint, aiKey, Assembly.GetExecutingAssembly(), "Custom Action Tester");

        public string RepositoryName => "CustomActionTester";

        public string UserName => "rappen";

        public void ShowAboutDialog()
        {
            var about = new About(this)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            about.lblVersion.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            about.ShowDialog();
        }

        public CustomActionTester()
        {
            InitializeComponent();
        }

        internal void LogUse(string action, double? count = null, double? duration = null)
        {
            ai.WriteEvent(action, count, duration, HandleAIResult);
        }

        private void HandleAIResult(string result)
        {
            if (!string.IsNullOrEmpty(result))
            {
                LogError("Failed to write to Application Insights:\n{0}", result);
            }
        }

        private void CustomActionTester_Load(object sender, EventArgs e)
        {
            LogUse("Load");
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

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            cmbCustomActions.OrganizationService = newService;
            txtUniqueName.OrganizationService = newService;
            txtUniqueName.OrganizationService = newService;
            txtCreatedBy.OrganizationService = newService;
            gridInputParams.OrganizationService = newService;
            gridOutputParams.OrganizationService = newService;

            if (newService != null)
            {
                GetCustomActions();
            }
        }

        private void cmbCustomActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUniqueName.Entity = cmbCustomActions.SelectedEntity;
            txtMessageName.Entity = cmbCustomActions.SelectedEntity;
            txtCreatedBy.Entity = cmbCustomActions.SelectedEntity;
            GetInputParams();
        }

        private void GetInputParams()
        {
            var qx = new QueryExpression("sdkmessagerequestfield");
            qx.Distinct = true;
            qx.ColumnSet.AddColumns("name", "position", "optional", "parser", "fieldmask");
            qx.AddOrder("position", OrderType.Ascending);
            var req = qx.AddLink("sdkmessagerequest", "sdkmessagerequestid", "sdkmessagerequestid");
            var pair = req.AddLink("sdkmessagepair", "sdkmessagepairid", "sdkmessagepairid");
            var msg = pair.AddLink("sdkmessage", "sdkmessageid", "sdkmessageid");
            var wf = msg.AddLink("workflow", "sdkmessageid", "sdkmessageid");
            wf.LinkCriteria.AddCondition("workflowid", ConditionOperator.Equal, cmbCustomActions.SelectedEntity.Id);
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
            var qx = new QueryExpression("sdkmessageresponsefield");
            qx.Distinct = true;
            qx.ColumnSet.AddColumns("name", "position", "parameterbindinginformation", "formatter", "publicname");
            qx.AddOrder("position", OrderType.Ascending);
            var resp = qx.AddLink("sdkmessageresponse", "sdkmessageresponseid", "sdkmessageresponseid");
            var req = resp.AddLink("sdkmessagerequest", "sdkmessagerequestid", "sdkmessagerequestid");
            var pair = req.AddLink("sdkmessagepair", "sdkmessagepairid", "sdkmessagepairid");
            var msg = pair.AddLink("sdkmessage", "sdkmessageid", "sdkmessageid");
            var wf = msg.AddLink("workflow", "sdkmessageid", "sdkmessageid");
            wf.LinkCriteria.AddCondition("workflowid", ConditionOperator.Equal, cmbCustomActions.SelectedEntity.Id);
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

        private void gridInputParams_RecordEnter(object sender, xrmtb.XrmToolBox.Controls.CRMRecordEventArgs e)
        {
            e.Entity.TryGetAttributeValue("value", out string value);
            txtInputParamValue.Text = value;
        }

        private void btnSaveInputParamValue_Click(object sender, EventArgs e)
        {
            if (gridInputParams.SelectedCellRecords.FirstOrDefault() is Entity input)
            {
                input["value"] = txtInputParamValue.Text;
                gridInputParams.Refresh();
                gridInputParams.AutoResizeColumns();
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            ExecuteCA();
        }

        private void ExecuteCA()
        {
            var request = new OrganizationRequest(txtMessageName.Text);
            foreach (var input in gridInputParams.DataSource as IEnumerable<Entity>)
            {
                if (input.TryGetAttributeValue("name", out string name) &&
                    input.TryGetAttributeValue("value", out string value))
                {
                    request[name] = value;
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

        private void gridOutputParams_RecordClick(object sender, xrmtb.XrmToolBox.Controls.CRMRecordEventArgs e)
        {
            FormatResultDetail();
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

        private string GetResultDetailValue()
        {
            var record = gridOutputParams.SelectedCellRecords.FirstOrDefault();
            if (record?.Contains("value") != true)
            {
                txtResultDetail.Text = string.Empty;
                return null;
            }
            return record["value"].ToString();
        }

        private void rbFormatResult_CheckedChanged(object sender, EventArgs e)
        {
            FormatResultDetail();
        }

        private void tslAbout_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }
    }
}