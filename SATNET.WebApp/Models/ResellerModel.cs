using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class ResellerModel
    {
        public ResellerModel()
        {
            Name = Type = Email = Address = ContactNumber = "";
            TypeId = Id = -1;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public string  Code { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        [DisplayName("Contact Number")]
        public string ContactNumber { get; set; }
    }

    public class CreateResellerModel
    {
        public CreateResellerModel()
        {
            ResellerModel = new ResellerModel();
            ResellerType = new List<LookUpModel>();
        }
        public ResellerModel ResellerModel { get; set; }
        public List<LookUpModel> ResellerType { get; set; }
    }
}
