using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.DataEditing;
using System.IO;
using Rensoft.Hosting.DataAccess.Importing;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class Customer : RhspData, IRhspImportable, IUniqueData
    {
        public ImportStatus ImportStatus { get; set; }
        public string ImportMessage { get; set; }
        public bool ImportSelected { get; set; }

        public ImportSource ImportSource
        {
            get { return ImportSource.Legacy; }
        }

        public string ImportName
        {
            get { return Code + " - " + Name; }
        }

        public override UniqueDataID UniqueDataID
        {
            get { return new UniqueDataID(Code); }
        }

        public string CodeAndName
        {
            get { return string.Format("{0} - {1}", Code, Name); }
        }

        public Customer(RhspDataID hostingID)
            : base(hostingID) { }

        public string GetWebsiteDirectory(string baseDirectory)
        {
            return Path.Combine(baseDirectory, Code);
        }

        public bool IsReadyForImport()
        {
            if (!ImportSelected)
            {
                return false;
            }

            if (string.IsNullOrEmpty(Code))
            {
                return false;
            }

            if (string.IsNullOrEmpty(Name))
            {
                return false;
            }

            return true;
        }
    }
}
