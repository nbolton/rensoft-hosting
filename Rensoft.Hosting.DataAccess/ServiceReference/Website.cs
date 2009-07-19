using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.DataEditing;
using System.IO;
using Rensoft.Hosting.DataAccess.Importing;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class Website : RhspData, IRhspImportable, IUniqueData
    {
        public WebsiteHost PrimaryHost
        {
            get
            {
                if (PrimaryHostID == null)
                {
                    throw new Exception(
                        "The primary host cannot be found because the primary host ID was not set.");
                }

                return HostArray.Where(h => h.DataID == PrimaryHostID).Single();
            }
        }

        public string Name
        {
            get { return PrimaryHost.Name; }
        }

        public override UniqueDataID UniqueDataID
        {
            get { return new UniqueDataID(DataID.Value); }
        }

        private List<WebsiteHost> getHostList()
        {
            return new List<WebsiteHost>(HostArray);
        }

        public Website(RhspDataID hostingID)
            : base(hostingID)
        {
            IisSite = new WebsiteIisSite();
        }

        public string GetDirectory(string baseDirectory)
        {
            if (Customer == null)
            {
                throw new InvalidOperationException(
                    "Cannot get website directory when customer is not set.");
            }

            return Path.Combine(Customer.GetWebsiteDirectory(baseDirectory), Name);
        }

        #region IRhspImportable Members

        public string ImportID
        {
            get { throw new NotImplementedException(); }
        }

        public string ImportName
        {
            get { throw new NotImplementedException(); }
        }

        public ImportSource ImportSource
        {
            get { throw new NotImplementedException(); }
        }

        public ImportStatus ImportStatus
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public ImportAction ImportAction
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ImportMessage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool ImportSelected
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadyForImport()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
