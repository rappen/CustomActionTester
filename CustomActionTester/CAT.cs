using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using System;
using System.Diagnostics;
using System.Windows.Forms;
using xrmtb.XrmToolBox.Controls.Controls;
using XrmToolBox.Extensibility;

namespace Rappen.XTB.CAT
{
    public partial class CustomActionTester : PluginControlBase
    {
        #region Private Fields

        private InputValue inputdlg;

        #endregion Private Fields

        #region Public Constructors

        public CustomActionTester()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
            LoadEntities();
            inputdlg = null;
            cmbSolution.OrganizationService = newService;
            cmbCustomActions.OrganizationService = newService;
            cmbCustomAPIs.OrganizationService = newService;
            txtUniqueName.OrganizationService = newService;
            txtUniqueName.OrganizationService = newService;
            txtCreatedBy.OrganizationService = newService;
            gridInputParams.OrganizationService = newService;
            gridOutputParams.OrganizationService = newService;
            txtCDSDataHelper.OrganizationService = newService;
            if (newService != null)
            {
                GetSolutions(chkSolManaged.Checked, chkSolInvisible.Checked);
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

        private void chkSolFilter_CheckedChanged(object sender, EventArgs e)
        {
            GetSolutions(chkSolManaged.Checked, chkSolInvisible.Checked);
        }

        private void cmbCustomActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is CDSDataComboBox cmb)
            {
                SetCustomAction(cmb.SelectedEntity);
            }
        }

        private void cmbSolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCustomActions(cmbSolution.SelectedEntity);
        }

        private void CustomActionTester_Load(object sender, EventArgs e)
        {
            LogUse("Load");
        }

        private void gridInputParams_RecordDoubleClick(object sender, xrmtb.XrmToolBox.Controls.CRMRecordEventArgs e)
        {
            GetInputParamValue(e);
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
            splitRight.Panel2Collapsed = sender == picHistoryClose;
            picHistoryOpen.Visible = splitRight.Panel2Collapsed;
            picHistoryClose.Left = picHistoryClose.Parent.Width - 19;
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
    }
}