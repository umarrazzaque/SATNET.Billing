using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class HardwareComponentRegistrationService : IService<HardwareComponentRegistration>
    {
        public Task<StatusModel> Add(HardwareComponentRegistration obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponentRegistration/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    if (obj.SerialNumbers.Length >= 1) {
                        var serials = obj.SerialNumbers[0];
                        var sList = serials.Split(',');
                        foreach (var item in sList)
                        {
                            var specs = item.Split("---");

                            obj.SerialNumber = specs[0];
                            obj.UniqueIdentifier = specs[1];
                            retId = uow.HardwareComponentRegistrations.Add(obj).Result;
                        }
                    }
                    
                    //retId = uow.HardwareComponentRegistrations.Add(obj).Result;
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

        public Task<HardwareComponentRegistration> Get(int id)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponentRegistration/Index" };
            var retModel = new HardwareComponentRegistration();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retModel = uow.HardwareComponentRegistrations.Get(id).Result;
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

        public Task<List<HardwareComponentRegistration>> List(HardwareComponentRegistration obj)
        {
            List<HardwareComponentRegistration> retList = new List<HardwareComponentRegistration>();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retList = uow.HardwareComponentRegistrations.List(obj).Result;
                }
            }
            catch (Exception e)
            {

            }
            return Task.FromResult(retList);
        }

        public Task<StatusModel> Update(HardwareComponentRegistration obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponentRegistration/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.HardwareComponentRegistrations.Update(obj).Result;
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
