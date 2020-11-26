using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using System.Windows.Forms;
using xrmtb.XrmToolBox.Controls.Controls;
using ParamType = Rappen.XTB.CAT.Customapirequestparameter.Type_OptionSet;

namespace Rappen.XTB.CAT
{
    public partial class InputValue : Form
    {
        #region Private Fields

        private Control focus;
        private IOrganizationService service;

        #endregion Private Fields

        #region Public Constructors

        public InputValue(IOrganizationService service, EntityMetadataProxy[] entities)
        {
            InitializeComponent();
            this.service = service;
            cmbEntity.Items.AddRange(entities);
        }

        #endregion Public Constructors

        #region Public Properties

        public object FormattedResult { get; private set; }

        public object Result { get; private set; }

        #endregion Public Properties

        #region Public Methods

        public DialogResult ShowDialog(Entity input, IWin32Window owner)
        {
            var type = GetType(input);
            lblName.Text = input["name"].ToString();
            lblType.Text = type.ToString();
            txtRecord.OrganizationService = service;
            if (!ParseInput(type, input))
            {
                MessageBox.Show($"Type {type} is not yet supported by CAT.", "Input Parameter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return DialogResult.Cancel;
            }
            var result = ShowDialog(owner);
            if (result == DialogResult.OK)
            {
                result = HandleInput(type);
            }
            return result;
        }

        #endregion Public Methods

        #region Private Event Handlers

        private void btnLookup_Click(object sender, EventArgs e)
        {
            if (!(cmbEntity.SelectedItem is EntityMetadataProxy entity))
            {
                return;
            }
            var lkp = new CDSLookupDialog
            {
                Service = txtRecord.OrganizationService,
                LogicalName = entity.Metadata.LogicalName
            };
            if (lkp.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            txtRecord.Entity = lkp.Entity;
        }

        private void cmbEntity_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRecord.Entity = null;
        }

        private void InputValue_Shown(object sender, EventArgs e)
        {
            focus?.Focus();
        }

        #endregion Private Event Handlers

        #region Private Methods

        private static ParamType? GetType(Entity input)
        {
            if (input.TryGetAttributeValue("type", out string typestr))
            {
                typestr = typestr
                    .Replace("OptionSetValue", "Picklist")
                    .Replace("Int32", "Integer")
                    .Replace("Int64", "Integer")
                    .Replace("Double", "Float");
                if (Enum.TryParse(typestr, out ParamType type))
                {
                    return type;
                }
            }
            if (input.TryGetAttributeValue("type", out OptionSetValue typeosv))
            {
                return (ParamType)typeosv.Value;
            }
            return null;
        }

        private DialogResult HandleInput(ParamType? type)
        {
            var invalidstr = false;
            Result = null;
            FormattedResult = null;
            switch (type)
            {
                case ParamType.String:
                    Result = txtString.Text;
                    break;
                case ParamType.Integer:
                //case ParamType.Int:
                //case ParamType.Int32:
                //case ParamType.Int64:
                    if (!int.TryParse(txtString.Text, out int intvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    Result = intvalue;
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
                        Result = new Money(decvalue);
                        FormattedResult = decvalue;
                    }
                    else
                    {
                        Result = decvalue;
                    }
                    break;
                //case ParamType.Double:
                case ParamType.Float:
                    if (!double.TryParse(txtString.Text, out double douvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    Result = douvalue;
                    break;
                case ParamType.Picklist:
                    if (!int.TryParse(txtString.Text, out int osvvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    Result = new OptionSetValue(osvvalue);
                    FormattedResult = osvvalue.ToString();
                    break;
                case ParamType.Entity:
                case ParamType.EntityReference:
                    if (txtRecord.Entity == null)
                    {
                        MessageBox.Show($"No record selected", "Input Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return DialogResult.Cancel;
                    }
                    if (type == ParamType.Entity)
                    {
                        Result = txtRecord.Entity;
                    }
                    else if (type == ParamType.EntityReference)
                    {
                        Result = txtRecord.Entity.ToEntityReference();
                    }
                    FormattedResult = txtRecord.Text;
                    break;
                case ParamType.Boolean:
                    Result = chkBoolean.Checked;
                    break;
                case ParamType.DateTime:
                    Result = dtDateTime.Value;
                    break;
            }
            if (invalidstr)
            {
                MessageBox.Show($"Not a valid {type}: {txtString.Text}", "Input Parameter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.Cancel;
            }
            FormattedResult = FormattedResult ?? Result;
            return DialogResult.OK;
        }

        private bool ParseInput(ParamType? type, Entity input)
        {
            panString.Visible = false;
            panEntity.Visible = false;
            panBoolean.Visible = false;
            panDateTime.Visible = false;
            focus = null;
            lblHint.Text = "Enter value";
            var currentvalue = input.Contains("rawvalue") ? input["rawvalue"] : null;
            switch (type)
            {
                case ParamType.String:
                case ParamType.Integer:
                //case ParamType.Int:
                //case ParamType.Int32:
                //case ParamType.Int64:
                case ParamType.Decimal:
                //case ParamType.Double:
                case ParamType.Float:
                case ParamType.Money:
                    panString.Visible = true;
                    txtString.Text = currentvalue?.ToString();
                    focus = txtString;
                    break;
                case ParamType.Picklist:
                    lblHint.Text = "Enter numeric value of the OptionSetValue";
                    panString.Visible = true;
                    if (currentvalue is OptionSetValue osv)
                    {
                        txtString.Text = osv.Value.ToString();
                    }
                    focus = txtString;
                    break;
                case ParamType.Entity:
                case ParamType.EntityReference:
                    panEntity.Visible = true;
                    if (input.TryGetAttributeValue("entity", out EntityMetadataProxy entitytype) &&
                        cmbEntity.Items.Cast<EntityMetadataProxy>().FirstOrDefault(e => e.Metadata.LogicalName == entitytype.Metadata.LogicalName) is EntityMetadataProxy selentity)
                    {
                        cmbEntity.SelectedItem = selentity;
                        cmbEntity.Enabled = false;
                        lblHint.Text = $"Select {selentity.DisplayName}";
                        focus = btnLookup;
                    }
                    else
                    {
                        cmbEntity.Enabled = true;
                        lblHint.Text = "Select Entity and Record";
                        focus = cmbEntity;
                    }
                    if (currentvalue is Entity entity)
                    {
                        cmbEntity.SelectedItem = cmbEntity.Items.Cast<EntityMetadataProxy>().FirstOrDefault(e => e.Metadata.LogicalName == entity.LogicalName);
                        txtRecord.Entity = entity;
                    }
                    else if (currentvalue is EntityReference entref)
                    {
                        cmbEntity.SelectedItem = cmbEntity.Items.Cast<EntityMetadataProxy>().FirstOrDefault(e => e.Metadata.LogicalName == entref.LogicalName);
                        txtRecord.EntityReference = entref;
                    }
                    break;
                case ParamType.Boolean:
                    lblHint.Text = "Check or Uncheck to define true/false";
                    panBoolean.Visible = true;
                    chkBoolean.Checked = currentvalue is bool boolval && boolval;
                    focus = chkBoolean;
                    break;
                case ParamType.DateTime:
                    lblHint.Text = "Select Date and Time";
                    panDateTime.Visible = true;
                    if (currentvalue is DateTime dtvalue)
                    {
                        dtDateTime.Value = dtvalue;
                    }
                    focus = dtDateTime;
                    break;
                default:
                    return false;
            }
            return true;
        }

        #endregion Private Methods
    }
}
