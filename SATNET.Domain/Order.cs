using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Order : BaseEntity
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int HardwareId { get; set; }
        public string HardwareName { get; set; }
        public int PackageId { get; set; }
        public string PackageName { get; set; }
        public int RequestTypeId { get; set; }
        public string RequestTypeName { get; set; }
        public DateTime? UpgradeFrom { get; set; }
        public DateTime? UpgradeTo{ get; set; }
        public DateTime? DowngradeFrom { get; set; }
        public DateTime? DowngradeTo { get; set; }
        public DateTime? InstallationDate { get; set; }
        public int PlanTypeId { get; set; }
        public string IP { get; set; }
        public int? Download { get; set; }
        public int? Upload { get; set; }
    }
}
