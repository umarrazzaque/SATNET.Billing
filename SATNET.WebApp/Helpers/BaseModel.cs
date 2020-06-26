using SATNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        [DisplayName("Created On")]
        public DateTime? CreatedOn { get; set; }
        [DisplayName("CreatedBy")]
        public int CreatedBy { get; set; }
        [DisplayName("Last Modified Date")]
        public DateTime? UpdatedOn { get; set; }
        [DisplayName("Last Modified By")]
        public int UpdatedBy { get; set; }
        [DisplayName("Deleted On")]
        public DateTime? DeletedOn { get; set; }
        [DisplayName("Deleted By")]
        public int DeletedBy { get; set; }
        [DisplayName("Active")]
        public bool isDormant
        {
            get; set;
        }
        [DisplayName("Brief Description")]
        public string BriefDescription { get; set; }
    }
}
