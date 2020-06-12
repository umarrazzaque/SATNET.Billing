using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Order : BaseEntity
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int PackageId { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public string CustomerName { get; set; }
        public string PackageName { get; set; }
        public string RequestType { get; set; }
        public int HardwareId { get; set; }
        public string HardwareName { get; set; }
        public int CustomerId { get; set; }
    }
}
