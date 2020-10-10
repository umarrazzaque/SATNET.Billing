using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class CreditNote : BaseEntity
    {
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string CreditNoteNumber { get; set; }
        public int CustomerId { get; set; }
        public string Details { get; set; }
        public decimal Amount { get; set; }
        public string SiteName { get; set; }
        public string SubscriberName { get; set; }
        public string SiteCity { get; set; }
        public string SiteArea { get; set; }
        public DateTime CreditNoteDate { get; set; }
        public DateTime InvoiceDate { get; set; }
    }
}
