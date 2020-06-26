<<<<<<< HEAD
﻿namespace SATNET.Domain
{
    public class ServicePlan: BaseEntity
    {
        public string Name { get; set; }
        public int PlanTypeId { get; set; }
        public string PlanName { get; set; }
        public decimal DownloadMIR { get; set; }
        public decimal UploadMIR { get; set; }
        public decimal DownloadCIR { get; set; }
        public decimal UploadCIR { get; set; }
    }
}
=======
﻿using System;
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
>>>>>>> devUmerKhalid
