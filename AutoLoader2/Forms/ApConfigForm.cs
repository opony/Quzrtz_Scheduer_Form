using AutoLoader2.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AutoLoader2.Forms
{
    public partial class ApConfigForm : Form
    {
        public ApConfigForm()
        {
            InitializeComponent();
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void ApConfigForm_Load(object sender, EventArgs e)
        {
            thrCntTxt.Text = AppConfigFactory.ThreadCount.ToString();

        }

        

        private bool verify_input()
        {

            if (string.IsNullOrEmpty(thrCntTxt.Text.Trim()))
            {
                MessageBox.Show("Error","Thread count can't empty.");
                return false;
            }
            int intVal = 0;
            if (int.TryParse(thrCntTxt.Text, out intVal) != true)
            {
                MessageBox.Show("Error", "Thread count must be integer number.");
                return false;
            }
            
            return true;
        }
    }
}
