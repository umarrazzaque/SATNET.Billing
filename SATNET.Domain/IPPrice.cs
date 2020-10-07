using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class IPPrice : BaseEntity
    {
        public int IPId { get; set; }
        public string IP { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTier { get; set; }
        public decimal Price { get; set; }
    }
}
