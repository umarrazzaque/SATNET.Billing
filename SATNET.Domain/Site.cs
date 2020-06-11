using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Site : BaseEntity
    {
        
        public string Name { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string City { get; set; }
        public string Area { get; set; }
        public string  Subscriber{ get; set; }
        public DateTime ActivatedDate { get; set; }
    }
}
