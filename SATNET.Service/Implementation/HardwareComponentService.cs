using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class HardwareComponentService : IService<HardwareComponent>
    {
        public Task<StatusModel> Add(HardwareComponent obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponent/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.HardwareComponents.Add(obj).Result;
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
            throw new NotImplementedException();
        }

        public Task<HardwareComponent> Get(int id)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponent/Index" };
            var retModel = new HardwareComponent();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retModel = uow.HardwareComponents.Get(id).Result;
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

        public Task<List<HardwareComponent>> List(HardwareComponent obj)
        {
            List<HardwareComponent> retList = new List<HardwareComponent>();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retList = uow.HardwareComponents.List(obj).Result;
                }
            }
            catch (Exception e)
            {

            }
            return Task.FromResult(retList);
        }

        public Task<StatusModel> Update(HardwareComponent obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponent/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.HardwareComponents.Update(obj).Result;
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
