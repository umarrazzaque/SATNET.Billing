using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation.Extensions
{
    public static class HardCompRegExtensionService
    {
        public static async Task<StatusModel> AirMacImport(this IService<HardwareComponentRegistration> obj, List<HardwareComponentRegistration> recordsList)
        {
            var status = new StatusModel { IsSuccess = false };
            int dRow = -1;
            bool isSuccess = false;
            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    uow.BeginTransaction();
                    foreach (var item in recordsList)
                    {
                        dRow = await uow.HardwareComponentRegistrations.Add(item);
                        if (dRow > 0)
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            isSuccess = false;
                            break;
                        }
                    }
                    if (isSuccess)
                    {
                        uow.SaveChanges();
                        status.IsSuccess = true;
                        status.ErrorCode = "Transaction completed successfully.";
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.ErrorCode = "An error occured while processing request.";
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
            }
            //var uow = new UnitOfWorkFactory().Create();
            //try
            //{
            //    uow.BeginTransaction();
            //    foreach (var item in recordsList)
            //    {
            //        dRow = await uow.HardwareComponentRegistrations.Add(item);
            //        if (dRow > 0)
            //        {
            //            isSuccess = true;
            //        }
            //        else
            //        {
            //            isSuccess = false;
            //            break;
            //        }
            //    }
            //    if (isSuccess)
            //    {
            //        uow.SaveChanges();
            //        status.IsSuccess = true;
            //        status.ErrorCode = "Transaction completed successfully.";
            //    }
            //    else
            //    {
            //        status.IsSuccess = false;
            //        status.ErrorCode = "An error occured while processing request.";
            //    }
            //}
            //catch (Exception e)
            //{
            //    status.IsSuccess = false;
            //    status.ErrorCode = "An error occured while processing request.";
            //    status.ErrorDescription = e.Message;
            //}
            //finally
            //{
            //     uow.CloseConnection();
            //}
            return status;
            //foreach (var item in recordsList)
            //{

            //}
            //return true;
        }

        public static async Task<StatusModel> AirMacRegistrationImport(this IService<HardwareComponentRegistration> obj, List<HardwareComponentRegistration> recordsList)
        {
            var status = new StatusModel { IsSuccess = false };
            int dRow = -1;
            bool isSuccess = false;
            using (var uow = new UnitOfWorkFactory().Create())
            {
                try
                {
                    uow.BeginTransaction();
                    foreach (var item in recordsList)
                    {
                        item.Flag = "RegisterAIRMAC";
                        dRow = await uow.HardwareComponentRegistrations.Update(item);
                        if (dRow > 0)
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            isSuccess = false;
                            break;
                        }
                    }
                    if (isSuccess)
                    {
                        uow.SaveChanges();
                        status.IsSuccess = true;
                        status.ErrorCode = "Transaction completed successfully.";
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.ErrorCode = "An error occured while processing request.";
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
            }
            return status;
        }
    }
}
