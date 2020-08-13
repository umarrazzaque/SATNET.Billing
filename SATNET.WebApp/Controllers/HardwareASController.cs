using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Models.Lookup;

namespace SATNET.WebApp.Controllers
{
    
    public class HardwareASController : BaseController
    {
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        private readonly HardwareAttributes activeHardwareAttribute;
        private readonly string _responseUrl;
        public HardwareASController(IMapper mapper, IService<Lookup> lookUpService)
        {
            _lookUpService = lookUpService;
            _mapper = mapper;
            activeHardwareAttribute = HardwareAttributes.AntennaSize;
            _responseUrl = "/HardwareAS/Index";
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetHardwareMANList());
        }
        public IActionResult Add()
        {
            var resultModel = new CreateLookUpModel()
            {
                LookUpModel = new LookUpModel() { LookUpTypeId = Convert.ToInt32(activeHardwareAttribute) }
            };
            return View(resultModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateLookUpModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                Lookup obj = _mapper.Map<Lookup>(retModel.LookUpModel);
                statusModel = await _lookUpService.Add(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);

        }
        //[Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var resultModel = new CreateLookUpModel()
            {
                LookUpModel = _mapper.Map<LookUpModel>(await _lookUpService.Get(id))
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateLookUpModel retModel)
        {
            Lookup obj = _mapper.Map<Lookup>(retModel.LookUpModel);
            var statusModel = await _lookUpService.Update(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var statusModel = _lookUpService.Delete(id, 1).Result;
            statusModel.Html = RenderViewToString(this, "Index", await GetHardwareMANList());
            return Json(statusModel);
        }
        public async Task<List<LookUpModel>> GetHardwareMANList()
        {
            var retList = new List<LookUpModel>();
            var serviceResult = await _lookUpService.List(new Lookup { LookupTypeId = Convert.ToInt32(activeHardwareAttribute) });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<LookUpModel>>(serviceResult);
            }
            return retList;
        }
    }
}
