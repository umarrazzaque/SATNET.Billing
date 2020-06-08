using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Customer: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
    }
}
