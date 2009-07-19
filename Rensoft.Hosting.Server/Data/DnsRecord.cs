using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;
using Rensoft.Hosting.Server.Managers.Config;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    public class DnsRecord : RhspData, IRhspData, IRhspDataChild
    {
        public RhspDataID ParentID { get; set; }
        public IRhspDataParent Parent { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        [DataMember]
        public string Ttl { get; set; }

        [DataMember]
        public DnsRecordType RecordType { get; set; }

        [DataMember]
        public ChildPendingAction PendingAction { get; set; }

        public DnsRecord() { }

        public DnsRecord(RhspDataID dataID)
        {
            this.DataID = dataID;
        }

        public XElement ToXElement(HostingConfigManager manager)
        {
            return new XElement(
                "Record",
                new XElement("RecordID", DataID),
                new XElement("Name", Name),
                new XElement("Ttl", Ttl),
                new XElement("RecordType", RecordType.ToString()),
                new XElement("Value", Value)
            );
        }

        public void FromElement(XElement element, HostingConfigManager manager)
        {
            DataID = new RhspDataID(element.GetElementValue<string>("RecordID", false));
            Name = element.GetElementValue<string>("Name", false);
            Ttl = element.GetElementValue<string>("Ttl", true);
            RecordType = getRecordType(element.GetElementValue<string>("RecordType", false));
            Value = element.GetElementValue<string>("Value", false);
        }

        private DnsRecordType getRecordType(string stringValue)
        {
            return (DnsRecordType)Enum.Parse(typeof(DnsRecordType), stringValue);
        }

        public void UpdateElement(XElement element, HostingConfigManager manager)
        {
            element.SetElementValue("Name", Name);
            element.SetElementValue("Value", Value);
            element.SetElementValue("RecordType", RecordType.ToString());
            element.SetElementValue("Ttl", Ttl);
        }

        public static DnsRecord CreateFromElement(XElement element, HostingConfigManager manager)
        {
            DnsRecord record = new DnsRecord();
            record.FromElement(element, manager);
            return record;
        }
    }
}
