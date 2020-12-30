using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SATNET.WebApp.Models.Invoice
{
    public class MRCInvoiceViewModel : BaseModel
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
        public decimal Total { get; set; }
        public string SubscriberName { get; set; }
        public List<SOInvoiceItemViewModel> InvoiceItems { get; set; }
    }
}
