using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Internal;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Mappings;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.User;

namespace SATNET.WebApp.Controllers
{
    [Authorize(Policy = "UserAccessPolicy")]
    public class UserController : BaseController
    {
        private readonly IService<User> _userService;
        private readonly IService<Lookup> _lookupService;
        private readonly IService<Customer> _customerService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserController(IService<User> userService, UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager, IService<Lookup> lookupService, IService<Customer> customerService)
        {
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _lookupService = lookupService;
            _customerService = customerService;
        }
        public async Task<IActionResult> Index()
        {
            var customerTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerType) });
            ViewBag.UserTypeSelectList = new SelectList(customerTypes, "Id", "Name");
            return View(await GetUsers());
        }
        public async Task<IActionResult> Add()
        {
            UserViewModel model = new UserViewModel();
            var customerTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerType) });
            var priceTiers = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });

            model.UserTypeSelectList = new SelectList(customerTypes, "Id", "Name");
            model.PriceTierSelectList = new SelectList(priceTiers, "Id", "Name");

            return View(model);
        }
        [HttpPost]
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
                    PhoneNumber = model.Contact,
                    CreatedBy=1,
                    CreatedOn=System.DateTime.Now,
                    IsDeleted=false,
                    CustomerId=model.CustomerId,
                    UserTypeId = model.UserTypeId
                };
                //creating user
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //adding user role
                    //var role = model.Roles.FirstOrDefault().ToString();
                    //await _userManager.AddToRoleAsync(user, role);
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
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound(
                    $"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            //var roleResult  = _userManager.GetRolesAsync(user);

            var customer = await _customerService.List(new Customer() { Id=user.CustomerId});
            var customerPriceTierId = customer.FirstOrDefault().PriceTierId;
            UserEditViewModel model = new UserEditViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Contact = user.PhoneNumber,
                Email = user.Email,
                CustomerId=user.CustomerId,
                UserTypeId = user.UserTypeId,
                PriceTierId = customerPriceTierId
                //Roles = roleResult.Result.ToList()
            };
            var customers = await _customerService.List(new Customer() { PriceTierId= customerPriceTierId });
            var customerTypes = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerType) });
            var priceTiers = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });

            model.UserTypeSelectList = new SelectList(customerTypes, "Id", "Name");
            model.PriceTierSelectList = new SelectList(priceTiers, "Id", "Name");
            model.CustomerSelectList= new SelectList(customers, "Id", "Name");

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());

                user.Email = model.Email;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.Contact;
                user.UpdatedBy = 1;
                user.UpdatedOn = DateTime.Now;
                user.CustomerId = model.CustomerId;
                user.UserTypeId = model.UserTypeId;

                if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    var newPassword = _userManager.PasswordHasher.HashPassword(user, model.Password);
                    user.PasswordHash = newPassword;
                }

                //updating user roles
                //var role = model.Roles.First();
                //var roleResult = await _userManager.GetRolesAsync(user);
                //var oldRole = roleResult.First().ToString();
                //if (oldRole != role)
                //{
                //    await _userManager.RemoveFromRoleAsync(user, oldRole);
                //    await _userManager.AddToRoleAsync(user, role);
                //}

                //updating user details
                //await _userManager.AddToRoleAsync(user, role);
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
        public async Task<IActionResult> Delete(int id)
        {
            var status = new StatusModel { IsSuccess = false, ErrorDescription="" };
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                user.IsDeleted = true;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    status.IsSuccess = true;
                    status.Html = RenderViewToString(this, "Index", await GetUsers());                   
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        status.ErrorDescription += error.Description+"\n";
                    }
                }
            }
            return Json(status);
        }
        private async Task<List<UserViewModel>> GetUsers()
        {
            List<UserViewModel> model = new List<UserViewModel>();
            var svcResult = await _userService.List(new User());
            if (svcResult.Any())
            {
                model = UserMapping.GetListViewModel(svcResult);
            }
            return model;
        }
        public async Task<IActionResult> GetUsers(string userTypeId)
        {
            List<UserViewModel> model = new List<UserViewModel>();
            User obj = new User();
            obj.UserTypeId= string.IsNullOrEmpty(userTypeId) ? 0 : Convert.ToInt32(userTypeId);

            var svcResult = await _userService.List(obj);
            if (svcResult.Any())
            {
                model = UserMapping.GetListViewModel(svcResult);
            }
            return Json(new { isValid = true, html = RenderViewToString(this, "_UserList", model) });
        }
        public async Task<IActionResult> GetCustomers(string customerTypeId, string priceTierId)
        {
            Customer obj = new Customer();
            obj.TypeId = string.IsNullOrEmpty(customerTypeId) ? 0 : Convert.ToInt32(customerTypeId);
            obj.PriceTierId = string.IsNullOrEmpty(customerTypeId) ? 0 : Convert.ToInt32(priceTierId);

            var svcResult = await _customerService.List(obj);
            return Json(new SelectList(svcResult, "Id", "Name"));
        }

    }
}