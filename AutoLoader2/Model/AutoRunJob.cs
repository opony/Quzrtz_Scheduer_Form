using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static AutoLoader2.Enums.SchedulerEnum;

namespace AutoLoader2.Model
{
    public class AutoRunJob
    {
        public string JobID
        { get; set; }

        public string JobFunc
        { get; set; }

        public string JobAutoRunID
        { get; set; }

        public string JobHost
        { get; set; }

        public bool Enabled
        { get; set; }

        public string Status
        { get; set; }

        public string Msg
        { get; set; }

        public FreqType FreqType
        { get; set; }

        public string Freq
        { get; set; }

        public DateTime UpdateTime
        { get; set; }

        public DateTime StartTime
        { get; set; }

        public DateTime? LastRunTime
        { get; set; }

        public DateTime? NextRunTime
        { get; set; }

        public string ExecTime
        { get; set; }

        public string Param
        { get; set; }

        public string Description
        { get; set; }

        public string CreatedUser
        { get; set; }

        public DateTime CreatedTime
        { get; set; }


        public string ModifiedUser
        { get; set; }

        public DateTime ModifiedTime
        { get; set; }

        public string NotifyMail
        {
            get; set;
        }

    }
}
