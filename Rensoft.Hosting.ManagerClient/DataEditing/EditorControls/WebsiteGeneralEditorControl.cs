using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Windows.Forms.DataEditing;
using Rensoft.Hosting.ManagerClient.DataEditing.EditorForms;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorControls
{
    public partial class WebsiteGeneralEditorControl : Rensoft.Hosting.ManagerClient.DataEditing.LocalDataEditorControl
    {
        private const string defaultIisManagedRuntimeVersion = "v2.0";

        private Customer selectedCustomer;

        public string OriginalPrimaryHostName { get; set; }

        public event EventHandler SelectedCustomerChanged;
        public event EventHandler IisSiteEnabledChanged;
        //public event EventHandler PrimaryHostChanged;

        new WebsiteEditorForm ParentForm
        {
            get { return (WebsiteEditorForm)base.ParentForm; }
        }

        public bool IisSiteEnabled
        {
            get { return iisSiteEnableCheckBox.Checked; }
        }

        public WebsiteGeneralEditorControl()
        {
            InitializeComponent();

            ReflectDataToForm += new DataEditorReflectEventHandler(WebsiteGeneralEditorControl_ReflectDataToForm);
            ReflectFormToData += new DataEditorReflectEventHandler(WebsiteGeneralEditorControl_ReflectFormToData);
            ValidateData += new CancelEventHandler(WebsiteGeneralEditorControl_ValidateData);

            websiteIisManagedPipelineModeInfoBindingSource.Clear();
            addIisManagedPipelineMode(WebsiteIisManagedPipelineMode.Integrated);
            addIisManagedPipelineMode(WebsiteIisManagedPipelineMode.Classic);
        }

        void WebsiteGeneralEditorControl_ValidateData(object sender, CancelEventArgs e)
        {
            if (getSelectedPrimaryHost() == null)
            {
                DialogResult result = MessageBox.Show(
                    "No primary host has been selected.",
                    "Primary host", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
                return;
            }

            if ((ParentForm.EditorMode == DataEditorMode.Update) 
                && (OriginalPrimaryHostName != getSelectedPrimaryHost().Name))
            {
                DialogResult result = MessageBox.Show(
                    "Changing the primary host from '" + OriginalPrimaryHostName + "' " +
                    "to '" + getSelectedPrimaryHost().Name + "' will change the physial website " +
                    "directory which may affect FTP users and website configurations.\r\n\r\n" +
                    "Are you sure you want to continue?",
                    "Primary host change",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void addIisManagedPipelineMode(WebsiteIisManagedPipelineMode mode)
        {
            websiteIisManagedPipelineModeInfoBindingSource.Add(
                new WebsiteIisManagedPipelineModeInfo(mode));
        }

        void WebsiteGeneralEditorControl_ReflectDataToForm(object sender, DataEditorReflectEventArgs e)
        {
            Website website = e.GetData<Website>();

            customerComboBox.DisplayMember = "CodeAndName";
            customerComboBox.DataSource = ParentForm.CustomerList;

            switch (website.IisSite.Mode)
            {
                case WebsiteIisMode.Disabled:
                    iisSiteEnableCheckBox.Checked = false;
                    break;

                case WebsiteIisMode.Standard:
                    iisSiteEnableCheckBox.Checked = true;
                    iisStandardRadioButton.Checked = true;
                    break;

                case WebsiteIisMode.Redirect:
                    iisSiteEnableCheckBox.Checked = true;
                    iisRedirectRadioButton.Checked = true;
                    break;
            }
            
            // Always assign whether redirect enabled or not.
            iisRedirectTextBox.Text = website.IisSite.RedirectUrl;

            // Remember the original host so we can detect change later.
            OriginalPrimaryHostName = website.PrimaryHost.Name;

            if (e.Mode == DataEditorMode.Update)
            {
                customerComboBox.Enabled = false;
                setSelectedCustomer(website.CustomerID);

                // Update primary hosts then set primary host.
                UpdatePrimaryHostCombobox(website.HostArray);
                setSelectedPrimaryHost(website.PrimaryHostID);
            }

            iisManagedPipelineModeComboBox.SelectedValue = website.IisSite.ManagedPipelineMode;
            setIisManagedRuntimeVersion(website.IisSite.ManagedRuntimeVersion);
        }

        private void setIisManagedRuntimeVersion(string version)
        {
            if (!string.IsNullOrEmpty(version))
            {

                iisManagedRuntimeVersionComboBox.Text = version;
            }
            else
            {
                iisManagedRuntimeVersionComboBox.Text = defaultIisManagedRuntimeVersion;
            }
        }

        void WebsiteGeneralEditorControl_ReflectFormToData(object sender, DataEditorReflectEventArgs e)
        {
            Website website = e.GetData<Website>();

            website.CustomerID = getSelectedCustomer().DataID;
            website.PrimaryHostID = getSelectedPrimaryHost().DataID;

            if (iisSiteEnableCheckBox.Checked)
            {
                if (iisStandardRadioButton.Checked)
                {
                    website.IisSite.Mode = WebsiteIisMode.Standard;
                }
                else if (iisRedirectRadioButton.Checked)
                {
                    website.IisSite.Mode = WebsiteIisMode.Redirect;
                    website.IisSite.RedirectUrl = iisRedirectTextBox.Text;
                }
            }
            else
            {
                website.IisSite.Mode = WebsiteIisMode.Disabled;
            }

            website.IisSite.ManagedPipelineMode = getSelectedIisManagedPipelineMode();
            website.IisSite.ManagedRuntimeVersion = iisManagedRuntimeVersionComboBox.Text;
        }

        private WebsiteIisManagedPipelineMode getSelectedIisManagedPipelineMode()
        {
            return (WebsiteIisManagedPipelineMode)iisManagedPipelineModeComboBox.SelectedValue;
        }

        private Customer getSelectedCustomer()
        {
            if (selectedCustomer == null)
            {
                throw new Exception("No customer has been selected.");
            }
            return selectedCustomer;
        }

        public void UpdatePrimaryHostCombobox(IEnumerable<WebsiteHost> hosts)
        {
            object selectedValue = primaryHostComboBox.SelectedItem;
            websiteHostBindingSource.Clear();
            hosts.ToList().ForEach(h => websiteHostBindingSource.Add(h));
            websiteHostBindingSource.ResetBindings(false);
            primaryHostComboBox.SelectedItem = selectedValue;
        }

        private void setSelectedPrimaryHost(RhspDataID websiteHostID)
        {
            if (websiteHostID != null)
            {
                List<WebsiteHost> list = primaryHostComboBox.Items.Cast<WebsiteHost>().ToList();
                primaryHostComboBox.SelectedItem = list.Find(c => c.DataID == websiteHostID);
            }
        }

        private void setSelectedCustomer(RhspDataID customerID)
        {
            if (customerID != null)
            {
                List<Customer> list = customerComboBox.Items.Cast<Customer>().ToList();
                customerComboBox.SelectedItem = list.Find(c => c.DataID.Equals(customerID));
            }
        }

        private void customerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCustomer = (Customer)customerComboBox.SelectedItem;
            if (SelectedCustomerChanged != null) SelectedCustomerChanged(this, EventArgs.Empty);
            ChangeMade();
        }

        private void iisSiteEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            iisStandardRadioButton.Enabled = iisSiteEnableCheckBox.Checked;
            iisRedirectRadioButton.Enabled = iisSiteEnableCheckBox.Checked;
            iisManagedPipelineModeComboBox.Enabled = iisSiteEnableCheckBox.Checked;
            iisManagedRuntimeVersionComboBox.Enabled = iisSiteEnableCheckBox.Checked;

            if (IisSiteEnabledChanged != null) IisSiteEnabledChanged(this, EventArgs.Empty);
            ChangeMade();
        }

        private void iisRedirectRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            refreshIisRedirectTextBoxEnabled();
            ChangeMade();
        }

        private void refreshIisRedirectTextBoxEnabled()
        {
            iisRedirectTextBox.Enabled = iisRedirectRadioButton.Checked && iisRedirectRadioButton.Enabled;
        }

        private void iisStandardRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            ChangeMade();
        }

        private void iisRedirectTextBox_TextChanged(object sender, EventArgs e)
        {
            ChangeMade();
        }

        private void iisRedirectRadioButton_EnabledChanged(object sender, EventArgs e)
        {
            refreshIisRedirectTextBoxEnabled();
        }

        private void WebsiteGeneralEditorControl_Load(object sender, EventArgs e)
        {

        }

        private void primaryHostComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (PrimaryHostChanged != null) PrimaryHostChanged(this, EventArgs.Empty);
        }

        private WebsiteHost getSelectedPrimaryHost()
        {
            return (WebsiteHost)primaryHostComboBox.SelectedItem;
        }
    }
}
