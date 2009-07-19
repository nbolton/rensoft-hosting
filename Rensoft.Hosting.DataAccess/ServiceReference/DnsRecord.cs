using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.DataEditing;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class DnsRecord : RhspData
    {
        public DnsRecord(RhspDataID dataID)
            : base(dataID) { }

        public override UniqueDataID UniqueDataID
        {
            get { return new UniqueDataID(DataID); }
        }
    }
}
