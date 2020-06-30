using SATNET.WebApp.Helpers;
using SATNET.WebApp.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class ServicePlanModel : BaseModel
    {
        public ServicePlanModel()
        {

        }
        public string Name { get; set; }
        [DisplayName("Plan Type")]
        public int PlanTypeId { get; set; }
        public string PlanType { get; set; }
        [DisplayName("Download MIR")]
        public decimal DownloadMIR { get; set; }
        [DisplayName("Upload MIR")]
        public decimal UploadMIR { get; set; }
        [DisplayName("Download CIR")]
        public decimal DownloadCIR { get; set; }
        [DisplayName("Upload CIR")]
        public decimal UploadCIR { get; set; }
    }
    public class CreateServicePlanModel : BaseModel
    {
        public CreateServicePlanModel()
        {
            ServicePlanModel = new ServicePlanModel();
            PlanType = new List<LookUpModel>();
        }
        public ServicePlanModel ServicePlanModel { get; set; }
        public List<LookUpModel> PlanType { get; set; }
    }
}
