using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.DataAccess.Importing
{
    public interface IRhspImportable
    {
        RhspDataID DataID { get; }
        string ImportName { get; }
        ImportStatus ImportStatus { get; set; }
        string ImportMessage { get; set; }
        bool ImportSelected { get; set; }
        bool IsReadyForImport();
    }
}
