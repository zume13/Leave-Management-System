using LeaveManagement.Infrastructure.BackgroundJobs;
using Quartz;

namespace LeaveManagement.API.Extensions
{
    public static class QuartzExtension
    {
        public static IServiceCollection AddQuartz(this IServiceCollection services)
        {
            services.AddQuartz(config =>
            {
                var JobKey = new JobKey(nameof(ProcessOutBoxMessageJob));

                config.AddJob<ProcessOutBoxMessageJob>(opt => opt.WithIdentity(JobKey))
                .AddTrigger(
                    trigger =>
                    trigger.ForJob(JobKey)
                           .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(10)
                                        .RepeatForever()));
            });

            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

            return services;
        }
    }
}
