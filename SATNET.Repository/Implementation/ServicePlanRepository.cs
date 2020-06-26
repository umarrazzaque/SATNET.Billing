using Dapper;
<<<<<<< HEAD
using SATNET.Domain;
using SATNET.Repository.Core.Base;
using SATNET.Repository.Core.Interface;
=======
using Microsoft.Extensions.Configuration;
using SATNET.Domain;
>>>>>>> devUmerKhalid
using SATNET.Repository.Helper;
using SATNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
<<<<<<< HEAD
using System.Linq;
using System.Threading.Tasks;
namespace SATNET.Repository.Implementation
{
    public class ServicePlanRepository : RepositoryBase, IRepository<ServicePlan>
    {
        public ServicePlanRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<int> Add(ServicePlan obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_TypeId", obj.PlanTypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_DownloadMIR", obj.DownloadMIR, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_UploadMIR", obj.UploadMIR, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_DownloadCIR", obj.DownloadCIR, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_UploadCIR", obj.UploadCIR, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            _ = await dbCon.ExecuteScalarAsync<int>("ServicePlanAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Rec_Id", id, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Return_ID", -1, DbType.Int32, ParameterDirection.Output);
            _ = await dbCon.ExecuteScalarAsync<int>("ServicePlanDelete", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            return result;
        }
        public async Task<ServicePlan> Get(int id)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
            ServicePlan servicePlan = await dbCon.QueryFirstOrDefaultAsync<ServicePlan>("ServicePlaGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            return servicePlan;
        }
        public async Task<List<ServicePlan>> List(ServicePlan obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
            var result = await dbCon.QueryAsync<ServicePlan>("ServicePlanList", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            List<ServicePlan> servicePlans = result.ToList();
            return servicePlans;
        }
        public async Task<int> Update(ServicePlan obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_TypeId", obj.PlanTypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_DownloadMIR", obj.DownloadMIR, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_UploadMIR", obj.UploadMIR, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_DownloadCIR", obj.DownloadCIR, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_UploadCIR", obj.UploadCIR, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            _ = await dbCon.ExecuteScalarAsync<int>("ServicePlanAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
=======
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Implementation
{
    public class ServicePlanRepository : IRepository<ServicePlan>
    {
        private readonly IConfiguration _config;
        public ServicePlanRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<ServicePlan> Get(int id)
        {
            //ServicePlan package = new ServicePlan();
            //using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();
            //    var queryParameters = new DynamicParameters();
            //    queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
            //    package = await con.QueryFirstOrDefaultAsync<ServicePlan>("PackageGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            //}
            //return package;
            throw new NotImplementedException();
        }
        public async Task<List<ServicePlan>> List(ServicePlan obj)
        {
            var packages = new List<ServicePlan>();
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@PlanTypeId", obj.PlanTypeId, DbType.Int32, ParameterDirection.Input);
                var result = await con.QueryAsync<ServicePlan>("ServicePlanList", commandType: CommandType.StoredProcedure);
                packages = result.ToList();
            }
            return packages;
        }

        public async Task<int> Add(ServicePlan package)
        {
            throw new NotImplementedException();
            //int result = 0;
            //using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();
            //    var queryParameters = new DynamicParameters();
            //    queryParameters.Add("@Id", package.Id, DbType.Int32, ParameterDirection.Output);
            //    queryParameters.Add("@Name", package.Name, DbType.String, ParameterDirection.Input);
            //    queryParameters.Add("@Rate", package.Rate, DbType.Decimal, ParameterDirection.Input);
            //    queryParameters.Add("@Speed", package.Speed, DbType.Decimal, ParameterDirection.Input);
            //    queryParameters.Add("@Type", package.Type, DbType.String, ParameterDirection.Input);
            //    queryParameters.Add("@LoginUserId", package.CreatedBy, DbType.Int32, ParameterDirection.Input);
            //    int retResult = await con.ExecuteScalarAsync<int>("PackageAdd", commandType: CommandType.StoredProcedure, param: queryParameters);
            //    result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            //}
            //return result;
        }
        public async Task<int> Update(ServicePlan package)
        {
            throw new NotImplementedException();
            //int result = 0;
            //using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();
            //    var queryParameters = new DynamicParameters();
            //    queryParameters.Add("@P_Id", package.Id, DbType.Int32, ParameterDirection.InputOutput);
            //    queryParameters.Add("@P_Name", package.Name, DbType.String, ParameterDirection.Input);
            //    queryParameters.Add("@P_Rate", package.Rate, DbType.Decimal, ParameterDirection.Input);
            //    queryParameters.Add("@P_Speed", package.Speed, DbType.Decimal, ParameterDirection.Input);
            //    queryParameters.Add("@P_Type", package.Type, DbType.String, ParameterDirection.Input);
            //    queryParameters.Add("@LoginUserId", package.CreatedBy, DbType.Int32, ParameterDirection.Input);
            //    int retResult = await con.ExecuteScalarAsync<int>("PackageUpdate", commandType: CommandType.StoredProcedure, param: queryParameters);
            //    result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            //}
            //return result;
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
            //int result = 0;
            //using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            //{
            //    if (con.State == ConnectionState.Closed)
            //        con.Open();
            //    var queryParameters = new DynamicParameters();
            //    queryParameters.Add("@P_Rec_Id", id, DbType.Int32, ParameterDirection.Input);
            //    queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
            //    queryParameters.Add("@P_Return_ID", -1, DbType.Int32, ParameterDirection.Output);

            //    int retResult = await con.ExecuteScalarAsync<int>("PackageDelete", commandType: CommandType.StoredProcedure, param: queryParameters);
            //    result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            //}
            //return result;
>>>>>>> devUmerKhalid
        }
    }
}
