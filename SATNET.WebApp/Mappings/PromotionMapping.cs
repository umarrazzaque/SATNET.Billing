using SATNET.Domain;
using SATNET.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Mappings
{
    public class PromotionMapping
    {
        public static Promotion GetEntity(PromotionViewModel model)
        {
            Promotion obj = new Promotion()
            {
                Id = model.Id,
                Name = model.Name,
            };
            return obj;
        }
        public static PromotionViewModel GetViewModel(Promotion entity)
        {
            PromotionViewModel model = new PromotionViewModel()
            {
                Id = entity.Id,
                Name = entity.Name,
            };
            return model;
        }
        public static List<PromotionViewModel> GetListViewModel(List<Promotion> entityList)
        {
            List<PromotionViewModel> modelList = new List<PromotionViewModel>();
            entityList.ForEach(i =>
            {
                modelList.Add(GetViewModel(i));
            });
            return modelList;
        }
    }
}
