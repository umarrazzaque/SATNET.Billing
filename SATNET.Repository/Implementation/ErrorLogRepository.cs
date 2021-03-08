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
    public class ErrorLogRepository : IErrorLogRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public ErrorLogRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task Add(ErrorLog obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@Details", obj.Details, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@Module", obj.Module, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@ClassName", obj.ClassName, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@ClassName", obj.MethodName, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);

                int retResult = await con.ExecuteScalarAsync<int>("ErrorLogAdd", queryParameters, commandType: CommandType.StoredProcedure);
                result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            }
        }
    }
}
