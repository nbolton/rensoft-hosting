using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rensoft.Hosting.Server.Data;
using Rensoft.Hosting.Server.Managers;
using Rensoft.Hosting.Server;

namespace Rensoft.Hosting.Test
{
    [TestClass]
    public class DnsZoneTests : TestBase
    {
        [TestMethod]
        public void CrudTest()
        {
            DnsZoneManager manager = LocalContext.Default.CreateManager<DnsZoneManager>();

            DnsRecord r1 = new DnsRecord(RhspDataID.Generate());
            r1.PendingAction = ChildPendingAction.Create;
            r1.Name = "@";
            r1.RecordType = DnsRecordType.NS;
            r1.Value = "ns1.rensoft.net.";

            DnsRecord r2 = new DnsRecord(RhspDataID.Generate());
            r2.PendingAction = ChildPendingAction.Create;
            r2.Name = "@";
            r2.RecordType = DnsRecordType.A;
            r2.Value = "127.0.0.1";

            DnsZone zone1 = new DnsZone(RhspDataID.Generate());
            zone1.Name = "test.rensoft.net";
            zone1.RecordArray = new DnsRecord[] { r1, r2 };

            manager.Create(zone1);

            r2.Ttl = "100";
            r2.PendingAction = ChildPendingAction.Update;

            DnsRecord r3 = new DnsRecord(RhspDataID.Generate());
            r3.PendingAction = ChildPendingAction.Create;
            r3.Name = "a";
            r3.RecordType = DnsRecordType.CNAME;
            r3.Value = "@";

            zone1.RecordArray = new DnsRecord[] { r1, r2, r3 };

            manager.Update(zone1);

            manager.Delete(zone1.DataID);
        }
    }
}
