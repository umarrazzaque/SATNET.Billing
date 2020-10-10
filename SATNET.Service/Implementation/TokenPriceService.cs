using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class TokenPriceService : IService<TokenPrice>
    {
        private readonly IRepository<TokenPrice> _tokenPriceRepository;
        public TokenPriceService(IRepository<TokenPrice> tokenPriceRepository)
        {
            _tokenPriceRepository = tokenPriceRepository;
        }
        public async Task<TokenPrice> Get(int id)
        {
            var retModel = new TokenPrice();
            try
            {
                retModel = await _tokenPriceRepository.Get(id);
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
        public async Task<List<TokenPrice>> List(TokenPrice obj)
        {
            return await _tokenPriceRepository.List(obj);
        }
        public async Task<StatusModel> Add(TokenPrice obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/TokenPrice/Index" };
            try
            {
                int retId = -1;
                var tokens = await _tokenPriceRepository.List(new TokenPrice() { TokenId = obj.TokenId, PriceTierId=obj.PriceTierId });
                if (tokens.Count > 0)
                {
                    status.IsSuccess = false;
                    status.ErrorCode = "Token Price already exists.";
                    return status;
                }
                retId = await _tokenPriceRepository.Add(obj);
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
        public async Task<StatusModel> Update(TokenPrice obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/TokenPrice/Index" };
            try
            {
                int retId = -1;
                retId = await _tokenPriceRepository.Add(obj);
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/TokenPrice/Index" };
            try
            {
                int retId = -1;
                retId = await _tokenPriceRepository.Delete(id, deletedBy);
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
