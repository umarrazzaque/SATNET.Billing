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
                queryParameters.Add("@P_RName", obj.RName, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_RTypeId", obj.RTypeId, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_REmail", obj.REmail, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_RAddressType", obj.RAddress, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_RContactNumber", obj.RContactNumber, DbType.String, ParameterDirection.Input);
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

        public async Task<List<Reseller>> List()
        {
            List<Reseller> resellers = new List<Reseller>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var result = await con.QueryAsync<Reseller>("ResellerList", commandType: CommandType.StoredProcedure);
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
                queryParameters.Add("@P_RName", obj.RName, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_RTypeId", obj.RTypeId, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_REmail", obj.REmail, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_RAddressType", obj.RAddress, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_RContactNumber", obj.RContactNumber, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("ResellerUpdate", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            }
            return result;
        }
    }
}
