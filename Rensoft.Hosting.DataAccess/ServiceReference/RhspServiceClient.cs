using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Security.Principal;
using System.Diagnostics;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class RhspServiceClient
    {
        public RhspServiceClient(
            NetTcpBinding binding, 
            EndpointAddress remoteAddress,
            RhspSecurity security,
            TimeSpan timeout)
            : base(binding, remoteAddress)
        {
            if (security.AuthMode == RhspAuthMode.Custom)
            {
                // Impersonate the windows user instead of sending actual identity.
                this.ClientCredentials.Windows.AllowedImpersonationLevel = TokenImpersonationLevel.Impersonation;
                this.ClientCredentials.Windows.ClientCredential.UserName = security.CustomCredentials.UserName;
                this.ClientCredentials.Windows.ClientCredential.Password = security.CustomCredentials.Password;
            }

            this.InnerChannel.OperationTimeout = timeout;
        }
    }
}
