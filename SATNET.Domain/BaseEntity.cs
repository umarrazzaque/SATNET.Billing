using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string CreatedByRole { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool isDormant
        {
            get { return isDeleted == 1 ? true : false; }
            set { isDeleted = value ? 1 : 0; }
        }
        public int isDeleted { get; set; }
        public string BriefDescription { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int DeletedBy { get; set; }


        public String SearchBy { get; set; }
        public String Keyword { get; set; }
        public String Flag { set; get; }
        public String OrderBy { get; set; }
        public String SortOrder { get; set; }

        public int RecordsCount { get; set; }
        public string ErrorNumber { get; set; }
        public string ErrorSeverity { get; set; }
        public string ErrorState { get; set; }
        public string ErrorProcedure { get; set; }
        public string ErrorLine { get; set; }
        public string ErrorMessage { get; set; }
    }
}
