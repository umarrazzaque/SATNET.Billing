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
        public string Type { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
    }
}
