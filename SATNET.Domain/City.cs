using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public int AreaId { get; set; }
        public string AreaName { get; set; }
    }
}
