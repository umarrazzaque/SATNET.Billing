using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserController(IUserService userService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
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
                        LastName = i.LastName,
                        UserName = i.UserName,
                        Contact = i.Contact,
                        Email = i.Email
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
        public async Task<IActionResult> Add(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { 
                    UserName = model.UserName, 
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Contact,

                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Add");
            //return Json(new { isValid = true, html = RenderViewToString(this,"Index", list) });
        }
    }
}