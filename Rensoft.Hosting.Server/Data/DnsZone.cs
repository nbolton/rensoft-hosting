using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Managers.Config;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    [RhspData("DnsZone", "DnsZones", "DnsZoneID")]
    public class DnsZone : RhspData, IRhspData, IRhspDataChild
    {
        public RhspDataID ParentID
        {
            get { return WebsiteID; }
            set { WebsiteID = value; }
        }

        public IRhspDataParent Parent
        {
            get { return Website; }
            set { Website = (Website)value; }
        }

        public Website Website { get; set; }

        [DataMember]
        public RhspDataID WebsiteID { get; set; }

        [DataMember]
        public ChildPendingAction PendingAction { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DefaultTtl { get; set; }

        [DataMember]
        public DnsRecord[] RecordArray { get; set; }

        public DnsZone() { }

        public DnsZone(RhspDataID dataID)
        {
            this.DataID = dataID;
        }

        public XElement ToXElement(HostingConfigManager manager)
        {
            return new XElement(
                "DnsZone",
                new XElement("WebsiteID", WebsiteID),
                new XElement("Name", Name),
                new XElement("DefaultTtl", DefaultTtl),
                new XElement("Records",
                    from r in RecordArray
                    where r.PendingAction.Equals(ChildPendingAction.Create)
                    select r.ToXElement(manager)
                )
            );
        }

        public void FromElement(XElement element, HostingConfigManager manager)
        {
            WebsiteID = new RhspDataID(element.GetElementValue<string>("WebsiteID", false));
            Name = element.GetElementValue<string>("Name", false);
            DefaultTtl = element.GetElementValue<string>("DefaultTtl", false);
            this.RecordArray = getRecordArray(element, manager);
        }

        private DnsRecord[] getRecordArray(XElement element, HostingConfigManager manager)
        {
            if (element.Element("Records") == null)
            {
                return new DnsRecord[0];
            }
            else
            {
                var q = from e in element.Elements("Records").Elements()
                        select DnsRecord.CreateFromElement(e, manager);

                return q.ToArray();
            }
        }

        public void UpdateElement(XElement element, HostingConfigManager manager)
        {
            element.SetElementValue("Name", Name);
            element.SetElementValue("DefaultTtl", DefaultTtl);
            RecordArray.ToList().ForEach(r => processRecord(r, element, manager));
        }

        private void processRecord(DnsRecord record, XElement zoneElement, HostingConfigManager manager)
        {
            switch (record.PendingAction)
            {
                case ChildPendingAction.Create:
                    zoneElement.Element("Records").Add(record.ToXElement(manager));
                    break;

                case ChildPendingAction.Update:
                    record.UpdateElement(getRecordElement(zoneElement, record.DataID), manager);
                    break;

                case ChildPendingAction.Delete:
                    try
                    {
                        getRecordElement(zoneElement, record.DataID).Remove();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Could not remove DNS zone from config.", ex);
                    }
                    break;
            }
        }

        private XElement getRecordElement(XElement zoneElement, RhspDataID dataID)
        {
            var q = from r in zoneElement.Elements("Records").Elements()
                    where r.GetElementValue<string>("RecordID", false) == dataID.Value
                    select r;

            return q.First();
        }
    }
}