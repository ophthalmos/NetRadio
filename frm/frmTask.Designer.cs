namespace NetRadio
{
    partial class FrmTask
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
            components = new System.ComponentModel.Container();
            gbAction = new System.Windows.Forms.GroupBox();
            rbHibernate = new System.Windows.Forms.RadioButton();
            rbShutdown = new System.Windows.Forms.RadioButton();
            rbSleep = new System.Windows.Forms.RadioButton();
            rbRecStop = new System.Windows.Forms.RadioButton();
            rbRecord = new System.Windows.Forms.RadioButton();
            rbStop = new System.Windows.Forms.RadioButton();
            rbPlay = new System.Windows.Forms.RadioButton();
            gbTime = new System.Windows.Forms.GroupBox();
            nudMM = new NumericUpDown00();
            lblHour = new System.Windows.Forms.Label();
            nudHH = new NumericUpDown00();
            gbStation = new System.Windows.Forms.GroupBox();
            cmBxStations = new System.Windows.Forms.ComboBox();
            btnOK = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            gbAction.SuspendLayout();
            gbTime.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudMM).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudHH).BeginInit();
            gbStation.SuspendLayout();
            SuspendLayout();
            // 
            // gbAction
            // 
            gbAction.Controls.Add(rbHibernate);
            gbAction.Controls.Add(rbShutdown);
            gbAction.Controls.Add(rbSleep);
            gbAction.Controls.Add(rbRecStop);
            gbAction.Controls.Add(rbRecord);
            gbAction.Controls.Add(rbStop);
            gbAction.Controls.Add(rbPlay);
            gbAction.Location = new System.Drawing.Point(12, 12);
            gbAction.Name = "gbAction";
            gbAction.Size = new System.Drawing.Size(146, 228);
            gbAction.TabIndex = 0;
            gbAction.TabStop = false;
            gbAction.Text = "Action";
            // 
            // rbHibernate
            // 
            rbHibernate.AutoSize = true;
            rbHibernate.Location = new System.Drawing.Point(6, 169);
            rbHibernate.Name = "rbHibernate";
            rbHibernate.Size = new System.Drawing.Size(108, 23);
            rbHibernate.TabIndex = 6;
            rbHibernate.TabStop = true;
            rbHibernate.Text = "Hibernate PC";
            rbHibernate.UseVisualStyleBackColor = true;
            rbHibernate.CheckedChanged += RbHibernate_CheckedChanged;
            // 
            // rbShutdown
            // 
            rbShutdown.AutoSize = true;
            rbShutdown.Location = new System.Drawing.Point(6, 198);
            rbShutdown.Name = "rbShutdown";
            rbShutdown.Size = new System.Drawing.Size(114, 23);
            rbShutdown.TabIndex = 7;
            rbShutdown.TabStop = true;
            rbShutdown.Text = "Shut down PC";
            rbShutdown.UseVisualStyleBackColor = true;
            rbShutdown.CheckedChanged += RbShutdown_CheckedChanged;
            // 
            // rbSleep
            // 
            rbSleep.AutoSize = true;
            rbSleep.Location = new System.Drawing.Point(6, 140);
            rbSleep.Name = "rbSleep";
            rbSleep.Size = new System.Drawing.Size(99, 23);
            rbSleep.TabIndex = 5;
            rbSleep.TabStop = true;
            rbSleep.Text = "Sleep Mode";
            rbSleep.UseVisualStyleBackColor = true;
            rbSleep.CheckedChanged += RbSleep_CheckedChanged;
            // 
            // rbRecStop
            // 
            rbRecStop.AutoSize = true;
            rbRecStop.Location = new System.Drawing.Point(6, 111);
            rbRecStop.Name = "rbRecStop";
            rbRecStop.Size = new System.Drawing.Size(117, 23);
            rbRecStop.TabIndex = 4;
            rbRecStop.TabStop = true;
            rbRecStop.Text = "Stop recording";
            rbRecStop.UseVisualStyleBackColor = true;
            rbRecStop.CheckedChanged += RbRecStop_CheckedChanged;
            // 
            // rbRecord
            // 
            rbRecord.AutoSize = true;
            rbRecord.Location = new System.Drawing.Point(6, 82);
            rbRecord.Name = "rbRecord";
            rbRecord.Size = new System.Drawing.Size(118, 23);
            rbRecord.TabIndex = 3;
            rbRecord.TabStop = true;
            rbRecord.Text = "Start recording";
            rbRecord.UseVisualStyleBackColor = true;
            rbRecord.CheckedChanged += RbRecord_CheckedChanged;
            // 
            // rbStop
            // 
            rbStop.AutoSize = true;
            rbStop.Location = new System.Drawing.Point(6, 53);
            rbStop.Name = "rbStop";
            rbStop.Size = new System.Drawing.Size(103, 23);
            rbStop.TabIndex = 2;
            rbStop.TabStop = true;
            rbStop.Text = "Stop playing";
            rbStop.UseVisualStyleBackColor = true;
            rbStop.CheckedChanged += RbStop_CheckedChanged;
            // 
            // rbPlay
            // 
            rbPlay.AutoSize = true;
            rbPlay.Checked = true;
            rbPlay.Location = new System.Drawing.Point(6, 24);
            rbPlay.Name = "rbPlay";
            rbPlay.Size = new System.Drawing.Size(104, 23);
            rbPlay.TabIndex = 1;
            rbPlay.TabStop = true;
            rbPlay.Text = "Start playing";
            rbPlay.UseVisualStyleBackColor = true;
            rbPlay.CheckedChanged += RbPlay_CheckedChanged;
            // 
            // gbTime
            // 
            gbTime.Controls.Add(nudMM);
            gbTime.Controls.Add(lblHour);
            gbTime.Controls.Add(nudHH);
            gbTime.Location = new System.Drawing.Point(12, 310);
            gbTime.Name = "gbTime";
            gbTime.Size = new System.Drawing.Size(146, 59);
            gbTime.TabIndex = 14;
            gbTime.TabStop = false;
            gbTime.Text = "Time";
            // 
            // nudMM
            // 
            nudMM.Location = new System.Drawing.Point(76, 24);
            nudMM.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            nudMM.Name = "nudMM";
            nudMM.Size = new System.Drawing.Size(52, 25);
            nudMM.TabIndex = 10;
            nudMM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nudMM.Enter += NudMM_Enter;
            nudMM.KeyDown += NudMM_KeyDown;
            nudMM.Leave += NudMM_Leave;
            nudMM.MouseClick += NudMM_MouseClick;
            // 
            // lblHour
            // 
            lblHour.AutoSize = true;
            lblHour.Location = new System.Drawing.Point(61, 26);
            lblHour.Name = "lblHour";
            lblHour.Size = new System.Drawing.Size(12, 19);
            lblHour.TabIndex = 4;
            lblHour.Text = ":";
            // 
            // nudHH
            // 
            nudHH.Location = new System.Drawing.Point(6, 24);
            nudHH.Maximum = new decimal(new int[] { 24, 0, 0, 0 });
            nudHH.Name = "nudHH";
            nudHH.Size = new System.Drawing.Size(52, 25);
            nudHH.TabIndex = 9;
            nudHH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            nudHH.Enter += NudHH_Enter;
            nudHH.KeyDown += NudHH_KeyDown;
            nudHH.Leave += NudHH_Leave;
            nudHH.MouseClick += NudHH_MouseClick;
            // 
            // gbStation
            // 
            gbStation.Controls.Add(cmBxStations);
            gbStation.Location = new System.Drawing.Point(12, 244);
            gbStation.Name = "gbStation";
            gbStation.Size = new System.Drawing.Size(146, 60);
            gbStation.TabIndex = 13;
            gbStation.TabStop = false;
            gbStation.Text = "Station";
            // 
            // cmBxStations
            // 
            cmBxStations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            cmBxStations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cmBxStations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cmBxStations.FormattingEnabled = true;
            cmBxStations.Location = new System.Drawing.Point(6, 24);
            cmBxStations.Name = "cmBxStations";
            cmBxStations.Size = new System.Drawing.Size(132, 25);
            cmBxStations.TabIndex = 8;
            // 
            // btnOK
            // 
            btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnOK.Location = new System.Drawing.Point(12, 375);
            btnOK.Name = "btnOK";
            btnOK.Size = new System.Drawing.Size(70, 27);
            btnOK.TabIndex = 11;
            btnOK.Text = "Save";
            btnOK.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            btnCancel.Location = new System.Drawing.Point(88, 375);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(70, 27);
            btnCancel.TabIndex = 12;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // FrmTask
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(170, 414);
            Controls.Add(btnCancel);
            Controls.Add(btnOK);
            Controls.Add(gbStation);
            Controls.Add(gbTime);
            Controls.Add(gbAction);
            Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmTask";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Task";
            gbAction.ResumeLayout(false);
            gbAction.PerformLayout();
            gbTime.ResumeLayout(false);
            gbTime.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudMM).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudHH).EndInit();
            gbStation.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.GroupBox gbAction;
        private System.Windows.Forms.RadioButton rbPlay;
        private System.Windows.Forms.RadioButton rbRecStop;
        private System.Windows.Forms.RadioButton rbRecord;
        private System.Windows.Forms.RadioButton rbStop;
        private System.Windows.Forms.RadioButton rbSleep;
        private System.Windows.Forms.GroupBox gbTime;
        private NumericUpDown00 nudMM;
        private System.Windows.Forms.Label lblHour;
        private NumericUpDown00 nudHH;
        private System.Windows.Forms.GroupBox gbStation;
        private System.Windows.Forms.ComboBox cmBxStations;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.RadioButton rbShutdown;
        private System.Windows.Forms.RadioButton rbHibernate;
    }
}