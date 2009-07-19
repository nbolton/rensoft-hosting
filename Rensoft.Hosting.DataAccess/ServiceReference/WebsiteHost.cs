using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.DataEditing;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class WebsiteHost : RhspData, IRhspDataChild
    {
        public const int DefaultHttpPort = 80;
        public const int DefaultHttpsPort = 443;

        public override UniqueDataID UniqueDataID
        {
            get { return new UniqueDataID(DataID); }
        }

        public WebsiteHost(RhspDataID dataID)
            : base(dataID) { }
    }
}
