using SATNET.Domain.Reporting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Interface
{
    public interface IReportingService
    {
        public Task<ReceivablePerCategory> GetReceivablePerCategoryReport(int customerId, int siteId);
    }
}
