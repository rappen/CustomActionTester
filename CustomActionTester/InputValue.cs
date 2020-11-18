using Microsoft.Xrm.Sdk;
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
            switch (type)
            {
                case ParamType.String:
                case ParamType.Int:
                case ParamType.Int32:
                case ParamType.Int64:
                    form.panString.Visible = true;
                    form.txtString.Text = currentvalue?.ToString();
                    break;
                case ParamType.EntityReference:
                    form.panEntityReference.Visible = true;
                    form.cmbEntities.Service = service;
                    form.txtRecord.OrganizationService = service;
                    if (currentvalue is EntityReference entref)
                    {
                        form.txtRecord.EntityReference = entref;
                    }
                    break;
                case ParamType.Boolean:
                    form.panBoolean.Visible = true;
                    form.chkBoolean.Checked = currentvalue is bool boolval && boolval;
                    break;
                default:
                    return DialogResult.Cancel;
            }
            var result = form.ShowDialog(owner);
            if (result == DialogResult.OK)
            {
                switch (type)
                {
                    case ParamType.String:
                        var strvalue = form.txtString.Text;
                        input["rawvalue"] = strvalue;
                        input["value"] = strvalue;
                        break;
                    case ParamType.Int:
                    case ParamType.Int32:
                    case ParamType.Int64:
                        if (!int.TryParse(form.txtString.Text, out int intvalue))
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
                        input["rawvalue"] = intvalue;
                        input["value"] = intvalue;
                        break;
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
                        input["rawvalue"] = form.txtRecord.Entity.ToEntityReference();
                        input["value"] = form.txtRecord.Text;
                        break;
                    case ParamType.Boolean:
                        input["rawvalue"] = form.chkBoolean.Checked;
                        input["value"] = form.chkBoolean.Checked;
                        break;
                }
            }
            return result;
        }

        private static ParamType GetType(Entity input)
        {
            if (input.TryGetAttributeValue("type", out string typestr) && Enum.TryParse(typestr, out ParamType type))
            {
                return type;
            }
            return ParamType.String;
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
        Int,
        Int32,
        Int64,
        EntityReference,
        Boolean
    }
}
