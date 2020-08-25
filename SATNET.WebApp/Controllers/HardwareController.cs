using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class HardwareController : BaseController
    {
        private readonly IService<Hardware> _hardwareService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;   
        public HardwareController(IService<Hardware> hardwareService, IMapper mapper, IService<Lookup> lookUpService)
        {
            _hardwareService = hardwareService;
            _mapper = mapper;
            _lookUpService = lookUpService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetHardwareList());
        }

        public async Task<IActionResult> Reset()
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", await GetHardwareList()) });
        }
        public IActionResult Add()
        {
            var resultModel = new HardwareModel();
            return View(resultModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(HardwareModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = "/Hardware/Index" };
            if (ModelState.IsValid)
            {
                Hardware obj = _mapper.Map<Hardware>(retModel);
                statusModel = await _hardwareService.Add(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            return Json(statusModel);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var resultModel = _mapper.Map<HardwareModel>(await _hardwareService.Get(id));
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(HardwareModel hardwareModel)
        {
            Hardware obj = _mapper.Map<Hardware>(hardwareModel);
            var statusModel = await _hardwareService.Update(obj);
            return Json(statusModel);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Hardware obj = await _hardwareService.Get(id);
            HardwareModel retModel = _mapper.Map<HardwareModel>(obj);
            return View(retModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = _hardwareService.Delete(id, 1).Result;
            statusModel.Html = RenderViewToString(this, "Index", await GetHardwareList());
            return Json(statusModel);
        }

        private async Task<List<HardwareModel>> GetHardwareList()
        {
            var retList = new List<HardwareModel>();
            var serviceResult = await _hardwareService.List(new Hardware() { });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<HardwareModel>>(serviceResult);
            }
            return retList;
        }

    }
}