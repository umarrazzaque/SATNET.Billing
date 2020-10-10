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
    public class CreditNoteRepository : IRepository<CreditNote>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public CreditNoteRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<CreditNote> Get(int id)
        {
            var obj = new CreditNote();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                obj = await con.QueryFirstOrDefaultAsync<CreditNote>("CreditNoteGet", parms, commandType: CommandType.StoredProcedure);
            }
            return obj;
        }
        public async Task<List<CreditNote>> List(CreditNote obj)
        {
            var tokens = new List<CreditNote>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var parms = new DynamicParameters();
                parms.Add("@CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
                var result = await con.QueryAsync<CreditNote>("CreditNoteList", parms, commandType: CommandType.StoredProcedure);
                tokens = result.ToList();
            }
            return tokens;
        }
        public async Task<int> Add(CreditNote obj)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
                queryParameters.Add("@InvoiceId", obj.InvoiceId, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@Details", obj.Details, DbType.String, ParameterDirection.Input);
                queryParameters.Add("@Amount", obj.Amount, DbType.Decimal, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
                int retResult = await con.ExecuteScalarAsync<int>("CreditNoteAddOrUpdate", queryParameters, commandType: CommandType.StoredProcedure);
                result = Parse.ToInt32(queryParameters.Get<int>("@Id"));
            }
            return result;
        }
        public async Task<int> Update(CreditNote obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
                result = await con.ExecuteScalarAsync<int>("CreditNoteDelete", queryParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
    }
}
