using Dapper;
using Microsoft.Extensions.Configuration;
using SATNET.Domain;
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
    public class SystemLogRepository : IRepository<SystemLog>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public SystemLogRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<List<SystemLog>> List(SystemLog obj)
        {
            var logs = new List<SystemLog>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@EntityTypeId", obj.EntityTypeId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@Flag", obj.Flag, DbType.String, ParameterDirection.Input);

                var result = await con.QueryAsync<SystemLog>("SystemLogsList", parms, commandType: CommandType.StoredProcedure);
                logs = result.ToList();
            }
            return logs;
        }

        public async Task<SystemLog> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Add(SystemLog obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Update(SystemLog obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

    }
}
