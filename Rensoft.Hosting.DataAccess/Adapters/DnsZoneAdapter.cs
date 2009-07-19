using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    [RhspAdapterUsage("DnsZone")]
    public class DnsZoneAdapter : RhspAdapter
    {
        public DnsZone[] GetAll()
        {
            return Run<DnsZone[]>("GetAll");
        }

        public DnsZone[] GetAllFromMsDns()
        {
            return Run<DnsZone[]>("GetAllFromMsDns");
        }

        public void ReplaceBatch(object[] dnsZoneArray)
        {
            Run("ReplaceBatch", new { dnsZoneArray });
        }
    }
}
