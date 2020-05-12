using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool Dormant { get; set; }
    }
}
