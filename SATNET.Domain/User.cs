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
        public string Contact { get; set; }
        public string Email { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<string> Roles { get; set; }
    }
}