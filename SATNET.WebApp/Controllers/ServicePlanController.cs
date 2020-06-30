
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
    public class ServicePlanController : BaseController
    {
        private readonly IService<ServicePlan> _servicePlanService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        public ServicePlanController(IService<ServicePlan> servicePlanService, IMapper mapper, IService<Lookup> lookUpService)
        {
            _servicePlanService = servicePlanService;
            _mapper = mapper;
            _lookUpService = lookUpService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetServicePlanList());
        }

        [HttpGet]
        public IActionResult Add()
        {
            var resultModel = new CreateServicePlanModel()
            {
                ServicePlanModel = new ServicePlanModel(),
                PlanType = GetServicePlanTypeList().Result
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Add(CreateServicePlanModel createReturnModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlan/Index" };
            var retModel = createReturnModel.ServicePlanModel;
            if (ModelState.IsValid)
            {
                ServicePlan obj = _mapper.Map<ServicePlan>(retModel);
                statusModel = await _servicePlanService.Add(obj);
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
            var resultModel = new CreateServicePlanModel()
            {
                ServicePlanModel = _mapper.Map<ServicePlanModel>(await _servicePlanService.Get(id)),
                PlanType = GetServicePlanTypeList().Result
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateServicePlanModel createReturnModel)
        {
            ServicePlan obj = _mapper.Map<ServicePlan>(createReturnModel.ServicePlanModel);
            var statusModel = await _servicePlanService.Update(obj);
            return Json(statusModel);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ServicePlan obj = await _servicePlanService.Get(id);
            ServicePlanModel retModel = _mapper.Map<ServicePlanModel>(obj);
            return View(retModel);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var statusModel = await _servicePlanService.Delete(id, 1);
            statusModel.Html = RenderViewToString(this, "Index", await GetServicePlanList());
            return Json(statusModel);
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

        private async Task<List<LookUpModel>> GetServicePlanTypeList()
        {
            List<LookUpModel> retListModel = new List<LookUpModel>();
            var retList = await _lookUpService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.PlanType) });
            if (retList.Any())
            {
                retListModel = _mapper.Map<List<LookUpModel>>(retList);

            }
            return retListModel;
        }

    }
}