using SATNET.WebApp.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            Name = Code = Email = Address = ContactNumber = Notes = "";
            PriceTierId = TypeId = Id = -1;
        }
        public int Id { get; set; }

        public string Name { get; set; }
        public string Code { get; set; }
        [DisplayName("Customer Type")]
        public int TypeId { get; set; }
        public string CustomerType { get; set; }
        [DisplayName("Price Tier Type")]
        public int PriceTierId { get; set; }
        public string PriceTierName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Notes { get; set; }
    }

    public class CreateCustomerModel
    {
        public CreateCustomerModel()
        {
            CustomerModel = new CustomerModel();
            CustomerType = new List<LookUpModel>();
            PriceTier = new List<LookUpModel>();
        }
        public CustomerModel CustomerModel { get; set; }
        public List<LookUpModel> CustomerType { get; set; }
        public List<LookUpModel> PriceTier { get; set; }
    }
}
