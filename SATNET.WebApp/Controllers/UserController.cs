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
    [Authorize(Policy = "AdminPolicy")]
    public class UserController : BaseController
    {
        private readonly IService<User> _userService;
        private readonly IService<Lookup> _lookupService;
        private readonly IService<Customer> _customerService;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        public UserController(IService<User> userService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
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
            var customerTypes = GetCustomerTypes();
            ViewBag.UserTypeSelectList = new SelectList(customerTypes, "Id", "Name");

            return View(await GetUsers());
        }

        public async Task<IActionResult> Add()
        {
            UserViewModel model = new UserViewModel();
            var customerTypes = GetCustomerTypes();
            var priceTiers = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });

            model.UserTypeSelectList = new SelectList(customerTypes, "Id", "Name", "-Select User Type-");
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
                    CreatedBy = 1,
                    CreatedOn = DateTime.Now,
                    IsDeleted = false,
                    CustomerId = model.CustomerId,
                    UserTypeId = model.UserTypeId
                };
                //creating user
                try
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        //adding user role
                        if (!string.IsNullOrEmpty(model.RoleName))
                        {
                            var role = model.RoleName;
                            await _userManager.AddToRoleAsync(user, role);
                        }
                        return RedirectToAction("Index");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception e)
                {

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
            var roleResult = _userManager.GetRolesAsync(user);
            //int customerPriceTierId = 0;
            //if (user.CustomerId != 0)
            //{
            //    var customer = await _customerService.Get(user.CustomerId);
            //    customerPriceTierId = customer.PriceTierId;
            //}
            UserEditViewModel model = new UserEditViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Contact = user.PhoneNumber,
                Email = user.Email,
                CustomerId = user.CustomerId,
                UserTypeId = user.UserTypeId,
                RoleName = roleResult.Result.FirstOrDefault()
            };
            var customersList = await _customerService.List(new Customer());
            var userTypes = GetCustomerTypes();
            
            int roleTypeId = model.UserTypeId == Convert.ToInt32(UserType.Satnet) ? 
                Convert.ToInt32(UserRoles.Satnet) : 
                (model.UserTypeId == Convert.ToInt32(UserType.Customer) ? Convert.ToInt32(UserRoles.Reseller) : 0);
            var roles = _roleManager.Roles.Where(r => r.RoleType == roleTypeId).ToList();

            model.UserTypeSelectList = new SelectList(userTypes, "Id", "Name");
            model.CustomerSelectList = new SelectList(customersList, "Id", "Name");
            model.RoleSelectList = new SelectList(roles, "Name", "Name");

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
                var roleResult = await _userManager.GetRolesAsync(user);
                var oldRole = roleResult.FirstOrDefault();
                if (!string.IsNullOrEmpty(oldRole))
                {
                    await _userManager.RemoveFromRoleAsync(user, oldRole);
                }
                if (!string.IsNullOrEmpty(model.RoleName))
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                }
                //updating user details
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
            var status = new StatusModel { IsSuccess = false, ErrorDescription = "" };
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
                        status.ErrorDescription += error.Description + "\n";
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
            obj.UserTypeId = string.IsNullOrEmpty(userTypeId) ? 0 : Convert.ToInt32(userTypeId);

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

        public IActionResult GetRoles(string type)
        {
            int typeId = string.IsNullOrEmpty(type) ? 0 : Convert.ToInt32(type);

            var roles = _roleManager.Roles.Where(r => r.RoleType == typeId).ToList();
            return Json(new SelectList(roles, "Name", "Name"));
        }

        private List<Lookup> GetCustomerTypes()
        {
            var customerTypes = _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerType) });
            customerTypes.Result.RemoveAt(1);
            return customerTypes.Result;
        }

    }
}