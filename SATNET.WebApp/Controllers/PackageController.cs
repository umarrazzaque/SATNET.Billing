using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SATNET.WebApp.Models;
using SATNET.Service.Interface;
using SATNET.Service;
using SATNET.Domain;
using System;

namespace SATNET.WebApp.Controllers
{

    public class PackageController : BaseController
    {
        private readonly IService<ServicePlan> _packageService;
        public PackageController(IService<ServicePlan> packageService)
        {
            _packageService = packageService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetPackagesList());
        }

        public async Task<IActionResult> Reset()
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", await GetPackagesList()) });
        }
        public IActionResult Add()
        {
            CreatePackageModel packageModel = new CreatePackageModel
            {
                PackageModel = new PackageModel(),
                PackageTypesList = GetPackageTypeList()
            };

            return Json(new { isValid = true, html = RenderViewToString(this, "Add", packageModel) });
        }


        [HttpPost]
        public async Task<IActionResult> Add(CreatePackageModel createPackageModel)
        {
            throw new NotImplementedException();
            //PackageModel packageModel = createPackageModel.PackageModel;
            //var status = new StatusModel { IsSuccess = false, ResponseUrl = "Package/Index" };
            //if (ModelState.IsValid)
            //{
            //    status = _packageService.Add(new Package
            //    {
            //        Id = 0,
            //        Name = packageModel.Name,
            //        Type = packageModel.PackageType,
            //        Rate = packageModel.Rate,
            //        Speed = packageModel.Speed,
            //        CreatedBy = 1
            //    }).Result;
            //    //if (status.IsSuccess)
            //    //{
            //    //    //get error code description from configuration
            //    //    //status.ErrorCode = "";
            //    //    //status.Data = RenderViewToString(this, "Index", await GetPackagesList());
            //    //}
            //    //else
            //    //{
            //    //    //status.Data = RenderViewToString(this, "Index", await GetPackagesList());
            //    //}
            //    //return Json(new { isValid = false, msg = "Error in inserting the record!", html = RenderViewToString(this, "Index", await GetPackagesList()) });
            //}
            //else
            //{
            //    status.ErrorCode = "Error occured see entity validation errors.";
            //}
            //status.Html = RenderViewToString(this, "Index", await GetPackagesList());
            //return Json(status);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            throw new NotImplementedException();
            //ServicePlan package = await _packageService.Get(id);
            //PackageModel pacModel = new PackageModel
            //{
            //    PackageId = package.Id,
            //    Name = package.Name,
            //    Rate = package.Rate,
            //    Speed = package.Speed,
            //    PackageType = package.Type
            //};
            //CreatePackageModel packageModel = new CreatePackageModel
            //{
            //    PackageModel = pacModel,
            //    PackageTypesList = GetPackageTypeList()
            //};
            //var status = new StatusModel
            //{
            //    IsSuccess = true,
            //    Html = RenderViewToString(this, "Edit", packageModel)
            //};
            //return Json(status);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreatePackageModel createPackageModel)
        {
            throw new NotImplementedException();
            //PackageModel packageModel = createPackageModel.PackageModel;
            //var status = _packageService.Update(new ServicePlan
            //{
            //    Id = packageModel.PackageId,
            //    Name = packageModel.Name,
            //    Type = packageModel.PackageType,
            //    Rate = packageModel.Rate,
            //    Speed = packageModel.Speed,
            //    CreatedBy = 1
            //}).Result;
            //status.Html = RenderViewToString(this, "Index", await GetPackagesList());
            //return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            throw new NotImplementedException();
            //ServicePlan package = await _packageService.Get(id);
            //PackageModel packageModel = new PackageModel
            //{
            //    PackageId = package.Id,
            //    Name = package.Name,
            //    Rate = package.Rate,
            //    Speed = package.Speed,
            //    PackageType = package.Type
            //};
            //var status = new StatusModel
            //{
            //    IsSuccess = true,
            //    Html = RenderViewToString(this, "Details", packageModel)
            //};
            //return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            throw new NotImplementedException();
            //1  as loged in user id
            //var status = _packageService.Delete(id, 1).Result;
            //status.Html = RenderViewToString(this, "Index", await GetPackagesList());
            //return Json(status);
        }

        private async Task<List<PackageModel>> GetPackagesList()
        {
            throw new NotImplementedException();
            //PackageModelList packageList = new PackageModelList();
            //packageList.MenuModel = SetLayoutContent(heading: "Package",subHeading: "Listing");

            //List<PackageModel> packageListModel = new List<PackageModel>();
            //var serviceResult = await _packageService.List(new ServicePlan());
            //if (serviceResult.Any())
            //{
            //    serviceResult.ForEach(i =>
            //    {
            //        PackageModel package = new PackageModel()
            //        {
            //            PackageId = i.Id,
            //            Name = i.Name,
            //            PackageType = i.Type,
            //            Rate = i.Rate,
            //            Speed = i.Speed
            //        };
            //        packageListModel.Add(package);
            //    });
            //}
            //return packageListModel;
        }

        private IList<PackageTypeModel> GetPackageTypeList()
        {
            List<PackageTypeModel> packageTypeList = new List<PackageTypeModel>
            {
                new PackageTypeModel{ PackageTypeId = 1, PackageTypeName = "Standard"},
                new PackageTypeModel{ PackageTypeId = 2, PackageTypeName = "Addons"},
                new PackageTypeModel{ PackageTypeId = 3, PackageTypeName = "Token"}
            };
            return packageTypeList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="heading"></param>
        /// <param name="subHeading"></param>
        /// <returns></returns>
        /*private MenuModel SetLayoutContent(string heading, string subHeading)
        {
            MenuModel model = new MenuModel
            {
                Heading = heading,
                SubHeading = subHeading
            };
            return model;
        }*/

    }
}