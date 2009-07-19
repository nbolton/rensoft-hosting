namespace Rensoft.Hosting.ManagerClient.WebsiteWizard
{
    partial class WelcomePage
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
            this.label1 = new System.Windows.Forms.Label();
            this.manualRadioButton = new System.Windows.Forms.RadioButton();
            this.wizardRadioButton = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(421, 41);
            this.label1.TabIndex = 4;
            this.label1.Text = "Do you want to use the wizard to create this website? The wizard automatically ge" +
                "nerates IIS host names, DNS zones and security permissions neccecary for a gener" +
                "ic website.";
            // 
            // manualRadioButton
            // 
            this.manualRadioButton.AutoSize = true;
            this.manualRadioButton.Location = new System.Drawing.Point(6, 74);
            this.manualRadioButton.Name = "manualRadioButton";
            this.manualRadioButton.Size = new System.Drawing.Size(157, 17);
            this.manualRadioButton.TabIndex = 1;
            this.manualRadioButton.Text = "Create the website manually";
            this.manualRadioButton.UseVisualStyleBackColor = true;
            this.manualRadioButton.CheckedChanged += new System.EventHandler(this.manualRadioButton_CheckedChanged);
            // 
            // wizardRadioButton
            // 
            this.wizardRadioButton.AutoSize = true;
            this.wizardRadioButton.Checked = true;
            this.wizardRadioButton.Location = new System.Drawing.Point(6, 51);
            this.wizardRadioButton.Name = "wizardRadioButton";
            this.wizardRadioButton.Size = new System.Drawing.Size(208, 17);
            this.wizardRadioButton.TabIndex = 0;
            this.wizardRadioButton.TabStop = true;
            this.wizardRadioButton.Text = "Use the website wizard (reccomended)";
            this.wizardRadioButton.UseVisualStyleBackColor = true;
            // 
            // WelcomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.manualRadioButton);
            this.Controls.Add(this.wizardRadioButton);
            this.InfoText = "Choose wizard or manual.";
            this.Name = "WelcomePage";
            this.TitleText = "Website Wizard";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton manualRadioButton;
        private System.Windows.Forms.RadioButton wizardRadioButton;
    }
}
