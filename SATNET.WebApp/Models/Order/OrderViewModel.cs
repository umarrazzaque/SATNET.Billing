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
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
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
        [Required(ErrorMessage = "Service Plan type is required")]
        public int ServicePlanTypeId { get; set; }
        [DisplayName("Service Plan Type")]
        public SelectList ServicePlanTypeSelectList { get; set; }
        [Required(ErrorMessage = "Service plan is required")]
        public int ServicePlanId { get; set; }
        public string ServicePlanName { get; set; }
        [DisplayName("Service Plan")]
        public SelectList ServicePlanSelectList { get; set; }
        [Required(ErrorMessage = "Hardware is required")]
        public int HardwareId { get; set; }
        [DisplayName("Hardware")]
        public SelectList HardwareSelectList { get; set; }
        [DisplayName("IP")]
        public SelectList IPSelectList { get; set; }

        [DisplayName("IP")]
        public int? IPId { get; set; }
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
        public int? UpgradeFromId { get; set; }
        [DisplayName("Upgrade From")]
        public SelectList UpgradeFromSelectList { get; set; }
        [Required(ErrorMessage = "Upgrade To is required")]
        [DisplayName("Upgrade To")]
        public SelectList UpgradeToSelectList { get; set; }
        public int? UpgradeToId { get; set; }
        [Required(ErrorMessage = "Downgrade From is required")]
        public int? DowngradeFromId { get; set; }
        [DisplayName("Downgrade From")]
        public SelectList DowngradeFromSelectList { get; set; }
        [DisplayName("Downgrade To")]
        public SelectList DowngradeToSelectList { get; set; }
        [Required(ErrorMessage = "Downgrade To is required")]
        public int? DowngradeToId { get; set; }
        [Required(ErrorMessage = "Token is required")]
        public int? TokenId { get; set; }
        [DisplayName("Token")]
        public SelectList TokenSelectList { get; set; }
        [DisplayName("Promotion")]
        public SelectList PromotionSelectList { get; set; }
        public int? PromotionId { get; set; }
        public string Other { get; set; }
        public DateTime? ServiceOrderDate { get; set; }
    }
}
