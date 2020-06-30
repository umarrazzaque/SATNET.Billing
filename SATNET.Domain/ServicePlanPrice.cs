using System;
using System.Collections.Generic;
using System.Text;


namespace SATNET.Domain
{
    public class ServicePlanPrice : BaseEntity
    {
        public int ServicePlanId { get; set; }
        public string ServicePlanName { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTierName { get; set; }
        public decimal Price { get; set; }
        public decimal Markup { get; set; }
        public decimal MarkupPercent { get; set; }
    }
}