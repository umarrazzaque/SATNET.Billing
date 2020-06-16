using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class LookupService : IService<Lookup>, ILookupService
    {
        private readonly IRepository<Lookup> _lookupRepository;
        private readonly ILookupRepository _lookupRepository2;
        public LookupService(IRepository<Lookup> lookupRepository, ILookupRepository lookupRepository2)
        {
            _lookupRepository = lookupRepository;
            _lookupRepository2 = lookupRepository2;
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

        public async Task<List<Lookup>> List()
        {
            throw new NotImplementedException();
        }

        public async Task<StatusModel> Update(Lookup obj)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Lookup>> ListByFilter(int lookupTypeId)
        {
            return await _lookupRepository2.ListByFilter(lookupTypeId);
        }


    }
}
