using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Managers.Config;

namespace Rensoft.Hosting.Server
{
    public interface IRhspDataParent
    {
        RhspDataID DataID { get; }
        IEnumerable<IRhspDataChild> GetDataChildren();
    }
}
