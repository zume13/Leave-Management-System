using Quartz;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveManagement.Infrastructure.BackgroundJobs
{
    public class ProcessOutBoxMessageJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
