using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.Service
{
    public class StatusModel
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public bool IsSuccess { get; set; }
        public string Html { get; set; }
        public string ResponseUrl { get; set; }
    }
}
