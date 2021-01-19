using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Implementation.Extensions;
using SATNET.Service.Interface;
using SATNET.WebApp.Areas.Identity.Data;
using SATNET.WebApp.Models.Hardware;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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

        [Authorize(Policy = "ManageLogisticsPolicy")]
        public  IActionResult ImportRegisteredAirMAC()
        {
            var resultModel = new ImportHardwareComponentModel()
            {
                HardwareComponentRegistrationModel = new HardwareComponentRegistrationModel()
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> ImportRegisteredAirMacFile(IFormFile inputFile)
        {
            try
            {
                //check for inputFile extension
                if (!Path.GetExtension(inputFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new StatusModel() { IsSuccess = false, ErrorCode = "File Extension must be {.xlsx}." });
                }
                //get filename
                string fileName = ContentDispositionHeaderValue.Parse(inputFile.ContentDisposition).FileName.Trim('"');
                //set file path on server machine
                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot", ImportExportDirectoryPath.ExcelImportPath, fileName);
                //copy the file to server location for temporary storage
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await inputFile.CopyToAsync(stream);
                }
                //set excel configuration
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var retModel = new ImportHardwareComponentModel();
                //set stream to read the file from temporary upload location
                using (var stream = new MemoryStream())
                {
                    await inputFile.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            string briefDescription = "";
                            bool isExist = false;
                            int hardwareComponentId = -1;
                            
                            //check hardware component
                            string hardwareComponent = worksheet.Cells[row, 4].Value != null ? worksheet.Cells[row, 4].Value.ToString().Trim() : "";
                            if (hardwareComponent != "")
                            {
                                //try to avoid db call by checking from linq list
                                hardwareComponentId = SpecificationExists("HC", "HC.HCValue", hardwareComponent);
                                briefDescription += hardwareComponentId != -1 ? "MODEM MODEL NUMBER EXISTS - " : "MODEM NUMBER NOT EXISTS - ";
                            }
                            else
                            {
                                briefDescription += "Modem Model Number is empty - ";
                            }
                            //check serial number
                            string serialNumber = worksheet.Cells[row, 2].Value != null ? worksheet.Cells[row, 2].Value.ToString().Trim() : "";
                            //Check in ExistingList
                            if (serialNumber != "")
                            {
                                isExist = retModel.HardwareComponentImportList.Where(c => c.SerialNumber.Equals(serialNumber)).ToList().Count > 0 ? true : false;
                                briefDescription += isExist == true ? "Duplicate Serail Number - " : (SpecificationExists("AIRMAC", "HCR.SerialNumber", serialNumber) > 0 ? "SERIAL NUMBER EXISTS - " : "SERIAL NUMBER NOT EXISTS -");
                            }
                            else
                            {
                                briefDescription += "Serial Number is empty - ";
                            }
                            //check airmac number
                            string airMac = worksheet.Cells[row, 3].Value != null ? worksheet.Cells[row, 3].Value.ToString().Trim() : "";
                            if (airMac != "")
                            {
                                isExist = retModel.HardwareComponentImportList.Where(c => c.AIRMAC.Equals(airMac)).ToList().Count > 0 ? true : false;
                                briefDescription += isExist == true ? "Duplicate AIRMAC Number" : (SpecificationExists("AIRMAC", "HCR.AIRMAC", airMac) > 0 ? "AIRMAC NUMBER EXISTS." : "AIRMAC NUMBER NOT EXISTS.");
                            }
                            else
                            {
                                briefDescription += "AIRMAC Number is empty";
                            }
                            //check if each specification exist in the records
                            if (serialNumber.Contains("NUMBER EXISTS") && airMac.Contains("NUMBER EXISTS") && hardwareComponent.Contains("NUMBER EXISTS"))
                            {
                                //check the specs exist as a single record and not assigned to any customers
                                //check the modem model with airmac and serial number exists
                                //if the record exist then assign it to a customer
                                var obj = new HardwareComponentRegistration()
                                {
                                    Flag = "CHECK_AIRMAC_EXIST_IMPORT",
                                    HardwareComponentId = Convert.ToInt32(HardwareType.Modem),
                                    AIRMAC = airMac,
                                    SerialNumber = serialNumber,
                                    HardwareComponent = hardwareComponent
                                };
                                var reslist = _hardwareComponentRegistrationService.List(obj).Result;
                                if (reslist.Count == 1) {
                                    //there should be a sinlge record return from procedure call
                                    //add record in local list with insertion flag ok
                                }
                            }
                            else
                            {
                                retModel.HardwareComponentImportList.Add(new HardwareComponentRegistrationModel
                                {
                                    SerialNumber = serialNumber,
                                    AIRMAC = airMac,
                                    HardwareComponent = hardwareComponent,
                                    HardwareComponentId = hardwareComponentId,
                                    BriefDescription = briefDescription
                                });
                            }

                        }
                        retModel.isSuccess = retModel.HardwareComponentImportList.Where(c => c.BriefDescription.Contains("Number")).ToList().Count > 0 ? false : true;
                        if (retModel.isSuccess)
                        {
                            //Valid records, add records in DB
                            var hc_list = _mapper.Map<List<HardwareComponentRegistration>>(retModel.HardwareComponentImportList);
                            var res = await _hardwareComponentRegistrationService.AddBulk(hc_list);
                            if (res.IsSuccess == false)
                            {
                                retModel.HardwareComponentImportList.FirstOrDefault().BriefDescription += "-Number";
                            }
                        }
                        else
                        {
                        }
                    }
                }
                return PartialView("_AirMACPartialList", retModel.HardwareComponentImportList);
            }
            catch (Exception e)
            {
                return Json(new StatusModel() { IsSuccess = false, ErrorCode = e.Message + "Error while importing file." });
            }
        }
        public async Task<IActionResult> GetAIRMACListByFilter(string modem, string airmac, string customer, string flag)
        {
            int modemId = 0;
            if (modem != "")
            {
                modemId = Convert.ToInt32(modem);
            }
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

        //temp functions
        /// <summary>
        /// 
        /// </summary>
        /// <param name="section">HC or other</param>
        /// <param name="searchBy">AIRMAC Number</param>
        /// <param name="specs">Serial Number</param>
        /// <returns>Provides the count for the specification, if there is any record it will provide the list count or the id of first record</returns>
        private int SpecificationExists(string section, string searchBy, string specs)
        {
            var obj = new HardwareComponentRegistration()
            {
                Flag = "CheckSpecificationExists",
                Keyword = section,//Hardware Component
                SearchBy = searchBy,
                SortOrder = specs,
            };
            var reslist = _hardwareComponentRegistrationService.List(obj).Result;
            int resultCount;
            if (section.Equals("HC"))
            {
                if (reslist.Any())
                {
                    resultCount = reslist.FirstOrDefault().Id;
                }
                else
                {
                    resultCount = -1;
                }
            }
            else
            {
                resultCount = reslist.Count;
            }
            return resultCount;
        }
    }
}
