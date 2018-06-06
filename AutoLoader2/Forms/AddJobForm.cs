
using AutoLoader2.Model;
using AutoLoader2.Proxy;
using AutoLoader2.Schedulers;
using AutoLoader2.Util;

using NLog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static AutoLoader2.Enums.SchedulerEnum;

namespace AutoLoader2.Forms
{
    public partial class AddJobForm : Form
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        private FreqType freqType;
        public AutoRunJob AddJob;
        public string[] existsJobIDs;
        public AutoRunJob EditJob;
        public bool editMode;
        public bool readOnly;

        public AddJobForm()
        {
            InitializeComponent();
        }

        private void freqTypeRdo_CheckedChanged(object sender, EventArgs e)
        {
            if (monRdo.Checked)
            {
                freqType = FreqType.Monthly;
                dayCmb.Enabled = true;
                weekCmb.Enabled = false;
                hourNumDown.Enabled = true;
                minNumDown.Enabled = true;
                secNumDown.Enabled = true;
            }
            else if (weekRdo.Checked)
            {
                freqType = FreqType.Weekly;
                dayCmb.Enabled = false;
                weekCmb.Enabled = true;
                hourNumDown.Enabled = true;
                minNumDown.Enabled = true;
                secNumDown.Enabled = true;
            }
            else if (dailyRdo.Checked)
            {
                freqType = FreqType.Daily;
                dayCmb.Enabled = false;
                weekCmb.Enabled = false;
                hourNumDown.Enabled = true;
                minNumDown.Enabled = true;
                secNumDown.Enabled = true;
            }
            else if (hourRdo.Checked)
            {
                freqType = FreqType.Hour;
                dayCmb.Enabled = false;
                weekCmb.Enabled = false;
                hourNumDown.Enabled = true;
                minNumDown.Enabled = false;
                secNumDown.Enabled = false;
            }
            else if (minRdo.Checked)
            {
                freqType = FreqType.Min;
                dayCmb.Enabled = false;
                weekCmb.Enabled = false;
                hourNumDown.Enabled = false;
                minNumDown.Enabled = true;
                secNumDown.Enabled = false;
            }
            else if (secRdo.Checked)
            {
                freqType = FreqType.Sec;
                dayCmb.Enabled = false;
                weekCmb.Enabled = false;
                hourNumDown.Enabled = false;
                minNumDown.Enabled = false;
                secNumDown.Enabled = true;
            }
        }

        private void AddJobForm_Load(object sender, EventArgs e)
        {
            this.Text = this.Text + " " + AppConfigFactory.IP + " - " + AppConfigFactory.AutoRunID;
            Dictionary<string, Type> funcMap = JobFactory.GetJobMap();
            this.jobFuncCmb.Items.AddRange(funcMap.Keys.ToArray());

            if (editMode)
            {
                jobIDTxt.Enabled = false;
                SetEditJobInfo();
            }
            else if (readOnly)
            {
                SetEditJobInfo();
                saveBtn.Visible = false;
                closeBtn.Visible = false;
                userIDTxt.Text = EditJob.ModifiedUser;
            }
        }


        private void SetEditJobInfo()
        {
            jobIDTxt.Text = EditJob.JobID;
            jobFuncCmb.Text = EditJob.JobFunc;
            if (EditJob.FreqType == FreqType.Monthly)
            {
                monRdo.Checked = true;
                string[] tokens = EditJob.Freq.Split(' ');

                dayCmb.Text = tokens[0];

                string[] timeTokens = tokens[1].Split(':');

                hourNumDown.Value = int.Parse(timeTokens[0]);
                minNumDown.Value = int.Parse(timeTokens[1]);
                secNumDown.Value = int.Parse(timeTokens[2]);


            }
            else if (EditJob.FreqType == FreqType.Weekly)
            {
                weekRdo.Checked = true;
                string[] tokens = EditJob.Freq.Split(' ');

                weekCmb.Text = tokens[0];

                string[] timeTokens = tokens[1].Split(':');

                hourNumDown.Value = int.Parse(timeTokens[0]);
                minNumDown.Value = int.Parse(timeTokens[1]);
                secNumDown.Value = int.Parse(timeTokens[2]);


            }
            else if (EditJob.FreqType == FreqType.Daily)
            {
                dailyRdo.Checked = true;
                string[] timeTokens = EditJob.Freq.Split(':');

                hourNumDown.Value = int.Parse(timeTokens[0]);
                minNumDown.Value = int.Parse(timeTokens[1]);
                secNumDown.Value = int.Parse(timeTokens[2]);
            }
            else if (EditJob.FreqType == FreqType.Hour)
            {
                hourRdo.Checked = true;
                hourNumDown.Value = int.Parse(EditJob.Freq);
            }
            else if (EditJob.FreqType == FreqType.Min)
            {
                minRdo.Checked = true;
                minNumDown.Value = int.Parse(EditJob.Freq);
            }
            else if (EditJob.FreqType == FreqType.Sec)
            {
                secRdo.Checked = true;
                secNumDown.Value = int.Parse(EditJob.Freq);
            }

            stTimePick.Value = EditJob.StartTime;
            descTxt.Text = EditJob.Description;
            label10.Text = "Modified User";
            paramTxt.Text = EditJob.Param;
            mailTxt.Text = EditJob.NotifyMail;

        }

