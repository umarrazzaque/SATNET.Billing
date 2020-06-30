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
    public class UserRepository : IRepository<User>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public UserRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<User> Get(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<User>> List(User obj)
        {
            var users = new List<User>();
            IDbConnection con = new SqlConnection(_connectionString);
            using(con)
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var qResult = await con.QueryAsync<User>("UserList", commandType: CommandType.StoredProcedure);
                users = qResult.ToList();
                if (users.Any())
                {
                    List<string> roles = new List<string>();
                    var parms = new DynamicParameters();
                    foreach (var item in users)
                    {
                        parms.Add("@UserId", item.Id, DbType.Int32, ParameterDirection.Input);
                        var qResult2 = await con.QueryAsync<string>("UserRoleList", parms, commandType: CommandType.StoredProcedure);
                        item.Roles = qResult2.ToList();
                    }
                }
            }
            return users;
        }
        public async Task<int> Add(User obj) 
        {
            throw new NotImplementedException();
        }
        public async Task<int> Update(User obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }
    }
}
