using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Interface
{
    public interface IPackageRepository
    {

        public Task<ServicePlan> Get(int id);
        public Task<List<ServicePlan>> List();
        public Task<int> Add(ServicePlan package);
        public Task<int> Update(ServicePlan package);
        public Task<int> Delete(int id, int deletedBy);
    }
}
