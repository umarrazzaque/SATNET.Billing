using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Order : BaseEntity
    {
        public int SiteId { get; set; }
        public int CustomerId { get; set; }
        public string SiteName { get; set; }
        public string SiteCity { get; set; }
        public string SiteArea { get; set; }
        public string CustomerName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int? SubscriberId { get; set; }
        public string SubscriberName { get; set; }
        public string SubscriberCity { get; set; }
        public string SubscriberEmail { get; set; }
        public string SubscriberArea { get; set; }
        public string SubscriberNotes{ get; set; }
        public int HardwareId { get; set; }
        public string HardwareModel { get; set; }
        public decimal HardwarePrice { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public int? UpgradeFromId { get; set; }
        public int? UpgradeToId { get; set; }
        public int? DowngradeFromId { get; set; }
        public int? DowngradeToId { get; set; }
        public DateTime InstallationDate { get; set; }
        public int ServicePlanTypeId { get; set; }
        public string ServicePlanTypeName { get; set; }
        public int ServicePlanId { get; set; }
        public string ServicePlanName { get; set; }
        public decimal ServicePlanPrice { get; set; }
        public decimal ServiceProRataPrice { get; set; }
        public string ServicePlanUnit { get; set; }
        public int? IPId { get; set; }
        public string IPName { get; set; }
        public decimal IPPrice { get; set; }
        public DateTime ServiceOrderDate { get; set; }
        public int? Download { get; set; }
        public int? Upload { get; set; }
        public int? TokenId { get; set; }
        public int? PromotionId { get; set; }
        public string Other { get; set; }
        public decimal Total { get; set; }
    }
}
