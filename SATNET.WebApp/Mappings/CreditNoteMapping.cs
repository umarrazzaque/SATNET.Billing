using SATNET.Domain;
using SATNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Mappings
{
    public class CreditNoteMapping
    {
        public static CreditNote GetEntity(CreditNoteViewModel model)
        {
            CreditNote obj = new CreditNote()
            {
                Id = model.Id,
                Amount=model.Amount,
                Details = model.Details,
                InvoiceId = model.InvoiceId,
                InvoiceNumber = model.InvoiceNumber,
                CreditNoteNumber=model.CreditNoteNumber
            };
            return obj;
        }
        public static CreditNoteViewModel GetViewModel(CreditNote entity)
        {
            CreditNoteViewModel model = new CreditNoteViewModel()
            {
                Id = entity.Id,
                Amount = entity.Amount,
                Details = entity.Details,
                InvoiceId = entity.InvoiceId,
                InvoiceNumber = entity.InvoiceNumber,
                CreditNoteNumber = entity.CreditNoteNumber,
                SiteName  =entity.SiteName,
                SiteArea = entity.SiteArea,
                SubscriberName = entity.SubscriberName,
                SiteCity = entity.SiteCity,
                InvoiceDate = entity.InvoiceDate,
                CreditNoteDate = entity.CreditNoteDate
            };
            return model;
        }
        public static List<CreditNoteViewModel> GetListViewModel(List<CreditNote> entityList)
        {
            List<CreditNoteViewModel> modelList = new List<CreditNoteViewModel>();
            entityList.ForEach(i =>
            {
                modelList.Add(GetViewModel(i));
            });
            return modelList;
        }
    }
}
