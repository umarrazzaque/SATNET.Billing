using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class TokenService : IService<Token>
    {
        private readonly IRepository<Token> _tokenRepository;
        public TokenService(IRepository<Token> tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }
        public async Task<Token> Get(int id)
        {
            var retModel = new Token();
            try
            {
                retModel = await _tokenRepository.Get(id);
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
        public async Task<List<Token>> List(Token obj)
        {
            return await _tokenRepository.List(obj);
        }
        public async Task<StatusModel> Add(Token obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Token/Index" };
            try
            {
                int retId = -1;
                var tokens = await _tokenRepository.List(new Token() { Name = obj.Name });
                if (tokens.Count > 0)
                {
                    status.IsSuccess = false;
                    status.ErrorCode = "Token already exists.";
                    return status;
                }
                retId = await _tokenRepository.Add(obj);
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
        public async Task<StatusModel> Update(Token obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Token/Index" };
            try
            {
                int retId = -1;
                retId = await _tokenRepository.Add(obj);
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Token/Index" };
            try
            {
                int retId = -1;
                retId = await _tokenRepository.Delete(id, deletedBy);
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
