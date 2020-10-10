using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models.IPPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SATNET.WebApp.Controllers
{
    public class IPPriceController : BaseController
    {
        private readonly IService<IPPrice> _IPPService;
        private readonly IService<IP> _IPService;
        private readonly IService<Lookup> _lookupService;
        private readonly string _responseUrl;

        public IPPriceController(IService<IPPrice> IPPriceService, IService<IP> IPService, IService<Lookup> lookupService)
        {
            _IPPService = IPPriceService;
            _IPService = IPService;
            _lookupService = lookupService;
            _responseUrl = "/IPPrice/Index";
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetIPPriceList());
        }
        private async Task<List<IPPriceViewModel>> GetIPPriceList()
        {
            var retList = new List<IPPriceViewModel>();
            var result = await _IPPService.List(new IPPrice());
            if (result.Any())
            {
                retList = IPPriceMapping.GetListViewModel(result);
            }
            return retList;
        }
        public async Task<IActionResult> Add()
        {
            var model = new IPPriceViewModel();
            var priceTiers = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });
            model.PriceTierSelectList = new SelectList(priceTiers, "Id", "Name");
            var ips = await _IPService.List(new IP());
            ips = ips.Where(ip => ip.Id != 3).ToList();
            model.IPSelectList = new SelectList(ips, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(IPPriceViewModel model)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                IPPrice obj = IPPriceMapping.GetEntity(model);
                obj.CreatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                statusModel = await _IPPService.Add(obj);
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
            var ipprice = await _IPPService.Get(id);
            var model = IPPriceMapping.GetViewModel(ipprice);
            var priceTiers = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });
            model.PriceTierSelectList = new SelectList(priceTiers, "Id", "Name");
            var ips = await _IPService.List(new IP());
            model.IPSelectList = new SelectList(ips, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(IPPriceViewModel model)
        {
            IPPrice obj = IPPriceMapping.GetEntity(model);
            obj.UpdatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var statusModel = await _IPPService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = await _IPPService.Delete(id, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            statusModel.Html = RenderViewToString(this, "Index", await GetIPPriceList());
            return Json(statusModel);
        }
    }
}
