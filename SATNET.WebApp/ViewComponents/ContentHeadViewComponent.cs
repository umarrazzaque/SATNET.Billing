using Microsoft.AspNetCore.Mvc;
using SATNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.ViewComponents
{
    [ViewComponent(Name = "ContentHead")]
    public class ContentHeadViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(MenuModel model)
        {
            
            return View("ContentHead", model);
        }
    }
}
