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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InputValue));
            this.panEntity = new System.Windows.Forms.Panel();
            this.cmbEntity = new System.Windows.Forms.ComboBox();
            this.btnLookup = new System.Windows.Forms.Button();
            this.txtRecord = new xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox();
            this.panString = new System.Windows.Forms.Panel();
            this.txtString = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.panBoolean = new System.Windows.Forms.Panel();
            this.chkBoolean = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dtDateTime = new System.Windows.Forms.DateTimePicker();
            this.panDateTime = new System.Windows.Forms.Panel();
            this.tbnRemove = new System.Windows.Forms.Button();
            this.panEntity.SuspendLayout();
            this.panString.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panBoolean.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panDateTime.SuspendLayout();
            this.SuspendLayout();
            // 
            // panEntity
            // 
            this.panEntity.Controls.Add(this.cmbEntity);
            this.panEntity.Controls.Add(this.btnLookup);
            this.panEntity.Controls.Add(this.txtRecord);
            this.panEntity.Dock = System.Windows.Forms.DockStyle.Top;
            this.panEntity.Location = new System.Drawing.Point(40, 77);
            this.panEntity.Name = "panEntity";
            this.panEntity.Size = new System.Drawing.Size(336, 74);
            this.panEntity.TabIndex = 0;
            this.panEntity.Visible = false;
            // 
            // cmbEntity
            // 
            this.cmbEntity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbEntity.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbEntity.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbEntity.FormattingEnabled = true;
            this.cmbEntity.Location = new System.Drawing.Point(6, 16);
            this.cmbEntity.Name = "cmbEntity";
            this.cmbEntity.Size = new System.Drawing.Size(315, 21);
            this.cmbEntity.Sorted = true;
            this.cmbEntity.TabIndex = 5;
            this.cmbEntity.SelectedIndexChanged += new System.EventHandler(this.cmbEntity_SelectedIndexChanged);
            // 
            // btnLookup
            // 
            this.btnLookup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLookup.Location = new System.Drawing.Point(297, 42);
            this.btnLookup.Name = "btnLookup";
            this.btnLookup.Size = new System.Drawing.Size(24, 22);
            this.btnLookup.TabIndex = 4;
            this.btnLookup.Text = "...";
            this.btnLookup.UseVisualStyleBackColor = true;
            this.btnLookup.Click += new System.EventHandler(this.btnLookup_Click);
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
            this.txtRecord.Location = new System.Drawing.Point(6, 43);
            this.txtRecord.LogicalName = null;
            this.txtRecord.Name = "txtRecord";
            this.txtRecord.OrganizationService = null;
            this.txtRecord.Size = new System.Drawing.Size(286, 20);
            this.txtRecord.TabIndex = 2;
            // 
            // panString
            // 
            this.panString.Controls.Add(this.txtString);
            this.panString.Dock = System.Windows.Forms.DockStyle.Top;
            this.panString.Location = new System.Drawing.Point(40, 151);
            this.panString.Name = "panString";
            this.panString.Size = new System.Drawing.Size(336, 34);
            this.panString.TabIndex = 1;
            this.panString.Visible = false;
            // 
            // txtString
            // 
            this.txtString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtString.Location = new System.Drawing.Point(6, 6);
            this.txtString.Name = "txtString";
            this.txtString.Size = new System.Drawing.Size(315, 20);
            this.txtString.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbnRemove);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnOk);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(40, 146);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(336, 52);
            this.panel1.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(246, 17);
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
            this.btnOk.Location = new System.Drawing.Point(165, 17);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // panBoolean
            // 
            this.panBoolean.Controls.Add(this.chkBoolean);
            this.panBoolean.Dock = System.Windows.Forms.DockStyle.Top;
            this.panBoolean.Location = new System.Drawing.Point(40, 185);
            this.panBoolean.Name = "panBoolean";
            this.panBoolean.Size = new System.Drawing.Size(336, 27);
            this.panBoolean.TabIndex = 4;
            this.panBoolean.Visible = false;
            // 
            // chkBoolean
            // 
            this.chkBoolean.AutoSize = true;
            this.chkBoolean.Location = new System.Drawing.Point(81, 6);
            this.chkBoolean.Name = "chkBoolean";
            this.chkBoolean.Size = new System.Drawing.Size(15, 14);
            this.chkBoolean.TabIndex = 1;
            this.chkBoolean.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblType);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.lblName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(416, 77);
            this.panel2.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Type:";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblType.Location = new System.Drawing.Point(118, 45);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(31, 13);
            this.lblType.TabIndex = 2;
            this.lblType.Text = "type";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(43, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Variable:";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblName.Location = new System.Drawing.Point(118, 23);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(52, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "variable";
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 77);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(40, 121);
            this.panel3.TabIndex = 6;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(376, 77);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(40, 121);
            this.panel4.TabIndex = 7;
            // 
            // dtDateTime
            // 
            this.dtDateTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDateTime.Location = new System.Drawing.Point(81, 4);
            this.dtDateTime.Name = "dtDateTime";
            this.dtDateTime.Size = new System.Drawing.Size(159, 20);
            this.dtDateTime.TabIndex = 5;
            // 
            // panDateTime
            // 
            this.panDateTime.Controls.Add(this.dtDateTime);
            this.panDateTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.panDateTime.Location = new System.Drawing.Point(40, 212);
            this.panDateTime.Name = "panDateTime";
            this.panDateTime.Size = new System.Drawing.Size(336, 27);
            this.panDateTime.TabIndex = 8;
            this.panDateTime.Visible = false;
            // 
            // tbnRemove
            // 
            this.tbnRemove.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.tbnRemove.Location = new System.Drawing.Point(6, 17);
            this.tbnRemove.Name = "tbnRemove";
            this.tbnRemove.Size = new System.Drawing.Size(103, 23);
            this.tbnRemove.TabIndex = 2;
            this.tbnRemove.Text = "Remove Value";
            this.tbnRemove.UseVisualStyleBackColor = true;
            // 
            // InputValue
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(416, 198);
            this.Controls.Add(this.panDateTime);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panBoolean);
            this.Controls.Add(this.panString);
            this.Controls.Add(this.panEntity);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "InputValue";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input Parameter Dialog";
            this.panEntity.ResumeLayout(false);
            this.panEntity.PerformLayout();
            this.panString.ResumeLayout(false);
            this.panString.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panBoolean.ResumeLayout(false);
            this.panBoolean.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panDateTime.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panEntity;
        private System.Windows.Forms.Button btnLookup;
        private xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox txtRecord;
        private System.Windows.Forms.Panel panString;
        private System.Windows.Forms.TextBox txtString;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Panel panBoolean;
        private System.Windows.Forms.CheckBox chkBoolean;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DateTimePicker dtDateTime;
        private System.Windows.Forms.Panel panDateTime;
        private System.Windows.Forms.ComboBox cmbEntity;
        private System.Windows.Forms.Button tbnRemove;
    }
}