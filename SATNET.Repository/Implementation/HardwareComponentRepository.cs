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
    public class HardwareComponentRepository : RepositoryBase, IRepository<HardwareComponent>
    {
        public HardwareComponentRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public async Task<int> Add(HardwareComponent obj)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_HardwareTypeId", obj.HardwareTypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_HCValue", obj.HCValue, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_HCSpareTypeId", obj.HCSpareTypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_KIT_ID", obj.KitId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            int retResult = await dbCon.ExecuteScalarAsync<int>("HardwareComponentAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
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
            int retResult = await dbCon.ExecuteScalarAsync<int>("HardwareComponentDelete", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            return result;
        }

        public async Task<HardwareComponent> Get(int id)
        {
            HardwareComponent retObj = new HardwareComponent();
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
            retObj = await dbCon.QueryFirstOrDefaultAsync<HardwareComponent>("HardwareComponentGet", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            return retObj;
        }

        public async Task<List<HardwareComponent>> List(HardwareComponent obj)
        {
            List<HardwareComponent> retList = new List<HardwareComponent>();
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
            var result = await dbCon.QueryAsync<HardwareComponent>("HardwareComponentList", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            retList = result.ToList();
            return retList;
        }

        public async Task<int> Update(HardwareComponent obj)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_HardwareTypeId", obj.HardwareTypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_HCValue", obj.HCValue, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_HCSpareTypeId", obj.HCSpareTypeId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_KIT_ID", obj.KitId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            int retResult = await dbCon.ExecuteScalarAsync<int>("HardwareComponentAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }
    }
}
