namespace Rensoft.Hosting.ManagerClient.DataViewing
{
    partial class ServerStatusViewer
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
            this.serverDataGridView = new System.Windows.Forms.DataGridView();
            this.NameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ActionColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            ((System.ComponentModel.ISupportInitialize)(this.serverDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // serverDataGridView
            // 
            this.serverDataGridView.AllowUserToAddRows = false;
            this.serverDataGridView.AllowUserToDeleteRows = false;
            this.serverDataGridView.AllowUserToResizeRows = false;
            this.serverDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.serverDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.serverDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.serverDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.serverDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameColumn,
            this.ValueColumn,
            this.ActionColumn});
            this.serverDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverDataGridView.GridColor = System.Drawing.SystemColors.Control;
            this.serverDataGridView.Location = new System.Drawing.Point(0, 0);
            this.serverDataGridView.MultiSelect = false;
            this.serverDataGridView.Name = "serverDataGridView";
            this.serverDataGridView.ReadOnly = true;
            this.serverDataGridView.RowHeadersVisible = false;
            this.serverDataGridView.Size = new System.Drawing.Size(569, 318);
            this.serverDataGridView.TabIndex = 1;
            this.serverDataGridView.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.serverDataGridView_RowPrePaint);
            this.serverDataGridView.SelectionChanged += new System.EventHandler(this.serverDataGridView_SelectionChanged);
            this.serverDataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.serverDataGridView_CellContentClick);
            // 
            // NameColumn
            // 
            this.NameColumn.DataPropertyName = "Name";
            this.NameColumn.FillWeight = 50F;
            this.NameColumn.HeaderText = "Name";
            this.NameColumn.Name = "NameColumn";
            this.NameColumn.ReadOnly = true;
            // 
            // ValueColumn
            // 
            this.ValueColumn.DataPropertyName = "Value";
            this.ValueColumn.FillWeight = 50F;
            this.ValueColumn.HeaderText = "Value";
            this.ValueColumn.Name = "ValueColumn";
            this.ValueColumn.ReadOnly = true;
            // 
            // ActionColumn
            // 
            this.ActionColumn.DataPropertyName = "ActionText";
            this.ActionColumn.FillWeight = 20F;
            this.ActionColumn.HeaderText = "Action";
            this.ActionColumn.Name = "ActionColumn";
            this.ActionColumn.ReadOnly = true;
            this.ActionColumn.VisitedLinkColor = System.Drawing.Color.Blue;
            // 
            // ServerStatusViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.serverDataGridView);
            this.Name = "ServerStatusViewer";
            this.Controls.SetChildIndex(this.serverDataGridView, 0);
            ((System.ComponentModel.ISupportInitialize)(this.serverDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView serverDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueColumn;
        private System.Windows.Forms.DataGridViewLinkColumn ActionColumn;
    }
}
