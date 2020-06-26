using SATNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using SATNET.Domain;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using SATNET.Repository.Helper;
using SATNET.Repository.Core.Base;
using SATNET.Repository.Core.Interface;
using System.Transactions;

namespace SATNET.Repository.Implementation
{

    public class CustomerRepository : RepositoryBase, IRepository<Customer> //ICustomerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public CustomerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            //_configuration = configuration;
            //_connectionString = _configuration.GetConnectionString("DefaultConnection");
            _connectionString = "";
        }
        public async Task<int> Add(Customer obj)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_CustomerTypeId", obj.CustomerTypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Code", obj.Code, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Email", obj.Email, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Address", obj.Address, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_ContactNumber", obj.ContactNumber, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            int retResult = await dbCon.ExecuteScalarAsync<int>("CustomerAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);


            //var queryParams = new DynamicParameters();
            //queryParams.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            //queryParams.Add("@P_Name", null, DbType.String, ParameterDirection.Input);
            //queryParams.Add("@P_CustomerTypeId", obj.CustomerTypeId, DbType.Int32, ParameterDirection.Input);
            //queryParams.Add("@P_PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
            //queryParams.Add("@P_Code", obj.Code, DbType.String, ParameterDirection.Input);
            //queryParams.Add("@P_Email", obj.Email, DbType.String, ParameterDirection.Input);
            //queryParams.Add("@P_Address", obj.Address, DbType.String, ParameterDirection.Input);
            //queryParams.Add("@P_ContactNumber", obj.ContactNumber, DbType.String, ParameterDirection.Input);
            //queryParams.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            //retResult = await dbCon.ExecuteScalarAsync<int>("CustomerAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParams, transaction: UnitOfWork.Transaction);

            result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));

            return result;
        }

        public async Task<int> Delete(int id, int deletedBy)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Rec_Id", id, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@P_Return_ID", -1, DbType.Int32, ParameterDirection.Output);

                int retResult = await con.ExecuteScalarAsync<int>("CustomerDelete", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            }
            return result;
        }

        public async Task<Customer> Get(int id)
        {
            Customer reseller = new Customer();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
                reseller = await con.QueryFirstOrDefaultAsync<Customer>("CustomerGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            }
            return reseller;
        }

        public async Task<List<Customer>> List(Customer obj)
        {
            var dbCon = UnitOfWork.Connection;

            List<Customer> resellers = new List<Customer>();
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
            var result = await dbCon.QueryAsync<Customer>("CustomerList", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            resellers = result.ToList();
            return resellers;


        }

        public async Task<int> Update(Customer obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_CustomerTypeId", obj.CustomerTypeId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@P_PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@P_Code", obj.Code, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Email", obj.Email, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Address", obj.Address, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_ContactNumber", obj.ContactNumber, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("CustomerAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            }
            return result;
        }
    }
}
