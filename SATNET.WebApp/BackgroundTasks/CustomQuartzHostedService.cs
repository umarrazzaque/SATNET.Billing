using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using SATNET.WebApp.BackgroundTasks.Jobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SATNET.WebApp.BackgroundTasks
{
    public class CustomQuartzHostedService : IHostedService
    {
        private readonly ISchedulerFactory schedulerFactory;
        private readonly IJobFactory jobFactory;
        //private readonly JobMetadata jobMetadata;
        public CustomQuartzHostedService(ISchedulerFactory
            schedulerFactory,
            //JobMetadata jobMetadata,
            IJobFactory jobFactory)
        {
            this.schedulerFactory = schedulerFactory;
            //this.jobMetadata = jobMetadata;
            this.jobFactory = jobFactory;
        }
        public IScheduler Scheduler { get; set; }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //var invoiceJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(InvoiceJob), "Invoice Job", "0 */5 * ? * *");
            //IJobDetail invoiceJob = CreateJob(invoiceJobMetaData);
            //ITrigger invoiceTrigger = CreateTrigger(invoiceJobMetaData);

            //var MRCJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(MRCJob), "Invoice Job Monthly", "0 0 0 1 * ?"); // At 00:00:00am, on the 1st day, every month 
            var MRCJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(MRCJob), "Invoice Job Monthly", "0 */1 * ? * *"); // every 1 mins
            IJobDetail _MRCJob = CreateJob(MRCJobMetaData);
            ITrigger MRCTrigger = CreateTrigger(MRCJobMetaData);

            Scheduler = await schedulerFactory.GetScheduler();
            Scheduler.JobFactory = jobFactory;

            await Scheduler.ScheduleJob(_MRCJob, MRCTrigger, cancellationToken);
            //await Scheduler.ScheduleJob(invoiceJob, invoiceTrigger, cancellationToken);
            await Scheduler.Start(cancellationToken);
        }
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler?.Shutdown(cancellationToken);
        }
        private ITrigger CreateTrigger(JobMetadata jobMetadata)
        {
            return TriggerBuilder.Create()
            .WithIdentity(jobMetadata.JobId.ToString())
            .WithCronSchedule(jobMetadata.CronExpression)
            .WithDescription($"{jobMetadata.JobName}")
            .Build();
        }
        private IJobDetail CreateJob(JobMetadata jobMetadata)
        {
            return JobBuilder
            .Create(jobMetadata.JobType)
            .WithIdentity(jobMetadata.JobId.ToString())
            .WithDescription($"{jobMetadata.JobName}")
            .Build();
        }
    }
}
