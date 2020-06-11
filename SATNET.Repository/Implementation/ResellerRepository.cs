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

namespace SATNET.Repository.Implementation
{
    //public interface IResellerRepository: IRepository<Reseller>
    //{
    //    public Task<Reseller> GetDetail(int id);
    //}
    public class ResellerRepository : IRepository<Reseller> //IResellerRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public ResellerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> Add(Reseller obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.Output);
                queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_TypeId", obj.TypeId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@P_Code", obj.Code, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Email", obj.Email, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Address", obj.Address, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_ContactNumber", obj.ContactNumber, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("ResellerAdd", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            }
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

                int retResult = await con.ExecuteScalarAsync<int>("ResellerDelete", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            }
            return result;
        }

        public async Task<Reseller> Get(int id)
        {
            Reseller reseller = new Reseller();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
                reseller = await con.QueryFirstOrDefaultAsync<Reseller>("ResellerGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            }
            return reseller;
        }

        public async Task<List<Reseller>> List(Reseller obj)
        {
            List<Reseller> resellers = new List<Reseller>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
                var result = await con.QueryAsync<Reseller>("ResellerList", commandType: CommandType.StoredProcedure, param: queryParameters);

                resellers = result.ToList();
            }
            return resellers;
        }

        public async Task<int> Update(Reseller obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_TypeId", obj.TypeId, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Code", obj.Code, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Email", obj.Email, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Address", obj.Address, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_ContactNumber", obj.ContactNumber, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("ResellerUpdate", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            }
            return result;
        }
    }
}
