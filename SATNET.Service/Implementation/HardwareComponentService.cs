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
        public async Task<StatusModel> Add(HardwareComponent obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponent/Index" };
            int retId = -1;
            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    uow.BeginTransaction();
                    retId = await uow.HardwareComponents.Add(obj);
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
                    uow.Connection.Close();
                }
            }

            return status;
        }

        public Task<StatusModel> Delete(int recId, int deletedBy)
        {
            throw new NotImplementedException();
        }

        public async Task<HardwareComponent> Get(int id)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponent/Index" };
            var retModel = new HardwareComponent();
            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    retModel = await uow.HardwareComponents.Get(id);
                    if (retModel.Id != 0)
                    {

                    }
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    uow.Connection.Close();
                }
            }
            return retModel;
        }

        public async Task<List<HardwareComponent>> List(HardwareComponent obj)
        {
            List<HardwareComponent> retList = new List<HardwareComponent>();

            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    retList = await uow.HardwareComponents.List(obj);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    uow.Connection.Close();
                }
            }

            return retList;
        }

        public async Task<StatusModel> Update(HardwareComponent obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponent/Index" };

            int retId = -1;
            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    uow.BeginTransaction();
                    retId = await uow.HardwareComponents.Update(obj);
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
                    uow.Connection.Close();
                }

            }


            return status;
        }
    }
}
