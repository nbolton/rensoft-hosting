using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Win32;

namespace Rensoft.Hosting.Server.Managers.Config
{
    public partial class ServerConfigManager
    {
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

        public string WindowsServerName
        {
            get { return Get<string>("WindowsServerName"); }
            set { Set("WindowsServerName", value); }
        }

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

        public DirectoryInfo IscBindDirectory
        {
            get
            {
                return new DirectoryInfo(
                    (string)Registry.GetValue(
                        Get32BitSoftwareRegistryKey() + @"\ISC\BIND", "InstallDir", null));
            }
        }
        
        public DirectoryInfo FileZillaDirectory
        {
            get
            {
                return new DirectoryInfo(
                    (string)Registry.GetValue(
                        Get32BitSoftwareRegistryKey() + @"\FileZilla Server", "Install_Dir", null));
            }
        }

        public DirectoryInfo WebsiteDirectory
        {
            get { return new DirectoryInfo(Get<string>("WebsiteDirectory")); }
            set { Set("WebsiteDirectory", value.FullName); }
        }

        public DirectoryInfo LegacyWebsiteDirectory
        {
            get { return new DirectoryInfo(Get<string>("LegacyWebsiteDirectory", @"C:\inetpub\LocalUser")); }
            set { Set("LegacyWebsiteDirectory", value.FullName); }
        }

        public bool EnableLegacyMode
        {
            get { return Get<bool>("EnableLegacyMode", false); }
            set { Set("EnableLegacyMode", value); }
        }

        public string LegacyConnectionString
        {
            get { return Get<string>("LegacyConnectionString", string.Empty); }
            set { Set("LegacyConnectionString", value); }
        }
    }
}
