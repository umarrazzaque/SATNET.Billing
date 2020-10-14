using SATNET.Domain;
using SATNET.Domain.Enums;
using SATNET.Repository.Core;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class HardwareKitService : IService<HardwareKit>
    {
        public Task<StatusModel> Add(HardwareKit obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareKit/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.HardwareKits.Add(obj).Result;
                    if (retId != 0)
                    {
                        HardwareComponent hcObj = new HardwareComponent()
                        {
                            HardwareTypeId = Convert.ToInt32( HardwareType.Kit),
                            KitId = retId,
                            HCValue = obj.KitName
                        };
                        retId = uow.HardwareComponents.Add(hcObj).Result;
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareKit/Index" };
            try
            {
                int dRow = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    dRow = uow.HardwareKits.Delete(recId, deletedBy).Result;
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

        public Task<HardwareKit> Get(int id)
        {
            var retModel = new HardwareKit();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retModel = uow.HardwareKits.Get(id).Result;
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

        public Task<List<HardwareKit>> List(HardwareKit obj)
        {
            List<HardwareKit> retList = new List<HardwareKit>();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retList = uow.HardwareKits.List(obj).Result;
                }
            }
            catch (Exception e)
            {

            }
            return Task.FromResult(retList);
        }

        public Task<StatusModel> Update(HardwareKit obj)
        {
            var status = new StatusModel { IsSuccess = false, ErrorCode = "Error in updating the record.", ResponseUrl = "/HardwareKit/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.HardwareKits.Update(obj).Result;
                    if (retId != 0)
                    {
                        var hardCompList = uow.HardwareComponents.List(new HardwareComponent()
                        {
                            SearchBy = "HC.KITID",
                            Keyword = retId.ToString()//0321-5836090
                        }).Result;
                        if (hardCompList.Count > 0)
                        {
                            var hcObj = hardCompList[0];
                            hcObj.HCValue = obj.KitName;
                            if (hcObj != null)
                            {
                                retId = uow.HardwareComponents.Update(hcObj).Result;
                            }
                            if (retId != 0)
                            {
                                uow.SaveChanges();
                                status.IsSuccess = true;
                                status.ErrorCode = "Record update successfully.";
                            }
                        }
                        
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
