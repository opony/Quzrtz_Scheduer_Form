using AutoLoader2.Forms;
using AutoLoader2.Model;
using AutoLoader2.Proxy;
using AutoLoader2.Schedulers;
using AutoLoader2.Util;
using NLog;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static AutoLoader2.Enums.SchedulerEnum;

namespace AutoLoader2
{
    public partial class Form1 : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public delegate void JobStatusChange(string status, IJobExecutionContext context, JobExecutionException jobException);

        public delegate void SchedulerStatusChange(string status);

        AutoRunScheduler scheduler = null;
        private static readonly int ENABLED_FLAG = 1;
        private static readonly int DISABLE_FLAG = 0;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            NLog.LogManager.Configuration = new NLog.Config.XmlLoggingConfiguration(assemblyFolder + "\\NLog.config", true);
            
            this.Text = this.Text + " " + AppConfigFactory.IP + " - " + AppConfigFactory.AutoRunID;
            scheduler = new AutoRunScheduler(AppConfigFactory.ThreadCount);

            scheduler.SetJobListener(new JobStatusChange(UpdateJobStatus));
            scheduler.SetSchedulerListener(new SchedulerStatusChange(UpdateSchedulerStatus));

            scheduler.Start();
            UpdateForm();

            UpdateJobListGrid();

