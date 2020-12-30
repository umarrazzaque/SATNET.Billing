using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class MRCInvoiceService : IService<MRCInvoice>
    {
        private readonly IRepository<MRCInvoice> _invoiceMRCRepository;
        public MRCInvoiceService(IRepository<MRCInvoice> invoiceMRCRepository)
        {
            _invoiceMRCRepository = invoiceMRCRepository;
        }
        public async Task<MRCInvoice> Get(int id)
        {
            try
            {
                return await _invoiceMRCRepository.Get(id);
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<List<MRCInvoice>> List(MRCInvoice obj)
        {
            List<MRCInvoice> invoices = new List<MRCInvoice>();
            try
            {
                invoices = await _invoiceMRCRepository.List(obj);
            }
            catch (Exception ex)
            {

                throw;
            }

            return invoices;
        }
        public async Task<StatusModel> Add(MRCInvoice order) { throw new NotImplementedException(); }
        public async Task<StatusModel> Update(MRCInvoice order) { throw new NotImplementedException(); }
        public async Task<StatusModel> Delete(int id, int deletedBy) { throw new NotImplementedException(); }

    }
}
