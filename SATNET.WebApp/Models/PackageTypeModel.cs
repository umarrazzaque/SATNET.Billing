using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class PackageTypeModel: BaseModel
    {
        public PackageTypeModel()
        {
            PackageTypeId = -1;
            PackageTypeName = "";
        }
        public int PackageTypeId { get; set; }
        public string PackageTypeName { get; set; }
    }
}
