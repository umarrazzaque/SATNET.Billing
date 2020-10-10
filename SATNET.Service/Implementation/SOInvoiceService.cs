using SATNET.Domain;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class SOInvoiceService : IService<SOInvoice>
    {
        private readonly IRepository<SOInvoice> _invoiceRepository;
        public SOInvoiceService(IRepository<SOInvoice> invoiceRepository)
        {
            _invoiceRepository = invoiceRepository;
        }
        public async Task<SOInvoice> Get(int id) 
        {
            try
            {
                return await _invoiceRepository.Get(id);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
        public async Task<List<SOInvoice>> List(SOInvoice obj) 
        {
            return await _invoiceRepository.List(obj);
        }
        public async Task<StatusModel> Add(SOInvoice order) { throw new NotImplementedException(); }
        public async Task<StatusModel> Update(SOInvoice order) { throw new NotImplementedException(); }
        public async Task<StatusModel> Delete(int id, int deletedBy) { throw new NotImplementedException(); }

    }
}
