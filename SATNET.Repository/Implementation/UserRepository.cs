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
    }
}
