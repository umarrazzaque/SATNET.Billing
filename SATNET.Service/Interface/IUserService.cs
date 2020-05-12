using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Service.Interface
{
    public interface IUserService
    {
        public User GetUserById(int id);
        public List<User> GetAllUsers();
        public bool AddUser(User user);
        public bool UpdateUser(User user);
        public bool DeleteUser(int id);
    }
}
