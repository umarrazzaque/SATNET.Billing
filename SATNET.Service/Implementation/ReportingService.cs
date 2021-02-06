using SATNET.Domain.Reporting;
using SATNET.Repository.Interface;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SATNET.Service.Implementation
{
    public class ReportingService : IReportingService
    {
        private readonly IReportingRepository _reportingRepository;
        public ReportingService(IReportingRepository reportingRepository)
        {
            _reportingRepository = reportingRepository;
        }
        public async Task<ReceivablePerCategory> GetReceivablePerCategoryReport(int customerId, int siteId)
        {
            var result = await _reportingRepository.GetReceivablePerCategoryReport(customerId, siteId);
            result.Total = result.TokenTotal + result.ServicePlanTotal + result.PublicIPTotal + result.RebateTotal;
            return result;
        }
    }
}
