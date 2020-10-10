using SATNET.WebApp.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.IPPrice
{
    public class IPPriceViewModel: BaseModel
    {
        public int IPId { get; set; }
        public string IP { get; set; }
        public SelectList IPSelectList { get; set; }
        public SelectList PriceTierSelectList { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTier { get; set; }
        public decimal Price { get; set; }
    }
}
