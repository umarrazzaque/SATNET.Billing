using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Helpers;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Order;

namespace SATNET.WebApp.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IService<Customer> _customerService;
        private readonly IService<Order> _orderService;
        private readonly IService<Lookup> _lookupService;
        private readonly IService<Hardware> _hardwareService;
        private readonly IService<ServicePlan> _servicePlanService;
        private readonly IService<Site> _siteService;
        private readonly IService<Token> _tokenService;
        private readonly IService<Promotion> _promotionService;
        private readonly IService<IP> _ipService;
        private readonly IService<City> _cityService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(IService<Customer> customerService, IService<Order> orderService, IService<Hardware> hardwareService, IService<ServicePlan> servicePlanService, IService<Site> siteService
            , IService<Lookup> lookupService, IService<Token> tokenService, IService<Promotion> promotionService, IService<IP> ipService, IMapper mapper
            , UserManager<ApplicationUser> userManager, IService<City> cityService)
        {
            _customerService = customerService;
            _orderService = orderService;
            _hardwareService = hardwareService;
            _servicePlanService = servicePlanService;
            _siteService = siteService;
            _lookupService = lookupService;
            _tokenService = tokenService;
            _promotionService = promotionService;
            _ipService = ipService;
            _mapper = mapper;
            _userManager = userManager;
            _cityService = cityService;
        }
        [Authorize(Policy = "ReadOnlyServiceOrderPolicy")]
        public async Task<IActionResult> Index()
        {
            //var requestTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.OrderRequestType) });
            var orderStatuses = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.OrderStatus) });

            //ViewBag.RequestTypeSelectList = new SelectList(requestTypes, "Id", "Name");
            ViewBag.OrderStatusSelectList = new SelectList(orderStatuses, "Id", "Name");

            List<OrderViewModel> model = new List<OrderViewModel>();
            var serviceResult = await _orderService.List(new Order() { CustomerId = await GetCustomerId(), StatusId=20 });
            if (serviceResult.Any())
            {
                model = OrderMapping.GetListViewModel(serviceResult);
            }

            return View(model);
        }
        [Authorize(Policy = "ManageServiceOrderPolicy")]
        public async Task<IActionResult> Add()
        {
            Customer customer = new Customer();
            List<Customer> customers = new List<Customer>();
            List<Site> sites = new List<Site>();
            var customerId = await GetCustomerId();//Loggedin user customer Id
            if (customerId == 0)//true, satnet user
            {
                customers = await _customerService.List(new Customer());
            }
            ViewBag.CustomerId = customerId;
            OrderViewModel model = new OrderViewModel();
            var cities = await _cityService.List(new City());
            var scheduleDates = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.ScheduleDate) });
            var requestTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.OrderRequestType) });
            var servicePlanTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.ServicePlanType) });
            var hardwareConditions = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(HardwareAttributes.HardwareCondition) });
            var macAirNos = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(HardwareAttributes.MACAirNo) });
            var tokens = await _tokenService.List(new Token());
            var promotions = await _promotionService.List(new Promotion());
            var ips = await _ipService.List(new IP());

            model.RequestTypeSelectList = new SelectList(requestTypes, "Id", "Name");
            model.ScheduleDateSelectList= new SelectList(scheduleDates, "Id", "Name");
            model.ServicePlanTypeSelectList = new SelectList(servicePlanTypes, "Id", "Name");
            model.SiteSelectList = new SelectList(sites, "Id", "Name");
            model.TokenSelectList = new SelectList(tokens, "Id", "Name");
            model.PromotionSelectList = new SelectList(promotions, "Id", "Name");
            model.IPSelectList = new SelectList(ips, "Id", "Name");
            model.CustomerSelectList = new SelectList(customers, "Id", "Name");
            model.SiteCitySelectList = new SelectList(cities, "Id", "Name");
            model.HardwareConditionSelectList = new SelectList(hardwareConditions, "Id", "Name");
            model.MacAirNoSelectList= new SelectList(macAirNos, "Id", "Name");
            if (customerId > 0)//true, customer
            {
                customer = await _customerService.Get(customerId);
                model.CustomerName = customer.Name;
                //model.SiteName = GetLoggedInUserCustomerName3(customer.Name).ToUpper() + GetNumber(GetSiteCount() + 1);
            }

            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "ManageServiceOrderPolicy")]
        public async Task<IActionResult> Add(OrderViewModel model)
        {
            int customerId = 0;
            string siteName = "";
            customerId = model.CustomerId == 0 ? await GetCustomerId() : model.CustomerId;

            var order = new Order()
            {
                SiteId = model.SiteId,
                //HardwareId = model.HardwareId,
                //BillingId = model.BillingId,
                //ModemModelId = model.ModemModelId,
                //ModemSrNoId = model.ModemSrNoId,
                MacAirNoId = model.MacAirNoId,
                HardwareConditionId = model.HardwareConditionId,
                //AntennaSizeId = model.AntennaSizeId,
                //TransceiverWATTId = model.TransceiverWATTId,
                //TransceiverSrNoId = model.TransceiverSrNoId,
                ServicePlanId = model.ServicePlanId,
                DedicatedServicePlanName = model.DedicatedServicePlanName,
                RequestTypeId = model.RequestTypeId,
                UpgradeFromId = model.UpgradeFromId,
                UpgradeToId = model.UpgradeToId,
                DowngradeFromId = model.DowngradeFromId,
                DowngradeToId = model.DowngradeToId,
                CreatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier)),
                InstallationDate = model.InstallationDate,
                IPId = model.IPId,
                ChangeIPId = model.ChangeIPId,
                TokenId = model.TokenId,
                PromotionId = model.PromotionId,
                //Download = model.Download,
                //Upload = model.Upload,
                Other = model.Other,
                SubscriberArea = model.SubscriberArea,
                SubscriberCity = model.SubscriberCity,
                SubscriberEmail = model.SubscriberEmail,
                SubscriberName = model.SubscriberName,
                SubscriberNotes = model.SubscriberNotes,
                //SiteCity = model.SiteCity,
                //SiteName = model.SiteId > 0 ? model.SiteName : siteName,
                //SiteArea = model.SiteArea,
                CustomerId = customerId,
                SiteCityId = model.SiteCityId,
                ScheduleDateId = model.ScheduleDateId,
                ChangeServicePlanId = model.ChangeServicePlanId,
                NewMacAirNoId = model.NewMacAirNoId,
                ProRataQuota = model.ProRataQuota
            };

            var result = await _orderService.Add(order);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            var status = new StatusModel { IsSuccess = false, ErrorDescription = result.ErrorDescription };
            //status.Html = RenderViewToString(this, "Index", await GetOrderList());
            return Json(status);

        }

        public async Task<IActionResult> GetOrdersByDDLFilter(string statusValue)
        {
            List<OrderViewModel> model = new List<OrderViewModel>();
            Order obj = new Order();
            //obj.RequestTypeId = string.IsNullOrEmpty(requestTypeValue) ? 0 : Convert.ToInt32(requestTypeValue);
            obj.StatusId = string.IsNullOrEmpty(statusValue) ? 0 : Convert.ToInt32(statusValue);

            var serviceResult = await _orderService.List(obj);
            if (serviceResult.Any())
            {
                model = OrderMapping.GetListViewModel(serviceResult);
            }
            //return Json(new { isValid = true, html = RenderViewToString(this, "_OrderList", model) });
            return PartialView("_OrderList", model);
        }
        public async Task<IActionResult> GetServicePlansByType(string servicePlanTypeId)
        {
            ServicePlan obj = new ServicePlan();
            obj.PlanTypeId = string.IsNullOrEmpty(servicePlanTypeId) ? 0 : Convert.ToInt32(servicePlanTypeId);

            var servicePlans = await _servicePlanService.List(obj);
            return Json(new SelectList(servicePlans, "Id", "Name"));
        }

        [HttpGet]
        [Authorize(Policy = "ReadOnlyServiceOrderPolicy")]
        public async Task<IActionResult> Details(int id)
        {
            var svcResult = await _orderService.Get(id);
            OrderViewModel model = new OrderViewModel();
            model = OrderMapping.GetViewModel(svcResult);
            return View(model);
        }

        public async Task<IActionResult> GetProposedSiteName(int customerId)
        {
            string siteName = "";
            if (customerId > 0)
            {
                var customer = await _customerService.Get(customerId);
                siteName = GetProposedSiteNameImp(customerId, customer.Code);
            }
            return Json(new { siteName });
        }
        private string GetProposedSiteNameImp(int customerId, string customerCode)
        {
            string siteName = (customerCode.Equals("") || customerCode == null ?
                "XXX" : Get3LetterString(customerCode.ToUpper())) + GetNumber(GetSiteCount(customerId) + 1);
            return siteName;
        }
        private string Get3LetterString(string input)
        {
            int maxLength = input.Length >= 3 ? 3 : input.Length;
            var ret = input.Substring(0, maxLength);
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
            else if (to >= 100 && to < 1000)
            {
                retValue = oneZero + to;
            }
            return retValue;
        }
        private int GetSiteCount(int customerId)
        {
            int totalCount = 1;
            var serviceResult = _siteService.List(new Site() { CustomerId = customerId, Flag = "GET_SITE_COUNT" }).Result;
            if (serviceResult.Any())
            {
                totalCount = serviceResult.FirstOrDefault().RecordsCount;
            }
            return totalCount;
        }
        private async Task<int> GetCustomerId()
        {
            var user = await _userManager.GetUserAsync(User);
            return Utilities.TryInt32Parse(user.CustomerId);
        }
        public async Task<IActionResult> GetSites(int customerId, List<int> statusIds)
        {
            Site site = new Site()
            {
                CustomerId = customerId,
                StatusIds = statusIds
            };
            
            
            var sites = await _siteService.List(site);
            return Json(new SelectList(sites, "Id", "Name"));
        }
        public async Task<IActionResult> GetSiteDetails(int siteId)
        {
            var site = await _siteService.Get(siteId);
            //return Json(new { area = site.Area, city = site.City, ipid = site.IPId, subscribername=site.SubscriberName, subscriberarea = site.SubscriberArea,
            //subscriberemail = site.SubscriberEmail, subscribercity=site.SubscriberCity, subscribernotes = site.SubscriberNotes});
            return Json(site);

        }
        [HttpGet]
        public async Task<IActionResult> Action(int id,int statusId, string rejectReason)
        {
            string siteName = "";
            var status = new StatusModel();
            var order = await _orderService.Get(id);
            if (order.RequestTypeId == 1)
            {
                siteName = GetProposedSiteNameImp(order.CustomerId, order.CustomerCode);
            }
            var result = await _orderService.Update(new Order { Id = id, StatusId = statusId, RejectReason=rejectReason, SiteName=siteName, UpdatedBy= Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier)) });
            if (result.IsSuccess)
            {
                status.IsSuccess = true;
                status.IsReload = true;
            }
            else
            {
                status.IsSuccess = false;
                status.IsReload = false;
                status.ErrorDescription = "Some error occurred while processing the request.";
            }
            return Json(status);
        }
        [Authorize(Policy = "ManageServiceOrderPolicy")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _orderService.Delete(id, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            var status = new StatusModel();
            if (result.IsSuccess)
            {
                status.IsSuccess = true;
                status.IsReload = true;
            }
            else
            {
                status.IsSuccess = false;
                status.IsReload = false;
                status.ErrorDescription = "Some error occurred while processing the request.";
            }
            return Json(status);

        }

        [HttpGet]
        [Authorize(Policy = "ReadOnlyServiceOrderPolicy")]
        public async Task<IActionResult> ViewOrder(int id)
        {
            string viewName = "";
            OrderViewModel model = new OrderViewModel();
            var serviceResult = await _orderService.Get(id);
            if (serviceResult != null)
            {
                model = OrderMapping.GetViewModel(serviceResult);
            }
            switch (serviceResult.RequestTypeId)
            {
                case 1: //Activation
                case 32://Re-Activation
                    viewName = "Detail/Activation";
                    break;

                case 2://Termination
                    viewName = "Detail/Termination";
                    break;
                case 3://Upgrade
                    viewName = "Detail/Upgrade";
                    break;
                case 4://Downgrade
                    viewName = "Detail/Downgrade";
                    break;
                case 5://Token Top up
                    viewName = "Detail/TokenTopUp";
                    break;
                case 6://Lock
                    viewName = "Detail/Lock";
                    break;
                case 7://UnLock
                    viewName = "Detail/Unlock";
                    break;
                case 8://Other
                    viewName = "Detail/Other";
                    break;
                case 67://Change Plan
                    viewName = "Detail/ChangePlan";
                    break;
                case 68://Change IP
                    viewName = "Detail/ChangeIP";
                    break;
                case 69://Modem Swap
                    viewName = "Detail/ModemSwap";
                    break;
            }
            return View(viewName, model);
        }

        public async Task<IActionResult> GetCityArea(int cityId)
        {
            var city = await _cityService.Get(cityId);
            return Json(new
            {
                areaname = city.AreaName
            });
        }

        public async Task<IActionResult> GetProRataGB(string monthlyQuota, DateTime installationDate)
        {
            int quota = Convert.ToInt32(monthlyQuota);
            int installationMonth = DateAndTime.Month(installationDate);
            int installationDay = DateAndTime.Day(installationDate);
            int daysPerMonth = DateTime.DaysInMonth(2020, installationMonth);
            int remainingDays = daysPerMonth - installationDay + 1;
            decimal proRataQuotaGB = (quota * remainingDays)/30;

            return Json(new
            {
                proRataQuotaGB = proRataQuotaGB
            });
        }

    }
}