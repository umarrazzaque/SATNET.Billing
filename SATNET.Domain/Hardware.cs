using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Hardware : BaseEntity
    {
        public string ModemModel { get; set; }
        public string ModemSerialNo { get; set; }
        public string MACAirNo { get; set; }
        public string AntennaSize { get; set; }
        public string AntennaSrNo { get; set; }
        public string TransceiverWAAT { get; set; }
        public string TransceiverSrNo { get; set; }
        public decimal Price { get; set; }
    }
}
