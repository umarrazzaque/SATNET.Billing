using Dapper;
using Microsoft.Extensions.Configuration;
using SATNET.Domain;
using SATNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        public async Task<bool> Add(Order order)
        {
            var users = new List<User>();
            int orderId = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                orderId = await con.ExecuteScalarAsync<int>("InsertOrder", commandType: CommandType.StoredProcedure);
            }
            return orderId > 0 ? true:false;
        }

    }
}
