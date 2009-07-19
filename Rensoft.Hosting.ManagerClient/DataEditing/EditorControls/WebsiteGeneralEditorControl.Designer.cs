namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorControls
{
    partial class WebsiteGeneralEditorControl
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
            this.customerComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.iisSiteEnableCheckBox = new System.Windows.Forms.CheckBox();
            this.iisRedirectRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.iisManagedPipelineModeComboBox = new System.Windows.Forms.ComboBox();
            this.websiteIisManagedPipelineModeInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label4 = new System.Windows.Forms.Label();
            this.iisManagedRuntimeVersionComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.iisStandardRadioButton = new System.Windows.Forms.RadioButton();
            this.iisRedirectTextBox = new System.Windows.Forms.TextBox();
            this.primaryHostComboBox = new System.Windows.Forms.ComboBox();
            this.websiteHostBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.websiteIisManagedPipelineModeInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.websiteHostBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // customerComboBox
            // 
            this.customerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customerComboBox.FormattingEnabled = true;
            this.customerComboBox.Location = new System.Drawing.Point(100, 3);
            this.customerComboBox.Name = "customerComboBox";
            this.customerComboBox.Size = new System.Drawing.Size(232, 21);
            this.customerComboBox.TabIndex = 0;
            this.customerComboBox.SelectedIndexChanged += new System.EventHandler(this.customerComboBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Customer:";
            // 
            // iisSiteEnableCheckBox
            // 
            this.iisSiteEnableCheckBox.AutoSize = true;
            this.iisSiteEnableCheckBox.Location = new System.Drawing.Point(8, 21);
            this.iisSiteEnableCheckBox.Name = "iisSiteEnableCheckBox";
            this.iisSiteEnableCheckBox.Size = new System.Drawing.Size(140, 17);
            this.iisSiteEnableCheckBox.TabIndex = 0;
            this.iisSiteEnableCheckBox.Text = "Enable Microsoft IIS site";
            this.iisSiteEnableCheckBox.UseVisualStyleBackColor = true;
            this.iisSiteEnableCheckBox.CheckedChanged += new System.EventHandler(this.iisSiteEnableCheckBox_CheckedChanged);
            // 
            // iisRedirectRadioButton
            // 
            this.iisRedirectRadioButton.AutoSize = true;
            this.iisRedirectRadioButton.Enabled = false;
            this.iisRedirectRadioButton.Location = new System.Drawing.Point(8, 68);
            this.iisRedirectRadioButton.Name = "iisRedirectRadioButton";
            this.iisRedirectRadioButton.Size = new System.Drawing.Size(119, 17);
            this.iisRedirectRadioButton.TabIndex = 3;
            this.iisRedirectRadioButton.Text = "Redirection to URL:";
            this.iisRedirectRadioButton.UseVisualStyleBackColor = true;
            this.iisRedirectRadioButton.CheckedChanged += new System.EventHandler(this.iisRedirectRadioButton_CheckedChanged);
            this.iisRedirectRadioButton.EnabledChanged += new System.EventHandler(this.iisRedirectRadioButton_EnabledChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.iisManagedPipelineModeComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.iisManagedRuntimeVersionComboBox);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.iisRedirectRadioButton);
            this.groupBox1.Controls.Add(this.iisSiteEnableCheckBox);
            this.groupBox1.Controls.Add(this.iisStandardRadioButton);
            this.groupBox1.Controls.Add(this.iisRedirectTextBox);
            this.groupBox1.Location = new System.Drawing.Point(3, 60);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(5);
            this.groupBox1.Size = new System.Drawing.Size(514, 182);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Website options";
            // 
            // iisManagedPipelineModeComboBox
            // 
            this.iisManagedPipelineModeComboBox.DataSource = this.websiteIisManagedPipelineModeInfoBindingSource;
            this.iisManagedPipelineModeComboBox.DisplayMember = "Text";
            this.iisManagedPipelineModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.iisManagedPipelineModeComboBox.Enabled = false;
            this.iisManagedPipelineModeComboBox.FormattingEnabled = true;
            this.iisManagedPipelineModeComboBox.Location = new System.Drawing.Point(140, 148);
            this.iisManagedPipelineModeComboBox.Name = "iisManagedPipelineModeComboBox";
            this.iisManagedPipelineModeComboBox.Size = new System.Drawing.Size(96, 21);
            this.iisManagedPipelineModeComboBox.TabIndex = 5;
            this.iisManagedPipelineModeComboBox.ValueMember = "Mode";
            // 
            // websiteIisManagedPipelineModeInfoBindingSource
            // 
            this.websiteIisManagedPipelineModeInfoBindingSource.DataSource = typeof(Rensoft.Hosting.ManagerClient.DataEditing.WebsiteIisManagedPipelineModeInfo);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(123, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Managed pipeline mode:";
            // 
            // iisManagedRuntimeVersionComboBox
            // 
            this.iisManagedRuntimeVersionComboBox.Enabled = false;
            this.iisManagedRuntimeVersionComboBox.FormattingEnabled = true;
            this.iisManagedRuntimeVersionComboBox.Items.AddRange(new object[] {
            "v1.1",
            "v2.0"});
            this.iisManagedRuntimeVersionComboBox.Location = new System.Drawing.Point(140, 121);
            this.iisManagedRuntimeVersionComboBox.Name = "iisManagedRuntimeVersionComboBox";
            this.iisManagedRuntimeVersionComboBox.Size = new System.Drawing.Size(96, 21);
            this.iisManagedRuntimeVersionComboBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Managed runtime version:";
            // 
            // iisStandardRadioButton
            // 
            this.iisStandardRadioButton.AutoSize = true;
            this.iisStandardRadioButton.Checked = true;
            this.iisStandardRadioButton.Enabled = false;
            this.iisStandardRadioButton.Location = new System.Drawing.Point(8, 44);
            this.iisStandardRadioButton.Name = "iisStandardRadioButton";
            this.iisStandardRadioButton.Size = new System.Drawing.Size(107, 17);
            this.iisStandardRadioButton.TabIndex = 2;
            this.iisStandardRadioButton.TabStop = true;
            this.iisStandardRadioButton.Text = "Standard website";
            this.iisStandardRadioButton.UseVisualStyleBackColor = true;
            this.iisStandardRadioButton.CheckedChanged += new System.EventHandler(this.iisStandardRadioButton_CheckedChanged);
            // 
            // iisRedirectTextBox
            // 
            this.iisRedirectTextBox.Enabled = false;
            this.iisRedirectTextBox.Location = new System.Drawing.Point(8, 91);
            this.iisRedirectTextBox.Name = "iisRedirectTextBox";
            this.iisRedirectTextBox.Size = new System.Drawing.Size(321, 20);
            this.iisRedirectTextBox.TabIndex = 3;
            this.iisRedirectTextBox.TextChanged += new System.EventHandler(this.iisRedirectTextBox_TextChanged);
            // 
            // primaryHostComboBox
            // 
            this.primaryHostComboBox.DataSource = this.websiteHostBindingSource;
            this.primaryHostComboBox.DisplayMember = "Name";
            this.primaryHostComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.primaryHostComboBox.FormattingEnabled = true;
            this.primaryHostComboBox.Location = new System.Drawing.Point(100, 30);
            this.primaryHostComboBox.Name = "primaryHostComboBox";
            this.primaryHostComboBox.Size = new System.Drawing.Size(232, 21);
            this.primaryHostComboBox.TabIndex = 6;
            this.primaryHostComboBox.SelectedIndexChanged += new System.EventHandler(this.primaryHostComboBox_SelectedIndexChanged);
            // 
            // websiteHostBindingSource
            // 
            this.websiteHostBindingSource.DataSource = typeof(Rensoft.Hosting.DataAccess.ServiceReference.WebsiteHost);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Primary host:";
            // 
            // WebsiteGeneralEditorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.primaryHostComboBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.customerComboBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "WebsiteGeneralEditorControl";
            this.Load += new System.EventHandler(this.WebsiteGeneralEditorControl_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.websiteIisManagedPipelineModeInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.websiteHostBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox customerComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox iisSiteEnableCheckBox;
        private System.Windows.Forms.RadioButton iisRedirectRadioButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton iisStandardRadioButton;
        private System.Windows.Forms.TextBox iisRedirectTextBox;
        private System.Windows.Forms.ComboBox iisManagedPipelineModeComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.BindingSource websiteIisManagedPipelineModeInfoBindingSource;
        private System.Windows.Forms.ComboBox iisManagedRuntimeVersionComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox primaryHostComboBox;
        private System.Windows.Forms.BindingSource websiteHostBindingSource;


    }
}
