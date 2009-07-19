using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.Server.Managers;

namespace Rensoft.Hosting.Server.Data
{
    /// <summary>
    /// A clean website has not been contaminated by the service client.
    /// </summary>
    public class CleanWebsite : Website
    {
        internal void GenerateIisPasswordIfEmpty(RhspManager manager)
        {
            if (IisSite.IdentityPassword == null)
            {
                GenerateIisPassword(manager);
            }
        }
    }
}
