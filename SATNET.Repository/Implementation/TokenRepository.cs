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
    public class TokenRepository : IRepository<Token>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public TokenRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<Token> Get(int id)
        {
            var token = new Token();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                token = await con.QueryFirstOrDefaultAsync<Token>("TokenGet", parms, commandType: CommandType.StoredProcedure);
            }
            return token;
        }
        public async Task<List<Token>> List(Token obj)
        {
            var tokens = new List<Token>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var parms = new DynamicParameters();
                parms.Add("@Name", obj.Name, DbType.String, ParameterDirection.Input);
                var result = await con.QueryAsync<Token>("TokenList",parms, commandType: CommandType.StoredProcedure);
                tokens = result.ToList();
            }
            return tokens;
        }
        public async Task<int> Add(Token obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@Name", obj.Name, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@Validity", obj.Validity, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("TokenAddOrUpdate", queryParameters, commandType: CommandType.StoredProcedure);
                result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            }
            return result;
        }
        public async Task<int> Update(Token obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
                result = await con.ExecuteScalarAsync<int>("TokenDelete", queryParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
    }
}
