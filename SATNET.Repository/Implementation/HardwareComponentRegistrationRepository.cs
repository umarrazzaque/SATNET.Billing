using Dapper;
using SATNET.Domain;
using SATNET.Repository.Core.Base;
using SATNET.Repository.Core.Interface;
using SATNET.Repository.Helper;
using SATNET.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Implementation
{
    public class HardwareComponentRegistrationRepository : RepositoryBase, IRepository<HardwareComponentRegistration>
    {
        public HardwareComponentRegistrationRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<int> Add(HardwareComponentRegistration obj)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_SerialNumber", obj.SerialNumber, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_AIRMAC", obj.AIRMAC, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_HardwareComponentId", obj.HardwareComponentId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_IsUsed", obj.IsUsed, DbType.Boolean, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            int retResult = await dbCon.ExecuteScalarAsync<int>("HardCompRegAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }

        public async Task<int> Delete(int id, int deletedBy)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Rec_Id", id, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Return_ID", -1, DbType.Int32, ParameterDirection.Output);
            int retResult = await dbCon.ExecuteScalarAsync<int>("HardCompRegDelete", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            return result;
        }

        public async Task<HardwareComponentRegistration> Get(int id)
        {
            HardwareComponentRegistration retObj = new HardwareComponentRegistration();
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
            retObj = await dbCon.QueryFirstOrDefaultAsync<HardwareComponentRegistration>("HardCompRegGet", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            return retObj;
        }

        public async Task<List<HardwareComponentRegistration>> List(HardwareComponentRegistration obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_AIRMAC", obj.AIRMAC, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_HardwareComponentId", obj.HardwareComponentId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_IsUsed", obj.IsUsed, DbType.Boolean, ParameterDirection.Input);
            queryParameters.Add("@P_CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
            var result = await dbCon.QueryAsync<HardwareComponentRegistration>("HardCompRegList", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            List<HardwareComponentRegistration> retList = result.ToList();
            return retList;
        }

        public async Task<int> Update(HardwareComponentRegistration obj)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_SerialNumber", obj.SerialNumber, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Flag", obj.Flag, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_AIRMAC", obj.AIRMAC, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_HardwareComponentId", obj.HardwareComponentId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_IsUsed", obj.IsUsed, DbType.Int16, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.UpdatedBy, DbType.Int32, ParameterDirection.Input);
            int retResult = await dbCon.ExecuteScalarAsync<int>("HardCompRegAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }
    }
}
