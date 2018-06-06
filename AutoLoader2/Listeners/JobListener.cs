using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AutoLoader2.Form1;

namespace AutoLoader2.Listeners
{
    class JobListener : IJobListener
    {
        public static readonly String LISTENER_NAME = "AutoRunJobListener";
        public JobStatusChange JobStatsChangeEvent;
        public string Name
        {
            get
            {
                return LISTENER_NAME;
            }
        }

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            
        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {

            if (JobStatsChangeEvent != null)
            {
                JobStatsChangeEvent("Running", context, null);
            }

        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            if (JobStatsChangeEvent != null)
            {
                JobStatsChangeEvent("Completed", context, jobException);
            }

        }

    }
}
