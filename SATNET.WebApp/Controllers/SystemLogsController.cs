using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Helpers;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Invoice;
using SATNET.WebApp.Models.Report;

namespace SATNET.WebApp.Controllers
{
    [Authorize]
    public class SystemLogsController : Base2Controller
    {
        private readonly IService<SystemLog> _systemLogService;
        private readonly IMapper _mapper;
        public SystemLogsController(IMapper mapper, IService<SystemLog> systemLogService, UserManager<ApplicationUser> userManager, IService<Customer> customerService) : base(customerService, userManager)
        {
            _systemLogService = systemLogService;
            _mapper = mapper;
        }
        [Authorize(Policy = "ReadOnlySitePolicy")]
        public async Task<IActionResult> Index()
        {
            int customerId = await GetCustomerId();
            if (customerId == 0)
            {
                var customers = await GetCustomerList(new Customer());
                ViewBag.CustomerSelectList = new SelectList(customers, "Id", "Name");
                ViewBag.CustomerId = 0;
            }
            List<SystemLogViewModel> model = new List<SystemLogViewModel>(); 
            var result = await _systemLogService.List(new SystemLog(){EntityTypeId= 112, CustomerId=await GetCustomerId(), Flag="SiteLog" });
            if (result.Any())
            {
                model = _mapper.Map<List<SystemLogViewModel>>(result);
            }
            return View(model);
        }
        public async Task<IActionResult> GetAjaxLogList(int customerId)
        {
            List<SystemLogViewModel> model = new List<SystemLogViewModel>();
            var result = await _systemLogService.List(new SystemLog() { EntityTypeId = 112, CustomerId = customerId, Flag = "SiteLog" });
            if (result.Any())
            {
                model = _mapper.Map<List<SystemLogViewModel>>(result);
            }
            return PartialView("_List",model);
        }
    }
}
