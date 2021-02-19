using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class MRCInvoice : BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string SiteCity { get; set; }
        public string SiteArea { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Total { get; set; }
        public string SubscriberName { get; set; }
        public List<SOInvoiceItem> InvoiceItems { get; set; }
    }
}
