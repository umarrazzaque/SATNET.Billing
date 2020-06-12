using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class HardwareModel
    {
        public HardwareModel()
        {
            HardwareId = -1;
            ModemModel = ModemSerialNo = MACAirNo = AntennaSize = AntennaSrNo = TransceiverWAAT = TransceiverSrNo ="";
        }
        public int HardwareId { get; set; }
        [DisplayName("Modem Model")]
        public string ModemModel { get; set; }
        [DisplayName("Modem Sr No")]
        public string ModemSerialNo { get; set; }
        [DisplayName("MAC Air No")]
        public string MACAirNo { get; set; }
        [DisplayName("Antenna Size")]
        public string AntennaSize { get; set; }
        [DisplayName("Antenna Sr No")]
        public string AntennaSrNo { get; set; }
        [DisplayName("Transciever WAAT")]
        public string TransceiverWAAT { get; set; }
        [DisplayName("Transciever Sr No")]
        public string TransceiverSrNo { get; set; }
        public decimal Price { get; set; }
    }
    /*
     curl -H "Accept: application/json" "Content-Type: application/json" -X POST "http://10.2.0.50/rest/modem/USatcom/API001/lock"
     */
}
