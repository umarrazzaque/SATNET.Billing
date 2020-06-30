using SATNET.Domain;
using SATNET.Repository.Implementation;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class HardwareService : IService<Hardware>
    {
        private readonly IRepository<Hardware> _hardwareRepository;
        public HardwareService(IRepository<Hardware> hardwareRepository)
        {
            _hardwareRepository = hardwareRepository;
        }
        public Task<StatusModel> Add(Hardware obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Hardware/Index" };
            try
            {
                int retId = _hardwareRepository.Add(obj).Result;
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Hardware/Index" };
            try
            {
                int dRow = _hardwareRepository.Delete(recId, deletedBy).Result;
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

        public Task<Hardware> Get(int id)
        {
            var retModel = new Hardware();
            try
            {
                retModel = _hardwareRepository.Get(id).Result;
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

        public Task<List<Hardware>> List(Hardware obj)
        {

            return _hardwareRepository.List(new Hardware { });
        }

        public Task<StatusModel> Update(Hardware obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/Hardware/Index" };
            try
            {
                int retId = _hardwareRepository.Update(obj).Result;
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
