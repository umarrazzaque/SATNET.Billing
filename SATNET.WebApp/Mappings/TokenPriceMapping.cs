using SATNET.Domain;
using SATNET.WebApp.Models.TokenPrice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Mappings
{
    public class TokenPriceMapping
    {
        public static TokenPrice GetEntity(TokenPriceViewModel model)
        {
            TokenPrice entity = new TokenPrice()
            {
                Id = model.Id,
                Token = model.Token,
                TokenId = model.TokenId,
                PriceTier = model.PriceTier,
                PriceTierId=model.PriceTierId,
                Price = model.Price
            };
            return entity;
        }
        public static TokenPriceViewModel GetViewModel(TokenPrice entity)
        {
            TokenPriceViewModel model = new TokenPriceViewModel()
            {
                Id = entity.Id,
                Token = entity.Token,
                TokenId = entity.TokenId,
                PriceTier = entity.PriceTier,
                PriceTierId = entity.PriceTierId,
                Price = entity.Price
            };
            return model;
        }
        public static List<TokenPriceViewModel> GetListViewModel(List<TokenPrice> entityList)
        {
            List<TokenPriceViewModel> modelList = new List<TokenPriceViewModel>();
            entityList.ForEach(i =>
            {
                modelList.Add(GetViewModel(i));
            });
            return modelList;
        }
    }
}
