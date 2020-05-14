using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    public class DistributorUserController : Controller
    {
        private readonly IUserService _userService;
        public DistributorUserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            List<DistributorUserViewModel> model = new List<DistributorUserViewModel>();
            var serviceResult = await _userService.GetAllUsers();
            if (serviceResult.Any())
            {
                serviceResult.ForEach(i =>
                {
                    DistributorUserViewModel user = new DistributorUserViewModel()
                    {
                        DistributorId = i.DistributorId,
                        Id = i.Id,
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        UserName = i.UserName
                    };
                    model.Add(user);
                });
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(DistributorUserViewModel model)
        {
            if (ModelState.IsValid)
            {

            }
            return View();
        }

        //[HttpPut]
        //[Route()]
        //public async Task<IActionResult> Edit(DistributorUserViewModel model)
        //{
        //    var user = new User()
        //    {
        //        Id=model.Id,
        //        FirstName = model.FirstName,
        //        LastName=model.LastName,
        //        UpdatedOn=DateTime.Now,
        //        UpdatedBy=123
        //    };
        //    var serviceResult = await _userService.UpdateUser(user);

        //    return Json()
        //}

    }
}