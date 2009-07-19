using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Hosting.DataAccess;
using System.ServiceModel;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.Setup.Properties;
using System.Windows.Forms;
using System.ServiceProcess;
using System.Threading;
using Rensoft.ServiceProcess;
using System.IO;

namespace Rensoft.Hosting.Setup.Options
{
    public class ServerSetupOption : MsiSetupOption
    {
        public bool EnableLegacyMode { get; set; }
        public string LegacyConnectionString { get; set; }
        public string WindowsServerName { get; set; }
        public string AvailableIisIPs { get; set; }
        public string EncryptionSecret { get; set; }
        public string DnsSoaPrimaryServer { get; set; }
        public string DnsSoaHostmaster { get; set; }
        public string DnsSuggestAIPs { get; set; }
        public string DnsSuggestServers { get; set; }
        public string WebsiteDirectory { get; set; }
        public bool EncryptorSecretChange { get; set; }

        public override string SetupFileName
        {
            get { return "HostingServerSetup.msi"; }
        }

        public override string ProductName
        {
            get { return "Rensoft Hosting Server 2008"; }
        }

        public override string MsiTitle
        {
            get { return "HostingServerSetup"; }
        }

        public ServerSetupOption(SetupWizardForm setupWizard)
            : base(setupWizard) { }

        public override bool ProcessOption(UpdateStatusDelegate updateStatus)
        {
            if (!base.ProcessOption(updateStatus))
            {
                return false;
            }

            updateStatus(this, SetupStatus.InProgress, "Starting service...");
            ServiceController sc = new ServiceController("Rensoft.Hosting.Server");
            bool started = sc.Start(TimeSpan.FromSeconds(10));

            if (!started)
            {
                updateStatus(this, SetupStatus.Failure, "Service start failed");
                return false;
            }

            updateStatus(this, SetupStatus.InProgress, "Configuring...");

            ServerConfigAdapter adapter = LocalContext.Default.CreateAdapter<ServerConfigAdapter>();
            adapter.EnableLegacyMode = EnableLegacyMode;
            adapter.LegacyConnectionString = LegacyConnectionString;
            adapter.WindowsServerName = WindowsServerName;
            adapter.AvailableIisSiteIPs = AvailableIisIPs.Split(',');
            adapter.AvailableDnsServers = DnsSuggestServers.Split(',');
            adapter.AvailableDnsIPs = DnsSuggestAIPs.Split(',');
            adapter.FirstDnsNameServer = DnsSoaPrimaryServer;
            adapter.DnsHostmasterName = DnsSoaHostmaster;
            adapter.WebsiteDirectory = new DirectoryInfo(WebsiteDirectory);

            if (EncryptorSecretChange)
            {
                // Only change if forced (could mean loosing passwords).
                adapter.EncryptorSecret = EncryptionSecret;
            }

            return true;
        }
    }
}
