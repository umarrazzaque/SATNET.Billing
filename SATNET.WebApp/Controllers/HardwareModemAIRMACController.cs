using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Hardware;
using SATNET.WebApp.Models.Lookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Controllers
{
    public class HardwareModemAIRMACController : BaseController
    {
        private readonly IService<HardwareComponentRegistration> _hardwareComponentRegistrationService;
        private readonly IService<HardwareComponent> _hardwareComponentService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        private readonly string _responseUrl;
        public HardwareModemAIRMACController(IMapper mapper, IService<Lookup> lookUpService, IService<HardwareComponentRegistration> hardwareComponentRegistrationService,
            IService<HardwareComponent> hardwareComponentService)
        {
            _hardwareComponentRegistrationService = hardwareComponentRegistrationService;
            _hardwareComponentService = hardwareComponentService;
            _lookUpService = lookUpService;
            _mapper = mapper;
            _responseUrl = "/HardwareModemAIRMAC/Index";
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetAirmacList());
        }
        public async Task<List<HardwareComponentRegistrationModel>> GetAirmacList()
        {
            var retList = new List<HardwareComponentRegistrationModel>();
            var serviceResult = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { SearchBy= "HC.HardwareTypeId", Keyword=Convert.ToInt32(HardwareType.Modem).ToString() });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<HardwareComponentRegistrationModel>>(serviceResult);
            }
            return retList;
        }
        public async Task<IActionResult> Add()
        {
            var resultModel = new CreateHardwareComponentRegistrationModel()
            {
                HardwareComponentRegistrationModel = new HardwareComponentRegistrationModel(),

                
            };
            var hardwareModems = await _hardwareComponentService.List(new HardwareComponent()
            {
                Flag = "GET_BY_HARDWARE_TYPE",
                Keyword = Convert.ToInt32(HardwareType.Modem).ToString()
            });
            resultModel.HardwareComponentList = _mapper.Map<List<HardwareComponentModel>>(hardwareModems);
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateHardwareComponentRegistrationModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                HardwareComponentRegistration obj = _mapper.Map<HardwareComponentRegistration>(retModel.HardwareComponentRegistrationModel);
                statusModel = await _hardwareComponentRegistrationService.Add(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);

        }
    }
}
