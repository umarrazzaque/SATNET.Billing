using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Service;
using SATNET.Service.Implementation;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    public class ResellerController : BaseController
    {
        private readonly IServices<Reseller> _resellerService;
        public ResellerController(IServices<Reseller> resellerService)
        {
            _resellerService = resellerService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetResellersList());
        }

        public async Task<IActionResult> Reset()
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", await GetResellersList()) });
        }
        public IActionResult Add()
        {
            CreateResellerModel resellerModel = new CreateResellerModel
            {
                ResellerModel = new ResellerModel(),
                ResellerType = GetResellerTypeList()
            };

            return Json(new { isValid = true, html = RenderViewToString(this, "Add", resellerModel) });
        }


        [HttpPost]
        public async Task<IActionResult> Add(CreateResellerModel createResellerModel)
        {
            ResellerModel resellerModel = createResellerModel.ResellerModel;
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Reseller/Index" };
            if (ModelState.IsValid)
            {
                status = _resellerService.Add(new Reseller
                {
                    Id = 0,
                    RName = resellerModel.RName,
                    RTypeId = resellerModel.RTypeId,
                    REmail = resellerModel.REmail,
                    RAddress = resellerModel.RAddress,
                    RContactNumber = resellerModel.RContactNumber,
                    CreatedBy = 1
                }).Result;
            }
            else
            {
                status.ErrorCode = "Error occured see entity validation errors.";
            }
            status.Html = RenderViewToString(this, "Index", await GetResellersList());
            return Json(status);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Reseller reseller = await _resellerService.Get(id);
            ResellerModel resModel = new ResellerModel
            {
                ResellerId = reseller.Id,
                RName = reseller.RName,
                RTypeId = reseller.RTypeId,
                REmail = reseller.REmail,
                RAddress = reseller.RAddress,
                RContactNumber = reseller.RContactNumber
            };
            CreateResellerModel resellerModel = new CreateResellerModel
            {
                ResellerModel = resModel,
                ResellerType = GetResellerTypeList()
            };
            var status = new StatusModel
            {
                IsSuccess = true,
                Html = RenderViewToString(this, "Edit", resellerModel)
            };
            return Json(status);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateResellerModel createResellerModel)
        {
            ResellerModel resellerModel = createResellerModel.ResellerModel;
            var status = _resellerService.Update(new Reseller
            {
                Id = resellerModel.ResellerId,
                RName = resellerModel.RName,
                RTypeId = resellerModel.RTypeId,
                REmail = resellerModel.REmail,
                RAddress = resellerModel.RAddress,
                RContactNumber = resellerModel.RContactNumber,
                CreatedBy = 1
            }).Result;
            status.Html = RenderViewToString(this, "Index", await GetResellersList());
            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Reseller reseller = await _resellerService.Get(id);
            ResellerModel resellerModel = new ResellerModel
            {
                ResellerId = reseller.Id,
                RName = reseller.RName,
                RTypeId = reseller.RTypeId,
                REmail = reseller.REmail,
                RAddress = reseller.RAddress,
                RContactNumber = reseller.RContactNumber
            };
            var status = new StatusModel
            {
                IsSuccess = true,
                Html = RenderViewToString(this, "Details", resellerModel)
            };
            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var status = _resellerService.Delete(id, 1).Result;
            status.Html = RenderViewToString(this, "Index", await GetResellersList());
            return Json(status);
        }

        private async Task<List<ResellerModel>> GetResellersList()
        {
            List<ResellerModel> ResellerListModel = new List<ResellerModel>();
            var serviceResult = await _resellerService.List();
            if (serviceResult.Any())
            {
                serviceResult.ForEach(res =>
                {
                    ResellerModel reseller = new ResellerModel()
                    {
                        ResellerId = res.Id,
                        RName = res.RName,
                        RTypeId = res.RTypeId,
                        REmail = res.REmail,
                        RAddress = res.RAddress,
                        RContactNumber = res.RContactNumber
                    };
                    ResellerListModel.Add(reseller);
                });
            }
            return ResellerListModel;
        }

        private List<LookUpModel> GetResellerTypeList()
        {
            List<LookUpModel> resellerTypeList = new List<LookUpModel>
            {
                new LookUpModel{ Id = 1, Name = "Partner"},
                new LookUpModel{ Id = 2, Name = "Distributor"}
            };
            return resellerTypeList;
        }
    }
}