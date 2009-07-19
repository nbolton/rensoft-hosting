using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rensoft.Hosting.DataAccess.Importing
{
    public class ImportStatusEventArgs : EventArgs
    {
        public ImportStatus Status { get; private set; }
        public IRhspImportable ImportItem { get; private set; }
        public string Message { get; private set; }

        public ImportStatusEventArgs(
            ImportStatus status, 
            IRhspImportable importItem,
            string message)
        {
            this.Status = status;
            this.ImportItem = importItem;
            this.Message = message;
        }
    }
}
