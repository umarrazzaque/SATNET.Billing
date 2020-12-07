using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Domain
{
    public class SiteLedger
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public List<SOInvoice> Invoices { get; set; }
    }
}
