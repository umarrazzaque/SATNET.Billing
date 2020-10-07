using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class PromotionService : IService<Promotion>
    {
        private readonly IRepository<Promotion> _promotionRepository;
        public PromotionService(IRepository<Promotion> promotionRepository)
        {
            _promotionRepository = promotionRepository;
        }
        public async Task<Promotion> Get(int id)
        {
            var retModel = new Promotion();
            try
            {
                retModel = await _promotionRepository.Get(id);
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
            return retModel;
        }
        public async Task<List<Promotion>> List(Promotion obj)
        {
            return await _promotionRepository.List(obj);
        }
        public async Task<StatusModel> Add(Promotion obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Promotion/Index" };
            try
            {
                int retId = -1;
                retId = await _promotionRepository.Add(obj);
                if (retId != 0)
                {
                    status.IsSuccess = true;
                    status.ErrorCode = "Record inserted successfully.";
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
            return status;
        }
        public async Task<StatusModel> Update(Promotion obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Promotion/Index" };
            try
            {
                int retId = -1;
                retId = await _promotionRepository.Add(obj);
                if (retId != 0)
                {
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
            }
            return status;
        }
        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Promotion/Index" };
            try
            {
                int retId = -1;
                retId = await _promotionRepository.Delete(id, deletedBy);
                if (retId >= 0)
                {
                    status.IsSuccess = true;
                    status.ErrorCode = "Record deleted successfully.";
                }
                else
                {
                    status.IsSuccess = false;
                    status.ErrorCode = "Error in deleting the record.";
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
            return status;
        }

    }
}
