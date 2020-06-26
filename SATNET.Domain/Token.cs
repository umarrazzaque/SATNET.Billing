using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class Token : BaseEntity
    {
        public string Name { get; set; }
        public int Validity { get; set; }
    }
}
