using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server
{
    public interface IRhspDataChild
    {
        RhspDataID ParentID { get; set; }
        IRhspDataParent Parent { get; set; }
        ChildPendingAction PendingAction { get; set; }
    }
}
