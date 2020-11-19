using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;
using System.Windows.Forms;
using xrmtb.XrmToolBox.Controls.Controls;

namespace Rappen.XTB.CAT
{
    public partial class InputValue : Form
    {
        public InputValue()
        {
            InitializeComponent();
        }

        public static DialogResult ShowDialog(Entity input, IOrganizationService service, IWin32Window owner)
        {
            var form = new InputValue();
            var type = GetType(input);
            form.lblName.Text = input["name"].ToString();
            form.lblType.Text = type.ToString();
            var currentvalue = input.Contains("rawvalue") ? input["rawvalue"] : null;
            form.cmbEntities.Service = service;
            form.txtRecord.OrganizationService = service;
            if (!ParseInput(form, type, currentvalue))
            {
                MessageBox.Show($"Type {type} is not yet supported by CAT.", "Input Parameter", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return DialogResult.Cancel;
            }
            var result = form.ShowDialog(owner);
            if (result == DialogResult.OK)
            {
                result = HandleInput(input, service, owner, form, type);
            }
            return result;
        }

        private static bool ParseInput(InputValue form, ParamType type, object currentvalue)
        {
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
                    form.panString.Visible = true;
                    form.txtString.Text = currentvalue?.ToString();
                    break;
                case ParamType.Entity:
                case ParamType.EntityReference:
                    form.panEntityReference.Visible = true;
                    if (currentvalue is EntityReference entref)
                    {
                        form.txtRecord.EntityReference = entref;
                    }
                    break;
                case ParamType.Boolean:
                    form.panBoolean.Visible = true;
                    form.chkBoolean.Checked = currentvalue is bool boolval && boolval;
                    break;
                case ParamType.DateTime:
                    form.panDateTime.Visible = true;
                    if (currentvalue is DateTime dtvalue)
                    {
                        form.dtDateTime.Value = dtvalue;
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        private static DialogResult HandleInput(Entity input, IOrganizationService service, IWin32Window owner, InputValue form, ParamType type)
        {
            var invalidstr = false;
            input["value"] = null;
            switch (type)
            {
                case ParamType.String:
                    var strvalue = form.txtString.Text;
                    input["rawvalue"] = strvalue;
                    break;
                case ParamType.Integer:
                case ParamType.Int:
                case ParamType.Int32:
                case ParamType.Int64:
                    if (!int.TryParse(form.txtString.Text, out int intvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    input["rawvalue"] = intvalue;
                    break;
                case ParamType.Decimal:
                case ParamType.Money:
                    if (!decimal.TryParse(form.txtString.Text, out decimal decvalue))
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
                    if (!double.TryParse(form.txtString.Text, out double douvalue))
                    {
                        invalidstr = true;
                        break;
                    }
                    input["rawvalue"] = douvalue;
                    break;
                case ParamType.Entity:
                case ParamType.EntityReference:
                    if (form.txtRecord.Entity == null)
                    {
                        if (MessageBox.Show($"Please select a record", "Input Parameter", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                        {
                            return ShowDialog(input, service, owner);
                        }
                        else
                        {
                            return DialogResult.Cancel;
                        }
                    }
                    if (type == ParamType.Entity)
                    {
                        input["rawvalue"] = form.txtRecord.Entity;
                    }
                    else
                    {
                        input["rawvalue"] = form.txtRecord.Entity.ToEntityReference();
                    }
                    input["value"] = form.txtRecord.Text;
                    break;
                case ParamType.Boolean:
                    input["rawvalue"] = form.chkBoolean.Checked;
                    break;
                case ParamType.DateTime:
                    input["rawvalue"] = form.dtDateTime.Value;
                    break;
            }
            if (invalidstr)
            {
                if (MessageBox.Show($"Not a valid {type}: {form.txtString.Text}", "Input Parameter", MessageBoxButtons.OKCancel, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    return ShowDialog(input, service, owner);
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
            var lkp = new CDSLookupDialog
            {
                Service = cmbEntities.Service,
                LogicalName = cmbEntities.SelectedEntity.LogicalName
            };
            if (lkp.ShowDialog(this) != DialogResult.OK)
            {
                return;
            }
            txtRecord.Entity = lkp.Entity;
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
