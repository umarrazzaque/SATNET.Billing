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
        public int SiteId { get; set; }
        public int CustomerId { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Total { get; set; }
        public string SubscriberName { get; set; }
        public string SiteCity { get; set; }
        public string SiteName { get; set; }
        public string SubscriberEmail { get; set; }
        public string SiteArea { get; set; }
        public DateTime? PlannedInstallationDate { get; set; }
        public int? Validity { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public List<SOInvoiceItem> InvoiceItems { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class SOInvoiceItem : BaseEntity
    {

        public int InvoiceId { get; set; }
        public int ItemTypeId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal Unit { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
    }
}
