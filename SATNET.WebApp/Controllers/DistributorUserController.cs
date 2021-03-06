﻿using System; 
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    public class DistributorUserController : BaseController
    {
        private readonly IUserService _userService;
        public DistributorUserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            List<DistributorUserViewModel> model = new List<DistributorUserViewModel>();
            //var serviceResult = await _userService.GetAllUsers();
            //if (serviceResult.Any())
            //{
            //    serviceResult.ForEach(i =>
            //    {
            //        DistributorUserViewModel user = new DistributorUserViewModel()
            //        {
            //            DistributorId = i.DistributorId,
            //            Id = i.Id,
            //            FirstName = i.FirstName,
            //            LastName = i.LastName
            //        };
            //        model.Add(user);
            //    });
            //}

            return View(model);
        }

        public IActionResult Add()
        {
            DistributorUserViewModel model = new DistributorUserViewModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Add(DistributorUserViewModel model)
        {
            List<DistributorUserViewModel> list = new List<DistributorUserViewModel>();
            //DistributorUserViewModel model = new DistributorUserViewModel();
            return Json(new { isValid = true, html = RenderViewToString(this,"Index", list) });
        }
    }
}