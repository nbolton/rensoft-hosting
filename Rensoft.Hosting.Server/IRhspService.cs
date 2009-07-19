using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Diagnostics;
using Rensoft.Hosting.Server.Data;
using Rensoft.Hosting.Server.Managers;

namespace Rensoft.Hosting.Server
{
    [ServiceContract]
    [ServiceKnownType(typeof(RhspDataID))]
    [ServiceKnownType(typeof(Customer))]
    [ServiceKnownType(typeof(Website))]
    [ServiceKnownType(typeof(WebsiteHost))]
    [ServiceKnownType(typeof(DnsZone))]
    [ServiceKnownType(typeof(ServerStatusElement))]
    [ServiceKnownType(typeof(ServerStatusActionResult))]
    [ServiceKnownType(typeof(WebsiteDeleteResult))]
    [ServiceKnownType(typeof(CustomerDeleteResult))]
    public interface IRhspService
    {
        [OperationContract]
        RhspCommandResponse GetCommandResponse(RhspCommandCarrier carrier);
    }
}
