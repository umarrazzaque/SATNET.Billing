using SATNET.Domain;
using SATNET.WebApp.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Mappings
{
    public class SOInvoiceMapping
    {
        public static SOInvoiceViewModel GetViewModel(SOInvoice obj)
        {
            SOInvoiceViewModel model = new SOInvoiceViewModel()
            {
                Id = obj.Id,
                InvoiceNumber = obj.InvoiceNumber,
                OrderNumber = obj.OrderNumber,
                Status = obj.Status,
                DueDate = obj.DueDate,
                RequestType = obj.RequestType,
                Total = obj.Total,
                IP=obj.IP,
                IPPrice = obj.IPPrice,
                ServicePlan=obj.ServicePlan,
                ServicePlanPrice = obj.ServicePlanPrice,
                ProRataPrice = obj.ProRataPrice,
                IPProRataPrice = obj.IPProRataPrice,
                ProRataQuota = obj.ProRataQuota,
                ServicePlanUnit = obj.ServicePlanUnit,
                SiteArea = obj.SiteArea,
                SiteCity = obj.SiteCity,
                SiteName = obj.SiteName,
                SubscriberName = obj.SubscriberName,
                SubscriberEmail=obj.SubscriberEmail,
                RequestTypeId = obj.RequestTypeId,
                ServicePlanTypeId = obj.ServicePlanTypeId,
                ServicePlanType = obj.ServicePlanType,
                CreatedOn = obj.CreatedOn,
                Token = obj.Token,
                TokenPrice = obj.TokenPrice,
                PlannedInstallationDate = obj.PlannedInstallationDate,
                Validity = obj.Validity,
                ServicePlanTotal=obj.ServicePlanTotal,
                PromotionRebate = obj.PromotionRebate,
                ScheduleDate=obj.ScheduleDate
            };
            return model;
        }

        public static List<SOInvoiceViewModel> GetListViewModel(List<SOInvoice> entityList)
        {
            List<SOInvoiceViewModel> model = new List<SOInvoiceViewModel>();
            entityList.ForEach(i =>
            {
                model.Add(GetViewModel(i));
            });
            return model;
        }

    }
}
