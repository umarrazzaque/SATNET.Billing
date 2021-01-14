using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Service.Interface
{
    public interface IBackgroundTaskService
    {
        public void InsertMRCInvoice();
        public void InsertMRCInvoiceMonthly();
        public void LockSitesEndOfMonth();
        public void TerminateSitesEndOfMonth();
    }
}
