
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
using SATNET.WebApp.Models.Lookup;

namespace SATNET.WebApp.Controllers
{
    public class ServicePlanPriceController : BaseController
    {
        private readonly IService<ServicePlanPrice> _servicePlanPriceService;
        private readonly IService<ServicePlan> _servicePlanService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        public ServicePlanPriceController(IService<ServicePlan> servicePlanService, IMapper mapper, IService<Lookup> lookUpService,
            IService<ServicePlanPrice> servicePlanPriceService)
        {
            _servicePlanPriceService = servicePlanPriceService;
            _servicePlanService = servicePlanService;
            _mapper = mapper;
            _lookUpService = lookUpService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetServicePlanPriceList());
        }

        [HttpGet]
        public IActionResult Add()
        {
            var resultModel = new CreateServicePlanPriceModel()
            {
                ServicePlanPriceModel = new ServicePlanPriceModel(),
                ServicePlanList = GetServicePlanList().Result,
                PriceTierList = GetPriceTierList().Result
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateServicePlanPriceModel createReturnModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlanPrice/Index" };
            var retModel = createReturnModel.ServicePlanPriceModel;
            if (ModelState.IsValid)
            {
                ServicePlanPrice obj = _mapper.Map<ServicePlanPrice>(retModel);
                statusModel = await _servicePlanPriceService.Add(obj);
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
            var resultModel = new CreateServicePlanPriceModel()
            {
                ServicePlanPriceModel = _mapper.Map<ServicePlanPriceModel>(await _servicePlanPriceService.Get(id)),
                ServicePlanList = GetServicePlanList().Result,
                PriceTierList = GetPriceTierList().Result
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateServicePlanPriceModel createReturnModel)
        {
            ServicePlanPrice obj = _mapper.Map<ServicePlanPrice>(createReturnModel.ServicePlanPriceModel);
            var status = _servicePlanPriceService.Update(obj).Result;
            status.Html = RenderViewToString(this, "Index", await GetServicePlanPriceList());
            return Json(status);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ServicePlanPrice obj = await _servicePlanPriceService.Get(id);
            ServicePlanPriceModel retModel = _mapper.Map<ServicePlanPriceModel>(obj);
            return View(retModel);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _servicePlanPriceService.Delete(id, 1);
            status.Html = RenderViewToString(this, "Index", await GetServicePlanPriceList());
            return Json(status);
        }
        public async Task<List<ServicePlanPriceModel>> GetServicePlanPriceList()
        {
            var retList = new List<ServicePlanPriceModel>();
            var serviceResult = await _servicePlanPriceService.List(new ServicePlanPrice { });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<ServicePlanPriceModel>>(serviceResult);
            }
            return retList;
        }
        public async Task<List<ServicePlanModel>> GetServicePlanList()
        {
            var retList = new List<ServicePlanModel>();
            var serviceResult = await _servicePlanService.List(new ServicePlan { });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<ServicePlanModel>>(serviceResult);
            }
            return retList;
        }

        private async Task<List<LookUpModel>> GetPriceTierList()
        {
            List<LookUpModel> retListModel = new List<LookUpModel>();
            var retList = await _lookUpService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.PriceTier) });
            if (retList.Any())
            {
                retListModel = _mapper.Map<List<LookUpModel>>(retList);

            }
            return retListModel;
        }

    }
}