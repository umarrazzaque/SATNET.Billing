using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SATNET.Domain;
using SATNET.WebApp.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Mappings
{
    public class OrderMapping
    {
        public static OrderViewModel GetViewModel(Order obj)
        {
            OrderViewModel model = new OrderViewModel()
            {
                Id = obj.Id,
                OrderNumber = obj.OrderNumber,
                SiteName = obj.SiteName,
                ServicePlanTypeName = obj.ServicePlanTypeName,
                ServicePlanName = obj.ServicePlanName,
                ServicePlanTypeId = obj.ServicePlanTypeId,
                RequestTypeName = obj.RequestTypeName,
                SubscriberName = obj.SubscriberName,
                ServiceOrderDate = obj.CreatedOn,
                StatusName = obj.StatusName,
                RequestTypeId = obj.RequestTypeId,
                SubscriberArea = obj.SubscriberArea,
                SubscriberCity = obj.SubscriberCity,
                SubscriberEmail = obj.SubscriberEmail,
                ServicePlanPrice=obj.ServicePlanPrice,
                ServicePlanUnit = obj.ServicePlanUnit,
                ServicePlanProRataPrice = obj.ServiceProRataPrice,
                HardwareModel = obj.HardwareModel,
                HardwarePrice = obj.HardwarePrice,
                IPName = obj.IPName,
                IPPrice=obj.IPPrice,
                Total = obj.Total,
ModemModel = obj.ModemModel,
TransceiverWATT=obj.TransceiverWATT,
AntennaSize = obj.AntennaSize,
StatusId = obj.StatusId,
CustomerId=obj.CustomerId,
CustomerShortName=obj.CustomerShortName,
CustomerCode = obj.CustomerCode,
ScheduleDateName=obj.ScheduleDateName,
RejectReason = string.IsNullOrEmpty(obj.RejectReason) ? "Not Available":obj.RejectReason,
CustomerName = obj.CustomerName,
SiteCity = obj.SiteCity,
SiteArea = obj.SiteArea,
InstallationDate = obj.InstallationDate,
Promotion = obj.Promotion,
MacAirNo = obj.MacAirNo,
CurrentIPName = obj.CurrentIPName,
CurrentSpName = obj.CurrentSpName,
CurrentSpTypeName = obj.CurrentSpTypeName,
ChangeSpName = obj.ChangeSpName,
ChangeSpTypeName = obj.ChangeSpTypeName,
DowngradeToTypeName = obj.DowngradeToTypeName,
DowngradeToName=obj.DowngradeToName,
UpgradeToTypeName=obj.UpgradeToTypeName,
UpgradeToName=obj.UpgradeToName,
Token = obj.Token,
NewMacAirNo = obj.NewMacAirNo,
Other = obj.Other,
CreatedOn = obj.CreatedOn,
CreatedByName=obj.CreatedByName,
CreatedByRole=obj.CreatedByRole,
ProRataQuota = obj.ProRataQuota
            };
            return model;
        }

        public static Order GetEntity(OrderViewModel model)
        {
            Order obj = new Order()
            {
                Id = model.Id,
                SiteName = model.SiteName,
                ServicePlanName = model.ServicePlanName,
                RequestTypeName = model.RequestTypeName,
                SubscriberName = model.SubscriberName,
                StatusName = model.StatusName,
                RequestTypeId = model.RequestTypeId,
                ProRataQuota = model.ProRataQuota,
                ServicePlanTypeId=model.ServicePlanTypeId
            };
            return obj;
        }
        public static List<OrderViewModel> GetListViewModel(List<Order> entityList)
        {
            List<OrderViewModel> model = new List<OrderViewModel>();
            entityList.ForEach(i =>
            {
                model.Add(GetViewModel(i));
            });
            return model;
        }
    }
}
