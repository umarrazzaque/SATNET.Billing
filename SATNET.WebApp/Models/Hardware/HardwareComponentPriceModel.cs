using SATNET.Domain;
using SATNET.WebApp.Helpers;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.WebApp.Models.Lookup;

namespace SATNET.WebApp.Models.Hardware
{
    public class HardwareComponentPriceModel : BaseModel
    {
        public int HardwareComponentId { get; set; }
        public string HCValue { get; set; }
        public int PlanTypeId { get; set; }
        public string PlanType { get; set; }
        public int? HCSpareTypeId { get; set; }
        public string HCSpareType { get; set; }
        public int PriceTierId { get; set; }
        public string PriceTierName { get; set; }
        public decimal Price { get; set; }
    }
    public class CreateHardwareComponentPriceModel : BaseModel
    {
        public CreateHardwareComponentPriceModel()
        {
            HardwareComponentModel = new HardwareComponentModel();
            HardwareComponentPriceModel = new HardwareComponentPriceModel();
            HardwareComponentPriceList = new List<HardwareComponentPriceModel>();
            PriceTierList = new List<LookUpModel>();
        }
        public HardwareComponentModel HardwareComponentModel { get; set; }
        public HardwareComponentPriceModel HardwareComponentPriceModel { get; set; }
        public List<HardwareComponentPriceModel> HardwareComponentPriceList { get; set; }
        public List<LookUpModel> PriceTierList { get; set; }
    }
}
