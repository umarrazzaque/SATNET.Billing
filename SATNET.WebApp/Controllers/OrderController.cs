using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service.Interface;
using SATNET.WebApp.Helpers;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Order;

namespace SATNET.WebApp.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        //private readonly IOrderService _orderService;
        private readonly ILookupService _lookupService;
        private readonly IService<Hardware> _hardwareService;
        private readonly IService<Package> _packageService;
        public OrderController(/*IOrderService orderService, */ILookupService lookupService, IService<Hardware> hardwareService, IService<Package> packageService)
        {
            //_orderService = orderService;
            _lookupService = lookupService;
            _hardwareService = hardwareService;
            _packageService = packageService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetOrderList());
        }
        public async Task<IActionResult> AddSite()
        {
            CreateSiteViewModel model = new CreateSiteViewModel();

            var planTypes = await _lookupService.ListByFilter(Convert.ToInt32(LookupTypes.PlanType));
            var hardwares = await _hardwareService.List();
            var packages = await _packageService.List();

            model.HardwareSelectList = new SelectList(hardwares, "Id", "ModemModel");
            model.PlanTypeSelectList = new SelectList(planTypes, "Id", "Name");
            model.PackageSelectList = new SelectList(packages, "Id", "Name");
            model.ResellerName = "Satnet Partner";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddSite(CreateSiteViewModel model)
        {

            return View(model);
        }

        public async Task<IActionResult> Reset()
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", await GetOrderList()) });
        }

        private async Task<List<DistributorViewModel>> GetSiteList()
        {
            throw new NotImplementedException();
        }
        private async Task<List<OrderViewModel>> GetOrderList()
        {
            List<OrderViewModel> model = new List<OrderViewModel>();
            //var serviceResult = await _orderService.List();
            //if (serviceResult.Any())
            //{
            //    serviceResult.ForEach(i =>
            //    {
            //        OrderViewModel order = new OrderViewModel()
            //        {
            //            Id = i.Id,
            //            SiteName = i.SiteName,
            //            PackageName = i.PackageName,
            //            RequestTypeName = i.RequestTypeName,
            //            CustomerName = i.CustomerName,
            //            ServiceOrderDate = i.CreatedOn
            //        };
            //        model.Add(order);
            //    });
            //}
            return model;
        }
    }
}