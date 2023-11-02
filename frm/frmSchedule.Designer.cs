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
            System.Windows.Forms.ListViewItem listViewItem28 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem29 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem30 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem31 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem32 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem33 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem34 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem35 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            System.Windows.Forms.ListViewItem listViewItem36 = new System.Windows.Forms.ListViewItem(new string[] { "", "", "", "" }, -1);
            aLV = new System.Windows.Forms.ListView();
            chStatus = new System.Windows.Forms.ColumnHeader();
            chAction = new System.Windows.Forms.ColumnHeader();
            chStation = new System.Windows.Forms.ColumnHeader();
            chTime = new System.Windows.Forms.ColumnHeader();
            timerLabel = new System.Windows.Forms.Label();
            btnEdit = new System.Windows.Forms.Button();
            btnDelete = new System.Windows.Forms.Button();
            btnClose = new System.Windows.Forms.Button();
            cbKeepActionsActive = new System.Windows.Forms.CheckBox();
            lblRepeat = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // aLV
            // 
            aLV.CheckBoxes = true;
            aLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { chStatus, chAction, chStation, chTime });
            aLV.Dock = System.Windows.Forms.DockStyle.Top;
            aLV.FullRowSelect = true;
            listViewItem28.StateImageIndex = 0;
            listViewItem29.StateImageIndex = 0;
            listViewItem30.StateImageIndex = 0;
            listViewItem31.StateImageIndex = 0;
            listViewItem32.StateImageIndex = 0;
            listViewItem33.StateImageIndex = 0;
            listViewItem34.StateImageIndex = 0;
            listViewItem35.StateImageIndex = 0;
            listViewItem36.StateImageIndex = 0;
            aLV.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem28, listViewItem29, listViewItem30, listViewItem31, listViewItem32, listViewItem33, listViewItem34, listViewItem35, listViewItem36 });
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
            timerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
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
            // cbKeepActionsActive
            // 
            cbKeepActionsActive.AutoSize = true;
            cbKeepActionsActive.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cbKeepActionsActive.Location = new System.Drawing.Point(16, 278);
            cbKeepActionsActive.Name = "cbKeepActionsActive";
            cbKeepActionsActive.Size = new System.Drawing.Size(248, 23);
            cbKeepActionsActive.TabIndex = 7;
            cbKeepActionsActive.Text = "Keep active when restarting the app";
            cbKeepActionsActive.UseVisualStyleBackColor = true;
            cbKeepActionsActive.CheckedChanged += CbKeepActionsActive_CheckedChanged;
            // 
            // lblRepeat
            // 
            lblRepeat.AutoSize = true;
            lblRepeat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblRepeat.Location = new System.Drawing.Point(10, 301);
            lblRepeat.Name = "lblRepeat";
            lblRepeat.Size = new System.Drawing.Size(255, 15);
            lblRepeat.TabIndex = 8;
            lblRepeat.Text = "Repeats must be activated manually each time.";
            // 
            // FrmSchedule
            // 
            AcceptButton = btnClose;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(275, 355);
            Controls.Add(lblRepeat);
            Controls.Add(cbKeepActionsActive);
            Controls.Add(btnClose);
            Controls.Add(btnDelete);
            Controls.Add(btnEdit);
            Controls.Add(aLV);
            Controls.Add(timerLabel);
            Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSchedule";
            Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Schedule";
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
        private System.Windows.Forms.CheckBox cbKeepActionsActive;
        private System.Windows.Forms.Label lblRepeat;
    }
}