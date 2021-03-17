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
    public class CodeErrorLogRepository : ICodeErrorLogRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public CodeErrorLogRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task Add(CodeErrorLog obj)
        {
            int result = 0;
            try
            {
                using (IDbConnection con = new SqlConnection(_connectionString))
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@Details", obj.Details, DbType.String, ParameterDirection.Input);
                    queryParameters.Add("@Module", obj.Module, DbType.String, ParameterDirection.Input);
                    queryParameters.Add("@ClassName", obj.ClassName, DbType.String, ParameterDirection.Input);
                    queryParameters.Add("@MethodName", obj.MethodName, DbType.String, ParameterDirection.Input);
                    queryParameters.Add("@LoginByUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);

                    await con.ExecuteScalarAsync("CodeErrorLogAdd", queryParameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {

            }
        }
    }
}
