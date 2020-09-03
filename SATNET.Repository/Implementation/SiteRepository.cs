using Dapper;
using Microsoft.Extensions.Configuration;
using SATNET.Domain;
using SATNET.Repository.Core.Base;
using SATNET.Repository.Core.Interface;
using SATNET.Repository.Helper;
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
    public class SiteRepository : RepositoryBase, IRepository<Site>
    {
        public SiteRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            
        }
        public async Task<int> Add(Site obj)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_SubscriberId", obj.SubscriberId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_StatusId", obj.StatusId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_City", obj.City, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Area", obj.Area, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Subscriber", obj.Subscriber, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            int retResult = await dbCon.ExecuteScalarAsync<int>("SiteAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
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
            int retResult = await dbCon.ExecuteScalarAsync<int>("SiteDelete", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            result = Parse.ToInt32(queryParameters.Get<int>("@P_Return_ID"));
            return result;
        }

        public async Task<Site> Get(int id)
        {
            Site site = new Site();
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", id, DbType.Int32, ParameterDirection.Input);
            site = await dbCon.QueryFirstOrDefaultAsync<Site>("SiteGet", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            return site;
        }

        public async Task<List<Site>> List(Site obj)
        {
            List<Site> sites = new List<Site>();
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_SEARCHBY", obj.SearchBy, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_KEYWORD", obj.Keyword, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_FLAG", obj.Flag, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_SORTORDER", obj.SortOrder, DbType.String, ParameterDirection.Input);
            if (obj.StatusIds != null && obj.StatusIds.Count > 0)
            {
                foreach (var statusId in obj.StatusIds)
                {
                    queryParameters.Add("@StatusId", statusId, DbType.Int32, ParameterDirection.Input);
                    var result = await dbCon.QueryAsync<Site>("SiteList", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
                    sites = sites.Union(result).ToList();
                }
            }
            else
            {
                var result = await dbCon.QueryAsync<Site>("SiteList", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
                sites = result.ToList();
            }
            
            return sites;
        }

        public async Task<int> Update(Site obj)
        {
            int result = 0;
            var dbCon = UnitOfWork.Connection;
            var queryParameters = new DynamicParameters();
            queryParameters.Add("@P_Id", obj.Id, DbType.Int32, ParameterDirection.InputOutput);
            queryParameters.Add("@P_CustomerId", obj.CustomerId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_SubscriberId", obj.SubscriberId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_StatusId", obj.StatusId, DbType.Int32, ParameterDirection.Input);
            queryParameters.Add("@P_Name", obj.Name, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_City", obj.City, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Area", obj.Area, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@P_Subscriber", obj.Subscriber, DbType.String, ParameterDirection.Input);
            queryParameters.Add("@LoginUserId", obj.CreatedBy, DbType.Int32, ParameterDirection.Input);
            int retResult = await dbCon.ExecuteScalarAsync<int>("SiteAddOrUpdate", commandType: CommandType.StoredProcedure, param: queryParameters, transaction: UnitOfWork.Transaction);
            result = Parse.ToInt32(queryParameters.Get<int>("@P_Id"));
            return result;
        }
    }
}
