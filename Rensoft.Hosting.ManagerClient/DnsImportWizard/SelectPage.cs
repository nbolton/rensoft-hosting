using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.DnsImportWizard
{
    public partial class SelectPage : DnsImportWizardPage
    {
        private BindingList<DnsZone> bindingList;

        public SelectPage()
        {
            InitializeComponent();

            bindingList = new BindingList<DnsZone>();
            dataGridView.AutoGenerateColumns = false;
            dataGridView.DataSource = bindingList;

            LoadAsync += new DoWorkEventHandler(SelectPage_LoadAsync);
            AfterLoadAsync += new RunWorkerCompletedEventHandler(SelectPage_AfterLoadAsync);
            NextAsync += new DoWorkEventHandler(SelectPage_NextAsync);
        }

        void SelectPage_NextAsync(object sender, DoWorkEventArgs e)
        {
            SetImportZones(from zone in bindingList where zone.ImportSelected select zone);
        }

        void SelectPage_LoadAsync(object sender, DoWorkEventArgs e)
        {
            DnsZoneAdapter dzAdapter = GetSourceContext().CreateAdapter<DnsZoneAdapter>();
            WebsiteAdapter wAdapter = LocalContext.Default.CreateAdapter<WebsiteAdapter>();

            DnsZone[] zoneArray;
            switch (ParentWizard.ImportMode)
            {
                case DnsImportWizardMode.MicrosoftDnsZones:
                    zoneArray = dzAdapter.GetAllFromMsDns();
                    break;

                case DnsImportWizardMode.RensoftDnsZones:
                    zoneArray = dzAdapter.GetAll();
                    break;

                default:
                    throw new NotSupportedException();
            }

            // Select only zones which have a matching website into which it can be imported.
            e.Result = from zone in zoneArray
                       join website in wAdapter.GetAll() on zone.Name equals website.PrimaryHost.Name
                       select combineZoneWithWebsite(zone, website);
        }

        private DnsZone combineZoneWithWebsite(DnsZone zone, Website website)
        {
            zone.WebsiteID = website.DataID;
            return zone;
        }

        void SelectPage_AfterLoadAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            bindingList.Clear();
            foreach (DnsZone zone in (IEnumerable<DnsZone>)e.Result)
            {
                zone.ImportSelected = true;
                bindingList.Add(zone);
                zone.RecordArray.ToList().ForEach(r => r.PendingAction = ChildPendingAction.Create);
            }
            bindingList.ResetBindings();
        }
    }
}
