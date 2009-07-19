using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Rensoft.Hosting.Setup.Options
{
    public class ManagerSetupOption : MsiSetupOption
    {
        public override string SetupFileName
        {
            get { return "HostingManagerSetup.msi"; }
        }

        public override string ProductName
        {
            get { return "Rensoft Hosting Manager 2008"; }
        }

        public override string MsiTitle
        {
            get { return "HostingManagerSetup"; }
        }

        public ManagerSetupOption(SetupWizardForm setupWizard)
            : base(setupWizard) { }
    }
}
