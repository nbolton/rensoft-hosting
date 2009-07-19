using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.DataEditing;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class DnsZone : RhspData
    {
        public bool ImportSelected { get; set; }

        public DnsZone(RhspDataID dataID)
            : base(dataID) { }

        public override UniqueDataID UniqueDataID
        {
            get { return new UniqueDataID(DataID); }
        }

        public void ResetPendingActions()
        {
            PendingAction = ChildPendingAction.None;

            // Set a new array without all the deleted records.
            RecordArray = RecordArray.Where(
                r => r.PendingAction != ChildPendingAction.Delete).ToArray(); ;

            RecordArray.ToList().ForEach(r => r.PendingAction = ChildPendingAction.None);
        }
    }
}
