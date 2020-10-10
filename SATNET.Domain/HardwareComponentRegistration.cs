﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class HardwareComponentRegistration : BaseEntity
    {
        public string HCRegistrationNumber { get; set; }
        public string SerialNumber { get; set; }
        public int HardwareComponentId { get; set; }
        public string HardwareComponent { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public bool IsRegistered { get; set; }
    }
}
