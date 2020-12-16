using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Hardware;

namespace SATNET.WebApp.Controllers
{
    public class LogisticBUCController : BaseController
    {
        private readonly IService<HardwareComponentRegistration> _hardwareComponentRegistrationService;
        private readonly IService<HardwareComponent> _hardwareComponentService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        private readonly string _responseUrl;
        public LogisticBUCController(IMapper mapper, IService<Lookup> lookUpService, IService<HardwareComponentRegistration> hardwareComponentRegistrationService,
            IService<HardwareComponent> hardwareComponentService)
        {
            _hardwareComponentRegistrationService = hardwareComponentRegistrationService;
            _hardwareComponentService = hardwareComponentService;
            _lookUpService = lookUpService;
            _mapper = mapper;
            _responseUrl = "/LogisticBUC/Index";
        }
        [Authorize(Policy = "ReadOnlyLogisticsPolicy")]
        public async Task<IActionResult> Index()
        {
            return View(await GetBUCList());
        }
        private async Task<List<HardwareComponentRegistrationModel>> GetBUCList()
        {
            var retList = new List<HardwareComponentRegistrationModel>();
            var serviceResult = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { SearchBy = "HC.HardwareTypeId", Keyword = Convert.ToInt32(HardwareType.Transceiver).ToString() });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<HardwareComponentRegistrationModel>>(serviceResult);
            }
            return retList;
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        public async Task<IActionResult> Add()
        {
            var resultModel = new CreateHardwareComponentRegistrationModel()
            {
                HardwareComponentRegistrationModel = new HardwareComponentRegistrationModel(),


            };
            var hardwareModems = await _hardwareComponentService.List(new HardwareComponent()
            {
                SearchBy = "HC.HardwareTypeId",
                Keyword = Convert.ToInt32(HardwareType.Transceiver).ToString()
            });
            resultModel.HardwareComponentList = _mapper.Map<List<HardwareComponentModel>>(hardwareModems);
            return View(resultModel);
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateHardwareComponentRegistrationModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                var loginUserid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                HardwareComponentRegistration obj = _mapper.Map<HardwareComponentRegistration>(retModel.HardwareComponentRegistrationModel);
                obj.CreatedBy = loginUserid;
                statusModel = await _hardwareComponentRegistrationService.Add(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);

        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        public async Task<IActionResult> Edit(int id)
        {
            var obj = await _hardwareComponentRegistrationService.Get(id);
            var resultModel = new CreateHardwareComponentRegistrationModel()
            {
                HardwareComponentRegistrationModel = new HardwareComponentRegistrationModel()
                {
                    Id = obj.Id,
                    SerialNumber = obj.SerialNumber,
                    HardwareComponentId = obj.HardwareComponentId
                }
            };
            var hardwareModems = await _hardwareComponentService.List(new HardwareComponent()
            {
                Flag = "GET_BY_HARDWARE_TYPE",
                Keyword = Convert.ToInt32(HardwareType.Transceiver).ToString()
            });
            resultModel.HardwareComponentList = _mapper.Map<List<HardwareComponentModel>>(hardwareModems);
            return View(resultModel);
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateHardwareComponentRegistrationModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                var loginUserid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                HardwareComponentRegistration obj = _mapper.Map<HardwareComponentRegistration>(retModel.HardwareComponentRegistrationModel);
                obj.UpdatedBy = loginUserid;
                statusModel = await _hardwareComponentRegistrationService.Update(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var loginUserid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var statusModel = _hardwareComponentRegistrationService.Delete(id, loginUserid).Result;
            statusModel.Html = RenderViewToString(this, "Index", await GetBUCList());
            return Json(statusModel);
        }
    }
}
