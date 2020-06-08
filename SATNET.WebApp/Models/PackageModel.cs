using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using SATNET.Service;
namespace SATNET.WebApp.Models
{
    public class PackageModel : BaseModel
    {
        public PackageModel()
        {
            PackageId = 0;
            Name = PackageType = "";
            Rate = Speed = 0;
        }
        public int PackageId { get; set; }
        [DisplayName("Package Name")]
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public decimal Speed { get; set; }
        public decimal Price { get; set; }
        [DisplayName("Type")]
        public string PackageType { get; set; }
        public SelectList PackageTypeList { get; set; }
    }

    public class CreatePackageModel: BaseModel
    {
        public CreatePackageModel()
        {
            PackageModel = new PackageModel();
            PackageTypesList = new List<PackageTypeModel>();
        }
        public PackageModel PackageModel { get; set; }
        public IList<PackageTypeModel> PackageTypesList { get; set; }
    }
    
    public class PackageModelList : BaseModel
    {
        public PackageModelList()
        {
            PackageModels = new List<PackageModel>();
        }
        public List<PackageModel> PackageModels  { get; set; }
    }

   

    


}
