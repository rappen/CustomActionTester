using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Windows.Forms;
using xrmtb.XrmToolBox.Controls.Controls;

namespace Rappen.XTB.CAT
{
    public partial class InputValue : Form
    {
        IOrganizationService service;

        public InputValue(IOrganizationService service)
        {
            InitializeComponent();
            this.service = service;
        }

        public DialogResult ShowDialog(Entity input, IWin32Window owner)
        {
            var type = GetType(input);
            lblName.Text = input["name"].ToString();
            lblType.Text = type.ToString();
            var currentvalue = input.Contains("rawvalue") ? input["rawvalue"] : null;
            txtRecord.OrganizationService = service;
            if (!ParseInput(type, currentvalue))
            {
                MessageBox.Show($"Type {type} is not yet supported by CAT.", "Input Parameter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return DialogResult.Cancel;
            }
            var result = ShowDialog(owner);
            if (result == DialogResult.OK)
            {
                result = HandleInput(input, owner, type);
            }
            return result;
        }

        private bool ParseInput(ParamType type, object currentvalue)
        {
            panString.Visible = false;
            panEntity.Visible = false;
            panBoolean.Visible = false;
            panDateTime.Visible = false;
            switch (type)
            {
                case ParamType.String:
                case ParamType.Integer:
                case ParamType.Int:
                case ParamType.Int32:
                case ParamType.Int64:
                case ParamType.Decimal:
                case ParamType.Double:
                case ParamType.Float:
                case ParamType.Money:
                    panString.Visible = true;
                    txtString.Text = currentvalue?.ToString();
                    break;
                case ParamType.Entity:
                case ParamType.EntityReference:
                    panEntity.Visible = true;
                    if (currentvalue is Entity entity)
                    {
                        txtRecord.Entity = entity;
                    }
                    else if (currentvalue is EntityReference entref)
                    {
                        txtRecord.EntityReference = entref;
                    }
                    //PopulateEntities();
                    break;
                case ParamType.Boolean:
                    panBoolean.Visible = true;
                    chkBoolean.Checked = currentvalue is bool boolval && boolval;
                    break;
                case ParamType.DateTime:
                    panDateTime.Visible = true;
                    if (currentvalue is DateTime dtvalue)
                    {
                        dtDateTime.Value = dtvalue;
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        private DialogResult HandleInput(Entity input, IWin32Window owner, ParamType type)
        {
            var invalidstr = false;
            input["value"] = null;
            switch (type)
            {
                case ParamType.String:
                    var strvalue = txtString.Text;
                    input["rawvalue"] = strvalue;
                    break;
                case ParamType.Integer:
                case ParamType.Int:
                case ParamType.Int32:
                case ParamType.Int64:
                    if (!int.TryParse(txtString.Text, out int intvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    input["rawvalue"] = intvalue;
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
                        input["rawvalue"] = new Money(decvalue);
                    }
                    else
                    {
                        input["rawvalue"] = decvalue;
                    }
                    break;
                case ParamType.Double:
                case ParamType.Float:
                    if (!double.TryParse(txtString.Text, out double douvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    input["rawvalue"] = douvalue;
                    break;
                case ParamType.Entity:
                case ParamType.EntityReference:
                    if (txtRecord.Entity == null)
                    {
                        if (MessageBox.Show($"Please select a record", "Input Parameter", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                        {
                            return ShowDialog(input, owner);
                        }
                        else
                        {
                            return DialogResult.Cancel;
                        }
                    }
                    if (type == ParamType.Entity)
                    {
                        input["rawvalue"] = txtRecord.Entity;
                    }
                    else
                    {
                        input["rawvalue"] = txtRecord.Entity.ToEntityReference();
                    }
                    input["value"] = txtRecord.Text;
                    break;
                case ParamType.Boolean:
                    input["rawvalue"] = chkBoolean.Checked;
                    break;
                case ParamType.DateTime:
                    input["rawvalue"] = dtDateTime.Value;
                    break;
            }
            if (invalidstr)
            {
                if (MessageBox.Show($"Not a valid {type}: {txtString.Text}", "Input Parameter", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    return ShowDialog(input, owner);
                }
                else
                {
                    return DialogResult.Cancel;
                }
            }
            if (input["value"] == null)
            {
                input["value"] = input["rawvalue"];
            }
            return DialogResult.OK;
        }

        private static ParamType GetType(Entity input)
        {
            if (input.TryGetAttributeValue("type", out string typestr) && Enum.TryParse(typestr, out ParamType type))
            {
                return type;
            }
            return ParamType.Undefined;
        }

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

        private void PopulateEntities(EntityMetadataProxy[] entities)
        {
            cmbEntity.Items.Clear();
            cmbEntity.Items.AddRange(entities);
        }

    }

    public enum ParamType
    {
        String,
        Integer,
        Int,
        Int32,
        Int64,
        Decimal,
        Double,
        Float,
        Money,
        Entity,
        EntityReference,
        Boolean,
        DateTime,
        OptionSetValue,
        Undefined
    }
}
