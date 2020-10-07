using SATNET.Domain;
using SATNET.WebApp.Models.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Mappings
{
    public class TokenMapping
    {
        public static Token GetEntity(TokenViewModel model)
        {
            Token token = new Token()
            {
                Id = model.Id,
                Name = model.Name,
                Validity = model.Validity
            };
            return token;
        }
        public static TokenViewModel GetViewModel(Token entity)
        {
            TokenViewModel model = new TokenViewModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Validity = entity.Validity
            };
            return model;
        }
        public static List<TokenViewModel> GetListViewModel(List<Token> entityList)
        {
            List<TokenViewModel> modelList = new List<TokenViewModel>();
            entityList.ForEach(i =>
            {
                modelList.Add(GetViewModel(i));
            });
            return modelList;
        }
    }
}
