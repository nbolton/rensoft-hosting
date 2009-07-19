using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Rensoft.Hosting.DataAccess.ServiceReference
{
    public partial class RhspCommand
    {
        private RhspConnection conntection;

        public RhspConnection Conntection
        {
            get { return conntection; }
            set { conntection = value; }
        }

        private RhspCommand()
        {
            this.Parameters = new RhspParameterCollection();
        }

        public RhspCommand(RhspConnection conntection, string commandText) : this()
        {
            this.conntection = conntection;
            this.CommandText = commandText;
        }

        public RhspCommandResponse Execute()
        {
            RhspCommandCarrier carrier = new RhspCommandCarrier();
            carrier.Context = conntection.Context;
            carrier.Command = this;
            return conntection.Client.GetCommandResponse(carrier);
        }
    }
}
