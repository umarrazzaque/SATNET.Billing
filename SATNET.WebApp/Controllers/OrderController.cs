using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        public async Task<IActionResult> Index()
        {

            return View();
        }

        public async Task<IActionResult> Add()
        {
            OrderViewModel model = new OrderViewModel();
            return Json(new { isValid = true, html = RenderViewToString(this, "Add", model) });
        }

        private async Task<List<DistributorViewModel>> GetSiteList()
        {
            throw new NotImplementedException();
        }
        private async Task<List<OrderViewModel>> GetOrderList()
        {
            List<OrderViewModel> model = new List<OrderViewModel>();
            var serviceResult = await _orderService.List();
            if (serviceResult.Any())
            {
                serviceResult.ForEach(i =>
                {
                    OrderViewModel order = new OrderViewModel()
                    {
                        
                    };
                    model.Add(order);
                });
            }
            return model;
        }
    }
}