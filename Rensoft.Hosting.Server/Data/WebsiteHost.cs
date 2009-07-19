using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Net;
using System.Xml.Linq;
using Rensoft.Hosting.Server.Managers.Config;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    [RhspData("WebsiteHost", "WebsiteHosts", "WebsiteHostID", SchemaVersion=2)]
    public class WebsiteHost : RhspData, IRhspData, IRhspDataChild
    {
        public const int DefaultHttpPort = 80;
        public const int DefaultHttpsPort = 443;

        public RhspDataID ParentID
        {
            get { return WebsiteID; }
            set { WebsiteID = value; }
        }

        public IRhspDataParent Parent { get; set; }

        [DataMember]
        public ChildPendingAction PendingAction { get; set; }

        [DataMember]
        public RhspDataID WebsiteID { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public string IpAddress { get; set; }

        public bool Primary { get; set; }

        [DataMember]
        public WebsiteHostProtocol Protocol { get; set; }

        public string GetBindingInformation()
        {
            return string.Format("{0}:{1}:{2}", IpAddress, Port, Name);
        }

        public XElement ToXElement(HostingConfigManager manager)
        {
            return new XElement(
               "WebsiteHost",
               new XElement("WebsiteID", WebsiteID),
               new XElement("Name", Name),
               new XElement("Port", Port),
               new XElement("IpAddress", IpAddress),
               new XElement("Protocol", Protocol)
           );
        }

        public void FromElement(XElement element, HostingConfigManager manager)
        {
            WebsiteID = element.GetElementValue<RhspDataID>("WebsiteID", true);
            Name = element.GetElementValue<string>("Name", true);
            Port = element.GetElementValue<int>("Port", true);
            IpAddress = element.GetElementValue<string>("IpAddress", true);
            Protocol = element.GetElementValue<WebsiteHostProtocol>("Protocol", true);

            if (manager.GetDataSchemaVersion<WebsiteHost>(DataID) == FirstSchemaVersion)
            {
                Primary = element.GetElementValue<bool>("Primary", true);
            }
        }

        public void UpdateElement(XElement element, HostingConfigManager manager)
        {
            element.SetElementValue("WebsiteID", WebsiteID);
            element.SetElementValue("Name", Name);
            element.SetElementValue("Port", Port);
            element.SetElementValue("IpAddress", IpAddress);
            element.SetElementValue("Protocol", Protocol);
        }

        public string GetIisProtocol()
        {
            switch (Protocol)
            {
                case WebsiteHostProtocol.Http: return "http";
                case WebsiteHostProtocol.Https: return "https";
                default: throw new NotSupportedException();
            }
        }
    }
}
