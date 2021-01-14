using System;
using System.Collections.Generic;
using System.Text;

namespace SATNET.Repository.Interface
{
    public interface IBackgroundTaskRepository
    {
        public void InsertMRCInvoice(int siteId);
    }
}
