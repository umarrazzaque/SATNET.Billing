using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Package : BaseEntity
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public decimal Speed { get; set; }
        public string Type { get; set; }
    }
}
