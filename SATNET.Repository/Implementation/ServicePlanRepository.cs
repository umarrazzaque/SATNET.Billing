using Dapper;
using SATNET.Domain;
using SATNET.Repository.Core.Base;
using SATNET.Repository.Core.Interface;
using SATNET.Repository.Helper;
using SATNET.Repository.Interface;
using System.Collections.Generic;
using System.Data;
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
            ServicePlan servicePlan = await dbCon.QueryFirstOrDefaultAsync<ServicePlan>("ServicePlanGet", commandType: CommandType.StoredProcedure, param: queryParameters);
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
        }
    }
}