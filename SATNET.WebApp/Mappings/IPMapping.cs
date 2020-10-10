using SATNET.Domain;
using SATNET.WebApp.Models.IP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Mappings
{
    public class IPMapping
    {
        public static IP GetEntity(IPViewModel model)
        {
            IP ip = new IP()
            {
                Id=model.Id,
                Name = model.Name
            };
            return ip;
        }
        public static IPViewModel GetViewModel(IP entity)
        {
            IPViewModel model = new IPViewModel()
            {
                Id=entity.Id,
                Name = entity.Name
            };
            return model;
        }
        public static List<IPViewModel> GetListViewModel(List<IP> entityList)
        {
            List<IPViewModel> modelList = new List<IPViewModel>();
            entityList.ForEach(i =>
            {
                modelList.Add(GetViewModel(i));
            });
            return modelList;
        }
    }
}
