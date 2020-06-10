using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Reseller: BaseEntity
    {
        public string RName { get; set; }
        public int RTypeId { get; set; }
        public string RType { get; set; }
        public string REmail { get; set; }
        public string RAddress { get; set; }
        public string RContactNumber { get; set; }
    }
}
