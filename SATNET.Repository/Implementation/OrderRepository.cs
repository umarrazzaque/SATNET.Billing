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
    public class OrderRepository : IRepository<Order>
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
        public async Task<List<Order>> List(Order obj)
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
        public async Task<int> Add(Order obj)
        {
            int orderId = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@SiteId", obj.SiteId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@HardwareId", obj.HardwareId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@PackageId", obj.PackageId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@UpgradeFrom", obj.UpgradeFrom, DbType.DateTime, ParameterDirection.Input);
                queryParameters.Add("@UpgradeTo", obj.UpgradeTo, DbType.DateTime, ParameterDirection.Input);
                queryParameters.Add("@DowngradeFrom", obj.DowngradeFrom, DbType.DateTime, ParameterDirection.Input);
                queryParameters.Add("@DowngradeTo", obj.DowngradeTo, DbType.DateTime, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@InstallationDate", obj.InstallationDate, DbType.DateTime, ParameterDirection.Input);
                queryParameters.Add("@PlanTypeId", obj.PlanTypeId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@IP", obj.IP, DbType.String, ParameterDirection.Input);
                orderId = await con.ExecuteScalarAsync<int>("OrderInsert", commandType: CommandType.StoredProcedure);
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
