using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Helpers;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Order;

namespace SATNET.WebApp.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IService<Order> _orderService;
        private readonly IService<Lookup> _lookupService;
        private readonly IService<Hardware> _hardwareService;
        private readonly IService<ServicePlan> _packageService;
        private readonly IService<Site> _siteService;
        private readonly IService<Token> _tokenService;
        private readonly IService<Promotion> _promotionService;
        private readonly IService<IP> _ipService;

        public OrderController(IService<Order> orderService, IService<Hardware> hardwareService, IService<ServicePlan> packageService, IService<Site> siteService
            , IService<Lookup> lookupService, IService<Token> tokenService, IService<Promotion> promotionService, IService<IP> ipService)
        {
            _orderService = orderService;
            _hardwareService = hardwareService;
            _packageService = packageService;
            _siteService = siteService;
            _lookupService = lookupService;
            _tokenService = tokenService;
            _promotionService = promotionService;
            _ipService = ipService;
        }
        public async Task<IActionResult> Index()
        {
            var requestTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.RequestType) });
            var orderStatuses = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.OrderStatus) });

            ViewBag.RequestTypeSelectList = new SelectList(requestTypes, "Id", "Name");
            ViewBag.OrderStatusSelectList = new SelectList(orderStatuses, "Id", "Name");

            List<OrderViewModel> model = new List<OrderViewModel>();
            var serviceResult = await _orderService.List(new Order());
            if (serviceResult.Any())
            {
                model = OrderMapping.GetListViewModel(serviceResult);
            }

            return View(model);
        }
        public async Task<IActionResult> Add()
        {
            OrderViewModel model = new OrderViewModel();

            var sites = await _siteService.List(new Site());
            var requestTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.RequestType) });
            var servicePlanTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.PlanType) });
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
            model.CustomerName = "Satnet Customer";

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
                ServicePlanTypeId = model.ServicePlanTypeId,
                IPId = model.IPId,
                TokenId = model.TokenId,
                PromotionId = model.PromotionId,
                Download = model.Download,
                Upload = model.Upload,
                SubscriberArea = model.SubscriberArea,
                SubscriberCity = model.SubscriberCity,
                SubscriberEmail = model.SubscriberEmail,
                SubscriberName = model.SubscriberName,
                SubscriberNotes = model.SubscriberNotes
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
    }
}