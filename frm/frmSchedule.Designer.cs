namespace NetRadio
{
    partial class FrmSchedule
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            aLV = new System.Windows.Forms.ListView();
            chStatus = new System.Windows.Forms.ColumnHeader();
            chAction = new System.Windows.Forms.ColumnHeader();
            chStation = new System.Windows.Forms.ColumnHeader();
            chTime = new System.Windows.Forms.ColumnHeader();
            timerLabel = new System.Windows.Forms.Label();
            btnEdit = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            cbRepeatActionsDaily = new System.Windows.Forms.CheckBox();
            lblRepeat = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // aLV
            // 
            aLV.CheckBoxes = true;
            aLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { chStatus, chAction, chStation, chTime });
            aLV.Dock = System.Windows.Forms.DockStyle.Top;
            aLV.FullRowSelect = true;
            listViewItem1.StateImageIndex = 0;
            listViewItem2.StateImageIndex = 0;
            listViewItem3.StateImageIndex = 0;
            listViewItem4.StateImageIndex = 0;
            listViewItem5.StateImageIndex = 0;
            listViewItem6.StateImageIndex = 0;
            listViewItem7.StateImageIndex = 0;
            listViewItem8.StateImageIndex = 0;
            listViewItem9.StateImageIndex = 0;
            aLV.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem1, listViewItem2, listViewItem3, listViewItem4, listViewItem5, listViewItem6, listViewItem7, listViewItem8, listViewItem9 });
            aLV.Location = new System.Drawing.Point(10, 55);
            aLV.MultiSelect = false;
            aLV.Name = "aLV";
            aLV.Size = new System.Drawing.Size(255, 217);
            aLV.TabIndex = 0;
            aLV.UseCompatibleStateImageBehavior = false;
            aLV.View = System.Windows.Forms.View.Details;
            aLV.ItemCheck += ALV_ItemCheck;
            aLV.ItemChecked += ALV_ItemChecked;
            aLV.MouseDoubleClick += TimerListview_MouseDoubleClick;
            aLV.MouseDown += ALV_MouseDown;
            // 
            // chStatus
            // 
            chStatus.Text = "";
            chStatus.Width = 21;
            // 
            // chAction
            // 
            chAction.Text = "Action";
            chAction.Width = 100;
            // 
            // chStation
            // 
            chStation.Text = "Station";
            chStation.Width = 80;
            // 
            // chTime
            // 
            chTime.Text = "Time";
            chTime.Width = 50;
            // 
            // timerLabel
            // 
            timerLabel.Dock = System.Windows.Forms.DockStyle.Top;
            timerLabel.Font = new System.Drawing.Font("Segoe UI", 9F);
            timerLabel.Location = new System.Drawing.Point(10, 0);
            timerLabel.Name = "timerLabel";
            timerLabel.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            timerLabel.Size = new System.Drawing.Size(255, 55);
            timerLabel.TabIndex = 2;
            timerLabel.Text = "You can set up tasks and NetRadio will auto-\r\nmatically perform the selected actions at the\r\ntime you choose, provided it is running.";
            // 
            // btnEdit
            // 
            btnEdit.Location = new System.Drawing.Point(10, 322);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new System.Drawing.Size(75, 27);
            btnEdit.TabIndex = 4;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            btnEdit.Click += BtnEdit_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(91, 322);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(75, 27);
            btnDelete.TabIndex = 5;
            btnDelete.Text = "Delete";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += BtnDelete_Click;
            // 
            // btnClose
            // 
            btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            btnClose.Location = new System.Drawing.Point(189, 322);
            btnClose.Name = "btnClose";
            btnClose.Size = new System.Drawing.Size(75, 27);
            btnClose.TabIndex = 6;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // cbRepeatActionsDaily
            // 
            cbRepeatActionsDaily.AutoSize = true;
            cbRepeatActionsDaily.Font = new System.Drawing.Font("Segoe UI", 10F);
            cbRepeatActionsDaily.Location = new System.Drawing.Point(16, 278);
            cbRepeatActionsDaily.Name = "cbRepeatActionsDaily";
            cbRepeatActionsDaily.Size = new System.Drawing.Size(154, 23);
            cbRepeatActionsDaily.TabIndex = 7;
            cbRepeatActionsDaily.Text = "Repeat all tasks daily";
            cbRepeatActionsDaily.UseVisualStyleBackColor = true;
            cbRepeatActionsDaily.CheckedChanged += CbRepeatActionsDaily_CheckedChanged;
            // 
            // lblRepeat
            // 
            lblRepeat.AutoSize = true;
            lblRepeat.Font = new System.Drawing.Font("Segoe UI", 9F);
            lblRepeat.ForeColor = System.Drawing.Color.IndianRed;
            lblRepeat.Location = new System.Drawing.Point(10, 301);
            lblRepeat.Name = "lblRepeat";
            lblRepeat.Size = new System.Drawing.Size(255, 15);
            lblRepeat.TabIndex = 8;
            lblRepeat.Text = "Caution: The computer is shut down every day!";
            lblRepeat.Visible = false;
            // 
            // FrmSchedule
            // 
            AcceptButton = btnClose;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(275, 355);
            Controls.Add(lblRepeat);
            Controls.Add(cbRepeatActionsDaily);
            Controls.Add(btnClose);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(aLV);
            Controls.Add(timerLabel);
            Font = new System.Drawing.Font("Segoe UI", 10F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSchedule";
            Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Schedule";
            Load += FrmSchedule_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView aLV;
        private System.Windows.Forms.ColumnHeader chStatus;
        private System.Windows.Forms.ColumnHeader chAction;
        private System.Windows.Forms.ColumnHeader chStation;
        private System.Windows.Forms.ColumnHeader chTime;
        private System.Windows.Forms.Label timerLabel;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox cbRepeatActionsDaily;
        private System.Windows.Forms.Label lblRepeat;
    }
}