using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.DataAccess
{
    public interface IRhspDataChild
    {
        ChildPendingAction PendingAction { get; set; }
    }
}
