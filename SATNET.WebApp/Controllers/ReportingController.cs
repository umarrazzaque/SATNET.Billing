using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Models.Invoice;
using SATNET.WebApp.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Controllers
{
    public class ReportingController : Base2Controller
    {
        private readonly IReportingService _reportingService; 
        private readonly IService<SOInvoice> _invoiceService;
        private readonly IService<Site> _siteService;
        private readonly IMapper _mapper;
        public ReportingController(IMapper mapper, IService<SOInvoice> invoiceService, IService<Site> siteService, IReportingService reportingService, IService<Customer> customerService, UserManager<ApplicationUser> userManager) :base(customerService, userManager)
        {
            _reportingService = reportingService;
            _invoiceService = invoiceService;
            _siteService = siteService;
            _mapper = mapper;
        }

        #region Site Ledger Report

        [HttpGet]
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> GetSiteLedgerReport()
        {
            //var model = new SiteLedgerAllReport();
            List<SiteLedgerViewModel> siteLedgers = new List<SiteLedgerViewModel>();
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
                siteLedgers = await GetSiteLedgerList(customerId, 0, DateTime.MinValue, DateTime.MinValue);
            }
            //model.SiteLedgers = siteLedgers;
            //model.ReceivableReport = await GetReceivableReport(customerId, 0);
            return View("SiteLedger/Index", siteLedgers);
        }

        public async Task<IActionResult> GetAjaxSiteLedgerReport(int customerId, int siteId, DateTime startDate, DateTime endDate)
        {
            //var model = new SiteLedgerAllReport();
            var siteLedgers = await GetSiteLedgerList(customerId, siteId, startDate, endDate);
            if (customerId > 0)
            {
                var customer = await GetCustomer(customerId);
                ViewBag.CustomerName = customer.Name;
            }
            //model.SiteLedgers = siteLedgers;
            //model.ReceivableReport = await GetReceivableReport(customerId, siteId);
            return PartialView("SiteLedger/_List", siteLedgers);
        }

        private async Task<List<SiteLedgerViewModel>> GetSiteLedgerList(int customerId, int siteId, DateTime startDate, DateTime endDate)
        {
            List<SiteLedgerViewModel> siteLedgerList = new List<SiteLedgerViewModel>();
            var sites = await _siteService.List(new Site() { CustomerId = customerId, Id = siteId });
            sites = sites.OrderBy(s => s.Name).ToList();
            ViewBag.SiteSelectList = new SelectList(sites, "Id", "Name");
            foreach (var site in sites)
            {
                var siteLedgerViewModel = new SiteLedgerViewModel() { Name = site.Name };

                var invoices = await _invoiceService.List(new SOInvoice() { SiteId = site.Id, StartDate = startDate, EndDate = endDate });
                if (invoices.Any())
                {
                    invoices = invoices.OrderBy(i => i.Id).ToList();
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


        #endregion

        #region Receivable per Category
        [HttpGet]
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> GetReceivableByCategoryReport()
        {
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
            }
            var model = await GetReceivableReport(customerId, 0);
            return View("ReceivableByCategory/Index", model);
        }

        public async Task<IActionResult> GetAjaxReceivableReport(int customerId, int siteId)
        {
            var model = await GetReceivableReport(customerId, siteId);
            return PartialView("ReceivableByCategory/_List", model);
        }

        private async Task<ReceivablePerCategoryViewModel> GetReceivableReport(int customerId, int siteId)
        {
            ReceivablePerCategoryViewModel model = new ReceivablePerCategoryViewModel();
            var result = await _reportingService.GetReceivablePerCategoryReport(customerId, siteId);
            model = _mapper.Map<ReceivablePerCategoryViewModel>(result);
            return model;
        }
        #endregion
    }
}
