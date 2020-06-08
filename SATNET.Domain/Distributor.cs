using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Distributor : BaseEntity
    {
        public string Name { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTier { get; set; }
    }
}
