﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using SATNET.Domain;
using SATNET.Repository.Helper;
using SATNET.Repository.Interface;

namespace SATNET.Repository.Implementation
{
    public class HardwareRepository : IRepository<Hardware>
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public HardwareRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> Add(Hardware obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.Output);
                //queryParameters.Add("@P_HKit", obj.HKit, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_ModemModel", obj.ModemModel, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_ModemSerialNo", obj.ModemSerialNo, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_MACAirNo", obj.MACAirNo, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_AntennaSize", obj.AntennaSize, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_AntennaSrNo", obj.AntennaSrNo, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_TransceiverWAAT", obj.TransceiverWAAT, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_TransceiverSrNo", obj.TransceiverSrNo, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Price", obj.Price, DbType.Decimal, ParameterDirection.Input);

                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("HardwareAdd", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("P_Id"));
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

                int retResult = await con.ExecuteScalarAsync<int>("HardwareDelete", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            }
            return result;
        }

        public async Task<Hardware> Get(int id)
        {
            Hardware hardware = new Hardware();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
                hardware = await con.QueryFirstOrDefaultAsync<Hardware>("HardwareGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            }
            return hardware;
        }

        public async Task<List<Hardware>> List(Hardware obj)
        {
            List<Hardware> hardwares = new List<Hardware>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
                var result = await con.QueryAsync<Hardware>("HardwareList", commandType: CommandType.StoredProcedure, param: queryParameters);
                hardwares = result.ToList();
            }
            return hardwares;
        }

        public async Task<int> Update(Hardware obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@P_ModemModel", obj.ModemModel, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_ModemSerialNo", obj.ModemSerialNo, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_MACAirNo", obj.MACAirNo, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_AntennaSize", obj.AntennaSize, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_AntennaSrNo", obj.AntennaSrNo, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_TransceiverWAAT", obj.TransceiverWAAT, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_TransceiverSrNo", obj.TransceiverSrNo, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@P_Price", obj.Price, DbType.Decimal, ParameterDirection.Input);

                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("HardwareUpdate", commandType: CommandType.StoredProcedure, param: queryParameters);
                result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            }
            return result;
        }
    }
}
