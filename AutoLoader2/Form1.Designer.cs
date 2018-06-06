namespace AutoLoader2
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.addBtn = new System.Windows.Forms.Button();
            this.jobGrid = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.paramCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.thrCntTxt = new System.Windows.Forms.TextBox();
            this.enbBtn = new System.Windows.Forms.Button();
            this.disabledBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.editJobBtn = new System.Windows.Forms.Button();
            this.delJobBtn = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.autoRunStop = new System.Windows.Forms.Button();
            this.runOnceBtn = new System.Windows.Forms.Button();
            this.schRunBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.schStateLab = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.runCntTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.enabTxt = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.jobGrid)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // addBtn
            // 
            this.addBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.addBtn.Location = new System.Drawing.Point(6, 23);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(97, 23);
            this.addBtn.TabIndex = 0;
            this.addBtn.Text = "Add New Job";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // jobGrid
            // 
            this.jobGrid.AllowUserToAddRows = false;
            this.jobGrid.AllowUserToDeleteRows = false;
            this.jobGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.jobGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column1,
            this.Column14,
            this.Column17,
            this.Column16,
            this.Column2,
            this.Column19,
            this.Column12,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column20,
            this.Column13,
            this.paramCol,
            this.Column11,
            this.Column15,
            this.Column8,
            this.Column9,
            this.Column7,
            this.Column10,
            this.Column18});
            this.jobGrid.Location = new System.Drawing.Point(12, 158);
            this.jobGrid.Name = "jobGrid";
            this.jobGrid.ReadOnly = true;
            this.jobGrid.RowTemplate.Height = 24;
            this.jobGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.jobGrid.Size = new System.Drawing.Size(966, 334);
            this.jobGrid.TabIndex = 1;
            this.jobGrid.DoubleClick += new System.EventHandler(this.jobGrid_DoubleClick);
            // 
            // Column6
            // 
            this.Column6.DataPropertyName = "ENABLED";
            this.Column6.HeaderText = "Enabled";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Column6.TrueValue = "1";
            this.Column6.Width = 50;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "JOB_ID";
            this.Column1.HeaderText = "Job ID";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 300;
            // 
            // Column14
            // 
            this.Column14.DataPropertyName = "JOB_FUNC";
            this.Column14.HeaderText = "Job Func";
            this.Column14.Name = "Column14";
            this.Column14.ReadOnly = true;
            this.Column14.Width = 300;
            // 
            // Column17
            // 
            this.Column17.DataPropertyName = "JOB_HOST";
            this.Column17.HeaderText = "Job Host";
            this.Column17.Name = "Column17";
            this.Column17.ReadOnly = true;
            // 
            // Column16
            // 
            this.Column16.DataPropertyName = "JOB_AUTO_RUN_ID";
            this.Column16.HeaderText = "Auto Run ID";
            this.Column16.Name = "Column16";
            this.Column16.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.DataPropertyName = "STATUS";
            this.Column2.HeaderText = "Status";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column19
            // 
            this.Column19.DataPropertyName = "UPDATE_TIME";
            dataGridViewCellStyle1.Format = "yyyy-MM-dd HH:mm:ss";
            this.Column19.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column19.HeaderText = "Update Time";
            this.Column19.Name = "Column19";
            this.Column19.ReadOnly = true;
            this.Column19.Width = 150;
            // 
            // Column12
            // 
            this.Column12.DataPropertyName = "FREQ_TYPE";
            this.Column12.HeaderText = "Freq Type";
            this.Column12.Name = "Column12";
            this.Column12.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.DataPropertyName = "FREQ";
            this.Column3.HeaderText = "Freq";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column4
            // 
            this.Column4.DataPropertyName = "START_TIME";
            dataGridViewCellStyle2.Format = "yyyy-MM-dd HH:mm:ss";
            this.Column4.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column4.HeaderText = "Start Time";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 150;
            // 
            // Column5
            // 
            this.Column5.DataPropertyName = "LAST_RUN_TIME";
            dataGridViewCellStyle3.Format = "yyyy-MM-dd HH:mm:ss";
            this.Column5.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column5.HeaderText = "Last Run Time";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 150;
            // 
            // Column20
            // 
            this.Column20.DataPropertyName = "NEXT_RUN_TIME";
            dataGridViewCellStyle4.Format = "yyyy-MM-dd HH:mm:ss";
            this.Column20.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column20.HeaderText = "Next Run Time";
            this.Column20.Name = "Column20";
            this.Column20.ReadOnly = true;
            this.Column20.Width = 150;
            // 
            // Column13
            // 
            this.Column13.DataPropertyName = "EXEC_TIME";
            this.Column13.HeaderText = "Exec Time";
            this.Column13.Name = "Column13";
            this.Column13.ReadOnly = true;
            // 
            // paramCol
            // 
            this.paramCol.DataPropertyName = "PARAM";
            this.paramCol.HeaderText = "Paramter";
            this.paramCol.Name = "paramCol";
            this.paramCol.ReadOnly = true;
            this.paramCol.Width = 500;
            // 
            // Column11
            // 
            this.Column11.DataPropertyName = "DESCRIPTION";
            this.Column11.HeaderText = "Desc";
            this.Column11.Name = "Column11";
            this.Column11.ReadOnly = true;
            // 
            // Column15
            // 
            this.Column15.DataPropertyName = "MSG";
            this.Column15.HeaderText = "Msg";
            this.Column15.Name = "Column15";
            this.Column15.ReadOnly = true;
            // 
            // Column8
            // 
            this.Column8.DataPropertyName = "CREATED_TIME";
            dataGridViewCellStyle5.Format = "yyyy-MM-dd HH:mm:ss";
            this.Column8.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column8.HeaderText = "Created Time";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 150;
            // 
            // Column9
            // 
            this.Column9.DataPropertyName = "CREATED_USER";
            this.Column9.HeaderText = "Created User";
            this.Column9.Name = "Column9";
            this.Column9.ReadOnly = true;
            // 
            // Column7
            // 
            this.Column7.DataPropertyName = "MODIFIED_TIME";
            dataGridViewCellStyle6.Format = "yyyy-MM-dd HH:mm:ss";
            this.Column7.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column7.HeaderText = "Modified Time";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 150;
            // 
            // Column10
            // 
            this.Column10.DataPropertyName = "MODIFIED_USER";
            this.Column10.HeaderText = "Modified User";
            this.Column10.Name = "Column10";
            this.Column10.ReadOnly = true;
            // 
            // Column18
            // 
            this.Column18.DataPropertyName = "NOTIFY_MAIL";
            this.Column18.HeaderText = "Mail";
            this.Column18.Name = "Column18";
            this.Column18.ReadOnly = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(518, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "Thread Count :";
            // 
            // thrCntTxt
            // 
            this.thrCntTxt.Location = new System.Drawing.Point(600, 4);
            this.thrCntTxt.Name = "thrCntTxt";
            this.thrCntTxt.ReadOnly = true;
            this.thrCntTxt.Size = new System.Drawing.Size(50, 22);
            this.thrCntTxt.TabIndex = 3;
            this.thrCntTxt.Text = "5";
            // 
            // enbBtn
            // 
            this.enbBtn.BackColor = System.Drawing.Color.LimeGreen;
            this.enbBtn.Location = new System.Drawing.Point(6, 16);
            this.enbBtn.Name = "enbBtn";
            this.enbBtn.Size = new System.Drawing.Size(75, 23);
            this.enbBtn.TabIndex = 0;
            this.enbBtn.Text = "Enabled Job";
            this.enbBtn.UseVisualStyleBackColor = false;
            this.enbBtn.Click += new System.EventHandler(this.enbBtn_Click);
            // 
            // disabledBtn
            // 
            this.disabledBtn.BackColor = System.Drawing.Color.Red;
            this.disabledBtn.Location = new System.Drawing.Point(144, 16);
            this.disabledBtn.Name = "disabledBtn";
            this.disabledBtn.Size = new System.Drawing.Size(75, 23);
            this.disabledBtn.TabIndex = 0;
            this.disabledBtn.Text = "Disabled Job";
            this.disabledBtn.UseVisualStyleBackColor = false;
            this.disabledBtn.Click += new System.EventHandler(this.disabledBtn_Click);
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.Red;
            this.closeBtn.Location = new System.Drawing.Point(869, 125);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(109, 23);
            this.closeBtn.TabIndex = 4;
            this.closeBtn.Text = "[X] Close Form";
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            // 
            // editJobBtn
            // 
            this.editJobBtn.BackColor = System.Drawing.Color.Yellow;
            this.editJobBtn.Location = new System.Drawing.Point(121, 23);
            this.editJobBtn.Name = "editJobBtn";
            this.editJobBtn.Size = new System.Drawing.Size(75, 23);
            this.editJobBtn.TabIndex = 5;
            this.editJobBtn.Text = "Edit Job";
            this.editJobBtn.UseVisualStyleBackColor = false;
            this.editJobBtn.Click += new System.EventHandler(this.editJobBtn_Click);
            // 
            // delJobBtn
            // 
            this.delJobBtn.BackColor = System.Drawing.Color.Red;
            this.delJobBtn.Location = new System.Drawing.Point(220, 23);
            this.delJobBtn.Name = "delJobBtn";
            this.delJobBtn.Size = new System.Drawing.Size(75, 23);
            this.delJobBtn.TabIndex = 6;
            this.delJobBtn.Text = "Delete Job";
            this.delJobBtn.UseVisualStyleBackColor = false;
            this.delJobBtn.Click += new System.EventHandler(this.delJobBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 52);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(322, 100);
            this.tabControl1.TabIndex = 7;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.enbBtn);
            this.tabPage1.Controls.Add(this.disabledBtn);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(314, 74);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Enable Job";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.addBtn);
            this.tabPage2.Controls.Add(this.delJobBtn);
            this.tabPage2.Controls.Add(this.editJobBtn);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(314, 74);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Add Job";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.autoRunStop);
            this.tabPage3.Controls.Add(this.runOnceBtn);
            this.tabPage3.Controls.Add(this.schRunBtn);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(314, 74);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Auto Run Control";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // autoRunStop
            // 
            this.autoRunStop.BackColor = System.Drawing.Color.Red;
            this.autoRunStop.Location = new System.Drawing.Point(166, 12);
            this.autoRunStop.Name = "autoRunStop";
            this.autoRunStop.Size = new System.Drawing.Size(90, 23);
            this.autoRunStop.TabIndex = 1;
            this.autoRunStop.Text = "Auto Run Stop";
            this.autoRunStop.UseVisualStyleBackColor = false;
            this.autoRunStop.Click += new System.EventHandler(this.autoRunStop_Click);
            // 
            // runOnceBtn
            // 
            this.runOnceBtn.BackColor = System.Drawing.Color.Yellow;
            this.runOnceBtn.Location = new System.Drawing.Point(12, 48);
            this.runOnceBtn.Name = "runOnceBtn";
            this.runOnceBtn.Size = new System.Drawing.Size(90, 23);
            this.runOnceBtn.TabIndex = 0;
            this.runOnceBtn.Text = "Run Once Job";
            this.runOnceBtn.UseVisualStyleBackColor = false;
            this.runOnceBtn.Click += new System.EventHandler(this.runOnceBtn_Click);
            // 
            // schRunBtn
            // 
            this.schRunBtn.BackColor = System.Drawing.Color.LimeGreen;
            this.schRunBtn.Location = new System.Drawing.Point(12, 12);
            this.schRunBtn.Name = "schRunBtn";
            this.schRunBtn.Size = new System.Drawing.Size(90, 23);
            this.schRunBtn.TabIndex = 0;
            this.schRunBtn.Text = "Auto Run Start";
            this.schRunBtn.UseVisualStyleBackColor = false;
            this.schRunBtn.Click += new System.EventHandler(this.schRunBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "Auto Run Status :";
            // 
            // schStateLab
            // 
            this.schStateLab.AutoSize = true;
            this.schStateLab.BackColor = System.Drawing.Color.Red;
            this.schStateLab.Font = new System.Drawing.Font("新細明體", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.schStateLab.Location = new System.Drawing.Point(118, 6);
            this.schStateLab.Name = "schStateLab";
            this.schStateLab.Size = new System.Drawing.Size(42, 19);
            this.schStateLab.TabIndex = 9;
            this.schStateLab.Text = "Stop";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(350, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Running Count :";
            // 
            // runCntTxt
            // 
            this.runCntTxt.Location = new System.Drawing.Point(440, 4);
            this.runCntTxt.Name = "runCntTxt";
            this.runCntTxt.ReadOnly = true;
            this.runCntTxt.Size = new System.Drawing.Size(50, 22);
            this.runCntTxt.TabIndex = 3;
            this.runCntTxt.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(190, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "Enabled Count :";
            // 
            // enabTxt
            // 
            this.enabTxt.Location = new System.Drawing.Point(280, 3);
            this.enabTxt.Name = "enabTxt";
            this.enabTxt.ReadOnly = true;
            this.enabTxt.Size = new System.Drawing.Size(50, 22);
            this.enabTxt.TabIndex = 3;
            this.enabTxt.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(990, 504);
            this.Controls.Add(this.schStateLab);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.enabTxt);
            this.Controls.Add(this.runCntTxt);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.thrCntTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.jobGrid);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.jobGrid)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.DataGridView jobGrid;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox thrCntTxt;
        private System.Windows.Forms.Button enbBtn;
        private System.Windows.Forms.Button disabledBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button editJobBtn;
        private System.Windows.Forms.Button delJobBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button autoRunStop;
        private System.Windows.Forms.Button schRunBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label schStateLab;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox runCntTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox enabTxt;
        private System.Windows.Forms.Button runOnceBtn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column14;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column17;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column16;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column19;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column12;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column20;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column13;
        private System.Windows.Forms.DataGridViewTextBoxColumn paramCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column15;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column9;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column10;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column18;
    }
}

