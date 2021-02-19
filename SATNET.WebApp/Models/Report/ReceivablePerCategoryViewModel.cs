using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Report
{
    public class ReceivablePerCategoryViewModel
    {
        public decimal ServicePlanTotal { get; set; }
        public decimal PublicIPTotal { get; set; }
        public decimal TokenTotal { get; set; }
        public decimal RebateTotal { get; set; }
        public decimal Total { get; set; }
    }
}
