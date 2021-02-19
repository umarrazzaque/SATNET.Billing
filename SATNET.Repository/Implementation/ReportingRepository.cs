using Dapper;
using Microsoft.Extensions.Configuration;
using SATNET.Domain;
using SATNET.Domain.Reporting;
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
    public class ReportingRepository : IReportingRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public ReportingRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<ReceivablePerCategory> GetReceivablePerCategoryReport(int customerId, int siteId)
        {
            var rpc = new ReceivablePerCategory();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@CustomerId", customerId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@SiteId", siteId, DbType.Int32, ParameterDirection.Input);
                var multiResult = await con.QueryMultipleAsync("ReportReceivablePerCategory", parms, commandType: CommandType.StoredProcedure);
                rpc.ServicePlanTotal = multiResult.ReadSingleOrDefault<decimal>();
                rpc.PublicIPTotal = multiResult.ReadSingleOrDefault<decimal>();
                rpc.TokenTotal = multiResult.ReadSingleOrDefault<decimal>();

            }
            return rpc;
        }
    }
}
