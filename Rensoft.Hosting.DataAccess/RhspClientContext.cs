using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;
using System.ServiceModel;
using Rensoft.Hosting.DataAccess.Adapters;

namespace Rensoft.Hosting.DataAccess
{
    public class RhspClientContext
    {
        private RhspClientManager manager;

        public EndpointAddress Address { get; protected set; }
        public NetTcpBinding Binding { get; protected set; }
        public RhspSecurity Security { get; protected set; }
        public string EncryptorSecret { get; set; }

        public TimeSpan Timeout
        {
            get { return manager.Timeout; }
            set { manager.Timeout = value; }
        }

        public int HostingUserID
        {
            get { return manager.Context.HostingUserID; }
            set { manager.Context.HostingUserID = value; }
        }

        public RhspClientContext(string serviceUri, RhspSecurity security)
        {
            this.Binding = new NetTcpBinding();

            // Set allowed message size to 512kB.
            this.Binding.MaxReceivedMessageSize = 524288;

            this.Address = new EndpointAddress(serviceUri);
            this.Security = security;

            this.manager = new RhspClientManager
            {
                Binding = this.Binding,
                Address = this.Address,
                Security = this.Security
            };
        }

        public TAdapter CreateAdapter<TAdapter>()
            where TAdapter : RhspAdapter
        {
            return manager.CreateAdapter<TAdapter>();
        }

        public RhspConnection CreateConnection()
        {
            return manager.CreateConnection();
        }

        public Password EncryptPassword(string plainText)
        {
            return Password.FromPlainText(plainText, EncryptorSecret);
        }
    }
}
