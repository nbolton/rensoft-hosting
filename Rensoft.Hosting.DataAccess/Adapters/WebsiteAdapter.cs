using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    [RhspAdapterUsage("Website")]
    public class WebsiteAdapter : RhspAdapter
    {
        public void Create(Website website)
        {
            Run("Create", new { website });
            //RhspConnection connection = CreateConnection();
            //RhspCommand command = new RhspCommand(connection, "Website.Create");
            //command.Parameters.Add("website", website);
            //command.Execute();
        }

        public bool ExistsWithAnyHost(Website checkWebsite)
        {
            return Run<bool>("ExistsWithAnyHost", new { checkWebsite });
            //RhspConnection connection = CreateConnection();
            //RhspCommand command = new RhspCommand(connection, "Website.ExistsWithAnyHost");
            //command.Parameters.Add("checkWebsite", checkWebsite);
            //RhspCommandResponse response = command.Execute();
            //return response.GetResult<bool>();
        }

        public Website Get(RhspDataID dataID)
        {
            return Run<Website>("Get", new { dataID });
            //RhspConnection connection = CreateConnection();
            //RhspCommand command = new RhspCommand(connection, "Website.Get");
            //command.Parameters.Add("dataID", dataID);
            //RhspCommandResponse response = command.Execute();
            //return response.GetResult<Website>();
        }

        public Website[] GetAll()
        {
            return Run<Website[]>("GetAll");
            //RhspConnection connection = CreateConnection();
            //RhspCommand command = new RhspCommand(connection, "Website.GetAll");
            //RhspCommandResponse response = command.Execute();
            //return response.GetResult<Website[]>();
        }

        public void Update(Website website)
        {
            Run("Update", new { website });
            //RhspConnection connection = CreateConnection();
            //RhspCommand command = new RhspCommand(connection, "Website.Update");
            //command.Parameters.Add("website", website);
            //command.Execute();
        }

        public WebsiteDeleteResult Delete(RhspDataID dataID)
        {
            return Run<WebsiteDeleteResult>("Delete", new { dataID });
            //RhspConnection connection = CreateConnection();
            //RhspCommand command = new RhspCommand(connection, "Website.Delete");
            //command.Parameters.Add("dataID", dataID);
            //command.Execute();
        }

        public string[] GetIpAddressArray()
        {
            return Run<string[]>("GetIpAddressArray");
            //RhspConnection connection = CreateConnection();
            //RhspCommand command = new RhspCommand(connection, "Website.GetIpAddressArray");
            //RhspCommandResponse response = command.Execute();
            //return response.GetResult<string[]>();
        }

        public bool SecurityValid(Website website, string currentPrimaryHostName)
        {
            return Run<bool>("SecurityValid", new { website, currentPrimaryHostName });

            //RhspConnection connection = CreateConnection();
            //RhspCommand command = new RhspCommand(connection, "Website.AnySecurityTemplateInvalid");
            //command.Parameters.Add("checkWebsite", checkWebsite);
            //RhspCommandResponse response = command.Execute();
            //return response.GetResult<bool>();
        }

        public Website[] GetFromCustomer(RhspDataID customerID)
        {
            return Run<Website[]>("GetFromCustomer", new { customerID });
        }
    }
}
