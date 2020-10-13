using SATNET.WebApp.Helpers;
using SATNET.WebApp.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Hardware
{
    public class HardwareComponentRegistrationModel : BaseModel
    {
        [DisplayName("Registration Number")]
        public string HCRegistrationNumber { get; set; }
        [DisplayName("MAC Address")]
        public string SerialNumber { get; set; }
        [DisplayName("Hardware Type")]
        [Required(ErrorMessage = "Hardware Type field is required!")]
        public int HardwareTypeId { get; set; }
        public string HardwareType { get; set; }
        [DisplayName("Hardware Component")]
        public int HardwareComponentId { get; set; }
        public string HardwareComponent { get; set; }
        [DisplayName("Customer")]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        [DisplayName("Registered")]
        public bool IsRegistered { get; set; }
        public string[] SerialNumbers { get; set; }
    }
    public class CreateHardwareComponentRegistrationModel : BaseModel
    {
        public CreateHardwareComponentRegistrationModel()
        {
            HardwareComponentRegistrationModel = new HardwareComponentRegistrationModel();
            HardwareComponentList = new List<HardwareComponentModel>();
            HardwareTypes = new List<LookUpModel>();
        }
        public HardwareComponentRegistrationModel HardwareComponentRegistrationModel { get; set; }
        public List<HardwareComponentModel> HardwareComponentList { get; set; }
        public List<LookUpModel> HardwareTypes { get; set; }
        public List<CustomerModel> CustomerList { get; set; }
    }
}
