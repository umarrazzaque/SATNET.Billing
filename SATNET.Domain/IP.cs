using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class IP:BaseEntity
    {
        public string Name { get; set; }
        public string Subnet { get; set; }
        public string IPs { get; set; }
        public string Hosts { get; set; }
        public int IPTypeId { get; set; }
    }
}
