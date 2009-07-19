namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorForms
{
    partial class CustomerEditorForm
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
            this.customerGeneralEditorControl1 = new Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.CustomerGeneralEditorControl();
            this.customerFtpUsersEditorControl1 = new Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.CustomerFtpUsersEditorControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.ftpAccessTabPage = new System.Windows.Forms.TabPage();
            this.ContentPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.ftpAccessTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContentPanel
            // 
            this.ContentPanel.Controls.Add(this.tabControl1);
            this.ContentPanel.Size = new System.Drawing.Size(482, 287);
            this.ContentPanel.TabIndex = 0;
            // 
            // customerGeneralEditorControl1
            // 
            this.customerGeneralEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customerGeneralEditorControl1.Location = new System.Drawing.Point(3, 3);
            this.customerGeneralEditorControl1.Name = "customerGeneralEditorControl1";
            this.customerGeneralEditorControl1.Size = new System.Drawing.Size(458, 245);
            this.customerGeneralEditorControl1.TabIndex = 0;
            // 
            // customerFtpUsersEditorControl1
            // 
            this.customerFtpUsersEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customerFtpUsersEditorControl1.Location = new System.Drawing.Point(3, 3);
            this.customerFtpUsersEditorControl1.Name = "customerFtpUsersEditorControl1";
            this.customerFtpUsersEditorControl1.Size = new System.Drawing.Size(473, 217);
            this.customerFtpUsersEditorControl1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.generalTabPage);
            this.tabControl1.Controls.Add(this.ftpAccessTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(5, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(472, 277);
            this.tabControl1.TabIndex = 0;
            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.customerGeneralEditorControl1);
            this.generalTabPage.Location = new System.Drawing.Point(4, 22);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.generalTabPage.Size = new System.Drawing.Size(464, 251);
            this.generalTabPage.TabIndex = 0;
            this.generalTabPage.Text = "General";
            this.generalTabPage.UseVisualStyleBackColor = true;
            // 
            // ftpAccessTabPage
            // 
            this.ftpAccessTabPage.Controls.Add(this.customerFtpUsersEditorControl1);
            this.ftpAccessTabPage.Location = new System.Drawing.Point(4, 22);
            this.ftpAccessTabPage.Name = "ftpAccessTabPage";
            this.ftpAccessTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ftpAccessTabPage.Size = new System.Drawing.Size(479, 223);
            this.ftpAccessTabPage.TabIndex = 1;
            this.ftpAccessTabPage.Text = "FTP Access";
            this.ftpAccessTabPage.UseVisualStyleBackColor = true;
            // 
            // CustomerEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(482, 358);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "CustomerEditorForm";
            this.Text = "Customer Editor";
            this.ContentPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.ftpAccessTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.CustomerFtpUsersEditorControl customerFtpUsersEditorControl1;
        private Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.CustomerGeneralEditorControl customerGeneralEditorControl1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.TabPage ftpAccessTabPage;

    }
}
