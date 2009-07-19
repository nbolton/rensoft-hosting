using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rensoft.Hosting.ManagerClient
{
    public static class Extensions
    {
        public static string GetHostAndPort(this Uri uri)
        {
            return uri.Host + ":" + uri.Port;
        }
    }
}
