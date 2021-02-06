using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain.Reporting
{
    public class ReceivablePerCategory
    {
        public decimal ServicePlanTotal { get; set; }
        public decimal PublicIPTotal { get; set; }
        public decimal TokenTotal { get; set; }
        public decimal RebateTotal { get; set; }
        public decimal Total { get; set; }
    }
}
