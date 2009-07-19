using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.ManagerClient.DataEditing
{
    public class SecurityTemplateAccessInfo
    {
        public SecurityTemplateAccess Access { get; private set; }

        public string Name
        {
            get { return getName(); }
        }

        public SecurityTemplateAccessInfo(SecurityTemplateAccess access)
        {
            this.Access = access;
        }

        private string getName()
        {
            switch (Access)
            {
                case SecurityTemplateAccess.Allow: return "Allow";
                case SecurityTemplateAccess.Deny: return "Deny";
                default: throw new NotSupportedException();
            }
        }
    }
}
