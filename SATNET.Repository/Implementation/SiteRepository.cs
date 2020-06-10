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
                queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.Output);
                queryParameters.Add("@P_SiteName", obj.SiteName, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_SiteStatusId", obj.SiteStatusId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@P_City", obj.City, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Area", obj.Area, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("SiteAdd", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            }
            return result;
        }

        public async Task<int> Delete(int id, int deletedBy)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Rec_Id", id, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@P_Return_ID", -1, DbType.Int32, ParameterDirection.Output);

                int retResult = await con.ExecuteScalarAsync<int>("SiteDelete", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            }
            return result;
        }

        public async Task<Site> Get(int id)
        {
            Site site = new Site();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
                site = await con.QueryFirstOrDefaultAsync<Site>("SiteGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            }
            return site;
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
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@P_SiteName", obj.SiteName, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_SiteStatusId", obj.SiteStatusId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@P_City", obj.City, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Area", obj.Area, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("SiteUpdate", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            }
            return result;
        }
    }
}
