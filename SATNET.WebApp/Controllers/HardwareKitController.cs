using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service.Interface;
using SATNET.WebApp.Models.Lookup;
using SATNET.Service;
using Microsoft.AspNetCore.Authorization;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class HardwareKitController : BaseController
    {
        private readonly IService<HardwareKit> _hardwareKitService;
        private readonly IService<HardwareComponent> _hardwareComponentService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        private readonly string _responseUrl;

        public HardwareKitController(IMapper mapper, IService<Lookup> lookUpService, IService<HardwareKit> hardwareKitService,
            IService<HardwareComponent> hardwareComponentService)
        {
            _hardwareKitService = hardwareKitService;
            _hardwareComponentService = hardwareComponentService;
            _lookUpService = lookUpService;
            _mapper = mapper;
            _responseUrl = "/HardwareKit/Index";
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetHardwareKitList());
        }

        public IActionResult Add()
        {
            var resultModel = new CreateHardwareKitModel() { 
                HardwareKitModel = new HardwareKitModel(),
                ModemModels = _mapper.Map<List<HardwareComponentModel>> (_hardwareComponentService.List(new HardwareComponent() {
                    SearchBy = "HC.HardwareTypeId", 
                    Keyword = Convert.ToInt32 (HardwareType.Modem).ToString()
                }).Result),
                AntennaMeters = _mapper.Map<List<HardwareComponentModel>>(_hardwareComponentService.List(new HardwareComponent()
                {
                    SearchBy = "HC.HardwareTypeId",
                    Keyword = Convert.ToInt32(HardwareType.AntennaMeter).ToString()
                }).Result)
            };
            return View(resultModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateHardwareKitModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                HardwareKit obj = _mapper.Map<HardwareKit>(retModel.HardwareKitModel);
                statusModel = await _hardwareKitService.Add(obj);
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
            var resultModel = new CreateHardwareKitModel()
            {
                HardwareKitModel = _mapper.Map< HardwareKitModel> (_hardwareKitService.Get(id).Result),
                ModemModels = _mapper.Map<List<HardwareComponentModel>>(_hardwareComponentService.List(new HardwareComponent()
                {
                    SearchBy = "HC.HardwareTypeId",
                    Keyword = Convert.ToInt32(HardwareType.Modem).ToString()
                }).Result),
                AntennaMeters = _mapper.Map<List<HardwareComponentModel>>(_hardwareComponentService.List(new HardwareComponent()
                {
                    SearchBy = "HC.HardwareTypeId",
                    Keyword = Convert.ToInt32(HardwareType.AntennaMeter).ToString()
                }).Result)
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateHardwareKitModel retModel)
        {
            HardwareKit obj = _mapper.Map<HardwareKit>(retModel.HardwareKitModel);
            var statusModel = await _hardwareKitService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = _hardwareKitService.Delete(id, 1).Result;
            statusModel.Html = RenderViewToString(this, "Index", await GetHardwareKitList());
            return Json(statusModel);
        }
        public async Task<List<HardwareKitModel>> GetHardwareKitList()
        {
            var retList = new List<HardwareKitModel>();
            var serviceResult = await _hardwareKitService.List(new HardwareKit() { });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<HardwareKitModel>>(serviceResult);
            }
            return retList;
        }
        private List<LookUpTypeModel> GetLookTypeList()
        {
            List<LookUpTypeModel> lookupTypeList = new List<LookUpTypeModel>
            {
                new LookUpTypeModel { Id = 1010, Name = "Modem Model"},
                new LookUpTypeModel { Id = 1011, Name = "Modem Serial No"}
            };
            return lookupTypeList;
        }
    }
}
