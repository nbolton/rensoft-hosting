using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Rensoft.Hosting.Server.Managers;
using Rensoft.Hosting.Server.Managers.Config;

namespace Rensoft.Hosting.Server
{
    public class RhspServiceContext
    {
        public ServerConfigManager ServerConfig { get; private set; }
        public HostingConfigManager HostingConfig { get; private set; }
        public string ConfigDirectory { get; set; }

        public RhspServiceContext()
        {
            this.ConfigDirectory = AppDomain.CurrentDomain.BaseDirectory;
            this.ServerConfig = RhspManager.CreateManager<ServerConfigManager>(this);
            this.HostingConfig = RhspManager.CreateManager<HostingConfigManager>(this);
        }

        public SqlConnection CreateLegacyConnection()
        {
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = ServerConfig.LegacyConnectionString;
            return connection;
        }
    }
}
