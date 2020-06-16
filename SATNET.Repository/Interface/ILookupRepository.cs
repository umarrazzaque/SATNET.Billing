using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Interface
{
    public interface ILookupRepository
    {
        public Task<List<Lookup>> ListByFilter(int lookupTypeId);
    }
}
