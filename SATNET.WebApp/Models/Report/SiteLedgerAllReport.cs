using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.Models.Report
{
    public class SiteLedgerAllReport
    {
        public SiteLedgerAllReport()
        {
            SiteLedgers = new List<SiteLedgerViewModel>();
            ReceivableReport = new ReceivablePerCategoryViewModel();
        }
        public List<SiteLedgerViewModel> SiteLedgers { get; set; }
        public ReceivablePerCategoryViewModel ReceivableReport { get; set; }
    }
}
