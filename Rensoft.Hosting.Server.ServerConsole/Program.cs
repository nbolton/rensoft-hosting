using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Rensoft.Hosting.Server.ServerConsole.Properties;

namespace Rensoft.Hosting.Server.ServerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RhspService server = new RhspService();
            ServiceHost host = new ServiceHost(server);

            Console.Write("Starting service... ");
            host.Open();
            Console.WriteLine("Done");

            Console.WriteLine();
            Console.Write("Press any key to exit.");
            Console.ReadKey();

            Console.Write("Stopping service... ");
            host.Close();
            Console.WriteLine("Done");
        }
    }
}
