using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class CodeErrorLog : BaseEntity
    {
        public string Details { get; set; }
        public string Module { get; set; }
        public string ClassName { get; set; }
        public string MethodName { get; set; }
    }
}
