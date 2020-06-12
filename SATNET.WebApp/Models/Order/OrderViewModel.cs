using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Order
{
    public class OrderViewModel: BaseModel
    {
        [DisplayName("Distributor Name")]
        public string DistributorName { get; set; }
        public int SiteId { get; set; }
        [DisplayName("Site Name")]
        public string SiteName { get; set; }
        public string PackageName { get; set; }
        public DateTime? ServiceOrderDate { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCity { get; set; }
        public string Notes { get; set; }
        public DateTime? UpgradeFrom { get; set; }
        public DateTime? UpgradeTo { get; set; }
        public DateTime? DowngradeFrom { get; set; }
        public DateTime? DowngradeTo { get; set; }
        public string Token { get; set; }
        public string Promotion { get; set; }
    }
}
