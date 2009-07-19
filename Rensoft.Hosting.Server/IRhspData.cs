using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Rensoft.Hosting.Server.Data;
using Rensoft.Hosting.Server.Managers.Config;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server
{
    public interface IRhspData
    {
        RhspDataID DataID { get; set; }
        XElement ToXElement(HostingConfigManager manager);
        void FromElement(XElement element, HostingConfigManager manager);
        void UpdateElement(XElement element, HostingConfigManager manager);
    }
}
