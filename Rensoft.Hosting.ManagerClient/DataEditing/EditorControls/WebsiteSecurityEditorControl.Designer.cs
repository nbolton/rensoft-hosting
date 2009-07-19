namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorControls
{
    partial class WebsiteSecurityEditorControl
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
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.securityTemplateAccessInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.RelativePathColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccessColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ReadColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.WriteColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DeleteColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.securityTemplateAccessInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.addButton.Location = new System.Drawing.Point(3, 267);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "&Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Location = new System.Drawing.Point(84, 267);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(75, 23);
            this.removeButton.TabIndex = 2;
            this.removeButton.Text = "&Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RelativePathColumn,
            this.AccessColumn,
            this.ReadColumn,
            this.WriteColumn,
            this.DeleteColumn});
            this.dataGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView.Location = new System.Drawing.Point(3, 3);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(514, 258);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellValueChanged);
            this.dataGridView.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView_CellBeginEdit);
            this.dataGridView.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dataGridView_RowPrePaint);
            // 
            // securityTemplateAccessInfoBindingSource
            // 
            this.securityTemplateAccessInfoBindingSource.DataSource = typeof(Rensoft.Hosting.ManagerClient.DataEditing.SecurityTemplateAccessInfo);
            // 
            // RelativePathColumn
            // 
            this.RelativePathColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RelativePathColumn.DataPropertyName = "RelativePath";
            this.RelativePathColumn.HeaderText = "Relative path";
            this.RelativePathColumn.Name = "RelativePathColumn";
            // 
            // AccessColumn
            // 
            this.AccessColumn.DataPropertyName = "Access";
            this.AccessColumn.DataSource = this.securityTemplateAccessInfoBindingSource;
            this.AccessColumn.DisplayMember = "Name";
            this.AccessColumn.HeaderText = "Access";
            this.AccessColumn.Name = "AccessColumn";
            this.AccessColumn.ValueMember = "Access";
            this.AccessColumn.Width = 60;
            // 
            // ReadColumn
            // 
            this.ReadColumn.DataPropertyName = "Read";
            this.ReadColumn.HeaderText = "Read";
            this.ReadColumn.Name = "ReadColumn";
            this.ReadColumn.Width = 45;
            // 
            // WriteColumn
            // 
            this.WriteColumn.DataPropertyName = "Write";
            this.WriteColumn.HeaderText = "Write";
            this.WriteColumn.Name = "WriteColumn";
            this.WriteColumn.Width = 45;
            // 
            // DeleteColumn
            // 
            this.DeleteColumn.DataPropertyName = "Delete";
            this.DeleteColumn.HeaderText = "Delete";
            this.DeleteColumn.Name = "DeleteColumn";
            this.DeleteColumn.Width = 45;
            // 
            // WebsiteSecurityEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.removeButton);
            this.Controls.Add(this.dataGridView);
            this.Name = "WebsiteSecurityEditorControl";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.securityTemplateAccessInfoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.BindingSource securityTemplateAccessInfoBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn RelativePathColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn AccessColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ReadColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn WriteColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn DeleteColumn;
    }
}
