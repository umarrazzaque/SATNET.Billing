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
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            List<UserViewModel> model = new List<UserViewModel>();
            var serviceResult = await _userService.GetAllUsers();
            if (serviceResult.Any())
            {
                serviceResult.ForEach(i =>
                {
                    UserViewModel user = new UserViewModel()
                    {
                        DistributorId = i.DistributorId,
                        Id = i.Id,
                        FirstName = i.FirstName,
                        LastName = i.LastName,
                        UserName = i.UserName,
                        EmailAddress = i.EmailAddress
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            return RedirectToAction(nameof(Index));
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