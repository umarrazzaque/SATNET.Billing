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
    public class HardwareKitRepository : RepositoryBase, IRepository<HardwareKit>
    {
        public HardwareKitRepository(IUnitOfWork unitOfWork): base(unitOfWork)
        {

        }

        public async Task<int> Add(HardwareKit obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_KitName", obj.KitName, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_ModemModelId", obj.ModemModelId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_AntennaMeterId", obj.AntennaMeterId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_PowerWATT", obj.PowerWATT, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_NPRMPieces", obj.NPRMPieces, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_RG6", obj.RG6, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_ConnectorPieces", obj.ConnectorPieces, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            _ = await dbCon.ExecuteScalarAsync<int>("HardwareKitAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }

        public async Task<int> Delete(int id, int deletedBy)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Rec_Id", id, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", deletedBy, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Return_ID", -1, DbType.Int32, ParameterDirection.Output);
            _ = await dbCon.ExecuteScalarAsync<int>("HardwareKitDelete", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            return result;
        }

        public async Task<HardwareKit> Get(int id)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
            HardwareKit HardwareKit = await dbCon.QueryFirstOrDefaultAsync<HardwareKit>("HardwareKitGet", commandType: CommandType.StoredProcedure, param: queryParameters);
            return HardwareKit;
        }

        public async Task<List<HardwareKit>> List(HardwareKit obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
            var result = await dbCon.QueryAsync<HardwareKit>("HardwareKitList", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            List<HardwareKit> HardwareKits = result.ToList();
            return HardwareKits;
        }

        public async Task<int> Update(HardwareKit obj)
        {
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_KitName", obj.KitName, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_ModemModelId", obj.ModemModelId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_AntennaMeterId", obj.AntennaMeterId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_PowerWATT", obj.PowerWATT, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_NPRMPieces", obj.NPRMPieces, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_RG6", obj.RG6, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_ConnectorPieces", obj.ConnectorPieces, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            _ = await dbCon.ExecuteScalarAsync<int>("HardwareKitAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            int result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }
    }
}
