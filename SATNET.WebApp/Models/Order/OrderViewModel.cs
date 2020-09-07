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
        public string OrderNumber { get; set; }
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        public string CustomerShortName { get; set; }
        public string StatusName { get; set; }
        public int StatusId { get; set; }
        [Required(ErrorMessage = "Site name is required")]
        public string SiteName { get; set; }
        [Required(ErrorMessage = "Site is required")]
        public int SiteId { get; set; }
        [Required(ErrorMessage = "Site area is required")]
        public string SiteArea { get; set; }
        [Required(ErrorMessage = "Site city is required")]
        public string SiteCity { get; set; }
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
        [Required(ErrorMessage = "Dedicated service plan is required")]
        public string DedicatedServicePlanName { get; set; }
        public string ServicePlanTypeName { get; set; }
        public string ServicePlanUnit { get; set; }
        public decimal ServicePlanPrice { get; set; }
        public decimal ServicePlanProRataPrice { get; set; }

        [DisplayName("Service Plan")]
        public SelectList ServicePlanSelectList { get; set; }
        [Required(ErrorMessage = "Hardware is required")]
        public int HardwareId { get; set; }
        [Required(ErrorMessage = "Hardware Billing is required")]
        public int BillingId { get; set; }
        public SelectList BillingSelectList { get; set; }
        [Required(ErrorMessage = "Modem Model is required")]
        public int ModemModelId { get; set; }
        public SelectList ModemModelSelectList { get; set; }
        [Required(ErrorMessage = "Modem Serial No is required")]
        public int ModemSrNoId { get; set; }
        public SelectList ModemSrNoSelectList { get; set; }
        [Required(ErrorMessage = "Mac Air No is required")]
        public int MacAirNoId { get; set; }
        public SelectList MacAirNoSelectList { get; set; }
        [Required(ErrorMessage = "Antenna size is required")]
        public int AntennaSizeId { get; set; }
        public SelectList AntennaSizeSelectList { get; set; }
        [Required(ErrorMessage = "Antenna Serial No is required")]
        public string AntennaSrNo { get; set; }
        [Required(ErrorMessage = "Transceiver WATT is required")]
        public int TransceiverWATTId { get; set; }
        public SelectList TransceiverWATTSelectList { get; set; }
        [Required(ErrorMessage = "Transceiver Serial No is required")]
        public int TransceiverSrNoId { get; set; }
        public SelectList TransceiverSrNoSelectList { get; set; }
        public string HardwareModel { get; set; }
        [DisplayName("Hardware")]
        public SelectList HardwareSelectList { get; set; }
        public decimal HardwarePrice { get; set; }
        [DisplayName("IP")]
        public SelectList IPSelectList { get; set; }

        [DisplayName("IP")]
        public int? IPId { get; set; }
        [Required(ErrorMessage = "Change To IP is required")]
        public int ChangeIPId { get; set; }
        public string IPName { get; set; }
        public decimal IPPrice { get; set; }
        public DateTime ServiceOrderDate { get; set; }
        [DisplayName("Download")]
        [Required(ErrorMessage = "Download is required")]
        public int Download { get; set; }
        [DisplayName("Upload")]
        [Required(ErrorMessage = "Upload is required")]
        public int Upload { get; set; }
        [DisplayName("Planned Installation Date")]
        [Required(ErrorMessage = "Installation date is required")]
        public DateTime InstallationDate { get; set; }
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
        [DisplayName("Token(GB)")]
        public SelectList TokenSelectList { get; set; }
        [DisplayName("Promotion")]
        public SelectList PromotionSelectList { get; set; }
        public int? PromotionId { get; set; }
        public SelectList SiteCitySelectList { get; set; }
        [Required(ErrorMessage = "City is required")]
        public int SiteCityId { get; set; }
        public int SiteAreaId { get; set; }
        public SelectList CustomerSelectList { get; set; }
        [Required(ErrorMessage = "Customer is required")]
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        [Required(ErrorMessage = "Other field is required")]
        public string Other { get; set; }
        public decimal Total { get; set; }
        public string ModemModel { get; set; }
        public string TransceiverWATT { get; set; }
        public string AntennaSize { get; set; }
        public string RejectReason { get; set; }
        public SelectList ScheduleDateSelectList { get; set; }
        [Required(ErrorMessage = "Schedule date is required")]
        public int ScheduleDateId { get; set; }
        public string ScheduleDateName { get; set; }
        public SelectList HardwareConditionSelectList { get; set; }
        [Required(ErrorMessage = "Hardware condition is required")]
        public int HardwareConditionId { get; set; }
        [Required(ErrorMessage = "Change Service plan to is required")]
        public int ChangeServicePlanId { get; set; }
        [Required(ErrorMessage = "New Mac Air No is required")]
        public int NewMacAirNoId { get; set; }

    }
}
