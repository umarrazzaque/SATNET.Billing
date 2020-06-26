using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class ServicePlanService : IService<ServicePlan>
    {
        public Task<StatusModel> Add(ServicePlan obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlan/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.ServicePlans.Add(obj).Result;
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
            }
            catch (Exception e)
            {
                status.IsSuccess = false;
                status.ErrorCode = "An error occured while processing request.";
                status.ErrorDescription = e.Message;
            }
            return Task.FromResult(status);
        }

        public Task<StatusModel> Delete(int recId, int deletedBy)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlan/Index" };
            try
            {
                int dRow = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    dRow = uow.ServicePlans.Delete(recId, deletedBy).Result;
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
            }
            catch (Exception e)
            {
                status.ErrorCode = "Cannot delete record due to referential records.";
            }
            return Task.FromResult(status);
        }

        public Task<ServicePlan> Get(int id)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlan/Index" };
            var retModel = new ServicePlan();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retModel = uow.ServicePlans.Get(id).Result;
                    if (retModel.Id != 0)
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }
            return Task.FromResult(retModel);
        }

        public Task<List<ServicePlan>> List(ServicePlan obj)
        {
            List<ServicePlan> retList = new List<ServicePlan>();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retList = uow.ServicePlans.List(obj).Result;
                }
            }
            catch (Exception e)
            {

            }
            return Task.FromResult(retList);
        }

        public Task<StatusModel> Update(ServicePlan obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlan/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.ServicePlans.Update(obj).Result;
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
            }
            catch (Exception e)
            {
                status.IsSuccess = false;
                status.ErrorCode = "An error occured while processing request.";
                status.ErrorDescription = e.Message;
            }
            return Task.FromResult(status);
        }
    }
}
