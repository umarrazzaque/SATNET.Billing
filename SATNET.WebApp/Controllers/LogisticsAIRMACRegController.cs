using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Models.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SATNET.WebApp.Controllers
{
    public class LogisticsAIRMACRegController : Base2Controller
    {
        private readonly IService<HardwareComponent> _hardwareComponentService;
        private readonly IService<HardwareComponentRegistration> _hardwareComponentRegistrationService;
        private readonly IMapper _mapper;
        private readonly string _responseUrl;
        public LogisticsAIRMACRegController(IMapper mapper, IService<Customer> customerService, UserManager<ApplicationUser> userManager
            , IService<HardwareComponentRegistration> hardwareComponentRegistrationService, IService<HardwareComponent> hardwareComponentService) : base(customerService, userManager)
        {
            _hardwareComponentRegistrationService = hardwareComponentRegistrationService;
            _hardwareComponentService = hardwareComponentService;
            _mapper = mapper;
            _responseUrl = "/LogisticsAIRMACReg/Index";
        }
        [Authorize(Policy = "ReadOnlyLogisticsPolicy")]
        public async Task<IActionResult> Index()
        {
            var model = await GetRegisteredAIRMACList();
            ViewBag.SelectRegisteredAIRMACs = new SelectList(model, "AIRMAC", "AIRMAC");
            var customers = await GetCustomerList(new Customer());
            ViewBag.SelectCustomerList = new SelectList(customers, "Id", "Name");
            var hardwareModems = await _hardwareComponentService.List(new HardwareComponent()
            {
                SearchBy = "HC.HardwareTypeId",
                Keyword = Convert.ToInt32(HardwareType.Modem).ToString()
            });
            ViewBag.SelectHardwareModems = new SelectList(hardwareModems, "Id", "HCValue");

            return View(model);
        }
        public async Task<List<HardwareComponentRegistrationModel>> GetRegisteredAIRMACList()
        {
            var retList = new List<HardwareComponentRegistrationModel>();
            var serviceResult = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { Flag = "RegisteredAIRMAC" });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<HardwareComponentRegistrationModel>>(serviceResult);
            }
            return retList;
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        public async Task<IActionResult> Add()
        {
            var unRegisteredAIRMACs = new List<HardwareComponentRegistrationModel>();
            var model = new HardwareComponentRegistrationModel();
            var customers = await GetCustomerList(new Customer());
            model.CustomerSelectList = new SelectList(customers, "Id", "Name");
            var resultUnRegAIRMACs = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { Flag = "UnRegisteredAIRMAC" });
            if (resultUnRegAIRMACs.Any())
            {
                unRegisteredAIRMACs = _mapper.Map<List<HardwareComponentRegistrationModel>>(resultUnRegAIRMACs);
            }
            ViewBag.AIRMACs = unRegisteredAIRMACs;
            ViewBag.SelectUnRegisteredAIRMACs = new SelectList(unRegisteredAIRMACs, "AIRMAC", "AIRMAC");
            var hardwareModems = await _hardwareComponentService.List(new HardwareComponent()
            {
                SearchBy = "HC.HardwareTypeId",
                Keyword = Convert.ToInt32(HardwareType.Modem).ToString()
            });
            ViewBag.SelectHardwareModems = new SelectList(hardwareModems, "Id", "HCValue");

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
                obj.Flag = "RegisterAIRMAC";
                statusModel = await _hardwareComponentRegistrationService.Update(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);

        }
        public async Task<IActionResult> GetAIRMACListByFilter(string modem, string airmac, string customer, string flag)
        {
            string view = "";
            int modemId = 0;
            
            if (modem != "")
            {
                modemId = Convert.ToInt32(modem);
            }
            var registerModel = new HardwareComponentRegistrationModel();
            var airmacs = new List<HardwareComponentRegistrationModel>();
            var resultAIRMACs = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { Flag = flag, HardwareComponentId=modemId, AIRMAC=airmac, CustomerId=Convert.ToInt32(customer)});
            if (resultAIRMACs.Any())
            {
                airmacs = _mapper.Map<List<HardwareComponentRegistrationModel>>(resultAIRMACs);
            }
            ViewBag.AIRMACs = airmacs;
            if (flag == "RegisteredAIRMAC")
            {
                return PartialView("_List", airmacs);
            }
            else if(flag == "UnRegisteredAIRMAC")
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
