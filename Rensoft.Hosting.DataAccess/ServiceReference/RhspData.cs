using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.DataEditing;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public abstract partial class RhspData : IUniqueData
    {
        public abstract UniqueDataID UniqueDataID { get; }

        public RhspData(RhspDataID dataID)
        {
            this.DataID = dataID;
        }

        public bool Equals(IUniqueData other)
        {
            return this.UniqueDataID.Equals(other.UniqueDataID);
        }

        public static bool IsDeleteOrDiscard(IRhspDataChild dataChild)
        {
            return (dataChild.PendingAction == ChildPendingAction.Delete)
                || (dataChild.PendingAction == ChildPendingAction.Discard);
        }

        public static void SetDeleteOrDiscard(IRhspDataChild dataChild)
        {
            if (dataChild.PendingAction != ChildPendingAction.Create)
            {
                dataChild.PendingAction = ChildPendingAction.Delete;
            }
            else
            {
                dataChild.PendingAction = ChildPendingAction.Discard;
            }
        }
    }
}
