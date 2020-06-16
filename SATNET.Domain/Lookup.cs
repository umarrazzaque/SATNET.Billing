using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Lookup : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int LookupTypeId { get; set; }
    }
}
