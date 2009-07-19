using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Rensoft.Hosting.Properties;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Rensoft.Hosting.IscBind
{
    public class IscBindSetup
    {
        public string BasePath { get; private set; }
        public SecurityIdentifier AccountSid { get; private set; }

        public IscBindSetup(string basePath, SecurityIdentifier accountSid)
        {
            this.BasePath = basePath;
            this.AccountSid = accountSid;
        }

        public void InitializeConfig()
        {
            string rndcConfPath = Path.Combine(BasePath, @"etc\rndc.conf");
            string namedConfPath = Path.Combine(BasePath, @"etc\named.conf");
            string namedConfGenPath = Path.Combine(BasePath, @"etc\named.generated.conf");

            if (File.Exists(namedConfPath))
            {
                // Replace the file to make the process simpler.
                string oldPath = Path.Combine(BasePath, @"etc\named.conf." + Environment.TickCount);
                File.Move(namedConfPath, oldPath);
            }

            if (File.Exists(rndcConfPath))
            {
                // Replace the file to make the process simpler.
                string oldPath = Path.Combine(BasePath, @"etc\rndc.conf." + Environment.TickCount);
                File.Move(rndcConfPath, oldPath);
            }

            string etcPath = Path.Combine(BasePath, "etc");
            if (!Directory.Exists(etcPath))
            {
                // Ensure that the etc path exists.
                Directory.CreateDirectory(etcPath);
            }

            string rndcContent = rndcConfigGenerate(BasePath, rndcConfPath);

            string namedStart = "# Use with the following in named.conf, adjusting the allow list as needed:\r\n";
            string namedEnd = "\r\n# End of named.conf";

            int namedIndexStart = rndcContent.IndexOf(namedStart) + namedStart.Length;
            int namedIndexEnd = rndcContent.IndexOf(namedEnd);

            string rndcKeyTextCommented = rndcContent.Substring(
                namedIndexStart,
                namedIndexEnd - namedIndexStart);

            string rndcKeyText = rndcKeyTextCommented.Replace("# ", null);

            string namedConfig = getNamedConfigStart() + "\r\n\r\n" + rndcKeyText;
            File.WriteAllText(namedConfPath, namedConfig);

            File.WriteAllText(
                namedConfGenPath,
                "# This file will contain an automatically generated configuration. " +
                "Any changes made by a user may be lost.\r\n");
        }

        private string getNamedConfigStart()
        {
            MemoryStream namedStream = new MemoryStream(Resources.IscBindNamedConfig);
            StreamReader namedReader = new StreamReader(namedStream);
            string namedConfigStart = namedReader.ReadToEnd();

            // Hard code the full etc path into the config file so relative config paths work.
            namedConfigStart = namedConfigStart.Replace("<etcFullPath>", Path.Combine(BasePath, "etc"));

            return namedConfigStart;
        }

        private string rndcConfigGenerate(string iscBindDirectory, string rndcConfPath)
        {
            string rndcPath = Path.Combine(iscBindDirectory, @"bin\rndc-confgen.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo(rndcPath);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            Process p = Process.Start(startInfo);
            p.WaitForExit();
            string output = p.StandardOutput.ReadToEnd();
            File.WriteAllText(rndcConfPath, output);
            return output;
        }

        public void IntitializeSecurity()
        {
            DirectorySecurity security = Directory.GetAccessControl(BasePath);
            AuthorizationRuleCollection rules = security.GetAccessRules(
                true, false, typeof(SecurityIdentifier));

            var q = from r in rules.OfType<FileSystemAccessRule>()
                    where r.IdentityReference == AccountSid
                    select r;

            if (q.Count() != 0)
            {
                // Remove any existing rules which could be allow or deny.
                q.ToList().ForEach(r => security.RemoveAccessRule(r));
            }

            // BIND only requires modify (e.g. for process file and log).
            FileSystemAccessRule rule = new FileSystemAccessRule(
                AccountSid, 
                FileSystemRights.Modify,
                InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, 
                PropagationFlags.None, 
                AccessControlType.Allow);

            security.AddAccessRule(rule);

            Directory.SetAccessControl(BasePath, security);
        }
    }
}
