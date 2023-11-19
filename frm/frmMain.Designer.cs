using System.Windows.Forms;

namespace NetRadio
{
    partial class FrmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            tcMain = new TabControl();
            tpPlayer = new TabPage();
            rbtn25 = new RadioButton();
            contextMenuPlayer = new ContextMenuStrip(components);
            editStationToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator8 = new ToolStripSeparator();
            searchNewStationToolStripMenuItem = new ToolStripMenuItem();
            rbtn24 = new RadioButton();
            rbtn23 = new RadioButton();
            rbtn22 = new RadioButton();
            rbtn21 = new RadioButton();
            rbtn20 = new RadioButton();
            rbtn19 = new RadioButton();
            rbtn18 = new RadioButton();
            rbtn17 = new RadioButton();
            rbtn16 = new RadioButton();
            rbtn15 = new RadioButton();
            rbtn14 = new RadioButton();
            rbtn13 = new RadioButton();
            btnRecord = new Button();
            rbtn12 = new RadioButton();
            rbtn11 = new RadioButton();
            rbtn10 = new RadioButton();
            rbtn09 = new RadioButton();
            pnlDisplay = new Panel();
            panelLevel = new Panel();
            pbLevel = new PictureBox();
            volProgressBar = new ProgressBarEx();
            lblVolume = new Label();
            pbVolIcon = new PictureBox();
            lblD4 = new Label();
            lblD3 = new Label();
            lblD2 = new Label();
            lblD1 = new Label();
            btnReset = new Button();
            btnDecrease = new Button();
            btnIncrease = new Button();
            btnPlayStop = new Button();
            rbtn08 = new RadioButton();
            rbtn07 = new RadioButton();
            rbtn06 = new RadioButton();
            rbtn05 = new RadioButton();
            rbtn04 = new RadioButton();
            rbtn03 = new RadioButton();
            rbtn02 = new RadioButton();
            rbtn01 = new RadioButton();
            tpStations = new TabPage();
            btnSearch = new Button();
            btnDown = new Button();
            btnUp = new Button();
            dgvStations = new DataGridView();
            col1 = new DataGridViewTextBoxColumn();
            col2 = new DataGridViewTextBoxColumn();
            contextMenuStations = new ContextMenuStrip(components);
            editToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            addToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator4 = new ToolStripSeparator();
            moveToToolStripMenuItem = new ToolStripMenuItem();
            row1ToolStripMenuItem = new ToolStripMenuItem();
            row2ToolStripMenuItem = new ToolStripMenuItem();
            row3ToolStripMenuItem = new ToolStripMenuItem();
            row4ToolStripMenuItem = new ToolStripMenuItem();
            row5ToolStripMenuItem = new ToolStripMenuItem();
            row6ToolStripMenuItem = new ToolStripMenuItem();
            row7ToolStripMenuItem = new ToolStripMenuItem();
            row8ToolStripMenuItem = new ToolStripMenuItem();
            row9ToolStripMenuItem = new ToolStripMenuItem();
            row10ToolStripMenuItem = new ToolStripMenuItem();
            row11ToolStripMenuItem = new ToolStripMenuItem();
            row12ToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            upToolStripMenuItem = new ToolStripMenuItem();
            downToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator6 = new ToolStripSeparator();
            pgUpToolStripMenuItem = new ToolStripMenuItem();
            pgDnToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator3 = new ToolStripSeparator();
            topToolStripMenuItem = new ToolStripMenuItem();
            endToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            searchStationToolStripMenuItem = new ToolStripMenuItem();
            tpHistory = new TabPage();
            historyListView = new ListView();
            chDate = new ColumnHeader();
            chStation = new ColumnHeader();
            chSong = new ColumnHeader();
            contextMenuDisplay = new ContextMenuStrip(components);
            googleToolStripMenuItem = new ToolStripMenuItem();
            copyToClipboardToolStripMenuItem = new ToolStripMenuItem();
            tsSepListViewDeleteEntry = new ToolStripSeparator();
            tSMItemListViewDeleteEntry = new ToolStripMenuItem();
            historyPanel = new Panel();
            historyExportButton = new Button();
            histoyClearButton = new Button();
            cbLogHistory = new CheckBox();
            tpSettings = new TabPage();
            panel1 = new Panel();
            rbStartModeTray = new RadioButton();
            rbStartModeMini = new RadioButton();
            rbStartModeMain = new RadioButton();
            labelStartMode = new Label();
            gbAutoRecord = new GroupBox();
            cbActions = new CheckBox();
            btnActions = new Button();
            gbOutput = new GroupBox();
            cmbxOutput = new ComboBox();
            gbMiscel = new GroupBox();
            cbClose2Tray = new CheckBox();
            cbAutoStopRecording = new CheckBox();
            cbShowBalloonTip = new CheckBox();
            cbAlwaysOnTop = new CheckBox();
            gbAutostart = new GroupBox();
            cbAutostart = new CheckBox();
            gbPreselection = new GroupBox();
            lblAutostartStation = new Label();
            cmbxStation = new ComboBox();
            gbHotkeys = new GroupBox();
            lblHotkey = new Label();
            cmbxHotkey = new ComboBox();
            cbHotkey = new CheckBox();
            tpHelp = new TabPage();
            tableLayoutPanel0 = new TableLayoutPanel();
            lplTPLHelp2 = new Label();
            lplTPLHelp1 = new Label();
            panelHelp = new Panel();
            labelBrackets = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            label6 = new Label();
            label5 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            label4 = new Label();
            label3 = new Label();
            tpInfo = new TabPage();
            btnUpdateSettings = new Button();
            imageList = new ImageList(components);
            linkLabeGNU = new LinkLabel();
            label10 = new Label();
            label9 = new Label();
            linkLabel1 = new LinkLabel();
            lblAuthor = new Label();
            progressBar = new ProgressBar();
            lblUpdate = new Label();
            picBoxPayPal = new PictureBox();
            btnUpdate = new Button();
            label8 = new Label();
            label7 = new Label();
            lblCredits = new Label();
            lblHorizontalLine = new Label();
            lblCopyright = new Label();
            lblWebService = new Label();
            linkHomepage = new LinkLabel();
            linkPayPal = new LinkLabel();
            lblInformation = new Label();
            tpSectrum = new TabPage();
            Lbl19 = new Label();
            Lbl18 = new Label();
            Lbl17 = new Label();
            Lbl16 = new Label();
            Lbl15 = new Label();
            Lbl14 = new Label();
            Lbl13 = new Label();
            Lbl12 = new Label();
            Lbl11 = new Label();
            Lbl10 = new Label();
            Lbl09 = new Label();
            Lbl08 = new Label();
            Lbl07 = new Label();
            Lbl06 = new Label();
            Lbl05 = new Label();
            Lbl04 = new Label();
            Lbl03 = new Label();
            Lbl02 = new Label();
            Lbl01 = new Label();
            Lbl00 = new Label();
            spectrumPanel = new Panel();
            Bar01 = new VerticalProgressBar();
            Bar02 = new VerticalProgressBar();
            Bar03 = new VerticalProgressBar();
            Bar04 = new VerticalProgressBar();
            Bar05 = new VerticalProgressBar();
            Bar06 = new VerticalProgressBar();
            Bar07 = new VerticalProgressBar();
            Bar08 = new VerticalProgressBar();
            Bar09 = new VerticalProgressBar();
            Bar10 = new VerticalProgressBar();
            Bar11 = new VerticalProgressBar();
            Bar12 = new VerticalProgressBar();
            Bar13 = new VerticalProgressBar();
            Bar14 = new VerticalProgressBar();
            Bar15 = new VerticalProgressBar();
            Bar16 = new VerticalProgressBar();
            Bar17 = new VerticalProgressBar();
            Bar18 = new VerticalProgressBar();
            Bar19 = new VerticalProgressBar();
            Bar20 = new VerticalProgressBar();
            tpMiniplayer = new TabPage();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolTip = new ToolTip(components);
            notifyIcon = new NotifyIcon(components);
            contextMenuTrayIcon = new ContextMenuStrip(components);
            playPauseToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator7 = new ToolStripSeparator();
            controlToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator9 = new ToolStripSeparator();
            showToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            timerVolume = new Timer(components);
            timerLevel = new Timer(components);
            timerPause = new Timer(components);
            timerCloseFinally = new Timer(components);
            saveFileDialog = new SaveFileDialog();
            timer1 = new Timer(components);
            timerResume = new Timer(components);
            tcMain.SuspendLayout();
            tpPlayer.SuspendLayout();
            contextMenuPlayer.SuspendLayout();
            pnlDisplay.SuspendLayout();
            panelLevel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbLevel).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbVolIcon).BeginInit();
            tpStations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStations).BeginInit();
            contextMenuStations.SuspendLayout();
            tpHistory.SuspendLayout();
            contextMenuDisplay.SuspendLayout();
            historyPanel.SuspendLayout();
            tpSettings.SuspendLayout();
            panel1.SuspendLayout();
            gbAutoRecord.SuspendLayout();
            gbOutput.SuspendLayout();
            gbMiscel.SuspendLayout();
            gbAutostart.SuspendLayout();
            gbPreselection.SuspendLayout();
            gbHotkeys.SuspendLayout();
            tpHelp.SuspendLayout();
            tableLayoutPanel0.SuspendLayout();
            panelHelp.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tpInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBoxPayPal).BeginInit();
            tpSectrum.SuspendLayout();
            spectrumPanel.SuspendLayout();
            statusStrip.SuspendLayout();
            contextMenuTrayIcon.SuspendLayout();
            SuspendLayout();
            // 
            // tcMain
            // 
            tcMain.Controls.Add(tpPlayer);
            tcMain.Controls.Add(tpStations);
            tcMain.Controls.Add(tpHistory);
            tcMain.Controls.Add(tpSettings);
            tcMain.Controls.Add(tpHelp);
            tcMain.Controls.Add(tpInfo);
            tcMain.Controls.Add(tpSectrum);
            tcMain.Controls.Add(tpMiniplayer);
            tcMain.Dock = DockStyle.Top;
            tcMain.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            tcMain.HotTrack = true;
            tcMain.ImageList = imageList;
            tcMain.ItemSize = new System.Drawing.Size(50, 25);
            tcMain.Location = new System.Drawing.Point(0, 0);
            tcMain.Name = "tcMain";
            tcMain.SelectedIndex = 0;
            tcMain.ShowToolTips = true;
            tcMain.Size = new System.Drawing.Size(403, 370);
            tcMain.SizeMode = TabSizeMode.Fixed;
            tcMain.TabIndex = 0;
            tcMain.SelectedIndexChanged += TcMain_SelectedIndexChanged;
            // 
            // tpPlayer
            // 
            tpPlayer.Controls.Add(rbtn25);
            tpPlayer.Controls.Add(rbtn24);
            tpPlayer.Controls.Add(rbtn23);
            tpPlayer.Controls.Add(rbtn22);
            tpPlayer.Controls.Add(rbtn21);
            tpPlayer.Controls.Add(rbtn20);
            tpPlayer.Controls.Add(rbtn19);
            tpPlayer.Controls.Add(rbtn18);
            tpPlayer.Controls.Add(rbtn17);
            tpPlayer.Controls.Add(rbtn16);
            tpPlayer.Controls.Add(rbtn15);
            tpPlayer.Controls.Add(rbtn14);
            tpPlayer.Controls.Add(rbtn13);
            tpPlayer.Controls.Add(btnRecord);
            tpPlayer.Controls.Add(rbtn12);
            tpPlayer.Controls.Add(rbtn11);
            tpPlayer.Controls.Add(rbtn10);
            tpPlayer.Controls.Add(rbtn09);
            tpPlayer.Controls.Add(pnlDisplay);
            tpPlayer.Controls.Add(btnReset);
            tpPlayer.Controls.Add(btnDecrease);
            tpPlayer.Controls.Add(btnIncrease);
            tpPlayer.Controls.Add(btnPlayStop);
            tpPlayer.Controls.Add(rbtn08);
            tpPlayer.Controls.Add(rbtn07);
            tpPlayer.Controls.Add(rbtn06);
            tpPlayer.Controls.Add(rbtn05);
            tpPlayer.Controls.Add(rbtn04);
            tpPlayer.Controls.Add(rbtn03);
            tpPlayer.Controls.Add(rbtn02);
            tpPlayer.Controls.Add(rbtn01);
            tpPlayer.ImageIndex = 0;
            tpPlayer.Location = new System.Drawing.Point(4, 29);
            tpPlayer.Margin = new Padding(4, 3, 4, 3);
            tpPlayer.Name = "tpPlayer";
            tpPlayer.Padding = new Padding(3);
            tpPlayer.Size = new System.Drawing.Size(395, 337);
            tpPlayer.TabIndex = 0;
            tpPlayer.ToolTipText = "Player (F4)";
            tpPlayer.UseVisualStyleBackColor = true;
            tpPlayer.MouseUp += TpPlayer_MouseUp;
            // 
            // rbtn25
            // 
            rbtn25.Appearance = Appearance.Button;
            rbtn25.ContextMenuStrip = contextMenuPlayer;
            rbtn25.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn25.Location = new System.Drawing.Point(315, 308);
            rbtn25.Margin = new Padding(0, 3, 0, 3);
            rbtn25.Name = "rbtn25";
            rbtn25.Size = new System.Drawing.Size(79, 29);
            rbtn25.TabIndex = 25;
            rbtn25.Tag = "25";
            rbtn25.Text = "25";
            rbtn25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn25, "25");
            rbtn25.UseVisualStyleBackColor = true;
            rbtn25.CheckedChanged += RadioButton_CheckedChanged;
            rbtn25.Paint += RadioButton_Paint;
            // 
            // contextMenuPlayer
            // 
            contextMenuPlayer.Items.AddRange(new ToolStripItem[] { editStationToolStripMenuItem, toolStripSeparator8, searchNewStationToolStripMenuItem });
            contextMenuPlayer.Name = "contextMenuPlayer";
            contextMenuPlayer.Size = new System.Drawing.Size(214, 54);
            // 
            // editStationToolStripMenuItem
            // 
            editStationToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("editStationToolStripMenuItem.Image");
            editStationToolStripMenuItem.Name = "editStationToolStripMenuItem";
            editStationToolStripMenuItem.ShortcutKeyDisplayString = "F2";
            editStationToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            editStationToolStripMenuItem.Text = "Edit station button";
            editStationToolStripMenuItem.Click += EditStationToolStripMenuItem_Click;
            // 
            // toolStripSeparator8
            // 
            toolStripSeparator8.Name = "toolStripSeparator8";
            toolStripSeparator8.Size = new System.Drawing.Size(210, 6);
            // 
            // searchNewStationToolStripMenuItem
            // 
            searchNewStationToolStripMenuItem.Image = Properties.Resources.epul;
            searchNewStationToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.White;
            searchNewStationToolStripMenuItem.Name = "searchNewStationToolStripMenuItem";
            searchNewStationToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+F";
            searchNewStationToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            searchNewStationToolStripMenuItem.Text = "Search new station";
            searchNewStationToolStripMenuItem.Click += SearchNewStationToolStripMenuItem_Click;
            // 
            // rbtn24
            // 
            rbtn24.Appearance = Appearance.Button;
            rbtn24.ContextMenuStrip = contextMenuPlayer;
            rbtn24.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn24.Location = new System.Drawing.Point(237, 308);
            rbtn24.Margin = new Padding(0, 3, 0, 3);
            rbtn24.Name = "rbtn24";
            rbtn24.Size = new System.Drawing.Size(79, 29);
            rbtn24.TabIndex = 24;
            rbtn24.Tag = "24";
            rbtn24.Text = "24";
            rbtn24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn24, "24");
            rbtn24.UseVisualStyleBackColor = true;
            rbtn24.CheckedChanged += RadioButton_CheckedChanged;
            rbtn24.Paint += RadioButton_Paint;
            // 
            // rbtn23
            // 
            rbtn23.Appearance = Appearance.Button;
            rbtn23.ContextMenuStrip = contextMenuPlayer;
            rbtn23.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn23.Location = new System.Drawing.Point(159, 308);
            rbtn23.Margin = new Padding(0, 3, 0, 3);
            rbtn23.Name = "rbtn23";
            rbtn23.Size = new System.Drawing.Size(79, 29);
            rbtn23.TabIndex = 23;
            rbtn23.Tag = "23";
            rbtn23.Text = "23";
            rbtn23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn23, "23");
            rbtn23.UseVisualStyleBackColor = true;
            rbtn23.CheckedChanged += RadioButton_CheckedChanged;
            rbtn23.Paint += RadioButton_Paint;
            // 
            // rbtn22
            // 
            rbtn22.Appearance = Appearance.Button;
            rbtn22.ContextMenuStrip = contextMenuPlayer;
            rbtn22.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn22.Location = new System.Drawing.Point(81, 308);
            rbtn22.Margin = new Padding(0, 3, 0, 3);
            rbtn22.Name = "rbtn22";
            rbtn22.Size = new System.Drawing.Size(79, 29);
            rbtn22.TabIndex = 22;
            rbtn22.Tag = "22";
            rbtn22.Text = "22";
            rbtn22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn22, "22");
            rbtn22.UseVisualStyleBackColor = true;
            rbtn22.CheckedChanged += RadioButton_CheckedChanged;
            rbtn22.Paint += RadioButton_Paint;
            // 
            // rbtn21
            // 
            rbtn21.Appearance = Appearance.Button;
            rbtn21.ContextMenuStrip = contextMenuPlayer;
            rbtn21.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn21.Location = new System.Drawing.Point(3, 308);
            rbtn21.Margin = new Padding(0, 3, 0, 3);
            rbtn21.Name = "rbtn21";
            rbtn21.Size = new System.Drawing.Size(79, 29);
            rbtn21.TabIndex = 21;
            rbtn21.Tag = "21";
            rbtn21.Text = "21";
            rbtn21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn21, "21");
            rbtn21.UseVisualStyleBackColor = true;
            rbtn21.CheckedChanged += RadioButton_CheckedChanged;
            rbtn21.Paint += RadioButton_Paint;
            // 
            // rbtn20
            // 
            rbtn20.Appearance = Appearance.Button;
            rbtn20.ContextMenuStrip = contextMenuPlayer;
            rbtn20.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn20.Location = new System.Drawing.Point(315, 275);
            rbtn20.Margin = new Padding(0, 3, 0, 3);
            rbtn20.Name = "rbtn20";
            rbtn20.Size = new System.Drawing.Size(79, 29);
            rbtn20.TabIndex = 20;
            rbtn20.Tag = "20";
            rbtn20.Text = "20";
            rbtn20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn20, "20");
            rbtn20.UseVisualStyleBackColor = true;
            rbtn20.CheckedChanged += RadioButton_CheckedChanged;
            rbtn20.Paint += RadioButton_Paint;
            // 
            // rbtn19
            // 
            rbtn19.Appearance = Appearance.Button;
            rbtn19.ContextMenuStrip = contextMenuPlayer;
            rbtn19.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn19.Location = new System.Drawing.Point(237, 275);
            rbtn19.Margin = new Padding(0, 3, 0, 3);
            rbtn19.Name = "rbtn19";
            rbtn19.Size = new System.Drawing.Size(79, 29);
            rbtn19.TabIndex = 19;
            rbtn19.Tag = "19";
            rbtn19.Text = "19";
            rbtn19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn19, "19");
            rbtn19.UseVisualStyleBackColor = true;
            rbtn19.CheckedChanged += RadioButton_CheckedChanged;
            rbtn19.Paint += RadioButton_Paint;
            // 
            // rbtn18
            // 
            rbtn18.Appearance = Appearance.Button;
            rbtn18.ContextMenuStrip = contextMenuPlayer;
            rbtn18.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn18.Location = new System.Drawing.Point(159, 275);
            rbtn18.Margin = new Padding(0, 3, 0, 3);
            rbtn18.Name = "rbtn18";
            rbtn18.Size = new System.Drawing.Size(79, 29);
            rbtn18.TabIndex = 18;
            rbtn18.Tag = "18";
            rbtn18.Text = "18";
            rbtn18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn18, "18");
            rbtn18.UseVisualStyleBackColor = true;
            rbtn18.CheckedChanged += RadioButton_CheckedChanged;
            rbtn18.Paint += RadioButton_Paint;
            // 
            // rbtn17
            // 
            rbtn17.Appearance = Appearance.Button;
            rbtn17.ContextMenuStrip = contextMenuPlayer;
            rbtn17.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn17.Location = new System.Drawing.Point(81, 275);
            rbtn17.Margin = new Padding(0, 3, 0, 3);
            rbtn17.Name = "rbtn17";
            rbtn17.Size = new System.Drawing.Size(79, 29);
            rbtn17.TabIndex = 17;
            rbtn17.Tag = "17";
            rbtn17.Text = "17";
            rbtn17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn17, "17");
            rbtn17.UseVisualStyleBackColor = true;
            rbtn17.CheckedChanged += RadioButton_CheckedChanged;
            rbtn17.Paint += RadioButton_Paint;
            // 
            // rbtn16
            // 
            rbtn16.Appearance = Appearance.Button;
            rbtn16.ContextMenuStrip = contextMenuPlayer;
            rbtn16.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn16.Location = new System.Drawing.Point(3, 275);
            rbtn16.Margin = new Padding(0, 3, 0, 3);
            rbtn16.Name = "rbtn16";
            rbtn16.Size = new System.Drawing.Size(79, 29);
            rbtn16.TabIndex = 16;
            rbtn16.Tag = "16";
            rbtn16.Text = "16";
            rbtn16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn16, "16");
            rbtn16.UseVisualStyleBackColor = true;
            rbtn16.CheckedChanged += RadioButton_CheckedChanged;
            rbtn16.Paint += RadioButton_Paint;
            // 
            // rbtn15
            // 
            rbtn15.Appearance = Appearance.Button;
            rbtn15.ContextMenuStrip = contextMenuPlayer;
            rbtn15.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn15.Location = new System.Drawing.Point(315, 241);
            rbtn15.Margin = new Padding(0, 3, 0, 3);
            rbtn15.Name = "rbtn15";
            rbtn15.Size = new System.Drawing.Size(79, 29);
            rbtn15.TabIndex = 15;
            rbtn15.Tag = "15";
            rbtn15.Text = "15";
            rbtn15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn15, "15");
            rbtn15.UseVisualStyleBackColor = true;
            rbtn15.CheckedChanged += RadioButton_CheckedChanged;
            rbtn15.Paint += RadioButton_Paint;
            // 
            // rbtn14
            // 
            rbtn14.Appearance = Appearance.Button;
            rbtn14.ContextMenuStrip = contextMenuPlayer;
            rbtn14.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn14.Location = new System.Drawing.Point(237, 241);
            rbtn14.Margin = new Padding(0, 3, 0, 3);
            rbtn14.Name = "rbtn14";
            rbtn14.Size = new System.Drawing.Size(79, 29);
            rbtn14.TabIndex = 14;
            rbtn14.Tag = "14";
            rbtn14.Text = "14";
            rbtn14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn14, "14");
            rbtn14.UseVisualStyleBackColor = true;
            rbtn14.CheckedChanged += RadioButton_CheckedChanged;
            rbtn14.Paint += RadioButton_Paint;
            // 
            // rbtn13
            // 
            rbtn13.Appearance = Appearance.Button;
            rbtn13.ContextMenuStrip = contextMenuPlayer;
            rbtn13.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn13.Location = new System.Drawing.Point(159, 241);
            rbtn13.Margin = new Padding(0, 3, 0, 3);
            rbtn13.Name = "rbtn13";
            rbtn13.Size = new System.Drawing.Size(79, 29);
            rbtn13.TabIndex = 13;
            rbtn13.Tag = "13";
            rbtn13.Text = "13";
            rbtn13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn13, "13");
            rbtn13.UseVisualStyleBackColor = true;
            rbtn13.CheckedChanged += RadioButton_CheckedChanged;
            rbtn13.Paint += RadioButton_Paint;
            // 
            // btnRecord
            // 
            btnRecord.BackColor = System.Drawing.SystemColors.ControlDark;
            btnRecord.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnRecord.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            btnRecord.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            btnRecord.FlatStyle = FlatStyle.Flat;
            btnRecord.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btnRecord.Image = Properties.Resources.mic_white;
            btnRecord.Location = new System.Drawing.Point(315, 138);
            btnRecord.Margin = new Padding(4, 3, 4, 3);
            btnRecord.Name = "btnRecord";
            btnRecord.Size = new System.Drawing.Size(78, 29);
            btnRecord.TabIndex = 35;
            toolTip.SetToolTip(btnRecord, "Record (Ins)");
            btnRecord.UseVisualStyleBackColor = false;
            btnRecord.Click += BtnRecord_Click;
            // 
            // rbtn12
            // 
            rbtn12.Appearance = Appearance.Button;
            rbtn12.ContextMenuStrip = contextMenuPlayer;
            rbtn12.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn12.Location = new System.Drawing.Point(81, 241);
            rbtn12.Margin = new Padding(0, 3, 0, 3);
            rbtn12.Name = "rbtn12";
            rbtn12.Size = new System.Drawing.Size(79, 29);
            rbtn12.TabIndex = 12;
            rbtn12.Tag = "12";
            rbtn12.Text = "12";
            rbtn12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn12, "12");
            rbtn12.UseVisualStyleBackColor = true;
            rbtn12.CheckedChanged += RadioButton_CheckedChanged;
            rbtn12.Paint += RadioButton_Paint;
            // 
            // rbtn11
            // 
            rbtn11.Appearance = Appearance.Button;
            rbtn11.ContextMenuStrip = contextMenuPlayer;
            rbtn11.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn11.Location = new System.Drawing.Point(3, 241);
            rbtn11.Margin = new Padding(0, 3, 0, 3);
            rbtn11.Name = "rbtn11";
            rbtn11.Size = new System.Drawing.Size(79, 29);
            rbtn11.TabIndex = 11;
            rbtn11.Tag = "11";
            rbtn11.Text = "11";
            rbtn11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn11, "11");
            rbtn11.UseVisualStyleBackColor = true;
            rbtn11.CheckedChanged += RadioButton_CheckedChanged;
            rbtn11.Paint += RadioButton_Paint;
            // 
            // rbtn10
            // 
            rbtn10.Appearance = Appearance.Button;
            rbtn10.ContextMenuStrip = contextMenuPlayer;
            rbtn10.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn10.Location = new System.Drawing.Point(315, 208);
            rbtn10.Margin = new Padding(0, 3, 0, 3);
            rbtn10.Name = "rbtn10";
            rbtn10.Size = new System.Drawing.Size(79, 29);
            rbtn10.TabIndex = 10;
            rbtn10.Tag = "10";
            rbtn10.Text = "10";
            rbtn10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn10, "10");
            rbtn10.UseVisualStyleBackColor = true;
            rbtn10.CheckedChanged += RadioButton_CheckedChanged;
            rbtn10.Paint += RadioButton_Paint;
            // 
            // rbtn09
            // 
            rbtn09.Appearance = Appearance.Button;
            rbtn09.ContextMenuStrip = contextMenuPlayer;
            rbtn09.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn09.Location = new System.Drawing.Point(237, 208);
            rbtn09.Margin = new Padding(0, 3, 0, 3);
            rbtn09.Name = "rbtn09";
            rbtn09.Size = new System.Drawing.Size(79, 29);
            rbtn09.TabIndex = 9;
            rbtn09.Tag = "9";
            rbtn09.Text = "9";
            rbtn09.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn09, "9");
            rbtn09.UseVisualStyleBackColor = true;
            rbtn09.CheckedChanged += RadioButton_CheckedChanged;
            rbtn09.Paint += RadioButton_Paint;
            // 
            // pnlDisplay
            // 
            pnlDisplay.BorderStyle = BorderStyle.FixedSingle;
            pnlDisplay.Controls.Add(panelLevel);
            pnlDisplay.Controls.Add(volProgressBar);
            pnlDisplay.Controls.Add(lblVolume);
            pnlDisplay.Controls.Add(pbVolIcon);
            pnlDisplay.Controls.Add(lblD4);
            pnlDisplay.Controls.Add(lblD3);
            pnlDisplay.Controls.Add(lblD2);
            pnlDisplay.Controls.Add(lblD1);
            pnlDisplay.Dock = DockStyle.Top;
            pnlDisplay.Location = new System.Drawing.Point(3, 3);
            pnlDisplay.Margin = new Padding(4, 3, 4, 3);
            pnlDisplay.Name = "pnlDisplay";
            pnlDisplay.Size = new System.Drawing.Size(389, 128);
            pnlDisplay.TabIndex = 0;
            pnlDisplay.Paint += PnlDisplay_Paint;
            // 
            // panelLevel
            // 
            panelLevel.Controls.Add(pbLevel);
            panelLevel.Location = new System.Drawing.Point(370, -1);
            panelLevel.Name = "panelLevel";
            panelLevel.Size = new System.Drawing.Size(20, 129);
            panelLevel.TabIndex = 32;
            panelLevel.Paint += PanelLevel_Paint;
            // 
            // pbLevel
            // 
            pbLevel.Location = new System.Drawing.Point(4, 3);
            pbLevel.Margin = new Padding(4, 3, 4, 3);
            pbLevel.Name = "pbLevel";
            pbLevel.Size = new System.Drawing.Size(10, 122);
            pbLevel.TabIndex = 20;
            pbLevel.TabStop = false;
            pbLevel.Click += PbLevel_Click;
            // 
            // volProgressBar
            // 
            volProgressBar.Cursor = Cursors.Hand;
            volProgressBar.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            volProgressBar.Location = new System.Drawing.Point(33, 106);
            volProgressBar.Margin = new Padding(4, 3, 4, 3);
            volProgressBar.Name = "volProgressBar";
            volProgressBar.Size = new System.Drawing.Size(306, 10);
            volProgressBar.Step = 1;
            volProgressBar.Style = ProgressBarStyle.Continuous;
            volProgressBar.TabIndex = 30;
            volProgressBar.MouseDown += VolProgressBar_MouseDown;
            volProgressBar.MouseMove += VolProgressBar_MouseMove;
            // 
            // lblVolume
            // 
            lblVolume.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblVolume.Location = new System.Drawing.Point(338, 102);
            lblVolume.Margin = new Padding(4, 0, 4, 0);
            lblVolume.Name = "lblVolume";
            lblVolume.Size = new System.Drawing.Size(29, 17);
            lblVolume.TabIndex = 31;
            lblVolume.Text = "100";
            lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbVolIcon
            // 
            pbVolIcon.Image = Properties.Resources.volume;
            pbVolIcon.Location = new System.Drawing.Point(7, 104);
            pbVolIcon.Margin = new Padding(4, 3, 4, 3);
            pbVolIcon.Name = "pbVolIcon";
            pbVolIcon.Size = new System.Drawing.Size(19, 18);
            pbVolIcon.TabIndex = 21;
            pbVolIcon.TabStop = false;
            // 
            // lblD4
            // 
            lblD4.AutoEllipsis = true;
            lblD4.BackColor = System.Drawing.Color.Transparent;
            lblD4.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblD4.Location = new System.Drawing.Point(4, 77);
            lblD4.Margin = new Padding(4, 0, 4, 0);
            lblD4.Name = "lblD4";
            lblD4.Size = new System.Drawing.Size(370, 23);
            lblD4.TabIndex = 29;
            lblD4.Text = "-";
            lblD4.TextChanged += LblD4_TextChanged;
            lblD4.MouseClick += DisplayLabel_MouseClick;
            // 
            // lblD3
            // 
            lblD3.AutoEllipsis = true;
            lblD3.BackColor = System.Drawing.Color.Transparent;
            lblD3.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblD3.Location = new System.Drawing.Point(4, 54);
            lblD3.Margin = new Padding(4, 0, 4, 0);
            lblD3.Name = "lblD3";
            lblD3.Size = new System.Drawing.Size(370, 23);
            lblD3.TabIndex = 28;
            lblD3.Text = "-";
            lblD3.MouseClick += DisplayLabel_MouseClick;
            // 
            // lblD2
            // 
            lblD2.AutoEllipsis = true;
            lblD2.BackColor = System.Drawing.Color.Transparent;
            lblD2.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblD2.Location = new System.Drawing.Point(4, 31);
            lblD2.Margin = new Padding(4, 0, 4, 0);
            lblD2.Name = "lblD2";
            lblD2.Size = new System.Drawing.Size(370, 23);
            lblD2.TabIndex = 27;
            lblD2.Text = "-";
            lblD2.TextChanged += LblD2_TextChanged;
            lblD2.MouseClick += DisplayLabel_MouseClick;
            // 
            // lblD1
            // 
            lblD1.AutoEllipsis = true;
            lblD1.BackColor = System.Drawing.Color.Transparent;
            lblD1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lblD1.Location = new System.Drawing.Point(4, 8);
            lblD1.Margin = new Padding(4, 0, 4, 0);
            lblD1.Name = "lblD1";
            lblD1.Size = new System.Drawing.Size(370, 23);
            lblD1.TabIndex = 26;
            lblD1.Text = "-";
            lblD1.MouseClick += DisplayLabel_MouseClick;
            // 
            // btnReset
            // 
            btnReset.BackColor = System.Drawing.SystemColors.ControlDark;
            btnReset.Enabled = false;
            btnReset.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnReset.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            btnReset.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btnReset.Image = Properties.Resources.replay_white;
            btnReset.Location = new System.Drawing.Point(81, 138);
            btnReset.Margin = new Padding(4, 3, 4, 3);
            btnReset.Name = "btnReset";
            btnReset.Size = new System.Drawing.Size(78, 29);
            btnReset.TabIndex = 34;
            toolTip.SetToolTip(btnReset, "Reload (Backspace)");
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += BtnReset_Click;
            // 
            // btnDecrease
            // 
            btnDecrease.BackColor = System.Drawing.SystemColors.ControlDark;
            btnDecrease.Enabled = false;
            btnDecrease.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnDecrease.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            btnDecrease.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            btnDecrease.FlatStyle = FlatStyle.Flat;
            btnDecrease.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btnDecrease.Image = Properties.Resources.volume_down_white;
            btnDecrease.Location = new System.Drawing.Point(237, 138);
            btnDecrease.Margin = new Padding(4, 3, 4, 3);
            btnDecrease.Name = "btnDecrease";
            btnDecrease.Size = new System.Drawing.Size(78, 29);
            btnDecrease.TabIndex = 32;
            toolTip.SetToolTip(btnDecrease, "Decrease Volume (-)");
            btnDecrease.UseVisualStyleBackColor = false;
            btnDecrease.Click += BtnDecrease_Click;
            btnDecrease.MouseDown += BtnDecrease_MouseDown;
            btnDecrease.MouseUp += BtnDecrease_MouseUp;
            // 
            // btnIncrease
            // 
            btnIncrease.BackColor = System.Drawing.SystemColors.ControlDark;
            btnIncrease.Enabled = false;
            btnIncrease.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnIncrease.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            btnIncrease.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            btnIncrease.FlatStyle = FlatStyle.Flat;
            btnIncrease.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btnIncrease.Image = Properties.Resources.volume_up_white;
            btnIncrease.Location = new System.Drawing.Point(159, 138);
            btnIncrease.Margin = new Padding(0, 3, 0, 3);
            btnIncrease.Name = "btnIncrease";
            btnIncrease.Size = new System.Drawing.Size(78, 29);
            btnIncrease.TabIndex = 33;
            toolTip.SetToolTip(btnIncrease, "Increase Volume (+)");
            btnIncrease.UseVisualStyleBackColor = false;
            btnIncrease.Click += BtnIncrease_Click;
            btnIncrease.MouseDown += BtnIncrease_MouseDown;
            btnIncrease.MouseUp += BtnIncrease_MouseUp;
            // 
            // btnPlayStop
            // 
            btnPlayStop.BackColor = System.Drawing.SystemColors.ControlDark;
            btnPlayStop.Enabled = false;
            btnPlayStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            btnPlayStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
            btnPlayStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gray;
            btnPlayStop.FlatStyle = FlatStyle.Flat;
            btnPlayStop.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            btnPlayStop.Image = Properties.Resources.play_white;
            btnPlayStop.Location = new System.Drawing.Point(3, 138);
            btnPlayStop.Margin = new Padding(4, 3, 4, 3);
            btnPlayStop.Name = "btnPlayStop";
            btnPlayStop.Size = new System.Drawing.Size(78, 29);
            btnPlayStop.TabIndex = 31;
            toolTip.SetToolTip(btnPlayStop, "Play/Pause (Space)");
            btnPlayStop.UseVisualStyleBackColor = false;
            btnPlayStop.Click += BtnPlayStop_Click;
            // 
            // rbtn08
            // 
            rbtn08.Appearance = Appearance.Button;
            rbtn08.ContextMenuStrip = contextMenuPlayer;
            rbtn08.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn08.Location = new System.Drawing.Point(159, 208);
            rbtn08.Margin = new Padding(0, 3, 0, 3);
            rbtn08.Name = "rbtn08";
            rbtn08.Size = new System.Drawing.Size(79, 29);
            rbtn08.TabIndex = 8;
            rbtn08.Tag = "8";
            rbtn08.Text = "8";
            rbtn08.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn08, "8");
            rbtn08.UseVisualStyleBackColor = true;
            rbtn08.CheckedChanged += RadioButton_CheckedChanged;
            rbtn08.Paint += RadioButton_Paint;
            // 
            // rbtn07
            // 
            rbtn07.Appearance = Appearance.Button;
            rbtn07.ContextMenuStrip = contextMenuPlayer;
            rbtn07.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn07.Location = new System.Drawing.Point(81, 208);
            rbtn07.Margin = new Padding(0, 3, 0, 3);
            rbtn07.Name = "rbtn07";
            rbtn07.Size = new System.Drawing.Size(79, 29);
            rbtn07.TabIndex = 7;
            rbtn07.Tag = "7";
            rbtn07.Text = "7";
            rbtn07.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn07, "7");
            rbtn07.UseVisualStyleBackColor = true;
            rbtn07.CheckedChanged += RadioButton_CheckedChanged;
            rbtn07.Paint += RadioButton_Paint;
            // 
            // rbtn06
            // 
            rbtn06.Appearance = Appearance.Button;
            rbtn06.ContextMenuStrip = contextMenuPlayer;
            rbtn06.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn06.Location = new System.Drawing.Point(3, 208);
            rbtn06.Margin = new Padding(0, 3, 0, 3);
            rbtn06.Name = "rbtn06";
            rbtn06.Size = new System.Drawing.Size(79, 29);
            rbtn06.TabIndex = 6;
            rbtn06.Tag = "6";
            rbtn06.Text = "6";
            rbtn06.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn06, "6");
            rbtn06.UseVisualStyleBackColor = true;
            rbtn06.CheckedChanged += RadioButton_CheckedChanged;
            rbtn06.Paint += RadioButton_Paint;
            // 
            // rbtn05
            // 
            rbtn05.Appearance = Appearance.Button;
            rbtn05.ContextMenuStrip = contextMenuPlayer;
            rbtn05.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn05.Location = new System.Drawing.Point(315, 174);
            rbtn05.Margin = new Padding(0, 3, 0, 3);
            rbtn05.Name = "rbtn05";
            rbtn05.Size = new System.Drawing.Size(79, 29);
            rbtn05.TabIndex = 5;
            rbtn05.Tag = "5";
            rbtn05.Text = "5";
            rbtn05.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn05, "5");
            rbtn05.UseVisualStyleBackColor = true;
            rbtn05.CheckedChanged += RadioButton_CheckedChanged;
            rbtn05.Paint += RadioButton_Paint;
            // 
            // rbtn04
            // 
            rbtn04.Appearance = Appearance.Button;
            rbtn04.ContextMenuStrip = contextMenuPlayer;
            rbtn04.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn04.Location = new System.Drawing.Point(237, 174);
            rbtn04.Margin = new Padding(0, 3, 0, 3);
            rbtn04.Name = "rbtn04";
            rbtn04.Size = new System.Drawing.Size(79, 29);
            rbtn04.TabIndex = 4;
            rbtn04.Tag = "4";
            rbtn04.Text = "4";
            rbtn04.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn04, "4");
            rbtn04.UseVisualStyleBackColor = true;
            rbtn04.CheckedChanged += RadioButton_CheckedChanged;
            rbtn04.Paint += RadioButton_Paint;
            // 
            // rbtn03
            // 
            rbtn03.Appearance = Appearance.Button;
            rbtn03.ContextMenuStrip = contextMenuPlayer;
            rbtn03.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn03.Location = new System.Drawing.Point(159, 174);
            rbtn03.Margin = new Padding(0, 3, 0, 3);
            rbtn03.Name = "rbtn03";
            rbtn03.Size = new System.Drawing.Size(79, 29);
            rbtn03.TabIndex = 3;
            rbtn03.Tag = "3";
            rbtn03.Text = "3";
            rbtn03.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn03, "3");
            rbtn03.UseVisualStyleBackColor = true;
            rbtn03.CheckedChanged += RadioButton_CheckedChanged;
            rbtn03.Paint += RadioButton_Paint;
            // 
            // rbtn02
            // 
            rbtn02.Appearance = Appearance.Button;
            rbtn02.ContextMenuStrip = contextMenuPlayer;
            rbtn02.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn02.Location = new System.Drawing.Point(81, 174);
            rbtn02.Margin = new Padding(0, 3, 0, 3);
            rbtn02.Name = "rbtn02";
            rbtn02.Size = new System.Drawing.Size(79, 29);
            rbtn02.TabIndex = 2;
            rbtn02.Tag = "2";
            rbtn02.Text = "2";
            rbtn02.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn02, "2");
            rbtn02.UseVisualStyleBackColor = true;
            rbtn02.CheckedChanged += RadioButton_CheckedChanged;
            rbtn02.Paint += RadioButton_Paint;
            // 
            // rbtn01
            // 
            rbtn01.Appearance = Appearance.Button;
            rbtn01.ContextMenuStrip = contextMenuPlayer;
            rbtn01.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            rbtn01.ForeColor = System.Drawing.SystemColors.ControlText;
            rbtn01.Location = new System.Drawing.Point(3, 174);
            rbtn01.Margin = new Padding(0, 3, 0, 3);
            rbtn01.Name = "rbtn01";
            rbtn01.Size = new System.Drawing.Size(79, 29);
            rbtn01.TabIndex = 1;
            rbtn01.Tag = "1";
            rbtn01.Text = "1";
            rbtn01.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            toolTip.SetToolTip(rbtn01, "1");
            rbtn01.UseVisualStyleBackColor = true;
            rbtn01.CheckedChanged += RadioButton_CheckedChanged;
            rbtn01.Paint += RadioButton_Paint;
            // 
            // tpStations
            // 
            tpStations.Controls.Add(btnSearch);
            tpStations.Controls.Add(btnDown);
            tpStations.Controls.Add(btnUp);
            tpStations.Controls.Add(dgvStations);
            tpStations.ImageIndex = 1;
            tpStations.Location = new System.Drawing.Point(4, 29);
            tpStations.Margin = new Padding(4, 3, 4, 3);
            tpStations.Name = "tpStations";
            tpStations.Padding = new Padding(3);
            tpStations.Size = new System.Drawing.Size(395, 337);
            tpStations.TabIndex = 1;
            tpStations.ToolTipText = "Stations (F2)";
            tpStations.UseVisualStyleBackColor = true;
            // 
            // btnSearch
            // 
            btnSearch.BackgroundImageLayout = ImageLayout.Center;
            btnSearch.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnSearch.Image = (System.Drawing.Image)resources.GetObject("btnSearch.Image");
            btnSearch.Location = new System.Drawing.Point(3, 309);
            btnSearch.Margin = new Padding(4, 3, 4, 3);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new System.Drawing.Size(131, 29);
            btnSearch.TabIndex = 1;
            btnSearch.Text = "Search...";
            btnSearch.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip.SetToolTip(btnSearch, "Internet search for the URL\r\nof a favorite radio station");
            btnSearch.UseVisualStyleBackColor = false;
            btnSearch.Click += BtnSearch_Click;
            // 
            // btnDown
            // 
            btnDown.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnDown.Location = new System.Drawing.Point(263, 309);
            btnDown.Margin = new Padding(4, 3, 4, 3);
            btnDown.Name = "btnDown";
            btnDown.Size = new System.Drawing.Size(131, 29);
            btnDown.TabIndex = 3;
            btnDown.Text = "▼ Move Down";
            toolTip.SetToolTip(btnDown, "Move Row Down");
            btnDown.UseVisualStyleBackColor = true;
            btnDown.Click += BtnDown_Click;
            // 
            // btnUp
            // 
            btnUp.BackgroundImageLayout = ImageLayout.Center;
            btnUp.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnUp.Location = new System.Drawing.Point(133, 309);
            btnUp.Margin = new Padding(4, 3, 4, 3);
            btnUp.Name = "btnUp";
            btnUp.Size = new System.Drawing.Size(130, 29);
            btnUp.TabIndex = 2;
            btnUp.Text = "▲ Move Up";
            toolTip.SetToolTip(btnUp, "move row up");
            btnUp.UseVisualStyleBackColor = true;
            btnUp.Click += BtnUp_Click;
            // 
            // dgvStations
            // 
            dgvStations.AllowDrop = true;
            dgvStations.AllowUserToAddRows = false;
            dgvStations.AllowUserToDeleteRows = false;
            dgvStations.AllowUserToResizeColumns = false;
            dgvStations.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStations.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dgvStations.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.ControlDark;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvStations.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvStations.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvStations.Columns.AddRange(new DataGridViewColumn[] { col1, col2 });
            dgvStations.ContextMenuStrip = contextMenuStations;
            dgvStations.Dock = DockStyle.Top;
            dgvStations.EnableHeadersVisualStyles = false;
            dgvStations.Location = new System.Drawing.Point(3, 3);
            dgvStations.Margin = new Padding(4, 3, 4, 3);
            dgvStations.MultiSelect = false;
            dgvStations.Name = "dgvStations";
            dgvStations.RowHeadersWidth = 35;
            dgvStations.ScrollBars = ScrollBars.Vertical;
            dgvStations.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStations.Size = new System.Drawing.Size(389, 306);
            dgvStations.TabIndex = 0;
            dgvStations.CellBeginEdit += DgvStations_CellBeginEdit;
            dgvStations.CellEndEdit += DgvStations_CellEndEdit;
            dgvStations.CellValueChanged += DgvStations_CellValueChanged;
            dgvStations.RowPostPaint += DgvStations_RowPostPaint;
            dgvStations.RowsRemoved += DgvStations_RowsRemoved;
            dgvStations.SelectionChanged += DgvStations_SelectionChanged;
            dgvStations.DragDrop += DgvStations_DragDrop;
            dgvStations.DragOver += DataGridView_DragOver;
            dgvStations.KeyDown += DgvStations_KeyDown;
            dgvStations.MouseClick += DgvStations_MouseClick;
            dgvStations.MouseDown += DgvStations_MouseDown;
            dgvStations.MouseMove += DgvStations_MouseMove;
            // 
            // col1
            // 
            col1.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col1.FillWeight = 50F;
            col1.HeaderText = "Station Name";
            col1.MaxInputLength = 256;
            col1.Name = "col1";
            // 
            // col2
            // 
            col2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            col2.FillWeight = 50F;
            col2.HeaderText = "URL";
            col2.MaxInputLength = 1024;
            col2.Name = "col2";
            // 
            // contextMenuStations
            // 
            contextMenuStations.Items.AddRange(new ToolStripItem[] { editToolStripMenuItem, toolStripSeparator5, addToolStripMenuItem, deleteToolStripMenuItem, toolStripSeparator4, moveToToolStripMenuItem, toolStripSeparator1, searchStationToolStripMenuItem });
            contextMenuStations.Name = "contextMenuStations";
            contextMenuStations.Size = new System.Drawing.Size(150, 132);
            // 
            // editToolStripMenuItem
            // 
            editToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("editToolStripMenuItem.Image");
            editToolStripMenuItem.Name = "editToolStripMenuItem";
            editToolStripMenuItem.ShortcutKeyDisplayString = "F2";
            editToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            editToolStripMenuItem.Text = "Edit";
            editToolStripMenuItem.Click += EditToolStripMenuItem_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(146, 6);
            // 
            // addToolStripMenuItem
            // 
            addToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("addToolStripMenuItem.Image");
            addToolStripMenuItem.Name = "addToolStripMenuItem";
            addToolStripMenuItem.ShortcutKeyDisplayString = "Ins";
            addToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            addToolStripMenuItem.Text = "Add";
            addToolStripMenuItem.Click += AddToolStripMenuItem_Click;
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("deleteToolStripMenuItem.Image");
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.ShortcutKeyDisplayString = "Del";
            deleteToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            deleteToolStripMenuItem.Text = "Delete";
            deleteToolStripMenuItem.Click += DeleteToolStripMenuItem_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(146, 6);
            // 
            // moveToToolStripMenuItem
            // 
            moveToToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { row1ToolStripMenuItem, row2ToolStripMenuItem, row3ToolStripMenuItem, row4ToolStripMenuItem, row5ToolStripMenuItem, row6ToolStripMenuItem, row7ToolStripMenuItem, row8ToolStripMenuItem, row9ToolStripMenuItem, row10ToolStripMenuItem, row11ToolStripMenuItem, row12ToolStripMenuItem, toolStripSeparator2, upToolStripMenuItem, downToolStripMenuItem, toolStripSeparator6, pgUpToolStripMenuItem, pgDnToolStripMenuItem, toolStripSeparator3, topToolStripMenuItem, endToolStripMenuItem });
            moveToToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("moveToToolStripMenuItem.Image");
            moveToToolStripMenuItem.Name = "moveToToolStripMenuItem";
            moveToToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            moveToToolStripMenuItem.Text = "Move to...";
            // 
            // row1ToolStripMenuItem
            // 
            row1ToolStripMenuItem.Name = "row1ToolStripMenuItem";
            row1ToolStripMenuItem.ShortcutKeyDisplayString = "Alt+1";
            row1ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row1ToolStripMenuItem.Text = "1.";
            row1ToolStripMenuItem.Click += Row1ToolStripMenuItem_Click;
            // 
            // row2ToolStripMenuItem
            // 
            row2ToolStripMenuItem.Name = "row2ToolStripMenuItem";
            row2ToolStripMenuItem.ShortcutKeyDisplayString = "Alt+2";
            row2ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row2ToolStripMenuItem.Text = "2.";
            row2ToolStripMenuItem.Click += Row2ToolStripMenuItem_Click;
            // 
            // row3ToolStripMenuItem
            // 
            row3ToolStripMenuItem.Name = "row3ToolStripMenuItem";
            row3ToolStripMenuItem.ShortcutKeyDisplayString = "Alt+3";
            row3ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row3ToolStripMenuItem.Text = "3.";
            row3ToolStripMenuItem.Click += Row3ToolStripMenuItem_Click;
            // 
            // row4ToolStripMenuItem
            // 
            row4ToolStripMenuItem.Name = "row4ToolStripMenuItem";
            row4ToolStripMenuItem.ShortcutKeyDisplayString = "Alt+4";
            row4ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row4ToolStripMenuItem.Text = "4.";
            row4ToolStripMenuItem.Click += Row4ToolStripMenuItem_Click;
            // 
            // row5ToolStripMenuItem
            // 
            row5ToolStripMenuItem.Name = "row5ToolStripMenuItem";
            row5ToolStripMenuItem.ShortcutKeyDisplayString = "Alt+5";
            row5ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row5ToolStripMenuItem.Text = "5.";
            row5ToolStripMenuItem.Click += Row5ToolStripMenuItem_Click;
            // 
            // row6ToolStripMenuItem
            // 
            row6ToolStripMenuItem.Name = "row6ToolStripMenuItem";
            row6ToolStripMenuItem.ShortcutKeyDisplayString = "Alt+6";
            row6ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row6ToolStripMenuItem.Text = "6.";
            row6ToolStripMenuItem.Click += Row6ToolStripMenuItem_Click;
            // 
            // row7ToolStripMenuItem
            // 
            row7ToolStripMenuItem.Name = "row7ToolStripMenuItem";
            row7ToolStripMenuItem.ShortcutKeyDisplayString = "Alt+7";
            row7ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row7ToolStripMenuItem.Text = "7.";
            row7ToolStripMenuItem.Click += Row7ToolStripMenuItem_Click;
            // 
            // row8ToolStripMenuItem
            // 
            row8ToolStripMenuItem.Name = "row8ToolStripMenuItem";
            row8ToolStripMenuItem.ShortcutKeyDisplayString = "Alt+8";
            row8ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row8ToolStripMenuItem.Text = "8.";
            row8ToolStripMenuItem.Click += Row8ToolStripMenuItem_Click;
            // 
            // row9ToolStripMenuItem
            // 
            row9ToolStripMenuItem.Name = "row9ToolStripMenuItem";
            row9ToolStripMenuItem.ShortcutKeyDisplayString = "Alt+9";
            row9ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row9ToolStripMenuItem.Text = "9.";
            row9ToolStripMenuItem.Click += Row9ToolStripMenuItem_Click;
            // 
            // row10ToolStripMenuItem
            // 
            row10ToolStripMenuItem.Name = "row10ToolStripMenuItem";
            row10ToolStripMenuItem.ShortcutKeyDisplayString = "";
            row10ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row10ToolStripMenuItem.Text = "10.";
            row10ToolStripMenuItem.Click += Row10ToolStripMenuItem_Click;
            // 
            // row11ToolStripMenuItem
            // 
            row11ToolStripMenuItem.Name = "row11ToolStripMenuItem";
            row11ToolStripMenuItem.ShortcutKeyDisplayString = "";
            row11ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row11ToolStripMenuItem.Text = "11.";
            row11ToolStripMenuItem.Click += Row11ToolStripMenuItem_Click;
            // 
            // row12ToolStripMenuItem
            // 
            row12ToolStripMenuItem.Name = "row12ToolStripMenuItem";
            row12ToolStripMenuItem.ShortcutKeyDisplayString = "";
            row12ToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            row12ToolStripMenuItem.Text = "12.";
            row12ToolStripMenuItem.Click += Row12ToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(189, 6);
            // 
            // upToolStripMenuItem
            // 
            upToolStripMenuItem.Name = "upToolStripMenuItem";
            upToolStripMenuItem.ShortcutKeyDisplayString = "Alt+Up";
            upToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            upToolStripMenuItem.Text = "Up";
            upToolStripMenuItem.Click += UpToolStripMenuItem_Click;
            // 
            // downToolStripMenuItem
            // 
            downToolStripMenuItem.Name = "downToolStripMenuItem";
            downToolStripMenuItem.ShortcutKeyDisplayString = "Alt+Down";
            downToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            downToolStripMenuItem.Text = "Down";
            downToolStripMenuItem.Click += DownToolStripMenuItem_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(189, 6);
            // 
            // pgUpToolStripMenuItem
            // 
            pgUpToolStripMenuItem.Name = "pgUpToolStripMenuItem";
            pgUpToolStripMenuItem.ShortcutKeyDisplayString = "Alt+PgUp";
            pgUpToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            pgUpToolStripMenuItem.Text = "Page up";
            pgUpToolStripMenuItem.Click += PgUpToolStripMenuItem_Click;
            // 
            // pgDnToolStripMenuItem
            // 
            pgDnToolStripMenuItem.Name = "pgDnToolStripMenuItem";
            pgDnToolStripMenuItem.ShortcutKeyDisplayString = "Alt+PgDn";
            pgDnToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            pgDnToolStripMenuItem.Text = "Page down";
            pgDnToolStripMenuItem.Click += PgDnToolStripMenuItem_Click;
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(189, 6);
            // 
            // topToolStripMenuItem
            // 
            topToolStripMenuItem.Name = "topToolStripMenuItem";
            topToolStripMenuItem.ShortcutKeyDisplayString = "Alt+Home";
            topToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            topToolStripMenuItem.Text = "Top";
            topToolStripMenuItem.Click += TopToolStripMenuItem_Click;
            // 
            // endToolStripMenuItem
            // 
            endToolStripMenuItem.Name = "endToolStripMenuItem";
            endToolStripMenuItem.ShortcutKeyDisplayString = "Alt+End";
            endToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            endToolStripMenuItem.Text = "End";
            endToolStripMenuItem.Click += EndToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
            // 
            // searchStationToolStripMenuItem
            // 
            searchStationToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("searchStationToolStripMenuItem.Image");
            searchStationToolStripMenuItem.Name = "searchStationToolStripMenuItem";
            searchStationToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+F";
            searchStationToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            searchStationToolStripMenuItem.Text = "Search";
            searchStationToolStripMenuItem.Click += SearchStationToolStripMenuItem_Click;
            // 
            // tpHistory
            // 
            tpHistory.Controls.Add(historyListView);
            tpHistory.Controls.Add(historyPanel);
            tpHistory.ImageIndex = 2;
            tpHistory.Location = new System.Drawing.Point(4, 29);
            tpHistory.Name = "tpHistory";
            tpHistory.Size = new System.Drawing.Size(395, 337);
            tpHistory.TabIndex = 5;
            tpHistory.ToolTipText = "History (F5)";
            tpHistory.UseVisualStyleBackColor = true;
            tpHistory.Leave += TpHistory_Leave;
            // 
            // historyListView
            // 
            historyListView.Columns.AddRange(new ColumnHeader[] { chDate, chStation, chSong });
            historyListView.ContextMenuStrip = contextMenuDisplay;
            historyListView.Dock = DockStyle.Fill;
            historyListView.FullRowSelect = true;
            historyListView.Location = new System.Drawing.Point(0, 0);
            historyListView.Name = "historyListView";
            historyListView.OwnerDraw = true;
            historyListView.ShowItemToolTips = true;
            historyListView.Size = new System.Drawing.Size(395, 301);
            historyListView.TabIndex = 1;
            historyListView.UseCompatibleStateImageBehavior = false;
            historyListView.View = View.Details;
            historyListView.ColumnClick += HistoryListView_ColumnClick;
            historyListView.DrawColumnHeader += HistoryListView_DrawColumnHeader;
            historyListView.DrawItem += HistoryListView_DrawItem;
            historyListView.KeyDown += HistoryListView_KeyDown;
            historyListView.MouseDoubleClick += HistoryListView_MouseDoubleClick;
            historyListView.MouseDown += HistoryListView_MouseDown;
            // 
            // chDate
            // 
            chDate.Text = "Time";
            // 
            // chStation
            // 
            chStation.Text = "Station";
            chStation.Width = 80;
            // 
            // chSong
            // 
            chSong.Text = "Song";
            chSong.Width = 255;
            // 
            // contextMenuDisplay
            // 
            contextMenuDisplay.Items.AddRange(new ToolStripItem[] { googleToolStripMenuItem, copyToClipboardToolStripMenuItem, tsSepListViewDeleteEntry, tSMItemListViewDeleteEntry });
            contextMenuDisplay.Name = "contextMenuDisplay";
            contextMenuDisplay.Size = new System.Drawing.Size(243, 76);
            contextMenuDisplay.Opening += ContextMenuDisplay_Opening;
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
            // tsSepListViewDeleteEntry
            // 
            tsSepListViewDeleteEntry.Name = "tsSepListViewDeleteEntry";
            tsSepListViewDeleteEntry.Size = new System.Drawing.Size(239, 6);
            // 
            // tSMItemListViewDeleteEntry
            // 
            tSMItemListViewDeleteEntry.Image = Properties.Resources.remove;
            tSMItemListViewDeleteEntry.ImageTransparentColor = System.Drawing.Color.White;
            tSMItemListViewDeleteEntry.Name = "tSMItemListViewDeleteEntry";
            tSMItemListViewDeleteEntry.ShortcutKeyDisplayString = "Del";
            tSMItemListViewDeleteEntry.Size = new System.Drawing.Size(242, 22);
            tSMItemListViewDeleteEntry.Text = "Remove Entry";
            tSMItemListViewDeleteEntry.Click += TSMItemListViewDeleteEntry_Click;
            // 
            // historyPanel
            // 
            historyPanel.BackColor = System.Drawing.SystemColors.Control;
            historyPanel.Controls.Add(historyExportButton);
            historyPanel.Controls.Add(histoyClearButton);
            historyPanel.Controls.Add(cbLogHistory);
            historyPanel.Dock = DockStyle.Bottom;
            historyPanel.Location = new System.Drawing.Point(0, 301);
            historyPanel.Name = "historyPanel";
            historyPanel.Size = new System.Drawing.Size(395, 36);
            historyPanel.TabIndex = 0;
            // 
            // historyExportButton
            // 
            historyExportButton.Enabled = false;
            historyExportButton.Location = new System.Drawing.Point(279, 4);
            historyExportButton.Name = "historyExportButton";
            historyExportButton.Size = new System.Drawing.Size(115, 29);
            historyExportButton.TabIndex = 4;
            historyExportButton.Text = "Export to CSV";
            historyExportButton.UseVisualStyleBackColor = true;
            historyExportButton.Click += HistoryExportButton_Click;
            // 
            // histoyClearButton
            // 
            histoyClearButton.Enabled = false;
            histoyClearButton.Location = new System.Drawing.Point(158, 4);
            histoyClearButton.Name = "histoyClearButton";
            histoyClearButton.Size = new System.Drawing.Size(115, 29);
            histoyClearButton.TabIndex = 3;
            histoyClearButton.Text = "Clear all entries";
            histoyClearButton.UseVisualStyleBackColor = true;
            histoyClearButton.Click += HistoyClearButton_Click;
            // 
            // cbLogHistory
            // 
            cbLogHistory.AutoSize = true;
            cbLogHistory.Checked = true;
            cbLogHistory.CheckState = CheckState.Checked;
            cbLogHistory.Location = new System.Drawing.Point(8, 8);
            cbLogHistory.Name = "cbLogHistory";
            cbLogHistory.Size = new System.Drawing.Size(131, 23);
            cbLogHistory.TabIndex = 2;
            cbLogHistory.Text = "Log song history";
            cbLogHistory.UseVisualStyleBackColor = true;
            cbLogHistory.CheckedChanged += CbLogHistory_CheckedChanged;
            // 
            // tpSettings
            // 
            tpSettings.Controls.Add(panel1);
            tpSettings.ImageIndex = 3;
            tpSettings.Location = new System.Drawing.Point(4, 29);
            tpSettings.Margin = new Padding(4, 3, 4, 3);
            tpSettings.Name = "tpSettings";
            tpSettings.Size = new System.Drawing.Size(395, 337);
            tpSettings.TabIndex = 3;
            tpSettings.ToolTipText = "Settings (F8)";
            tpSettings.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            panel1.Controls.Add(rbStartModeTray);
            panel1.Controls.Add(rbStartModeMini);
            panel1.Controls.Add(rbStartModeMain);
            panel1.Controls.Add(labelStartMode);
            panel1.Controls.Add(gbAutoRecord);
            panel1.Controls.Add(gbOutput);
            panel1.Controls.Add(gbMiscel);
            panel1.Controls.Add(gbAutostart);
            panel1.Controls.Add(gbPreselection);
            panel1.Controls.Add(gbHotkeys);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(395, 337);
            panel1.TabIndex = 3;
            // 
            // rbStartModeTray
            // 
            rbStartModeTray.AutoSize = true;
            rbStartModeTray.Location = new System.Drawing.Point(298, 69);
            rbStartModeTray.Name = "rbStartModeTray";
            rbStartModeTray.Size = new System.Drawing.Size(91, 23);
            rbStartModeTray.TabIndex = 10;
            rbStartModeTray.Text = "Tray mode";
            rbStartModeTray.UseVisualStyleBackColor = true;
            rbStartModeTray.CheckedChanged += RbStartMode_CheckedChanged;
            // 
            // rbStartModeMini
            // 
            rbStartModeMini.AutoSize = true;
            rbStartModeMini.Location = new System.Drawing.Point(204, 69);
            rbStartModeMini.Name = "rbStartModeMini";
            rbStartModeMini.Size = new System.Drawing.Size(91, 23);
            rbStartModeMini.TabIndex = 9;
            rbStartModeMini.Text = "Miniplayer";
            rbStartModeMini.UseVisualStyleBackColor = true;
            rbStartModeMini.CheckedChanged += RbStartMode_CheckedChanged;
            // 
            // rbStartModeMain
            // 
            rbStartModeMain.AutoSize = true;
            rbStartModeMain.Checked = true;
            rbStartModeMain.Location = new System.Drawing.Point(92, 69);
            rbStartModeMain.Name = "rbStartModeMain";
            rbStartModeMain.Size = new System.Drawing.Size(109, 23);
            rbStartModeMain.TabIndex = 8;
            rbStartModeMain.TabStop = true;
            rbStartModeMain.Text = "Main window";
            rbStartModeMain.UseVisualStyleBackColor = true;
            rbStartModeMain.CheckedChanged += RbStartMode_CheckedChanged;
            // 
            // labelStartMode
            // 
            labelStartMode.AutoSize = true;
            labelStartMode.Location = new System.Drawing.Point(9, 71);
            labelStartMode.Name = "labelStartMode";
            labelStartMode.Size = new System.Drawing.Size(80, 19);
            labelStartMode.TabIndex = 7;
            labelStartMode.Text = "Start mode:";
            // 
            // gbAutoRecord
            // 
            gbAutoRecord.Controls.Add(cbActions);
            gbAutoRecord.Controls.Add(btnActions);
            gbAutoRecord.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            gbAutoRecord.Location = new System.Drawing.Point(266, 10);
            gbAutoRecord.Margin = new Padding(4, 3, 4, 3);
            gbAutoRecord.Name = "gbAutoRecord";
            gbAutoRecord.Padding = new Padding(4, 3, 4, 3);
            gbAutoRecord.Size = new System.Drawing.Size(121, 50);
            gbAutoRecord.TabIndex = 6;
            gbAutoRecord.TabStop = false;
            gbAutoRecord.Text = "Scheduled tasks";
            // 
            // cbActions
            // 
            cbActions.AutoSize = true;
            cbActions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cbActions.Location = new System.Drawing.Point(8, 25);
            cbActions.Margin = new Padding(4, 3, 4, 3);
            cbActions.Name = "cbActions";
            cbActions.Size = new System.Drawing.Size(15, 14);
            cbActions.TabIndex = 1;
            cbActions.UseVisualStyleBackColor = true;
            cbActions.CheckedChanged += CbActions_CheckedChanged;
            // 
            // btnActions
            // 
            btnActions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnActions.Location = new System.Drawing.Point(30, 18);
            btnActions.Name = "btnActions";
            btnActions.Size = new System.Drawing.Size(84, 26);
            btnActions.TabIndex = 0;
            btnActions.Text = "Settings…";
            btnActions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            btnActions.UseVisualStyleBackColor = true;
            btnActions.Click += BtnActions_Click;
            // 
            // gbOutput
            // 
            gbOutput.Controls.Add(cmbxOutput);
            gbOutput.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            gbOutput.Location = new System.Drawing.Point(9, 286);
            gbOutput.Margin = new Padding(4, 3, 4, 3);
            gbOutput.Name = "gbOutput";
            gbOutput.Padding = new Padding(4, 3, 4, 3);
            gbOutput.Size = new System.Drawing.Size(378, 49);
            gbOutput.TabIndex = 5;
            gbOutput.TabStop = false;
            gbOutput.Text = "Output device";
            // 
            // cmbxOutput
            // 
            cmbxOutput.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbxOutput.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cmbxOutput.FormattingEnabled = true;
            cmbxOutput.Location = new System.Drawing.Point(8, 18);
            cmbxOutput.Margin = new Padding(4, 3, 4, 3);
            cmbxOutput.Name = "cmbxOutput";
            cmbxOutput.Size = new System.Drawing.Size(362, 25);
            cmbxOutput.TabIndex = 4;
            cmbxOutput.DropDown += CmbxOutput_DropDown;
            cmbxOutput.SelectedIndexChanged += CmbxOutput_SelectedIndexChanged;
            cmbxOutput.DropDownClosed += CmbxOutput_DropDownClosed;
            // 
            // gbMiscel
            // 
            gbMiscel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            gbMiscel.Controls.Add(cbClose2Tray);
            gbMiscel.Controls.Add(cbAutoStopRecording);
            gbMiscel.Controls.Add(cbShowBalloonTip);
            gbMiscel.Controls.Add(cbAlwaysOnTop);
            gbMiscel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            gbMiscel.Location = new System.Drawing.Point(9, 159);
            gbMiscel.Margin = new Padding(4, 3, 4, 3);
            gbMiscel.Name = "gbMiscel";
            gbMiscel.Padding = new Padding(4, 3, 4, 3);
            gbMiscel.Size = new System.Drawing.Size(379, 121);
            gbMiscel.TabIndex = 2;
            gbMiscel.TabStop = false;
            gbMiscel.Text = "Miscellaneous";
            // 
            // cbClose2Tray
            // 
            cbClose2Tray.AutoSize = true;
            cbClose2Tray.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cbClose2Tray.Location = new System.Drawing.Point(9, 45);
            cbClose2Tray.Margin = new Padding(4, 3, 4, 3);
            cbClose2Tray.Name = "cbClose2Tray";
            cbClose2Tray.Size = new System.Drawing.Size(355, 23);
            cbClose2Tray.TabIndex = 4;
            cbClose2Tray.Text = "Close button minimizes to tray instead of terminating";
            cbClose2Tray.UseVisualStyleBackColor = true;
            cbClose2Tray.CheckedChanged += CbClose2Tray_CheckedChanged;
            // 
            // cbAutoStopRecording
            // 
            cbAutoStopRecording.AutoSize = true;
            cbAutoStopRecording.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cbAutoStopRecording.Location = new System.Drawing.Point(9, 93);
            cbAutoStopRecording.Margin = new Padding(4, 3, 4, 3);
            cbAutoStopRecording.Name = "cbAutoStopRecording";
            cbAutoStopRecording.Size = new System.Drawing.Size(329, 23);
            cbAutoStopRecording.TabIndex = 3;
            cbAutoStopRecording.Text = "Automatically stop recording when song changes";
            cbAutoStopRecording.UseVisualStyleBackColor = true;
            cbAutoStopRecording.CheckedChanged += CbAutoStopRecording_CheckedChanged;
            // 
            // cbShowBalloonTip
            // 
            cbShowBalloonTip.AutoSize = true;
            cbShowBalloonTip.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cbShowBalloonTip.Location = new System.Drawing.Point(9, 69);
            cbShowBalloonTip.Margin = new Padding(4, 3, 4, 3);
            cbShowBalloonTip.Name = "cbShowBalloonTip";
            cbShowBalloonTip.Size = new System.Drawing.Size(318, 23);
            cbShowBalloonTip.TabIndex = 2;
            cbShowBalloonTip.Text = "Display song title as a balloon tip in the taskbar";
            cbShowBalloonTip.UseVisualStyleBackColor = true;
            cbShowBalloonTip.CheckedChanged += CbShowBalloonTip_CheckedChanged;
            // 
            // cbAlwaysOnTop
            // 
            cbAlwaysOnTop.AutoSize = true;
            cbAlwaysOnTop.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cbAlwaysOnTop.Location = new System.Drawing.Point(9, 21);
            cbAlwaysOnTop.Margin = new Padding(4, 3, 4, 3);
            cbAlwaysOnTop.Name = "cbAlwaysOnTop";
            cbAlwaysOnTop.Size = new System.Drawing.Size(270, 23);
            cbAlwaysOnTop.TabIndex = 2;
            cbAlwaysOnTop.Text = "Stay always on top of all other windows";
            cbAlwaysOnTop.UseVisualStyleBackColor = true;
            cbAlwaysOnTop.CheckedChanged += CbAlwaysOnTop_CheckedChanged;
            // 
            // gbAutostart
            // 
            gbAutostart.Controls.Add(cbAutostart);
            gbAutostart.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            gbAutostart.Location = new System.Drawing.Point(141, 10);
            gbAutostart.Margin = new Padding(4, 3, 4, 3);
            gbAutostart.Name = "gbAutostart";
            gbAutostart.Padding = new Padding(4, 3, 4, 3);
            gbAutostart.Size = new System.Drawing.Size(116, 50);
            gbAutostart.TabIndex = 0;
            gbAutostart.TabStop = false;
            gbAutostart.Text = "Autostart";
            // 
            // cbAutostart
            // 
            cbAutostart.AutoSize = true;
            cbAutostart.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cbAutostart.Location = new System.Drawing.Point(8, 21);
            cbAutostart.Margin = new Padding(4, 3, 4, 3);
            cbAutostart.Name = "cbAutostart";
            cbAutostart.Size = new System.Drawing.Size(106, 23);
            cbAutostart.TabIndex = 0;
            cbAutostart.Text = "At user login";
            cbAutostart.UseVisualStyleBackColor = true;
            cbAutostart.CheckedChanged += CbAutostart_CheckedChanged;
            // 
            // gbPreselection
            // 
            gbPreselection.BackColor = System.Drawing.SystemColors.ControlLightLight;
            gbPreselection.Controls.Add(lblAutostartStation);
            gbPreselection.Controls.Add(cmbxStation);
            gbPreselection.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            gbPreselection.Location = new System.Drawing.Point(9, 10);
            gbPreselection.Margin = new Padding(4, 3, 4, 3);
            gbPreselection.Name = "gbPreselection";
            gbPreselection.Padding = new Padding(4, 3, 4, 3);
            gbPreselection.Size = new System.Drawing.Size(123, 50);
            gbPreselection.TabIndex = 0;
            gbPreselection.TabStop = false;
            gbPreselection.Text = "Autoplay";
            // 
            // lblAutostartStation
            // 
            lblAutostartStation.AutoSize = true;
            lblAutostartStation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblAutostartStation.Location = new System.Drawing.Point(6, 22);
            lblAutostartStation.Margin = new Padding(4, 0, 4, 0);
            lblAutostartStation.Name = "lblAutostartStation";
            lblAutostartStation.Size = new System.Drawing.Size(55, 19);
            lblAutostartStation.TabIndex = 10;
            lblAutostartStation.Text = "Station:";
            // 
            // cmbxStation
            // 
            cmbxStation.BackColor = System.Drawing.SystemColors.ControlLightLight;
            cmbxStation.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbxStation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cmbxStation.FormattingEnabled = true;
            cmbxStation.Items.AddRange(new object[] { "", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25" });
            cmbxStation.Location = new System.Drawing.Point(69, 19);
            cmbxStation.Margin = new Padding(4, 3, 4, 3);
            cmbxStation.MaxLength = 1;
            cmbxStation.Name = "cmbxStation";
            cmbxStation.Size = new System.Drawing.Size(45, 25);
            cmbxStation.TabIndex = 9;
            cmbxStation.SelectedIndexChanged += CmbxStation_SelectedIndexChanged;
            // 
            // gbHotkeys
            // 
            gbHotkeys.BackColor = System.Drawing.SystemColors.ControlLightLight;
            gbHotkeys.Controls.Add(lblHotkey);
            gbHotkeys.Controls.Add(cmbxHotkey);
            gbHotkeys.Controls.Add(cbHotkey);
            gbHotkeys.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            gbHotkeys.Location = new System.Drawing.Point(9, 103);
            gbHotkeys.Margin = new Padding(4, 3, 4, 3);
            gbHotkeys.Name = "gbHotkeys";
            gbHotkeys.Padding = new Padding(4, 3, 4, 3);
            gbHotkeys.Size = new System.Drawing.Size(379, 50);
            gbHotkeys.TabIndex = 1;
            gbHotkeys.TabStop = false;
            gbHotkeys.Text = "Global hotkey";
            // 
            // lblHotkey
            // 
            lblHotkey.AutoSize = true;
            lblHotkey.Enabled = false;
            lblHotkey.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblHotkey.Location = new System.Drawing.Point(176, 22);
            lblHotkey.Margin = new Padding(4, 0, 4, 0);
            lblHotkey.Name = "lblHotkey";
            lblHotkey.Size = new System.Drawing.Size(83, 19);
            lblHotkey.TabIndex = 7;
            lblHotkey.Text = "Ctrl +Win +";
            // 
            // cmbxHotkey
            // 
            cmbxHotkey.BackColor = System.Drawing.SystemColors.ControlLightLight;
            cmbxHotkey.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbxHotkey.Enabled = false;
            cmbxHotkey.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cmbxHotkey.FormattingEnabled = true;
            cmbxHotkey.Items.AddRange(new object[] { "", "A", "B", "C", "D", "E", "G", "H", "I", "J", "K", "L", "M", "N", "O", "Q", "R", "S", "T", "V", "W", "X", "Y", "Z" });
            cmbxHotkey.Location = new System.Drawing.Point(264, 18);
            cmbxHotkey.Margin = new Padding(4, 3, 4, 3);
            cmbxHotkey.MaxLength = 1;
            cmbxHotkey.Name = "cmbxHotkey";
            cmbxHotkey.Size = new System.Drawing.Size(58, 25);
            cmbxHotkey.TabIndex = 6;
            cmbxHotkey.SelectedIndexChanged += CmbxHotkey_SelectedIndexChanged;
            cmbxHotkey.Leave += CmbxHotkey_Leave;
            // 
            // cbHotkey
            // 
            cbHotkey.AutoSize = true;
            cbHotkey.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            cbHotkey.Location = new System.Drawing.Point(8, 20);
            cbHotkey.Margin = new Padding(4, 3, 4, 3);
            cbHotkey.Name = "cbHotkey";
            cbHotkey.Size = new System.Drawing.Size(158, 23);
            cbHotkey.TabIndex = 0;
            cbHotkey.Text = "Use system wide key:";
            toolTip.SetToolTip(cbHotkey, "Brings this window to front. Switches between miniplayer\r\nand main gui. Double key press terminates application.");
            cbHotkey.UseVisualStyleBackColor = true;
            cbHotkey.CheckedChanged += CbHotkey_CheckedChanged;
            // 
            // tpHelp
            // 
            tpHelp.Controls.Add(tableLayoutPanel0);
            tpHelp.Controls.Add(panelHelp);
            tpHelp.ImageIndex = 4;
            tpHelp.Location = new System.Drawing.Point(4, 29);
            tpHelp.Margin = new Padding(4, 3, 4, 3);
            tpHelp.Name = "tpHelp";
            tpHelp.Size = new System.Drawing.Size(395, 337);
            tpHelp.TabIndex = 4;
            tpHelp.ToolTipText = "Help (F9)";
            tpHelp.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel0
            // 
            tableLayoutPanel0.BackColor = System.Drawing.Color.Gainsboro;
            tableLayoutPanel0.ColumnCount = 2;
            tableLayoutPanel0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            tableLayoutPanel0.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            tableLayoutPanel0.Controls.Add(lplTPLHelp2, 1, 0);
            tableLayoutPanel0.Controls.Add(lplTPLHelp1, 0, 0);
            tableLayoutPanel0.Location = new System.Drawing.Point(9, 12);
            tableLayoutPanel0.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel0.Name = "tableLayoutPanel0";
            tableLayoutPanel0.RowCount = 1;
            tableLayoutPanel0.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel0.Size = new System.Drawing.Size(379, 24);
            tableLayoutPanel0.TabIndex = 2;
            // 
            // lplTPLHelp2
            // 
            lplTPLHelp2.AutoSize = true;
            lplTPLHelp2.Dock = DockStyle.Left;
            lplTPLHelp2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lplTPLHelp2.Location = new System.Drawing.Point(220, 0);
            lplTPLHelp2.Margin = new Padding(4, 0, 4, 0);
            lplTPLHelp2.Name = "lplTPLHelp2";
            lplTPLHelp2.Size = new System.Drawing.Size(60, 24);
            lplTPLHelp2.TabIndex = 1;
            lplTPLHelp2.Text = "Shortcut";
            lplTPLHelp2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lplTPLHelp1
            // 
            lplTPLHelp1.AutoSize = true;
            lplTPLHelp1.Dock = DockStyle.Left;
            lplTPLHelp1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            lplTPLHelp1.Location = new System.Drawing.Point(4, 0);
            lplTPLHelp1.Margin = new Padding(4, 0, 4, 0);
            lplTPLHelp1.Name = "lplTPLHelp1";
            lplTPLHelp1.Size = new System.Drawing.Size(48, 24);
            lplTPLHelp1.TabIndex = 0;
            lplTPLHelp1.Text = "Action";
            lplTPLHelp1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelHelp
            // 
            panelHelp.BackColor = System.Drawing.SystemColors.ControlLightLight;
            panelHelp.Controls.Add(labelBrackets);
            panelHelp.Controls.Add(tableLayoutPanel1);
            panelHelp.Controls.Add(label6);
            panelHelp.Controls.Add(label5);
            panelHelp.Controls.Add(tableLayoutPanel2);
            panelHelp.Dock = DockStyle.Fill;
            panelHelp.Location = new System.Drawing.Point(0, 0);
            panelHelp.Margin = new Padding(4, 3, 4, 3);
            panelHelp.Name = "panelHelp";
            panelHelp.Size = new System.Drawing.Size(395, 337);
            panelHelp.TabIndex = 7;
            // 
            // labelBrackets
            // 
            labelBrackets.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            labelBrackets.Location = new System.Drawing.Point(13, 285);
            labelBrackets.Margin = new Padding(4, 0, 4, 0);
            labelBrackets.Name = "labelBrackets";
            labelBrackets.Size = new System.Drawing.Size(372, 52);
            labelBrackets.TabIndex = 7;
            labelBrackets.Text = "Use the Esc key to switch between main and miniplayer.\r\nEsc + Shift terminates (even if \"Close to Tray\" is active).";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            tableLayoutPanel1.Controls.Add(label1, 1, 0);
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Location = new System.Drawing.Point(9, 60);
            tableLayoutPanel1.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new System.Drawing.Size(379, 108);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label1.Location = new System.Drawing.Point(220, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(68, 102);
            label1.TabIndex = 1;
            label1.Text = "Space\r\n+/-\r\n1-9\r\nBackspace\r\nInsert\r\nF2";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label2.Location = new System.Drawing.Point(4, 0);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(107, 102);
            label2.TabIndex = 0;
            label2.Text = "Play/Pause\r\nChange volume\r\nSelect station\r\nReset connection\r\nRecord stream\r\nEdit button text";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.BackColor = System.Drawing.SystemColors.ControlLightLight;
            label6.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label6.Location = new System.Drawing.Point(13, 176);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(119, 17);
            label6.TabIndex = 6;
            label6.Text = "Stations shortcuts";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = System.Drawing.SystemColors.ControlLightLight;
            label5.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            label5.Location = new System.Drawing.Point(13, 43);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(107, 17);
            label5.TabIndex = 5;
            label5.Text = "Player shortcuts";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = System.Drawing.SystemColors.ControlLightLight;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43F));
            tableLayoutPanel2.Controls.Add(label4, 0, 0);
            tableLayoutPanel2.Controls.Add(label3, 1, 0);
            tableLayoutPanel2.Location = new System.Drawing.Point(9, 193);
            tableLayoutPanel2.Margin = new Padding(4, 3, 4, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 9F));
            tableLayoutPanel2.Size = new System.Drawing.Size(379, 80);
            tableLayoutPanel2.TabIndex = 4;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = System.Drawing.Color.Transparent;
            label4.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label4.Location = new System.Drawing.Point(4, 0);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(161, 68);
            label4.TabIndex = 0;
            label4.Text = "Current row to position n\r\nMove row up/down\r\nMove row far up/down\r\nMove row to new position";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = System.Drawing.Color.Transparent;
            label3.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label3.Location = new System.Drawing.Point(220, 0);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(135, 68);
            label3.TabIndex = 1;
            label3.Text = "Alt and 1-9\r\nAlt and Up/Down\r\nAlt and PgUp/PgDn\r\nDrag 'n drop (mouse)";
            // 
            // tpInfo
            // 
            tpInfo.Controls.Add(btnUpdateSettings);
            tpInfo.Controls.Add(linkLabeGNU);
            tpInfo.Controls.Add(label10);
            tpInfo.Controls.Add(label9);
            tpInfo.Controls.Add(linkLabel1);
            tpInfo.Controls.Add(lblAuthor);
            tpInfo.Controls.Add(progressBar);
            tpInfo.Controls.Add(lblUpdate);
            tpInfo.Controls.Add(picBoxPayPal);
            tpInfo.Controls.Add(btnUpdate);
            tpInfo.Controls.Add(label8);
            tpInfo.Controls.Add(label7);
            tpInfo.Controls.Add(lblCredits);
            tpInfo.Controls.Add(lblHorizontalLine);
            tpInfo.Controls.Add(lblCopyright);
            tpInfo.Controls.Add(lblWebService);
            tpInfo.Controls.Add(linkHomepage);
            tpInfo.Controls.Add(linkPayPal);
            tpInfo.Controls.Add(lblInformation);
            tpInfo.ImageIndex = 5;
            tpInfo.Location = new System.Drawing.Point(4, 29);
            tpInfo.Margin = new Padding(4, 3, 4, 3);
            tpInfo.Name = "tpInfo";
            tpInfo.Size = new System.Drawing.Size(395, 337);
            tpInfo.TabIndex = 2;
            tpInfo.ToolTipText = "Information (F11)";
            tpInfo.UseVisualStyleBackColor = true;
            // 
            // btnUpdateSettings
            // 
            btnUpdateSettings.ImageIndex = 3;
            btnUpdateSettings.ImageList = imageList;
            btnUpdateSettings.Location = new System.Drawing.Point(356, 307);
            btnUpdateSettings.Name = "btnUpdateSettings";
            btnUpdateSettings.Size = new System.Drawing.Size(32, 27);
            btnUpdateSettings.TabIndex = 21;
            toolTip.SetToolTip(btnUpdateSettings, "Update Notification");
            btnUpdateSettings.UseVisualStyleBackColor = true;
            btnUpdateSettings.Click += BtnUpdateSettings_Click;
            // 
            // imageList
            // 
            imageList.ColorDepth = ColorDepth.Depth8Bit;
            imageList.ImageStream = (ImageListStreamer)resources.GetObject("imageList.ImageStream");
            imageList.TransparentColor = System.Drawing.Color.Transparent;
            imageList.Images.SetKeyName(0, "18_home.png");
            imageList.Images.SetKeyName(1, "18_edit.png");
            imageList.Images.SetKeyName(2, "18_history.png");
            imageList.Images.SetKeyName(3, "18_settings.png");
            imageList.Images.SetKeyName(4, "18_help.png");
            imageList.Images.SetKeyName(5, "18_info.png");
            imageList.Images.SetKeyName(6, "18_graphic.png");
            imageList.Images.SetKeyName(7, "18_launch.png");
            // 
            // linkLabeGNU
            // 
            linkLabeGNU.AutoSize = true;
            linkLabeGNU.BackColor = System.Drawing.SystemColors.ControlLightLight;
            linkLabeGNU.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            linkLabeGNU.Location = new System.Drawing.Point(303, 7);
            linkLabeGNU.Name = "linkLabeGNU";
            linkLabeGNU.Size = new System.Drawing.Size(36, 17);
            linkLabeGNU.TabIndex = 20;
            linkLabeGNU.TabStop = true;
            linkLabeGNU.Text = "GNU";
            linkLabeGNU.LinkClicked += LinkLabeGNU_LinkClicked;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.BackColor = System.Drawing.SystemColors.ControlLightLight;
            label10.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label10.Location = new System.Drawing.Point(8, 7);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(300, 34);
            label10.TabIndex = 19;
            label10.Text = "The program is distributed under the terms of the\r\nGeneral Public License version 3 or later.";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.BackColor = System.Drawing.SystemColors.ControlLightLight;
            label9.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label9.Location = new System.Drawing.Point(8, 173);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(87, 17);
            label9.TabIndex = 18;
            label9.Text = "Open Source:";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            linkLabel1.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            linkLabel1.Location = new System.Drawing.Point(144, 173);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new System.Drawing.Size(207, 17);
            linkLabel1.TabIndex = 17;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "github.com/ophthalmos/NetRadio";
            linkLabel1.LinkClicked += LinkLabel1_LinkClicked;
            // 
            // lblAuthor
            // 
            lblAuthor.AutoSize = true;
            lblAuthor.BackColor = System.Drawing.SystemColors.ControlLightLight;
            lblAuthor.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblAuthor.Location = new System.Drawing.Point(144, 194);
            lblAuthor.Name = "lblAuthor";
            lblAuthor.Size = new System.Drawing.Size(184, 17);
            lblAuthor.TabIndex = 16;
            lblAuthor.Text = "Wilhelm Happe, Kiel, Germany";
            // 
            // progressBar
            // 
            progressBar.Location = new System.Drawing.Point(180, 307);
            progressBar.Margin = new Padding(4, 3, 4, 3);
            progressBar.Name = "progressBar";
            progressBar.Size = new System.Drawing.Size(170, 27);
            progressBar.TabIndex = 15;
            progressBar.Visible = false;
            // 
            // lblUpdate
            // 
            lblUpdate.BackColor = System.Drawing.Color.White;
            lblUpdate.Location = new System.Drawing.Point(180, 311);
            lblUpdate.Margin = new Padding(4, 0, 4, 0);
            lblUpdate.Name = "lblUpdate";
            lblUpdate.Size = new System.Drawing.Size(171, 22);
            lblUpdate.TabIndex = 14;
            lblUpdate.Text = "Installed version:";
            // 
            // picBoxPayPal
            // 
            picBoxPayPal.BackColor = System.Drawing.SystemColors.ControlLightLight;
            picBoxPayPal.Image = (System.Drawing.Image)resources.GetObject("picBoxPayPal.Image");
            picBoxPayPal.Location = new System.Drawing.Point(144, 85);
            picBoxPayPal.Margin = new Padding(4, 3, 4, 3);
            picBoxPayPal.Name = "picBoxPayPal";
            picBoxPayPal.Size = new System.Drawing.Size(117, 57);
            picBoxPayPal.TabIndex = 7;
            picBoxPayPal.TabStop = false;
            picBoxPayPal.Click += PicBoxPayPal_Click;
            picBoxPayPal.MouseEnter += PicBoxPayPal_MouseEnter;
            picBoxPayPal.MouseLeave += PicBoxPayPal_MouseLeave;
            // 
            // btnUpdate
            // 
            btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            btnUpdate.Location = new System.Drawing.Point(6, 307);
            btnUpdate.Margin = new Padding(4, 3, 4, 3);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new System.Drawing.Size(170, 27);
            btnUpdate.TabIndex = 13;
            btnUpdate.Text = "Check for updates";
            btnUpdate.UseMnemonic = false;
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += BtnUpdate_Click;
            btnUpdate.Paint += BtnUpdate_Paint;
            // 
            // label8
            // 
            label8.BorderStyle = BorderStyle.Fixed3D;
            label8.Location = new System.Drawing.Point(6, 218);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(383, 2);
            label8.TabIndex = 12;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = System.Drawing.SystemColors.ControlLightLight;
            label7.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label7.Location = new System.Drawing.Point(8, 45);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(331, 34);
            label7.TabIndex = 11;
            label7.Text = "Developing high quality applications takes a lot of time.\r\nYou can show your appreciation by making a donation.";
            // 
            // lblCredits
            // 
            lblCredits.BackColor = System.Drawing.SystemColors.ControlLightLight;
            lblCredits.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblCredits.Location = new System.Drawing.Point(8, 223);
            lblCredits.Name = "lblCredits";
            lblCredits.Size = new System.Drawing.Size(388, 72);
            lblCredits.TabIndex = 10;
            lblCredits.Text = "Acknowledgments\r\nNetRadio uses libraries for streaming and audio playback:\r\nbass.dll © 1999-2022, Un4seen Developments Ltd.\r\nbass.net.dll © 2005-2022 by radio42, Hamburg, Germany";
            // 
            // lblHorizontalLine
            // 
            lblHorizontalLine.BorderStyle = BorderStyle.Fixed3D;
            lblHorizontalLine.Location = new System.Drawing.Point(6, 298);
            lblHorizontalLine.Margin = new Padding(4, 0, 4, 0);
            lblHorizontalLine.Name = "lblHorizontalLine";
            lblHorizontalLine.Size = new System.Drawing.Size(383, 2);
            lblHorizontalLine.TabIndex = 9;
            // 
            // lblCopyright
            // 
            lblCopyright.AutoSize = true;
            lblCopyright.BackColor = System.Drawing.SystemColors.ControlLightLight;
            lblCopyright.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblCopyright.Location = new System.Drawing.Point(8, 194);
            lblCopyright.Name = "lblCopyright";
            lblCopyright.Size = new System.Drawing.Size(112, 17);
            lblCopyright.TabIndex = 8;
            lblCopyright.Text = "Author/Copyright:";
            // 
            // lblWebService
            // 
            lblWebService.AutoSize = true;
            lblWebService.BackColor = System.Drawing.SystemColors.ControlLightLight;
            lblWebService.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblWebService.Location = new System.Drawing.Point(8, 152);
            lblWebService.Name = "lblWebService";
            lblWebService.Size = new System.Drawing.Size(101, 17);
            lblWebService.TabIndex = 5;
            lblWebService.Text = "Project Website:";
            // 
            // linkHomepage
            // 
            linkHomepage.AutoSize = true;
            linkHomepage.BackColor = System.Drawing.SystemColors.ControlLightLight;
            linkHomepage.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            linkHomepage.Location = new System.Drawing.Point(144, 152);
            linkHomepage.Name = "linkHomepage";
            linkHomepage.Size = new System.Drawing.Size(197, 17);
            linkHomepage.TabIndex = 2;
            linkHomepage.TabStop = true;
            linkHomepage.Text = "www.ophthalmostar.de/freeware";
            linkHomepage.LinkClicked += LinkHomepage_LinkClicked;
            // 
            // linkPayPal
            // 
            linkPayPal.AutoSize = true;
            linkPayPal.BackColor = System.Drawing.SystemColors.ControlLightLight;
            linkPayPal.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            linkPayPal.Location = new System.Drawing.Point(8, 89);
            linkPayPal.Name = "linkPayPal";
            linkPayPal.Size = new System.Drawing.Size(121, 17);
            linkPayPal.TabIndex = 1;
            linkPayPal.TabStop = true;
            linkPayPal.Text = "Donate with PayPal:";
            linkPayPal.LinkClicked += LinkPayPal_LinkClicked;
            // 
            // lblInformation
            // 
            lblInformation.BackColor = System.Drawing.SystemColors.ControlLightLight;
            lblInformation.Dock = DockStyle.Fill;
            lblInformation.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            lblInformation.Location = new System.Drawing.Point(0, 0);
            lblInformation.Name = "lblInformation";
            lblInformation.Size = new System.Drawing.Size(395, 337);
            lblInformation.TabIndex = 0;
            // 
            // tpSectrum
            // 
            tpSectrum.Controls.Add(Lbl19);
            tpSectrum.Controls.Add(Lbl18);
            tpSectrum.Controls.Add(Lbl17);
            tpSectrum.Controls.Add(Lbl16);
            tpSectrum.Controls.Add(Lbl15);
            tpSectrum.Controls.Add(Lbl14);
            tpSectrum.Controls.Add(Lbl13);
            tpSectrum.Controls.Add(Lbl12);
            tpSectrum.Controls.Add(Lbl11);
            tpSectrum.Controls.Add(Lbl10);
            tpSectrum.Controls.Add(Lbl09);
            tpSectrum.Controls.Add(Lbl08);
            tpSectrum.Controls.Add(Lbl07);
            tpSectrum.Controls.Add(Lbl06);
            tpSectrum.Controls.Add(Lbl05);
            tpSectrum.Controls.Add(Lbl04);
            tpSectrum.Controls.Add(Lbl03);
            tpSectrum.Controls.Add(Lbl02);
            tpSectrum.Controls.Add(Lbl01);
            tpSectrum.Controls.Add(Lbl00);
            tpSectrum.Controls.Add(spectrumPanel);
            tpSectrum.ImageIndex = 6;
            tpSectrum.Location = new System.Drawing.Point(4, 29);
            tpSectrum.Name = "tpSectrum";
            tpSectrum.Size = new System.Drawing.Size(395, 337);
            tpSectrum.TabIndex = 6;
            tpSectrum.ToolTipText = "Visualization (F12)";
            tpSectrum.UseVisualStyleBackColor = true;
            // 
            // Lbl19
            // 
            Lbl19.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl19.Location = new System.Drawing.Point(369, 323);
            Lbl19.Margin = new Padding(0);
            Lbl19.Name = "Lbl19";
            Lbl19.Size = new System.Drawing.Size(19, 15);
            Lbl19.TabIndex = 52;
            Lbl19.Text = "20K";
            Lbl19.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl18
            // 
            Lbl18.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl18.Location = new System.Drawing.Point(350, 323);
            Lbl18.Margin = new Padding(0);
            Lbl18.Name = "Lbl18";
            Lbl18.Size = new System.Drawing.Size(19, 15);
            Lbl18.TabIndex = 51;
            Lbl18.Text = "14K";
            Lbl18.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl17
            // 
            Lbl17.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl17.Location = new System.Drawing.Point(331, 323);
            Lbl17.Margin = new Padding(0);
            Lbl17.Name = "Lbl17";
            Lbl17.Size = new System.Drawing.Size(19, 15);
            Lbl17.TabIndex = 50;
            Lbl17.Text = "11K";
            Lbl17.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl16
            // 
            Lbl16.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl16.Location = new System.Drawing.Point(312, 323);
            Lbl16.Margin = new Padding(0);
            Lbl16.Name = "Lbl16";
            Lbl16.Size = new System.Drawing.Size(19, 15);
            Lbl16.TabIndex = 49;
            Lbl16.Text = "7.7K";
            Lbl16.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl15
            // 
            Lbl15.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl15.Location = new System.Drawing.Point(293, 323);
            Lbl15.Margin = new Padding(0);
            Lbl15.Name = "Lbl15";
            Lbl15.Size = new System.Drawing.Size(19, 15);
            Lbl15.TabIndex = 48;
            Lbl15.Text = "5.6K";
            Lbl15.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl14
            // 
            Lbl14.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl14.Location = new System.Drawing.Point(274, 323);
            Lbl14.Margin = new Padding(0);
            Lbl14.Name = "Lbl14";
            Lbl14.Size = new System.Drawing.Size(19, 15);
            Lbl14.TabIndex = 47;
            Lbl14.Text = "4.1K";
            Lbl14.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl13
            // 
            Lbl13.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl13.Location = new System.Drawing.Point(255, 323);
            Lbl13.Margin = new Padding(0);
            Lbl13.Name = "Lbl13";
            Lbl13.Size = new System.Drawing.Size(19, 15);
            Lbl13.TabIndex = 46;
            Lbl13.Text = "3.0K";
            Lbl13.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl12
            // 
            Lbl12.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl12.Location = new System.Drawing.Point(236, 323);
            Lbl12.Margin = new Padding(0);
            Lbl12.Name = "Lbl12";
            Lbl12.Size = new System.Drawing.Size(19, 15);
            Lbl12.TabIndex = 45;
            Lbl12.Text = "2.2K";
            Lbl12.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl11
            // 
            Lbl11.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl11.Location = new System.Drawing.Point(217, 323);
            Lbl11.Margin = new Padding(0);
            Lbl11.Name = "Lbl11";
            Lbl11.Size = new System.Drawing.Size(19, 15);
            Lbl11.TabIndex = 44;
            Lbl11.Text = "1.6K";
            Lbl11.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl10
            // 
            Lbl10.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl10.Location = new System.Drawing.Point(198, 323);
            Lbl10.Margin = new Padding(0);
            Lbl10.Name = "Lbl10";
            Lbl10.Size = new System.Drawing.Size(19, 15);
            Lbl10.TabIndex = 43;
            Lbl10.Text = "1.2K";
            Lbl10.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl09
            // 
            Lbl09.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl09.Location = new System.Drawing.Point(179, 323);
            Lbl09.Margin = new Padding(0);
            Lbl09.Name = "Lbl09";
            Lbl09.Size = new System.Drawing.Size(19, 15);
            Lbl09.TabIndex = 42;
            Lbl09.Text = "850";
            Lbl09.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl08
            // 
            Lbl08.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl08.Location = new System.Drawing.Point(160, 323);
            Lbl08.Margin = new Padding(0);
            Lbl08.Name = "Lbl08";
            Lbl08.Size = new System.Drawing.Size(19, 15);
            Lbl08.TabIndex = 41;
            Lbl08.Text = "620";
            Lbl08.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl07
            // 
            Lbl07.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl07.Location = new System.Drawing.Point(141, 323);
            Lbl07.Margin = new Padding(0);
            Lbl07.Name = "Lbl07";
            Lbl07.Size = new System.Drawing.Size(19, 15);
            Lbl07.TabIndex = 40;
            Lbl07.Text = "453";
            Lbl07.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl06
            // 
            Lbl06.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl06.Location = new System.Drawing.Point(122, 323);
            Lbl06.Margin = new Padding(0);
            Lbl06.Name = "Lbl06";
            Lbl06.Size = new System.Drawing.Size(19, 15);
            Lbl06.TabIndex = 39;
            Lbl06.Text = "331";
            Lbl06.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl05
            // 
            Lbl05.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl05.Location = new System.Drawing.Point(103, 323);
            Lbl05.Margin = new Padding(0);
            Lbl05.Name = "Lbl05";
            Lbl05.Size = new System.Drawing.Size(19, 15);
            Lbl05.TabIndex = 38;
            Lbl05.Text = "241";
            Lbl05.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl04
            // 
            Lbl04.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl04.Location = new System.Drawing.Point(84, 323);
            Lbl04.Margin = new Padding(0);
            Lbl04.Name = "Lbl04";
            Lbl04.Size = new System.Drawing.Size(19, 15);
            Lbl04.TabIndex = 37;
            Lbl04.Text = "176";
            Lbl04.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl03
            // 
            Lbl03.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl03.Location = new System.Drawing.Point(65, 323);
            Lbl03.Margin = new Padding(0);
            Lbl03.Name = "Lbl03";
            Lbl03.Size = new System.Drawing.Size(19, 15);
            Lbl03.TabIndex = 36;
            Lbl03.Text = "129";
            Lbl03.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl02
            // 
            Lbl02.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl02.Location = new System.Drawing.Point(46, 323);
            Lbl02.Margin = new Padding(0);
            Lbl02.Name = "Lbl02";
            Lbl02.Size = new System.Drawing.Size(19, 15);
            Lbl02.TabIndex = 35;
            Lbl02.Text = "94";
            Lbl02.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl01
            // 
            Lbl01.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl01.Location = new System.Drawing.Point(27, 323);
            Lbl01.Margin = new Padding(0);
            Lbl01.Name = "Lbl01";
            Lbl01.Size = new System.Drawing.Size(19, 15);
            Lbl01.TabIndex = 34;
            Lbl01.Text = "69";
            Lbl01.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Lbl00
            // 
            Lbl00.Font = new System.Drawing.Font("Segoe UI", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            Lbl00.Location = new System.Drawing.Point(8, 323);
            Lbl00.Margin = new Padding(0);
            Lbl00.Name = "Lbl00";
            Lbl00.Size = new System.Drawing.Size(19, 15);
            Lbl00.TabIndex = 33;
            Lbl00.Text = "50";
            Lbl00.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // spectrumPanel
            // 
            spectrumPanel.BackColor = System.Drawing.Color.AliceBlue;
            spectrumPanel.BorderStyle = BorderStyle.FixedSingle;
            spectrumPanel.Controls.Add(Bar01);
            spectrumPanel.Controls.Add(Bar02);
            spectrumPanel.Controls.Add(Bar03);
            spectrumPanel.Controls.Add(Bar04);
            spectrumPanel.Controls.Add(Bar05);
            spectrumPanel.Controls.Add(Bar06);
            spectrumPanel.Controls.Add(Bar07);
            spectrumPanel.Controls.Add(Bar08);
            spectrumPanel.Controls.Add(Bar09);
            spectrumPanel.Controls.Add(Bar10);
            spectrumPanel.Controls.Add(Bar11);
            spectrumPanel.Controls.Add(Bar12);
            spectrumPanel.Controls.Add(Bar13);
            spectrumPanel.Controls.Add(Bar14);
            spectrumPanel.Controls.Add(Bar15);
            spectrumPanel.Controls.Add(Bar16);
            spectrumPanel.Controls.Add(Bar17);
            spectrumPanel.Controls.Add(Bar18);
            spectrumPanel.Controls.Add(Bar19);
            spectrumPanel.Controls.Add(Bar20);
            spectrumPanel.Location = new System.Drawing.Point(6, 8);
            spectrumPanel.Name = "spectrumPanel";
            spectrumPanel.Size = new System.Drawing.Size(384, 312);
            spectrumPanel.TabIndex = 32;
            // 
            // Bar01
            // 
            Bar01.BackColor = System.Drawing.Color.AliceBlue;
            Bar01.Location = new System.Drawing.Point(1, 0);
            Bar01.Maximum = 255;
            Bar01.Name = "Bar01";
            Bar01.Size = new System.Drawing.Size(19, 312);
            Bar01.TabIndex = 0;
            // 
            // Bar02
            // 
            Bar02.BackColor = System.Drawing.Color.AliceBlue;
            Bar02.Location = new System.Drawing.Point(20, 0);
            Bar02.Maximum = 255;
            Bar02.Name = "Bar02";
            Bar02.Size = new System.Drawing.Size(19, 312);
            Bar02.TabIndex = 1;
            // 
            // Bar03
            // 
            Bar03.BackColor = System.Drawing.Color.AliceBlue;
            Bar03.Location = new System.Drawing.Point(39, 0);
            Bar03.Maximum = 255;
            Bar03.Name = "Bar03";
            Bar03.Size = new System.Drawing.Size(19, 312);
            Bar03.TabIndex = 2;
            // 
            // Bar04
            // 
            Bar04.BackColor = System.Drawing.Color.AliceBlue;
            Bar04.Location = new System.Drawing.Point(58, 0);
            Bar04.Maximum = 255;
            Bar04.Name = "Bar04";
            Bar04.Size = new System.Drawing.Size(19, 312);
            Bar04.TabIndex = 3;
            // 
            // Bar05
            // 
            Bar05.BackColor = System.Drawing.Color.AliceBlue;
            Bar05.Location = new System.Drawing.Point(77, 0);
            Bar05.Maximum = 255;
            Bar05.Name = "Bar05";
            Bar05.Size = new System.Drawing.Size(19, 312);
            Bar05.TabIndex = 4;
            // 
            // Bar06
            // 
            Bar06.BackColor = System.Drawing.Color.AliceBlue;
            Bar06.Location = new System.Drawing.Point(96, 0);
            Bar06.Maximum = 255;
            Bar06.Name = "Bar06";
            Bar06.Size = new System.Drawing.Size(19, 312);
            Bar06.TabIndex = 5;
            // 
            // Bar07
            // 
            Bar07.BackColor = System.Drawing.Color.AliceBlue;
            Bar07.Location = new System.Drawing.Point(115, 0);
            Bar07.Maximum = 255;
            Bar07.Name = "Bar07";
            Bar07.Size = new System.Drawing.Size(19, 312);
            Bar07.TabIndex = 6;
            // 
            // Bar08
            // 
            Bar08.BackColor = System.Drawing.Color.AliceBlue;
            Bar08.Location = new System.Drawing.Point(134, 0);
            Bar08.Maximum = 255;
            Bar08.Name = "Bar08";
            Bar08.Size = new System.Drawing.Size(19, 312);
            Bar08.TabIndex = 7;
            // 
            // Bar09
            // 
            Bar09.BackColor = System.Drawing.Color.AliceBlue;
            Bar09.Location = new System.Drawing.Point(153, 0);
            Bar09.Maximum = 255;
            Bar09.Name = "Bar09";
            Bar09.Size = new System.Drawing.Size(19, 312);
            Bar09.TabIndex = 8;
            // 
            // Bar10
            // 
            Bar10.BackColor = System.Drawing.Color.AliceBlue;
            Bar10.Location = new System.Drawing.Point(172, 0);
            Bar10.Maximum = 255;
            Bar10.Name = "Bar10";
            Bar10.Size = new System.Drawing.Size(19, 312);
            Bar10.TabIndex = 9;
            // 
            // Bar11
            // 
            Bar11.BackColor = System.Drawing.Color.AliceBlue;
            Bar11.Location = new System.Drawing.Point(191, 0);
            Bar11.Maximum = 255;
            Bar11.Name = "Bar11";
            Bar11.Size = new System.Drawing.Size(19, 312);
            Bar11.TabIndex = 10;
            // 
            // Bar12
            // 
            Bar12.BackColor = System.Drawing.Color.AliceBlue;
            Bar12.Location = new System.Drawing.Point(210, 0);
            Bar12.Maximum = 255;
            Bar12.Name = "Bar12";
            Bar12.Size = new System.Drawing.Size(19, 312);
            Bar12.TabIndex = 11;
            // 
            // Bar13
            // 
            Bar13.BackColor = System.Drawing.Color.AliceBlue;
            Bar13.Location = new System.Drawing.Point(229, 0);
            Bar13.Maximum = 255;
            Bar13.Name = "Bar13";
            Bar13.Size = new System.Drawing.Size(19, 312);
            Bar13.TabIndex = 12;
            // 
            // Bar14
            // 
            Bar14.BackColor = System.Drawing.Color.AliceBlue;
            Bar14.Location = new System.Drawing.Point(248, 0);
            Bar14.Maximum = 255;
            Bar14.Name = "Bar14";
            Bar14.Size = new System.Drawing.Size(19, 312);
            Bar14.TabIndex = 13;
            // 
            // Bar15
            // 
            Bar15.BackColor = System.Drawing.Color.AliceBlue;
            Bar15.Location = new System.Drawing.Point(267, 0);
            Bar15.Maximum = 255;
            Bar15.Name = "Bar15";
            Bar15.Size = new System.Drawing.Size(19, 312);
            Bar15.TabIndex = 14;
            // 
            // Bar16
            // 
            Bar16.BackColor = System.Drawing.Color.AliceBlue;
            Bar16.Location = new System.Drawing.Point(286, 0);
            Bar16.Maximum = 255;
            Bar16.Name = "Bar16";
            Bar16.Size = new System.Drawing.Size(19, 312);
            Bar16.TabIndex = 15;
            // 
            // Bar17
            // 
            Bar17.BackColor = System.Drawing.Color.AliceBlue;
            Bar17.Location = new System.Drawing.Point(305, 0);
            Bar17.Maximum = 255;
            Bar17.Name = "Bar17";
            Bar17.Size = new System.Drawing.Size(19, 312);
            Bar17.TabIndex = 16;
            // 
            // Bar18
            // 
            Bar18.BackColor = System.Drawing.Color.AliceBlue;
            Bar18.Location = new System.Drawing.Point(324, 0);
            Bar18.Maximum = 255;
            Bar18.Name = "Bar18";
            Bar18.Size = new System.Drawing.Size(19, 312);
            Bar18.TabIndex = 17;
            // 
            // Bar19
            // 
            Bar19.BackColor = System.Drawing.Color.AliceBlue;
            Bar19.Location = new System.Drawing.Point(343, 0);
            Bar19.Maximum = 255;
            Bar19.Name = "Bar19";
            Bar19.Size = new System.Drawing.Size(19, 312);
            Bar19.TabIndex = 18;
            // 
            // Bar20
            // 
            Bar20.BackColor = System.Drawing.Color.AliceBlue;
            Bar20.Location = new System.Drawing.Point(362, 0);
            Bar20.Maximum = 255;
            Bar20.Name = "Bar20";
            Bar20.Size = new System.Drawing.Size(19, 312);
            Bar20.TabIndex = 19;
            // 
            // tpMiniplayer
            // 
            tpMiniplayer.ImageIndex = 7;
            tpMiniplayer.Location = new System.Drawing.Point(4, 29);
            tpMiniplayer.Name = "tpMiniplayer";
            tpMiniplayer.Size = new System.Drawing.Size(395, 337);
            tpMiniplayer.TabIndex = 7;
            tpMiniplayer.ToolTipText = "MiniPlayer (Esc)";
            tpMiniplayer.UseVisualStyleBackColor = true;
            // 
            // statusStrip
            // 
            statusStrip.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new System.Drawing.Point(0, 374);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new System.Drawing.Size(403, 22);
            statusStrip.SizingGrip = false;
            statusStrip.TabIndex = 1;
            statusStrip.ItemClicked += StatusStrip_ItemClicked;
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.AutoSize = false;
            toolStripStatusLabel.Font = new System.Drawing.Font("Segoe UI", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            toolStripStatusLabel.Margin = new Padding(9, 1, 0, 2);
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new System.Drawing.Size(377, 19);
            toolStripStatusLabel.Spring = true;
            toolStripStatusLabel.Text = "Ready";
            toolStripStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            toolStripStatusLabel.Click += ToolStripStatusLabel1_Click;
            // 
            // notifyIcon
            // 
            notifyIcon.ContextMenuStrip = contextMenuTrayIcon;
            notifyIcon.Icon = (System.Drawing.Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "NetRadio";
            notifyIcon.Visible = true;
            notifyIcon.MouseClick += NotifyIcon_MouseClick;
            // 
            // contextMenuTrayIcon
            // 
            contextMenuTrayIcon.Items.AddRange(new ToolStripItem[] { playPauseToolStripMenuItem, toolStripSeparator7, controlToolStripMenuItem, toolStripSeparator9, showToolStripMenuItem, exitToolStripMenuItem });
            contextMenuTrayIcon.Name = "contextMenuStrip";
            contextMenuTrayIcon.Size = new System.Drawing.Size(128, 104);
            // 
            // playPauseToolStripMenuItem
            // 
            playPauseToolStripMenuItem.Enabled = false;
            playPauseToolStripMenuItem.Image = Properties.Resources.play;
            playPauseToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Fuchsia;
            playPauseToolStripMenuItem.Name = "playPauseToolStripMenuItem";
            playPauseToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            playPauseToolStripMenuItem.Text = "PlayPause";
            playPauseToolStripMenuItem.Click += PlayPauseToolStripMenuItem_Click;
            // 
            // toolStripSeparator7
            // 
            toolStripSeparator7.Name = "toolStripSeparator7";
            toolStripSeparator7.Size = new System.Drawing.Size(124, 6);
            // 
            // controlToolStripMenuItem
            // 
            controlToolStripMenuItem.Image = Properties.Resources.volume;
            controlToolStripMenuItem.Name = "controlToolStripMenuItem";
            controlToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            controlToolStripMenuItem.Text = "Sound";
            controlToolStripMenuItem.Click += ControlToolStripMenuItem_Click;
            // 
            // toolStripSeparator9
            // 
            toolStripSeparator9.Name = "toolStripSeparator9";
            toolStripSeparator9.Size = new System.Drawing.Size(124, 6);
            // 
            // showToolStripMenuItem
            // 
            showToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            showToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("showToolStripMenuItem.Image");
            showToolStripMenuItem.Name = "showToolStripMenuItem";
            showToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            showToolStripMenuItem.Text = "Show";
            showToolStripMenuItem.Click += ShowToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("exitToolStripMenuItem.Image");
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitToolStripMenuItem_Click;
            // 
            // timerVolume
            // 
            timerVolume.Interval = 20;
            timerVolume.Tick += TimerVolume_Tick;
            // 
            // timerLevel
            // 
            timerLevel.Interval = 32;
            timerLevel.Tick += TimerLevel_Tick;
            // 
            // timerPause
            // 
            timerPause.Interval = 30000;
            timerPause.Tick += TimerPause_Tick;
            // 
            // timerCloseFinally
            // 
            timerCloseFinally.Interval = 1500;
            timerCloseFinally.Tick += TimerCloseFinally_Tick;
            // 
            // saveFileDialog
            // 
            saveFileDialog.DefaultExt = "*.csv";
            saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*";
            saveFileDialog.InitialDirectory = "Environment.SpecialFolder.MyDocuments";
            saveFileDialog.RestoreDirectory = true;
            // 
            // timer1
            // 
            timer1.Interval = 1500;
            // 
            // timerResume
            // 
            timerResume.Interval = 1000;
            timerResume.Tick += TimerResume_Tick;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(403, 396);
            Controls.Add(statusStrip);
            Controls.Add(tcMain);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            HelpButton = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmMain";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "NetRadio";
            HelpButtonClicked += FrmMain_HelpButtonClicked;
            Activated += FrmMain_Activated;
            Deactivate += FrmMain_Deactivate;
            FormClosing += FrmMain_FormClosing;
            Load += FrmMain_Load;
            Shown += FrmMain_Shown;
            Click += FrmMain_Click;
            HelpRequested += FrmMain_HelpRequested;
            Move += FrmMain_Move;
            tcMain.ResumeLayout(false);
            tpPlayer.ResumeLayout(false);
            contextMenuPlayer.ResumeLayout(false);
            pnlDisplay.ResumeLayout(false);
            panelLevel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pbLevel).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbVolIcon).EndInit();
            tpStations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvStations).EndInit();
            contextMenuStations.ResumeLayout(false);
            tpHistory.ResumeLayout(false);
            contextMenuDisplay.ResumeLayout(false);
            historyPanel.ResumeLayout(false);
            historyPanel.PerformLayout();
            tpSettings.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            gbAutoRecord.ResumeLayout(false);
            gbAutoRecord.PerformLayout();
            gbOutput.ResumeLayout(false);
            gbMiscel.ResumeLayout(false);
            gbMiscel.PerformLayout();
            gbAutostart.ResumeLayout(false);
            gbAutostart.PerformLayout();
            gbPreselection.ResumeLayout(false);
            gbPreselection.PerformLayout();
            gbHotkeys.ResumeLayout(false);
            gbHotkeys.PerformLayout();
            tpHelp.ResumeLayout(false);
            tableLayoutPanel0.ResumeLayout(false);
            tableLayoutPanel0.PerformLayout();
            panelHelp.ResumeLayout(false);
            panelHelp.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tpInfo.ResumeLayout(false);
            tpInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picBoxPayPal).EndInit();
            tpSectrum.ResumeLayout(false);
            spectrumPanel.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            contextMenuTrayIcon.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tcMain;
        private TabPage tpPlayer;
        private TabPage tpStations;
        private StatusStrip statusStrip;
        private DataGridView dgvStations;
        private RadioButton rbtn01;
        private RadioButton rbtn02;
        private RadioButton rbtn07;
        private RadioButton rbtn06;
        private RadioButton rbtn05;
        private RadioButton rbtn04;
        private RadioButton rbtn03;
        private RadioButton rbtn08;
        private ToolStripStatusLabel toolStripStatusLabel;
        private Button btnDecrease;
        private Button btnIncrease;
        private Button btnPlayStop;
        private ToolTip toolTip;
        private Button btnReset;
        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuTrayIcon;
        private ToolStripMenuItem showToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private Timer timerVolume;
        private TabPage tpInfo;
        private Label lblInformation;
        private LinkLabel linkPayPal;
        private LinkLabel linkHomepage;
        private Panel pnlDisplay;
        private Label lblD4;
        private Label lblD3;
        private Label lblD2;
        private Label lblD1;
        private TabPage tpSettings;
        private GroupBox gbPreselection;
        private CheckBox cbAutostart;
        private GroupBox gbHotkeys;
        private CheckBox cbHotkey;
        private Button btnDown;
        private Button btnUp;
        private Label lblHotkey;
        private ComboBox cmbxHotkey;
        private GroupBox gbMiscel;
        private Button btnSearch;
        private DataGridViewTextBoxColumn col1;
        private DataGridViewTextBoxColumn col2;
        private TabPage tpHelp;
        private TableLayoutPanel tableLayoutPanel0;
        private Label lplTPLHelp2;
        private Label lplTPLHelp1;
        private Label label6;
        private Label label5;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label3;
        private Label label4;
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private Label label2;
        private Panel panelHelp;
        private Panel panel1;
        private ContextMenuStrip contextMenuStations;
        private ToolStripMenuItem deleteToolStripMenuItem;
        private ToolStripMenuItem searchStationToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem addToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripMenuItem moveToToolStripMenuItem;
        private ToolStripMenuItem row1ToolStripMenuItem;
        private ToolStripMenuItem row2ToolStripMenuItem;
        private ToolStripMenuItem row3ToolStripMenuItem;
        private ToolStripMenuItem row4ToolStripMenuItem;
        private ToolStripMenuItem row5ToolStripMenuItem;
        private ToolStripMenuItem row10ToolStripMenuItem;
        private ToolStripMenuItem row12ToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem upToolStripMenuItem;
        private ToolStripMenuItem downToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator3;
        private ToolStripMenuItem topToolStripMenuItem;
        private ToolStripMenuItem endToolStripMenuItem;
        private ToolStripMenuItem editToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripMenuItem pgUpToolStripMenuItem;
        private ToolStripMenuItem pgDnToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private Label lblWebService;
        private ToolStripMenuItem playPauseToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator7;
        private ComboBox cmbxStation;
        private Label lblAutostartStation;
        private GroupBox gbAutostart;
        private PictureBox picBoxPayPal;
        private Label lblCopyright;
        private ContextMenuStrip contextMenuPlayer;
        private ToolStripMenuItem editStationToolStripMenuItem;
        private ContextMenuStrip contextMenuDisplay;
        private ToolStripMenuItem googleToolStripMenuItem;
        private RadioButton rbtn12;
        private RadioButton rbtn11;
        private RadioButton rbtn10;
        private RadioButton rbtn09;
        private ToolStripMenuItem row11ToolStripMenuItem;
        private ToolStripMenuItem row6ToolStripMenuItem;
        private ToolStripMenuItem row7ToolStripMenuItem;
        private ToolStripMenuItem row8ToolStripMenuItem;
        private ToolStripMenuItem row9ToolStripMenuItem;
        private ProgressBarEx volProgressBar;
        private CheckBox cbAlwaysOnTop;
        private CheckBox cbShowBalloonTip;
        private ToolStripMenuItem copyToClipboardToolStripMenuItem;
        private Button btnRecord;
        private RadioButton rbtn25;
        private RadioButton rbtn24;
        private RadioButton rbtn23;
        private RadioButton rbtn22;
        private RadioButton rbtn21;
        private RadioButton rbtn20;
        private RadioButton rbtn19;
        private RadioButton rbtn18;
        private RadioButton rbtn17;
        private RadioButton rbtn16;
        private RadioButton rbtn15;
        private RadioButton rbtn14;
        private RadioButton rbtn13;
        private ToolStripSeparator toolStripSeparator8;
        private ToolStripMenuItem searchNewStationToolStripMenuItem;
        private Label labelBrackets;
        private Label lblCredits;
        private Label lblHorizontalLine;
        private Timer timerLevel;
        private PictureBox pbLevel;
        private PictureBox pbVolIcon;
        private Timer timerPause;
        private CheckBox cbAutoStopRecording;
        private Label lblUpdate;
        private Button btnUpdate;
        private Label label8;
        private Label label7;
        private ProgressBar progressBar;
        private Timer timerCloseFinally;
        private Label lblVolume;
        private ComboBox cmbxOutput;
        private GroupBox gbOutput;
        private ToolStripMenuItem controlToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator9;
        private TabPage tpHistory;
        private ListView historyListView;
        private ColumnHeader chDate;
        private ColumnHeader chStation;
        private ColumnHeader chSong;
        private Panel historyPanel;
        private Button historyExportButton;
        private Button histoyClearButton;
        private CheckBox cbLogHistory;
        private SaveFileDialog saveFileDialog;
        private TabPage tpSectrum;
        private VerticalProgressBar Bar01;
        private VerticalProgressBar Bar02;
        private VerticalProgressBar Bar03;
        private VerticalProgressBar Bar04;
        private VerticalProgressBar Bar05;
        private VerticalProgressBar Bar06;
        private VerticalProgressBar Bar07;
        private VerticalProgressBar Bar08;
        private VerticalProgressBar Bar09;
        private VerticalProgressBar Bar10;
        private VerticalProgressBar Bar11;
        private VerticalProgressBar Bar12;
        private VerticalProgressBar Bar13;
        private VerticalProgressBar Bar14;
        private VerticalProgressBar Bar15;
        private VerticalProgressBar Bar16;
        private VerticalProgressBar Bar17;
        private VerticalProgressBar Bar18;
        private VerticalProgressBar Bar19;
        private VerticalProgressBar Bar20;
        private Panel spectrumPanel;
        private Label Lbl19;
        private Label Lbl18;
        private Label Lbl17;
        private Label Lbl16;
        private Label Lbl15;
        private Label Lbl14;
        private Label Lbl13;
        private Label Lbl12;
        private Label Lbl11;
        private Label Lbl10;
        private Label Lbl09;
        private Label Lbl08;
        private Label Lbl07;
        private Label Lbl06;
        private Label Lbl05;
        private Label Lbl04;
        private Label Lbl03;
        private Label Lbl02;
        private Label Lbl01;
        private Label Lbl00;
        private Label lblAuthor;
        private ToolStripSeparator tsSepListViewDeleteEntry;
        private ToolStripMenuItem tSMItemListViewDeleteEntry;
        private GroupBox gbAutoRecord;
        private Button btnActions;
        private CheckBox cbActions;
        private Timer timer1;
        private Timer timerResume;
        private Label label10;
        private Label label9;
        private LinkLabel linkLabel1;
        private LinkLabel linkLabeGNU;
        private Panel panelLevel;
        private ImageList imageList;
        private TabPage tpMiniplayer;
        private CheckBox cbClose2Tray;
        private Button btnUpdateSettings;
        private Label labelStartMode;
        private RadioButton rbStartModeTray;
        private RadioButton rbStartModeMini;
        private RadioButton rbStartModeMain;
    }
}

