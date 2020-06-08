using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace SATNET.Service.Interface
{
    public interface IPackageService
    {
        public Task<Package> Get(int id);
        public Task<List<Package>> List();
        public Task<StatusModel> Add(Package package);
        public Task<StatusModel> Update(Package package);
        public Task<StatusModel> Delete(int recId, int deletedBy);
    }
}
