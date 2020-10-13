using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class HardwareComponentPriceService : IService<HardwareComponentPrice>
    {
        public Task<StatusModel> Add(HardwareComponentPrice obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponent/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.HardwareComponentPrices.Add(obj).Result;
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

        public Task<HardwareComponentPrice> Get(int id)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponent/Index" };
            var retModel = new HardwareComponentPrice();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retModel = uow.HardwareComponentPrices.Get(id).Result;
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

        public Task<List<HardwareComponentPrice>> List(HardwareComponentPrice obj)
        {
            List<HardwareComponentPrice> retList = new List<HardwareComponentPrice>();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retList = uow.HardwareComponentPrices.List(obj).Result;
                }
            }
            catch (Exception e)
            {

            }
            return Task.FromResult(retList);
        }

        public Task<StatusModel> Update(HardwareComponentPrice obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponent/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.HardwareComponentPrices.Update(obj).Result;
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
