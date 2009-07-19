using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Rensoft.Hosting.DataAccess.ServiceReference;
using Rensoft.Hosting.DataAccess.Adapters;
using Rensoft.Hosting.ManagerClient.Properties;
using Rensoft.Hosting.DataAccess;
using System.ServiceModel.Description;

namespace Rensoft.Hosting.ManagerClient
{
    public class LocalContext
    {
        public RhspClientContext ClientContext { get; set;  }
        public string EncryptorSecret { get; set; }

        public static LocalContext Default = new LocalContext();

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            assertClientContext();
            return ClientContext.CreateAdapter<TAdapter>();
        }

        public RhspConnection CreateConnection()
        {
            assertClientContext();
            return ClientContext.CreateConnection();
        }

        public Password EncryptPassword(string plainText)
        {
            assertClientContext();
            return ClientContext.EncryptPassword(plainText);
        }

        private void assertClientContext()
        {
            if (ClientContext == null)
            {
                throw new Exception("Client context not set.");
            }
        }
    }
}
