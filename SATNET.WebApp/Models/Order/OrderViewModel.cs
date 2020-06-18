using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Order
{
    public class OrderViewModel: BaseModel
    {
        [DisplayName("Reseller/Partner Name")]
        public string ResellerName { get; set; }
        public string StatusName { get; set; }
        public string SiteName { get; set; }
        [Required(ErrorMessage = "Site is required")]
        public int SiteId { get; set; }
        [DisplayName("Site")]
        public SelectList SiteSelectList { get; set; }
        [Required(ErrorMessage = "Request Type is required")]
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        [DisplayName("Request Type")]
        public SelectList RequestTypeSelectList { get; set; }
        [Required(ErrorMessage = "Plan type is required")]
        public int PlanTypeId { get; set; }
        [DisplayName("Plan Type")]
        public SelectList PlanTypeSelectList { get; set; }
        [Required(ErrorMessage = "Hardware is required")]
        public int HardwareId { get; set; }
        [DisplayName("Hardware")]
        public SelectList HardwareSelectList { get; set; }
        [Required(ErrorMessage = "Package is required")]
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        [DisplayName("Package")]
        public SelectList PackageSelectList { get; set; }
        [DisplayName("IP")]
        public string IP { get; set; }
        [DisplayName("Download")]
        [Required(ErrorMessage = "Download is required")]
        public int Download { get; set; }
        [DisplayName("Upload")]
        [Required(ErrorMessage = "Upload is required")]
        public int Upload { get; set; }
        [DisplayName("Planned Installation Date")]
        [Required(ErrorMessage = "Installation date is required")]
        public DateTime? InstallationDate { get; set; }
        [DisplayName("Name")]
        [Required(ErrorMessage = "Subscriber name is required")]
        public string SubscriberName { get; set; }
        [DisplayName("City")]
        [Required(ErrorMessage = "Subscriber city is required")]
        public string SubscriberCity { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Subscriber email is required")]
        [EmailAddress]
        public string SubscriberEmail { get; set; }
        [DisplayName("Area")]
        [Required(ErrorMessage = "Subscriber area is required")]
        public string SubscriberArea { get; set; }
        [DisplayName("Notes")]
        public string SubscriberNotes { get; set; }
        [Required(ErrorMessage = "Upgrade From is required")]
        [DataType(DataType.DateTime)]
        public DateTime? UpgradeFrom { get; set; }
        [Required(ErrorMessage = "Upgrade To is required")]
        [DataType(DataType.DateTime)]
        public DateTime? UpgradeTo { get; set; }
        [Required(ErrorMessage = "Downgrade From is required")]
        [DataType(DataType.DateTime)]
        public DateTime? DowngradeFrom { get; set; }
        [Required(ErrorMessage = "Downgrade To is required")]
        [DataType(DataType.DateTime)]
        public DateTime? DowngradeTo { get; set; }
        public string Token { get; set; }
        public string Promotion { get; set; }
        public string Other { get; set; }
        public DateTime? ServiceOrderDate { get; set; }
    }
}
