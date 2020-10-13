using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class HardwareComponentPrice : BaseEntity
    {
        public int HardwareComponentId { get; set; }
        public string HCValue { get; set; }
        public int PlanTypeId { get; set; }
        public string PlanType { get; set; }
        public int? HCSpareTypeId { get; set; }
        public string HCSpareType { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTierName { get; set; }
        public decimal Price { get; set; }

    }

    
}
