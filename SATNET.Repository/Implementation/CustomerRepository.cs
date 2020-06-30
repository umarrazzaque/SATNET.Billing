using SATNET.Repository.Interface;
using System.Collections.Generic;
using SATNET.Domain;
using System.Threading.Tasks;
using System.Data;
using Dapper;
using System.Linq;
using SATNET.Repository.Helper;
using SATNET.Repository.Core.Base;
using SATNET.Repository.Core.Interface;
namespace SATNET.Repository.Implementation
{
    public class CustomerRepository : RepositoryBase, IRepository<Customer> //ICustomerRepository
    {
        public CustomerRepository(IUnitOfWork unitOfWork) : base(unitOfWork) { }
        public async Task<int> Add(Customer obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_TypeId", obj.TypeId, DbType.Int32, ParameterDirection.Input);

            queryParameters.Add("@P_PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Code", obj.Code, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Email", obj.Email, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Address", obj.Address, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_ContactNumber", obj.ContactNumber, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            _ = await dbCon.ExecuteScalarAsync<int>("CustomerAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
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
            _ = await dbCon.ExecuteScalarAsync<int>("CustomerDelete", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            return result;
        }
        public async Task<Customer> Get(int id)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
            Customer customer = await dbCon.QueryFirstOrDefaultAsync<Customer>("CustomerGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            return customer;
        }
        public async Task<List<Customer>> List(Customer obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
            var result = await dbCon.QueryAsync<Customer>("CustomerList", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            List<Customer> customers = result.ToList();
            return customers;
        }
        public async Task<int> Update(Customer obj)
        {

            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_TypeId", obj.TypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Code", obj.Code, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Email", obj.Email, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Address", obj.Address, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_ContactNumber", obj.ContactNumber, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            _ = await dbCon.ExecuteScalarAsync<int>("CustomerAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }
    }
}