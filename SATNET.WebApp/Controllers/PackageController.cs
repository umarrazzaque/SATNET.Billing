using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SATNET.WebApp.Attributes;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    [Authorize]
    public class PackageController : BaseController
    {
     
        public PackageController()
        {

        }
        public async  Task<IActionResult> Index()
        {
            return View(GetPackagesList());
        }

        public async Task<IActionResult> Reset()
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", GetPackagesList()) });
        }
        public async Task<IActionResult> Add()
        {

            PackageModel packageModel = new PackageModel();
            packageModel.MenuModel = SetLayoutContent(heading: "Package", subHeading: "Add");

            return Json(new { isValid = true, html = RenderViewToString(this, "Add", packageModel) });
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(PackageModel model)
        {
            
            //DistributorUserViewModel model = new DistributorUserViewModel();
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", GetPackagesList()) });
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            PackageModel packageModel = new PackageModel { PackageId = 5, PackageName = "Text is there", Rate = 0 } ;
            packageModel.MenuModel = SetLayoutContent(heading: "Package", subHeading: "Edit");

            return Json(new { isValid = true, html = RenderViewToString(this, "Edit", packageModel) });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PackageModel model)
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", GetPackagesList()) });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            PackageModel packageModel = new PackageModel { PackageId = 5, PackageName = "Text is there", Rate = 0 };
            packageModel.MenuModel = SetLayoutContent(heading: "Package", subHeading: "Detail");

            return Json(new { isValid = true, html = RenderViewToString(this, "Details", packageModel) });
        }

        private PackageModelList GetPackagesList()
        {
            PackageModelList packageList = new PackageModelList();
            packageList.MenuModel = SetLayoutContent(heading: "Package",subHeading: "Listing");

            packageList.PackageModels.Add(new PackageModel { PackageId = 1, PackageName = "ABC", Rate = 1 });
            packageList.PackageModels.Add(new PackageModel { PackageId = 2, PackageName = "QWE", Rate = 2 });
            packageList.PackageModels.Add(new PackageModel { PackageId = 3, PackageName = "QWE", Rate = 3 });
            packageList.PackageModels.Add(new PackageModel { PackageId = 4, PackageName = "ZXC", Rate = 4 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "POI", Rate = 5 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "LKJ", Rate = 6 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "MNB", Rate = 7 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "BVC", Rate = 8 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "HGF", Rate = 9 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "TRE", Rate = 10 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "DSA", Rate = 11 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "JHG", Rate = 12 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "IUY", Rate = 13 });
            packageList.PackageModels.Add(new PackageModel { PackageName = "NBV", Rate = 14 });
            return packageList;
        }

        private MenuModel SetLayoutContent(string heading, string subHeading)
        {
            MenuModel model = new MenuModel
            {
                Heading = heading,
                SubHeading = subHeading
            };
            return model;
        }

    }
}