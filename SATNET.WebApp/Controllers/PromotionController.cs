using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SATNET.WebApp.Controllers
{
    public class PromotionController : BaseController
    {
        private readonly IService<Promotion> _promotionService;
        private readonly string _responseUrl;
        public PromotionController(IService<Promotion> promotionService)
        {
            _promotionService = promotionService;
            _responseUrl = "/Promotion/Index";
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetPromotionList());
        }
        public IActionResult Add()
        {
            return View(new PromotionViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Add(PromotionViewModel model)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                Promotion obj = PromotionMapping.GetEntity(model);
                obj.CreatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                statusModel = await _promotionService.Add(obj);
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
            var model = new PromotionViewModel();
            var token = await _promotionService.Get(id);
            model = PromotionMapping.GetViewModel(token);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PromotionViewModel model)
        {
            Promotion obj = PromotionMapping.GetEntity(model);
            obj.UpdatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var statusModel = await _promotionService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = await _promotionService.Delete(id, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            statusModel.Html = RenderViewToString(this, "Index", await GetPromotionList());
            return Json(statusModel);
        }
        private async Task<List<PromotionViewModel>> GetPromotionList()
        {
            var retList = new List<PromotionViewModel>();
            var result = await _promotionService.List(new Promotion());
            if (result.Any())
            {
                retList = PromotionMapping.GetListViewModel(result);
            }
            return retList;
        }
    }
}
