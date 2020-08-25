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
                ServicePlanName = obj.ServicePlanName,
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
RejectReason = string.IsNullOrEmpty(obj.RejectReason) ? "Not Available":obj.RejectReason
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
                RequestTypeId = model.RequestTypeId
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
