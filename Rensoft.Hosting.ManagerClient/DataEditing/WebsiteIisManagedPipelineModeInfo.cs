using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rensoft.Hosting.DataAccess.ServiceReference;

namespace Rensoft.Hosting.ManagerClient.DataEditing
{
    public class WebsiteIisManagedPipelineModeInfo
    {
        public WebsiteIisManagedPipelineMode Mode { get; private set; }

        public string Text
        {
            get
            {
                switch (Mode)
                {
                    case WebsiteIisManagedPipelineMode.Classic:
                        return "Classic";

                    case WebsiteIisManagedPipelineMode.Integrated:
                        return "Integrated";

                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public WebsiteIisManagedPipelineModeInfo(WebsiteIisManagedPipelineMode mode)
        {
            this.Mode = mode;
        }
    }
}
