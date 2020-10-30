using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Hardware;
using SATNET.WebApp.Models.Lookup;

namespace SATNET.WebApp.Controllers
{
    public class HardwareComponentRegistrationController : BaseController
    {
        private readonly IService<HardwareComponentRegistration> _hardwareComponentRegistrationService;
        private readonly IService<HardwareComponent> _hardwareComponentService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        private readonly string _responseUrl;
        public HardwareComponentRegistrationController(IMapper mapper, IService<Lookup> lookUpService, IService<HardwareComponentRegistration> hardwareComponentRegistrationService,
            IService<HardwareComponent> hardwareComponentService)
        {
            _hardwareComponentRegistrationService = hardwareComponentRegistrationService;
            _hardwareComponentService = hardwareComponentService;
            _lookUpService = lookUpService;
            _mapper = mapper;
            _responseUrl = "/HardwareComponentRegistration/Index";
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetHardwareComponentRegistrationList());
        }
        public async Task<IActionResult> Add()
        {
            var resultModel = new CreateHardwareComponentRegistrationModel()
            {
                HardwareComponentRegistrationModel = new HardwareComponentRegistrationModel(),
                //HardwareComponentList = _mapper.Map<List<HardwareComponentModel>>(_hardwareComponentService.List(new HardwareComponent()
                //{
                //}).Result),
                HardwareTypes = _mapper.Map<List<LookUpModel>>( await _lookUpService.List(new Lookup() {
                    LookupTypeId = Convert.ToInt32(LookupTypes.HardwareType)
                }))
            };
            resultModel.HardwareTypes = GetHarwareTypes().Result;
            return View(resultModel);
        }
        private async Task<List<LookUpModel>> GetHarwareTypes()
        {
            var retList = new List<LookUpModel>();
            var resList = await _lookUpService.List(new Lookup()
            {
                LookupTypeId = Convert.ToInt32(LookupTypes.HardwareType)
            });
            resList = resList.Where(ht => ht.Id == Convert.ToInt32(HardwareType.Modem) || 
            ht.Id == Convert.ToInt32(HardwareType.Transceiver)).ToList();
            retList = _mapper.Map<List<LookUpModel>>(resList);
            return retList;
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
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var objModel = _mapper.Map<HardwareComponentRegistrationModel>(_hardwareComponentRegistrationService.Get(id).Result);
            var hcl = _mapper.Map<List<HardwareComponentModel>>(_hardwareComponentService.List(new HardwareComponent() { }).Result);
            var ht = _mapper.Map<List<LookUpModel>>(await _lookUpService.List(new Lookup()
            {
                LookupTypeId = Convert.ToInt32(LookupTypes.HardwareType)
            }));
            var hcm = hcl.Where(c => c.Id == objModel.HardwareComponentId).FirstOrDefault();
            if (hcm != null)
            {
                objModel.HardwareTypeId = hcm.HardwareTypeId;
            }
            
            var resultModel = new CreateHardwareComponentRegistrationModel()
            {
                HardwareComponentRegistrationModel = objModel,
                HardwareComponentList = hcl,
                HardwareTypes = ht,
                CustomerList = new List<CustomerModel>()
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateHardwareComponentRegistrationModel retModel)
        {
            HardwareComponentRegistration obj = _mapper.Map<HardwareComponentRegistration>(retModel.HardwareComponentRegistrationModel);
            var statusModel = await _hardwareComponentRegistrationService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }
        public async Task<List<HardwareComponentRegistrationModel>> GetHardwareComponentRegistrationList()
        {
            var retList = new List<HardwareComponentRegistrationModel>();
            var serviceResult = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<HardwareComponentRegistrationModel>>(serviceResult);
            }
            return retList;
        }

        public async Task<IActionResult> GetHardwareComponetByHardwareType(string hardware_type_id)
        {
            if (hardware_type_id != null)
            {
                var retList = new List<HardwareComponentModel>();
                var resList = await _hardwareComponentService.List(new HardwareComponent() { 
                    Flag = "GET_BY_HARDWARE_TYPE",
                    Keyword = hardware_type_id
                });
                retList = _mapper.Map<List<HardwareComponentModel>>(resList);
                return Json(retList);
            }
            else
            {
                return Json("Error in Model Binding");
            }
        }
    }
}
