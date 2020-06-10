using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class HardwareModel
    {
        public HardwareModel()
        {
            HardwareId = -1;
            HKit = Antenna = Modem = Transceiver = "";
        }
        public int HardwareId { get; set; }
        public string HKit { get; set; }
        public string Antenna { get; set; }
        public string Modem { get; set; }
        public string Transceiver { get; set; }
    }

}
