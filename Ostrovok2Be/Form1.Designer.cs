﻿namespace Ostrovok2Be
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
            this.rdAuto = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.btn_Pause = new System.Windows.Forms.Button();
            this.rd_Check = new System.Windows.Forms.RadioButton();
            this.rd_Check_Update = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startbtn
            // 
            this.startbtn.Location = new System.Drawing.Point(406, 334);
            this.startbtn.Name = "startbtn";
            this.startbtn.Size = new System.Drawing.Size(75, 50);
            this.startbtn.TabIndex = 0;
            this.startbtn.Text = "Start";
            this.startbtn.UseVisualStyleBackColor = true;
            this.startbtn.Click += new System.EventHandler(this.startbtn_Click);
            // 
            // idletime
            // 
            this.idletime.Location = new System.Drawing.Point(39, 57);
            this.idletime.Name = "idletime";
            this.idletime.Size = new System.Drawing.Size(88, 20);
            this.idletime.TabIndex = 1;
            this.idletime.Text = "1000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(144, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "The time between 2 requests";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "List Country";
            // 
            // refreshbtn
            // 
            this.refreshbtn.Location = new System.Drawing.Point(406, 261);
            this.refreshbtn.Name = "refreshbtn";
            this.refreshbtn.Size = new System.Drawing.Size(75, 50);
            this.refreshbtn.TabIndex = 0;
            this.refreshbtn.Text = "Refresh";
            this.refreshbtn.UseVisualStyleBackColor = true;
            this.refreshbtn.Click += new System.EventHandler(this.refreshbtn_Click);
            // 
            // countrylist
            // 
            this.countrylist.FormattingEnabled = true;
            this.countrylist.Location = new System.Drawing.Point(39, 124);
            this.countrylist.Name = "countrylist";
            this.countrylist.Size = new System.Drawing.Size(120, 94);
            this.countrylist.TabIndex = 5;
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(27, 350);
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
            this.process_lb.Location = new System.Drawing.Point(27, 334);
            this.process_lb.Name = "process_lb";
            this.process_lb.Size = new System.Drawing.Size(71, 13);
            this.process_lb.TabIndex = 7;
            this.process_lb.Text = "Main Process";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rd_Check_Update);
            this.groupBox1.Controls.Add(this.rd_Check);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.rdAuto);
            this.groupBox1.Location = new System.Drawing.Point(183, 118);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(166, 160);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Run mode";
            // 
            // rdAuto
            // 
            this.rdAuto.AutoSize = true;
            this.rdAuto.Location = new System.Drawing.Point(39, 25);
            this.rdAuto.Name = "rdAuto";
            this.rdAuto.Size = new System.Drawing.Size(47, 17);
            this.rdAuto.TabIndex = 0;
            this.rdAuto.TabStop = true;
            this.rdAuto.Text = "Auto";
            this.rdAuto.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(39, 57);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(70, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Contitnue";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // btn_Pause
            // 
            this.btn_Pause.Location = new System.Drawing.Point(406, 191);
            this.btn_Pause.Name = "btn_Pause";
            this.btn_Pause.Size = new System.Drawing.Size(75, 57);
            this.btn_Pause.TabIndex = 9;
            this.btn_Pause.Text = "Pause";
            this.btn_Pause.UseVisualStyleBackColor = true;
            // 
            // rd_Check
            // 
            this.rd_Check.AutoSize = true;
            this.rd_Check.Location = new System.Drawing.Point(39, 95);
            this.rd_Check.Name = "rd_Check";
            this.rd_Check.Size = new System.Drawing.Size(56, 17);
            this.rd_Check.TabIndex = 2;
            this.rd_Check.TabStop = true;
            this.rd_Check.Text = "Check";
            this.rd_Check.UseVisualStyleBackColor = true;
            // 
            // rd_Check_Update
            // 
            this.rd_Check_Update.AutoSize = true;
            this.rd_Check_Update.Location = new System.Drawing.Point(39, 131);
            this.rd_Check_Update.Name = "rd_Check_Update";
            this.rd_Check_Update.Size = new System.Drawing.Size(94, 17);
            this.rd_Check_Update.TabIndex = 3;
            this.rd_Check_Update.TabStop = true;
            this.rd_Check_Update.Text = "Check &Update";
            this.rd_Check_Update.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 408);
            this.Controls.Add(this.btn_Pause);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.process_lb);
            this.Controls.Add(this.pBar);
            this.Controls.Add(this.countrylist);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idletime);
            this.Controls.Add(this.refreshbtn);
            this.Controls.Add(this.startbtn);
            this.Name = "Form1";
            this.Text = "Os2Be";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        internal System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.Label process_lb;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton rdAuto;
        private System.Windows.Forms.RadioButton rd_Check_Update;
        private System.Windows.Forms.RadioButton rd_Check;
        private System.Windows.Forms.Button btn_Pause;
    }
}
