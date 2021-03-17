using Dapper;
using Microsoft.Extensions.Configuration;
using SATNET.Domain;
using SATNET.Repository.Helper;
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
                parms.Add("@ScheduleDateId", obj.ScheduleDateId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@RequestTypeId", obj.RequestTypeId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@SiteId", obj.SiteId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@Flag", obj.Flag, DbType.String, ParameterDirection.Input);
                if (obj.CreatedOn != DateTime.MinValue)
                {
                    parms.Add("@CreatedOn", obj.CreatedOn, DbType.DateTime, ParameterDirection.Input);
                }
                parms.Add("@CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
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
                    var paramsOrder = new DynamicParameters();
                    paramsOrder.Add("@SiteId", obj.SiteId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@ServicePlanId", obj.ServicePlanId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@DedicatedServicePlanName", obj.DedicatedServicePlanName, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@UpgradeFromId", obj.UpgradeFromId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@UpgradeToId", obj.UpgradeToId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@DowngradeFromId", obj.DowngradeFromId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@DowngradeToId", obj.DowngradeToId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                    
                    if (obj.InstallationDate == DateTime.MinValue)
                    {
                        paramsOrder.Add("@PlannedInstallationDate", DBNull.Value, DbType.DateTime, ParameterDirection.Input);
                    }
                    else
                    {
                        paramsOrder.Add("@PlannedInstallationDate", obj.InstallationDate, DbType.DateTime, ParameterDirection.Input);
                    }
                    paramsOrder.Add("@RequestTypeId", obj.RequestTypeId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@ScheduleDateId", obj.ScheduleDateId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@IPId", obj.IPId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@ChangeIPId", obj.ChangeIPId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@TokenId", obj.TokenId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@AirMac", obj.AirMac, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@HardwareConditionId", obj.HardwareConditionId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@PromotionId", obj.PromotionId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@SiteCityId", obj.SiteCityId, DbType.Int32, ParameterDirection.Input);
                    //paramsOrder.Add("@SiteCity", obj.SiteCity, DbType.String, ParameterDirection.Input);
                    //paramsOrder.Add("@SiteArea", obj.SiteArea, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@DedicatedServicePlanName", obj.DedicatedServicePlanName, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@SubscriberName", obj.SubscriberName, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@SubscriberCity", obj.SubscriberCity, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@SubscriberEmail", obj.SubscriberEmail, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@SubscriberArea", obj.SubscriberArea, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@SubscriberNotes", obj.SubscriberNotes, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@Other", obj.Other, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@ChangeServicePlanId", obj.ChangeServicePlanId, DbType.Int32, ParameterDirection.Input);
                    paramsOrder.Add("@NewAirMac", obj.NewAirMac, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@ProRataQuota", obj.ProRataQuota, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@UpgradeToProRataQuota", obj.UpgradeToProRataQuota, DbType.String, ParameterDirection.Input);
                    paramsOrder.Add("@IsServicePlanFull", obj.IsServicePlanFull, DbType.Boolean, ParameterDirection.Input);
                    paramsOrder.Add("@IsServicePlanFullUpgrade", obj.IsServicePlanFullUpgrade, DbType.Boolean, ParameterDirection.Input);
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
        public async Task<int> Update(Order obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                //var updateTransaction = con.BeginTransaction();

                try
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.Input);
                    queryParameters.Add("@StatusId", obj.StatusId, DbType.Int32, ParameterDirection.Input);
                    queryParameters.Add("@RejectReason", obj.RejectReason, DbType.String, ParameterDirection.Input);
                    queryParameters.Add("@ActivationSiteName", obj.SiteName, DbType.String, ParameterDirection.Input);
                    queryParameters.Add("@LoginUserId", obj.UpdatedBy, DbType.Int32, ParameterDirection.Input);
                    result = await con.ExecuteScalarAsync<int>("OrderUpdate", queryParameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return result;
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
                var result = await con.ExecuteScalarAsync<int>("OrderDelete", queryParameters, commandType: CommandType.StoredProcedure);
                return result;
            }
        }

    }
}
