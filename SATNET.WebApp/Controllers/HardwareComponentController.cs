using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Hardware;
using SATNET.WebApp.Models.Lookup;

namespace SATNET.WebApp.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class HardwareComponentController : BaseController
    {
        private readonly IService<HardwareComponent> _hardwareComponentService;
        private readonly IService<HardwareComponentPrice> _hardwareCompnentPriceService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        
        private readonly string _responseUrl;
        public HardwareComponentController(IMapper mapper, IService<Lookup> lookUpService, IService<HardwareComponent> hardwareComponent,
            IService<HardwareComponentPrice> hardwareComponentPrice)
        {
            _hardwareComponentService = hardwareComponent;
            _hardwareCompnentPriceService = hardwareComponentPrice;
            _lookUpService = lookUpService;
            _mapper = mapper;
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
                HardwareTypes = GetSpareTypes(new Lookup() {
                    Flag = "GET_BY_HARDWARE_TYPE_OT_SPARE",
                    Keyword = Convert.ToInt32(LookupTypes.HardwareType).ToString(),
                    SearchBy = Convert.ToInt32(HardwareType.Kit).ToString()
                }),
                //_mapper.Map<List<LookUpModel>>( _lookUpService.List(new Lookup() {  LookupTypeId = Convert.ToInt32(LookupTypes.HardwareType) }).Result),
                SpareTypes = GetSpareTypes(new Lookup()
                {
                    Flag = "GET_BY_HARDWARE_TYPE_OT_SPARE",
                    Keyword = Convert.ToInt32(LookupTypes.HardwareType).ToString(),
                    SearchBy = string.Format("{0},{1}", Convert.ToInt32(HardwareType.Kit).ToString(), Convert.ToInt32(HardwareType.Spare).ToString())
                })

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
                HardwareTypes = GetSpareTypes(new Lookup()
                {
                    Flag = "GET_BY_HARDWARE_TYPE_OT_SPARE",
                    Keyword = Convert.ToInt32(LookupTypes.HardwareType).ToString(),
                    SearchBy = Convert.ToInt32(HardwareType.Spare).ToString()
                }),
                //_mapper.Map<List<LookUpModel>>( _lookUpService.List(new Lookup() {  LookupTypeId = Convert.ToInt32(LookupTypes.HardwareType) }).Result),
                SpareTypes = GetSpareTypes(new Lookup()
                {
                    Flag = "GET_BY_HARDWARE_TYPE_OT_SPARE",
                    Keyword = Convert.ToInt32(LookupTypes.HardwareType).ToString(),
                    SearchBy = string.Format("{0},{1}", Convert.ToInt32(HardwareType.Kit).ToString(),Convert.ToInt32(HardwareType.Spare).ToString())
                })
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
            var loginUserId = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var statusModel = await _hardwareComponentService.Delete(id, loginUserId);
            statusModel.Html = RenderViewToString(this, "Index", await GetHardwareComponentList());
            return Json(statusModel);
        }
        #region Hardware Component Prices
        [HttpGet]
        public async Task<IActionResult> HCPriceList(int id)
        {
            var hard_comp_model = _mapper.Map<HardwareComponentModel>(await _hardwareComponentService.Get(id));
            var resultModel = new CreateHardwareComponentPriceModel()
            {
                HardwareComponentModel = hard_comp_model,
                HardwareComponentPriceModel = new HardwareComponentPriceModel(),
                HardwareComponentPriceList =  _mapper.Map<List<HardwareComponentPriceModel>> ( 
                    await _hardwareCompnentPriceService.List(new HardwareComponentPrice() { 
                        SearchBy = "H.HardwareComponentId",
                        Keyword = hard_comp_model.Id.ToString()
                    })),
                PriceTierList = GetPriceTierList(hard_comp_model.Id.ToString()).Result
            };
            

            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> HCPriceList(CreateHardwareComponentPriceModel retModel)
        {
            HardwareComponentPrice obj = _mapper.Map<HardwareComponentPrice>(retModel.HardwareComponentPriceModel);
            obj.HardwareComponentId = retModel.HardwareComponentModel.Id;
            var statusModel = await _hardwareCompnentPriceService.Add(obj);
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);
        }
        private async Task<List<LookUpModel>> GetPriceTierList(string hardwareComponentId, string mode = null, int? priceTierId = null)
        {

            var svcResult = await _lookUpService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });
            var hardCompPriceListByHC = await _hardwareCompnentPriceService.List(new HardwareComponentPrice
            {
                
                SearchBy = "H.HardwareComponentId",
                Keyword = string.IsNullOrEmpty(hardwareComponentId) ? "0" : hardwareComponentId
            });
            foreach (var hcPP in hardCompPriceListByHC)
            {
                var item = svcResult.SingleOrDefault(i => i.Id == hcPP.PriceTierId);
                if (item != null)
                {
                    //if (item.Id == priceTierId)
                        //!(mode.Equals("edit") && 
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

        private async Task<List<LookUpModel>> GetPriceTierList()
        {
            List<LookUpModel> retListModel = new List<LookUpModel>();
            var retList = await _lookUpService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerPriceTier) });
            if (retList.Any())
            {
                retListModel = _mapper.Map<List<LookUpModel>>(retList);
            }
            return retListModel;
        }
        #endregion
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

        private List<LookUpModel> GetSpareTypes(Lookup obj)
        {
            var retList = new List<LookUpModel>();
            var tempList = (_lookUpService.List(obj)).Result;
                
            retList = _mapper.Map<List<LookUpModel>>(tempList);
            return retList;
        }
    }
}
