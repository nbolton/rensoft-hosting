using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.DataAccess.Adapters
{
    public partial class ServerConfigAdapter : RhspAdapter
    {
        protected void Set(string settingName, object value)
        {
            RhspConnection connecton = CreateConnection();
            RhspCommand command = new RhspCommand(connecton, "ServerConfig.Set");
            command.Parameters.Add("settingName", settingName);
            command.Parameters.Add("value", value);
            RhspCommandResponse response = command.Execute();
        }

        protected TValue Get<TValue>(string settingName)
        {
            RhspConnection connecton = CreateConnection();
            RhspCommand command = new RhspCommand(connecton, "ServerConfig.Get");
            command.Parameters.Add("settingName", settingName);
            command.Parameters.Add("typeName", typeof(TValue).FullName);
            RhspCommandResponse response = command.Execute();
            return response.GetResult<TValue>();
        }
    }
}
