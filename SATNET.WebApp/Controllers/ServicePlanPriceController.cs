using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Lookup;


namespace SATNET.WebApp.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
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
                //ServicePlanList = GetServicePlanList().Result,
                ServicePlanTypeList = GetServicePlanTypeList().Result,
                //PriceTierList = GetPriceTierList().Result
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
            ServicePlanPriceModel obj = _mapper.Map<ServicePlanPriceModel>(await _servicePlanPriceService.Get(id));
            var resultModel = new CreateServicePlanPriceModel()
            {
                ServicePlanPriceModel = obj,
                ServicePlanTypeList = GetServicePlanTypeList().Result,
                ServicePlanList = GetServicePlanList().Result.ToList().Where(sp => sp.PlanTypeId == obj.PlanTypeId).ToList(),
                PriceTierList = GetPriceTierList(obj.ServicePlanId.ToString(), "edit", obj.PriceTierId).Result
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
        #region Main List
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
        #endregion
        public async Task<List<LookUpModel>> GetServicePlanTypeList()
        {
            List<LookUpModel> retListModel = new List<LookUpModel>();
            var retList = await _lookUpService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.ServicePlanType) });
            if (retList.Any())
            {
                retListModel = _mapper.Map<List<LookUpModel>>(retList);

            }
            return retListModel;
        }
        #region Service Plan 
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
        public async Task<IActionResult> GetFilteredServicePlan(string planTypeId)
        {
            ServicePlan obj = new ServicePlan();
            obj.PlanTypeId = string.IsNullOrEmpty(planTypeId) ? 0 : Convert.ToInt32(planTypeId);

            var svcResult = await _servicePlanService.List(obj);
            return Json(new SelectList(svcResult, "Id", "Name"));
        }
        #endregion
        #region Price List
        private async Task<List<LookUpModel>> GetPriceTierList(string servicePlanId, string mode, int priceTierId)
        {

            var svcResult = await _lookUpService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });
            var servicePPListBySP = await _servicePlanPriceService.List(new ServicePlanPrice
            {
                Flag = "GET_BY_SERVICE_PLAN",
                SearchBy = "SPP.ServicePlanId",
                Keyword = string.IsNullOrEmpty(servicePlanId) ? "0" : servicePlanId
            });
            foreach (var servicePP in servicePPListBySP)
            {
                var item = svcResult.SingleOrDefault(i => i.Id == servicePP.PriceTierId);
                if (item != null)
                {
                    if (!(mode.Equals("edit") && item.Id == priceTierId))
                    {
                        svcResult.Remove(item);
                    }
                }
            }
            List<LookUpModel> retListModel = new List<LookUpModel>();
            if (svcResult.Any())
            {
                retListModel = _mapper.Map<List<LookUpModel>>(svcResult);
            }
            return retListModel;
        }
        public async Task<IActionResult> GetFilteredPlanPrice(string servicePlanId, string mode, string priceTierId)
        {
            var svcResult = await GetPriceTierList(servicePlanId, mode,
            string.IsNullOrEmpty(priceTierId) ? 0 : Convert.ToInt32(priceTierId));
            return Json(new SelectList(svcResult, "Id", "Name"));
        }
        #endregion

    }
}