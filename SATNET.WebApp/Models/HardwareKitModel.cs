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
    public class HardwareKitModel : BaseModel
    {
        [DisplayName("Kit Name")]
        [Required(ErrorMessage = "Kit Name field is required")]
        public string KitName { get; set; }
        [DisplayName("Modem Model")]
        [Required(ErrorMessage = "Modem Model field is required")]
        public int ModemModelId { get; set; }
        public string ModemModel { get; set; }
        [DisplayName("Antenna Meters")]
        [Required(ErrorMessage = "Antenna Meters field is required")]
        public int AntennaMeterId { get; set; }
        public string AntennaMeter { get; set; }
        [DisplayName("BUC(WATT)")]
        [Required(ErrorMessage = "BUC(WATT) field is required")]
        public int PowerWATT { get; set; }
        [DisplayName("NPRM Pieces")]
        [Required(ErrorMessage = "NPRM Pieces field is required")]
        public int NPRMPieces { get; set; }
        [DisplayName("RG6 Cable")]
        [Required(ErrorMessage = "RG6 Cable field is required")]
        public int RG6 { get; set; }
        [DisplayName("Connector Pieces")]
        [Required(ErrorMessage = "Connector Pieces field is required")]
        public int ConnectorPieces { get; set; }
    }
    public class CreateHardwareKitModel : BaseModel
    {
        public CreateHardwareKitModel()
        {
            HardwareKitModel = new HardwareKitModel();
            ModemModels = new List<HardwareComponentModel>();
            AntennaMeters = new List<HardwareComponentModel>();
        }
        public HardwareKitModel HardwareKitModel { get; set; }
        public List<HardwareComponentModel> ModemModels { get; set; }
        public List<HardwareComponentModel> AntennaMeters { get; set; }
    }
}
