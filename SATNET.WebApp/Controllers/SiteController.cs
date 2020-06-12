using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    public class SiteController : BaseController
    {
        private readonly IService<Site> _siteService;
        public SiteController(IService<Site> siteService)
        {
            _siteService = siteService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetPackagesList());
        }

        public async Task<IActionResult> Reset()
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", await GetPackagesList()) });
        }
        public IActionResult Add()
        {
            CreateSiteModel siteModel = new CreateSiteModel
            {
                SiteModel = new SiteModel(),
                SiteStatus = GetSiteStatusList()
            };

            return Json(new { isValid = true, html = RenderViewToString(this, "Add", siteModel) });
        }


        [HttpPost]
        public async Task<IActionResult> Add(CreateSiteModel createSiteModel)
        {
            throw new NotImplementedException();

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            throw new NotImplementedException();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateSiteModel createSiteModel)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<List<SiteModel>> GetPackagesList()
        {
            throw new NotImplementedException();
        }

        private List<LookUpModel> GetSiteStatusList()
        {
            throw new NotImplementedException();
        }
    }
}