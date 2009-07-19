using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Rensoft.Hosting.Server.Managers
{
    [DebuggerStepThrough]
    public abstract class LegacyManager : RhspManager
    {
        public LegacyManager()
        {
            BeforeInvokeMethod += new EventHandler(LegacyManager_BeforeInvokeMethod);
        }

        void LegacyManager_BeforeInvokeMethod(object sender, EventArgs e)
        {
            if (!Context.ServerConfig.EnableLegacyMode)
            {
                throw new RhspException(
                    "This server does not support legacy " +
                    "calls because legacy mode is not enabled.");
            }
        }
    }
}
