using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
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
            lblSelect.Text = $"{catTool.Target} Select";
            lblCustomWhat.Text = catTool.Target;
            rbHistGroupAPI.Text = catTool.Target;
            colAPI.Text = catTool.Target;
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
                GetSolutions(GetSolutionType());
            }
        }

        #endregion Public Methods

        #region Private Methods

        private SolutionType GetSolutionType()
        {
            if (rbSolUnmanaged.Checked)
            {
                return SolutionType.Unmanaged;
            }
            if (rbSolManaged.Checked)
            {
                return SolutionType.Managed;
            }
            return SolutionType.All;
        }

        private void SetSolutionType(SolutionType type)
        {
            switch (type)
            {
                case SolutionType.Unmanaged:
                    rbSolUnmanaged.Checked = true;
                    break;

                case SolutionType.Managed:
                    rbSolManaged.Checked = true;
                    break;

                default:
                    rbSolAll.Checked = true;
                    break;
            }
        }

        #endregion Private Methods

        #region Private Methods Events

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
            GetSolutions(GetSolutionType());
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
            GetHistoryFromFile();
            LoadAndShowHistoryIfNeeded();
        }

        private void gridOutputParams_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            FormatResultDetail();
        }

        private void picHistory_Click(object sender, EventArgs e)
        {
            splitToolHistory.Panel2Collapsed = !splitToolHistory.Panel2Collapsed;
            LoadAndShowHistoryIfNeeded();
        }

        private void rbFormatResult_CheckedChanged(object sender, EventArgs e)
        {
            FormatResultDetail();
        }

        private void tslAbout_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

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

        private void listHistory_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableHistButtons();
        }

        private void btnHistReload_Click(object sender, EventArgs e)
        {
            if (listHistory.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag is CATRequest request)
            {
                ReloadHistoryItem(request);
            }
        }

        private void rbHistGroupX_CheckedChanged(object sender, EventArgs e)
        {
            ShowHistory();
        }

        private void btnHistDelete_Click(object sender, EventArgs e)
        {
            DeleteHistories(listHistory.SelectedItems.Cast<ListViewItem>()
                .Where(i => i.Tag is CATRequest)
                .Select(i => i.Tag as CATRequest).ToList());
        }

        private void btnHistDeleteAll_Click(object sender, EventArgs e)
        {
            DeleteHistories(listHistory.Items.Cast<ListViewItem>()
                .Where(i => i.Tag is CATRequest)
                .Select(i => i.Tag as CATRequest).ToList());
        }

        #endregion Private Methods Events

        private void txtString_TextChanged(object sender, EventArgs e)
        {
            SetInputParamValue(string.IsNullOrEmpty(txtString.Text));
        }

        private void dtDateTime_ValueChanged(object sender, EventArgs e)
        {
            SetInputParamValue(false);
        }

        private void chkBoolean_CheckedChanged(object sender, EventArgs e)
        {
            SetInputParamValue(false);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            SetInputParamValue(true);
        }

        private void txtString_Enter(object sender, EventArgs e)
        {
            _editinginputvalue = true;
        }

        private void txtString_Leave(object sender, EventArgs e)
        {
            _editinginputvalue = false;
        }

        private void btnMultilines_Click(object sender, EventArgs e)
        {
            EditMultiline();
        }
    }
}