namespace NetRadio
{
    partial class MiniPlayer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MiniPlayer));
            panelTitle = new System.Windows.Forms.Panel();
            btnRestore = new System.Windows.Forms.Button();
            imageList = new System.Windows.Forms.ImageList(components);
            labelD2 = new System.Windows.Forms.Label();
            contextMenuDisplay = new System.Windows.Forms.ContextMenuStrip(components);
            googleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            copyToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            volProgressBar = new ProgressBarEx();
            pictureBoxLevel = new System.Windows.Forms.PictureBox();
            btnPlayPause = new System.Windows.Forms.Button();
            cmBxStations = new System.Windows.Forms.ComboBox();
            btnAOT = new System.Windows.Forms.Button();
            toolTip = new System.Windows.Forms.ToolTip(components);
            btnReload = new System.Windows.Forms.Button();
            timerVolTT = new System.Windows.Forms.Timer(components);
            panelDown = new System.Windows.Forms.Panel();
            panelControls = new System.Windows.Forms.Panel();
            panelTitle.SuspendLayout();
            contextMenuDisplay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLevel).BeginInit();
            panelControls.SuspendLayout();
            SuspendLayout();
            // 
            // panelTitle
            // 
            panelTitle.BackColor = System.Drawing.Color.AliceBlue;
            panelTitle.Controls.Add(btnRestore);
            panelTitle.Controls.Add(labelD2);
            panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            panelTitle.Location = new System.Drawing.Point(0, 0);
            panelTitle.Name = "panelTitle";
            panelTitle.Size = new System.Drawing.Size(256, 23);
            panelTitle.TabIndex = 0;
            panelTitle.Paint += Panel_Paint;
            panelTitle.MouseDoubleClick += Panel_MouseDoubleClick;
            panelTitle.MouseDown += MiniPlayer_MouseDown;
            // 
            // btnRestore
            // 
            btnRestore.BackColor = System.Drawing.Color.LightSteelBlue;
            btnRestore.FlatAppearance.BorderColor = System.Drawing.Color.LightSteelBlue;
            btnRestore.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Maroon;
            btnRestore.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Crimson;
            btnRestore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnRestore.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnRestore.ForeColor = System.Drawing.SystemColors.HighlightText;
            btnRestore.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            btnRestore.ImageIndex = 0;
            btnRestore.ImageList = imageList;
            btnRestore.Location = new System.Drawing.Point(231, -1);
            btnRestore.Name = "btnRestore";
            btnRestore.Size = new System.Drawing.Size(26, 26);
            btnRestore.TabIndex = 2;
            btnRestore.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            toolTip.SetToolTip(btnRestore, "Main Window (Esc)\r\nWith Shift-Key: Exit");
            btnRestore.UseVisualStyleBackColor = false;
            btnRestore.Click += BtnRestore_Click;
            btnRestore.Paint += BtnRestore_Paint;
            btnRestore.MouseDown += BtnRestore_MouseDown;
            btnRestore.MouseEnter += BtnRestore_MouseEnter;
            btnRestore.MouseLeave += BtnRestore_MouseLeave;
            btnRestore.MouseUp += BtnRestore_MouseUp;
            // 
            // imageList
            // 
            imageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList.ImageStream");
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            imageList.Images.SetKeyName(0, "18_restore1.png");
            imageList.Images.SetKeyName(1, "18_restore2.png");
            imageList.Images.SetKeyName(2, "18_restore3.png");
            imageList.Images.SetKeyName(3, "18_close.png");
            imageList.Images.SetKeyName(4, "18_restore4.png");
            // 
            // labelD2
            // 
            labelD2.AutoEllipsis = true;
            labelD2.BackColor = System.Drawing.Color.Transparent;
            labelD2.ContextMenuStrip = contextMenuDisplay;
            labelD2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            labelD2.Location = new System.Drawing.Point(7, 7);
            labelD2.MaximumSize = new System.Drawing.Size(223, 15);
            labelD2.Name = "labelD2";
            labelD2.Size = new System.Drawing.Size(223, 15);
            labelD2.TabIndex = 1;
            labelD2.Text = "-";
            labelD2.TextChanged += LabelD2_TextChanged;
            labelD2.MouseDoubleClick += LabelD2_MouseDoubleClick;
            labelD2.MouseDown += MiniPlayer_MouseDown;
            // 
            // contextMenuDisplay
            // 
            contextMenuDisplay.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { googleToolStripMenuItem, copyToClipboardToolStripMenuItem, toolStripSeparator1, closeToolStripMenuItem });
            contextMenuDisplay.Name = "contextMenuDisplay";
            contextMenuDisplay.Size = new System.Drawing.Size(243, 76);
            // 
            // googleToolStripMenuItem
            // 
            googleToolStripMenuItem.Image = Properties.Resources.websearch;
            googleToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            googleToolStripMenuItem.Name = "googleToolStripMenuItem";
            googleToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+G";
            googleToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            googleToolStripMenuItem.Text = "Google Songtitle";
            googleToolStripMenuItem.Click += GoogleToolStripMenuItem_Click;
            // 
            // copyToClipboardToolStripMenuItem
            // 
            copyToClipboardToolStripMenuItem.Image = Properties.Resources.retrun;
            copyToClipboardToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            copyToClipboardToolStripMenuItem.Name = "copyToClipboardToolStripMenuItem";
            copyToClipboardToolStripMenuItem.ShortcutKeyDisplayString = "DoubleClick";
            copyToClipboardToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            copyToClipboardToolStripMenuItem.Text = "Copy to Clipboard";
            copyToClipboardToolStripMenuItem.Click += CopyToClipboardToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(239, 6);
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Image = Properties.Resources.cancel;
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.ShortcutKeyDisplayString = "Alt+F4";
            closeToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.Click += CloseToolStripMenuItem_Click;
            // 
            // volProgressBar
            // 
            volProgressBar.Cursor = System.Windows.Forms.Cursors.Hand;
            volProgressBar.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            volProgressBar.Location = new System.Drawing.Point(121, 33);
            volProgressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            volProgressBar.Name = "volProgressBar";
            volProgressBar.Size = new System.Drawing.Size(115, 8);
            volProgressBar.Step = 1;
            volProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            volProgressBar.TabIndex = 31;
            toolTip.SetToolTip(volProgressBar, "Volume(+/–)");
            volProgressBar.MouseDown += VolProgressBar_MouseDown;
            volProgressBar.MouseMove += VolProgressBar_MouseMove;
            volProgressBar.MouseUp += VolProgressBar_MouseUp;
            // 
            // pictureBoxLevel
            // 
            pictureBoxLevel.BackColor = System.Drawing.Color.FromArgb(235, 235, 235);
            pictureBoxLevel.Location = new System.Drawing.Point(1, 33);
            pictureBoxLevel.Name = "pictureBoxLevel";
            pictureBoxLevel.Size = new System.Drawing.Size(118, 8);
            pictureBoxLevel.TabIndex = 2;
            pictureBoxLevel.TabStop = false;
            pictureBoxLevel.Paint += PictureBoxLevel_Paint;
            pictureBoxLevel.MouseDown += MiniPlayer_MouseDown;
            // 
            // btnPlayPause
            // 
            btnPlayPause.BackColor = System.Drawing.SystemColors.ControlDark;
            btnPlayPause.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnPlayPause.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            btnPlayPause.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            btnPlayPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnPlayPause.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnPlayPause.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btnPlayPause.Image = Properties.Resources.play_white;
            btnPlayPause.Location = new System.Drawing.Point(1, 3);
            btnPlayPause.Name = "btnPlayPause";
            btnPlayPause.Size = new System.Drawing.Size(25, 25);
            btnPlayPause.TabIndex = 1;
            toolTip.SetToolTip(btnPlayPause, "Play/Pause (Space)");
            btnPlayPause.UseVisualStyleBackColor = false;
            btnPlayPause.Click += BtnPlayPause_Click;
            // 
            // cmBxStations
            // 
            cmBxStations.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            cmBxStations.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            cmBxStations.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmBxStations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cmBxStations.FormattingEnabled = true;
            cmBxStations.Location = new System.Drawing.Point(55, 4);
            cmBxStations.Name = "cmBxStations";
            cmBxStations.Size = new System.Drawing.Size(152, 23);
            cmBxStations.TabIndex = 2;
            toolTip.SetToolTip(cmBxStations, "List (F2)");
            cmBxStations.SelectedIndexChanged += CmBxStations_SelectedIndexChanged;
            // 
            // btnAOT
            // 
            btnAOT.BackColor = System.Drawing.Color.Maroon;
            btnAOT.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnAOT.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            btnAOT.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Sienna;
            btnAOT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnAOT.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnAOT.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btnAOT.Image = Properties.Resources.pinpush;
            btnAOT.Location = new System.Drawing.Point(211, 3);
            btnAOT.Name = "btnAOT";
            btnAOT.Size = new System.Drawing.Size(25, 25);
            btnAOT.TabIndex = 3;
            toolTip.SetToolTip(btnAOT, "Always on Top (Pos1)");
            btnAOT.UseVisualStyleBackColor = false;
            btnAOT.Click += BtnAOT_Click;
            // 
            // btnReload
            // 
            btnReload.BackColor = System.Drawing.SystemColors.ControlDark;
            btnReload.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnReload.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            btnReload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            btnReload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnReload.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            btnReload.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btnReload.Image = Properties.Resources.replay_white;
            btnReload.Location = new System.Drawing.Point(26, 3);
            btnReload.Name = "btnReload";
            btnReload.Size = new System.Drawing.Size(25, 25);
            btnReload.TabIndex = 32;
            toolTip.SetToolTip(btnReload, "Reload (Backspace)");
            btnReload.UseVisualStyleBackColor = false;
            btnReload.Click += BtnReload_Click;
            // 
            // timerVolTT
            // 
            timerVolTT.Interval = 500;
            timerVolTT.Tick += TimerVolTT_Tick;
            // 
            // panelDown
            // 
            panelDown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            panelDown.Location = new System.Drawing.Point(54, 3);
            panelDown.Name = "panelDown";
            panelDown.Size = new System.Drawing.Size(154, 25);
            panelDown.TabIndex = 4;
            // 
            // panelControls
            // 
            panelControls.BackColor = System.Drawing.SystemColors.ControlLightLight;
            panelControls.Controls.Add(btnReload);
            panelControls.Controls.Add(btnPlayPause);
            panelControls.Controls.Add(volProgressBar);
            panelControls.Controls.Add(cmBxStations);
            panelControls.Controls.Add(pictureBoxLevel);
            panelControls.Controls.Add(btnAOT);
            panelControls.Controls.Add(panelDown);
            panelControls.Location = new System.Drawing.Point(10, 25);
            panelControls.Name = "panelControls";
            panelControls.Size = new System.Drawing.Size(237, 43);
            panelControls.TabIndex = 32;
            panelControls.MouseDown += MiniPlayer_MouseDown;
            // 
            // MiniPlayer
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(256, 77);
            Controls.Add(panelControls);
            Controls.Add(panelTitle);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MiniPlayer";
            Opacity = 0D;
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            Text = "MiniPlayer";
            TopMost = true;
            Activated += MiniPlayer_Activated;
            Deactivate += MiniPlayer_Deactivate;
            FormClosing += MiniPlayer_FormClosing;
            Shown += MiniPlayer_Shown;
            MouseDoubleClick += MiniPlayer_MouseDoubleClick;
            MouseDown += MiniPlayer_MouseDown;
            panelTitle.ResumeLayout(false);
            contextMenuDisplay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxLevel).EndInit();
            panelControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Button btnPlayPause;
        private System.Windows.Forms.Label labelD2;
        private System.Windows.Forms.PictureBox pictureBoxLevel;
        private System.Windows.Forms.Button btnRestore;
        private ProgressBarEx volProgressBar;
        private System.Windows.Forms.ComboBox cmBxStations;
        private System.Windows.Forms.Button btnAOT;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Timer timerVolTT;
        private System.Windows.Forms.Panel panelDown;
        private System.Windows.Forms.ContextMenuStrip contextMenuDisplay;
        private System.Windows.Forms.ToolStripMenuItem googleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.Button btnReload;
        private System.Windows.Forms.ImageList imageList;
    }
}