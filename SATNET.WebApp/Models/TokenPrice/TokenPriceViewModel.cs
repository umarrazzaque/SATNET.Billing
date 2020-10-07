using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.TokenPrice
{
    public class TokenPriceViewModel : BaseModel
    {
        public int TokenId { get; set; }
        public string Token { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTier { get; set; }
        public SelectList TokenSelectList { get; set; }
        public SelectList PriceTierSelectList { get; set; }
        public decimal Price { get; set; }
    }
}
