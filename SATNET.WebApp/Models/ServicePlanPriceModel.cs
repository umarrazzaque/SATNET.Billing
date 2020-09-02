﻿using SATNET.WebApp.Helpers;
using SATNET.WebApp.Models.Lookup;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models
{
    public class ServicePlanPriceModel : BaseModel
    {
        public ServicePlanPriceModel()
        {

        }
        [DisplayName("Plan Name")]
        public int ServicePlanId { get; set; }
        public string ServicePlanName { get; set; }
        [DisplayName("Plan Type")]
        public int PlanTypeId { get; set; }
        public string PlanType { get; set; }
        [DisplayName("Price Tier")]
        public int PriceTierId { get; set; }
        public string PriceTierName { get; set; }
        public decimal Price { get; set; }
        public decimal Markup { get; set; }
        [DisplayName("Markup %")]
        public decimal MarkupPercent { get; set; }
    }
    public class CreateServicePlanPriceModel : BaseModel
    {
        public CreateServicePlanPriceModel()
        {
            ServicePlanPriceModel = new ServicePlanPriceModel();
            ServicePlanList = new List<ServicePlanModel>();
            ServicePlanTypeList = new List<LookUpModel>();
            PriceTierList = new List<LookUpModel>();
        }
        public ServicePlanPriceModel ServicePlanPriceModel { get; set; }
        public List<ServicePlanModel> ServicePlanList { get; set; }
        public List<LookUpModel> ServicePlanTypeList { get; set; }
        public List<LookUpModel> PriceTierList { get; set; }
    }
}
