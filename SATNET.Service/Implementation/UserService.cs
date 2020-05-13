using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<User> GetUserById(int id)
        {
            throw new NotImplementedException();
        }
        public Task<List<User>> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
        public Task<bool> AddUser(User user)
        {
            throw new NotImplementedException();
        }
        public Task<bool> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
