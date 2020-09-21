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

namespace SATNET.WebApp.Controllers
{
    [Authorize]
    public class SOInvoiceController : BaseController
    {
        private readonly IService<SOInvoice> _invoiceService;
        private readonly IService<Lookup> _lookupService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public SOInvoiceController(IService<SOInvoice> invoiceService, IService<Lookup> lookupService, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _userManager = userManager;
            _mapper = mapper;
            _lookupService = lookupService;
        }
        [Authorize(Policy = "ReadOnlySOInvoicePolicy")]
        public async Task<IActionResult> Index()
        {
            var invoiceStatuses = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.InvoiceStatus) });

            ViewBag.InvoiceStatusSelectList = new SelectList(invoiceStatuses, "Id", "Name");

            List<SOInvoiceViewModel> model = new List<SOInvoiceViewModel>();
            var serviceResult = await _invoiceService.List(new SOInvoice() { CustomerId = await GetCustomerId(), StatusId = 71 });//71:Due
            if (serviceResult.Any())
            {
                model = SOInvoiceMapping.GetListViewModel(serviceResult);
            }

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
                model = SOInvoiceMapping.GetViewModel(serviceResult);
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


        private async Task<int> GetCustomerId()
        {
            var user = await _userManager.GetUserAsync(User);
            return Utilities.TryInt32Parse(user.CustomerId);
        }

    }
}
