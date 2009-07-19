namespace Rensoft.Hosting.ManagerClient.WebsiteWizard
{
    partial class SettingsPage
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
            this.hostNameTextBox = new System.Windows.Forms.TextBox();
            this.wwwHostNameCheckBox = new System.Windows.Forms.CheckBox();
            this.websiteOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.iisPortTextBox = new System.Windows.Forms.TextBox();
            this.iisIpAddressComboBox = new System.Windows.Forms.ComboBox();
            this.iisRedirectRadioButton = new System.Windows.Forms.RadioButton();
            this.iisStandardRadioButton = new System.Windows.Forms.RadioButton();
            this.iisRedirectTextBox = new System.Windows.Forms.TextBox();
            this.iisEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dnsOptionsGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dnsARecordIpCmboBox = new System.Windows.Forms.ComboBox();
            this.dnsIgnoreRadioButton = new System.Windows.Forms.RadioButton();
            this.dnsCreateRadioButton = new System.Windows.Forms.RadioButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.websiteOptionsGroupBox.SuspendLayout();
            this.dnsOptionsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // hostNameTextBox
            // 
            this.hostNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.hostNameTextBox.Location = new System.Drawing.Point(108, 7);
            this.hostNameTextBox.Name = "hostNameTextBox";
            this.hostNameTextBox.Size = new System.Drawing.Size(248, 20);
            this.hostNameTextBox.TabIndex = 0;
            this.toolTip.SetToolTip(this.hostNameTextBox, "Host name (e.g. contoso.com)");
            // 
            // wwwHostNameCheckBox
            // 
            this.wwwHostNameCheckBox.AutoSize = true;
            this.wwwHostNameCheckBox.Checked = true;
            this.wwwHostNameCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.wwwHostNameCheckBox.Location = new System.Drawing.Point(108, 33);
            this.wwwHostNameCheckBox.Name = "wwwHostNameCheckBox";
            this.wwwHostNameCheckBox.Size = new System.Drawing.Size(248, 17);
            this.wwwHostNameCheckBox.TabIndex = 1;
            this.wwwHostNameCheckBox.Text = "Also create a \'www\' host name where available";
            this.wwwHostNameCheckBox.UseVisualStyleBackColor = true;
            // 
            // websiteOptionsGroupBox
            // 
            this.websiteOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.websiteOptionsGroupBox.Controls.Add(this.label3);
            this.websiteOptionsGroupBox.Controls.Add(this.iisPortTextBox);
            this.websiteOptionsGroupBox.Controls.Add(this.iisIpAddressComboBox);
            this.websiteOptionsGroupBox.Controls.Add(this.iisRedirectRadioButton);
            this.websiteOptionsGroupBox.Controls.Add(this.iisStandardRadioButton);
            this.websiteOptionsGroupBox.Controls.Add(this.iisRedirectTextBox);
            this.websiteOptionsGroupBox.Controls.Add(this.iisEnableCheckBox);
            this.websiteOptionsGroupBox.Location = new System.Drawing.Point(4, 56);
            this.websiteOptionsGroupBox.Name = "websiteOptionsGroupBox";
            this.websiteOptionsGroupBox.Padding = new System.Windows.Forms.Padding(5);
            this.websiteOptionsGroupBox.Size = new System.Drawing.Size(432, 132);
            this.websiteOptionsGroupBox.TabIndex = 2;
            this.websiteOptionsGroupBox.TabStop = false;
            this.websiteOptionsGroupBox.Text = "Website options";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "IP address and port:";
            // 
            // iisPortTextBox
            // 
            this.iisPortTextBox.Location = new System.Drawing.Point(283, 47);
            this.iisPortTextBox.Name = "iisPortTextBox";
            this.iisPortTextBox.Size = new System.Drawing.Size(42, 20);
            this.iisPortTextBox.TabIndex = 2;
            this.iisPortTextBox.Text = "80";
            this.toolTip.SetToolTip(this.iisPortTextBox, "Port number (normally 80)");
            // 
            // iisIpAddressComboBox
            // 
            this.iisIpAddressComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.iisIpAddressComboBox.FormattingEnabled = true;
            this.iisIpAddressComboBox.Location = new System.Drawing.Point(139, 46);
            this.iisIpAddressComboBox.Name = "iisIpAddressComboBox";
            this.iisIpAddressComboBox.Size = new System.Drawing.Size(138, 21);
            this.iisIpAddressComboBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.iisIpAddressComboBox, "Available IP addresses.");
            // 
            // iisRedirectRadioButton
            // 
            this.iisRedirectRadioButton.AutoSize = true;
            this.iisRedirectRadioButton.Location = new System.Drawing.Point(8, 99);
            this.iisRedirectRadioButton.Name = "iisRedirectRadioButton";
            this.iisRedirectRadioButton.Size = new System.Drawing.Size(119, 17);
            this.iisRedirectRadioButton.TabIndex = 4;
            this.iisRedirectRadioButton.Text = "Redirection to URL:";
            this.iisRedirectRadioButton.UseVisualStyleBackColor = true;
            this.iisRedirectRadioButton.CheckedChanged += new System.EventHandler(this.iisRedirectRadioButton_CheckedChanged);
            // 
            // iisStandardRadioButton
            // 
            this.iisStandardRadioButton.AutoSize = true;
            this.iisStandardRadioButton.Checked = true;
            this.iisStandardRadioButton.Location = new System.Drawing.Point(8, 75);
            this.iisStandardRadioButton.Name = "iisStandardRadioButton";
            this.iisStandardRadioButton.Size = new System.Drawing.Size(107, 17);
            this.iisStandardRadioButton.TabIndex = 3;
            this.iisStandardRadioButton.TabStop = true;
            this.iisStandardRadioButton.Text = "Standard website";
            this.iisStandardRadioButton.UseVisualStyleBackColor = true;
            // 
            // iisRedirectTextBox
            // 
            this.iisRedirectTextBox.Enabled = false;
            this.iisRedirectTextBox.Location = new System.Drawing.Point(139, 97);
            this.iisRedirectTextBox.Name = "iisRedirectTextBox";
            this.iisRedirectTextBox.Size = new System.Drawing.Size(186, 20);
            this.iisRedirectTextBox.TabIndex = 5;
            // 
            // iisEnableCheckBox
            // 
            this.iisEnableCheckBox.AutoSize = true;
            this.iisEnableCheckBox.Checked = true;
            this.iisEnableCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.iisEnableCheckBox.Location = new System.Drawing.Point(8, 23);
            this.iisEnableCheckBox.Name = "iisEnableCheckBox";
            this.iisEnableCheckBox.Size = new System.Drawing.Size(138, 17);
            this.iisEnableCheckBox.TabIndex = 0;
            this.iisEnableCheckBox.Text = "Create Microsoft IIS site";
            this.iisEnableCheckBox.UseVisualStyleBackColor = true;
            this.iisEnableCheckBox.CheckedChanged += new System.EventHandler(this.iisEnableCheckBox_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Primary host name:";
            // 
            // dnsOptionsGroupBox
            // 
            this.dnsOptionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dnsOptionsGroupBox.Controls.Add(this.label1);
            this.dnsOptionsGroupBox.Controls.Add(this.dnsARecordIpCmboBox);
            this.dnsOptionsGroupBox.Controls.Add(this.dnsIgnoreRadioButton);
            this.dnsOptionsGroupBox.Controls.Add(this.dnsCreateRadioButton);
            this.dnsOptionsGroupBox.Location = new System.Drawing.Point(4, 194);
            this.dnsOptionsGroupBox.Name = "dnsOptionsGroupBox";
            this.dnsOptionsGroupBox.Padding = new System.Windows.Forms.Padding(5);
            this.dnsOptionsGroupBox.Size = new System.Drawing.Size(432, 104);
            this.dnsOptionsGroupBox.TabIndex = 3;
            this.dnsOptionsGroupBox.TabStop = false;
            this.dnsOptionsGroupBox.Text = "DNS options";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Zone A record IP:";
            // 
            // dnsARecordIpCmboBox
            // 
            this.dnsARecordIpCmboBox.FormattingEnabled = true;
            this.dnsARecordIpCmboBox.Location = new System.Drawing.Point(139, 44);
            this.dnsARecordIpCmboBox.Name = "dnsARecordIpCmboBox";
            this.dnsARecordIpCmboBox.Size = new System.Drawing.Size(138, 21);
            this.dnsARecordIpCmboBox.TabIndex = 1;
            this.toolTip.SetToolTip(this.dnsARecordIpCmboBox, "Available IP addresses.");
            this.dnsARecordIpCmboBox.TextChanged += new System.EventHandler(this.dnsARecordIpCmboBox_TextChanged);
            // 
            // dnsIgnoreRadioButton
            // 
            this.dnsIgnoreRadioButton.AutoSize = true;
            this.dnsIgnoreRadioButton.Location = new System.Drawing.Point(8, 70);
            this.dnsIgnoreRadioButton.Name = "dnsIgnoreRadioButton";
            this.dnsIgnoreRadioButton.Size = new System.Drawing.Size(264, 17);
            this.dnsIgnoreRadioButton.TabIndex = 2;
            this.dnsIgnoreRadioButton.Text = "Use external DNS server (no DNS zone is created)";
            this.dnsIgnoreRadioButton.UseVisualStyleBackColor = true;
            // 
            // dnsCreateRadioButton
            // 
            this.dnsCreateRadioButton.AutoSize = true;
            this.dnsCreateRadioButton.Checked = true;
            this.dnsCreateRadioButton.Location = new System.Drawing.Point(8, 21);
            this.dnsCreateRadioButton.Name = "dnsCreateRadioButton";
            this.dnsCreateRadioButton.Size = new System.Drawing.Size(296, 17);
            this.dnsCreateRadioButton.TabIndex = 0;
            this.dnsCreateRadioButton.TabStop = true;
            this.dnsCreateRadioButton.Text = "Use the DNS server on this service (creates a DNS zone)";
            this.dnsCreateRadioButton.UseVisualStyleBackColor = true;
            this.dnsCreateRadioButton.CheckedChanged += new System.EventHandler(this.dnsCreateRadioButton_CheckedChanged);
            // 
            // SettingsPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.websiteOptionsGroupBox);
            this.Controls.Add(this.dnsOptionsGroupBox);
            this.Controls.Add(this.hostNameTextBox);
            this.Controls.Add(this.wwwHostNameCheckBox);
            this.InfoText = "Specify the website and DNS options.";
            this.Name = "SettingsPage";
            this.Size = new System.Drawing.Size(439, 301);
            this.TitleText = "Website settings";
            this.websiteOptionsGroupBox.ResumeLayout(false);
            this.websiteOptionsGroupBox.PerformLayout();
            this.dnsOptionsGroupBox.ResumeLayout(false);
            this.dnsOptionsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox hostNameTextBox;
        private System.Windows.Forms.CheckBox wwwHostNameCheckBox;
        private System.Windows.Forms.GroupBox websiteOptionsGroupBox;
        private System.Windows.Forms.GroupBox dnsOptionsGroupBox;
        private System.Windows.Forms.RadioButton iisRedirectRadioButton;
        private System.Windows.Forms.RadioButton iisStandardRadioButton;
        private System.Windows.Forms.TextBox iisRedirectTextBox;
        private System.Windows.Forms.RadioButton dnsIgnoreRadioButton;
        private System.Windows.Forms.RadioButton dnsCreateRadioButton;
        private System.Windows.Forms.TextBox iisPortTextBox;
        private System.Windows.Forms.ComboBox iisIpAddressComboBox;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox dnsARecordIpCmboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox iisEnableCheckBox;
        private System.Windows.Forms.Label label3;
    }
}
