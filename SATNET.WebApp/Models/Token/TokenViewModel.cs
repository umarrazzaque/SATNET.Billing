using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Token
{
    public class TokenViewModel : BaseModel
    {
        [Required(ErrorMessage = "Token is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Validity is required")]
        public int Validity { get; set; }
    }
}
