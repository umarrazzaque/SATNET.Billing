using SATNET.WebApp.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class PromotionViewModel : BaseModel
    {
        [Required(ErrorMessage = "Promotion is required")]
        public string Name { get; set; }
    }
}
