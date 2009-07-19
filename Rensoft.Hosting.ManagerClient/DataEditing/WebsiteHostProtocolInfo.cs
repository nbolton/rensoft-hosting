using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.ManagerClient.DataEditing
{
    public class WebsiteHostProtocolInfo
    {
        public WebsiteHostProtocol Protocol { get; private set; }

        public string Name
        {
            get
            {
                switch (Protocol)
                {
                    case WebsiteHostProtocol.Http:
                        return "http";

                    case WebsiteHostProtocol.Https:
                        return "https";

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public WebsiteHostProtocolInfo(WebsiteHostProtocol protocol)
        {
            this.Protocol = protocol;
        }
    }
}
