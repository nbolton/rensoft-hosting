namespace Rensoft.Hosting.ManagerClient
{
    partial class MainScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainScreen));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.serverTabPage = new System.Windows.Forms.TabPage();
            this.serverViewer1 = new Rensoft.Hosting.ManagerClient.DataViewing.ServerStatusViewer();
            this.customerTabPage = new System.Windows.Forms.TabPage();
            this.customerViewer1 = new Rensoft.Hosting.ManagerClient.DataViewing.CustomerViewer();
            this.websiteTabPage = new System.Windows.Forms.TabPage();
            this.websiteViewer1 = new Rensoft.Hosting.ManagerClient.DataViewing.WebsiteViewer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.websiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importMicrosoftDnsZonesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.importRensoftDnsZonesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.serverTabPage.SuspendLayout();
            this.customerTabPage.SuspendLayout();
            this.websiteTabPage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.tabControl1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(692, 427);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(692, 451);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.serverTabPage);
            this.tabControl1.Controls.Add(this.customerTabPage);
            this.tabControl1.Controls.Add(this.websiteTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(692, 427);
            this.tabControl1.TabIndex = 3;
            // 
            // serverTabPage
            // 
            this.serverTabPage.Controls.Add(this.serverViewer1);
            this.serverTabPage.Location = new System.Drawing.Point(4, 22);
            this.serverTabPage.Name = "serverTabPage";
            this.serverTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.serverTabPage.Size = new System.Drawing.Size(684, 401);
            this.serverTabPage.TabIndex = 2;
            this.serverTabPage.Text = "Server";
            this.serverTabPage.UseVisualStyleBackColor = true;
            // 
            // serverViewer1
            // 
            this.serverViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serverViewer1.Location = new System.Drawing.Point(3, 3);
            this.serverViewer1.Name = "serverViewer1";
            this.serverViewer1.Size = new System.Drawing.Size(678, 395);
            this.serverViewer1.TabIndex = 0;
            // 
            // customerTabPage
            // 
            this.customerTabPage.Controls.Add(this.customerViewer1);
            this.customerTabPage.Location = new System.Drawing.Point(4, 22);
            this.customerTabPage.Name = "customerTabPage";
            this.customerTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.customerTabPage.Size = new System.Drawing.Size(684, 401);
            this.customerTabPage.TabIndex = 0;
            this.customerTabPage.Text = "Customers";
            this.customerTabPage.UseVisualStyleBackColor = true;
            // 
            // customerViewer1
            // 
            this.customerViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customerViewer1.Location = new System.Drawing.Point(3, 3);
            this.customerViewer1.Name = "customerViewer1";
            this.customerViewer1.Size = new System.Drawing.Size(678, 395);
            this.customerViewer1.TabIndex = 0;
            // 
            // websiteTabPage
            // 
            this.websiteTabPage.Controls.Add(this.websiteViewer1);
            this.websiteTabPage.Location = new System.Drawing.Point(4, 22);
            this.websiteTabPage.Name = "websiteTabPage";
            this.websiteTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.websiteTabPage.Size = new System.Drawing.Size(684, 401);
            this.websiteTabPage.TabIndex = 1;
            this.websiteTabPage.Text = "Websites";
            this.websiteTabPage.UseVisualStyleBackColor = true;
            // 
            // websiteViewer1
            // 
            this.websiteViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.websiteViewer1.Location = new System.Drawing.Point(3, 3);
            this.websiteViewer1.Name = "websiteViewer1";
            this.websiteViewer1.Size = new System.Drawing.Size(678, 395);
            this.websiteViewer1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(692, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.customerToolStripMenuItem,
            this.websiteToolStripMenuItem});
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.newToolStripMenuItem.Text = "&New";
            // 
            // customerToolStripMenuItem
            // 
            this.customerToolStripMenuItem.Name = "customerToolStripMenuItem";
            this.customerToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.customerToolStripMenuItem.Text = "&Customer";
            this.customerToolStripMenuItem.Click += new System.EventHandler(this.customerToolStripMenuItem_Click);
            // 
            // websiteToolStripMenuItem
            // 
            this.websiteToolStripMenuItem.Enabled = false;
            this.websiteToolStripMenuItem.Name = "websiteToolStripMenuItem";
            this.websiteToolStripMenuItem.Size = new System.Drawing.Size(120, 22);
            this.websiteToolStripMenuItem.Text = "&Website";
            this.websiteToolStripMenuItem.Click += new System.EventHandler(this.websiteToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(92, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(95, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.toolStripSeparator2,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importMicrosoftDnsZonesToolStripMenuItem,
            this.importRensoftDnsZonesToolStripMenuItem});
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.importToolStripMenuItem.Text = "&Import";
            // 
            // importMicrosoftDnsZonesToolStripMenuItem
            // 
            this.importMicrosoftDnsZonesToolStripMenuItem.Name = "importMicrosoftDnsZonesToolStripMenuItem";
            this.importMicrosoftDnsZonesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.importMicrosoftDnsZonesToolStripMenuItem.Text = "Microsoft DNS Zones";
            this.importMicrosoftDnsZonesToolStripMenuItem.Click += new System.EventHandler(this.importMsDnsZonesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Enabled = false;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Enabled = false;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 451);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(692, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(38, 17);
            this.statusLabel.Text = "Status";
            // 
            // importRensoftDnsZonesToolStripMenuItem
            // 
            this.importRensoftDnsZonesToolStripMenuItem.Name = "importRensoftDnsZonesToolStripMenuItem";
            this.importRensoftDnsZonesToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.importRensoftDnsZonesToolStripMenuItem.Text = "Rensoft DNS Zones";
            this.importRensoftDnsZonesToolStripMenuItem.Click += new System.EventHandler(this.importRensoftDnsZonesToolStripMenuItem_Click);
            // 
            // MainScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 473);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainScreen";
            this.Text = "Rensoft Hosting Manager 2008";
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.serverTabPage.ResumeLayout(false);
            this.customerTabPage.ResumeLayout(false);
            this.websiteTabPage.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem websiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage customerTabPage;
        private System.Windows.Forms.TabPage websiteTabPage;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private Rensoft.Hosting.ManagerClient.DataViewing.CustomerViewer customerViewer1;
        private Rensoft.Hosting.ManagerClient.DataViewing.WebsiteViewer websiteViewer1;
        private System.Windows.Forms.TabPage serverTabPage;
        private Rensoft.Hosting.ManagerClient.DataViewing.ServerStatusViewer serverViewer1;
        private System.Windows.Forms.ToolStripMenuItem importMicrosoftDnsZonesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importRensoftDnsZonesToolStripMenuItem;
    }
}

