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
        private readonly ILookupService _lookupService;
        private readonly IMapper _mapper;
        public CustomerController(IService<Customer> customerService, ILookupService lookupService,
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
                CustomerType = GetCustomerTypeList()
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
                status = _customerService.Add(customerObj).Result;
                
            }
            else
            {
                status.ErrorCode = "Error occured see entity validation errors.";
            }
            //'
            status.Html = RenderViewToString(this, "Index", await GetCustomersList());
            return Json(status);

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Customer customer = await _customerService.Get(id);
            CreateCustomerModel customerModel = new CreateCustomerModel
            {
                CustomerModel = _mapper.Map<CustomerModel>(customer),
                CustomerType = GetCustomerTypeList()
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
            List<CustomerModel> CustomerListModel = new List<CustomerModel>();
            var serviceResult = await _customerService.List();
            if (serviceResult.Any())
            {
                serviceResult.ForEach(res =>
                {

                    CustomerModel customer = _mapper.Map<CustomerModel>(res);
                    CustomerListModel.Add(customer);
                });
            }
            return CustomerListModel;
        }

        private List<LookUpModel> GetCustomerTypeList()
        {
            //var result = await _lookupService.ListByFilter(Convert.ToInt32(LookupTypes.PlanType));
            List<LookUpModel> customerTypeList = new List<LookUpModel>
            {
                new LookUpModel{ Id = 1, Name = "Partner"},
                new LookUpModel{ Id = 2, Name = "Distributor"}
            };
            return customerTypeList;
            //return result;
        }
    }
}