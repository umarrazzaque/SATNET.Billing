using SATNET.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Interface
{
    public interface IOrderService
    {
        public Task<Order> Get(int id);
        public Task<List<Order>> List();
        public Task<StatusModel> Add(Order order);
        public Task<StatusModel> Update(Order order);
        public Task<StatusModel> Delete(int id, int deletedBy);
    }
}
