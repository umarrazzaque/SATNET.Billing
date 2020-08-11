using SATNET.Domain;
using SATNET.Repository.Core;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class LookupService : IService<Lookup>
    {
        private readonly IRepository<Lookup> _lookupRepository;
        public LookupService(IRepository<Lookup> lookupRepository)
        {
            _lookupRepository = lookupRepository;
        }

        public Task<StatusModel> Add(Lookup obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HarwareAttribute/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.Lookups.Add(obj).Result;
                    if (retId != 0)
                    {
                        uow.SaveChanges();
                        status.IsSuccess = true;
                        status.ErrorCode = "Record insert successfully.";
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.ErrorCode = "Error in inserting the record.";
                    }
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

        public async Task<StatusModel> Delete(int id, int deletedBy)
        {
            throw new NotImplementedException();
        }

        public Task<Lookup> Get(int id)
        {
            var retModel = new Lookup();
            try
            {
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    retModel = uow.Lookups.Get(id).Result;
                    if (retModel.Id != 0)
                    {

                    }
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

        public async Task<List<Lookup>> List(Lookup obj)
        {
            return await _lookupRepository.List(obj);
        }

        public Task<StatusModel> Update(Lookup obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/HardwareAttribute/Index" };
            try
            {
                int retId = -1;
                using (var uow = new UnitOfWorkFactory().Create())
                {
                    uow.BeginTransaction();
                    retId = uow.Lookups.Update(obj).Result;
                    if (retId != 0)
                    {
                        uow.SaveChanges();
                        status.IsSuccess = true;
                        status.ErrorCode = "Record update successfully.";
                    }
                    else
                    {
                        status.IsSuccess = false;
                        status.ErrorCode = "Error in updating the record.";
                    }
                }
            }
            catch (Exception e)
            {
                status.IsSuccess = false;
                status.ErrorCode = "An error occured while processing request.";
                status.ErrorDescription = e.Message;
            }
            return Task.FromResult(status);
        }
    }
}
