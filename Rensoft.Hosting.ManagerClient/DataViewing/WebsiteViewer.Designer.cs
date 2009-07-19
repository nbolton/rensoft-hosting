namespace Rensoft.Hosting.ManagerClient.DataViewing
{
    partial class WebsiteViewer
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
            this.listView = new System.Windows.Forms.ListView();
            this.addressColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.customerColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.browseWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exploreContentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDirectoryBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView
            // 
            this.listView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.addressColumnHeader,
            this.customerColumnHeader});
            this.listView.ContextMenuStrip = this.contextMenuStrip1;
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(569, 296);
            this.listView.TabIndex = 1;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.VirtualMode = true;
            this.listView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.listView_RetrieveVirtualItem);
            // 
            // addressColumnHeader
            // 
            this.addressColumnHeader.Text = "Address";
            this.addressColumnHeader.Width = 300;
            // 
            // customerColumnHeader
            // 
            this.customerColumnHeader.Text = "Customer";
            this.customerColumnHeader.Width = 200;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.browseWebsiteToolStripMenuItem,
            this.exploreContentToolStripMenuItem,
            this.toolStripSeparator1,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 98);
            // 
            // browseWebsiteToolStripMenuItem
            // 
            this.browseWebsiteToolStripMenuItem.Image = global::Rensoft.Hosting.ManagerClient.Properties.Resources.Preview;
            this.browseWebsiteToolStripMenuItem.Name = "browseWebsiteToolStripMenuItem";
            this.browseWebsiteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.browseWebsiteToolStripMenuItem.Text = "Browse Website";
            this.browseWebsiteToolStripMenuItem.Click += new System.EventHandler(this.previewToolStripMenuItem_Click);
            // 
            // exploreContentToolStripMenuItem
            // 
            this.exploreContentToolStripMenuItem.Name = "exploreContentToolStripMenuItem";
            this.exploreContentToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exploreContentToolStripMenuItem.Text = "Explore Content";
            this.exploreContentToolStripMenuItem.Click += new System.EventHandler(this.exploreContentToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::Rensoft.Hosting.ManagerClient.Properties.Resources.Delete;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.deleteToolStripMenuItem.Text = "&Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // openDirectoryBackgroundWorker
            // 
            this.openDirectoryBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.openDirectoryBackgroundWorker_DoWork);
            this.openDirectoryBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.openDirectoryBackgroundWorker_RunWorkerCompleted);
            // 
            // WebsiteViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.listView);
            this.Name = "WebsiteViewer";
            this.Controls.SetChildIndex(this.listView, 0);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView;
        private System.Windows.Forms.ColumnHeader addressColumnHeader;
        private System.Windows.Forms.ColumnHeader customerColumnHeader;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem browseWebsiteToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker openDirectoryBackgroundWorker;
        private System.Windows.Forms.ToolStripMenuItem exploreContentToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
    }
}
