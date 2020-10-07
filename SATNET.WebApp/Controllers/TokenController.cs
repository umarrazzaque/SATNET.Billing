using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SATNET.WebApp.Controllers
{
    public class TokenController : BaseController
    {
        private readonly IService<Token> _tokenService;
        private readonly string _responseUrl;
        public TokenController(IService<Token> tokenService)
        {
            _tokenService = tokenService;
            _responseUrl = "/Token/Index";
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetTokenList());
        }
        public IActionResult Add()
        {
            return View(new TokenViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Add(TokenViewModel model)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                Token obj = TokenMapping.GetEntity(model);
                obj.CreatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                statusModel = await _tokenService.Add(obj);
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
            var model = new TokenViewModel();
            var token = await _tokenService.Get(id);
            model = TokenMapping.GetViewModel(token);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(TokenViewModel model)
        {
            Token obj = TokenMapping.GetEntity(model);
            obj.UpdatedBy = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var statusModel = await _tokenService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = await _tokenService.Delete(id, Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            statusModel.Html = RenderViewToString(this, "Index", await GetTokenList());
            return Json(statusModel);
        }
        private async Task<List<TokenViewModel>> GetTokenList()
        {
            var retList = new List<TokenViewModel>();
            var result = await _tokenService.List(new Token());
            if (result.Any())
            {
                retList = TokenMapping.GetListViewModel(result);
            }
            return retList;
        }
    }
}
