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
    public class PackageService : IService<Package>
    {
        private readonly IRepository<Package> _packageRepository;
        public PackageService(IRepository<Package> packageRepository)
        {
            _packageRepository = packageRepository;
        }
        public Task<Package> Get(int id)
        {
            var retModel = new Package();
            try
            {
                retModel = _packageRepository.Get(id).Result;
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
        public Task<List<Package>> List()
        {
            return _packageRepository.List();
        }
        public Task<StatusModel> Add(Package package)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Package/Index" };
            try
            {
                int retId = _packageRepository.Add(package).Result;
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
            //return _packageRepository.Add(package);
        }
        public Task<StatusModel> Update(Package package)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Package/Index" };
            try
            {
                int retId = _packageRepository.Update(package).Result;
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "Package/Index" };
            try
            {
                int dRow = _packageRepository.Delete(recId, deletedBy).Result;
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
