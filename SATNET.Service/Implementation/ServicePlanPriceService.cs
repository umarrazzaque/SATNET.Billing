using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class ServicePlanPriceService : IService<ServicePlanPrice>
    {
        public Task<StatusModel> Add(ServicePlanPrice obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlanPrice/Index" };
            int retId = -1;
            var uow = new UnitOfWorkFactory().Create();
            try
            {
                uow.BeginTransaction();
                retId = uow.ServicePlanPrices.Add(obj).Result;
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
            return Task.FromResult(status);
        }

        public Task<StatusModel> Delete(int recId, int deletedBy)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlanPrice/Index" };
            int dRow = -1;
            var uow = new UnitOfWorkFactory().Create();
            try
            {
                uow.BeginTransaction();
                dRow = uow.ServicePlanPrices.Delete(recId, deletedBy).Result;
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
            return Task.FromResult(status);
        }

        public Task<ServicePlanPrice> Get(int id)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlanPrice/Index" };
            var retModel = new ServicePlanPrice();
            var uow = new UnitOfWorkFactory().Create();
            try
            {
                retModel = uow.ServicePlanPrices.Get(id).Result;
                if (retModel.Id != 0)
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
            return Task.FromResult(retModel);
        }

        public Task<List<ServicePlanPrice>> List(ServicePlanPrice obj)
        {
            List<ServicePlanPrice> retList = new List<ServicePlanPrice>();
            var uow = new UnitOfWorkFactory().Create();
            try
            {
                retList = uow.ServicePlanPrices.List(obj).Result;

            }
            catch (Exception e)
            {

            }
            finally
            {
                 uow.CloseConnection();
            }
            return Task.FromResult(retList);
        }

        public Task<StatusModel> Update(ServicePlanPrice obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlanPrice/Index" };
            int retId = -1;
            var uow = new UnitOfWorkFactory().Create();
            try
            {
                uow.BeginTransaction();
                retId = uow.ServicePlanPrices.Update(obj).Result;
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
            return Task.FromResult(status);
        }
    }
}
