namespace Rensoft.Hosting.Setup
{
    partial class ComponentPage
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
            this.flowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // flowLayoutPanel
            // 
            this.flowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel.Name = "flowLayoutPanel";
            this.flowLayoutPanel.Size = new System.Drawing.Size(427, 247);
            this.flowLayoutPanel.TabIndex = 1;
            // 
            // ComponentPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.flowLayoutPanel);
            this.InfoText = "Which components should be installed?";
            this.Name = "ComponentPage";
            this.TitleText = "Choose Components";
            this.LoadAsync += new System.ComponentModel.DoWorkEventHandler(this.ComponentPage_LoadAsync);
            this.AfterNextAsync += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ComponentPage_AfterNextAsync);
            this.AfterLoadAsync += new System.ComponentModel.RunWorkerCompletedEventHandler(this.ComponentPage_AfterLoadAsync);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel;

    }
}
