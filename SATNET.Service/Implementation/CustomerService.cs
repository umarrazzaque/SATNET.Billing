using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using SATNET.Domain;
using System.Threading.Tasks;
using SATNET.Repository.Interface;
using SATNET.Repository.Implementation;
using SATNET.Repository.Core;

namespace SATNET.Service.Implementation
{
    public class CustomerService : IService<Customer>
    {

        public CustomerService()
        {

        }
        public Task<StatusModel> Add(Customer obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Customer/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.Customers.Add(obj).Result;
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Customer/Index" };
            try
            {
                int dRow = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    dRow = uow.Customers.Delete(recId, deletedBy).Result;
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

        public Task<Customer> Get(int id)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Customer/Index" };
            var retModel = new Customer();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retModel = uow.Customers.Get(id).Result;
                    if (retModel.Id != 0)
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }
            finally
            {

            }
            return Task.FromResult(retModel);
        }


        public Task<List<Customer>> List(Customer obj)
        {
            List<Customer> retList = new List<Customer>();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retList = uow.Customers.List(obj).Result;
                }
            }
            catch (Exception e)
            {

            }
            return Task.FromResult(retList);
        }
        public Task<StatusModel> Update(Customer obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Customer/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.Customers.Update(obj).Result;
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
            finally
            {

            }
            return Task.FromResult(status);
        }
    }
}