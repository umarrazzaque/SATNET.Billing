using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class IPPriceService : IService<IPPrice>
    {
        private readonly IRepository<IPPrice> _ippRepository;
        public IPPriceService(IRepository<IPPrice> ippRepository)
        {
            _ippRepository = ippRepository;
        }
        public async Task<IPPrice> Get(int id)
        {
            var retModel = new IPPrice();
            try
            {
                retModel = await _ippRepository.Get(id);
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
        public async Task<List<IPPrice>> List(IPPrice obj)
        {
            return await _ippRepository.List(obj);
        }
        public async Task<StatusModel> Add(IPPrice obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/IPPrice/Index" };
            try
            {
                int retId = -1;
                var priceLists = await _ippRepository.List(new IPPrice() { IPId = obj.IPId, PriceTierId = obj.PriceTierId });
                if (priceLists.Count > 0)
                {
                    status.IsSuccess = false;
                    status.ErrorCode = "Price for this IP and price tier already exists.";
                    return status;
                }
                retId = await _ippRepository.Add(obj);
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
        public async Task<StatusModel> Update(IPPrice obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/IPPrice/Index" };
            try
            {
                int retId = -1;
                retId = await _ippRepository.Add(obj);
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/IPPrice/Index" };
            try
            {
                int retId = -1;
                retId = await _ippRepository.Delete(id, deletedBy);
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
