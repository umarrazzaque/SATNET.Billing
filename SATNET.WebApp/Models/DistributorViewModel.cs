using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class DistributorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTierName { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
