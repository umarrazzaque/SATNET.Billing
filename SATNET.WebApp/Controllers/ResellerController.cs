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
using SATNET.WebApp.Models.Lookup;

namespace SATNET.WebApp.Controllers
{
    public class ResellerController : BaseController
    {
        private readonly IService<Customer> _resellerService;
        public ResellerController(IService<Customer> resellerService)
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
                status = _resellerService.Add(new Customer
                {
                    Id = 0,
                    Name = resellerModel.Name,
                    TypeId = resellerModel.TypeId,
                    Code = resellerModel.Code,
                    Email = resellerModel.Email,
                    Address = resellerModel.Address,
                    ContactNumber = resellerModel.ContactNumber,
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
            Customer reseller = await _resellerService.Get(id);
            ResellerModel resModel = new ResellerModel
            {
                Id = reseller.Id,
                Name = reseller.Name,
                TypeId = reseller.TypeId,
                Code = reseller.Code,
                Email = reseller.Email,
                Address = reseller.Address,
                ContactNumber = reseller.ContactNumber
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
            var status = _resellerService.Update(new Customer
            {
                Id = resellerModel.Id,
                Name = resellerModel.Name,
                TypeId = resellerModel.TypeId,
                Code = resellerModel.Code,
                Email = resellerModel.Email,
                Address = resellerModel.Address,
                ContactNumber = resellerModel.ContactNumber,
                CreatedBy = 1
            }).Result;
            status.Html = RenderViewToString(this, "Index", await GetResellersList());
            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Customer reseller = await _resellerService.Get(id);
            ResellerModel resellerModel = new ResellerModel
            {
                Id = reseller.Id,
                Name = reseller.Name,
                TypeId = reseller.TypeId,
                Code = reseller.Code,
                Email = reseller.Email,
                Address = reseller.Address,
                ContactNumber = reseller.ContactNumber
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
            var serviceResult = await _resellerService.List(new Customer());
            if (serviceResult.Any())
            {
                serviceResult.ForEach(res =>
                {
                    ResellerModel reseller = new ResellerModel()
                    {
                        Id = res.Id,
                        Name = res.Name,
                        TypeId = res.TypeId,
                        Code = res.Code,
                        Email = res.Email,
                        Address = res.Address,
                        ContactNumber = res.ContactNumber
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