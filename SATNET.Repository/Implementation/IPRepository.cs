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
    public class IPRepository : IRepository<IP>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public IPRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<IP> Get(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<IP>> List(IP obj)
        {
            var tokens = new List<IP>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var result = await con.QueryAsync<IP>("IPList", commandType: CommandType.StoredProcedure);
                tokens = result.ToList();
            }
            return tokens;
        }
        public async Task<int> Add(IP obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Update(IP obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

    }
}
