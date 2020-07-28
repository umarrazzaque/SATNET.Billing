using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private int customerId;

        public OrderController(IService<Customer> customerService, IService<Order> orderService, IService<Hardware> hardwareService, IService<ServicePlan> servicePlanService, IService<Site> siteService
            , IService<Lookup> lookupService, IService<Token> tokenService, IService<Promotion> promotionService, IService<IP> ipService, IMapper mapper
            , UserManager<ApplicationUser> userManager)
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
        }
        public async Task<IActionResult> Index()
        {
            var requestTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.OrderRequestType) });
            var orderStatuses = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.OrderStatus) });

            ViewBag.RequestTypeSelectList = new SelectList(requestTypes, "Id", "Name");
            ViewBag.OrderStatusSelectList = new SelectList(orderStatuses, "Id", "Name");

            List<OrderViewModel> model = new List<OrderViewModel>();
            var serviceResult = await _orderService.List(new Order() { CustomerId=await GetCustomerId()});
            if (serviceResult.Any())
            {
                model = OrderMapping.GetListViewModel(serviceResult);
            }

            return View(model);
        }
        public async Task<IActionResult> Add()
        {
            Customer customer = new Customer();
            List<Customer> customers = new List<Customer>();
            var customerId = await GetCustomerId();
            if (customerId == 0)//true, satnet user
            {
                customers = await _customerService.List(new Customer());
            }
            ViewBag.CustomerId = customerId;
            OrderViewModel model = new OrderViewModel();
            var sites = await _siteService.List(new Site() { CustomerId=customerId});
            var requestTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.OrderRequestType) });
            var servicePlanTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.ServicePlanType) });
            var hardwares = await _hardwareService.List(new Hardware());
            var tokens = await _tokenService.List(new Token());
            var promotions = await _promotionService.List(new Promotion());
            var ips = await _ipService.List(new IP());

            model.RequestTypeSelectList = new SelectList(requestTypes, "Id", "Name");
            model.HardwareSelectList = new SelectList(hardwares, "Id", "ModemModel");
            model.ServicePlanTypeSelectList = new SelectList(servicePlanTypes, "Id", "Name");
            model.SiteSelectList = new SelectList(sites, "Id", "Name");
            model.TokenSelectList = new SelectList(tokens, "Id", "Name");
            model.PromotionSelectList = new SelectList(promotions, "Id", "Name");
            model.IPSelectList = new SelectList(ips, "Id", "Name");
            model.CustomerSelectList = new SelectList(customers, "Id", "Name");
            if (customerId > 0)//true, customer
            {
                customer = await _customerService.Get(customerId);
                model.CustomerName = customer.Name;
                model.SiteName = GetLoggedInUserCustomerName3(customer.Name).ToUpper() + GetNumber(GetSiteCount() + 1);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(OrderViewModel model)
        {
            var result = await _orderService.Add(new Order()
            {
                SiteId = model.SiteId,
                HardwareId = model.HardwareId,
                ServicePlanId = model.ServicePlanId,
                RequestTypeId = model.RequestTypeId,
                UpgradeFromId = model.UpgradeFromId,
                UpgradeToId = model.UpgradeToId,
                DowngradeFromId = model.DowngradeFromId,
                DowngradeToId = model.DowngradeToId,
                CreatedBy = 1,
                InstallationDate = model.InstallationDate,
                IPId = model.IPId,
                TokenId = model.TokenId,
                PromotionId = model.PromotionId,
                Download = model.Download,
                Upload = model.Upload,
                Other = model.Other,
                SubscriberArea = model.SubscriberArea,
                SubscriberCity = model.SubscriberCity,
                SubscriberEmail = model.SubscriberEmail,
                SubscriberName = model.SubscriberName,
                SubscriberNotes = model.SubscriberNotes,
                SiteCity = model.SiteCity,
                SiteName = model.SiteName,
                SiteArea = model.SiteArea,
                CustomerId = await GetCustomerId()
            }); 
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
            }
            var status = new StatusModel { IsSuccess = false,ErrorDescription=result.ErrorDescription };
            //status.Html = RenderViewToString(this, "Index", await GetOrderList());
            return Json(status);

        }

        public async Task<IActionResult> GetOrdersByDDLFilter(string requestTypeValue, string statusValue)
        {
            List<OrderViewModel> model = new List<OrderViewModel>();
            Order obj = new Order();
            obj.RequestTypeId = string.IsNullOrEmpty(requestTypeValue) ? 0 : Convert.ToInt32(requestTypeValue);
            obj.StatusId = string.IsNullOrEmpty(statusValue) ? 0 : Convert.ToInt32(statusValue);

            var serviceResult = await _orderService.List(obj);
            if (serviceResult.Any())
            {
                model = OrderMapping.GetListViewModel(serviceResult);
            }
            return Json(new { isValid = true, html = RenderViewToString(this, "_OrderList", model) });
        }
        public async Task<IActionResult> GetServicePlansByType(string servicePlanTypeId)
        {
            ServicePlan obj = new ServicePlan();
            obj.PlanTypeId = string.IsNullOrEmpty(servicePlanTypeId) ? 0 : Convert.ToInt32(servicePlanTypeId);

            var servicePlans = await _servicePlanService.List(obj);
            return Json(new SelectList(servicePlans, "Id", "Name"));            
        }

        [HttpGet]
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
                siteName = GetLoggedInUserCustomerName3(customer.Name).ToUpper() + GetNumber(GetSiteCount() + 1);
            }
            return Json(new { siteName });
        }
        private string GetLoggedInUserCustomerName3(string customerName)
        {
            int maxLength = customerName.Length >= 3 ? 3 : customerName.Length;
            var ret = customerName.Substring(0, maxLength);
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
        private int GetSiteCount()
        {
            int totalCount = 1;
            var serviceResult = _siteService.List(new Site() {CustomerId=GetCustomerId().Result, Flag = "GET_SITE_COUNT" }).Result;
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
    }
}