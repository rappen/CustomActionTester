namespace Rappen.XTB.CAT
{
    partial class InputValue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panEntityReference = new System.Windows.Forms.Panel();
            this.btnLookup = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRecord = new xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbEntities = new xrmtb.XrmToolBox.Controls.EntitiesDropdownControl();
            this.panString = new System.Windows.Forms.Panel();
            this.txtString = new System.Windows.Forms.TextBox();
            this.lblText = new System.Windows.Forms.Label();
            this.panHeader = new System.Windows.Forms.Panel();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panEntityReference.SuspendLayout();
            this.panString.SuspendLayout();
            this.panHeader.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panEntityReference
            // 
            this.panEntityReference.Controls.Add(this.btnLookup);
            this.panEntityReference.Controls.Add(this.label2);
            this.panEntityReference.Controls.Add(this.txtRecord);
            this.panEntityReference.Controls.Add(this.label1);
            this.panEntityReference.Controls.Add(this.cmbEntities);
            this.panEntityReference.Dock = System.Windows.Forms.DockStyle.Top;
            this.panEntityReference.Location = new System.Drawing.Point(0, 48);
            this.panEntityReference.Name = "panEntityReference";
            this.panEntityReference.Size = new System.Drawing.Size(426, 79);
            this.panEntityReference.TabIndex = 0;
            this.panEntityReference.Visible = false;
            // 
            // btnLookup
            // 
            this.btnLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLookup.Location = new System.Drawing.Point(387, 42);
            this.btnLookup.Name = "btnLookup";
            this.btnLookup.Size = new System.Drawing.Size(24, 22);
            this.btnLookup.TabIndex = 4;
            this.btnLookup.Text = "...";
            this.btnLookup.UseVisualStyleBackColor = true;
            this.btnLookup.Click += new System.EventHandler(this.btnLookup_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Record";
            // 
            // txtRecord
            // 
            this.txtRecord.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRecord.BackColor = System.Drawing.SystemColors.Window;
            this.txtRecord.DisplayFormat = "";
            this.txtRecord.Entity = null;
            this.txtRecord.EntityReference = null;
            this.txtRecord.Id = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.txtRecord.Location = new System.Drawing.Point(119, 43);
            this.txtRecord.LogicalName = null;
            this.txtRecord.Name = "txtRecord";
            this.txtRecord.OrganizationService = null;
            this.txtRecord.Size = new System.Drawing.Size(263, 20);
            this.txtRecord.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Entity";
            // 
            // cmbEntities
            // 
            this.cmbEntities.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEntities.AutoLoadData = true;
            this.cmbEntities.LanguageCode = 1033;
            this.cmbEntities.Location = new System.Drawing.Point(115, 12);
            this.cmbEntities.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbEntities.Name = "cmbEntities";
            this.cmbEntities.Service = null;
            this.cmbEntities.Size = new System.Drawing.Size(295, 25);
            this.cmbEntities.SolutionFilter = null;
            this.cmbEntities.TabIndex = 0;
            // 
            // panString
            // 
            this.panString.Controls.Add(this.txtString);
            this.panString.Controls.Add(this.lblText);
            this.panString.Dock = System.Windows.Forms.DockStyle.Top;
            this.panString.Location = new System.Drawing.Point(0, 127);
            this.panString.Name = "panString";
            this.panString.Size = new System.Drawing.Size(426, 53);
            this.panString.TabIndex = 1;
            this.panString.Visible = false;
            // 
            // txtString
            // 
            this.txtString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtString.Location = new System.Drawing.Point(119, 16);
            this.txtString.Name = "txtString";
            this.txtString.Size = new System.Drawing.Size(292, 20);
            this.txtString.TabIndex = 1;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(36, 19);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(28, 13);
            this.lblText.TabIndex = 0;
            this.lblText.Text = "Text";
            // 
            // panHeader
            // 
            this.panHeader.Controls.Add(this.textBox2);
            this.panHeader.Controls.Add(this.label4);
            this.panHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panHeader.Location = new System.Drawing.Point(0, 0);
            this.panHeader.Name = "panHeader";
            this.panHeader.Size = new System.Drawing.Size(426, 48);
            this.panHeader.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Window;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.Location = new System.Drawing.Point(205, 16);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(274, 19);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "Blabla";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(33, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(149, 19);
            this.label4.TabIndex = 0;
            this.label4.Text = "Set value for input parameter";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 129);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 52);
            this.panel1.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(336, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(255, 17);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // InputValue
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(426, 181);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panString);
            this.Controls.Add(this.panEntityReference);
            this.Controls.Add(this.panHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "InputValue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input Parameter Dialog";
            this.panEntityReference.ResumeLayout(false);
            this.panEntityReference.PerformLayout();
            this.panString.ResumeLayout(false);
            this.panString.PerformLayout();
            this.panHeader.ResumeLayout(false);
            this.panHeader.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panEntityReference;
        private System.Windows.Forms.Button btnLookup;
        private System.Windows.Forms.Label label2;
        private xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox txtRecord;
        private System.Windows.Forms.Label label1;
        private xrmtb.XrmToolBox.Controls.EntitiesDropdownControl cmbEntities;
        private System.Windows.Forms.Panel panString;
        private System.Windows.Forms.TextBox txtString;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Panel panHeader;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
    }
}