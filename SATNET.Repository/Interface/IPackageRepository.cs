using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Interface
{
    public interface IPackageRepository
    {

        public Task<Package> Get(int id);
        public Task<List<Package>> List();
        public Task<int> Add(Package package);
        public Task<int> Update(Package package);
        public Task<int> Delete(int id, int deletedBy);
    }
}
