using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Hosting.DataAccess.Adapters;
using System.Linq;

namespace Rensoft.Hosting.ManagerClient.WebsiteWizard
{
    public partial class SettingsPage : Rensoft.Hosting.ManagerClient.WebsiteWizard.WebsiteWizardPage
    {
        private BindingSource iisIpBindingSource;
        private BindingSource dnsIpBindingSource;
        private string[] dnsServerArray;
        private string customDnsIp = string.Empty;

        public SettingsPage()
        {
            InitializeComponent();

            iisIpBindingSource = new BindingSource();
            dnsIpBindingSource = new BindingSource();
            dnsIpBindingSource.AllowNew = true;

            iisIpAddressComboBox.DataSource = iisIpBindingSource;
            dnsARecordIpCmboBox.DataSource = dnsIpBindingSource;

            LoadAsync += new DoWorkEventHandler(SettingsPage_LoadAsync);
            AfterLoadAsync += new RunWorkerCompletedEventHandler(SettingsPage_AfterLoadAsync);
            BeforeNextAsync += new DoWorkEventHandler(SettingsPage_BeforeNextAsync);
            AfterNextAsync += new RunWorkerCompletedEventHandler(SettingsPage_AfterNextAsync);
        }

        void SettingsPage_BeforeNextAsync(object sender, DoWorkEventArgs e)
        {
            if (string.IsNullOrEmpty(hostNameTextBox.Text))
            {
                e.Cancel = true;
                MessageBox.Show(
                    "A host name must be specified.",
                    "Host name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        void SettingsPage_AfterLoadAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            iisIpBindingSource.ResetBindings(false);
            dnsIpBindingSource.ResetBindings(false);
        }

        void SettingsPage_LoadAsync(object sender, DoWorkEventArgs e)
        {
            ServerConfigAdapter sca = LocalContext.Default.CreateAdapter<ServerConfigAdapter>();
            iisIpBindingSource.DataSource = sca.AvailableIisSiteIPs;
            dnsIpBindingSource.DataSource = (new string[] { customDnsIp }).Union(sca.AvailableDnsIPs).Where(ip => !string.IsNullOrEmpty(ip)).Distinct();
            dnsServerArray = sca.AvailableDnsServers;
        }

        void SettingsPage_AfterNextAsync(object sender, RunWorkerCompletedEventArgs e)
        {
            List<SecurityTemplate> stList = new List<SecurityTemplate>();
            List<WebsiteHost> hostList = new List<WebsiteHost>();
            List<DnsZone> dnsZoneList = new List<DnsZone>();
            string iisRedirectUrl = string.Empty;

            //Website.Name = hostNameTextBox.Text;

            if (iisEnableCheckBox.Checked)
            {
                if (iisStandardRadioButton.Checked)
                {
                    Website.IisSite.Mode = WebsiteIisMode.Standard;
                }

                if (iisRedirectRadioButton.Checked)
                {
                    Website.IisSite.Mode = WebsiteIisMode.Redirect;
                    iisRedirectUrl = iisRedirectTextBox.Text;
                }

                // Only create security template when there will be a directory to use.
                SecurityTemplate securityTemplate = new SecurityTemplate(RhspDataID.Generate());
                securityTemplate.PendingAction = ChildPendingAction.Create;
                securityTemplate.RelativePath = "\\";
                securityTemplate.Read = true;
                securityTemplate.Access = SecurityTemplateAccess.Allow;
                securityTemplate.UseIisIdentity = true;
                stList.Add(securityTemplate);
            }
            else
            {
                Website.IisSite.Mode = WebsiteIisMode.Disabled;
            }

            WebsiteHost primaryHost = new WebsiteHost(RhspDataID.Generate());
            primaryHost.PendingAction = ChildPendingAction.Create;
            primaryHost.Port = getIisPort();
            primaryHost.Name = hostNameTextBox.Text;
            primaryHost.IpAddress = (string)iisIpBindingSource.Current;
            //primaryHost.Primary = true;
            Website.PrimaryHostID = primaryHost.DataID;
            hostList.Add(primaryHost);

            if (wwwHostNameCheckBox.Checked)
            {
                WebsiteHost wwwHost = new WebsiteHost(RhspDataID.Generate());
                wwwHost.PendingAction = ChildPendingAction.Create;
                wwwHost.Port = getIisPort();
                wwwHost.Name = "www." + primaryHost.Name;
                wwwHost.IpAddress = primaryHost.IpAddress;
                hostList.Add(wwwHost);
            }

            if (dnsCreateRadioButton.Checked)
            {
                List<DnsRecord> recordList = new List<DnsRecord>();

                DnsZone zone = new DnsZone(RhspDataID.Generate());
                zone.PendingAction = ChildPendingAction.Create;
                zone.Name = hostNameTextBox.Text;
                zone.DefaultTtl = "1h";
                dnsZoneList.Add(zone);

                foreach (string dnsServer in dnsServerArray)
                {
                    DnsRecord nsRecord = new DnsRecord(RhspDataID.Generate());
                    nsRecord.PendingAction = ChildPendingAction.Create;
                    nsRecord.Name = "@";
                    nsRecord.RecordType = DnsRecordType.NS;
                    nsRecord.Value = dnsServer;
                    recordList.Add(nsRecord);
                }

                DnsRecord rootRecord = new DnsRecord(RhspDataID.Generate());
                rootRecord.PendingAction = ChildPendingAction.Create;
                rootRecord.Name = "@";
                rootRecord.RecordType = DnsRecordType.A;
                rootRecord.Value = (string)dnsIpBindingSource.Current;
                recordList.Add(rootRecord);

                if (wwwHostNameCheckBox.Checked)
                {
                    DnsRecord wwwRecord = new DnsRecord(RhspDataID.Generate());
                    wwwRecord.PendingAction = ChildPendingAction.Create;
                    wwwRecord.Name = "www";
                    wwwRecord.RecordType = DnsRecordType.CNAME;
                    wwwRecord.Value = "@";
                    recordList.Add(wwwRecord);
                }

                zone.RecordArray = recordList.ToArray();
            }

            Website.HostArray = hostList.ToArray();
            Website.SecurityArray = stList.ToArray();
            Website.DnsZoneArray = dnsZoneList.ToArray();
            Website.IisSite.RedirectUrl = iisRedirectUrl;
        }

        private int getIisPort()
        {
            return int.Parse(iisPortTextBox.Text);
        }

        private void dnsCreateRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            dnsARecordIpCmboBox.Enabled = dnsCreateRadioButton.Checked;
        }

        private void iisRedirectRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            iisRedirectTextBox.Enabled = iisRedirectRadioButton.Checked 
                && iisRedirectRadioButton.Enabled;
        }

        private void dnsARecordIpCmboBox_TextChanged(object sender, EventArgs e)
        {
            customDnsIp = dnsARecordIpCmboBox.Text;
        }

        private void iisEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            iisIpAddressComboBox.Enabled = iisEnableCheckBox.Checked;
            iisPortTextBox.Enabled = iisEnableCheckBox.Checked;
            iisStandardRadioButton.Enabled = iisEnableCheckBox.Checked;
            iisRedirectRadioButton.Enabled = iisEnableCheckBox.Checked;
            iisRedirectTextBox.Enabled = iisEnableCheckBox.Checked & iisRedirectRadioButton.Checked;
        }
    }
}
