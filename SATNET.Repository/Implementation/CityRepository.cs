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
    public class CityRepository : IRepository<City>
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;
        public CityRepository(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }
        public async Task<City> Get(int id)
        {
            var city = new City();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();

                var parms = new DynamicParameters();
                parms.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
                city = await con.QueryFirstOrDefaultAsync<City>("CityGet", parms, commandType: CommandType.StoredProcedure);
            }
            return city;
        }
        public async Task<List<City>> List(City obj)
        {
            var cities = new List<City>();
            using (IDbConnection con = new SqlConnection(_connectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                var result = await con.QueryAsync<City>("CityList", commandType: CommandType.StoredProcedure);
                cities = result.ToList();
            }
            return cities;
        }
        public async Task<int> Add(City obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Update(City obj)
        {
            throw new NotImplementedException();
        }
        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();        
        }

    }
}
