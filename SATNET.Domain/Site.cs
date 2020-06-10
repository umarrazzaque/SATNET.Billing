using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Site : BaseEntity
    {
        
        public string SiteName { get; set; }
        public int SiteStatusId { get; set; }
        public string SiteStaus { get; set; }
        public string City { get; set; }
        public string Area { get; set; }

    }
}
