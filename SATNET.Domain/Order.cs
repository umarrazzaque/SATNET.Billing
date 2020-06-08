using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Order : BaseEntity
    {
        public int SiteId { get; set; }
        public int PackageId { get; set; }
        public int RequestTypeId { get; set; }
        public Customer Customer { get; set; }
    }
}
