using Quartz;
using SATNET.Domain;
using SATNET.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.BackgroundTasks.Jobs
{
    [DisallowConcurrentExecution]
    public class InvoiceJob : IJob
    {
        private readonly IBackgroundTaskService _backgroundTaskService;
        public InvoiceJob(IBackgroundTaskService backgroundTaskService)
        {
            _backgroundTaskService = backgroundTaskService;

        }
        public Task Execute(IJobExecutionContext context)
        {
            //_logger.LogInformation("As-Salam-o-Alikum!");
            _backgroundTaskService.InsertMRCInvoice();
            return Task.CompletedTask;
        }

    }
}
