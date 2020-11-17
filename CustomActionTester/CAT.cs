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
            txtResultDetail.Text = FormatResultDetail();
        }

        private void rbFormatResult_CheckedChanged(object sender, EventArgs e)
        {
            txtResultDetail.Text = FormatResultDetail();
        }

        private void tslAbout_Click(object sender, EventArgs e)
        {
            ShowAboutDialog();
        }

        #endregion Private Methods

        private void gridInputParams_RecordDoubleClick(object sender, xrmtb.XrmToolBox.Controls.CRMRecordEventArgs e)
        {
            if (InputValue.ShowDialog(e.Entity, Service, this) == DialogResult.OK)
            {
                gridInputParams.Refresh();
                gridInputParams.AutoResizeColumns();
            }
        }
    }
}