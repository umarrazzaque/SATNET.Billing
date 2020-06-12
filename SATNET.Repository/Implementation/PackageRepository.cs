﻿using Dapper;
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
    public class PackageRepository : IRepository<Package>
    {
        private readonly IConfiguration _config;
        public PackageRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task<Package> Get(int id)
        {
            Package package = new Package();
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
                package = await con.QueryFirstOrDefaultAsync<Package>("PackageGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            }
            return package;
        }
        public async Task<List<Package>> List()
        {
            var packages = new List<Package>();
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var result = await con.QueryAsync<Package>("PackageList", commandType: CommandType.StoredProcedure);
                packages = result.ToList();
            }
            return packages;
        }
        public async Task<int> Add(Package package)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", package.Id, DbType.Int32, ParameterDirection.Output);
                queryParameters.Add("@Name", package.Name, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@Rate", package.Rate, DbType.Decimal, ParameterDirection.Input);
                queryParameters.Add("@Speed", package.Speed, DbType.Decimal, ParameterDirection.Input);
                queryParameters.Add("@Type", package.Type, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", package.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("PackageAdd", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            }
            return result;
        }
        public async Task<int> Update(Package package)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", package.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@P_Name", package.Name, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Rate", package.Rate, DbType.Decimal, ParameterDirection.Input);
                queryParameters.Add("@P_Speed", package.Speed, DbType.Decimal, ParameterDirection.Input);
                queryParameters.Add("@P_Type", package.Type, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", package.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("PackageUpdate", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            }
            return result;
        }
        public async Task<int> Delete(int id, int deletedBy)
        {

            int result = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Rec_Id", id, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@P_Return_ID", -1, DbType.Int32, ParameterDirection.Output);

                int retResult = await con.ExecuteScalarAsync<int>("PackageDelete", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            }
            return result;
        }
    }
}
