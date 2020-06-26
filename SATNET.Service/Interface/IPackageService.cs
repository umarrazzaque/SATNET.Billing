using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace SATNET.Service.Interface
{
    public interface IPackageService
    {
        public Task<ServicePlan> Get(int id);
        public Task<List<ServicePlan>> List();
        public Task<StatusModel> Add(ServicePlan package);
        public Task<StatusModel> Update(ServicePlan package);
        public Task<StatusModel> Delete(int recId, int deletedBy);
    }
}
