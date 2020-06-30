using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SATNET.WebApp.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            //Package package = await _packageService.Get(id);
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
            return View();
        }
    }
}