using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class TokenPrice : BaseEntity
    {
        public int TokenId { get; set; }
        public string Token { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTier { get; set; }
        public decimal Price { get; set; }
    }
}
