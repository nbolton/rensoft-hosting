using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Windows.Forms.DataAction;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Windows.Forms.DataEditing;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.DataEditing.EditorForms
{
    public partial class WebsiteEditorForm : Rensoft.Hosting.ManagerClient.DataEditing.LocalDataEditorForm
    {
        private List<Customer> customerList;

        public List<Customer> CustomerList
        {
            get { return customerList; }
        }

        public Website EditorData
        {
            get { return GetEditorData<Website>(); }
        }

        public bool IisSiteEnabled
        {
            get { return websiteGeneralEditorControl1.IisSiteEnabled; }
        }

        public WebsiteEditorForm()
        {
            InitializeComponent();

            this.customerList = new List<Customer>();

            AddEditorControl(websiteGeneralEditorControl1);
            AddEditorControl(websiteHostNameEditorControl1);
            AddEditorControl(websiteDnsZoneEditorControl1);
            AddEditorControl(websiteSecurityEditorControl1);

            Load += new EventHandler(WebsiteEditorForm_Load);
            LoadAsync += new DataActionEventHandler(WebsiteEditorForm_LoadAsync);
            CreateAsync += new DataActionEventHandler(WebsiteEditorForm_CreateAsync);
            UpdateAsync += new DataActionEventHandler(WebsiteEditorForm_UpdateAsync);
            AfterSaveAsync += new DataActionAfterEventHandler(WebsiteEditorForm_AfterSaveAsync);
            BeforeSaveAsync += new DataActionBeforeEventHandler(WebsiteEditorForm_BeforeSaveAsync);

            //websiteHostNameEditorControl1.CellEndEdit += new EventHandler(websiteHostNameEditorControl1_CellEndEdit);
            websiteGeneralEditorControl1.SelectedCustomerChanged += new EventHandler(websiteGeneralEditorControl1_SelectedCustomerChanged);
            websiteGeneralEditorControl1.IisSiteEnabledChanged += new EventHandler(websiteGeneralEditorControl1_IisSiteEnabledChanged);
            websiteHostNameEditorControl1.ReflectDataToForm += new DataEditorReflectEventHandler(websiteHostNameEditorControl1_ReflectDataToForm);
            websiteHostNameEditorControl1.DataChanged += new EventHandler(websiteHostNameEditorControl1_DataChanged);
        }

        void websiteHostNameEditorControl1_DataChanged(object sender, EventArgs e)
        {
            updatePrimaryHostCombobox();
        }

        void WebsiteEditorForm_Load(object sender, EventArgs e)
        {
        }

        void websiteHostNameEditorControl1_ReflectDataToForm(object sender, DataEditorReflectEventArgs e)
        {
            //updatePrimaryHostCombobox();
        }

        void WebsiteEditorForm_BeforeSaveAsync(object sender, DataActionBeforeEventArgs e)
        {
            // Ensures that all security rules apply to the IIS security model.
            GetEditorData<Website>().SecurityArray.ToList().ForEach(
                s => s.UseIisIdentity = true);
        }

        void websiteGeneralEditorControl1_IisSiteEnabledChanged(object sender, EventArgs e)
        {
            //websiteHostNameEditorControl1.Enabled = websiteGeneralEditorControl1.IisSiteEnabled;
            //websiteSecurityEditorControl1.Enabled = websiteGeneralEditorControl1.IisSiteEnabled;
        }

        void websiteGeneralEditorControl1_SelectedCustomerChanged(object sender, EventArgs e)
        {
            //websiteSecurityEditorControl1.Usernames = getUsersForSecurity();
        }

        //public string GetIisIdentityUserName()
        //{
        //    string customerCode = websiteGeneralEditorControl1.GetSelectedCustomer().Code;
        //    string primaryHostName = websiteGeneralEditorControl1.PrimaryHostName;
        //    return string.Format("{0} - {1}", customerCode, primaryHostName);
        //}

        //private IEnumerable<string> getUsersForSecurity()
        //{
        //    string customerCode = websiteGeneralEditorControl1.GetSelectedCustomer().Code;
        //    string primaryHostName = websiteGeneralEditorControl1.PrimaryHostName;
        //    return string.Format("{0} - {1}", customerCode, primaryHostName);

        //    //List<string> list = new List<string>();
        //    //list.Add(websiteGeneralEditorControl1.GetSelectedCustomer().Code + "-PublicWebsite");
        //    //list.Add(websiteGeneralEditorControl1.GetSelectedCustomer().Code + "-WorkerProcess");
        //    //return list;
        //}

        void websiteHostNameEditorControl1_CellEndEdit(object sender, EventArgs e)
        {
            //websiteSecurityEditorControl1.Usernames = getUsersForSecurity();
            //websiteGeneralEditorControl1.SetPrimaryHostText(
            //    websiteHostNameEditorControl1.GetPrimaryHostText());
        }

        private void updatePrimaryHostCombobox()
        {
            websiteGeneralEditorControl1.UpdatePrimaryHostCombobox(
                websiteHostNameEditorControl1.Hosts.Where(h => isValidPrimaryHost(h)));
        }

        private bool isValidPrimaryHost(WebsiteHost h)
        {
            bool b = (h.Port == WebsiteHost.DefaultHttpPort)
                && (h.Protocol == WebsiteHostProtocol.Http);
            return b;
        }

        void WebsiteEditorForm_AfterSaveAsync(object sender, DataActionAfterEventArgs e)
        {
            if (!e.Cancelled)
            {
                resetPendingActions(EditorData);

                // Update the original now save has been run in case re-saved.
                websiteGeneralEditorControl1.OriginalPrimaryHostName = 
                    GetEditorData<Website>().PrimaryHost.Name;
            }
        }

        private void resetPendingActions(Website website)
        {
            // Set a new arrays without all the deleted items.
            website.DnsZoneArray = website.DnsZoneArray.Where(z => z.PendingAction != ChildPendingAction.Delete).ToArray();
            website.SecurityArray = website.SecurityArray.Where(r => r.PendingAction != ChildPendingAction.Delete).ToArray();
            website.HostArray = website.HostArray.Where(h => !RhspData.IsDeleteOrDiscard(h)).ToArray();

            // Then, mark the pending actions to none so duplicates arent created.
            website.DnsZoneArray.ToList().ForEach(z => z.ResetPendingActions());
            website.SecurityArray.ToList().ForEach(r => r.PendingAction = ChildPendingAction.None);
            website.HostArray.ToList().ForEach(h => h.PendingAction = ChildPendingAction.None);

            // Refresh the editor controls with the removed data.
            websiteDnsZoneEditorControl1.RunReflectDataToForm(DataEditorMode.None);
            websiteSecurityEditorControl1.RunReflectDataToForm(DataEditorMode.None);
            websiteHostNameEditorControl1.RunReflectDataToForm(DataEditorMode.None);
        }

        void WebsiteEditorForm_LoadAsync(object sender, DataActionEventArgs e)
        {
            WebsiteAdapter wa = CreateAdapter<WebsiteAdapter>();

            if (EditorMode == DataEditorMode.Update)
            {
                // Replace stale data with fresh data.
                SetEditorData(wa.Get(GetEditorData<Website>().DataID));
            }

            customerList.Clear();
            customerList.AddRange(CreateAdapter<CustomerAdapter>().GetAll());

            websiteHostNameEditorControl1.IpAddressArray = wa.GetIpAddressArray();
        }

        void WebsiteEditorForm_CreateAsync(object sender, DataActionEventArgs e)
        {
            WebsiteAdapter wa = CreateAdapter<WebsiteAdapter>();

            if (wa.ExistsWithAnyHost(GetEditorData<Website>()))
            {
                e.UserMessage = "A website already exists with one or more conflicting host names.";
                e.Cancelled = true;
            }
            else if (!wa.SecurityValid(EditorData, websiteGeneralEditorControl1.OriginalPrimaryHostName))
            {
                e.UserMessage = "One or more security rules have either an invalid path or username.";
                e.Cancelled = true;
            }

            if (!e.Cancelled)
            {
                wa.Create(GetEditorData<Website>());
            }
        }

        void WebsiteEditorForm_UpdateAsync(object sender, DataActionEventArgs e)
        {
            WebsiteAdapter wa = CreateAdapter<WebsiteAdapter>();

            if (wa.ExistsWithAnyHost(GetEditorData<Website>()))
            {
                e.UserMessage = "An other website already exists with one or more conflicting host names.";
                e.Cancelled = true;
            }
            else if (!wa.SecurityValid(EditorData, websiteGeneralEditorControl1.OriginalPrimaryHostName))
            {
                e.UserMessage = "One or more security rules have either an invalid path or username.";
                e.Cancelled = true;
            }

            if (!e.Cancelled)
            {
                wa.Update(GetEditorData<Website>());
            }
        }
    }
}
