using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Controllers
{
    public class SharedController : Controller
    {
        private readonly IService<Site> _siteService;
        public SharedController(IService<Site> siteService)
        {
            _siteService = siteService;
        }
        public async Task<IActionResult> GetSiteSelectList(int customerId)
        {
            Site site = new Site()
            {
                CustomerId = customerId
            };
            var sites = await _siteService.List(site);
            return Json(new SelectList(sites, "Id", "Name"));
        }

    }
}
