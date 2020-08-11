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
using Microsoft.AspNetCore.Http.Extensions;
using SATNET.Service;

namespace SATNET.WebApp.Controllers
{
    public class HardwareSerialNoController : Controller
    {
        private string _hardwareAttr = "";
        const string HardwareAttrType = "_HardwareAttributeType";
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        private readonly HardwareAttributes activeHardwareAttribute;

        public HardwareSerialNoController(IMapper mapper, IService<Lookup> lookUpService)
        {
            _lookUpService = lookUpService;
            _mapper = mapper;
            activeHardwareAttribute = HardwareAttributes.ModemSrNo;
        }
        public async Task<IActionResult> Index(string hardwareAttr)
        {
            //string reqURL = Request.GetDisplayUrl();
            //var res = reqURL.Split("/");
            //byte[] activeHardwareAttrTypeB = Encoding.ASCII.GetBytes(hardwareAttr);
            //HttpContext.Session.Set(HardwareAttrType, activeHardwareAttrTypeB);
            return View(await GetHardwareAttrList());
        }

        public IActionResult Add()
        {
            var resultModel = new CreateLookUpModel() { 
                LookUpModel = new LookUpModel() { LookUpTypeId = Convert.ToInt32(activeHardwareAttribute) },
                LookupType = GetLookTypeList()
            };
            return View(resultModel);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateLookUpModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareSerialNo/Index" };
            if (ModelState.IsValid)
            {
                Lookup obj = _mapper.Map<Lookup>(retModel.LookUpModel);
                statusModel = await _lookUpService.Add(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            statusModel.ResponseUrl = "/HardwareSerialNo/Index";
            return Json(statusModel);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var resultModel = new CreateLookUpModel()
            {
                LookUpModel = _mapper.Map<LookUpModel>(await _lookUpService.Get(id)),
                LookupType = GetLookTypeList()
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateLookUpModel retModel)
        {
            Lookup obj = _mapper.Map<Lookup>(retModel.LookUpModel);
            var statusModel = await _lookUpService.Update(obj);
            statusModel.ResponseUrl = "/HardwareSerialNo/Index";
            return Json(statusModel);
        }
        public async Task<List<LookUpModel>> GetHardwareAttrList()
        {
            var retList = new List<LookUpModel>();
            var serviceResult = await _lookUpService.List(new Lookup { LookupTypeId = Convert.ToInt32(activeHardwareAttribute) });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<LookUpModel>>(serviceResult);
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
