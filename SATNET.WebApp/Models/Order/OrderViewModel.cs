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
        public string SiteName { get; set; }
        public int SiteId { get; set; }
        [DisplayName("Site")]
        [Required(ErrorMessage = "Site is required")]
        public SelectList SiteSelectList { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        [DisplayName("Request Type")]
        [Required(ErrorMessage = "Request Type is required")]
        public SelectList RequestTypeSelectList { get; set; }
        public int PlanTypeId { get; set; }
        [DisplayName("Plan Type")]
        [Required(ErrorMessage = "Plan type is required")]
        public SelectList PlanTypeSelectList { get; set; }
        public int HardwareId { get; set; }
        [DisplayName("Hardware")]
        [Required(ErrorMessage = "Hardware is required")]
        public SelectList HardwareSelectList { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        [DisplayName("Package")]
        [Required(ErrorMessage = "Package is required")]
        public SelectList PackageSelectList { get; set; }
        [DisplayName("IP")]
        [Required(ErrorMessage = "IP is required")]
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
        [Required(ErrorMessage = "Customer name is required")]
        public string CustomerName { get; set; }
        [DisplayName("City")]
        [Required(ErrorMessage = "Customer city is required")]
        public string CustomerCity { get; set; }
        [DisplayName("Email")]
        [Required(ErrorMessage = "Customer email is required")]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        [DisplayName("Area")]
        [Required(ErrorMessage = "Customer area is required")]
        public string CustomerArea { get; set; }
        [DisplayName("Notes")]
        public string CustomerNotes { get; set; }
        public DateTime? UpgradeFrom { get; set; }
        public DateTime? UpgradeTo { get; set; }
        public DateTime? DowngradeFrom { get; set; }
        public DateTime? DowngradeTo { get; set; }
        public string Token { get; set; }
        public string Promotion { get; set; }
        public string Other { get; set; }
        public DateTime ServiceOrderDate { get; set; }
    }
}
