using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class SiteService : IServices<Site>
    {
        private readonly IRepository<Site> _siteRepository;
        public SiteService(IRepository<Site> siteRepository)
        {
            _siteRepository = siteRepository;
        }
        public Task<StatusModel> Add(Site obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Site/Index" };
            try
            {
                int retId = _siteRepository.Add(obj).Result;
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Site/Index" };
            try
            {
                int dRow = _siteRepository.Delete(recId, deletedBy).Result;
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

        public Task<Site> Get(int id)
        {
            var retModel = new Site();
            try
            {
                retModel = _siteRepository.Get(id).Result;
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

        public Task<List<Site>> List()
        {
            return _siteRepository.List(new Site { });
        }

        public Task<StatusModel> Update(Site obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Site/Index" };
            try
            {
                int retId = _siteRepository.Update(obj).Result;
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
