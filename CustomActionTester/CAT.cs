using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace Rappen.XTB.CAT
{
    public partial class CustomActionTester : PluginControlBase
    {
        #region Private Fields

        private ICATTool catTool;
        private const string aiEndpoint = "https://dc.services.visualstudio.com/v2/track";
        private const string aiKey = "eed73022-2444-45fd-928b-5eebd8fa46a6";    // jonas@rappen.net tenant, XrmToolBox
        private AppInsights ai;

        #endregion Private Fields

        #region Public Constructors

        public CustomActionTester(ICATTool catinstance)
        {
            catTool = catinstance;
            ai = new AppInsights(aiEndpoint, aiKey, Assembly.GetExecutingAssembly(), catTool.Name);
            InitializeComponent();
            gridInputParams.AutoGenerateColumns = false;
            gridOutputParams.AutoGenerateColumns = false;
            FixFormForTool();
        }

        private void FixFormForTool()
        {
            TabIcon = catTool.Logo16;
            PluginIcon = catTool.Icon16;
            tslAbout.Image = catTool.Logo24;
            gbCustomWhat.Text = catTool.Target;
            lblCustomWhat.Text = catTool.Target;
            btnManage.Visible = !string.IsNullOrWhiteSpace(catTool.ManagerTool)/* && PluginManagerExtended.Instance.Plugins.Any(p => p.Metadata.Name == catTool.ManagerTool)*/;
            RefreshLayout();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            btnOpenAction.Text = $"Open in {(detail.WebApplicationUrl.ToLower().Contains("dynamics.com") ? "Dataverse" : "CRM")}";
            LoadEntities();
            cmbSolution.Service = newService;
            cmbCustomActions.Service = newService;
            rhCustomAction.Service = newService;
            rhScopeRecord.Service = newService;
            rhRecord.Service = newService;
            rhResult.Service = newService;
            gridInputParams.Service = newService;
            gridOutputParams.Service = newService;
            if (newService != null)
            {
                GetSolutions(rbSolManaged.Checked);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void btnExecute_Click(object sender, EventArgs e)
        {
            ExecuteCA();
        }

        private void btnPTV_Click(object sender, EventArgs e)
        {
            TraceLastExecution();
        }

        private void rbSolFilter_CheckedChanged(object sender, EventArgs e)
        {
            GetSolutions(rbSolManaged.Checked);
        }

        private void cmbCustomActions_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SetCustomAction(cmbCustomActions.SelectedRecord);
        }

        private void cmbSolution_SelectionChangeCommitted(object sender, EventArgs e)
        {
            GetCustomActions(cmbSolution.SelectedRecord);
        }

        private void CustomActionTester_Load(object sender, EventArgs e)
        {
            LogUse("Load");
        }

        private void gridOutputParams_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            FormatResultDetail();
        }

        private void llCallHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/rappen/CustomActionTester/issues/3");
        }

        private void picHistory_Click(object sender, EventArgs e)
        {
            splitRight.Panel2Collapsed = !splitRight.Panel2Collapsed;
            picHistoryOpen.Visible = splitRight.Panel2Collapsed;
            picHistoryClose.Visible = !splitRight.Panel2Collapsed;
            if (!splitRight.Panel2Collapsed)
            {
                LoadHistory();
            }
        }

        private void rbFormatResult_CheckedChanged(object sender, EventArgs e)
        {
            FormatResultDetail();
        }

        private void tslAbout_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

        #endregion Private Methods

        private void btnScopeRecord_Click(object sender, EventArgs e)
        {
            LookupBoundRecord();
        }

        private void mnuShowDisplay_Click(object sender, EventArgs e)
        {
            mnuShowDisplay.Checked = true;
            mnuShowUnique.Checked = false;
            RefreshLayout();
        }

        private void mnuShowUnique_Click(object sender, EventArgs e)
        {
            mnuShowUnique.Checked = true;
            mnuShowDisplay.Checked = false;
            RefreshLayout();
        }

        private void btnOpenAction_Click(object sender, EventArgs e)
        {
            OpenAction(cmbCustomActions.SelectedRecord);
        }

        private void gridInputParams_RecordEnter(object sender, Rappen.XTB.Helpers.Controls.XRMRecordEventArgs e)
        {
            PopulateInputParamValue(e.Entity);
        }

        private void btnCAValueSet_Click(object sender, EventArgs e)
        {
            SetInputParamValue(sender == btnCAValueClear);
        }

        private void btnLookup_Click(object sender, EventArgs e)
        {
            LookupInputParamRecord();
        }

        private void cmbEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            rhScopeRecord.Record = null;
        }

        private void btnManage_Click(object sender, EventArgs e)
        {
            ManageAction(cmbCustomActions.SelectedRecord);
        }
    }
}