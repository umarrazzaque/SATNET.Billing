﻿using Dapper;
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
    public class SOInvoiceRepository : IRepository<SOInvoice>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public SOInvoiceRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<SOInvoice> Get(int id) 
        {
            var invoice = new SOInvoice();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                invoice = await con.QueryFirstOrDefaultAsync<SOInvoice>("InvoiceSOGet", parms, commandType: CommandType.StoredProcedure);
            }
            return invoice;
        }

        public async Task<List<SOInvoice>> List(SOInvoice obj) {
            var orders = new List<SOInvoice>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@StatusId", obj.StatusId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
                var result = await con.QueryAsync<SOInvoice>("InvoiceSOList", parms, commandType: CommandType.StoredProcedure);
                orders = result.ToList();
            }
            return orders;
        }

        public async Task<int> Add(SOInvoice obj) { throw new NotImplementedException(); }

        public async Task<int> Update(SOInvoice obj) { throw new NotImplementedException(); }
        public async Task<int> Delete(int id, int deletedBy) { throw new NotImplementedException(); }
    }
}
