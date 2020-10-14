using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class CreditNoteViewModel : BaseModel
    {
        [Required(ErrorMessage = "Invoice is required")]
        public int InvoiceId { get; set; }
        public string InvoiceNumber { get; set; }
        public string CreditNoteNumber { get; set; }
        public SelectList InvoiceSelectList { get; set; }
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Comments are required")]
        public string Details { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        public string SiteName { get; set; }
        public string SubscriberName { get; set; }
        public string SiteCity { get; set; }
        public string SiteArea { get; set; }
        public DateTime CreditNoteDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public SelectList CustomerSelectList { get; set; }
    }
}
