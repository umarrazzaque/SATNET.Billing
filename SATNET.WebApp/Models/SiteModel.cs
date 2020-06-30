using SATNET.WebApp.Helpers;
using SATNET.WebApp.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class SiteModel : BaseModel
    {
        public SiteModel() 
        {
            Id = StatusId = -1;
            Name = Status = City = Area = BriefDescription = "";
        }
        [DisplayName("Customer")]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        [DisplayName("Subscriber")]
        public int? SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        [DisplayName("Status")]
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string Subscriber { get; set; }
        [DisplayName("ActivationDate")]
        public DateTime ActivationDate { get; set; }
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
