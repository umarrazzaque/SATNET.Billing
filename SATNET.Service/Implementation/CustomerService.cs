using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using SATNET.Domain;
using System.Threading.Tasks;
using SATNET.Repository.Interface;
using SATNET.Repository.Implementation;
using SATNET.Repository.Core;
using System.Data.SqlClient;

namespace SATNET.Service.Implementation
{
    public class CustomerService : IService<Customer>
    {
        private readonly ExceptionService _exceptionService;
        public CustomerService()
        {
            _exceptionService = new ExceptionService();
        }
        public async Task<StatusModel> Add(Customer obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Customer/Index" };
            int retId = -1;
            var uow = new UnitOfWorkFactory().Create();
            try
            {

                    uow.BeginTransaction();
                    retId = await uow.Customers.Add(obj);
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
                status.ErrorCode = _exceptionService.HandleException(e).ErrorCode;
                status.ErrorDescription = e.Message;
            }
            finally
            {
                 uow.CloseConnection();
            }
            return status;
        }

        public async Task<StatusModel> Delete(int recId, int deletedBy)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Customer/Index" };
            int dRow = -1;
            var uow = new UnitOfWorkFactory().Create();
            try
            {


                    uow.BeginTransaction();
                    dRow = await uow.Customers.Delete(recId, deletedBy);
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
            return status;
        }

        public async Task<Customer> Get(int id)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Customer/Index" };
            var retModel = new Customer();
            var uow = new UnitOfWorkFactory().Create();
            try
            {
                    retModel = await uow.Customers.Get(id);
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
            return retModel;
        }


        public async Task<List<Customer>> List(Customer obj)
        {
            List<Customer> retList = new List<Customer>();
            var uow = new UnitOfWorkFactory().Create();
            try
            {
               
             
                    retList = await uow.Customers.List(obj);
             
            }
            catch (Exception e)
            {

            }
            finally
            {
                 uow.CloseConnection();
            }
            return retList;
        }
        public async Task<StatusModel> Update(Customer obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Customer/Index" };
            int retId = -1;
            var uow = new UnitOfWorkFactory().Create();
            try
            {

               
                    uow.BeginTransaction();
                    retId = await uow.Customers.Update(obj);
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
            return status;
        }
    }
}