namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorForms
{
    partial class WebsiteEditorForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.generalTabPage = new System.Windows.Forms.TabPage();
            this.websiteGeneralEditorControl1 = new Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.WebsiteGeneralEditorControl();
            this.hostNamesTabPage = new System.Windows.Forms.TabPage();
            this.websiteHostNameEditorControl1 = new Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.WebsiteHostEditorControl();
            this.dnsZonesTabPage = new System.Windows.Forms.TabPage();
            this.websiteDnsZoneEditorControl1 = new Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.WebsiteDnsZoneEditorControl();
            this.securityTabPage = new System.Windows.Forms.TabPage();
            this.websiteSecurityEditorControl1 = new Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.WebsiteSecurityEditorControl();
            this.ContentPanel.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.generalTabPage.SuspendLayout();
            this.hostNamesTabPage.SuspendLayout();
            this.dnsZonesTabPage.SuspendLayout();
            this.securityTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // ContentPanel
            // 
            this.ContentPanel.Controls.Add(this.tabControl1);
            this.ContentPanel.Size = new System.Drawing.Size(542, 302);
            this.ContentPanel.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.generalTabPage);
            this.tabControl1.Controls.Add(this.hostNamesTabPage);
            this.tabControl1.Controls.Add(this.dnsZonesTabPage);
            this.tabControl1.Controls.Add(this.securityTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(5, 5);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(532, 292);
            this.tabControl1.TabIndex = 0;
            // 
            // generalTabPage
            // 
            this.generalTabPage.Controls.Add(this.websiteGeneralEditorControl1);
            this.generalTabPage.Location = new System.Drawing.Point(4, 22);
            this.generalTabPage.Name = "generalTabPage";
            this.generalTabPage.Size = new System.Drawing.Size(524, 266);
            this.generalTabPage.TabIndex = 0;
            this.generalTabPage.Text = "General";
            this.generalTabPage.UseVisualStyleBackColor = true;
            // 
            // websiteGeneralEditorControl1
            // 
            this.websiteGeneralEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.websiteGeneralEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.websiteGeneralEditorControl1.Name = "websiteGeneralEditorControl1";
            this.websiteGeneralEditorControl1.Size = new System.Drawing.Size(524, 266);
            this.websiteGeneralEditorControl1.TabIndex = 0;
            // 
            // hostNamesTabPage
            // 
            this.hostNamesTabPage.Controls.Add(this.websiteHostNameEditorControl1);
            this.hostNamesTabPage.Location = new System.Drawing.Point(4, 22);
            this.hostNamesTabPage.Name = "hostNamesTabPage";
            this.hostNamesTabPage.Size = new System.Drawing.Size(524, 266);
            this.hostNamesTabPage.TabIndex = 1;
            this.hostNamesTabPage.Text = "Host Names";
            this.hostNamesTabPage.UseVisualStyleBackColor = true;
            // 
            // websiteHostNameEditorControl1
            // 
            this.websiteHostNameEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.websiteHostNameEditorControl1.IpAddressArray = new string[0];
            this.websiteHostNameEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.websiteHostNameEditorControl1.Name = "websiteHostNameEditorControl1";
            this.websiteHostNameEditorControl1.Size = new System.Drawing.Size(524, 266);
            this.websiteHostNameEditorControl1.TabIndex = 0;
            // 
            // dnsZonesTabPage
            // 
            this.dnsZonesTabPage.Controls.Add(this.websiteDnsZoneEditorControl1);
            this.dnsZonesTabPage.Location = new System.Drawing.Point(4, 22);
            this.dnsZonesTabPage.Name = "dnsZonesTabPage";
            this.dnsZonesTabPage.Size = new System.Drawing.Size(479, 223);
            this.dnsZonesTabPage.TabIndex = 2;
            this.dnsZonesTabPage.Text = "DNS Zones";
            this.dnsZonesTabPage.UseVisualStyleBackColor = true;
            // 
            // websiteDnsZoneEditorControl1
            // 
            this.websiteDnsZoneEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.websiteDnsZoneEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.websiteDnsZoneEditorControl1.Name = "websiteDnsZoneEditorControl1";
            this.websiteDnsZoneEditorControl1.Size = new System.Drawing.Size(479, 223);
            this.websiteDnsZoneEditorControl1.TabIndex = 0;
            // 
            // securityTabPage
            // 
            this.securityTabPage.Controls.Add(this.websiteSecurityEditorControl1);
            this.securityTabPage.Location = new System.Drawing.Point(4, 22);
            this.securityTabPage.Name = "securityTabPage";
            this.securityTabPage.Size = new System.Drawing.Size(524, 266);
            this.securityTabPage.TabIndex = 3;
            this.securityTabPage.Text = "Security";
            this.securityTabPage.UseVisualStyleBackColor = true;
            // 
            // websiteSecurityEditorControl1
            // 
            this.websiteSecurityEditorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.websiteSecurityEditorControl1.Location = new System.Drawing.Point(0, 0);
            this.websiteSecurityEditorControl1.Name = "websiteSecurityEditorControl1";
            this.websiteSecurityEditorControl1.Size = new System.Drawing.Size(524, 266);
            this.websiteSecurityEditorControl1.TabIndex = 0;
            // 
            // WebsiteEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(542, 373);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "WebsiteEditorForm";
            this.Text = "Website Editor";
            this.ContentPanel.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.generalTabPage.ResumeLayout(false);
            this.hostNamesTabPage.ResumeLayout(false);
            this.dnsZonesTabPage.ResumeLayout(false);
            this.securityTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage generalTabPage;
        private System.Windows.Forms.TabPage hostNamesTabPage;
        private Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.WebsiteGeneralEditorControl websiteGeneralEditorControl1;
        private Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.WebsiteHostEditorControl websiteHostNameEditorControl1;
        private System.Windows.Forms.TabPage dnsZonesTabPage;
        private Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.WebsiteDnsZoneEditorControl websiteDnsZoneEditorControl1;
        private System.Windows.Forms.TabPage securityTabPage;
        private Rensoft.Hosting.ManagerClient.DataEditing.EditorControls.WebsiteSecurityEditorControl websiteSecurityEditorControl1;
    }
}
