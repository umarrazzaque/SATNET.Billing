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
    public class PromotionRepository : IRepository<Promotion>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public PromotionRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<Promotion> Get(int id)
        {
            var obj = new Promotion();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                obj = await con.QueryFirstOrDefaultAsync<Promotion>("PromotionGet", parms, commandType: CommandType.StoredProcedure);
            }
            return obj;
        }
        public async Task<List<Promotion>> List(Promotion obj)
        {
            var tokens = new List<Promotion>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var result = await con.QueryAsync<Promotion>("PromotionList", commandType: CommandType.StoredProcedure);
                tokens = result.ToList();
            }
            return tokens;
        }
        public async Task<int> Add(Promotion obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@Name", obj.Name, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("PromotionAddOrUpdate", queryParameters, commandType: CommandType.StoredProcedure);
                result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            }
            return result;
        }
        public async Task<int> Update(Promotion obj)
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
                result = await con.ExecuteScalarAsync<int>("PromotionDelete", queryParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
    }
}
