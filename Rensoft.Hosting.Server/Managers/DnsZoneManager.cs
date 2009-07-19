using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Data;
using Rensoft.ServerManagement.DNS;

namespace Rensoft.Hosting.Server.Managers
{
    [RhspManagerUsage("DnsZone")]
    public class DnsZoneManager : RhspManager
    {
        private const string defaultTtl = "1h";

        [RhspModuleMethod]
        public void Create(DnsZone dnsZone)
        {
            enforceUniqueZone(dnsZone);
            HostingConfig.Create(dnsZone);
            CreateManager<IscBindManager>().CreateZone(dnsZone);
        }

        [RhspModuleMethod]
        public void ReplaceBatch(object[] dnsZoneArray)
        {
            dnsZoneArray.ToList().ForEach(dnsZone => Replace((DnsZone)dnsZone));
        }

        [RhspModuleMethod]
        public void Replace(DnsZone dnsZone)
        {
            // Remove any existing zones with matching name.
            var existing = from dz in HostingConfig.GetArray<DnsZone>()
                           where dz.Name == dnsZone.Name
                           select dz;
            existing.ToList().ForEach(dz => Delete(dz.DataID));

            // Now proceed to create the new zone.
            Create(dnsZone);
        }

        [RhspModuleMethod]
        public void Update(DnsZone dnsZone)
        {
            enforceUniqueZone(dnsZone);
            HostingConfig.Update(dnsZone);
            CreateManager<IscBindManager>().UpdateZone(dnsZone);
        }

        [RhspModuleMethod]
        public void Delete(RhspDataID dataID)
        {
            DnsZone dnsZone = HostingConfig.GetSingle<DnsZone>(dataID);
            HostingConfig.Delete<DnsZone>(dataID);
            CreateManager<IscBindManager>().DeleteZone(dnsZone);
        }

        private void enforceUniqueZone(DnsZone zone)
        {
            // Find zones with same name other than the current one.
            var q = from z in HostingConfig.GetArray<DnsZone>()
                    where z.Name == zone.Name
                    where !z.DataID.Equals(zone.DataID)
                    select z;

            if (q.Count() != 0)
            {
                throw new Exception(
                    "A DNS zone with the name '" + zone.Name + "' already exists.");
            }
        }

        public void Process(DnsZone dnsZone)
        {
            switch (dnsZone.PendingAction)
            {
                case ChildPendingAction.Create: Create(dnsZone); break;
                case ChildPendingAction.Update: Update(dnsZone); break;
                case ChildPendingAction.Delete: Delete(dnsZone.DataID); break;
            }
        }

        [RhspModuleMethod]
        public DnsZone[] GetAllFromMsDns()
        {
            MsDnsManager dnsManager = new MsDnsManager(ServerConfig.WindowsServerName);
            List<MsDnsZone> mdzList = dnsManager.GetZones(true);
            List<DnsZone> dzList = new List<DnsZone>();
            mdzList.ForEach(mdz => dzList.Add(convertToDnsZone(mdz)));
            cleanDnsZones(dzList);
            return dzList.ToArray();
        }

        private void cleanDnsZones(List<DnsZone> dzList)
        {
            foreach (DnsZone zone in dzList)
            {
                foreach (DnsRecord record in zone.RecordArray)
                {
                    // Ensure that zone alias is used where no name.
                    if (string.IsNullOrEmpty(record.Name))
                    {
                        record.Name = "@";
                    }

                    // Ensure that zone alias is used where record points to zone.
                    if (record.Value.TrimEnd('.') == zone.Name)
                    {
                        record.Value = "@";
                    }

                    // If TTL and default TTL are the same, then set to null.
                    if ((record.Ttl == "3600") && (zone.DefaultTtl == "1h"))
                    {
                        record.Ttl = null;
                    }
                }
            }
        }

        [RhspModuleMethod]
        public DnsZone[] GetAll()
        {
            return HostingConfig.GetArray<DnsZone>();
        }

        private DnsZone convertToDnsZone(MsDnsZone mdz)
        {
            DnsZone dnsZone = new DnsZone(RhspDataID.Generate());
            dnsZone.DefaultTtl = defaultTtl;
            dnsZone.Name = mdz.Name;
            dnsZone.RecordArray = convertToDnsRecords(mdz.MixedReords).ToArray();
            return dnsZone;
        }

        private IEnumerable<DnsRecord> convertToDnsRecords(IEnumerable<MsDnsRecord> mdrList)
        {
            List<DnsRecord> recordList = new List<DnsRecord>();
            foreach (MsDnsRecord mdr in mdrList.Where(mdr => (mdr.DnsType != MsDnsRecordType.Soa)))
            {
                DnsRecord record = new DnsRecord(RhspDataID.Generate());
                record.Name = mdr.Name;
                record.Value = getRecordValue(mdr);
                record.RecordType = convertToRecordType(mdr.DnsType);
                record.Ttl = (mdr.TTL != 0) ? mdr.TTL.ToString() : null;
                recordList.Add(record);
            }
            return recordList;
        }

        private string getRecordValue(MsDnsRecord mdr)
        {
            if (mdr is MsDnsMxRecord)
            {
                MsDnsMxRecord mxRecord = (MsDnsMxRecord)mdr;
                return string.Format("{0} {1}", mxRecord.Priority, mdr.Value);
            }
            else
            {
                return mdr.Value;
            }
        }

        private DnsRecordType convertToRecordType(MsDnsRecordType msDnsRecordType)
        {
            switch (msDnsRecordType)
            {
                case MsDnsRecordType.A:
                    return DnsRecordType.A;

                case MsDnsRecordType.Cname:
                    return DnsRecordType.CNAME;

                case MsDnsRecordType.Mx:
                    return DnsRecordType.MX;

                case MsDnsRecordType.Ns:
                    return DnsRecordType.NS;

                default:
                    throw new NotSupportedException("Microsoft DNS record type not supported.");
            }
        }
    }
}
