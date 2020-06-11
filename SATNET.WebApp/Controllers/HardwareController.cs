using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Service;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;

namespace SATNET.WebApp.Controllers
{
    public class HardwareController : BaseController
    {
        private readonly IServices<Hardware> _hardwareService;
        public HardwareController(IServices<Hardware> hardwareService)
        {
            _hardwareService = hardwareService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetHardwareList());
        }

        public async Task<IActionResult> Reset()
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", await GetHardwareList()) });
        }
        public IActionResult Add()
        {
            HardwareModel hardwareModel = new HardwareModel();

            return Json(new { isValid = true, html = RenderViewToString(this, "Add", hardwareModel) });
        }


        [HttpPost]
        public async Task<IActionResult> Add(HardwareModel hardwareModel)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Hardware/Index" };
            if (ModelState.IsValid)
            {
                status = _hardwareService.Add(new Hardware
                {
                    ModemSerialNo = hardwareModel.ModemSerialNo,
                    ModemModel = hardwareModel.ModemModel,
                    MACAirNo = hardwareModel.MACAirNo,
                    AntennaSize = hardwareModel.AntennaSize,
                    AntennaSrNo = hardwareModel.AntennaSrNo,
                    TransceiverSrNo = hardwareModel.TransceiverSrNo,
                    TransceiverWAAT = hardwareModel.TransceiverWAAT,
                    Price = hardwareModel.Price,
                    CreatedBy = 1
                }).Result;
            }
            else
            {
                status.ErrorCode = "Error occured see entity validation errors.";
            }
            status.Html = RenderViewToString(this, "Index", await GetHardwareList());
            return Json(status);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Hardware hardware = await _hardwareService.Get(id);
            HardwareModel hardwareModel = new HardwareModel
            {
                HardwareId = hardware.Id,
                ModemSerialNo = hardware.ModemSerialNo,
                ModemModel = hardware.ModemModel,
                MACAirNo = hardware.MACAirNo,
                AntennaSize = hardware.AntennaSize,
                AntennaSrNo = hardware.AntennaSrNo,
                TransceiverSrNo = hardware.TransceiverSrNo,
                TransceiverWAAT = hardware.TransceiverWAAT,
                Price = hardware.Price,
            };
            var status = new StatusModel
            {
                IsSuccess = true,
                Html = RenderViewToString(this, "Edit", hardwareModel)
            };
            return Json(status);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(HardwareModel hardwareModel)
        {
            var status = _hardwareService.Update(new Hardware
            {
                Id = hardwareModel.HardwareId,
                ModemSerialNo = hardwareModel.ModemSerialNo,
                ModemModel = hardwareModel.ModemModel,
                MACAirNo = hardwareModel.MACAirNo,
                AntennaSize = hardwareModel.AntennaSize,
                AntennaSrNo = hardwareModel.AntennaSrNo,
                TransceiverSrNo = hardwareModel.TransceiverSrNo,
                TransceiverWAAT = hardwareModel.TransceiverWAAT,
                Price = hardwareModel.Price,
                CreatedBy = 1
            }).Result;
            status.Html = RenderViewToString(this, "Index", await GetHardwareList());
            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Hardware hardware = await _hardwareService.Get(id);
            HardwareModel hardwareModel = new HardwareModel
            {
                HardwareId = hardware.Id,
                ModemSerialNo = hardware.ModemSerialNo,
                ModemModel = hardware.ModemModel,
                MACAirNo = hardware.MACAirNo,
                AntennaSize = hardware.AntennaSize,
                AntennaSrNo = hardware.AntennaSrNo,
                TransceiverSrNo = hardware.TransceiverSrNo,
                TransceiverWAAT = hardware.TransceiverWAAT,
                Price = hardware.Price,
            };
            var status = new StatusModel
            {
                IsSuccess = true,
                Html = RenderViewToString(this, "Details", hardwareModel)
            };
            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var status = _hardwareService.Delete(id, 1).Result;
            status.Html = RenderViewToString(this, "Index", await GetHardwareList());
            return Json(status);
        }

        private async Task<List<HardwareModel>> GetHardwareList()
        {
            //PackageModelList packageList = new PackageModelList();
            //packageList.MenuModel = SetLayoutContent(heading: "Hardware",subHeading: "Listing");

            List<HardwareModel> hardwareListModel = new List<HardwareModel>();
            var serviceResult = await _hardwareService.List();
            if (serviceResult.Any())
            {
                serviceResult.ForEach(har =>
                {
                    HardwareModel hardware = new HardwareModel()
                    {
                        HardwareId = har.Id,
                        ModemSerialNo = har.ModemSerialNo,
                        ModemModel = har.ModemModel,
                        MACAirNo = har.MACAirNo,
                        AntennaSize = har.AntennaSize,
                        AntennaSrNo = har.AntennaSrNo,
                        TransceiverSrNo = har.TransceiverSrNo,
                        TransceiverWAAT = har.TransceiverWAAT,
                        Price = har.Price,
                    };
                    hardwareListModel.Add(hardware);
                });
            }
            return hardwareListModel;
        }

    }
}