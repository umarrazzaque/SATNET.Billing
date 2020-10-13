using SATNET.WebApp.Helpers;
using SATNET.WebApp.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class HardwareComponentModel : BaseModel
    {
        [DisplayName("Hardware Type")]
        [Required(ErrorMessage = "Hardware Type field is required!")]
        public int HardwareTypeId { get; set; }
        public string HardwareType { get; set; }
        [DisplayName("Hardware Component")]
        [Required(ErrorMessage = "Hardware Component field is required!")]
        public string HCValue { get; set; }
        [DisplayName("Hardware Spare Type")]
        public int? HCSpareTypeId { get; set; }
        public string HCSpareType { get; set; }
    }

    public class CreateHardwareComponentModel : BaseModel
    {
        public CreateHardwareComponentModel()
        {
            HardwareComponentModel = new HardwareComponentModel();
            HardwareTypes = new List<LookUpModel>();
            SpareTypes = new List<LookUpModel>();
        }
        public HardwareComponentModel HardwareComponentModel { get; set; }
        public List<LookUpModel> HardwareTypes { get; set; }
        public List<LookUpModel> SpareTypes { get; set; }
    }

    
}
