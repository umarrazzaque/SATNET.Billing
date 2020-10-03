using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class SOInvoice : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public string OrderNumber { get; set; }
        public string RequestType { get; set; }
        public int RequestTypeId { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string ProRataQuota { get; set; }
        public decimal ProRataPrice { get; set; }
        public decimal IPProRataPrice { get; set; }
        public decimal IPPrice { get; set; }
        public decimal MacAirPrice { get; set; }
        public decimal ServicePlanPrice { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Total { get; set; }
        public string SubscriberName { get; set; }
        public string SiteCity { get; set; }
        public string SiteName { get; set; }
        public string SubscriberEmail { get; set; }
        public string SiteArea { get; set; }
        public string ServicePlan { get; set; }
        public string ServicePlanType { get; set; }
        public int ServicePlanTypeId { get; set; }
        public string ServicePlanUnit { get; set; }
        public string IP { get; set; }
        public string Token { get; set; }
        public decimal TokenPrice { get; set; }
    }
}
