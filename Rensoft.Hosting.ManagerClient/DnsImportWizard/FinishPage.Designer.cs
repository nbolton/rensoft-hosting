namespace Rensoft.Hosting.ManagerClient.DnsImportWizard
{
    partial class FinishPage
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
            this.createPanel = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.createPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // createPanel
            // 
            this.createPanel.Controls.Add(this.progressBar1);
            this.createPanel.Controls.Add(this.label2);
            this.createPanel.Location = new System.Drawing.Point(3, 40);
            this.createPanel.Name = "createPanel";
            this.createPanel.Size = new System.Drawing.Size(421, 49);
            this.createPanel.TabIndex = 8;
            this.createPanel.Visible = false;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(1, 18);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(418, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(421, 34);
            this.label2.TabIndex = 5;
            this.label2.Text = "Please wait while the DNS zones are created or replaced.";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(421, 34);
            this.label1.TabIndex = 7;
            this.label1.Text = "Once you have confirmed the all settings are correct, click the Finish button to " +
                "create or replace the DNS zones. This process may take a short while to complete" +
                ".";
            // 
            // FinishPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.createPanel);
            this.Controls.Add(this.label1);
            this.InfoText = "Click the Finish button to import the DNS zones.";
            this.Name = "FinishPage";
            this.TitleText = "Finish";
            this.createPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel createPanel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
