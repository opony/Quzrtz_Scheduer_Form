
using AutoLoader2.Listeners;
using AutoLoader2.Model;
using NLog;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using static AutoLoader2.Enums.SchedulerEnum;
using static AutoLoader2.Form1;

namespace AutoLoader2.Schedulers
{
    class AutoRunScheduler
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //IScheduler scheduler = (IScheduler)StdSchedulerFactory.GetDefaultScheduler();
        IScheduler scheduler = null;
        Dictionary<string, Type> jobMap = JobFactory.GetJobMap();

        public void SetJobListener(JobStatusChange jobStatsChange)
        {
            JobListener jobListener = new JobListener();
            jobListener.JobStatsChangeEvent += jobStatsChange;
            scheduler.ListenerManager.AddJobListener(jobListener, GroupMatcher<JobKey>.AnyGroup());
        }

        public void SetSchedulerListener(SchedulerStatusChange schedStatsChange)
        {
            SchedulerListener schedulerListener = new SchedulerListener();
            schedulerListener.SchedulerChangeEvent += schedStatsChange;

            scheduler.ListenerManager.AddSchedulerListener(schedulerListener);
        }

        public AutoRunScheduler(int threadCount)
        {
            var properties = new NameValueCollection();
            properties["quartz.threadPool.threadCount"] = threadCount.ToString();

            ISchedulerFactory sf = new StdSchedulerFactory(properties);
            scheduler = sf.GetScheduler();

        }
        //public void AddSchedulerJobs(List<AutoRunJob> runJobList)
        //{
            
        //    foreach (var runjob in runJobList)
        //    {
        //        if (runjob.Enabled == false)
        //            continue;

        //        //check job already existsed scheduler
        //        if (GetJobKeyByID(runjob.JobID) != null)
        //            continue;

                
        //        AddJob(runjob);
        //    }
        //}

        public int GetCurrentlyRunJobCount()
        {
            return scheduler.GetCurrentlyExecutingJobs().Count;
        }
        public void Start()
        {
            logger.Info("Scheduler Start");
            scheduler.Start();
            
        }

        public void ResumeAll()
        {
            logger.Info("Scheduler Resume All");
            scheduler.ResumeAll();
        }

        public void Stop()
        {
            logger.Info("Scheduler Pause all");
            scheduler.PauseAll();
        }

        public void Shutdown()
        {
            logger.Info("Scheduler shutdown");
            scheduler.Shutdown();
        }

        public bool RemoveJob(string jobID, out string msg)
        {
            logger.Info("RemoveJob {0}", jobID);
            JobKey jobKey = GetJobKeyByID(jobID);
            if (jobKey == null)
            {
                msg = "Can't found the job : " + jobID;
                logger.Error("Return : {0}", msg);
                return false;
            }

            scheduler.DeleteJob(jobKey);
            msg = "OK";
            logger.Info("Return : {0}", msg);
            return true;
        }

        private JobKey GetJobKeyByID(string jobID)
        {
            IList<string> jobGroups = scheduler.GetJobGroupNames();
            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = scheduler.GetJobKeys(groupMatcher);
                var selJobKey = (from row in jobKeys
                                where row.Name == jobID
                                select row).SingleOrDefault();
                return selJobKey;
            }

