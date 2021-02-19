using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class SystemLogViewModel : BaseModel
    {
        public int EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Description { get; set; }
        public string CustomerName { get; set; }
        public SelectList CustomerSelectList { get; set; }
    }
}
