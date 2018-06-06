using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using static AutoLoader2.Form1;

namespace AutoLoader2.Listeners
{
    class SchedulerListener : ISchedulerListener
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public SchedulerStatusChange SchedulerChangeEvent;
        public void JobAdded(IJobDetail jobDetail)
        {
            logger.Debug("The scheduler added job : {0}", jobDetail.Key.Name);
        }

        public void JobDeleted(JobKey jobKey)
        {
            logger.Debug("The scheduler deleted job : {0}", jobKey.Name);
        }

        public void JobPaused(JobKey jobKey)
        {
            logger.Debug("The scheduler paused job : {0}", jobKey.Name);
        }

        public void JobResumed(JobKey jobKey)
        {
            logger.Debug("The scheduler resumed job : {0}", jobKey.Name);
        }

        public void JobScheduled(ITrigger trigger)
        {
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void JobUnscheduled(TriggerKey triggerKey)
        {
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void JobsPaused(string jobGroup)
        {
            Trace.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void JobsResumed(string jobGroup)
        {
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void SchedulerError(string msg, SchedulerException cause)
        {
            logger.Error(cause,"Scheduler error , {0}", msg);
            //Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void SchedulerInStandbyMode()
        {
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void SchedulerShutdown()
        {
            logger.Debug("The scheduler Shutdown");
        }

        public void SchedulerShuttingdown()
        {
            logger.Debug("The scheduler Shuttingdown");
            //Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void SchedulerStarted()
        {

            logger.Debug("The scheduler Started");
            SchedulerChangeEvent("Start");
        }

        public void SchedulerStarting()
        {
            logger.Debug("The scheduler Starting");
        }

        public void SchedulingDataCleared()
        {
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void TriggerFinalized(ITrigger trigger)
        {
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void TriggerPaused(TriggerKey triggerKey)
        {
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void TriggerResumed(TriggerKey triggerKey)
        {
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void TriggersPaused(string triggerGroup)
        {
            SchedulerChangeEvent("Stop");
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }

        public void TriggersResumed(string triggerGroup)
        {
            SchedulerChangeEvent("Start");
            Console.WriteLine("The scheduler called {0}", MethodBase.GetCurrentMethod().Name);
        }
    }
}