        private void saveBtn_Click(object sender, EventArgs e)
        {

            if (VerifyInputFormat() == false)
                return;

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                if (editMode == false)
                    AddJob = GetRunJobObj();
                else
                    EditJob = GetRunJobObj();

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {

                logger.Error(ex, "Insert job fail");
                MessageBox.Show("Insert job fail." + ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
            
            
        }

        private bool VerifyInputFormat()
        {
            if (string.IsNullOrEmpty(userIDTxt.Text))
            {
                MessageBox.Show("Please input [User ID]");
                return false;
            }

            if (string.IsNullOrEmpty(jobIDTxt.Text))
            {
                MessageBox.Show("Please input [Job ID]");
                return false;
            }

            if (string.IsNullOrEmpty(mailTxt.Text))
            {
                MessageBox.Show("Please input [Notify Mail] list");
                return false;
            }

            if (string.IsNullOrEmpty(jobFuncCmb.Text))
            {
                MessageBox.Show("Please select [Function ID] list");
                return false;
            }

            if (hourRdo.Checked)
            {
                if (hourNumDown.Value <= 0)
                {
                    MessageBox.Show("Hour must be > 0");
                    return false;
                }
            }

            if (minRdo.Checked)
            {
                if (minNumDown.Value <= 0)
                {
                    MessageBox.Show("Min must be > 0");
                    return false;
                }
            }

            if (secRdo.Checked)
            {
                if (secNumDown.Value <= 0)
                {
                    MessageBox.Show("Sec must be > 0");
                    return false;
                }
            }

            if (editMode == false)
            {
                if (existsJobIDs.Contains(jobIDTxt.Text))
                {
                    MessageBox.Show("Job is duplicated.");
                    return false;
                }
            }
            
            return true;
        }
        private AutoRunJob GetRunJobObj()
        {
            AutoRunJob runJob = new AutoRunJob();
            runJob.JobID = jobIDTxt.Text.Trim();
            runJob.JobFunc = jobFuncCmb.Text.Trim();
            runJob.JobHost = AppConfigFactory.IP;
            runJob.JobAutoRunID = AppConfigFactory.AutoRunID;
            runJob.FreqType = freqType;
            runJob.Freq = GetFreqSetting();
            runJob.Enabled = false;
            runJob.StartTime = DateTime.Parse(stTimePick.Text);
            runJob.Param = paramTxt.Text;
            runJob.Description = descTxt.Text;
            runJob.ModifiedTime = DateTime.Now;
            runJob.ModifiedUser = userIDTxt.Text.Trim();
            runJob.CreatedTime = DateTime.Now;
            runJob.CreatedUser = userIDTxt.Text.Trim();
            runJob.NotifyMail = mailTxt.Text.Trim();
            return runJob;

        }

        private string GetFreqSetting()
        {
            string setting = "";
            if (monRdo.Checked)
            {   
                setting = dayCmb.Text + " " + hourNumDown.Value + ":" + minNumDown.Value + ":" + secNumDown.Value;
            }
            else if (weekRdo.Checked)
            {   
                setting = weekCmb.Text + " " + hourNumDown.Value + ":" + minNumDown.Value + ":" + secNumDown.Value;
            }
            else if (dailyRdo.Checked)
            {
                
                setting = hourNumDown.Value + ":" + minNumDown.Value + ":" + secNumDown.Value;
            }
            else if (hourRdo.Checked)
            {   
                setting = hourNumDown.Value.ToString();
            }
            else if (minRdo.Checked)
            {   
                setting = minNumDown.Value.ToString();
            }
            else if (secRdo.Checked)
            {   
                setting = secNumDown.Value.ToString();
            }

            return setting;
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
