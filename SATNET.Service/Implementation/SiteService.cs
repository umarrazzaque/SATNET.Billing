using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class SiteService : IService<Site>
    {
        private readonly IRepository<Site> _siteRepository;
        public SiteService(IRepository<Site> siteRepository)
        {
            _siteRepository = siteRepository;
        }
        public async Task<StatusModel> Add(Site obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Order/Index" };
            try
            {
                int retId = await _siteRepository.Add(obj);
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
            return status;
        }

        public async Task<StatusModel> Delete(int recId, int deletedBy)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Site/Index" };
            try
            {
                int dRow = await _siteRepository.Delete(recId, deletedBy);
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
                status.IsSuccess = false;
                status.ErrorCode = "An error occured while processing request.";
                status.ErrorDescription = e.Message;
            }
            finally
            {
            }
            return status;
        }

        public async Task<Site> Get(int id)
        {
            var retModel = new Site();
            try
            {
                retModel = await _siteRepository.Get(id);
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

        public async Task<List<Site>> List()
        {
            return await _siteRepository.List();
        }

        public async Task<StatusModel> Update(Site obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Site/Index" };
            try
            {
                int retId = await _siteRepository.Update(obj);
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
            return status;
        }
    }
}
