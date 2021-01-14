using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SATNET.WebApp.Models.Invoice
{
    public class SOInvoiceViewModel : BaseModel
    {
        public string InvoiceNumber { get; set; }
        public string OrderNumber { get; set; }
        public string RequestType { get; set; }
        public int RequestTypeId { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Total { get; set; }
        public int StatusId { get; set; }
        public int SiteId { get; set; }
        public string Status { get; set; }
        public string SubscriberName { get; set; }
        public string SiteName { get; set; }
        public string CustomerName { get; set; }
        public string SiteCity { get; set; }
        public string SubscriberEmail { get; set; }
        public string SiteArea { get; set; }
        public SelectList InvoiceStatusSelectList { get; set; }
        public DateTime? PlannedInstallationDate { get; set; }
        public int? Validity { get; set; }
        public DateTime? ScheduleDate { get; set; }
        public List<SOInvoiceItemViewModel> InvoiceItems { get; set; }
    }
    public class SOInvoiceItemViewModel :BaseModel
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
