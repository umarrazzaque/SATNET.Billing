using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class PackageModel : BaseModel
    {
        public PackageModel()
        {
            PackageId = 0;
            PackageName = PackageType = "";
            Rate = Speed = 0;
            PackageTypeList = new SelectList(
             new List<SelectListItem>
             {
                new SelectListItem {Text = "Package 1", Value = "1"},
                new SelectListItem {Text = "Package 2", Value = "2"},
             }, "Value", "Text");
        }
        public int PackageId { get; set; }
        [DisplayName("Package Name")]
        public string PackageName { get; set; }
        public decimal Rate { get; set; }
        public decimal Speed { get; set; }
        [DisplayName("Type")]
        public string PackageType { get; set; }
        public SelectList PackageTypeList { get; set; }
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
