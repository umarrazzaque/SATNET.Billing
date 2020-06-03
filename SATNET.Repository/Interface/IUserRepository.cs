using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Interface
{
    public interface IUserRepository
    {

        public Task<User> GetUserById(int id);
        public Task<List<User>> GetAllUsers();
        public Task<bool> AddUser(User user);
        public Task<bool> UpdateUser(User user);
        public Task<bool> DeleteUser(int id);
    }
}
