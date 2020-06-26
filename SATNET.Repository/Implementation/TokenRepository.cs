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
    public class TokenRepository : IRepository<Token>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public TokenRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<Token> Get(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Token>> List(Token obj)
        {
            var tokens = new List<Token>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var result = await con.QueryAsync<Token>("TokenList", commandType: CommandType.StoredProcedure);
                tokens = result.ToList();
            }
            return tokens;
        }
        public async Task<int> Add(Token obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Update(Token obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }
    }
}
