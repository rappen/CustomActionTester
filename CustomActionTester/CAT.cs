using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using System;
using System.Windows.Forms;
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
            txtUniqueName.OrganizationService = newService;
            txtUniqueName.OrganizationService = newService;
            txtCreatedBy.OrganizationService = newService;
            gridInputParams.OrganizationService = newService;
            gridOutputParams.OrganizationService = newService;

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

        private void chkSolFilter_CheckedChanged(object sender, EventArgs e)
        {
            GetSolutions(chkSolManaged.Checked, chkSolInvisible.Checked);
        }

        private void cmbCustomActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetCustomAction(cmbCustomActions.SelectedEntity);
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
            if (inputdlg == null)
            {
                inputdlg = new InputValue(Service, entities);
            }
            var dlgresult = inputdlg.ShowDialog(e.Entity, this);
            if (dlgresult == DialogResult.Cancel)
            {
                return;
            }
            if (dlgresult == DialogResult.OK && inputdlg.Result != null)
            {
                e.Entity["rawvalue"] = inputdlg.Result;
                e.Entity["value"] = inputdlg.FormattedResult;
            }
            else if (dlgresult == DialogResult.Ignore)
            {
                e.Entity.Attributes.Remove("value");
                e.Entity.Attributes.Remove("rawvalue");
            }
            gridInputParams.Refresh();
            gridInputParams.AutoResizeColumns();
        }

        private void gridOutputParams_RecordClick(object sender, xrmtb.XrmToolBox.Controls.CRMRecordEventArgs e)
        {
            FormatResultDetail();
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