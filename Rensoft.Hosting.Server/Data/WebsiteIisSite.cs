using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Rensoft.Hosting.Server.Data
{
    [DataContract]
    public class WebsiteIisSite
    {
        public Website Website { get; set; }

        [DataMember]
        public WebsiteIisMode Mode { get; set; }

        [DataMember]
        public WebsiteIisManagedPipelineMode ManagedPipelineMode { get; set; }

        [DataMember]
        public string ManagedRuntimeVersion { get; set; }

        [DataMember]
        public long SiteID { get; set; }

        [DataMember]
        public string SiteName
        {
            get { return getGenericName(); }
            set { }
        }

        [DataMember]
        public string RedirectUrl { get; set; }

        [DataMember]
        public string IdentitySid { get; set; }

        [DataMember]
        public Password IdentityPassword { get; set; }

        [DataMember]
        public string ApplicationPoolName
        {
            get { return getGenericName(); }
            set { }
        }

        [DataMember]
        public string IdentityUserName
        {
            get { return getIdentityUserName(); }
            set { }
        }

        private string getGenericName()
        {
            if (Mode == WebsiteIisMode.Disabled)
            {
                //throw new Exception("Cannot get IIS identity user name when IIS site is disabled.");
                return null;
            }

            assertDependantProperties();
            return string.Format("{0} - {1}", Website.Customer.Code, Website.PrimaryHost.Name);
        }

        private void assertDependantProperties()
        {
            if (Website == null)
            {
                throw new Exception("Website has not been set.");
            }

            if (Website.Customer == null)
            {
                throw new Exception("Customer has not been set on the website.");
            }
        }

        private string getIdentityUserName()
        {
            if (Mode == WebsiteIisMode.Disabled)
            {
                //throw new Exception("Cannot get IIS identity user name when IIS site is disabled.");
                return null;
            }

            if (SiteID == 0)
            {
                return null;
            }

            return string.Format("IUSR-{0}", SiteID);
        }
    }
}
