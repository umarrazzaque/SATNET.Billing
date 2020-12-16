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
        public async Task<StatusModel> Add(HardwareComponentRegistration obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponentRegistration/Index" };
            int retId = -1;
            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    uow.BeginTransaction();
                    if (obj.SerialNumbers.Length >= 1)
                    {
                        var serials = obj.SerialNumbers[0];
                        var sList = serials.Split(',');
                        foreach (var item in sList)
                        {
                            if (item != "")
                            {
                                var specs = item.Split("---");
                                if (specs.Length == 2) {
                                    obj.SerialNumber = specs[0];
                                    obj.AIRMAC = specs[1];
                                }
                                else if (specs.Length == 1) {
                                    obj.SerialNumber = specs[0];
                                }
                                
                                retId = await uow.HardwareComponentRegistrations.Add(obj);
                            }

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

        public async Task<StatusModel> Delete(int recId, int deletedBy)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/LogisticsAIRMAC/Index" };
            int dRow = -1;
            var uow = new UnitOfWorkFactory().Create();
            try
            {


                uow.BeginTransaction();
                dRow = await uow.HardwareComponentRegistrations.Delete(recId, deletedBy);
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
                uow.Connection.Close();
            }
            return status;
        }


        public async Task<HardwareComponentRegistration> Get(int id)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponentRegistration/Index" };
            var retModel = new HardwareComponentRegistration();
            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    retModel = await uow.HardwareComponentRegistrations.Get(id);
                    if (retModel.Id != 0)
                    {

                    }
                }
                catch (Exception e)
                {

                }
                finally
                {
                    uow.Connection.Close();
                }
            }
            return retModel;
        }

        public async Task<List<HardwareComponentRegistration>> List(HardwareComponentRegistration obj)
        {
            List<HardwareComponentRegistration> retList = new List<HardwareComponentRegistration>();
            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    retList = await uow.HardwareComponentRegistrations.List(obj);
                }
                catch (Exception e)
                {

                }
                finally
                {
                    uow.Connection.Close();
                }
            }
            return retList;
        }

        public async Task<StatusModel> Update(HardwareComponentRegistration obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareComponentRegistration/Index" };
            int retId = -1;
            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    uow.BeginTransaction();
                    if (obj.Flag == "RegisterAIRMAC")
                    {
                        if (obj.AIRMACs.Length > 0)
                        {
                            foreach (var item in obj.AIRMACs)
                            {
                                obj.AIRMAC = item;
                                retId = await uow.HardwareComponentRegistrations.Update(obj);
                            }
                        }
                    } else if (obj.Flag == "RegisterBUC") {
                        if (obj.AIRMACs.Length > 0)
                        {
                            foreach (var item in obj.AIRMACs)
                            {
                                obj.SerialNumber = item;
                                obj.AIRMAC = "";
                                retId = await uow.HardwareComponentRegistrations.Update(obj);
                            }
                        }
                    }
                    else
                    {
                        retId = await uow.HardwareComponentRegistrations.Update(obj);
                    }
                   
                    if (retId > 0)
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
