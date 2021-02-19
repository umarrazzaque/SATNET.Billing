using SATNET.Domain.Reporting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Repository.Interface
{
    public interface IReportingRepository
    {
        public Task<ReceivablePerCategory> GetReceivablePerCategoryReport(int customerId, int siteId);
    }
}
