using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Order
{
    public class CreateSiteViewModel
    {
        [DisplayName("Reseller/Partner Name")]
        public string ResellerName { get; set; }
        [DisplayName("Site Name")]
        [Required(ErrorMessage = "Site name is required")]
        public string Name { get; set; }
        public int PlanTypeId { get; set; }
        [DisplayName("Plan Type")]
        [Required(ErrorMessage = "Plan type is required")]
        public SelectList PlanTypeSelectList { get; set; }
        public int HardwareId { get; set; }
        [DisplayName("Hardware")]
        [Required(ErrorMessage = "Hardware is required")]
        public SelectList HardwareSelectList { get; set; }
        public int PackageId { get; set; }
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
        [DisplayName("Customer Name")]
        [Required(ErrorMessage = "Customer name is required")]
        public string CustomerName { get; set; }
        [DisplayName("Customer City")]
        [Required(ErrorMessage = "Customer city is required")]
        public string CustomerCity { get; set; }
        [DisplayName("Customer Email")]
        [Required(ErrorMessage = "Customer email is required")]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        [DisplayName("Customer Area")]
        [Required(ErrorMessage = "Customer area is required")]
        public string CustomerArea { get; set; }
        [DisplayName("Customer Notes")]
        public string CustomerNotes { get; set; }
    }
}
