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
            int retId = -1;
            var uow = new UnitOfWorkFactory().Create();

            try
            {

                uow.BeginTransaction();
                retId = uow.HardwareKits.Add(obj).Result;
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareKit/Index" };
            int dRow = -1;
            var uow = new UnitOfWorkFactory().Create();

            try
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

        public Task<HardwareKit> Get(int id)
        {
            var retModel = new HardwareKit();
            var uow = new UnitOfWorkFactory().Create();

            try
            {
                retModel = uow.HardwareKits.Get(id).Result;
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

        public Task<List<HardwareKit>> List(HardwareKit obj)
        {
            List<HardwareKit> retList = new List<HardwareKit>();
            var uow = new UnitOfWorkFactory().Create();

            try
            {
                retList = uow.HardwareKits.List(obj).Result;

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

        public Task<StatusModel> Update(HardwareKit obj)
        {
            var status = new StatusModel { IsSuccess = false, ErrorCode = "Error in updating the record.", ResponseUrl = "/HardwareKit/Index" };
            int retId = -1;
            var uow = new UnitOfWorkFactory().Create();

            try
            {

                uow.BeginTransaction();
                retId = uow.HardwareKits.Update(obj).Result;

                if (retId != 0)
                {
                    uow.SaveChanges();
                    status.IsSuccess = true;
                    status.ErrorCode = "Record updated successfully.";
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
