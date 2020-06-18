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
        private readonly ILookupService _lookupService;
        private readonly IService<Hardware> _hardwareService;
        private readonly IService<Package> _packageService;
        private readonly IService<Site> _siteService;
        public OrderController(IService<Order> orderService, ILookupService lookupService, IService<Hardware> hardwareService, IService<Package> packageService, IService<Site> siteService)
        {
            _orderService = orderService;
            _lookupService = lookupService;
            _hardwareService = hardwareService;
            _packageService = packageService;
            _siteService = siteService;
        }
        public async Task<IActionResult> Index()
        {
            var requestTypes = await _lookupService.ListByFilter(Convert.ToInt32(LookupTypes.RequestType));
            var orderStatuses = await _lookupService.ListByFilter(Convert.ToInt32(LookupTypes.OrderStatus));

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

            var planTypes = await _lookupService.ListByFilter(Convert.ToInt32(LookupTypes.PlanType));
            var requestTypes = await _lookupService.ListByFilter(Convert.ToInt32(LookupTypes.RequestType));
            var hardwares = await _hardwareService.List(new Hardware());
            var packages = await _packageService.List(new Package());
            var sites = await _siteService.List(new Site());

            model.RequestTypeSelectList = new SelectList(requestTypes, "Id", "Name");
            model.HardwareSelectList = new SelectList(hardwares, "Id", "ModemModel");
            model.PlanTypeSelectList = new SelectList(planTypes, "Id", "Name");
            model.PackageSelectList = new SelectList(packages, "Id", "Name");
            model.SiteSelectList = new SelectList(sites, "Id", "Name");
            model.ResellerName = "Satnet Partner";

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(OrderViewModel model)
        {
            var result = await _orderService.Add(new Order()
            {
                SiteId = model.SiteId,
                HardwareId = model.HardwareId,
                PackageId = model.PackageId,
                RequestTypeId = model.RequestTypeId,
                UpgradeFrom = model.UpgradeFrom,
                UpgradeTo = model.UpgradeTo,
                DowngradeFrom = model.DowngradeFrom,
                DowngradeTo = model.DowngradeTo,
                CreatedBy = 1,
                InstallationDate = model.InstallationDate,
                PlanTypeId = model.PlanTypeId,
                IP = model.IP,
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