using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.DataAccess.Importing
{
    public class ImportActionInfo
    {
        public ImportAction Action { get; private set; }
        public string Name { get; private set; }

        public ImportActionInfo(ImportAction action, string name)
        {
            this.Action = action;
            this.Name = name;
        }
    }
}
