using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models.TokenPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SATNET.WebApp.Controllers
{
    public class TokenPriceController : BaseController
    {
        private readonly IService<Token> _tokenService;
        private readonly IService<TokenPrice> _tokenPriceService;
        private readonly IService<Lookup> _lookupService;
        private readonly string _responseUrl;
        public TokenPriceController(IService<TokenPrice> tokenPriceService, IService<Lookup> lookupService, IService<Token> tokenService)
        {
            _tokenPriceService = tokenPriceService;
            _tokenService = tokenService;
            _lookupService = lookupService;
            _responseUrl = "/TokenPrice/Index";
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetTokenPriceList());
        }
        public async Task<IActionResult> Add()
        {
            var model = new TokenPriceViewModel();
            var priceTiers = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });
            model.PriceTierSelectList = new SelectList(priceTiers, "Id", "Name");
            var tokens = await _tokenService.List(new Token());
            model.TokenSelectList = new SelectList(tokens, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Add(TokenPriceViewModel model)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                TokenPrice obj = TokenPriceMapping.GetEntity(model);
                obj.CreatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                statusModel = await _tokenPriceService.Add(obj);
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
            var model = new TokenPriceViewModel();
            var token = await _tokenPriceService.Get(id);
            model = TokenPriceMapping.GetViewModel(token);
            var priceTiers = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });
            model.PriceTierSelectList = new SelectList(priceTiers, "Id", "Name");
            var tokens = await _tokenService.List(new Token());
            model.TokenSelectList = new SelectList(tokens, "Id", "Name");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TokenPriceViewModel model)
        {
            TokenPrice obj = TokenPriceMapping.GetEntity(model);
            obj.UpdatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var statusModel = await _tokenPriceService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = await _tokenPriceService.Delete(id, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            statusModel.Html = RenderViewToString(this, "Index", await GetTokenPriceList());
            return Json(statusModel);
        }
        private async Task<List<TokenPriceViewModel>> GetTokenPriceList()
        {
            var retList = new List<TokenPriceViewModel>();
            var result = await _tokenPriceService.List(new TokenPrice());
            if (result.Any())
            {
                retList = TokenPriceMapping.GetListViewModel(result);
            }
            return retList;
        }
    }
}
