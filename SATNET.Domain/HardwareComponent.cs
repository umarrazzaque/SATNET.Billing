using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class HardwareComponent : BaseEntity
    {
        public int HardwareTypeId { get; set; }
        public string HardwareType { get; set; }
        public string HCValue { get; set; }
        public int? HCSpareTypeId { get; set; }
        public string HCSpareType { get; set; }
    }
}
