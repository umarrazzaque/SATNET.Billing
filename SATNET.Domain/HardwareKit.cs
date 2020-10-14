using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class HardwareKit : BaseEntity
    {
        public string KitName { get; set; }
        public int ModemModelId { get; set; }
        public string ModemModel { get; set; }
        public int AntennaMeterId { get; set; }
        public string AntennaMeter { get; set; }
        public int PowerWATT { get; set; }
        public int NPRMPieces { get; set; }
        public int RG6 { get; set; }
        public int ConnectorPieces { get; set; }
    }
}
