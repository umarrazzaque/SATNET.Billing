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
            //test job
            var invoiceJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(InvoiceJob), "Invoice Job", "0 0 1 * * ?");// Every day at 1am
            //var invoiceJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(InvoiceJob), "Invoice Job", "0 */2 * ? * *");// Every 2 minutes
            IJobDetail invoiceJob = CreateJob(invoiceJobMetaData);
            ITrigger invoiceTrigger = CreateTrigger(invoiceJobMetaData);

            //MRC job which runs every month to generate recurring invoices
            var MRCJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(MRCJob), "MCR Job", "0 0 0 1 * ?"); // At 00:00:00am, on the 1st day, every month 
            IJobDetail _MRCJob = CreateJob(MRCJobMetaData);
            ITrigger MRCTrigger = CreateTrigger(MRCJobMetaData);

            //End of month job which runs at the end of month 
            //var endOfMonthJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(MRCJob), "End of month job", "0 0 21 ? * * *"); // At 21:00:00pm every day 
            //var endOfMonthJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(EndOfMonthJob), "End of month job", "0 */1 * ? * *"); // Every 1 minutes
            var endOfMonthJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(EndOfMonthJob), "End of month job", "0 0 21 L * ?"); // At 21:00:00pm, on the last day of the month, every month
            IJobDetail _endOfMonthJob = CreateJob(endOfMonthJobMetaData);
            ITrigger endOfMonthTrigger = CreateTrigger(endOfMonthJobMetaData);

            //10th of each month job e.g to terminate all locked sites of previous month.
            //var tenthOfMonthJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(TenthOfMonthJob), "Tenth of month Job", "0 */1 * ? * *"); // Every 1 minutes
            var tenthOfMonthJobMetaData = new JobMetadata(Guid.NewGuid(), typeof(TenthOfMonthJob), "Tenth of month Job", "0 0 0 10 * ?"); // At 00:00:00am, on the 10th day, every month
            IJobDetail _tenthOfMonthJob = CreateJob(tenthOfMonthJobMetaData);
            ITrigger tenthOfMonthTrigger = CreateTrigger(tenthOfMonthJobMetaData);


            Scheduler = await schedulerFactory.GetScheduler();
            Scheduler.JobFactory = jobFactory;

            await Scheduler.ScheduleJob(_MRCJob, MRCTrigger, cancellationToken);
            await Scheduler.ScheduleJob(_endOfMonthJob, endOfMonthTrigger, cancellationToken);
            await Scheduler.ScheduleJob(invoiceJob, invoiceTrigger, cancellationToken);
            await Scheduler.ScheduleJob(_tenthOfMonthJob, tenthOfMonthTrigger, cancellationToken);
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
