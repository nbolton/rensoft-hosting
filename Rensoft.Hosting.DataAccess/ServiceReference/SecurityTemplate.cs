using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.DataEditing;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class SecurityTemplate : RhspData
    {
        public override UniqueDataID UniqueDataID
        {
            get { return new UniqueDataID(DataID); }
        }

        public SecurityTemplate(RhspDataID dataID)
            : base(dataID) { }
    }
}
