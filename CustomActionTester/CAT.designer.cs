namespace Rappen.XTB.CAT
{
    partial class CustomActionTester
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomActionTester));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.btnExecute = new System.Windows.Forms.ToolStripButton();
            this.cmbCustomActions = new xrmtb.XrmToolBox.Controls.Controls.CDSDataComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtUniqueName = new xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox();
            this.txtMessageName = new xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCreatedBy = new xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gridInputParams = new xrmtb.XrmToolBox.Controls.CRMGridView();
            this.index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.option = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label5 = new System.Windows.Forms.Label();
            this.splitFullForm = new System.Windows.Forms.SplitContainer();
            this.splitLeft = new System.Windows.Forms.SplitContainer();
            this.gbCustomAction = new System.Windows.Forms.GroupBox();
            this.btnSaveInputParamValue = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.txtInputParamValue = new System.Windows.Forms.TextBox();
            this.grResults = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.gridOutputParams = new xrmtb.XrmToolBox.Controls.CRMGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbResultDetails = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rbFormatXML = new System.Windows.Forms.RadioButton();
            this.rbFormatJSON = new System.Windows.Forms.RadioButton();
            this.rbFormatText = new System.Windows.Forms.RadioButton();
            this.txtResultDetail = new System.Windows.Forms.RichTextBox();
            this.toolStripMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridInputParams)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitFullForm)).BeginInit();
            this.splitFullForm.Panel1.SuspendLayout();
            this.splitFullForm.Panel2.SuspendLayout();
            this.splitFullForm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitLeft)).BeginInit();
            this.splitLeft.Panel1.SuspendLayout();
            this.splitLeft.Panel2.SuspendLayout();
            this.splitLeft.SuspendLayout();
            this.gbCustomAction.SuspendLayout();
            this.grResults.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOutputParams)).BeginInit();
            this.gbResultDetails.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExecute});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(1045, 31);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // btnExecute
            // 
            this.btnExecute.Image = ((System.Drawing.Image)(resources.GetObject("btnExecute.Image")));
            this.btnExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(159, 28);
            this.btnExecute.Text = "Execute Custom Action";
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // cmbCustomActions
            // 
            this.cmbCustomActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCustomActions.DisplayFormat = "";
            this.cmbCustomActions.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCustomActions.FormattingEnabled = true;
            this.cmbCustomActions.Location = new System.Drawing.Point(114, 27);
            this.cmbCustomActions.Name = "cmbCustomActions";
            this.cmbCustomActions.OrganizationService = null;
            this.cmbCustomActions.Size = new System.Drawing.Size(289, 21);
            this.cmbCustomActions.TabIndex = 5;
            this.cmbCustomActions.SelectedIndexChanged += new System.EventHandler(this.cmbCustomActions_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Custom Action";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Unique Name";
            // 
            // txtUniqueName
            // 
            this.txtUniqueName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUniqueName.BackColor = System.Drawing.SystemColors.Window;
            this.txtUniqueName.DisplayFormat = "{{uniquename}}";
            this.txtUniqueName.Entity = null;
            this.txtUniqueName.EntityReference = null;
            this.txtUniqueName.Id = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.txtUniqueName.Location = new System.Drawing.Point(114, 55);
            this.txtUniqueName.LogicalName = null;
            this.txtUniqueName.Name = "txtUniqueName";
            this.txtUniqueName.OrganizationService = null;
            this.txtUniqueName.Size = new System.Drawing.Size(289, 20);
            this.txtUniqueName.TabIndex = 8;
            // 
            // txtMessageName
            // 
            this.txtMessageName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessageName.BackColor = System.Drawing.SystemColors.Window;
            this.txtMessageName.DisplayFormat = "{{M.name}}";
            this.txtMessageName.Entity = null;
            this.txtMessageName.EntityReference = null;
            this.txtMessageName.Id = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.txtMessageName.Location = new System.Drawing.Point(114, 81);
            this.txtMessageName.LogicalName = null;
            this.txtMessageName.Name = "txtMessageName";
            this.txtMessageName.OrganizationService = null;
            this.txtMessageName.Size = new System.Drawing.Size(289, 20);
            this.txtMessageName.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Message Name";
            // 
            // txtCreatedBy
            // 
            this.txtCreatedBy.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCreatedBy.BackColor = System.Drawing.SystemColors.Window;
            this.txtCreatedBy.DisplayFormat = "{{createdby}}";
            this.txtCreatedBy.Entity = null;
            this.txtCreatedBy.EntityReference = null;
            this.txtCreatedBy.Id = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.txtCreatedBy.Location = new System.Drawing.Point(114, 107);
            this.txtCreatedBy.LogicalName = null;
            this.txtCreatedBy.Name = "txtCreatedBy";
            this.txtCreatedBy.OrganizationService = null;
            this.txtCreatedBy.Size = new System.Drawing.Size(289, 20);
            this.txtCreatedBy.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 110);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Created By";
            // 
            // gridInputParams
            // 
            this.gridInputParams.AllowUserToOrderColumns = true;
            this.gridInputParams.AllowUserToResizeRows = false;
            this.gridInputParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridInputParams.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridInputParams.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridInputParams.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridInputParams.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.gridInputParams.ColumnOrder = "";
            this.gridInputParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.index,
            this.name,
            this.option,
            this.type,
            this.value});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridInputParams.DefaultCellStyle = dataGridViewCellStyle8;
            this.gridInputParams.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridInputParams.FilterColumns = "";
            this.gridInputParams.Location = new System.Drawing.Point(114, 133);
            this.gridInputParams.MultiSelect = false;
            this.gridInputParams.Name = "gridInputParams";
            this.gridInputParams.OrganizationService = null;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridInputParams.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.gridInputParams.RowHeadersVisible = false;
            this.gridInputParams.RowHeadersWidth = 16;
            this.gridInputParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridInputParams.ShowEditingIcon = false;
            this.gridInputParams.ShowIdColumn = false;
            this.gridInputParams.Size = new System.Drawing.Size(289, 169);
            this.gridInputParams.TabIndex = 13;
            this.gridInputParams.RecordEnter += new xrmtb.XrmToolBox.Controls.CRMRecordEventHandler(this.gridInputParams_RecordEnter);
            // 
            // index
            // 
            this.index.DataPropertyName = "position";
            this.index.HeaderText = "";
            this.index.Name = "index";
            this.index.ReadOnly = true;
            this.index.Width = 20;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.name.Width = 41;
            // 
            // option
            // 
            this.option.DataPropertyName = "optional";
            this.option.FalseValue = "";
            this.option.HeaderText = "Optional";
            this.option.Name = "option";
            this.option.ReadOnly = true;
            this.option.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.option.TrueValue = "";
            this.option.Width = 52;
            // 
            // type
            // 
            this.type.DataPropertyName = "type";
            this.type.HeaderText = "Type";
            this.type.Name = "type";
            this.type.ReadOnly = true;
            this.type.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.type.Width = 37;
            // 
            // value
            // 
            this.value.DataPropertyName = "value";
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            this.value.ReadOnly = true;
            this.value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.value.Width = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Input Params";
            // 
            // splitFullForm
            // 
            this.splitFullForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitFullForm.Location = new System.Drawing.Point(0, 31);
            this.splitFullForm.Name = "splitFullForm";
            // 
            // splitFullForm.Panel1
            // 
            this.splitFullForm.Panel1.Controls.Add(this.splitLeft);
            // 
            // splitFullForm.Panel2
            // 
            this.splitFullForm.Panel2.Controls.Add(this.gbResultDetails);
            this.splitFullForm.Size = new System.Drawing.Size(1045, 638);
            this.splitFullForm.SplitterDistance = 415;
            this.splitFullForm.SplitterWidth = 8;
            this.splitFullForm.TabIndex = 15;
            // 
            // splitLeft
            // 
            this.splitLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitLeft.Location = new System.Drawing.Point(0, 0);
            this.splitLeft.Name = "splitLeft";
            this.splitLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitLeft.Panel1
            // 
            this.splitLeft.Panel1.Controls.Add(this.gbCustomAction);
            // 
            // splitLeft.Panel2
            // 
            this.splitLeft.Panel2.Controls.Add(this.grResults);
            this.splitLeft.Size = new System.Drawing.Size(415, 638);
            this.splitLeft.SplitterDistance = 346;
            this.splitLeft.SplitterWidth = 8;
            this.splitLeft.TabIndex = 15;
            // 
            // gbCustomAction
            // 
            this.gbCustomAction.Controls.Add(this.btnSaveInputParamValue);
            this.gbCustomAction.Controls.Add(this.cmbCustomActions);
            this.gbCustomAction.Controls.Add(this.label7);
            this.gbCustomAction.Controls.Add(this.txtCreatedBy);
            this.gbCustomAction.Controls.Add(this.txtInputParamValue);
            this.gbCustomAction.Controls.Add(this.label2);
            this.gbCustomAction.Controls.Add(this.label1);
            this.gbCustomAction.Controls.Add(this.txtUniqueName);
            this.gbCustomAction.Controls.Add(this.txtMessageName);
            this.gbCustomAction.Controls.Add(this.gridInputParams);
            this.gbCustomAction.Controls.Add(this.label5);
            this.gbCustomAction.Controls.Add(this.label4);
            this.gbCustomAction.Controls.Add(this.label3);
            this.gbCustomAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCustomAction.Location = new System.Drawing.Point(0, 0);
            this.gbCustomAction.Name = "gbCustomAction";
            this.gbCustomAction.Size = new System.Drawing.Size(415, 346);
            this.gbCustomAction.TabIndex = 0;
            this.gbCustomAction.TabStop = false;
            this.gbCustomAction.Text = "Custom Action";
            // 
            // btnSaveInputParamValue
            // 
            this.btnSaveInputParamValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveInputParamValue.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveInputParamValue.Image")));
            this.btnSaveInputParamValue.Location = new System.Drawing.Point(376, 306);
            this.btnSaveInputParamValue.Name = "btnSaveInputParamValue";
            this.btnSaveInputParamValue.Size = new System.Drawing.Size(27, 23);
            this.btnSaveInputParamValue.TabIndex = 19;
            this.btnSaveInputParamValue.UseVisualStyleBackColor = true;
            this.btnSaveInputParamValue.Click += new System.EventHandler(this.btnSaveInputParamValue_Click);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 311);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "Set Value";
            // 
            // txtInputParamValue
            // 
            this.txtInputParamValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputParamValue.Location = new System.Drawing.Point(114, 308);
            this.txtInputParamValue.Name = "txtInputParamValue";
            this.txtInputParamValue.Size = new System.Drawing.Size(256, 20);
            this.txtInputParamValue.TabIndex = 17;
            // 
            // grResults
            // 
            this.grResults.Controls.Add(this.label6);
            this.grResults.Controls.Add(this.gridOutputParams);
            this.grResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grResults.Location = new System.Drawing.Point(0, 0);
            this.grResults.Name = "grResults";
            this.grResults.Size = new System.Drawing.Size(415, 284);
            this.grResults.TabIndex = 17;
            this.grResults.TabStop = false;
            this.grResults.Text = "Results";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Output Params";
            // 
            // gridOutputParams
            // 
            this.gridOutputParams.AllowUserToOrderColumns = true;
            this.gridOutputParams.AllowUserToResizeRows = false;
            this.gridOutputParams.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridOutputParams.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.gridOutputParams.BackgroundColor = System.Drawing.SystemColors.Window;
            this.gridOutputParams.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridOutputParams.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.gridOutputParams.ColumnOrder = "";
            this.gridOutputParams.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gridOutputParams.DefaultCellStyle = dataGridViewCellStyle11;
            this.gridOutputParams.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gridOutputParams.FilterColumns = "";
            this.gridOutputParams.Location = new System.Drawing.Point(114, 27);
            this.gridOutputParams.MultiSelect = false;
            this.gridOutputParams.Name = "gridOutputParams";
            this.gridOutputParams.OrganizationService = null;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridOutputParams.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.gridOutputParams.RowHeadersVisible = false;
            this.gridOutputParams.RowHeadersWidth = 16;
            this.gridOutputParams.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridOutputParams.ShowEditingIcon = false;
            this.gridOutputParams.ShowIdColumn = false;
            this.gridOutputParams.Size = new System.Drawing.Size(289, 237);
            this.gridOutputParams.TabIndex = 15;
            this.gridOutputParams.RecordClick += new xrmtb.XrmToolBox.Controls.CRMRecordEventHandler(this.gridOutputParams_RecordClick);
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "position";
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 20;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "name";
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 41;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "type";
            this.dataGridViewTextBoxColumn2.HeaderText = "Type";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 37;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "value";
            this.dataGridViewTextBoxColumn3.HeaderText = "Value";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 40;
            // 
            // gbResultDetails
            // 
            this.gbResultDetails.Controls.Add(this.txtResultDetail);
            this.gbResultDetails.Controls.Add(this.panel1);
            this.gbResultDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbResultDetails.Location = new System.Drawing.Point(0, 0);
            this.gbResultDetails.Name = "gbResultDetails";
            this.gbResultDetails.Size = new System.Drawing.Size(622, 638);
            this.gbResultDetails.TabIndex = 0;
            this.gbResultDetails.TabStop = false;
            this.gbResultDetails.Text = "Result Details";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rbFormatXML);
            this.panel1.Controls.Add(this.rbFormatJSON);
            this.panel1.Controls.Add(this.rbFormatText);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 16);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(616, 44);
            this.panel1.TabIndex = 1;
            // 
            // rbFormatXML
            // 
            this.rbFormatXML.AutoSize = true;
            this.rbFormatXML.Location = new System.Drawing.Point(184, 12);
            this.rbFormatXML.Name = "rbFormatXML";
            this.rbFormatXML.Size = new System.Drawing.Size(47, 17);
            this.rbFormatXML.TabIndex = 2;
            this.rbFormatXML.Text = "XML";
            this.rbFormatXML.UseVisualStyleBackColor = true;
            this.rbFormatXML.CheckedChanged += new System.EventHandler(this.rbFormatResult_CheckedChanged);
            // 
            // rbFormatJSON
            // 
            this.rbFormatJSON.AutoSize = true;
            this.rbFormatJSON.Location = new System.Drawing.Point(96, 12);
            this.rbFormatJSON.Name = "rbFormatJSON";
            this.rbFormatJSON.Size = new System.Drawing.Size(53, 17);
            this.rbFormatJSON.TabIndex = 1;
            this.rbFormatJSON.Text = "JSON";
            this.rbFormatJSON.UseVisualStyleBackColor = true;
            this.rbFormatJSON.CheckedChanged += new System.EventHandler(this.rbFormatResult_CheckedChanged);
            // 
            // rbFormatText
            // 
            this.rbFormatText.AutoSize = true;
            this.rbFormatText.Checked = true;
            this.rbFormatText.Location = new System.Drawing.Point(18, 12);
            this.rbFormatText.Name = "rbFormatText";
            this.rbFormatText.Size = new System.Drawing.Size(46, 17);
            this.rbFormatText.TabIndex = 0;
            this.rbFormatText.TabStop = true;
            this.rbFormatText.Text = "Text";
            this.rbFormatText.UseVisualStyleBackColor = true;
            this.rbFormatText.CheckedChanged += new System.EventHandler(this.rbFormatResult_CheckedChanged);
            // 
            // txtResultDetail
            // 
            this.txtResultDetail.BackColor = System.Drawing.SystemColors.Window;
            this.txtResultDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResultDetail.Location = new System.Drawing.Point(3, 60);
            this.txtResultDetail.Name = "txtResultDetail";
            this.txtResultDetail.ReadOnly = true;
            this.txtResultDetail.Size = new System.Drawing.Size(616, 575);
            this.txtResultDetail.TabIndex = 0;
            this.txtResultDetail.Text = "";
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.splitFullForm);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(1045, 669);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridInputParams)).EndInit();
            this.splitFullForm.Panel1.ResumeLayout(false);
            this.splitFullForm.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitFullForm)).EndInit();
            this.splitFullForm.ResumeLayout(false);
            this.splitLeft.Panel1.ResumeLayout(false);
            this.splitLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitLeft)).EndInit();
            this.splitLeft.ResumeLayout(false);
            this.gbCustomAction.ResumeLayout(false);
            this.gbCustomAction.PerformLayout();
            this.grResults.ResumeLayout(false);
            this.grResults.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridOutputParams)).EndInit();
            this.gbResultDetails.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private xrmtb.XrmToolBox.Controls.Controls.CDSDataComboBox cmbCustomActions;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox txtUniqueName;
        private xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox txtMessageName;
        private System.Windows.Forms.Label label3;
        private xrmtb.XrmToolBox.Controls.Controls.CDSDataTextBox txtCreatedBy;
        private System.Windows.Forms.Label label4;
        private xrmtb.XrmToolBox.Controls.CRMGridView gridInputParams;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SplitContainer splitFullForm;
        private System.Windows.Forms.SplitContainer splitLeft;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtInputParamValue;
        private System.Windows.Forms.Button btnSaveInputParamValue;
        private System.Windows.Forms.Label label6;
        private xrmtb.XrmToolBox.Controls.CRMGridView gridOutputParams;
        private System.Windows.Forms.ToolStripButton btnExecute;
        private System.Windows.Forms.DataGridViewTextBoxColumn index;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewCheckBoxColumn option;
        private System.Windows.Forms.DataGridViewTextBoxColumn type;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.GroupBox gbCustomAction;
        private System.Windows.Forms.GroupBox grResults;
        private System.Windows.Forms.GroupBox gbResultDetails;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton rbFormatXML;
        private System.Windows.Forms.RadioButton rbFormatJSON;
        private System.Windows.Forms.RadioButton rbFormatText;
        private System.Windows.Forms.RichTextBox txtResultDetail;
    }
}
