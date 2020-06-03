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
    [Authorize(Policy = "UserAccessPolicy")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserController(IUserService userService, UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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
        [Authorize(Policy = "UserEditPolicy")]
        public async Task<IActionResult> Add()
        {
            UserViewModel model = new UserViewModel();
            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "UserEditPolicy")]
        public async Task<IActionResult> Add(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.Contact
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

        [Authorize(Policy = "UserEditPolicy")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound(
                    $"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            UserEditViewModel model = new UserEditViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Contact = user.PhoneNumber,
                Email = user.Email
            };
            return View(model);
        }
        [HttpPost]
        [Authorize(Policy = "UserEditPolicy")]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());

                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.Contact;
                if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    var newPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);
                    user.PasswordHash = newPassword;
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Edit");
        }
        [Authorize(Policy = "UserEditPolicy")]
        public async Task<IActionResult> Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                user.IsDeleted = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View();
        }
    }
}