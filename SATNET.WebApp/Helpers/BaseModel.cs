using SATNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Helpers
{
    public class BaseModel
    {
        public BaseModel()
        {
            MenuModel = new MenuModel();
        }
        public MenuModel MenuModel { get; set; }
    }
}
