using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace Rappen.XTB.CAT
{
    public partial class CustomActionTester : PluginControlBase
    {
        private InputValue inputdlg;
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
            inputdlg = null;
            cmbCustomActions.OrganizationService = null;
            cmbCustomActions.SelectedIndex = -1;
            gridInputParams.DataSource = null;
            gridOutputParams.DataSource = null;
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

        #endregion Public Methods

        #region Private Methods

        private void btnExecute_Click(object sender, EventArgs e)
        {
            ExecuteCA();
        }

        private void cmbCustomActions_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUniqueName.Entity = cmbCustomActions.SelectedEntity;
            txtMessageName.Entity = cmbCustomActions.SelectedEntity;
            txtCreatedBy.Entity = cmbCustomActions.SelectedEntity;
            GetInputParams();
        }

        private void CustomActionTester_Load(object sender, EventArgs e)
        {
            LogUse("Load");
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

        private void gridInputParams_RecordDoubleClick(object sender, xrmtb.XrmToolBox.Controls.CRMRecordEventArgs e)
        {
            if (inputdlg == null)
            {
                inputdlg = new InputValue(Service);
            }
            var dlgresult = inputdlg.ShowDialog(e.Entity, this);
            if (dlgresult == DialogResult.Cancel)
            {
                return;
            }
            if (dlgresult == DialogResult.Ignore)
            {
                e.Entity.Attributes.Remove("value");
                e.Entity.Attributes.Remove("rawvalue");
            }
            gridInputParams.Refresh();
            gridInputParams.AutoResizeColumns();
        }
    }
}