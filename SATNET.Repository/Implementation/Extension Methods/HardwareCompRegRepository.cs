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

namespace SATNET.Repository.Implementation.Extension_Methods
{
    public static class HardwareCompRegRepository
    {

        public static async Task<bool> UnRegisterAirMac(this IRepository<HardwareComponentRegistration> obj, IConfiguration config,  string airmac, int actionBy)
        {
            string connectionString = config.GetConnectionString("DefaultConnection");
            using (IDbConnection con = new SqlConnection(connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                //var updateTransaction = con.BeginTransaction();

                try
                {
                    var queryParameters = new DynamicParameters();
                    queryParameters.Add("@AirMac", airmac, DbType.String, ParameterDirection.Input);
                    queryParameters.Add("@LoginUserId", actionBy, DbType.Int32, ParameterDirection.Input);
                    await con.ExecuteScalarAsync("[HardCompReg_UnRegister]", queryParameters, commandType: CommandType.StoredProcedure);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
            return true;
        }
    }
}