            SchedulerAddEnbabledJob();

        }

        private void UpdateSchedulerStatus(string status)
        {
            if (this.InvokeRequired)
            {
                SchedulerStatusChange d = new SchedulerStatusChange(UpdateSchedulerStatus);
                this.Invoke(d, new object[] { status });
            }
            else
            {
                schStateLab.Text = status;
                if (status == "Start")
                {
                    schStateLab.BackColor = Color.LimeGreen;
                }
                else
                {
                    schStateLab.BackColor = Color.Red;
                }
            }
            
        }

        private void SendErrorToNotifyMail(string jobID, string errMsg, string notifyMail)
        {
            if (errMsg.Length > 3500)
                errMsg = errMsg.Substring(0, 3500);

            logger.Error("[" + jobID + "]  error. {0}", errMsg);
            logger.Debug("Send mail to Nodify_Mail : {0}", notifyMail);
            string[] mailToArr = notifyMail.Split(';');
            try
            {
                SendMailUtil.SendMail(mailToArr, "AutoRun2 : [" + jobID + "]  error", errMsg);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Send mail error.");
            }

        }

        private void UpdateJobStatus(string status, IJobExecutionContext context, JobExecutionException jobException)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    
                    string msg = "";
                    string jobID = context.JobDetail.Key.Name;

                    if (jobException != null)
                    {
                        status = "Error";
                        msg = jobException.ToString();
                        string mailToStr = Convert.ToString(context.JobDetail.JobDataMap["Notify_Mail"]);
                        string[] mailToArr = mailToStr.Split(';');
                        try
                        {
                            SendErrorToNotifyMail(jobID, msg, mailToStr);
                        }
                        catch (Exception ex)
                        {

                            logger.Error(ex, "Send mail error.");
                            MessageBox.Show("Send mail error. " + ex.ToString());
                        }

                    }

                    DataTable tb = (DataTable)jobGrid.DataSource;
                    lock (tb)
                    {
                        var jobRow = (from row in tb.AsEnumerable()
                                      where row.Field<string>("JOB_ID") == jobID
                                      select row).Single();

                        string exectedTime = "";
                        DateTime triggerTime = context.FireTimeUtc.Value.DateTime.ToLocalTime();
                        if (status == "Completed" || status == "Error")
                        {
                            exectedTime = context.JobRunTime.ToString("hh\\:mm\\:ss");
                            DateTime? nextRunTime = null;
                            if (context.NextFireTimeUtc.HasValue)
                                nextRunTime = context.NextFireTimeUtc.Value.LocalDateTime;

                            //DateTime nextRunTime = context.NextFireTimeUtc.Value.LocalDateTime;
                            MesHistDataProxy.UpdateExecutedStatus(jobID, AppConfigFactory.IP, AppConfigFactory.AutoRunID, status, msg, exectedTime, triggerTime, nextRunTime);
                            jobRow["STATUS"] = status;
                            jobRow["MSG"] = msg;
                            jobRow["EXEC_TIME"] = exectedTime;
                            jobRow["UPDATE_TIME"] = DateTime.Now;
                            if (context.NextFireTimeUtc.HasValue)
                                jobRow["NEXT_RUN_TIME"] = nextRunTime;
                            jobRow["LAST_RUN_TIME"] = triggerTime;

                        }
                        else if (status == "Running")
                        {
                            //running status
                            MesHistDataProxy.UpdateRunningStatus(jobID, AppConfigFactory.IP, AppConfigFactory.AutoRunID, status, triggerTime);
                            jobRow["STATUS"] = status;
                            jobRow["MSG"] = msg;
                            jobRow["LAST_RUN_TIME"] = triggerTime;
                            jobRow["UPDATE_TIME"] = DateTime.Now;

                        }
                    }
                    
                    

                    JobStatusChange d = new JobStatusChange(UpdateJobStatus);
                    this.Invoke(d, new object[] { status, context, jobException });
                }
                else
                {
                    DataTable tb = (DataTable)jobGrid.DataSource;
                    lock (tb)
                    {
                        runCntTxt.Text = (from row in tb.AsEnumerable()
                                          where row.Field<string>("STATUS") == "Running"
                                          select row).Count().ToString();
                    }
                    
                    
                }
            }
            catch (Exception ex)
            {
                
                logger.Error(ex, "Update job status error.");
                SendMailUtil.SendMail(new string[] { "pony.liu@lextar.com" }, "Update job status error", ex.ToString() + "<br>" + ex.StackTrace);
                MessageBox.Show("Update job status error. " + ex.ToString());
            }
            


        }




        private void SchedulerAddEnbabledJob()
        {
            DataTable tb = (DataTable)jobGrid.DataSource;
            var enbRows = (from row in tb.AsEnumerable()
                           where row.Field<decimal>("ENABLED") == 1
                           select row).ToArray();

            enabTxt.Text = Convert.ToString(enbRows.Length);

            AutoRunJob runJob = null;
            DateTime nextRunTime = DateTime.Now;
            foreach (DataRow row in enbRows)
            {
                runJob = this.ConvertToAutoRunJob(row);
                nextRunTime = scheduler.AddJob(runJob);
                runJob.NextRunTime = nextRunTime;
                //MesHistDataProxy.UpdateJob(runJob);

                row["UPDATE_TIME"] = DateTime.Now;
                row["NEXT_RUN_TIME"] = nextRunTime;
            }

            

            //List < AutoRunJob > enabedJobList = new List<AutoRunJob>();

            //if (jobGrid.DataSource == null)
            //    return enabedJobList;

            //DataTable tb = (DataTable)jobGrid.DataSource;

            //var enbRows = (from row in tb.AsEnumerable()
            //               where row.Field<decimal>("ENABLED") == 1
            //               select row).ToArray();

            //AutoRunJob runJob = null;
            //foreach (DataRow row in enbRows)
            //{
            //    runJob = ConvertToAutoRunJob(row);
            //    enabedJobList.Add(runJob);
            //}

            //return enabedJobList;
        }

        private AutoRunJob ConvertToAutoRunJob(DataRow row)
        {
            AutoRunJob job = new AutoRunJob();
            job.JobID = row.Field<string>("JOB_ID");
            job.JobFunc = row.Field<string>("JOB_FUNC");
            job.JobHost = row.Field<string>("JOB_HOST");
            job.JobAutoRunID = row.Field<string>("JOB_AUTO_RUN_ID");
            job.Enabled = row.Field<Decimal>("ENABLED") == 1;
            job.Status = row.Field<string>("STATUS");
            string freqType = row.Field<string>("FREQ_TYPE");
            job.FreqType = (FreqType)Enum.Parse(typeof(FreqType), freqType);
            job.Freq = row.Field<string>("FREQ");
            job.StartTime = row.Field<DateTime>("START_TIME");
            if (row["LAST_RUN_TIME"] != DBNull.Value && row["LAST_RUN_TIME"] != null)
                job.LastRunTime = row.Field<DateTime>("LAST_RUN_TIME");
            job.ExecTime = row.Field<string>("EXEC_TIME");
            job.Msg = row.Field<string>("MSG");
            job.Param = row.Field<string>("PARAM");
            job.Description = row.Field<string>("DESCRIPTION");

            job.CreatedTime = row.Field<DateTime>("CREATED_TIME");
            job.CreatedUser = row.Field<string>("CREATED_USER");
            job.ModifiedTime = row.Field<DateTime>("MODIFIED_TIME");
            job.ModifiedUser = row.Field<string>("MODIFIED_USER");
            job.NotifyMail = row.Field<string>("NOTIFY_MAIL");
            

            return job;
        }
        private void UpdateJobListGrid()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                jobGrid.DataSource = MesHistDataProxy.QueryAllAutoRunJobs(AppConfigFactory.IP, AppConfigFactory.AutoRunID);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Update Job List fail.");
                MessageBox.Show("Update Job List fail." + ex.ToString());
                
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }

            
        }

        private void cfgBtn_Click(object sender, EventArgs e)
        {
            ApConfigForm apConfForm = new ApConfigForm();
            DialogResult dialogResult = apConfForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                UpdateForm();
            }
        }

        private void UpdateForm()
        {
            thrCntTxt.Text = AppConfigFactory.ThreadCount.ToString();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            AddJobForm jobForm = new AddJobForm();

            DataTable tb = (DataTable)jobGrid.DataSource;
            jobForm.existsJobIDs = (from row in tb.AsEnumerable()
                                    select row.Field<string>("JOB_ID")).ToArray();

            DialogResult dialogRes = jobForm.ShowDialog();
            if (dialogRes == DialogResult.OK)
            {
                try
                {
                    MesHistDataProxy.Insert(jobForm.AddJob);
                    JobListGridAddNewJob(jobForm.AddJob);
                    MessageBox.Show("Add new job completed~!");
                }
                catch (Exception ex)
                {

                    logger.Error(ex, "Add new job error.");
                    MessageBox.Show("Add new job error! " + ex.ToString());
                }
                

            }

            

        }

        private DataRow ConvertToRow(AutoRunJob runJob)
        {
            DataTable tb = (DataTable)jobGrid.DataSource;
            DataRow row = tb.NewRow();
            row["JOB_ID"] = runJob.JobID;
            row["JOB_FUNC"] = runJob.JobFunc;
            row["JOB_HOST"] = runJob.JobHost;
            row["JOB_AUTO_RUN_ID"] = runJob.JobAutoRunID;
            row["ENABLED"] = runJob.Enabled ? 1 : 0;
            row["STATUS"] = runJob.Status;
            row["FREQ_TYPE"] = runJob.FreqType.ToString();
            row["FREQ"] = runJob.Freq;
            row["START_TIME"] = runJob.StartTime;
            if(runJob.LastRunTime.HasValue)
                row["LAST_RUN_TIME"] = runJob.LastRunTime;

            row["EXEC_TIME"] = runJob.ExecTime;
            row["MSG"] = runJob.Msg;
            row["PARAM"] = runJob.Param;
            row["DESCRIPTION"] = runJob.Description;
            row["MODIFIED_TIME"] = runJob.ModifiedTime;
            row["MODIFIED_USER"] = runJob.ModifiedUser;
            row["CREATED_TIME"] = runJob.CreatedTime;
            row["CREATED_USER"] = runJob.CreatedUser;
            row["NOTIFY_MAIL"] = runJob.NotifyMail;

            return row;
        }

        private void JobListGridAddNewJob(AutoRunJob addJob)
        {
            //((List<AutoRunJob>)jobGrid.DataSource).Add(addJob);


            DataTable tb = (DataTable)jobGrid.DataSource;
            //DataRow row = tb.NewRow();
            //row["JOB_ID"] = addJob.JobID;
            //row["JOB_FUNC"] = addJob.JobFunc;
            //row["JOB_HOST"] = addJob.JobHost;
            //row["JOB_AUTO_RUN_ID"] = addJob.JobAutoRunID;
            //row["ENABLED"] = addJob.Enabled ? 1 : 0;
            //row["STATUS"] = addJob.Status;
            //row["FREQ_TYPE"] = addJob.FreqType.ToString();
            //row["FREQ"] = addJob.Freq;
            //row["START_TIME"] = addJob.StartTime;
            //row["LAST_RUN_TIME"] = addJob.LastRunTime;
            //row["EXEC_TIME"] = addJob.ExecTime;
            //row["MSG"] = addJob.Msg;
            //row["PARAM"] = addJob.Param;

            //row["DESCRIPTION"] = addJob.Description;
            //row["MODIFIED_TIME"] = addJob.ModifiedTime;
            //row["MODIFIED_USER"] = addJob.ModifiedUser;
            //row["CREATED_TIME"] = addJob.CreatedTime;
            //row["CREATED_USER"] = addJob.CreatedUser;
            //row["NOTIFY_MAIL"] = addJob.NotifyMail;
            DataRow row = ConvertToRow(addJob);
            tb.Rows.Add(row);


        }
        private void enbBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (jobGrid.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("Please select a job to do [Enabled].");
                    return;
                }

                DataTable tb = (DataTable)jobGrid.DataSource;

                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Do you want to enable Job .");
                foreach (DataGridViewRow selRow in jobGrid.SelectedRows)
                {

                    string jobID = Convert.ToString(tb.Rows[selRow.Index].Field<string>("JOB_ID"));
                    msg.AppendLine(jobID);
                }

                DialogResult dialogResult = MessageBox.Show(msg.ToString(), "Enable Job", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    DataRow dataRow = null;
                    
                    foreach (DataGridViewRow selRow in jobGrid.SelectedRows)
                    {
                        dataRow = tb.Rows[selRow.Index];
                        AutoRunJob runJob = this.ConvertToAutoRunJob(dataRow);
                        if (runJob.Enabled != true)
                        {
                            //set enable job
                            string status = "Start";
                            DateTime nextRunTime = scheduler.AddJob(runJob);
                            MesHistDataProxy.UpdateEnbableStatus(runJob.JobID, runJob.JobHost, runJob.JobAutoRunID, ENABLED_FLAG, status, nextRunTime);
                            
                            dataRow["ENABLED"] = ENABLED_FLAG;
                            dataRow["STATUS"] = status;
                            dataRow["UPDATE_TIME"] = DateTime.Now;
                            dataRow["NEXT_RUN_TIME"] = nextRunTime;
                        }
                    }

                    enabTxt.Text = (from row in tb.AsEnumerable()
                                 where row.Field<decimal>("ENABLED") == 1
                                 select row).Count().ToString();

                    //MessageBox.Show("Enabled completed~!");


                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Enable job error.");
                MessageBox.Show("Enable job error." + ex.ToString());
            }
            
                        
        }


        
        private void disabledBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (jobGrid.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("Please select a job to do [Disabled].");
                    return;
                }

                DataTable tb = (DataTable)jobGrid.DataSource;

                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Do you want to disable Job .");
                string jobID;
                foreach (DataGridViewRow selRow in jobGrid.SelectedRows)
                {

                    jobID = Convert.ToString(tb.Rows[selRow.Index].Field<string>("JOB_ID"));
                    msg.AppendLine(jobID);
                }


                
                DialogResult dialogResult = MessageBox.Show(msg.ToString(), "Disable Job", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string errMsg = "";
                    
                    DataRow dataRow = null;
                    foreach (DataGridViewRow selRow in jobGrid.SelectedRows)
                    {
                        dataRow = tb.Rows[selRow.Index];
                        AutoRunJob runJob = this.ConvertToAutoRunJob(dataRow);

                        if (runJob.Enabled)
                        {
                            if (scheduler.RemoveJob(runJob.JobID, out errMsg) != true)
                            {
                                MessageBox.Show("Disable job fail." + errMsg);
                                return;
                            }
                            //set distable
                            string status = "Stop";
                            
                            MesHistDataProxy.UpdateEnbableStatus(runJob.JobID, runJob.JobHost, runJob.JobAutoRunID, DISABLE_FLAG, status, null);
                            dataRow["ENABLED"] = DISABLE_FLAG;
                            dataRow["STATUS"] = status;
                            dataRow["UPDATE_TIME"] = DateTime.Now;
                        }
                    }

                    enabTxt.Text = (from row in tb.AsEnumerable()
                                    where row.Field<decimal>("ENABLED") == 1
                                    select row).Count().ToString();

                    //MessageBox.Show("Disable completed~!");
                }
            }
            catch (Exception ex)
            {

                logger.Error(ex, "Disable job error.");
                MessageBox.Show("Disable job error." + ex.ToString());
            }
            
            
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            if (schStateLab.Text == "Start")
            {
                MessageBox.Show("AutoRunLoader is Start, can't to close.");
                return;
            }

            int runCount = scheduler.GetCurrentlyRunJobCount();
            if (runCount > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Auto run have [" + runCount + "] running~! Do you want force stop?", "Disable Job", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    
                    scheduler.Shutdown();
                    this.Close();
                }
                else
                    return;   
            }
            scheduler.Shutdown();
            this.Close();
        }

        private void editJobBtn_Click(object sender, EventArgs e)
        {
            if (jobGrid.SelectedRows.Count <= 0)
            {
                MessageBox.Show("Please select a job to do [Edit].");
                return;
            }

            int selIdx = jobGrid.SelectedRows[0].Index;
            DataTable tb = (DataTable)jobGrid.DataSource;
            DataRow selRow = tb.Rows[selIdx];

            AutoRunJob editJob = this.ConvertToAutoRunJob(selRow);

            if (editJob.Enabled)
            {
                MessageBox.Show("The Job is [Enabled] , can't do [edit]");
                return;
            }
            AddJobForm jobForm = new AddJobForm();
            jobForm.editMode = true;
            jobForm.EditJob = editJob;
            
            DialogResult dialogRes = jobForm.ShowDialog();
            if (dialogRes == DialogResult.OK)
            {
                try
                {
                    editJob = jobForm.EditJob;
                    MesHistDataProxy.UpdateJob(jobForm.EditJob);
                    

                    selRow["JOB_FUNC"] = editJob.JobFunc;
                    selRow["FREQ_TYPE"] = editJob.FreqType.ToString();
                    selRow["FREQ"] = editJob.Freq;
                    selRow["START_TIME"] = editJob.StartTime;
                    selRow["PARAM"] = editJob.Param;
                    selRow["DESCRIPTION"] = editJob.Description;
                    selRow["MODIFIED_TIME"] = editJob.ModifiedTime;
                    selRow["MODIFIED_USER"] = editJob.ModifiedUser;
                    selRow["NOTIFY_MAIL"] = editJob.NotifyMail;
                    selRow["UPDATE_TIME"] = DateTime.Now;

                    MessageBox.Show("Edit job completed~!");
                }
                catch (Exception ex)
                {

                    logger.Error(ex, "Edit job error!");
                    MessageBox.Show("Edit job error! " + ex.ToString());
                }


            }


        }

        private void delJobBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (jobGrid.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("Please select a job to do [Edit].");
                    return;
                }

                int selIdx = jobGrid.SelectedRows[0].Index;
                DataTable tb = (DataTable)jobGrid.DataSource;
                DataRow delRow = tb.Rows[selIdx];
                AutoRunJob delJob = this.ConvertToAutoRunJob(delRow);
                if (delJob.Enabled)
                {
                    MessageBox.Show("The Job is [Enabled] , can't do [Delete]");
                    return;
                }
                StringBuilder msg = new StringBuilder();
                msg.AppendLine("Do you want to delete the job id ?")
                    .AppendLine("[" + delJob.JobID + "]");
                DialogResult dialogResult = MessageBox.Show(msg.ToString(), "Warning ! Delete Job", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    MesHistDataProxy.DeleteJob(delJob.JobID, AppConfigFactory.IP, AppConfigFactory.AutoRunID);
                    tb.Rows.Remove(delRow);
                    MessageBox.Show("Delete Job [" + delJob.JobID + "] completed~!");
                }

            }
            catch (Exception ex)
            {

                logger.Error(ex, "Delete Job error.");
                MessageBox.Show("Delete Job error. " + ex.ToString());
            }

            
        }

        private void schRunBtn_Click(object sender, EventArgs e)
        {
            scheduler.ResumeAll();
        }

        private void autoRunStop_Click(object sender, EventArgs e)
        {

            scheduler.Stop();
        }

        private void runOnceBtn_Click(object sender, EventArgs e)
        {
            if (jobGrid.SelectedRows.Count <= 0)
            {
                MessageBox.Show("Please select a job .");
                return;
            }

            AutoRunJob job = GetSelectedJobFromGrid();
            if (job.Enabled == false)
            {
                MessageBox.Show("Can't run once the [Disabled] Job.");
                return;
            }

            scheduler.RunOnce(job.JobID);

        }

        private AutoRunJob GetSelectedJobFromGrid()
        {
            int selIdx = jobGrid.SelectedRows[0].Index;
            DataTable tb = (DataTable)jobGrid.DataSource;
            DataRow delRow = tb.Rows[selIdx];
            AutoRunJob job = this.ConvertToAutoRunJob(delRow);

            return job;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            

            if (schStateLab.Text == "Start")
            {
                MessageBox.Show("AutoRunLoader is Start, can't to close.");
                e.Cancel = true;
                return;
            }

            int runCount = scheduler.GetCurrentlyRunJobCount();
            if (runCount > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Auto run have [" + runCount + "] running~! Do you want force stop?", "Disable Job", MessageBoxButtons.YesNo);
                if (dialogResult != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            
            scheduler.Shutdown();

        }

        private void jobGrid_DoubleClick(object sender, EventArgs e)
        {   
            int selIdx = jobGrid.SelectedRows[0].Index;
            DataTable tb = (DataTable)jobGrid.DataSource;
            DataRow selRow = tb.Rows[selIdx];
            AutoRunJob editJob = this.ConvertToAutoRunJob(selRow);
            AddJobForm jobForm = new AddJobForm();
            jobForm.readOnly = true;
            jobForm.EditJob = editJob;

            DialogResult dialogRes = jobForm.ShowDialog();
        }
    }
}
