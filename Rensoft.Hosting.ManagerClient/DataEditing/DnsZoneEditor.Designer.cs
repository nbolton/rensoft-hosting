namespace Rensoft.Hosting.ManagerClient.DataEditing
{
    partial class DnsZoneEditor
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DnsZoneEditor));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.recordRemoveButton = new System.Windows.Forms.Button();
            this.recordDataGridView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TtlColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TypeColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dnsRecordTypeInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.recordAddButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.defaultTtlTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.acceptButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.recordDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dnsRecordTypeInfoBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.recordRemoveButton);
            this.groupBox1.Controls.Add(this.recordDataGridView);
            this.groupBox1.Controls.Add(this.recordAddButton);
            this.groupBox1.Location = new System.Drawing.Point(12, 93);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox1.Size = new System.Drawing.Size(449, 330);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Records";
            // 
            // recordRemoveButton
            // 
            this.recordRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recordRemoveButton.Location = new System.Drawing.Point(89, 296);
            this.recordRemoveButton.Name = "recordRemoveButton";
            this.recordRemoveButton.Size = new System.Drawing.Size(75, 23);
            this.recordRemoveButton.TabIndex = 3;
            this.recordRemoveButton.Text = "&Remove";
            this.recordRemoveButton.UseVisualStyleBackColor = true;
            this.recordRemoveButton.Click += new System.EventHandler(this.recordRemoveButton_Click);
            // 
            // recordDataGridView
            // 
            this.recordDataGridView.AllowUserToAddRows = false;
            this.recordDataGridView.AllowUserToDeleteRows = false;
            this.recordDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.recordDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.recordDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.recordDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.recordDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.TtlColumn,
            this.TypeColumn,
            this.ValueColumn});
            this.recordDataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.recordDataGridView.Location = new System.Drawing.Point(8, 21);
            this.recordDataGridView.Name = "recordDataGridView";
            this.recordDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.recordDataGridView.Size = new System.Drawing.Size(433, 269);
            this.recordDataGridView.TabIndex = 0;
            this.recordDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.recordDataGridView_CellValueChanged);
            // 
            // NameColumn
            // 
            this.NameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.NameColumn.DataPropertyName = "Name";
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            // 
            // TtlColumn
            // 
            this.TtlColumn.DataPropertyName = "Ttl";
            this.TtlColumn.HeaderText = "TTL";
            this.TtlColumn.Name = "TtlColumn";
            this.TtlColumn.Width = 50;
            // 
            // TypeColumn
            // 
            this.TypeColumn.DataPropertyName = "RecordType";
            this.TypeColumn.DataSource = this.dnsRecordTypeInfoBindingSource;
            this.TypeColumn.DisplayMember = "Name";
            this.TypeColumn.HeaderText = "Type";
            this.TypeColumn.Name = "TypeColumn";
            this.TypeColumn.ValueMember = "RecordType";
            this.TypeColumn.Width = 80;
            // 
            // dnsRecordTypeInfoBindingSource
            // 
            this.dnsRecordTypeInfoBindingSource.DataSource = typeof(Rensoft.Hosting.ManagerClient.DataEditing.DnsRecordTypeInfo);
            // 
            // ValueColumn
            // 
            this.ValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ValueColumn.DataPropertyName = "Value";
            this.ValueColumn.HeaderText = "Value";
            this.ValueColumn.Name = "ValueColumn";
            // 
            // recordAddButton
            // 
            this.recordAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.recordAddButton.Location = new System.Drawing.Point(8, 296);
            this.recordAddButton.Name = "recordAddButton";
            this.recordAddButton.Size = new System.Drawing.Size(75, 23);
            this.recordAddButton.TabIndex = 2;
            this.recordAddButton.Text = "&Add";
            this.recordAddButton.UseVisualStyleBackColor = true;
            this.recordAddButton.Click += new System.EventHandler(this.recordAddButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.defaultTtlTextBox);
            this.groupBox2.Controls.Add(this.nameTextBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(8);
            this.groupBox2.Size = new System.Drawing.Size(449, 75);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Zone";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Default TTL:";
            // 
            // defaultTtlTextBox
            // 
            this.defaultTtlTextBox.Location = new System.Drawing.Point(94, 44);
            this.defaultTtlTextBox.Name = "defaultTtlTextBox";
            this.defaultTtlTextBox.Size = new System.Drawing.Size(48, 20);
            this.defaultTtlTextBox.TabIndex = 1;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.Location = new System.Drawing.Point(94, 18);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(344, 20);
            this.nameTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // acceptButton
            // 
            this.acceptButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.acceptButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.acceptButton.Location = new System.Drawing.Point(305, 429);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(75, 23);
            this.acceptButton.TabIndex = 2;
            this.acceptButton.Text = "&OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(386, 429);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // DnsZoneEditor
            // 
            this.AcceptButton = this.acceptButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(473, 463);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.acceptButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DnsZoneEditor";
            this.Text = "Dns Zone Editor";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.recordDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dnsRecordTypeInfoBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView recordDataGridView;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox defaultTtlTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource dnsRecordTypeInfoBindingSource;
        private System.Windows.Forms.Button recordRemoveButton;
        private System.Windows.Forms.Button recordAddButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TtlColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn TypeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueColumn;
    }
}