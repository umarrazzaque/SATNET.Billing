using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class ServicePlan : BaseEntity
    {
        public int PlanTypeId { get; set; }
        public string Name { get; set; }
        public string DownloadMIR { get; set; }
        public string UploadMIR { get; set; }
        public int DownloadCIR { get; set; }
        public int UploadCIR { get; set; }
    }
}
