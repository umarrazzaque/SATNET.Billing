using AutoMapper;
using SATNET.Domain;
using SATNET.WebApp.Models;
using SATNET.WebApp.Models.Lookup;
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
        }
    }
}
