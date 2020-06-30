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
    public class ServicePlanPriceRepository : RepositoryBase, IRepository<ServicePlanPrice>
    {
        public ServicePlanPriceRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<int> Add(ServicePlanPrice obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_ServicePlanId", obj.ServicePlanId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Price", obj.Price, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_Markup", obj.Markup, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_MarkupPercent", obj.MarkupPercent, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            _ = await dbCon.ExecuteScalarAsync<int>("ServicePlanPriceAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
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
            _ = await dbCon.ExecuteScalarAsync<int>("ServicePlanPriceDelete", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            return result;
        }
        public async Task<ServicePlanPrice> Get(int id)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
            ServicePlanPrice ServicePlanPrice = await dbCon.QueryFirstOrDefaultAsync<ServicePlanPrice>("ServicePlanPriceGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            return ServicePlanPrice;
        }
        public async Task<List<ServicePlanPrice>> List(ServicePlanPrice obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
            var result = await dbCon.QueryAsync<ServicePlanPrice>("ServicePlanPriceList", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            List<ServicePlanPrice> ServicePlanPrices = result.ToList();
            return ServicePlanPrices;
        }
        public async Task<int> Update(ServicePlanPrice obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_ServicePlanId", obj.ServicePlanId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Price", obj.Price, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_Markup", obj.Markup, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@P_MarkupPercent", obj.MarkupPercent, DbType.Decimal, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            _ = await dbCon.ExecuteScalarAsync<int>("ServicePlanPriceAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }
    }
}