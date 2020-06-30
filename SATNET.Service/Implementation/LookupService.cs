using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class LookupService : IService<Lookup>
    {
        private readonly IRepository<Lookup> _lookupRepository;
        public LookupService(IRepository<Lookup> lookupRepository)
        {
            _lookupRepository = lookupRepository;
        }

        public async Task<StatusModel> Add(Lookup obj)
        {
            throw new NotImplementedException();
        }

        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

        public async Task<Lookup> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Lookup>> List(Lookup obj)
        {
            return await _lookupRepository.List(obj);
        }

        public async Task<StatusModel> Update(Lookup obj)
        {
            throw new NotImplementedException();
        }
    }
}
