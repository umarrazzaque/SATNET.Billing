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
    public class SiteRepository : IRepository<Site>
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public SiteRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> Add(Site obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@DistributorId", obj.DistributorId, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@HardwareId", obj.HardwareId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@PackageId", obj.PackageId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@PlanTypeId", obj.PlanTypeId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@StatusId", obj.PlanTypeId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@Name", obj.Name, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@IP", obj.IP, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@Download", obj.Download, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@Upload", obj.Upload, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@InstallationDate", obj.InstallationDate, DbType.DateTime, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("SiteAdd", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(retResult);
            }
            return result;
        }

        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

        public async Task<Site> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Site>> List()
        {
            List<Site> sites = new List<Site>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var result = await con.QueryAsync<Site>("SiteList", commandType: CommandType.StoredProcedure);
                sites = result.ToList();
            }
            return sites;
        }

        public async Task<int> Update(Site obj)
        {
            throw new NotImplementedException();
        }
    }
}
