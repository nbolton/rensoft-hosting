using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Rensoft.Hosting.DataAccess
{
    public class RhspSecurity
    {
        public static RhspSecurity Default
        {
            get { return new RhspSecurity { AuthMode = RhspAuthMode.Default }; }
        }

        public RhspAuthMode AuthMode { get; set; }
        public NetworkCredential CustomCredentials { get; set; }
    }
}
