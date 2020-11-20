using SATNET.WebApp.Models.Invoice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Report
{
    public class SiteLedgerViewModel
    {
        public SiteLedgerViewModel()
        {
            InvoiceViewModels = new List<SOInvoiceViewModel>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public List<SOInvoiceViewModel> InvoiceViewModels { get; set; }
    }
}
