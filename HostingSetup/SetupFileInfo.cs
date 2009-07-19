using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HostingSetup
{
    public class SetupFileInfo
    {
        public string TargetFile { get; private set; }
        public object Resource { get; private set; }

        public SetupFileInfo(string targetFile, object resource)
        {
            this.TargetFile = targetFile;
            this.Resource = resource;
        }
    }
}
