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
    public class LookupRepository : IRepository<Lookup>
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public LookupRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> Add(Lookup obj)
        {
            throw new NotImplementedException();        
        }

        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

        public async Task<Lookup> Get(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Lookup>> List(Lookup obj)
        {
            List<Lookup> lookups = new List<Lookup>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@LookupTypeId", obj.LookupTypeId, DbType.Int32, ParameterDirection.Input);
                var result = await con.QueryAsync<Lookup>("LookupList", queryParameters, commandType: CommandType.StoredProcedure);
                lookups = result.ToList();
            }
            return lookups;
        }

        public async Task<int> Update(Lookup obj)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Lookup>> ListByFilter(int lookupTypeId)
        {
            List<Lookup> lookups = new List<Lookup>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var queryParameters = new DynamicParameters();
                queryParameters.Add("@LookupTypeId", lookupTypeId, DbType.Int32, ParameterDirection.Input);
                var result = await con.QueryAsync<Lookup>("LookupListByFilter", queryParameters, commandType: CommandType.StoredProcedure);
                lookups = result.ToList();
            }
            return lookups;
        }

    }
}
