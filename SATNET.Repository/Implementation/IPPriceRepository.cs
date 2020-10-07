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
    public class IPPriceRepository : IRepository<IPPrice>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public IPPriceRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<IPPrice> Get(int id)
        {
            var iP = new IPPrice();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                iP = await con.QueryFirstOrDefaultAsync<IPPrice>("IPPriceGet", parms, commandType: CommandType.StoredProcedure);
            }
            return iP;
        }
        public async Task<List<IPPrice>> List(IPPrice obj)
        {
            var prices = new List<IPPrice>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var parms = new DynamicParameters();
                parms.Add("@@IPId", obj.IPId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@@PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
                var result = await con.QueryAsync<IPPrice>("IPPriceList", parms,commandType: CommandType.StoredProcedure);
                prices = result.ToList();
            }
            return prices;
        }
        public async Task<int> Add(IPPrice obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@IPId", obj.IPId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@Price", obj.Price, DbType.Decimal, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("IPPriceAddOrUpdate", queryParameters, commandType: CommandType.StoredProcedure);
                result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            }
            return result;
        }
        public async Task<int> Update(IPPrice obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@IPId", obj.IPId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@PriceTierId", obj.PriceTierId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@Price", obj.Price, DbType.Decimal, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("IPPriceAddOrUpdate", queryParameters, commandType: CommandType.StoredProcedure);
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
                result = await con.ExecuteScalarAsync<int>("IPPriceDelete", queryParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
    }
}
