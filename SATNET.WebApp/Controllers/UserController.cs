using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    public class UserController : BaseController
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
                        Id = i.Id,
                        FirstName = i.FirstName,
                        LastName = i.LastName
                    };
                    model.Add(user);
                });
            }

            return View(model);
        }

        public IActionResult Add()
        {
            UserViewModel model = new UserViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Add(UserViewModel model)
        {
            List<UserViewModel> list = new List<UserViewModel>();
            return Json(new { isValid = true, html = RenderViewToString(this,"Index", list) });
        }
    }
}