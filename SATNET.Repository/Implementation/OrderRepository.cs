using Dapper;
using Microsoft.Extensions.Configuration;
using SATNET.Domain;
using SATNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Implementation
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _config;
        public OrderRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<Order> Get(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Order>> List()
        {
            var orders = new List<Order>();
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var result = await con.QueryAsync<Order>("OrderList", commandType: CommandType.StoredProcedure);
                orders = result.ToList();
            }
            return orders;
        }
        public async Task<int> Add(Order order)
        {
            int orderId = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                orderId = await con.ExecuteScalarAsync<int>("InsertOrder", commandType: CommandType.StoredProcedure);
            }
            return orderId;
        }
        public async Task<int> Update(Order order)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

    }
}
