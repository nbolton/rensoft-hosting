namespace Rensoft.Hosting.ManagerClient.DataViewing
{
    partial class LocalDataViewerControl
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
            this.refreshPanel = new System.Windows.Forms.Panel();
            this.refreshLinkLabel = new System.Windows.Forms.LinkLabel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.refreshPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // refreshPanel
            // 
            this.refreshPanel.Controls.Add(this.refreshLinkLabel);
            this.refreshPanel.Controls.Add(this.pictureBox1);
            this.refreshPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.refreshPanel.Location = new System.Drawing.Point(0, 296);
            this.refreshPanel.Name = "refreshPanel";
            this.refreshPanel.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.refreshPanel.Size = new System.Drawing.Size(569, 22);
            this.refreshPanel.TabIndex = 0;
            // 
            // refreshLinkLabel
            // 
            this.refreshLinkLabel.AutoSize = true;
            this.refreshLinkLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.refreshLinkLabel.Location = new System.Drawing.Point(16, 3);
            this.refreshLinkLabel.Name = "refreshLinkLabel";
            this.refreshLinkLabel.Padding = new System.Windows.Forms.Padding(1, 2, 0, 0);
            this.refreshLinkLabel.Size = new System.Drawing.Size(45, 15);
            this.refreshLinkLabel.TabIndex = 1;
            this.refreshLinkLabel.TabStop = true;
            this.refreshLinkLabel.Text = "Refresh";
            this.refreshLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.refreshLinkLabel_LinkClicked);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = global::Rensoft.Hosting.ManagerClient.Properties.Resources.Refresh;
            this.pictureBox1.Location = new System.Drawing.Point(0, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 19);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // LocalDataViewerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.refreshPanel);
            this.Name = "LocalDataViewerControl";
            this.refreshPanel.ResumeLayout(false);
            this.refreshPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel refreshPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel refreshLinkLabel;
    }
}
