namespace Rensoft.Hosting.Setup
{
    partial class SettingsPage2
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dnsServersTextBox = new System.Windows.Forms.TextBox();
            this.dnsIpsTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.hostmasterTextBox = new System.Windows.Forms.TextBox();
            this.primaryTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(112, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Suggest DNS servers:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Suggest DNS A IPs:";
            // 
            // dnsServersTextBox
            // 
            this.dnsServersTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dnsServersTextBox.Location = new System.Drawing.Point(149, 73);
            this.dnsServersTextBox.Name = "dnsServersTextBox";
            this.dnsServersTextBox.Size = new System.Drawing.Size(311, 20);
            this.dnsServersTextBox.TabIndex = 2;
            this.dnsServersTextBox.Text = "ns1.rensoft.net.,ns2.rensoft.net.";
            // 
            // dnsIpsTextBox
            // 
            this.dnsIpsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dnsIpsTextBox.Location = new System.Drawing.Point(149, 99);
            this.dnsIpsTextBox.Name = "dnsIpsTextBox";
            this.dnsIpsTextBox.Size = new System.Drawing.Size(311, 20);
            this.dnsIpsTextBox.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dnsIpsTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.hostmasterTextBox);
            this.groupBox1.Controls.Add(this.primaryTextBox);
            this.groupBox1.Controls.Add(this.dnsServersTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(475, 132);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "DNS settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "SOA hostmaster:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "SOA primary host:";
            // 
            // hostmasterTextBox
            // 
            this.hostmasterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hostmasterTextBox.Location = new System.Drawing.Point(149, 47);
            this.hostmasterTextBox.Name = "hostmasterTextBox";
            this.hostmasterTextBox.Size = new System.Drawing.Size(162, 20);
            this.hostmasterTextBox.TabIndex = 1;
            this.hostmasterTextBox.Text = "hostmaster.rensoft.net.";
            // 
            // primaryTextBox
            // 
            this.primaryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.primaryTextBox.Location = new System.Drawing.Point(149, 21);
            this.primaryTextBox.Name = "primaryTextBox";
            this.primaryTextBox.Size = new System.Drawing.Size(162, 20);
            this.primaryTextBox.TabIndex = 0;
            this.primaryTextBox.Text = "ns1.rensoft.net.";
            // 
            // SettingsPage2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.groupBox1);
            this.InfoText = "Settings to be applied during the setup process.";
            this.Name = "SettingsPage2";
            this.Size = new System.Drawing.Size(481, 274);
            this.TitleText = "Settings (2 of 2)";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox dnsServersTextBox;
        private System.Windows.Forms.TextBox dnsIpsTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hostmasterTextBox;
        private System.Windows.Forms.TextBox primaryTextBox;
    }
}
