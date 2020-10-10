using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class CreditNoteService : IService<CreditNote>
    {
        private readonly IRepository<CreditNote> _creditnoteRepository;
        public CreditNoteService(IRepository<CreditNote> creditnoteRepository)
        {
            _creditnoteRepository = creditnoteRepository;
        }
        public async Task<CreditNote> Get(int id)
        {
            var retModel = new CreditNote();
            try
            {
                retModel = await _creditnoteRepository.Get(id);
            }
            catch (Exception e)
            {

            }
            finally
            {

            }
            return retModel;
        }
        public async Task<List<CreditNote>> List(CreditNote obj)
        {
            return await _creditnoteRepository.List(obj);
        }
        public async Task<StatusModel> Add(CreditNote obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/CreditNote/Index" };
            try
            {
                int retId = -1;
                retId = await _creditnoteRepository.Add(obj);
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
        public async Task<StatusModel> Update(CreditNote obj)
        {
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/CreditNote/Index" };
            try
            {
                int retId = -1;
                retId = await _creditnoteRepository.Add(obj);
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
            var status = new StatusModel { IsSuccess = false, ResponseUrl = "/CreditNote/Index" };
            try
            {
                int retId = -1;
                retId = await _creditnoteRepository.Delete(id, deletedBy);
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
