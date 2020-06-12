using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Site : BaseEntity
    {
        public int DistributorId { get; set; }
        public string Name { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int PlanTypeId { get; set; }
        public string PlanTypeName { get; set; }
        public int HardwareId { get; set; }
        public string HardwareName { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public string IP { get; set; }
        public int Download { get; set; }
        public int Upload { get; set; }
        public DateTime? InstallationDate { get; set; }
        public int CustomerId { get; set; }
    }
}
