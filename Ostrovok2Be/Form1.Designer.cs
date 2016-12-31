namespace Ostrovok2Be
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.startbtn = new System.Windows.Forms.Button();
            this.idletime = new System.Windows.Forms.TextBox();
            this.IntervalCallTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.refreshbtn = new System.Windows.Forms.Button();
            this.countrylist = new System.Windows.Forms.CheckedListBox();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.process_lb = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rd_Check_Update = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.rdAuto = new System.Windows.Forms.RadioButton();
            this.btn_Pause = new System.Windows.Forms.Button();
            this.btn_continue = new System.Windows.Forms.Button();
            this.lb_Info = new System.Windows.Forms.Label();
            this.cb_En = new System.Windows.Forms.CheckBox();
            this.cb_Ru = new System.Windows.Forms.CheckBox();
            this.cb_De = new System.Windows.Forms.CheckBox();
            this.group_LangSelect = new System.Windows.Forms.GroupBox();
            this.btn_Exit = new System.Windows.Forms.Button();
            this.tb_listIds = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.gb_select = new System.Windows.Forms.GroupBox();
            this.dt_Fromdate = new System.Windows.Forms.DateTimePicker();
            this.dt_Todate = new System.Windows.Forms.DateTimePicker();
            this.gr_Currency = new System.Windows.Forms.GroupBox();
            this.cb_Usd = new System.Windows.Forms.CheckBox();
            this.cb_Vnd = new System.Windows.Forms.CheckBox();
            this.cb_Rub = new System.Windows.Forms.CheckBox();
            this.cb_Eur = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.group_LangSelect.SuspendLayout();
            this.gb_select.SuspendLayout();
            this.gr_Currency.SuspendLayout();
            this.SuspendLayout();
            // 
            // startbtn
            // 
            this.startbtn.Location = new System.Drawing.Point(726, 301);
            this.startbtn.Name = "startbtn";
            this.startbtn.Size = new System.Drawing.Size(75, 50);
            this.startbtn.TabIndex = 0;
            this.startbtn.Text = "Start";
            this.startbtn.UseVisualStyleBackColor = true;
            this.startbtn.Click += new System.EventHandler(this.startbtn_Click);
            // 
            // idletime
            // 
            this.idletime.Location = new System.Drawing.Point(15, 62);
            this.idletime.Name = "idletime";
            this.idletime.Size = new System.Drawing.Size(88, 20);
            this.idletime.TabIndex = 1;
            this.idletime.Text = "1000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "The time between 2 requests";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "By Country";
            // 
            // refreshbtn
            // 
            this.refreshbtn.Location = new System.Drawing.Point(726, 191);
            this.refreshbtn.Name = "refreshbtn";
            this.refreshbtn.Size = new System.Drawing.Size(75, 50);
            this.refreshbtn.TabIndex = 0;
            this.refreshbtn.Text = "Refresh";
            this.refreshbtn.UseVisualStyleBackColor = true;
            this.refreshbtn.Click += new System.EventHandler(this.refreshbtn_Click);
            // 
            // countrylist
            // 
            this.countrylist.CheckOnClick = true;
            this.countrylist.FormattingEnabled = true;
            this.countrylist.Location = new System.Drawing.Point(28, 37);
            this.countrylist.Name = "countrylist";
            this.countrylist.Size = new System.Drawing.Size(120, 154);
            this.countrylist.TabIndex = 5;
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(15, 420);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(322, 23);
            this.pBar.TabIndex = 6;
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_ProgressChanged);
            // 
            // process_lb
            // 
            this.process_lb.AutoSize = true;
            this.process_lb.Location = new System.Drawing.Point(14, 401);
            this.process_lb.Name = "process_lb";
            this.process_lb.Size = new System.Drawing.Size(71, 13);
            this.process_lb.TabIndex = 7;
            this.process_lb.Text = "Main Process";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rd_Check_Update);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.rdAuto);
            this.groupBox1.Location = new System.Drawing.Point(194, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 125);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Run mode";
            // 
            // rd_Check_Update
            // 
            this.rd_Check_Update.AutoSize = true;
            this.rd_Check_Update.Location = new System.Drawing.Point(39, 80);
            this.rd_Check_Update.Name = "rd_Check_Update";
            this.rd_Check_Update.Size = new System.Drawing.Size(87, 17);
            this.rd_Check_Update.TabIndex = 3;
            this.rd_Check_Update.Text = "Update Price";
            this.rd_Check_Update.UseVisualStyleBackColor = true;
            this.rd_Check_Update.MouseLeave += new System.EventHandler(this.rd_Check_Update_MouseLeave);
            this.rd_Check_Update.MouseHover += new System.EventHandler(this.rd_Check_Update_MouseHover);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(39, 51);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(70, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Contitnue";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.MouseLeave += new System.EventHandler(this.radioButton2_MouseLeave);
            this.radioButton2.MouseHover += new System.EventHandler(this.radioButton2_MouseHover);
            // 
            // rdAuto
            // 
            this.rdAuto.AutoSize = true;
            this.rdAuto.Checked = true;
            this.rdAuto.Location = new System.Drawing.Point(39, 25);
            this.rdAuto.Name = "rdAuto";
            this.rdAuto.Size = new System.Drawing.Size(47, 17);
            this.rdAuto.TabIndex = 0;
            this.rdAuto.TabStop = true;
            this.rdAuto.Text = "Auto";
            this.rdAuto.UseVisualStyleBackColor = true;
            this.rdAuto.MouseLeave += new System.EventHandler(this.rdAuto_MouseLeave);
            this.rdAuto.MouseHover += new System.EventHandler(this.rdAuto_MouseHover);
            // 
            // btn_Pause
            // 
            this.btn_Pause.Location = new System.Drawing.Point(726, 128);
            this.btn_Pause.Name = "btn_Pause";
            this.btn_Pause.Size = new System.Drawing.Size(75, 57);
            this.btn_Pause.TabIndex = 9;
            this.btn_Pause.Text = "Pause";
            this.btn_Pause.UseVisualStyleBackColor = true;
            this.btn_Pause.Click += new System.EventHandler(this.btn_Pause_Click);
            this.btn_Pause.MouseLeave += new System.EventHandler(this.btn_Pause_MouseLeave);
            this.btn_Pause.MouseHover += new System.EventHandler(this.btn_Pause_MouseHover);
            // 
            // btn_continue
            // 
            this.btn_continue.Location = new System.Drawing.Point(726, 65);
            this.btn_continue.Name = "btn_continue";
            this.btn_continue.Size = new System.Drawing.Size(75, 57);
            this.btn_continue.TabIndex = 9;
            this.btn_continue.Text = "Continue";
            this.btn_continue.UseVisualStyleBackColor = true;
            this.btn_continue.Click += new System.EventHandler(this.btn_continue_Click);
            this.btn_continue.MouseLeave += new System.EventHandler(this.btn_continue_MouseLeave);
            this.btn_continue.MouseHover += new System.EventHandler(this.btn_continue_MouseHover);
            // 
            // lb_Info
            // 
            this.lb_Info.AutoSize = true;
            this.lb_Info.Location = new System.Drawing.Point(452, 13);
            this.lb_Info.Name = "lb_Info";
            this.lb_Info.Size = new System.Drawing.Size(31, 13);
            this.lb_Info.TabIndex = 10;
            this.lb_Info.Text = "Infor:";
            // 
            // cb_En
            // 
            this.cb_En.AutoSize = true;
            this.cb_En.Checked = true;
            this.cb_En.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_En.Location = new System.Drawing.Point(19, 19);
            this.cb_En.Name = "cb_En";
            this.cb_En.Size = new System.Drawing.Size(39, 17);
            this.cb_En.TabIndex = 0;
            this.cb_En.Text = "En";
            this.cb_En.UseVisualStyleBackColor = true;
            // 
            // cb_Ru
            // 
            this.cb_Ru.AutoSize = true;
            this.cb_Ru.Location = new System.Drawing.Point(19, 42);
            this.cb_Ru.Name = "cb_Ru";
            this.cb_Ru.Size = new System.Drawing.Size(40, 17);
            this.cb_Ru.TabIndex = 0;
            this.cb_Ru.Text = "Ru";
            this.cb_Ru.UseVisualStyleBackColor = true;
            // 
            // cb_De
            // 
            this.cb_De.AutoSize = true;
            this.cb_De.Location = new System.Drawing.Point(19, 65);
            this.cb_De.Name = "cb_De";
            this.cb_De.Size = new System.Drawing.Size(40, 17);
            this.cb_De.TabIndex = 0;
            this.cb_De.Text = "De";
            this.cb_De.UseVisualStyleBackColor = true;
            // 
            // group_LangSelect
            // 
            this.group_LangSelect.Controls.Add(this.cb_De);
            this.group_LangSelect.Controls.Add(this.cb_Ru);
            this.group_LangSelect.Controls.Add(this.cb_En);
            this.group_LangSelect.Location = new System.Drawing.Point(389, 50);
            this.group_LangSelect.Name = "group_LangSelect";
            this.group_LangSelect.Size = new System.Drawing.Size(109, 100);
            this.group_LangSelect.TabIndex = 11;
            this.group_LangSelect.TabStop = false;
            this.group_LangSelect.Text = "Select Language";
            // 
            // btn_Exit
            // 
            this.btn_Exit.Location = new System.Drawing.Point(726, 357);
            this.btn_Exit.Name = "btn_Exit";
            this.btn_Exit.Size = new System.Drawing.Size(75, 50);
            this.btn_Exit.TabIndex = 0;
            this.btn_Exit.Text = "Exit";
            this.btn_Exit.UseVisualStyleBackColor = true;
            this.btn_Exit.Click += new System.EventHandler(this.ExitProgram);
            // 
            // tb_listIds
            // 
            this.tb_listIds.Location = new System.Drawing.Point(169, 35);
            this.tb_listIds.Multiline = true;
            this.tb_listIds.Name = "tb_listIds";
            this.tb_listIds.Size = new System.Drawing.Size(421, 169);
            this.tb_listIds.TabIndex = 12;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(166, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "By List Region Ids ( , ) ";
            // 
            // gb_select
            // 
            this.gb_select.Controls.Add(this.countrylist);
            this.gb_select.Controls.Add(this.label2);
            this.gb_select.Controls.Add(this.label3);
            this.gb_select.Controls.Add(this.tb_listIds);
            this.gb_select.Location = new System.Drawing.Point(15, 165);
            this.gb_select.Name = "gb_select";
            this.gb_select.Size = new System.Drawing.Size(696, 220);
            this.gb_select.TabIndex = 14;
            this.gb_select.TabStop = false;
            this.gb_select.Text = "Select Box";
            // 
            // dt_Fromdate
            // 
            this.dt_Fromdate.Location = new System.Drawing.Point(633, 62);
            this.dt_Fromdate.Name = "dt_Fromdate";
            this.dt_Fromdate.Size = new System.Drawing.Size(179, 20);
            this.dt_Fromdate.TabIndex = 15;
            this.dt_Fromdate.ValueChanged += new System.EventHandler(this.dt_Fromdate_ValueChanged);
            // 
            // dt_Todate
            // 
            this.dt_Todate.Location = new System.Drawing.Point(633, 102);
            this.dt_Todate.Name = "dt_Todate";
            this.dt_Todate.Size = new System.Drawing.Size(179, 20);
            this.dt_Todate.TabIndex = 15;
            // 
            // gr_Currency
            // 
            this.gr_Currency.Controls.Add(this.cb_Eur);
            this.gr_Currency.Controls.Add(this.cb_Rub);
            this.gr_Currency.Controls.Add(this.cb_Vnd);
            this.gr_Currency.Controls.Add(this.cb_Usd);
            this.gr_Currency.Location = new System.Drawing.Point(504, 50);
            this.gr_Currency.Name = "gr_Currency";
            this.gr_Currency.Size = new System.Drawing.Size(123, 109);
            this.gr_Currency.TabIndex = 16;
            this.gr_Currency.TabStop = false;
            this.gr_Currency.Text = "Currency";
            // 
            // cb_Usd
            // 
            this.cb_Usd.AutoSize = true;
            this.cb_Usd.Checked = true;
            this.cb_Usd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Usd.Location = new System.Drawing.Point(7, 20);
            this.cb_Usd.Name = "cb_Usd";
            this.cb_Usd.Size = new System.Drawing.Size(49, 17);
            this.cb_Usd.TabIndex = 0;
            this.cb_Usd.Text = "USD";
            this.cb_Usd.UseVisualStyleBackColor = true;
            // 
            // cb_Vnd
            // 
            this.cb_Vnd.AutoSize = true;
            this.cb_Vnd.Checked = true;
            this.cb_Vnd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Vnd.Location = new System.Drawing.Point(7, 42);
            this.cb_Vnd.Name = "cb_Vnd";
            this.cb_Vnd.Size = new System.Drawing.Size(49, 17);
            this.cb_Vnd.TabIndex = 0;
            this.cb_Vnd.Text = "VND";
            this.cb_Vnd.UseVisualStyleBackColor = true;
            // 
            // cb_Rub
            // 
            this.cb_Rub.AutoSize = true;
            this.cb_Rub.Checked = true;
            this.cb_Rub.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Rub.Location = new System.Drawing.Point(6, 65);
            this.cb_Rub.Name = "cb_Rub";
            this.cb_Rub.Size = new System.Drawing.Size(49, 17);
            this.cb_Rub.TabIndex = 0;
            this.cb_Rub.Text = "RUB";
            this.cb_Rub.UseVisualStyleBackColor = true;
            // 
            // cb_Eur
            // 
            this.cb_Eur.AutoSize = true;
            this.cb_Eur.Checked = true;
            this.cb_Eur.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cb_Eur.Location = new System.Drawing.Point(6, 83);
            this.cb_Eur.Name = "cb_Eur";
            this.cb_Eur.Size = new System.Drawing.Size(49, 17);
            this.cb_Eur.TabIndex = 0;
            this.cb_Eur.Text = "EUR";
            this.cb_Eur.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 456);
            this.Controls.Add(this.gr_Currency);
            this.Controls.Add(this.dt_Todate);
            this.Controls.Add(this.dt_Fromdate);
            this.Controls.Add(this.gb_select);
            this.Controls.Add(this.group_LangSelect);
            this.Controls.Add(this.lb_Info);
            this.Controls.Add(this.btn_continue);
            this.Controls.Add(this.btn_Pause);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.process_lb);
            this.Controls.Add(this.pBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idletime);
            this.Controls.Add(this.refreshbtn);
            this.Controls.Add(this.btn_Exit);
            this.Controls.Add(this.startbtn);
            this.Name = "Form1";
            this.Text = "                                        ";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.group_LangSelect.ResumeLayout(false);
            this.group_LangSelect.PerformLayout();
            this.gb_select.ResumeLayout(false);
            this.gb_select.PerformLayout();
            this.gr_Currency.ResumeLayout(false);
            this.gr_Currency.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startbtn;
        private System.Windows.Forms.TextBox idletime;
        private System.Windows.Forms.Timer IntervalCallTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button refreshbtn;
        private System.Windows.Forms.CheckedListBox countrylist;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label process_lb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton rdAuto;
        private System.Windows.Forms.RadioButton rd_Check_Update;
        private System.Windows.Forms.Button btn_Pause;
        private System.Windows.Forms.Button btn_continue;
        private System.Windows.Forms.Label lb_Info;
        private System.Windows.Forms.CheckBox cb_En;
        private System.Windows.Forms.CheckBox cb_Ru;
        private System.Windows.Forms.CheckBox cb_De;
        private System.Windows.Forms.GroupBox group_LangSelect;
        private System.Windows.Forms.Button btn_Exit;
        private System.Windows.Forms.TextBox tb_listIds;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox gb_select;

        private System.Windows.Forms.DateTimePicker dt_Fromdate;
        private System.Windows.Forms.DateTimePicker dt_Todate;
        private System.Windows.Forms.GroupBox gr_Currency;
        private System.Windows.Forms.CheckBox cb_Eur;
        private System.Windows.Forms.CheckBox cb_Rub;
        private System.Windows.Forms.CheckBox cb_Vnd;
        private System.Windows.Forms.CheckBox cb_Usd;

        internal System.ComponentModel.BackgroundWorker bgWorker;

    }
}

