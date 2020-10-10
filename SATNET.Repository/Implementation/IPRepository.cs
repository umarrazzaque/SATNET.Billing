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
    public class IPRepository : IRepository<IP>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public IPRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<IP> Get(int id)
        {
            var iP = new IP();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                iP = await con.QueryFirstOrDefaultAsync<IP>("IPGet", parms, commandType: CommandType.StoredProcedure);
            }
            return iP;
        }
        public async Task<List<IP>> List(IP obj)
        {
            var tokens = new List<IP>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var result = await con.QueryAsync<IP>("IPList", commandType: CommandType.StoredProcedure);
                tokens = result.ToList();
            }
            return tokens;
        }
        public async Task<int> Add(IP obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@Name", obj.Name, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@Subnet", obj.Subnet, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@IPs", obj.IPs, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@Hosts", obj.Hosts, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@IPTypeId", obj.IPTypeId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("IPAddOrUpdate", queryParameters, commandType: CommandType.StoredProcedure);
                result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            }
            return result;
        }
        public async Task<int> Update(IP obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@Name", obj.Name, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@Subnet", obj.Subnet, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@IPs", obj.IPs, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@Hosts", obj.Hosts, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@IPTypeId", obj.IPTypeId, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                await con.ExecuteScalarAsync<int>("IPAddOrUpdate", queryParameters, commandType: CommandType.StoredProcedure);
                result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            }
            return result;
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
                result = await con.ExecuteScalarAsync<int>("IPDelete", queryParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

    }
}
