using Dapper;
using Microsoft.Extensions.Configuration;
using SATNET.Domain;
using SATNET.Repository.Core.Base;
using SATNET.Repository.Core.Interface;
using SATNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SATNET.Repository.Helper;

namespace SATNET.Repository.Implementation
{
    public class LookupRepository : RepositoryBase, IRepository<Lookup>
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public LookupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<int> Add(Lookup obj)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Description", obj.Description, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_LookUpTypeId", obj.LookupTypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            int retResult = await dbCon.ExecuteScalarAsync<int>("LookUpAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }

        public async Task<int> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

        public async Task<Lookup> Get(int id)
        {
            Lookup lookup = new Lookup();
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
            lookup = await dbCon.QueryFirstOrDefaultAsync<Lookup>("LookupGet", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            return lookup;
        }

        public async Task<List<Lookup>> List(Lookup obj)
        {
            List<Lookup> lookups = new List<Lookup>();
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@LookupTypeId", obj.LookupTypeId, DbType.Int32, ParameterDirection.Input);
            var result = await dbCon.QueryAsync<Lookup>("LookupList", queryParameters, commandType: CommandType.StoredProcedure, transaction: UnitOfWork.Transaction);
            lookups = result.ToList();
            return lookups;
        }

        public async Task<int> Update(Lookup obj)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Description", obj.Description, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_LookUpTypeId", obj.LookupTypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            int retResult = await dbCon.ExecuteScalarAsync<int>("LookUpAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
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
