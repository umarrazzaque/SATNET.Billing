using AutoMapper;
using SATNET.Domain;
using SATNET.Domain.Reporting;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Hardware;
using SATNET.WebApp.Models.Invoice;
using SATNET.WebApp.Models.Lookup;
using SATNET.WebApp.Models.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerModel>();
            CreateMap<CustomerModel, Customer>();
            CreateMap<Lookup, LookUpModel>();
            CreateMap<LookUpModel, Lookup>();
            CreateMap<Site, SiteModel>().ReverseMap();
            CreateMap<ServicePlan, ServicePlanModel>().ReverseMap();
            CreateMap<ServicePlanPrice, ServicePlanPriceModel>().ReverseMap();
            CreateMap<Hardware, HardwareModel>().ReverseMap();
            CreateMap<Lookup, LookUpModel>().ReverseMap();
            CreateMap<HardwareComponent, HardwareComponentModel>().ReverseMap();
            CreateMap<HardwareKit, HardwareKitModel>().ReverseMap();
            CreateMap<HardwareComponentPrice, HardwareComponentPriceModel>().ReverseMap();
            CreateMap<HardwareComponentRegistration, HardwareComponentRegistrationModel>().ReverseMap();
            CreateMap<SOInvoice, SOInvoiceViewModel>().ReverseMap();
            CreateMap<SOInvoiceItem, SOInvoiceItemViewModel>().ReverseMap();
            CreateMap<MRCInvoice, MRCInvoiceViewModel>().ReverseMap();
            CreateMap<ReceivablePerCategory, ReceivablePerCategoryViewModel>().ReverseMap();
            CreateMap<SystemLog, SystemLogViewModel>().ReverseMap();
        }
    }
}
