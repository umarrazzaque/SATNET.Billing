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
            get { return isActive == 1 ? true : false; }
            set { isActive = value ? 1 : 0; }
        }
        public int isActive { get; set; }
        public string BriefDescription { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int DeletedBy { get; set; }


        public String SearchBy { get; set; }
        public String Keyword { get; set; }
        public String Flag { set; get; }
        public String OrderBy { get; set; }
        public String SortOrder { get; set; }
    }
}
