using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Service;
using SATNET.Service.Implementation;
using SATNET.Service.Interface;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Lookup;

namespace SATNET.WebApp.Controllers
{
    public class CustomerController : BaseController
    {
        private readonly IService<Customer> _customerService;
        private readonly IService<Lookup> _lookupService;
        private readonly IMapper _mapper;

        public CustomerController(IService<Customer> customerService, IService<Lookup> lookupService,
            IMapper mapper)
        {
            _customerService = customerService;
            _lookupService = lookupService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            return View(await GetCustomersList());
        }

        public async Task<IActionResult> Reset()
        {
            return Json(new { isValid = true, html = RenderViewToString(this, "Index", await GetCustomersList()) });
        }
        [HttpGet]
        public IActionResult Add()
        {
            CreateCustomerModel customerModel = new CreateCustomerModel
            {
                CustomerModel = new CustomerModel(),
                CustomerType = GetCustomerTypeList().Result,
                PriceTier = GetPriceTierList().Result
            };
            return View(customerModel);
            //return Json(new { isValid = true, html = RenderViewToString(this, "Add", customerModel) });
        }


        [HttpPost]
        public async Task<IActionResult> Add(CreateCustomerModel createCustomerModel)
        {
            CustomerModel customerModel = createCustomerModel.CustomerModel;
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Customer/Index" };
            if (ModelState.IsValid)
            {
                Customer customerObj = _mapper.Map<Customer>(customerModel);
                status = await _customerService.Add(customerObj);

            }
            else
            {
                status.ErrorCode = "Error occured see entity validation errors.";
            }
            //'
            //status.Html = RenderViewToString(this, "Index", await GetCustomersList());
            return Json(status);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Customer customer = await _customerService.Get(id);
            CreateCustomerModel customerModel = new CreateCustomerModel
            {
                CustomerModel = _mapper.Map<CustomerModel>(customer),
                CustomerType = GetCustomerTypeList().Result,
                PriceTier = GetPriceTierList().Result
            };
            return View(customerModel);
            //var status = new StatusModel
            //{
            //    IsSuccess = true,
            //    Html = RenderViewToString(this, "Edit", customerModel)
            //};
            //return Json(status);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CreateCustomerModel createCustomerModel)
        {

            Customer customerObj = _mapper.Map<Customer>(createCustomerModel.CustomerModel);
            var status = _customerService.Update(customerObj).Result;
            status.Html = RenderViewToString(this, "Index", await GetCustomersList());
            return Json(status);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            Customer customer = await _customerService.Get(id);
            CustomerModel customerModel = _mapper.Map<CustomerModel>(customer);
            //var status = new StatusModel
            //{
            //    IsSuccess = true,
            //    Html = RenderViewToString(this, "Details", customerModel)
            //};
            //return Json(status);
            return View(customerModel);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            //1  as loged in user id
            var status = _customerService.Delete(id, 1).Result;
            status.Html = RenderViewToString(this, "Index", await GetCustomersList());
            return Json(status);
        }

        private async Task<List<CustomerModel>> GetCustomersList()
        {
            List<CustomerModel> customerListModel = new List<CustomerModel>();
            var serviceResult = await _customerService.List(new Customer { });
            if (serviceResult.Any())
            {
                customerListModel = _mapper.Map<List<CustomerModel>>(serviceResult);
            }
            return customerListModel;
        }

        private async Task<List<LookUpModel>> GetCustomerTypeList()
        {
            List<LookUpModel> customerTypeListModel = new List<LookUpModel>();
            var retList = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.CustomerType) });
            if (retList.Any())
            {
                customerTypeListModel = _mapper.Map<List<LookUpModel>>(retList);

            }
            return customerTypeListModel;
        }

        private async Task<List<LookUpModel>> GetPriceTierList()
        {
            List<LookUpModel> customerTypeListModel = new List<LookUpModel>();
            var retList = await _lookupService.List(new Lookup() { LookupTypeId = Convert.ToInt32(LookupTypes.PriceTier) });
            if (retList.Any())
            {
                customerTypeListModel = _mapper.Map<List<LookUpModel>>(retList);
            }
            return customerTypeListModel;

        }
    }
}
