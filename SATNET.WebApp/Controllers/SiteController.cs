using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Helpers;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Lookup;

namespace SATNET.WebApp.Controllers
{
    [Authorize(Policy = "ReadOnlySitePolicy")]
    public class SiteController : Base2Controller
    {
        private readonly IService<Site> _siteService;
        private readonly IService<Promotion> _promotionService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        public SiteController(IService<Site> siteService, UserManager<ApplicationUser> userManager, IMapper mapper, IService<Lookup> lookUpService
            , IService<Customer> customerService, IService<Promotion> promotionService) : base(customerService, userManager)
        {
            _siteService = siteService;
            _mapper = mapper;
            _lookUpService = lookUpService;
            _promotionService = promotionService;
        }
        public async Task<IActionResult> Index()
        {
            int customerId = await GetCustomerId();
            if (customerId == 0)
            {
                var customers = await GetCustomerList(new Customer());
                ViewBag.CustomerSelectList = new SelectList(customers, "Id", "Name");
            }
            ViewBag.CustomerId = customerId;
            var statuses = await _lookUpService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.SiteStatus) });
            ViewBag.SiteStatusSelectList = new SelectList(statuses, "Id", "Name");
            var promotions = await _promotionService.List(new Promotion());
            ViewBag.PromotionSelectList = new SelectList(promotions, "Id", "Name");
            return View(await GetSiteList(customerId, 0,0));
        }

        public async Task<IActionResult> Reset()
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", await GetSiteList(await GetCustomerId(), 0,0)) });
        }
        public IActionResult Add()
        {
            var userName = User.FindFirstValue(ClaimTypes.Name);
            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var resultModel = new CreateSiteModel
            {
                SiteModel = new SiteModel {
                    Name = GetLoggedInUserName3(userName).ToUpper() + '-' + GetNumber(GetSiteCount() + 1),
                    CustomerId = userId
                },
                SiteStatus = GetSiteStatusList().Result
            };
            return View(resultModel);
            //return Json(new { isValid = true, html = RenderViewToString(this, "Add", siteModel) });
        }

        private int GetSiteCount()
        {
            int totalCount = 1;
            var serviceResult = _siteService.List(new Site() { Flag = "GET_SITE_COUNT"}).Result;
            if (serviceResult.Any()){
                totalCount = serviceResult.FirstOrDefault().RecordsCount;
            }
            return totalCount;
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateSiteModel createReturnModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = "/Site/Index" };
            var retModel = createReturnModel.SiteModel;
            retModel.CreatedBy = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ModelState.IsValid)
            {
                Site obj = _mapper.Map<Site>(retModel);
                statusModel = await _siteService.Add(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            //statusModel.Html = RenderViewToString(this, "Index", await GetSiteList());
            return Json(statusModel);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Site site = await _siteService.Get(id);
            var resultModel = new CreateSiteModel
            {
                SiteModel = _mapper.Map<SiteModel>(await _siteService.Get(id)),
                SiteStatus = GetSiteStatusList().Result
            };
            //var status = new StatusModel
            //{
            //    IsSuccess = true,
            //    Html = RenderViewToString(this, "Edit", siteModel)
            //};
            //return Json(status);
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateSiteModel createReturnModel)
        {
            Site obj = _mapper.Map<Site>(createReturnModel.SiteModel);
            var statusModel = await _siteService.Update(obj);
            return Json(statusModel);
            //status.Html = RenderViewToString(this, "Index", await GetSiteList());
            //return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Site obj = await _siteService.Get(id);
            SiteModel retModel = _mapper.Map<SiteModel>(obj);
            ViewBag.SiteId = id;
            //var status = new StatusModel
            //{
            //    IsSuccess = true,
            //    Html = RenderViewToString(this, "Details", siteModel)
            //};
            //return Json(status);
            return View(retModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = _siteService.Delete(id, 1).Result;
            statusModel.Html = RenderViewToString(this, "Index", await GetSiteList(await GetCustomerId(), 0,0));
            return Json(statusModel);
        }

        private async Task<List<SiteModel>> GetSiteList(int customerId, int siteStatusId, int promotionId)
        {
            //PackageModelList packageList = new PackageModelList();
            //packageList.MenuModel = SetLayoutContent(heading: "Site",subHeading: "Listing");

            var retList = new List<SiteModel>();
            var serviceResult = await _siteService.List(new Site() { CustomerId = customerId, StatusId=siteStatusId, PromotionId=promotionId});
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<SiteModel>>(serviceResult);
            }
            return retList;
        }

        private async Task<List<LookUpModel>> GetSiteStatusList()
        {
            List<LookUpModel> retListModel = new List<LookUpModel>();
            var retList = await _lookUpService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.SiteStatus) });
            if (retList.Any())
            {
                retListModel = _mapper.Map<List<LookUpModel>>(retList);

            }
            return retListModel;
        }

        private string GetLoggedInUserName3(string userName)
        {
            int maxLength = userName.Length >= 3 ? 3 : userName.Length;
            var ret = userName.Substring(0, maxLength);
            return ret;
        }
        private string GetNumber(int to)
        {
            var oneZero = "0";
            var twoZero = "00";
            var retValue = "";
            if (to < 10)
            {
                retValue = twoZero + to;
            }
            else if (to >= 10 && to < 99)
            {
                retValue = oneZero + to;
            }
            return retValue;
        }

        //private List<LookUpModel> GetSiteStatusList()
        //{
        //    List<LookUpModel> siteStatusList = new List<LookUpModel>
        //    {
        //        new LookUpModel{ Id = 1, Name = "Active"},
        //        new LookUpModel{ Id = 2, Name = "Lock"},
        //        new LookUpModel{ Id = 3, Name = "Terminated"}
        //    };
        //    return siteStatusList;
        //}

        public async Task<IActionResult> GetSiteListAjax(int customerId, int statusId, int promotionId)
        {
            var model = await GetSiteList(customerId, statusId, promotionId);
            return PartialView("_Site", model);
        }

        public async Task<IActionResult> Excel(int siteId)
        {
            string fileName;
            //List<SiteModel> sites = new List<SiteModel>();
            Site obj = await _siteService.Get(siteId);
            //SiteModel model = _mapper.Map<SiteModel>(obj);
            fileName = obj.Name + ".xlsx";
            //sites.Add(model);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Site Detail");
                //ws.Cells["A1"].LoadFromCollection(sites, true);
                // Load your collection site detail
                ws.Cells[1, 1].Value = "Name";
                ws.Cells[1, 2].Value = "Status";
                ws.Cells[1, 3].Value = "City";
                ws.Cells[1, 4].Value = "Area";
                ws.Cells[1, 5].Value = "Subscriber";
                ws.Cells[1, 6].Value = "Promotion";
                ws.Cells[1, 7].Value = "Service Plan Type";
                ws.Cells[1, 8].Value = "Service Plan";
                ws.Cells[1, 9].Value = "Modem Model";
                ws.Cells[1, 10].Value = "AIRMAC";
                ws.Cells[1, 11].Value = "IP";
                ws.Cells[1, 12].Value = "Planned Activation Date";
                ws.Cells[1, 13].Value = "Last Modified Date";
                ws.Cells[1, 14].Value = "Next Billing Date";
                ws.Cells[1, 1, 1, 20].Style.Font.Bold = true;

                ws.Cells[2, 1].Value = obj.Name;
                ws.Cells[2, 2].Value = obj.Status;
                ws.Cells[2, 3].Value = obj.City;
                ws.Cells[2, 4].Value = obj.Area;
                ws.Cells[2, 5].Value = obj.SubscriberName;
                ws.Cells[2, 6].Value = obj.Promotion;
                ws.Cells[2, 7].Value = obj.ServicePlanType;
                ws.Cells[2, 8].Value = obj.ServicePlan;
                ws.Cells[2, 9].Value = obj.ModemModel;
                ws.Cells[2, 10].Value = obj.AirMac;
                ws.Cells[2, 11].Value = obj.IPName;
                ws.Cells[2, 12].Value = obj.ActivationDate.ToShortDateString();
                ws.Cells[2, 13].Value = obj.UpdatedOn;
                ws.Cells[2, 14].Value = obj.NextBillingDate.ToShortDateString();

                Byte[] fileBytes = pck.GetAsByteArray();

                return File(fileBytes, "application/vnd.ms-excel", fileName);
            }
        }
    }
}