            return null;

        }

        public void RunOnce(string jobID)
        {
            logger.Info("Trigger RunOnce : {0}", jobID);
            JobKey jobKey = GetJobKeyByID(jobID);
            if (jobKey == null)
            {
                throw new Exception("Can't found the job " + jobID + " in the auto run scheduler.");
            }

            scheduler.TriggerJob(jobKey);
        }
        public DateTime AddJob(AutoRunJob runJob)
        {
            logger.Info("Scheduler add job.");
            logger.Debug("Job ID : {0}" + runJob.JobID);
            logger.Debug("Job Func : {0}" + runJob.JobFunc);
            logger.Debug("FreqType : {0}" + runJob.FreqType.ToString());
            logger.Debug("Freq : {0}" + runJob.Freq);

            IJobDetail job = JobBuilder.Create(jobMap[runJob.JobFunc])
                            .WithIdentity(runJob.JobID)
                            .UsingJobData("Param", runJob.Param)
                            .UsingJobData("Notify_Mail", runJob.NotifyMail)
                            .Build();

            ITrigger trigger = null;
            if (runJob.FreqType == FreqType.Monthly)
                trigger = GetMonthlyTrigger(runJob.StartTime, runJob.Freq);
            else if(runJob.FreqType == FreqType.Weekly)
                trigger = GetWeeklyTrigger(runJob.StartTime, runJob.Freq);
            else if (runJob.FreqType == FreqType.Daily)
                trigger = GetDailyTrigger(runJob.StartTime, runJob.Freq);
            else if (runJob.FreqType == FreqType.Hour)
                trigger = GetHourTrigger(runJob.StartTime, runJob.Freq);
            else if (runJob.FreqType == FreqType.Min)
                trigger = GetMinTrigger(runJob.StartTime, runJob.Freq);
            else if (runJob.FreqType == FreqType.Sec)
                trigger = GetSecTrigger(runJob.StartTime, runJob.Freq);

            
            scheduler.ScheduleJob(job, trigger);
            DateTime nextFireTime = trigger.GetFireTimeAfter(null).Value.LocalDateTime;
            logger.Info("Next fire time : {0}", nextFireTime.ToString("yyyy-MM-dd HH:mm:ss"));
            return nextFireTime;
        }

        private ITrigger GetMonthlyTrigger(DateTime stTime, string freq)
        {
            //ex: 12 13:00:00
            string[] tokens = freq.Split(' ');
            int dayOfMonth = int.Parse(tokens[0]);

            tokens = tokens[1].Split(':');
            int hour = int.Parse(tokens[0]);
            int min = int.Parse(tokens[1]);
            ITrigger trigger = TriggerBuilder.Create()
                                .StartAt(stTime)
                                .WithSchedule(CronScheduleBuilder.MonthlyOnDayAndHourAndMinute(dayOfMonth, hour, min))
                                .Build();
            
            
            return trigger;
        }

        private ITrigger GetWeeklyTrigger(DateTime stTime, string freq)
        {
            string[] tokens = freq.Split(' ');
            string week = tokens[0];
            DayOfWeek dayOfWeek = DayOfWeek.Monday;
            tokens = tokens[1].Split(':');
            int hour = int.Parse(tokens[0]);
            int min = int.Parse(tokens[1]);

            if (week == "Sun")
            {
                dayOfWeek = DayOfWeek.Sunday;
            }
            else if (week == "Mon")
            {
                dayOfWeek = DayOfWeek.Monday;
            }
            else if (week == "Tue")
            {
                dayOfWeek = DayOfWeek.Tuesday;
            }
            else if (week == "Wen")
            {
                dayOfWeek = DayOfWeek.Wednesday;
            }
            else if (week == "Thu")
            {
                dayOfWeek = DayOfWeek.Thursday;
            }
            else if (week == "Fri")
            {
                dayOfWeek = DayOfWeek.Friday;
            }
            else if (week == "Sat")
            {
                dayOfWeek = DayOfWeek.Saturday;
            }

            ITrigger trigger = TriggerBuilder.Create()
                                .StartAt(stTime)
                                .WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(dayOfWeek, hour, min))
                                .Build();

            return trigger;

        }

        private ITrigger GetDailyTrigger(DateTime stTime, string freq)
        {
            string[] tokens = freq.Split(':');
            int hour = int.Parse(tokens[0]);
            int min = int.Parse(tokens[1]);

            ITrigger trigger = TriggerBuilder.Create()
                                .StartAt(stTime)
                                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(hour, min))
                                .Build();

            return trigger;
        }

        private ITrigger GetHourTrigger(DateTime stTime, string freq)
        {
            int hour = int.Parse(freq);
            ITrigger trigger = TriggerBuilder.Create()
                                .StartAt(stTime)
                                .WithSimpleSchedule(x => x
                                    .WithIntervalInHours(hour)
                                    .RepeatForever())
                                .Build();

            return trigger;
        }

        private ITrigger GetMinTrigger(DateTime stTime, string freq)
        {
            int min = int.Parse(freq);
            ITrigger trigger = TriggerBuilder.Create()
                                .StartAt(stTime)
                                .WithSimpleSchedule(x => x
                                    .WithIntervalInMinutes(min)
                                    .RepeatForever())
                                .Build();

            return trigger;
        }

        private ITrigger GetSecTrigger(DateTime stTime, string freq)
        {
            int sec = int.Parse(freq);
            ITrigger trigger = TriggerBuilder.Create()
                                .StartAt(stTime)
                                .WithSimpleSchedule(x => x
                                    .WithIntervalInSeconds(sec)
                                    .RepeatForever())
                                .Build();

            return trigger;
        }
    }
}
