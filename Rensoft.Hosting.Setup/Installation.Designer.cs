namespace Rensoft.Installer.Client
{
    partial class Installation
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.installDataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.ResultColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.ComponentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MessageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.installDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // installDataGridView
            // 
            this.installDataGridView.AllowUserToAddRows = false;
            this.installDataGridView.AllowUserToDeleteRows = false;
            this.installDataGridView.AllowUserToResizeRows = false;
            this.installDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.installDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.installDataGridView.BackgroundColor = System.Drawing.SystemColors.Window;
            this.installDataGridView.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.installDataGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.installDataGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.installDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.installDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ResultColumn,
            this.ComponentColumn,
            this.StatusColumn,
            this.MessageColumn});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.installDataGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.installDataGridView.GridColor = System.Drawing.SystemColors.Window;
            this.installDataGridView.Location = new System.Drawing.Point(8, 32);
            this.installDataGridView.Margin = new System.Windows.Forms.Padding(8);
            this.installDataGridView.Name = "installDataGridView";
            this.installDataGridView.ReadOnly = true;
            this.installDataGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.installDataGridView.RowHeadersVisible = false;
            this.installDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.installDataGridView.Size = new System.Drawing.Size(411, 213);
            this.installDataGridView.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(5);
            this.label1.Size = new System.Drawing.Size(427, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Please wait, your Rensoft applications are now installing...";
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // ResultColumn
            // 
            this.ResultColumn.FillWeight = 10F;
            this.ResultColumn.HeaderText = "";
            this.ResultColumn.Name = "ResultColumn";
            this.ResultColumn.ReadOnly = true;
            this.ResultColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // ComponentColumn
            // 
            this.ComponentColumn.HeaderText = "Component";
            this.ComponentColumn.Name = "ComponentColumn";
            this.ComponentColumn.ReadOnly = true;
            this.ComponentColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // StatusColumn
            // 
            this.StatusColumn.FillWeight = 30F;
            this.StatusColumn.HeaderText = "Status";
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.ReadOnly = true;
            this.StatusColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MessageColumn
            // 
            this.MessageColumn.FillWeight = 50F;
            this.MessageColumn.HeaderText = "Message";
            this.MessageColumn.Name = "MessageColumn";
            this.MessageColumn.ReadOnly = true;
            this.MessageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MessageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Installation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.installDataGridView);
            this.Controls.Add(this.label1);
            this.Name = "Installation";
            ((System.ComponentModel.ISupportInitialize)(this.installDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView installDataGridView;
        private System.Windows.Forms.Label label1;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.DataGridViewImageColumn ResultColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComponentColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StatusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn MessageColumn;
    }
}
