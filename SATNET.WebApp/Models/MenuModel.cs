using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class MenuModel
    {
        public MenuModel()
        {
            Heading = SubHeading = "";
            BreadCrums = new List<MenuModel>();
        }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public List<MenuModel> BreadCrums { get; set; }
    }
}
