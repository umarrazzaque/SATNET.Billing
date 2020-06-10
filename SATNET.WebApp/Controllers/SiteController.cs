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
        private readonly IServices<Site> _siteService;
        public SiteController(IServices<Site> siteService)
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
            SiteModel siteModel = createSiteModel.SiteModel;
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Site/Index" };
            if (ModelState.IsValid)
            {
                status = _siteService.Add(new Site
                {
                    Id = 0,
                    SiteName = siteModel.SiteName,
                    SiteStatusId = siteModel.SiteStatusId,
                    Area = siteModel.Area,
                    City = siteModel.City,
                    CreatedBy = 1
                }).Result;
                //if (status.IsSuccess)
                //{
                //    //get error code description from configuration
                //    //status.ErrorCode = "";
                //    //status.Data = RenderViewToString(this, "Index", await GetPackagesList());
                //}
                //else
                //{
                //    //status.Data = RenderViewToString(this, "Index", await GetPackagesList());
                //}
                //return Json(new { isValid = false, msg = "Error in inserting the record!", html = RenderViewToString(this, "Index", await GetPackagesList()) });
            }
            else
            {
                status.ErrorCode = "Error occured see entity validation errors.";
            }
            status.Html = RenderViewToString(this, "Index", await GetPackagesList());
            return Json(status);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Site site = await _siteService.Get(id);
            SiteModel pacModel = new SiteModel
            {
                SiteId = site.Id,
                SiteName = site.SiteName,
                SiteStatusId = site.SiteStatusId,
                Area = site.Area,
                City = site.City
            };
            CreateSiteModel siteModel = new CreateSiteModel
            {
                SiteModel = pacModel,
                SiteStatus = GetSiteStatusList()
            };
            var status = new StatusModel
            {
                IsSuccess = true,
                Html = RenderViewToString(this, "Edit", siteModel)
            };
            return Json(status);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateSiteModel createSiteModel)
        {
            SiteModel siteModel = createSiteModel.SiteModel;
            var status = _siteService.Update(new Site
            {
                Id = siteModel.SiteId,
                SiteName = siteModel.SiteName,
                SiteStatusId = siteModel.SiteStatusId,
                Area = siteModel.Area,
                City = siteModel.City,
                CreatedBy = 1
            }).Result;
            status.Html = RenderViewToString(this, "Index", await GetPackagesList());
            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Site site = await _siteService.Get(id);
            SiteModel siteModel = new SiteModel
            {
                SiteId = site.Id,
                SiteName = site.SiteName,
                SiteStatusId = site.SiteStatusId,
                Area = site.Area,
                City = site.City
            };
            var status = new StatusModel
            {
                IsSuccess = true,
                Html = RenderViewToString(this, "Details", siteModel)
            };
            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var status = _siteService.Delete(id, 1).Result;
            status.Html = RenderViewToString(this, "Index", await GetPackagesList());
            return Json(status);
        }

        private async Task<List<SiteModel>> GetPackagesList()
        {
            //PackageModelList packageList = new PackageModelList();
            //packageList.MenuModel = SetLayoutContent(heading: "Site",subHeading: "Listing");

            List<SiteModel> siteListModel = new List<SiteModel>();
            var serviceResult = await _siteService.List();
            if (serviceResult.Any())
            {
                serviceResult.ForEach(site =>
                {
                    SiteModel siteModel = new SiteModel()
                    {
                        SiteId = site.Id,
                        SiteName = site.SiteName,
                        SiteStatusId = site.SiteStatusId,
                        Area = site.Area,
                        City = site.City
                    };
                    siteListModel.Add(siteModel);
                });
            }
            return siteListModel;
        }

        private List<LookUpModel> GetSiteStatusList()
        {
            List<LookUpModel> siteStatusList = new List<LookUpModel>
            {
                new LookUpModel{ Id = 1, Name = "Active"},
                new LookUpModel{ Id = 2, Name = "Lock"},
                new LookUpModel{ Id = 3, Name = "Terminated"}
            };
            return siteStatusList;
        }
    }
}