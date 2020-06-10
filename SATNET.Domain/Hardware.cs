using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Hardware : BaseEntity
    {
        public string HKit { get; set; }
        public string Antenna { get; set; }
        public string Modem { get; set; }
        public string Transceiver { get; set; }
    }
}
