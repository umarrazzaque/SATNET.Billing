using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models.IP;

namespace SATNET.WebApp.Controllers
{
    public class IPController : BaseController
    {
        private readonly IService<IP> _IPService;
        private readonly string _responseUrl;
        public IPController(IService<IP> IPService)
        {
            _IPService = IPService;
            _responseUrl = "/IP/Index";
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetIPList());
        }
        public IActionResult Add()
        {
            return View(new IPViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Add(IPViewModel model)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                IP obj = IPMapping.GetEntity(model);
                obj.CreatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                statusModel = await _IPService.Add(obj);
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
            var model = new IPViewModel();
            var ip = await _IPService.Get(id);
            model = IPMapping.GetViewModel(ip);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(IPViewModel model)
        {
            IP obj = IPMapping.GetEntity(model);
            obj.UpdatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var statusModel = await _IPService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = await _IPService.Delete(id, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            statusModel.Html = RenderViewToString(this, "Index", await GetIPList());
            return Json(statusModel);
        }
        private async Task<List<IPViewModel>> GetIPList()
        {
            var retList = new List<IPViewModel>();
            var result = await _IPService.List(new IP());
            if (result.Any())
            {
                retList = IPMapping.GetListViewModel(result);
            }
            return retList;
        }
    }
}
