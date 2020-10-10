using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace SATNET.Service.Implementation
{
    public class IPService : IService<IP>
    {
        private readonly IRepository<IP> _ipRepository;
        public IPService(IRepository<IP> ipRepository)
        {
            _ipRepository = ipRepository;
        }
        public async Task<IP> Get(int id)
        {
            var retModel = new IP();
            try
            {
                retModel = await _ipRepository.Get(id);
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
        public async Task<List<IP>> List(IP obj)
        {
            return await _ipRepository.List(obj);
        }
        public async Task<StatusModel> Add(IP obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/IP/Index" };
            try
            {
                int retId = -1;
                retId = await _ipRepository.Add(obj);
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
        public async Task<StatusModel> Update(IP obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/IP/Index" };
            try
            {
                int retId = -1;
                retId = await _ipRepository.Add(obj);
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/IP/Index" };
            try
            {
                int retId = -1;
                retId = await _ipRepository.Delete(id, deletedBy);
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
