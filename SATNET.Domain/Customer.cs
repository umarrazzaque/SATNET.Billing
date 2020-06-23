using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Customer: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        public int CustomerTypeId { get; set; }
        public int PriceTierId { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Notes { get; set; }
    }
}
