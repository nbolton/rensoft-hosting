using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    public class ServerStatusAdapter : RhspAdapter
    {
        public ServerStatusElement[] GetElementArray()
        {
            RhspConnection connection = CreateConnection();
            RhspCommand command = new RhspCommand(connection, "ServerStatus.GetElementArray");
            RhspCommandResponse response = command.Execute();
            return response.GetResult<ServerStatusElement[]>();
        }

        public ServerStatusActionResult RunActionCommand(string actionCommand)
        {
            RhspConnection connection = CreateConnection();
            RhspCommand command = new RhspCommand(connection, "ServerStatus." + actionCommand);
            RhspCommandResponse response = command.Execute();
            return response.GetResult<ServerStatusActionResult>();
        }
    }
}
