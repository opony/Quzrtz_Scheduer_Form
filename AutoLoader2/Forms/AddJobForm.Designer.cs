namespace AutoLoader2.Forms
{
    partial class AddJobForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.jobIDTxt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.secNumDown = new System.Windows.Forms.NumericUpDown();
            this.minNumDown = new System.Windows.Forms.NumericUpDown();
            this.hourNumDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.weekCmb = new System.Windows.Forms.ComboBox();
            this.dayCmb = new System.Windows.Forms.ComboBox();
            this.secRdo = new System.Windows.Forms.RadioButton();
            this.minRdo = new System.Windows.Forms.RadioButton();
            this.hourRdo = new System.Windows.Forms.RadioButton();
            this.weekRdo = new System.Windows.Forms.RadioButton();
            this.dailyRdo = new System.Windows.Forms.RadioButton();
            this.monRdo = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.stTimePick = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.descTxt = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.paramTxt = new System.Windows.Forms.TextBox();
            this.saveBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.userIDTxt = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.mailTxt = new System.Windows.Forms.TextBox();
            this.jobFuncCmb = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secNumDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minNumDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.hourNumDown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Job ID :";
            // 
            // jobIDTxt
            // 
            this.jobIDTxt.Location = new System.Drawing.Point(77, 6);
            this.jobIDTxt.Name = "jobIDTxt";
            this.jobIDTxt.Size = new System.Drawing.Size(300, 22);
            this.jobIDTxt.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.secNumDown);
            this.groupBox1.Controls.Add(this.minNumDown);
            this.groupBox1.Controls.Add(this.hourNumDown);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.weekCmb);
            this.groupBox1.Controls.Add(this.dayCmb);
            this.groupBox1.Controls.Add(this.secRdo);
            this.groupBox1.Controls.Add(this.minRdo);
            this.groupBox1.Controls.Add(this.hourRdo);
            this.groupBox1.Controls.Add(this.weekRdo);
            this.groupBox1.Controls.Add(this.dailyRdo);
            this.groupBox1.Controls.Add(this.monRdo);
            this.groupBox1.Location = new System.Drawing.Point(14, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(363, 163);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Freq Type";
            // 
            // secNumDown
            // 
            this.secNumDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.secNumDown.Location = new System.Drawing.Point(218, 118);
            this.secNumDown.Maximum = new decimal(new int[] {
            55,
            0,
            0,
            0});
            this.secNumDown.Name = "secNumDown";
            this.secNumDown.Size = new System.Drawing.Size(62, 22);
            this.secNumDown.TabIndex = 3;
            // 
            // minNumDown
            // 
            this.minNumDown.Location = new System.Drawing.Point(135, 118);
            this.minNumDown.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.minNumDown.Name = "minNumDown";
            this.minNumDown.Size = new System.Drawing.Size(62, 22);
            this.minNumDown.TabIndex = 3;
            // 
            // hourNumDown
            // 
            this.hourNumDown.Location = new System.Drawing.Point(52, 118);
            this.hourNumDown.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.hourNumDown.Name = "hourNumDown";
            this.hourNumDown.Size = new System.Drawing.Size(62, 22);
            this.hourNumDown.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label6.Location = new System.Drawing.Point(201, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 16);
            this.label6.TabIndex = 2;
            this.label6.Text = ":";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label5.Location = new System.Drawing.Point(120, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = ":";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Time :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Week :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Day :";
            // 
            // weekCmb
            // 
            this.weekCmb.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.weekCmb.Enabled = false;
            this.weekCmb.FormattingEnabled = true;
            this.weekCmb.Items.AddRange(new object[] {
            "Sun",
            "Mon",
            "Tue",
            "Wen",
            "Thu",
            "Fri",
            "Sat"});
            this.weekCmb.Location = new System.Drawing.Point(50, 88);
            this.weekCmb.Name = "weekCmb";
            this.weekCmb.Size = new System.Drawing.Size(62, 20);
            this.weekCmb.TabIndex = 1;
            this.weekCmb.Text = "Mon";
            // 
            // dayCmb
            // 
            this.dayCmb.FormattingEnabled = true;
            this.dayCmb.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"});
            this.dayCmb.Location = new System.Drawing.Point(50, 58);
            this.dayCmb.Name = "dayCmb";
            this.dayCmb.Size = new System.Drawing.Size(62, 20);
            this.dayCmb.TabIndex = 1;
            this.dayCmb.Text = "1";
            // 
            // secRdo
            // 
            this.secRdo.AutoSize = true;
            this.secRdo.Location = new System.Drawing.Point(300, 21);
            this.secRdo.Name = "secRdo";
            this.secRdo.Size = new System.Drawing.Size(39, 16);
            this.secRdo.TabIndex = 0;
            this.secRdo.Text = "Sec";
            this.secRdo.UseVisualStyleBackColor = true;
            this.secRdo.CheckedChanged += new System.EventHandler(this.freqTypeRdo_CheckedChanged);
            // 
            // minRdo
            // 
            this.minRdo.AutoSize = true;
            this.minRdo.Location = new System.Drawing.Point(252, 21);
            this.minRdo.Name = "minRdo";
            this.minRdo.Size = new System.Drawing.Size(42, 16);
            this.minRdo.TabIndex = 0;
            this.minRdo.Text = "Min";
            this.minRdo.UseVisualStyleBackColor = true;
            this.minRdo.CheckedChanged += new System.EventHandler(this.freqTypeRdo_CheckedChanged);
            // 
            // hourRdo
            // 
            this.hourRdo.AutoSize = true;
            this.hourRdo.Location = new System.Drawing.Point(199, 21);
            this.hourRdo.Name = "hourRdo";
            this.hourRdo.Size = new System.Drawing.Size(47, 16);
            this.hourRdo.TabIndex = 0;
            this.hourRdo.Text = "Hour";
            this.hourRdo.UseVisualStyleBackColor = true;
            // 
            // weekRdo
            // 
            this.weekRdo.AutoSize = true;
            this.weekRdo.Location = new System.Drawing.Point(80, 21);
            this.weekRdo.Name = "weekRdo";
            this.weekRdo.Size = new System.Drawing.Size(59, 16);
            this.weekRdo.TabIndex = 0;
            this.weekRdo.Text = "Weekly";
            this.weekRdo.UseVisualStyleBackColor = true;
            this.weekRdo.CheckedChanged += new System.EventHandler(this.freqTypeRdo_CheckedChanged);
            // 
            // dailyRdo
            // 
            this.dailyRdo.AutoSize = true;
            this.dailyRdo.Location = new System.Drawing.Point(145, 21);
            this.dailyRdo.Name = "dailyRdo";
            this.dailyRdo.Size = new System.Drawing.Size(48, 16);
            this.dailyRdo.TabIndex = 0;
            this.dailyRdo.Text = "Daily";
            this.dailyRdo.UseVisualStyleBackColor = true;
            this.dailyRdo.CheckedChanged += new System.EventHandler(this.freqTypeRdo_CheckedChanged);
            // 
            // monRdo
            // 
            this.monRdo.AutoSize = true;
            this.monRdo.Checked = true;
            this.monRdo.Location = new System.Drawing.Point(6, 21);
            this.monRdo.Name = "monRdo";
            this.monRdo.Size = new System.Drawing.Size(63, 16);
            this.monRdo.TabIndex = 0;
            this.monRdo.TabStop = true;
            this.monRdo.Text = "Monthly";
            this.monRdo.UseVisualStyleBackColor = true;
            this.monRdo.CheckedChanged += new System.EventHandler(this.freqTypeRdo_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 244);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 2;
            this.label7.Text = "Start Time :";
            // 
            // stTimePick
            // 
            this.stTimePick.CustomFormat = "yyyy-MM-dd HH:mm:00";
            this.stTimePick.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.stTimePick.Location = new System.Drawing.Point(77, 237);
            this.stTimePick.Name = "stTimePick";
            this.stTimePick.Size = new System.Drawing.Size(153, 22);
            this.stTimePick.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 272);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(33, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "Desc :";
            // 
            // descTxt
            // 
            this.descTxt.Location = new System.Drawing.Point(77, 269);
            this.descTxt.MaxLength = 1000;
            this.descTxt.Multiline = true;
            this.descTxt.Name = "descTxt";
            this.descTxt.Size = new System.Drawing.Size(300, 112);
            this.descTxt.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(405, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(57, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "Parameter :";
            // 
            // paramTxt
            // 
            this.paramTxt.Location = new System.Drawing.Point(468, 6);
            this.paramTxt.MaxLength = 3500;
            this.paramTxt.Multiline = true;
            this.paramTxt.Name = "paramTxt";
            this.paramTxt.Size = new System.Drawing.Size(537, 197);
            this.paramTxt.TabIndex = 1;
            // 
            // saveBtn
            // 
            this.saveBtn.Location = new System.Drawing.Point(278, 421);
            this.saveBtn.Name = "saveBtn";
            this.saveBtn.Size = new System.Drawing.Size(75, 23);
            this.saveBtn.TabIndex = 4;
            this.saveBtn.Text = "Save";
            this.saveBtn.UseVisualStyleBackColor = true;
            this.saveBtn.Click += new System.EventHandler(this.saveBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.Location = new System.Drawing.Point(407, 421);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(75, 23);
            this.closeBtn.TabIndex = 4;
            this.closeBtn.Text = "Cancel";
            this.closeBtn.UseVisualStyleBackColor = true;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 400);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 12);
            this.label10.TabIndex = 2;
            this.label10.Text = "Created User :";
            // 
            // userIDTxt
            // 
            this.userIDTxt.Location = new System.Drawing.Point(89, 397);
            this.userIDTxt.Name = "userIDTxt";
            this.userIDTxt.Size = new System.Drawing.Size(106, 22);
            this.userIDTxt.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 36);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "Job Func :";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(391, 228);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(65, 12);
            this.label12.TabIndex = 2;
            this.label12.Text = "Notify Mail :";
            // 
            // mailTxt
            // 
            this.mailTxt.Location = new System.Drawing.Point(468, 228);
            this.mailTxt.MaxLength = 500;
            this.mailTxt.Multiline = true;
            this.mailTxt.Name = "mailTxt";
            this.mailTxt.Size = new System.Drawing.Size(537, 44);
            this.mailTxt.TabIndex = 1;
            // 
            // jobFuncCmb
            // 
            this.jobFuncCmb.FormattingEnabled = true;
            this.jobFuncCmb.Location = new System.Drawing.Point(77, 36);
            this.jobFuncCmb.Name = "jobFuncCmb";
            this.jobFuncCmb.Size = new System.Drawing.Size(300, 20);
            this.jobFuncCmb.TabIndex = 5;
            // 
            // AddJobForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1017, 456);
            this.Controls.Add(this.jobFuncCmb);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.saveBtn);
            this.Controls.Add(this.stTimePick);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.descTxt);
            this.Controls.Add(this.mailTxt);
            this.Controls.Add(this.paramTxt);
            this.Controls.Add(this.userIDTxt);
            this.Controls.Add(this.jobIDTxt);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Name = "AddJobForm";
            this.Text = "AddJobForm";
            this.Load += new System.EventHandler(this.AddJobForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.secNumDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minNumDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.hourNumDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox jobIDTxt;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton minRdo;
        private System.Windows.Forms.RadioButton hourRdo;
        private System.Windows.Forms.RadioButton dailyRdo;
        private System.Windows.Forms.RadioButton monRdo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox weekCmb;
        private System.Windows.Forms.ComboBox dayCmb;
        private System.Windows.Forms.RadioButton secRdo;
        private System.Windows.Forms.RadioButton weekRdo;
        private System.Windows.Forms.NumericUpDown minNumDown;
        private System.Windows.Forms.NumericUpDown hourNumDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown secNumDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker stTimePick;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox descTxt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox paramTxt;
        private System.Windows.Forms.Button saveBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox userIDTxt;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox mailTxt;
        private System.Windows.Forms.ComboBox jobFuncCmb;
    }
}