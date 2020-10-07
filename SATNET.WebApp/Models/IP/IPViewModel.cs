using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.IP
{
    public class IPViewModel : BaseModel
    {
        [Required(ErrorMessage = "IP is required")]
        public string Name { get; set; }
        public string Subnet { get; set; }
        public string IPs { get; set; }
        public string Hosts { get; set; }
        public int IPTypeId { get; set; }
    }
}
