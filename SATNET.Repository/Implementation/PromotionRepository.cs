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
    public class PromotionRepository : IRepository<Promotion>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public PromotionRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<Promotion> Get(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Promotion>> List(Promotion obj)
        {
            var tokens = new List<Promotion>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var result = await con.QueryAsync<Promotion>("PromotionList", commandType: CommandType.StoredProcedure);
                tokens = result.ToList();
            }
            return tokens;
        }
        public async Task<int> Add(Promotion obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Update(Promotion obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }
    }
}
