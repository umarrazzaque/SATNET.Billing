using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using SATNET.Domain.Enums;
using SATNET.WebApp.Models.Hardware;
using SATNET.Service;
using System.Security.Claims;

namespace SATNET.WebApp.Controllers
{
    public class LogisticBUCRegController : Base2Controller
    {
        private readonly IService<HardwareComponent> _hardwareComponentService;
        private readonly IService<HardwareComponentRegistration> _hardwareComponentRegistrationService;
        private readonly IMapper _mapper;
        private readonly string _responseUrl;
        public LogisticBUCRegController(IMapper mapper, IService<Customer> customerService, UserManager<ApplicationUser> userManager
            , IService<HardwareComponentRegistration> hardwareComponentRegistrationService, IService<HardwareComponent> hardwareComponentService) : base(customerService, userManager)
        {
            _hardwareComponentRegistrationService = hardwareComponentRegistrationService;
            _hardwareComponentService = hardwareComponentService;
            _mapper = mapper;
            _responseUrl = "/LogisticBUCReg/Index";
        }
        [Authorize(Policy = "ReadOnlyLogisticsPolicy")]
        public async Task<IActionResult> Index()
        {
            var model = await GetRegisteredAIRMACList();
            var customers = await GetCustomerList(new Customer());
            ViewBag.SelectCustomerList = new SelectList(customers, "Id", "Name");
            var hardwareModems = await _hardwareComponentService.List(new HardwareComponent()
            {
                SearchBy = "HC.HardwareTypeId",
                Keyword = Convert.ToInt32(HardwareType.Transceiver).ToString()
            });
            ViewBag.SelectHardwareModems = new SelectList(hardwareModems, "Id", "HCValue");
            return View(model);
        }
        public async Task<List<HardwareComponentRegistrationModel>> GetRegisteredAIRMACList()
        {
            var retList = new List<HardwareComponentRegistrationModel>();
            var serviceResult = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { Flag = "RegisteredBUC" });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<HardwareComponentRegistrationModel>>(serviceResult);
            }
            return retList;
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        public async Task<IActionResult> Add()
        {
            var model = new HardwareComponentRegistrationModel();
            var customers = await GetCustomerList(new Customer());
            model.CustomerSelectList = new SelectList(customers, "Id", "Name");
            var hardwareModems = await _hardwareComponentService.List(new HardwareComponent()
            {
                SearchBy = "HC.HardwareTypeId",
                Keyword = Convert.ToInt32(HardwareType.Transceiver).ToString()
            });
            ViewBag.SelectHardwareModems = new SelectList(hardwareModems, "Id", "HCValue");
            var unRegisteredAIRMACs = new List<HardwareComponentRegistrationModel>();
            var serviceResult = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { Flag = "UnregisteredBUC" });
            if (serviceResult.Any())
            {
                unRegisteredAIRMACs = _mapper.Map<List<HardwareComponentRegistrationModel>>(serviceResult);
            }
            ViewBag.AIRMACs = unRegisteredAIRMACs;
            ViewBag.SelectUnRegisteredAIRMACs = new SelectList(unRegisteredAIRMACs, "AIRMAC", "AIRMAC");
            

            return View(model);
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Add(HardwareComponentRegistrationModel model)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                var loginUserid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                var obj = _mapper.Map<HardwareComponentRegistration>(model);
                obj.UpdatedBy = loginUserid;
                obj.Flag = "RegisterBUC";
                statusModel = await _hardwareComponentRegistrationService.Update(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);

        }
        public async Task<IActionResult> GetBUCListByFilter(string transceiver, string customer, string flag)
        {
            string view = "";
            int modemId = 0;

            if (transceiver != "")
            {
                modemId = Convert.ToInt32(transceiver);
            }
            var registerModel = new HardwareComponentRegistrationModel();
            var bucserials = new List<HardwareComponentRegistrationModel>();
            var serviceResult = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { Flag = flag, HardwareComponentId = modemId, CustomerId = Convert.ToInt32(customer) });
            if (serviceResult.Any())
            {
                bucserials = _mapper.Map<List<HardwareComponentRegistrationModel>>(serviceResult);
            }
            ViewBag.AIRMACs = bucserials;
            if (flag == "RegisteredBUC")
            {
                return PartialView("_LogisticBUCRegList", bucserials);
            }
            else if (flag == "UnregisteredBUC")
            {
                return PartialView("_AddList", new HardwareComponentRegistrationModel());
            }
            else
            {
                return null;
            }
        }
    }
}
