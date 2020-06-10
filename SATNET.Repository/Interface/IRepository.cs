using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Interface
{
    public interface IRepository<T>
    {
        public Task<T> Get(int id);
        public Task<List<T>> List();
        public Task<int> Add(T obj);
        public Task<int> Update(T obj);
        public Task<int> Delete(int id, int deletedBy);
    }
}
