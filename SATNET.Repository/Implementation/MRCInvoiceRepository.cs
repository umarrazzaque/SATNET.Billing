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
    public class MRCInvoiceRepository : IRepository<MRCInvoice>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public MRCInvoiceRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<MRCInvoice> Get(int id)
        {
            var invoice = new MRCInvoice();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                invoice = await con.QueryFirstOrDefaultAsync<MRCInvoice>("InvoiceMRCGet", parms, commandType: CommandType.StoredProcedure);
                var parms2 = new DynamicParameters();
                parms2.Add("@InvoiceId", id, DbType.Int32, ParameterDirection.Input);
                var invoiceItems = await con.QueryAsync<SOInvoiceItem>("InvoiceItemList", parms2, commandType: CommandType.StoredProcedure);
                invoice.InvoiceItems = invoiceItems.ToList();
            }
            return invoice;
        }

        public async Task<List<MRCInvoice>> List(MRCInvoice obj)
        {
            var invoices = new List<MRCInvoice>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Flag", obj.Flag, DbType.String, ParameterDirection.Input);
                parms.Add("@CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
                parms.Add("@SiteId", obj.SiteId, DbType.Int32, ParameterDirection.Input);

                var result = await con.QueryAsync<MRCInvoice>("[InvoiceMRCList]", parms, commandType: CommandType.StoredProcedure);
                invoices = result.ToList();
            }
            return invoices;
        }

        public async Task<int> Add(MRCInvoice obj) { throw new NotImplementedException(); }

        public async Task<int> Update(MRCInvoice obj) { throw new NotImplementedException(); }
        public async Task<int> Delete(int id, int deletedBy) { throw new NotImplementedException(); }

    }
}
