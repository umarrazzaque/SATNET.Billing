using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SATNET.WebApp.BackgroundTasks.Jobs
{
    [DisallowConcurrentExecution]
    public class NotificationJob : IJob
    {
        private readonly ILogger<NotificationJob> _logger;
        public NotificationJob(ILogger<NotificationJob> logger)
        {
            _logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("As-Salam-o-Alikum!");
            return Task.CompletedTask;
        }
    }
}
