using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    class Package : BaseEntity
    {
        public string PackageName { get; set; }
        public decimal Rate { get; set; }
        public decimal Speed { get; set; }
        public string PackageType { get; set; }
    }
}
