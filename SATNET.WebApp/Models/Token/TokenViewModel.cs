using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Token
{
    public class TokenViewModel : BaseModel
    {
        public string Name { get; set; }
        public int Validity { get; set; }
    }
}
