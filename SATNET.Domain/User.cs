using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime PasswordExpiry { get; set; }
        public int DistributorId { get; set; }
    }
}
