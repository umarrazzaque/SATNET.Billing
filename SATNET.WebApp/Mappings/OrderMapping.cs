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
                SiteName = obj.SiteName,
                ServicePlanName = obj.ServicePlanName,
                RequestTypeName = obj.RequestTypeName,
                SubscriberName = obj.SubscriberName,
                ServiceOrderDate = obj.CreatedOn,
                StatusName = obj.StatusName
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
                StatusName = model.StatusName
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
