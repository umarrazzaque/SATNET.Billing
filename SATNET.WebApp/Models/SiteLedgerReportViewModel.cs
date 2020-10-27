using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class SiteLedgerReportViewModel : BaseModel
    {
        public string SiteName { get; set; }
        public DateTime TransactionDate { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Total { get; set; }
        public decimal ProRataPrice { get; set; }
        public decimal IPPrice { get; set; }
        public decimal ServicePlanPrice { get; set; }
        public decimal IPProRataPrice { get; set; }
        public decimal TokenPrice { get; set; }
        public decimal ServicePlanTotal { get; set; }
        public decimal PromotionRebate { get; set; }
        public int Validity { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}
