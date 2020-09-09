using SATNET.Domain;
using SATNET.WebApp.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Mappings
{
    public class UserMapping
    {
        public static UserViewModel GetViewModel(User obj)
        {
            UserViewModel model = new UserViewModel()
            {
                Id = obj.Id,
                FirstName = obj.FirstName,
                LastName = obj.LastName,
                //UserName = obj.UserName,
                Email = obj.Email,
                Contact = obj.Contact,
                CustomerId = obj.CustomerId,
                CustomerName = obj.CustomerName,
                Roles = obj.Roles,
                UserTypeId=obj.UserTypeId,
                UserTypeName= obj.UserTypeName
            };
            return model;
        }

        public static User GetEntity(UserViewModel model)
        {
            User obj = new User()
            {
                //Id = model.Id,
                //UserName=model.UserName,
                //UserTypeId=model.UserTypeId,
                //CustomerId=model.CustomerId,
                //Roles = model.Roles,
                
            };
            return obj;
        }
        public static List<UserViewModel> GetListViewModel(List<User> entityList)
        {
            List<UserViewModel> model = new List<UserViewModel>();
            entityList.ForEach(o =>
            {
                model.Add(GetViewModel(o));
            });
            return model;
        }

    }
}
