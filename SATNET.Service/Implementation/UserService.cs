using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class UserService : IService<User>
    {
        private readonly IRepository<User> _userRepository;
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<StatusModel> Add(User obj)
        {
            throw new NotImplementedException();
        }

        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> List(User obj)
        {
            return await _userRepository.List(obj);
        }

        public async Task<StatusModel> Update(User obj)
        {
            throw new NotImplementedException();
        }

    }
}
