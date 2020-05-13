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
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _config;
        public UserRepository(IConfiguration config)
        {
            _config = config;
        }
        public Task<User> GetUserById(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<List<User>> GetAllUsers()
        {
            var conString = _config.GetConnectionString("DefaultConnection");
            var users = new List<User>();
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var result = await con.QueryAsync<User>("GetAllDistributorUsers", commandType: CommandType.StoredProcedure);
                users = result.ToList();
            }
            return users;
        }
        public async Task<bool> AddUser(User user)
        {
            int rowsAffected = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@FirstName", user.FirstName);
                parameters.Add("@LastName", user.LastName);
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@Password", user.Password);
                parameters.Add("@DistributorId", user.DistributorId);
                rowsAffected = await con.ExecuteAsync("GetAllDistributorUsers", parameters, commandType: CommandType.StoredProcedure);
            }
            return rowsAffected > 0 ? true : false;
        }
        public Task<bool> UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
    }
}
