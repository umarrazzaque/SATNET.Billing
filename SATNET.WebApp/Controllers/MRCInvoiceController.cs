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
using SATNET.WebApp.Models.Invoice;
using SATNET.WebApp.Models.Report;


namespace SATNET.WebApp.Controllers
{
    public class MRCInvoiceController : Base2Controller
    {
        private readonly IService<MRCInvoice> _invoiceMRCService;
        private readonly IMapper _mapper;
        public MRCInvoiceController(IService<MRCInvoice> invoiceMRCService, UserManager<ApplicationUser> userManager, IMapper mapper, IService<Customer> customerService) : base(customerService, userManager)
        {
            _invoiceMRCService = invoiceMRCService;
            _mapper = mapper;
        }
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> Index()
        {
            List<Customer> customers = new List<Customer>();
            int customerId = await GetCustomerId();
            if (customerId == 0)
            {
                customers = await GetCustomerList(new Customer());
                ViewBag.CustomerSelectList = new SelectList(customers, "Id", "Name");
            }
            var model = await GetMRCInvoiceList(customerId, 0, DateTime.MinValue, DateTime.MinValue);
            return View(model);
        }
        private async Task<List<MRCInvoiceViewModel>> GetMRCInvoiceList(int customerId, int siteId, DateTime startDate, DateTime endDate)
        {
            List<MRCInvoiceViewModel> model = new List<MRCInvoiceViewModel>();
            var serviceResult = await _invoiceMRCService.List(new MRCInvoice() { CustomerId = customerId, SiteId = siteId, StartDate = startDate, EndDate = endDate });
            if (serviceResult != null)
            {
                model = _mapper.Map<List<MRCInvoiceViewModel>>(serviceResult);
            }
            return model;
        }
        [HttpGet]
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> ViewInvoice(int id)
        {
            MRCInvoiceViewModel model = new MRCInvoiceViewModel();
            var serviceResult = await _invoiceMRCService.Get(id);
            if (serviceResult != null)
            {
                model = _mapper.Map<MRCInvoiceViewModel>(serviceResult);
            }

            return View(model);
        }
        public async Task<IActionResult> FilterInvoiceList(int customerId, int siteId, DateTime startDate, DateTime endDate)
        {
            var model = await GetMRCInvoiceList(customerId, siteId, startDate, endDate);
            return PartialView("_List", model);
        }

    }
}
