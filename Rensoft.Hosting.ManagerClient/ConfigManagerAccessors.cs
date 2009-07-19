using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess;

namespace Rensoft.Hosting.ManagerClient
{
    public partial class ConfigManager
    {
        public string[] ServerNameArray
        {
            get { return GetArray<string>("ServerNameArray"); }
            set { SetArray("ServerNameArray", value); }
        }

        public RhspAuthMode AuthMode
        {
            get { return GetSingle<RhspAuthMode>("AuthMode"); }
            set { SetSinlge("AuthMode", value); }
        }
    }
}
