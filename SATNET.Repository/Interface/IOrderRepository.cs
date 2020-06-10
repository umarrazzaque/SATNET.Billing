using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Interface
{
    public interface IOrderRepository
    {
        public Task<Order> Get(int id);
        public Task<List<Order>> List();
        public Task<int> Add(Order order);
        public Task<int> Update(Order order);
        public Task<int> Delete(int id, int deletedBy);
    }
}
