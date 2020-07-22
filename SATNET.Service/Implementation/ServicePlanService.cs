using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SATNET.Service;

namespace SATNET.Service.Implementation
{
    public class ServicePlanService : IService<ServicePlan>
    {
        private readonly IRepository<ServicePlan> _ServicePlanRepository;
        public ServicePlanService(IRepository<ServicePlan> ServicePlanRepository)
        {
            _ServicePlanRepository = ServicePlanRepository;
        }
        public Task<ServicePlan> Get(int id)
        {
            var retModel = new ServicePlan();
            try
            {
                retModel = _ServicePlanRepository.Get(id).Result;
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
        public async Task<List<ServicePlan>> List(ServicePlan obj)
        {
            return await _ServicePlanRepository.List(obj);
        }
        public Task<StatusModel> Add(ServicePlan ServicePlan)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "ServicePlan/Index" };
            try
            {
                int retId = _ServicePlanRepository.Add(ServicePlan).Result;
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
            //return _ServicePlanRepository.Add(ServicePlan);
        }
        public Task<StatusModel> Update(ServicePlan ServicePlan)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/ServicePlan/Index" };
            try
            {
                int retId = _ServicePlanRepository.Update(ServicePlan).Result;
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
        public Task<StatusModel> Delete(int recId, int deletedBy)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "ServicePlan/Index" };
            try
            {
                int dRow = _ServicePlanRepository.Delete(recId, deletedBy).Result;
                if (dRow > 0) {
                    status.IsSuccess = true;
                    status.ErrorCode = "Transaction completed successfully.";
                } else {
                    status.ErrorCode = "An error occured while processing request.";
                } 
            } catch (Exception e) {
                status.ErrorCode = "Cannot delete record due to referential records.";
            } finally { 
            }
            return Task.FromResult(status);
        }
    }
}
