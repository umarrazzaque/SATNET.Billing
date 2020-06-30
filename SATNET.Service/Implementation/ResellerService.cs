using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using SATNET.Domain;
using System.Threading.Tasks;
using SATNET.Repository.Interface;
using SATNET.Repository.Implementation;

namespace SATNET.Service.Implementation
{
    public class ResellerService : IService<Customer>
    {

        private readonly IRepository<Customer> _resellerRepository;
        public ResellerService(IRepository<Customer> resellerRepository)
        {
            _resellerRepository = resellerRepository;
        }
        public Task<StatusModel> Add(Customer obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Reseller/Index" };
            try
            {
                int retId = _resellerRepository.Add(obj).Result;
                if (retId != 0)
                {
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

            }
            return Task.FromResult(status);
        }

        public Task<StatusModel> Delete(int recId, int deletedBy)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Reseller/Index" };
            try
            {
                int dRow = _resellerRepository.Delete(recId, deletedBy).Result;
                if (dRow > 0)
                {
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
            }
            return Task.FromResult(status);
        }

        public Task<Customer> Get(int id)
        {
            var retModel = new Customer();
            try
            {
                retModel = _resellerRepository.Get(id).Result;
                if (retModel.Id != 0)
                {

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
            return _resellerRepository.List(obj);
        }

        public Task<StatusModel> Update(Customer obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Reseller/Index" };
            try
            {
                int retId = _resellerRepository.Update(obj).Result;
                if (retId != 0)
                {
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

            }
            return Task.FromResult(status);
        }
    }
}
