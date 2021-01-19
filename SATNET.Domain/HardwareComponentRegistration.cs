using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class HardwareComponentRegistration : BaseEntity
    {
        public string SerialNumber { get; set; }
        public string AIRMAC { get; set; }
        public int HardwareComponentId { get; set; }
        public int HardwareTypeId { get; set; }
        public string HardwareComponent { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool IsUsed { get; set; }
        public int HardwareConditionId { get; set; }
        public string [] SerialNumbers { get; set; }
        public string[] AIRMACs { get; set; }
    }
}
