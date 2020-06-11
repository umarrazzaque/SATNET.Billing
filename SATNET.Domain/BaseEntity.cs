using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool isDormant
        {
            get { return Dormant == 1 ? true : false; }
            set { Dormant = value ? 1 : 0; }
        }
        public int Dormant { get; set; }
        public string BriefDescription { get; set; }



        public String SearchBy { get; set; }
        public String Keyword { get; set; }
        public String Flag { set; get; }
        public String OrderBy { get; set; }
        public String SortOrder { get; set; }
    }
}
