﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Helpers;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SATNET.WebApp.Controllers
{
    public class CreditNoteController : BaseController
    {
        private readonly IService<CreditNote> _creditnoteService;
        private readonly IService<SOInvoice> _invoiceService;
        private readonly string _responseUrl;
        private readonly UserManager<ApplicationUser> _userManager;
        public CreditNoteController(IService<CreditNote> creditnoteService, UserManager<ApplicationUser> userManager, IService<SOInvoice> invoiceService)
        {
            _creditnoteService = creditnoteService;
            _invoiceService = invoiceService;
            _userManager = userManager;
            _responseUrl = "/CreditNote/Index";
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetCreditNoteList());
        }
        public async Task<IActionResult> Add()
        {
            var model = new CreditNoteViewModel();
            var invoices = await _invoiceService.List(new SOInvoice() { CustomerId=await GetCustomerId()});
            model.InvoiceSelectList = new SelectList(invoices, "Id", "InvoiceNumber");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreditNoteViewModel model)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                CreditNote obj = CreditNoteMapping.GetEntity(model);
                obj.CreatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                statusModel = await _creditnoteService.Add(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var obj = await _creditnoteService.Get(id);
            var model = CreditNoteMapping.GetViewModel(obj);
            var invoices = await _invoiceService.List(new SOInvoice() { CustomerId = await GetCustomerId() });
            model.InvoiceSelectList = new SelectList(invoices, "Id", "InvoiceNumber");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreditNoteViewModel model)
        {
            CreditNote obj = CreditNoteMapping.GetEntity(model);
            obj.UpdatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var statusModel = await _creditnoteService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = await _creditnoteService.Delete(id, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            statusModel.Html = RenderViewToString(this, "Index", await GetCreditNoteList());
            return Json(statusModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            var obj = await _creditnoteService.Get(id);
            var model = CreditNoteMapping.GetViewModel(obj);
            return View("Detail/Details", model);
        }

        private async Task<List<CreditNoteViewModel>> GetCreditNoteList()
        {
            var retList = new List<CreditNoteViewModel>();
            var result = await _creditnoteService.List(new CreditNote() { CustomerId=await GetCustomerId()});
            if (result.Any())
            {
                retList = CreditNoteMapping.GetListViewModel(result);
            }
            return retList;
        }
        private async Task<int> GetCustomerId()
        {
            var user = await _userManager.GetUserAsync(User);
            return Utilities.TryInt32Parse(user.CustomerId);
        }

    }
}