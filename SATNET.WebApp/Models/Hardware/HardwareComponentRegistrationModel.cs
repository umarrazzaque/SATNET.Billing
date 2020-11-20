using Microsoft.AspNetCore.Mvc.Rendering;
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
        [DisplayName("Serial Number")]
        public string SerialNumber { get; set; }
        [DisplayName("MAC Address")]
        public string AIRMAC { get; set; }
        [DisplayName("Hardware Type")]
        [Required(ErrorMessage = "Hardware Type field is required!")]
        public int HardwareTypeId { get; set; }
        public string HardwareType { get; set; }
        [DisplayName("Hardware Component")]
        public int HardwareComponentId { get; set; }
        public string HardwareComponent { get; set; }
        [DisplayName("Customer")]
        [Required(ErrorMessage ="Required Field")]
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool IsUsed { get; set; }
        public string[] SerialNumbers { get; set; }
        public string[] AIRMACs { get; set; }
        public SelectList CustomerSelectList { get; set; }
        public SelectList ModemSelectList { get; set; }
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
