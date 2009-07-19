namespace Rensoft.Hosting.Setup
{
    partial class SettingsPage1
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
            this.legacyCheckBox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.legacyConnectionStringTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.encryptorSecretChangeCheckBox = new System.Windows.Forms.CheckBox();
            this.websiteDirectoryButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.websiteDirectoryTextBox = new System.Windows.Forms.TextBox();
            this.encryptorSecretTextBox = new System.Windows.Forms.TextBox();
            this.iisIpsTextBox = new System.Windows.Forms.TextBox();
            this.wsnTextBox = new System.Windows.Forms.TextBox();
            this.websiteDirectoryFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // legacyCheckBox
            // 
            this.legacyCheckBox.AutoSize = true;
            this.legacyCheckBox.Location = new System.Drawing.Point(12, 22);
            this.legacyCheckBox.Name = "legacyCheckBox";
            this.legacyCheckBox.Size = new System.Drawing.Size(122, 17);
            this.legacyCheckBox.TabIndex = 0;
            this.legacyCheckBox.Text = "Enable legacy mode";
            this.legacyCheckBox.UseVisualStyleBackColor = true;
            this.legacyCheckBox.CheckedChanged += new System.EventHandler(this.legacyCheckBox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Legacy SQL database connection string:";
            // 
            // legacyConnectionStringTextBox
            // 
            this.legacyConnectionStringTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.legacyConnectionStringTextBox.Enabled = false;
            this.legacyConnectionStringTextBox.Location = new System.Drawing.Point(37, 67);
            this.legacyConnectionStringTextBox.Name = "legacyConnectionStringTextBox";
            this.legacyConnectionStringTextBox.Size = new System.Drawing.Size(406, 20);
            this.legacyConnectionStringTextBox.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.legacyCheckBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.legacyConnectionStringTextBox);
            this.groupBox2.Location = new System.Drawing.Point(6, 145);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(456, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Legacy mode";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.encryptorSecretChangeCheckBox);
            this.groupBox1.Controls.Add(this.websiteDirectoryButton);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.websiteDirectoryTextBox);
            this.groupBox1.Controls.Add(this.encryptorSecretTextBox);
            this.groupBox1.Controls.Add(this.iisIpsTextBox);
            this.groupBox1.Controls.Add(this.wsnTextBox);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(456, 136);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server settings";
            // 
            // encryptorSecretChangeCheckBox
            // 
            this.encryptorSecretChangeCheckBox.AutoSize = true;
            this.encryptorSecretChangeCheckBox.Location = new System.Drawing.Point(146, 78);
            this.encryptorSecretChangeCheckBox.Name = "encryptorSecretChangeCheckBox";
            this.encryptorSecretChangeCheckBox.Size = new System.Drawing.Size(15, 14);
            this.encryptorSecretChangeCheckBox.TabIndex = 15;
            this.encryptorSecretChangeCheckBox.UseVisualStyleBackColor = true;
            this.encryptorSecretChangeCheckBox.CheckedChanged += new System.EventHandler(this.encryptorSecretChangeCheckBox_CheckedChanged);
            // 
            // websiteDirectoryButton
            // 
            this.websiteDirectoryButton.Location = new System.Drawing.Point(368, 99);
            this.websiteDirectoryButton.Name = "websiteDirectoryButton";
            this.websiteDirectoryButton.Size = new System.Drawing.Size(75, 23);
            this.websiteDirectoryButton.TabIndex = 2;
            this.websiteDirectoryButton.Text = "Browse...";
            this.websiteDirectoryButton.UseVisualStyleBackColor = true;
            this.websiteDirectoryButton.Click += new System.EventHandler(this.websiteDirectoryButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Websites directory:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(92, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Encryption secret:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Available IIS site IPs:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Windows server name:";
            // 
            // websiteDirectoryTextBox
            // 
            this.websiteDirectoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.websiteDirectoryTextBox.Location = new System.Drawing.Point(146, 101);
            this.websiteDirectoryTextBox.Name = "websiteDirectoryTextBox";
            this.websiteDirectoryTextBox.Size = new System.Drawing.Size(216, 20);
            this.websiteDirectoryTextBox.TabIndex = 2;
            this.websiteDirectoryTextBox.Text = "C:\\Websites";
            // 
            // encryptorSecretTextBox
            // 
            this.encryptorSecretTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.encryptorSecretTextBox.Enabled = false;
            this.encryptorSecretTextBox.Location = new System.Drawing.Point(164, 75);
            this.encryptorSecretTextBox.Name = "encryptorSecretTextBox";
            this.encryptorSecretTextBox.Size = new System.Drawing.Size(279, 20);
            this.encryptorSecretTextBox.TabIndex = 2;
            this.encryptorSecretTextBox.EnabledChanged += new System.EventHandler(this.encryptorSecretTextBox_EnabledChanged);
            // 
            // iisIpsTextBox
            // 
            this.iisIpsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.iisIpsTextBox.Location = new System.Drawing.Point(146, 49);
            this.iisIpsTextBox.Name = "iisIpsTextBox";
            this.iisIpsTextBox.Size = new System.Drawing.Size(297, 20);
            this.iisIpsTextBox.TabIndex = 1;
            // 
            // wsnTextBox
            // 
            this.wsnTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.wsnTextBox.Location = new System.Drawing.Point(146, 23);
            this.wsnTextBox.Name = "wsnTextBox";
            this.wsnTextBox.Size = new System.Drawing.Size(297, 20);
            this.wsnTextBox.TabIndex = 0;
            // 
            // SettingsPage1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.InfoText = "Settings to be applied during the setup process.";
            this.Name = "SettingsPage1";
            this.Size = new System.Drawing.Size(462, 322);
            this.TitleText = "Settings (1 of 2)";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox legacyCheckBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox legacyConnectionStringTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox iisIpsTextBox;
        private System.Windows.Forms.TextBox wsnTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox encryptorSecretTextBox;
        private System.Windows.Forms.Button websiteDirectoryButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox websiteDirectoryTextBox;
        private System.Windows.Forms.FolderBrowserDialog websiteDirectoryFolderBrowserDialog;
        private System.Windows.Forms.CheckBox encryptorSecretChangeCheckBox;
    }
}
