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
        private readonly IService<Hardware> _hardwareService;
        public HardwareController(IService<Hardware> hardwareService)
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
                    Id = 0,
                    HKit = hardwareModel.HKit,
                    Modem = hardwareModel.Modem,
                    Antenna = hardwareModel.Antenna,
                    Transceiver = hardwareModel.Transceiver,
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
                HKit = hardware.HKit,
                Modem = hardware.Modem,
                Antenna = hardware.Antenna,
                Transceiver = hardware.Transceiver
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
                Id = 0,
                HKit = hardwareModel.HKit,
                Modem = hardwareModel.Modem,
                Antenna = hardwareModel.Antenna,
                Transceiver = hardwareModel.Transceiver,
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
                HKit = hardware.HKit,
                Modem = hardware.Modem,
                Antenna = hardware.Antenna,
                Transceiver = hardware.Transceiver
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
                        HKit = har.HKit,
                        Modem = har.Modem,
                        Antenna = har.Antenna,
                        Transceiver = har.Transceiver
                    };
                    hardwareListModel.Add(hardware);
                });
            }
            return hardwareListModel;
        }

    }
}