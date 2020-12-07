using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Order : BaseEntity
    {
        public string OrderNumber { get; set; }
        public int SiteId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCode { get; set; }
        public string SiteName { get; set; }
        public string SiteCity { get; set; }
        public int SiteCityId { get; set; }
        public string SiteArea { get; set; }
        public string CustomerName { get; set; }
        public string CustomerShortName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int? SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string SubscriberCity { get; set; }
        public string SubscriberEmail { get; set; }
        public string SubscriberArea { get; set; }
        public string SubscriberNotes{ get; set; }
        public int BillingId { get; set; }
        public string AirMac { get; set; }
        public string Promotion { get; set; }
        public string HardwareModel { get; set; }
        public decimal HardwarePrice { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public int? UpgradeFromId { get; set; }
        public int? UpgradeToId { get; set; }
        public string UpgradeToTypeName { get; set; }
        public string UpgradeToName { get; set; }
        public int? DowngradeFromId { get; set; }
        public int? DowngradeToId { get; set; }
        public string DowngradeToTypeName { get; set; }
        public string DowngradeToName { get; set; }
        public DateTime InstallationDate { get; set; }
        public int ServicePlanTypeId { get; set; }
        public string ServicePlanTypeName { get; set; }
        public string DedicatedServicePlanName { get; set; }
        public int ServicePlanId { get; set; }
        public string ServicePlanName { get; set; }
        public string CurrentSpTypeName { get; set; }
        public string CurrentSpName { get; set; }
        public string ChangeSpTypeName { get; set; }
        public string ChangeSpName { get; set; }
        public decimal ServicePlanPrice { get; set; }
        public decimal ServiceProRataPrice { get; set; }
        public decimal ProRataQuota { get; set; }
        public decimal UpgradeToProRataQuota { get; set; }
        public string ServicePlanUnit { get; set; }
        public int? IPId { get; set; }
        public int ChangeIPId { get; set; }
        public string IPName { get; set; }
        public string CurrentIPName { get; set; }
        public decimal IPPrice { get; set; }
        public DateTime ServiceOrderDate { get; set; }
        public int? Download { get; set; }
        public int? Upload { get; set; }
        public int? TokenId { get; set; }
        public string Token { get; set; }
        public int? PromotionId { get; set; }
        public string Other { get; set; }
        public decimal Total { get; set; }
        public string ModemModel { get; set; }
        public string RejectReason { get; set; }
        public int ScheduleDateId { get; set; }
        public string ScheduleDateName { get; set; }
        public int HardwareConditionId { get; set; }
        public int ChangeServicePlanId { get; set; }
        public string NewAirMac { get; set; }
        public bool IsServicePlanFull { get; set; }
    }
}
