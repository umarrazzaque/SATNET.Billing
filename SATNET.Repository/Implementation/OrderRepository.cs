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
        private readonly string _connectionString;
        public OrderRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<Order> Get(int id)
        {
            var order = new Order();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                order = await con.QueryFirstOrDefaultAsync<Order>("OrderGet", parms, commandType: CommandType.StoredProcedure);
            }
            return order;
        }
        public async Task<List<Order>> List(Order obj)
        {
            var orders = new List<Order>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@StatusId", obj.StatusId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@RequestTypeId", obj.RequestTypeId, DbType.Int32, ParameterDirection.Input);
                var result = await con.QueryAsync<Order>("OrderList",parms, commandType: CommandType.StoredProcedure);
                orders = result.ToList();
            }
            return orders;
        }
        public async Task<int> Add(Order obj)
        {
            int orderId = 0;

            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var transaction = con.BeginTransaction();
                
                try
                {
                    if (obj.RequestTypeId == 1 && !string.IsNullOrEmpty(obj.SubscriberName) && !string.IsNullOrEmpty(obj.SubscriberCity))
                    {
                        int subscriberId = 0;
                        var paramsSubs = new DynamicParameters();

                        paramsSubs.Add("@Name", obj.SubscriberName, DbType.String, ParameterDirection.Input);
                        paramsSubs.Add("@City", obj.SubscriberCity, DbType.String, ParameterDirection.Input);
                        paramsSubs.Add("@Area", obj.SubscriberArea, DbType.String, ParameterDirection.Input);
                        paramsSubs.Add("@Email", obj.SubscriberEmail, DbType.String, ParameterDirection.Input);
                        paramsSubs.Add("@Notes", obj.SubscriberNotes, DbType.String, ParameterDirection.Input);
                        paramsSubs.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                        subscriberId = await con.ExecuteScalarAsync<int>("SubscriberInsert", paramsSubs, transaction, commandType: CommandType.StoredProcedure);

                        var paramsSite = new DynamicParameters();
                        paramsSite.Add("@Id", obj.SiteId, DbType.Int32, ParameterDirection.Input);
                        paramsSite.Add("@SubscriberId", subscriberId, DbType.Int32, ParameterDirection.Input);
                        var siteResult = await con.ExecuteScalarAsync<int>("SiteUpdateSubscriber", paramsSite, transaction, commandType: CommandType.StoredProcedure);
                    }

                    var paramsOrder = new DynamicParameters();
                    paramsOrder.Add("@SiteId", obj.SiteId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@HardwareId", obj.HardwareId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@ServicePlanId", obj.ServicePlanId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@UpgradeFromId", obj.UpgradeFromId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@UpgradeToId", obj.UpgradeToId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@DowngradeFromId", obj.DowngradeFromId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@DowngradeToId", obj.DowngradeToId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                    
                    if (obj.InstallationDate == DateTime.MinValue)
                    {
                        paramsOrder.Add("@InstallationDate", DBNull.Value, DbType.DateTime, ParameterDirection.Input);
                    }
                    else
                    {
                        paramsOrder.Add("@InstallationDate", obj.InstallationDate, DbType.DateTime, ParameterDirection.Input);
                    }
                    paramsOrder.Add("@RequestTypeId", obj.RequestTypeId, DbType.Int32, ParameterDirection.Input);
                    //paramsOrder.Add("@Download", obj.Download, DbType.Int32, ParameterDirection.Input);
                    //paramsOrder.Add("@Upload", obj.Upload, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@IPId", obj.IPId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@TokenId", obj.TokenId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@PromotionId", obj.PromotionId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@Other", obj.Other, DbType.String, ParameterDirection.Input);
                    orderId = await con.ExecuteScalarAsync<int>("OrderInsert", paramsOrder, transaction, commandType: CommandType.StoredProcedure);

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                }
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
