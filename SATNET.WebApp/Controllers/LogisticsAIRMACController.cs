using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Implementation.Extensions;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;
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
    public class LogisticsAIRMACController : BaseController
    {
        private readonly IService<HardwareComponentRegistration> _hardwareComponentRegistrationService;
        private readonly IService<HardwareComponent> _hardwareComponentService;
        private readonly IService<Lookup> _lookUpService;
        private readonly IMapper _mapper;
        private readonly string _responseUrl;
        public LogisticsAIRMACController(IMapper mapper, IService<Lookup> lookUpService, IService<HardwareComponentRegistration> hardwareComponentRegistrationService,
            IService<HardwareComponent> hardwareComponentService)
        {
            _hardwareComponentRegistrationService = hardwareComponentRegistrationService;
            _hardwareComponentService = hardwareComponentService;
            _lookUpService = lookUpService;
            _mapper = mapper;
            _responseUrl = "/LogisticsAIRMAC/Index";
        }
        [Authorize(Policy = "ReadOnlyLogisticsPolicy")]
        public async Task<IActionResult> Index()
        {
            return View(await GetAirmacList());
        }
        private async Task<List<HardwareComponentRegistrationModel>> GetAirmacList()
        {
            var retList = new List<HardwareComponentRegistrationModel>();
            var serviceResult = await _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { SearchBy = "HC.HardwareTypeId", Keyword = Convert.ToInt32(HardwareType.Modem).ToString() });
            if (serviceResult.Any())
            {
                retList = _mapper.Map<List<HardwareComponentRegistrationModel>>(serviceResult);
            }
            return retList;
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        public async Task<IActionResult> Add()
        {
            var resultModel = new CreateHardwareComponentRegistrationModel()
            {
                HardwareComponentRegistrationModel = new HardwareComponentRegistrationModel(),


            };
            var hardwareModems = await _hardwareComponentService.List(new HardwareComponent()
            {
                SearchBy = "HC.HardwareTypeId",
                Keyword = Convert.ToInt32(HardwareType.Modem).ToString()
            });
            resultModel.HardwareComponentList = _mapper.Map<List<HardwareComponentModel>>(hardwareModems);
            return View(resultModel);
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Add(CreateHardwareComponentRegistrationModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            if (ModelState.IsValid)
            {
                var loginUserid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                HardwareComponentRegistration obj = _mapper.Map<HardwareComponentRegistration>(retModel.HardwareComponentRegistrationModel);
                obj.CreatedBy = loginUserid;
                statusModel = await _hardwareComponentRegistrationService.Add(obj);
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }

            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);

        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        public async Task<IActionResult> Edit(int id)
        {
            var obj = await _hardwareComponentRegistrationService.Get(id);
            var resultModel = new CreateHardwareComponentRegistrationModel()
            {
                HardwareComponentRegistrationModel = new HardwareComponentRegistrationModel()
                {
                    Id = obj.Id,
                    AIRMAC = obj.AIRMAC,
                    SerialNumber = obj.SerialNumber,
                    HardwareComponentId = obj.HardwareComponentId
                }
            };
            var hardwareModems = await _hardwareComponentService.List(new HardwareComponent()
            {
                Flag = "GET_BY_HARDWARE_TYPE",
                Keyword = Convert.ToInt32(HardwareType.Modem).ToString()
            });
            resultModel.HardwareComponentList = _mapper.Map<List<HardwareComponentModel>>(hardwareModems);
            return View(resultModel);
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        [HttpPost]
        public async Task<IActionResult> Edit(CreateHardwareComponentRegistrationModel retModel)
        {
            var statusModel = new StatusModel { IsSuccess = false, ResponseUrl = _responseUrl };
            bool isAllow = true;
            if (ModelState.IsValid)
            {
                var loginUserid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
                HardwareComponentRegistration obj = _mapper.Map<HardwareComponentRegistration>(retModel.HardwareComponentRegistrationModel);
                obj.UpdatedBy = loginUserid;
                var clist = _hardwareComponentRegistrationService.List(new HardwareComponentRegistration() { 
                    Flag = "CHECK_AIRMAC_SERIAL_EXIST",
                    Keyword = Convert.ToInt32(HardwareType.Modem).ToString(),
                    SearchBy = retModel.HardwareComponentRegistrationModel.SerialNumber,
                    SortOrder = retModel.HardwareComponentRegistrationModel.AIRMAC
                }).Result;
                if (clist.Any()) {
                    if (clist.FirstOrDefault().Id == retModel.HardwareComponentRegistrationModel.Id)
                    {
                        isAllow = true;
                    }
                    else {
                        isAllow = false;
                        statusModel.IsSuccess = false;
                        statusModel.ErrorCode = "Record already exist in DB!";
                    }
                }

                if (isAllow) {
                    statusModel = await _hardwareComponentRegistrationService.Update(obj);
                }
                
            }
            else
            {
                statusModel.ErrorCode = "Error occured see entity validation errors.";
            }
            statusModel.ResponseUrl = _responseUrl;
            return Json(statusModel);

        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var loginUserid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var statusModel = _hardwareComponentRegistrationService.Delete(id, loginUserid).Result;
            statusModel.Html = RenderViewToString(this, "Index", await GetAirmacList());
            return Json(statusModel);
        }
        [Authorize(Policy = "ManageLogisticsPolicy")]
        public IActionResult ImportAirMac()
        {
            var resultModel = new ImportHardwareComponentModel()
            {
                HardwareComponentRegistrationModel = new HardwareComponentRegistrationModel()
            };
            return View(resultModel);
        }
        [HttpPost]
        public async Task<IActionResult> ImportAirMACFile(IFormFile inputFile)
        {
            try {
                //check for inputFile extension
                if (!Path.GetExtension(inputFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return Json(new StatusModel() { IsSuccess = false, ErrorCode = "File Extension must be {.xlsx}." });
                }
                //get filename
                //string fileName = ContentDispositionHeaderValue.Parse(inputFile.ContentDisposition).FileName.Trim('"');
                //set file path on server machine
                //var path = Path.Combine(
                //            Directory.GetCurrentDirectory(), "wwwroot", ImportExportDirectoryPath.ExcelImportPath, fileName);
                ////copy the file to server location for temporary storage
                //using (var stream = new FileStream(path, FileMode.Create))
                //{
                //    await inputFile.CopyToAsync(stream);
                //}
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
                                briefDescription += hardwareComponentId == -1 ? "Modem Model Number Not Exists - " : "OK";
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
                                briefDescription += isExist == true ? "Duplicate Serail Number - " : (SpecificationExists("AIRMAC", "HCR.SerialNumber", serialNumber) > 0 ? "Serail Number Exist - " : "OK-");
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
                                briefDescription += isExist == true ? "Duplicate AIRMAC Number" : (SpecificationExists("AIRMAC", "HCR.AIRMAC", airMac) > 0 ? "AIRMAC Number Exist - " : "OK-");
                            }
                            else
                            {
                                briefDescription += "AIRMAC Number is empty";
                            }
                            if (serialNumber.Equals("") && airMac.Equals("") && hardwareComponent.Equals(""))
                            {
                                briefDescription += "OOOPPPPSSSS!";
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
            catch (Exception e) {
                return Json(new StatusModel() { IsSuccess = false, ErrorCode = e.Message + "Error while importing file." });
            }
            
        }
        private int SpecificationExists(string section, string searchBy, string specs) {
            int resultCount = -1;
            var obj = new HardwareComponentRegistration()
            {
                Flag = "CheckSpecificationExists",
                Keyword = section,//Hardware Component
                SearchBy = searchBy,
                SortOrder = specs,
            };
            var reslist =  _hardwareComponentRegistrationService.List(obj).Result;
            if (section.Equals("HC")) {
                if (reslist.Any()) {
                    resultCount = reslist.FirstOrDefault().Id;
                }
                else {
                    resultCount = -1;
                }
            } 
            else { 
                resultCount = reslist.Count;
            }
            return resultCount;
        }
        public async Task<IActionResult> CheckRecordinDB(string section, string[] specsArray)
        {
            string response = "";
            if (specsArray != null)
            {
                var statusModel = new StatusModel { IsSuccess = true, ResponseUrl = _responseUrl };
                if (section.Equals("AIRMAC"))
                {
                }
                else
                {
                }
                int recordCounter = 1;
                foreach (var item in specsArray)
                {
                    string[] specsObj = item.Split("---");
                    string serialNumber = specsObj[0];
                    string airMacNumber = specsObj[1];
                    var obj = new HardwareComponentRegistration()
                    {
                        Flag = "CHECK_SERIALNUMBER_EXIST",
                        Keyword = serialNumber,
                        AIRMAC = airMacNumber != null ? airMacNumber : "",
                        SearchBy = section
                    };
                    var reslist = await _hardwareComponentRegistrationService.List(obj);
                    if (reslist.Count > 0)
                    {
                        response += "Record Number " + recordCounter + ",";
                        statusModel.IsSuccess = false;
                    }
                    else
                    {
                        response += "";
                    }
                    recordCounter++;
                }
                response = response.Equals("") ? "" : response.Substring(0, response.Length - 1);
                statusModel.ErrorCode = response;
                return Json(statusModel);
            }
            else
            {
                return Json("Error in Model Binding");
            }
        }
    }
}
