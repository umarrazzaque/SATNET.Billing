using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Customer: BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int TypeId { get; set; }
<<<<<<< HEAD
        public string CustomerType { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTierName { get; set; }
=======
        public int PriceTierId { get; set; }
        public string Type { get; set; }
>>>>>>> devUmerKhalid
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
    }
}
