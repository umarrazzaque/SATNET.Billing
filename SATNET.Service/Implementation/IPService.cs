using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace SATNET.Service.Implementation
{
    public class IPService : IService<IP>
    {
        private readonly IRepository<IP> _ipRepository;
        public IPService(IRepository<IP> ipRepository)
        {
            _ipRepository = ipRepository;
        }
        public async Task<IP> Get(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<IP>> List(IP obj)
        {
            return await _ipRepository.List(obj);
        }
        public async Task<StatusModel> Add(IP IP)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Update(IP IP)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

    }
}
