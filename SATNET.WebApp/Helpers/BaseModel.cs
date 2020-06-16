using SATNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Helpers
{
    public class BaseModel
    {
        public BaseModel()
        {   
        }
        public int Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
    }
}
