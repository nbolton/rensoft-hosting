using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    public partial class ServerConfigAdapter
    {
        public string DnsHostmasterName
        {
            get { return Get<string>("DnsHostmasterName"); }
            set { Set("DnsHostmasterName", value); }
        }

        public string FirstDnsNameServer
        {
            get { return Get<string>("FirstDnsNameServer"); }
            set { Set("FirstDnsNameServer", value); }
        }

        public string EncryptorSecret
        {
            get { return Get<string>("EncryptorSecret"); }
            set { Set("EncryptorSecret", value); }
        }

        public string[] AvailableDnsIPs
        {
            get { return Get<string>("AvailableDnsIPs").Split(','); }
            set { Set("AvailableDnsIPs", string.Join(",", value)); }
        }

        public string[] AvailableDnsServers
        {
            get { return Get<string>("AvailableDnsServers").Split(','); }
            set { Set("AvailableDnsServers", string.Join(",", value)); }
        }

        public string[] AvailableIisSiteIPs
        {
            get { return Get<string>("AvailableIisSiteIPs").Split(','); }
            set { Set("AvailableIisSiteIPs", string.Join(",", value)); }
        }

        public bool EnableLegacyMode
        {
            get { return Get<bool>("EnableLegacyMode"); }
            set { Set("EnableLegacyMode", value); }
        }

        public string LegacyConnectionString
        {
            get { return Get<string>("LegacyConnectionString"); }
            set { Set("LegacyConnectionString", value); }
        }

        public string WindowsServerName
        {
            get { return Get<string>("WindowsServerName"); }
            set { Set("WindowsServerName", value); }
        }

        public DirectoryInfo WebsiteDirectory
        {
            get { return new DirectoryInfo(Get<string>("WebsiteDirectory")); }
            set { Set("WebsiteDirectory", value.FullName); }
        }
    }
}
