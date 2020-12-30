using Dapper;
using Microsoft.Extensions.Configuration;
using SATNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace SATNET.Repository.Implementation
{
    public class BackgroundTaskRepository : IBackgroundTaskRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public BackgroundTaskRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public void InsertMRCInvoice(int siteId)
        {
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                try
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@SiteId", siteId, DbType.Int32, ParameterDirection.Input);
                    con.ExecuteScalar("InvoiceMRCInsert", queryParameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception e)
                {
                }
            }
        }
    }
}
