using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class SiteService : IService<Site>
    {

        public async Task<StatusModel> Add(Site obj)
        {
            var uow = new UnitOfWorkFactory().Create();
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Site/Index" };
            try
            {
                int retId = -1;

                uow.BeginTransaction();
                retId = await uow.Sites.Add(obj);
                if (retId != 0)
                {
                    uow.SaveChanges();
                    status.IsSuccess = true;
                    status.ErrorCode = "Record insert successfully.";
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = "Error in inserting the record.";
                }
            }
            catch (Exception e)
            {
                status.IsSuccess = false;
                status.ErrorCode = "An error occured while processing request.";
                status.ErrorDescription = e.Message;
            }
            finally
            {
                uow.CloseConnection();
            }
            return status;
        }

        public async Task<StatusModel> Delete(int recId, int deletedBy)
        {
            var uow = new UnitOfWorkFactory().Create();
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Site/Index" };
            try
            {
                int dRow = -1;

                uow.BeginTransaction();
                dRow = await uow.Sites.Delete(recId, deletedBy);
                if (dRow > 0)
                {
                    uow.SaveChanges();
                    status.IsSuccess = true;
                    status.ErrorCode = "Transaction completed successfully.";
                }
                else
                {
                    status.ErrorCode = "An error occured while processing request.";
                }
            }
            catch (Exception e)
            {
                status.ErrorCode = "Cannot delete record due to referential records.";
            }
            finally
            {
                uow.CloseConnection();
            }
            return status;
        }

        public async Task<Site> Get(int id)
        {
            var uow = new UnitOfWorkFactory().Create();
            var retModel = new Site();
            try
            {

                retModel = await uow.Sites.Get(id);
                if (retModel == null || retModel.Id != 0)
                {
                }
            }
            catch (Exception e)
            {

            }
            finally
            {
                uow.CloseConnection();
            }
            return retModel;
        }

        public async Task<List<Site>> List(Site obj)
        {
            var uow = new UnitOfWorkFactory().Create();
            List<Site> retList = new List<Site>();
            try
            {

                retList = await uow.Sites.List(obj);

            }
            catch (Exception e)
            {

            }
            finally
            {
                uow.CloseConnection();
            }
            return retList;
        }

        public async Task<StatusModel> Update(Site obj)
        {
            var uow = new UnitOfWorkFactory().Create();
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Site/Index" };
            try
            {
                int retId = -1;
                uow.BeginTransaction();
                retId = await uow.Sites.Update(obj);
                if (retId != 0)
                {
                    uow.SaveChanges();
                    status.IsSuccess = true;
                    status.ErrorCode = "Record update successfully.";
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = "Error in updating the record.";
                }
            }
            catch (Exception e)
            {
                status.IsSuccess = false;
                status.ErrorCode = "An error occured while processing request.";
                status.ErrorDescription = e.Message;
            }
            finally
            {
                uow.CloseConnection();
            }
            return status;
        }
    }
}
