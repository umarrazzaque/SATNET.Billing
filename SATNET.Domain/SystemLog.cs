using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class SystemLog : BaseEntity
    {
        public int EntityTypeId { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public string Description { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
    }
}
