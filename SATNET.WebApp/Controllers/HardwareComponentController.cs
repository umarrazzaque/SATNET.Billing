using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Lookup;

namespace SATNET.WebApp.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class HardwareComponentController : BaseController
    {
        private readonly IService<HardwareComponent> _hardwareComponentService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        private readonly HardwareType activeHardwareAttribute;
        private readonly string _responseUrl;
        public HardwareComponentController(IMapper mapper, IService<Lookup> lookUpService, IService<HardwareComponent> hardwareComponent)
        {
            _hardwareComponentService = hardwareComponent;
            _lookUpService = lookUpService;
            _mapper = mapper;
            activeHardwareAttribute = HardwareType.TransceiverWATT;
            _responseUrl = "/HardwareComponent/Index";
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetHardwareComponentList());
        }

        public IActionResult Add()
        {
            var resultModel = new CreateHardwareComponentModel()
            {
                HardwareComponentModel = new HardwareComponentModel (),
                HardwareTypes = _mapper.Map<List<LookUpModel>>( _lookUpService.List(new Lookup() {  LookupTypeId = Convert.ToInt32(LookupTypes.HardwareType) }).Result),
                SpareTypes = GetSpareTypes()


            };
            return View(resultModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> Add(CreateHardwareComponentModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                HardwareComponent obj = _mapper.Map<HardwareComponent>(retModel.HardwareComponentModel);
                statusModel = await _hardwareComponentService.Add(obj);
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
            var resultModel = new CreateHardwareComponentModel()
            {
                HardwareComponentModel =  _mapper.Map<HardwareComponentModel>(await _hardwareComponentService.Get(id)),
                HardwareTypes =  _mapper.Map<List<LookUpModel>>( _lookUpService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.HardwareType) }).Result),
                SpareTypes = GetSpareTypes()
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateHardwareComponentModel retModel)
        {
            HardwareComponent obj = _mapper.Map<HardwareComponent>(retModel.HardwareComponentModel);
            var statusModel = await _hardwareComponentService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = _lookUpService.Delete(id, 1).Result;
            statusModel.Html = RenderViewToString(this, "Index", await GetHardwareComponentList());
            return Json(statusModel);
        }
        public async Task<List<HardwareComponentModel>> GetHardwareComponentList()
        {
            var retList = new List<HardwareComponentModel>();
            var serviceResult = await _hardwareComponentService.List(new HardwareComponent());
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<HardwareComponentModel>>(serviceResult);
            }
            return retList;
        }

        private List<LookUpModel> GetSpareTypes()
        {
            var retList = new List<LookUpModel>();
            var tempList = (_lookUpService.List(new Lookup()
            {
                Flag = "GET_BY_HARDWARE_TYPE_OT_SPARE",
                Keyword = Convert.ToInt32(LookupTypes.HardwareType).ToString(),
                SearchBy = "L.LookupTypeId"
            }).Result);
            retList = _mapper.Map<List<LookUpModel>>(tempList);
            return retList;
        }
    }
}
