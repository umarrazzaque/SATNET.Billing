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
            Id = StatusId = -1;
            Name = Status = City = Area = "";
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Subscriber { get; set; }
        public DateTime ActivatedDate { get; set; }
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
