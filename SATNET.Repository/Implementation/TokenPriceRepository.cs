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
    public class TokenPriceRepository : IRepository<TokenPrice>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public TokenPriceRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<TokenPrice> Get(int id)
        {
            var token = new TokenPrice();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                token = await con.QueryFirstOrDefaultAsync<TokenPrice>("TokenPriceGet", parms, commandType: CommandType.StoredProcedure);
            }
            return token;
        }
        public async Task<List<TokenPrice>> List(TokenPrice obj)
        {
            var tokens = new List<TokenPrice>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var parms = new DynamicParameters();
                parms.Add("@TokenId", obj.TokenId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
                var result = await con.QueryAsync<TokenPrice>("TokenPriceList", parms, commandType: CommandType.StoredProcedure);
                tokens = result.ToList();
            }
            return tokens;
        }
        public async Task<int> Add(TokenPrice obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@TokenId", obj.TokenId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@Price", obj.Price, DbType.Decimal, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("TokenPriceAddOrUpdate", queryParameters, commandType: CommandType.StoredProcedure);
                result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            }
            return result;
        }
        public async Task<int> Update(TokenPrice obj)
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
                result = await con.ExecuteScalarAsync<int>("TokenPriceDelete", queryParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
    }
}
