using SATNET.Domain;
using SATNET.WebApp.Models.IPPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Mappings
{
    public class IPPriceMapping
    {
        public static IPPrice GetEntity(IPPriceViewModel model)
        {
            IPPrice ipPrice = new IPPrice()
            {
                Id = model.Id,
                IPId = model.IPId,
                PriceTierId = model.PriceTierId,
                Price = model.Price

            };
            return ipPrice;
        }
        public static IPPriceViewModel GetViewModel(IPPrice entity)
        {
            IPPriceViewModel model = new IPPriceViewModel()
            {
                Id = entity.Id,
                IPId = entity.IPId,
                PriceTierId = entity.PriceTierId,
                Price = entity.Price,
                IP = entity.IP,
                PriceTier = entity.PriceTier
            };
            return model;
        }
        public static List<IPPriceViewModel> GetListViewModel(List<IPPrice> entityList)
        {
            List<IPPriceViewModel> modelList = new List<IPPriceViewModel>();
            entityList.ForEach(i =>
            {
                modelList.Add(GetViewModel(i));
            });
            return modelList;
        }
    }
}
