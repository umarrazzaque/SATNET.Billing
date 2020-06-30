
﻿using System;
using System.Collections.Generic;
using System.Text;


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
