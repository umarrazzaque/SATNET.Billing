using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class SystemLogService : IService<SystemLog>
    {
        private readonly IRepository<SystemLog> _systemLogRepository;
        public SystemLogService(IRepository<SystemLog> systemLogRepository)
        {
            _systemLogRepository = systemLogRepository;
        }

        public async Task<List<SystemLog>> List(SystemLog obj)
        {
            return await _systemLogRepository.List(obj);
        }

        public async Task<SystemLog> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<StatusModel> Add(SystemLog obj)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Update(SystemLog obj)
        {
            throw new NotImplementedException();
        }
        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }
    }
}
