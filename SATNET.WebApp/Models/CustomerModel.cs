using SATNET.WebApp.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class CustomerModel
    {
        public CustomerModel()
        {
            Name = Code = ShortName = Email = Address = ContactNumber = Notes = "";
            PriceTierId = TypeId = Id = -1;
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Name field is required!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Code field is required!")]
        [MaxLength(3, ErrorMessage = "Code cannot be greater than 3")]
        public string Code { get; set; }
        [DisplayName("Short Name")]
        [Required(ErrorMessage = "Short Name field is required!")]
        public string ShortName { get; set; }
        [DisplayName("Customer Type")]
        public int TypeId { get; set; }
        public string CustomerType { get; set; }
        [DisplayName("Price Tier Type")]
        [Required(ErrorMessage = "Price Tier field is required!")]
        public int PriceTierId { get; set; }
        [DisplayName("Price Tier")]
        public string PriceTierName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        [DisplayName("Contact Number")]
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
