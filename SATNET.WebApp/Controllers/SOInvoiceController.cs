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
    [Authorize]
    public class SOInvoiceController : Base2Controller
    {
        private readonly IService<SOInvoice> _invoiceService;
        private readonly IService<Site> _siteService;
        private readonly IService<Lookup> _lookupService;
        private readonly IMapper _mapper;
        public SOInvoiceController(IService<SOInvoice> invoiceService, IService<Site> siteService, IService<Lookup> lookupService, UserManager<ApplicationUser> userManager, IMapper mapper, IService<Customer> customerService) : base(customerService, userManager)
        {
            _siteService = siteService;
            _invoiceService = invoiceService;
            _mapper = mapper;
            _lookupService = lookupService;
        }
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> Index()
        {
            var invoiceStatuses = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.InvoiceStatus) });

            ViewBag.InvoiceStatusSelectList = new SelectList(invoiceStatuses, "Id", "Name");
            var model = await GetInvoiceList(71);

            return View(model);
        }

        [HttpGet]
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> ViewInvoice(int id)
        {
            string viewName = "";
            SOInvoiceViewModel model = new SOInvoiceViewModel();
            var serviceResult = await _invoiceService.Get(id);
            if (serviceResult != null)
            {
                model = _mapper.Map<SOInvoiceViewModel>(serviceResult);
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
                    viewName = "Detail/Token";
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

        public async Task<IActionResult> FilterInvoiceList(int statusId)
        {
            var model = await GetInvoiceList(statusId);
            return PartialView("_List", model);
        }

        private async Task<List<SOInvoiceViewModel>> GetInvoiceList(int statusId)
        {
            List<SOInvoiceViewModel> objList = new List<SOInvoiceViewModel>();
            var serviceResult = await _invoiceService.List(new SOInvoice() { CustomerId = await GetCustomerId(), StatusId = statusId });
            if (serviceResult.Any())
            {
                objList = SOInvoiceMapping.GetListViewModel(serviceResult);
            }
            return objList;
        }

        [HttpGet]
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> GetSiteLedgerReport()
        {
            List<SiteLedgerViewModel> model = new List<SiteLedgerViewModel>();
            List<Customer> customers = new List<Customer>();
            int customerId = await GetCustomerId();
            if (customerId == 0)
            {
                customers = await GetCustomerList(new Customer());
                ViewBag.CustomerSelectList = new SelectList(customers, "Id", "Name");
                ViewBag.CustomerId = 0;
            }
            else
            {
                var customer = await GetCustomer(customerId);
                ViewBag.CustomerName = customer.Name;
                ViewBag.CustomerId = customerId;
                model = await GetSiteLedgerList(customerId, 0);
            }

            return View("Report/SiteLedger", model);
        }

        public async Task<IActionResult> GetAjaxSiteLedgerReport(int customerId, int siteId)
        {
            var model = await GetSiteLedgerList(customerId, siteId);
            if (customerId > 0)
            {
                var customer = await GetCustomer(customerId);
                ViewBag.CustomerName = customer.Name;
            }

            return PartialView("Report/_SiteLedgerList", model);
        }

        private async Task<List<SiteLedgerViewModel>> GetSiteLedgerList(int customerId, int siteId)
        {
            List<SiteLedgerViewModel> siteLedgerList = new List<SiteLedgerViewModel>();
            var sites = await _siteService.List(new Site() { CustomerId = customerId, Id = siteId });
            ViewBag.SiteSelectList = new SelectList(sites, "Id", "Name");
            foreach (var site in sites)
            {
                var siteLedgerViewModel = new SiteLedgerViewModel() { Name = site.Name };
                DateTime.Today.AddMonths(-1);
                var invoices = await _invoiceService.List(new SOInvoice() { SiteId = site.Id, StartDate= DateTime.Today.AddMonths(-1).Date, EndDate=DateTime.Now.Date });
                if (invoices.Any())
                {
                    foreach (var inv in invoices)
                    {
                        var invoice = await _invoiceService.Get(inv.Id);
                        var invoiceViewModel = _mapper.Map<SOInvoiceViewModel>(invoice);
                        siteLedgerViewModel.InvoiceViewModels.Add(invoiceViewModel);
                    }
                }
                siteLedgerList.Add(siteLedgerViewModel);
            }
            return siteLedgerList;
        }
        public async Task<IActionResult> GetSiteSelectList(int customerId)
        {
            Site site = new Site()
            {
                CustomerId = customerId
            };
            var sites = await _siteService.List(site);
            return Json(new SelectList(sites, "Id", "Name"));
        }
    }
}
