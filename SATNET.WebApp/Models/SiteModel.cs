using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class SiteModel
    {
        public SiteModel()
        {
            SiteId = SiteStatusId = -1;
            SiteName = SiteStaus = City = Area = "";
        }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int SiteStatusId { get; set; }
        public string SiteStaus { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
    }

    public class CreateSiteModel
    {
        public CreateSiteModel()
        {
            SiteModel = new SiteModel();
            SiteStatus = new List<LookUpModel>();
        }
        public SiteModel SiteModel { get; set; }
        public List<LookUpModel> SiteStatus { get; set; }
    }
}
