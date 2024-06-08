using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace NetRadio
{
    public partial class MiniPlayer : Form
    {
        internal static Point MpMousePos => MousePosition; //{  get { return MousePosition; } }
        internal ProgressBarEx MpVolProgBar { get { return volProgressBar; } set { volProgressBar = value; } }
        internal Button MpBtnAOT { get { return btnAOT; } set { btnAOT = value; } }
        internal ComboBox MpCmBxStations { get { return cmBxStations; } set { cmBxStations = value; } }
        internal Button MpBtnPlay { get { return btnPlayPause; } set { btnPlayPause = value; } }
        internal PictureBox MpPBLevel { get { return pictureBoxLevel; } set { pictureBoxLevel = value; } }
        internal ToolTip MpToolTip { get { return toolTip; } set { toolTip = value; } }

        public event EventHandler FormHide;
        public event EventHandler PlayPause;
        public event EventHandler PlayerReset;
        public event EventHandler VolumeProgress;
        //public event EventHandler<VolumeEventArgs> VolumeMouseWheelX; // file clsUtilities
        public event MouseEventHandler VolumeMouseWheel; // file clsUtilities
        public event EventHandler IncreaseVolume;
        public event EventHandler DecreaseVolume;
        public event EventHandler StationChanged;
        public event EventHandler F4_ShowPlayer;
        public event EventHandler F5_ShowHistory;
        public event EventHandler F12_ShowSpectrum;
        private int levelLeft, levelRight;
        private static MiniPlayer _this; // Objektverweis für statische Methode
        private readonly Region _client;
        //private static string labelD1Text = string.Empty;
        private bool insideRestoreBtn = false;
        private bool shiftRestoreBtn = false;
        private bool rightMouseBtnDown = false;

        public MiniPlayer()
        {
            InitializeComponent();
            new DropShadow().ApplyShadows(this);
            _this = this;
            _client = Region.FromHrgn(NativeMethods.CreateRoundRectRgn(1, 1, Width - 1, Height - 1, 15, 15));
            Region = Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, Width, Height, 15, 15)); // FormBorderStyle.None
            //volProgressBar.MouseWheel += VolProgressBar_MouseWheel;
        }

        protected virtual void OnFormHide(EventArgs e) { FormHide?.Invoke(this, e); }
        protected virtual void OnPlayPause(EventArgs e) { PlayPause?.Invoke(this, e); }
        protected virtual void OnPlayerReset(EventArgs e) { PlayerReset?.Invoke(this, e); }
        protected virtual void OnVolumeProgress(EventArgs e) { VolumeProgress?.Invoke(this, e); }
        //protected virtual void OnVolumeMouseWheelX(VolumeEventArgs e) { VolumeMouseWheelX?.Invoke(this, e); }
        protected virtual void OnVolumeMouseWheel(MouseEventArgs e) { VolumeMouseWheel?.Invoke(this, e); }
        protected virtual void OnIncreaseVolume(EventArgs e) { IncreaseVolume?.Invoke(this, e); }
        protected virtual void OnDecreaseVolume(EventArgs e) { DecreaseVolume?.Invoke(this, e); }
        protected virtual void OnStationChanged(EventArgs e) { StationChanged?.Invoke(this, e); }
        protected virtual void OnF4_ShowPlayer(EventArgs e) { F4_ShowPlayer?.Invoke(this, e); }
        protected virtual void OnF5_ShowHistory(EventArgs e) { F5_ShowHistory?.Invoke(this, e); }
        protected virtual void OnF12_ShowSpectrum(EventArgs e) { F12_ShowSpectrum?.Invoke(this, e); }

        public static void DrawLevelMeter(int above, int below)
        {
            if (above != _this.levelLeft && below != _this.levelRight)
            {
                _this.levelLeft = above;
                _this.levelRight = below;
                Bitmap bm = new(_this.pictureBoxLevel.ClientSize.Width, _this.pictureBoxLevel.ClientSize.Height);
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    g.Clear(_this.pictureBoxLevel.BackColor);
                    using Pen p = new(new LinearGradientBrush(new Point(0, 0), new Point(140, 0), SystemColors.Highlight, Color.Coral), 2.0f); // 3.0f
                    p.DashCap = DashCap.Flat;
                    p.DashPattern = [1.0f, 1.0f];
                    g.DrawLine(p, 2, 2, _this.levelLeft, 2); // x, y, 
                    g.DrawLine(p, 2, 5, _this.levelRight, 5);
                }
                _this.pictureBoxLevel.Image = bm; // pictureBox2.Refresh();
            }
        }

        //public static void MpLblD1_Text(string text) { _this.labelD1.Text = text; }
        public bool MpVisible() { return Visible; }
        public static void MpLblD2_Text(string text) { _this.labelD2.Text = text; }

        private void Panel_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            Color color1, color2;
            if (ActiveForm == this)
            {
                color1 = Color.AliceBlue;
                color2 = Color.LightSteelBlue;
            }
            else
            {
                color1 = Color.GhostWhite;
                color2 = Color.WhiteSmoke;
            }
            LinearGradientBrush myBrush = new(p.PointToClient(p.Parent.PointToScreen(p.Location)), new Point(p.Width, p.Height), color1, color2);
            //LinearGradientBrush myBrush = new(new Point(0, 0), new Point(Width, Height), color1, color2);
            e.Graphics.FillRectangle(myBrush, ClientRectangle);
        }

        private void BtnRestore_Paint(object sender, PaintEventArgs e)
        {
            if (insideRestoreBtn) // btnRestore.ClientRectangle.Contains(PointToClient(Control.MousePosition)))
            {
                if (shiftRestoreBtn || rightMouseBtnDown) { btnRestore.ImageIndex = 3; }
                else { btnRestore.ImageIndex = 1; } //btnRestore.ForeColor = SystemColors.HighlightText;
            }
            else if (ActiveForm == this)
            {
                btnRestore.BackColor = btnRestore.FlatAppearance.BorderColor = Color.LightSteelBlue;
                if (shiftRestoreBtn) { btnRestore.ImageIndex = 4; }
                else { btnRestore.ImageIndex = 0; } //btnRestore.ForeColor = SystemColors.HighlightText;
            }
            else
            {
                btnRestore.BackColor = btnRestore.FlatAppearance.BorderColor = Color.GhostWhite;
                btnRestore.ImageIndex = 2; // btnRestore.ForeColor = SystemColors.Control;
            }
        }

        private void MiniPlayer_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Clicks != 1) { return; } // MouseDoubleClick event not firing after add MouseDown event
            //if (e.Button == MouseButtons.Left && cmBxStations.Focused) // + wird nicht als Hotkey für Volume erkannt sonder in Textbox geschrieben
            //{
            //    int index = cmBxStations.FindStringExact(labelD1Text);
            //    if (index >= 0) { cmBxStations.SelectedIndex = index; }
            //    else { cmBxStations.Text = ""; }
            //    panel.Focus();
            //}
            NativeMethods.ReleaseCapture();
            NativeMethods.SendMessage(Handle, NativeMethods.WM_NCLBUTTONDOWN, NativeMethods.HT_CAPTION, 0);
        }

        private void BtnRestore_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift) { Application.Exit(); } //  || rightMouseBtnDown wird hier nicht erkannt
            else { OnFormHide(null); }
        }

        private void BtnPlayPause_Click(object sender, EventArgs e) { OnPlayPause(null); }
        private void BtnReload_Click(object sender, EventArgs e) { OnPlayerReset(null); }
        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if ((ModifierKeys & Keys.Shift) == Keys.Shift) { Application.Exit(); }
            else { OnFormHide(null); }
        }

        //private void SelectStationByPosition(int position)
        //{
        //    if (cmBxStations.Items.Count >= position) { cmBxStations.SelectedIndex = position - 1; }
        //}

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                case Keys.Escape | Keys.Shift:
                case Keys.F4 | Keys.Control:
                case Keys.F4 | Keys.Control | Keys.Shift:
                    {
                        if ((ModifierKeys & Keys.Shift) == Keys.Shift) { Application.Exit(); }
                        else
                        {
                            OnFormHide(null);
                        }
                        return true;
                    }
                case Keys.F2:
                case Keys.Enter | Keys.Shift:
                    {
                        if (!cmBxStations.DroppedDown)
                        {
                            cmBxStations.Focus();
                            cmBxStations.DroppedDown = true; // !cmBxStations.DroppedDown;
                            return true;
                        }
                        else if (cmBxStations.DroppedDown)
                        {
                            cmBxStations.Focus();
                            cmBxStations.DroppedDown = false;
                            return true;
                        }
                        else { return false; }
                    }
                case Keys.F4:
                    {
                        OnF4_ShowPlayer(null);
                        return true;
                    }
                case Keys.F5:
                    {
                        OnF5_ShowHistory(null);
                        return true;
                    }
                case Keys.F12:
                    {
                        OnF12_ShowSpectrum(null);
                        return true;
                    }
                case Keys.Space:
                    {
                        if (ActiveControl != cmBxStations)
                        {
                            OnPlayPause(null);
                            return true;
                        }
                        else { return false; }
                    }
                case Keys.Add:
                case Keys.Oemplus:
                    {
                        if (ActiveControl != cmBxStations)
                        {
                            OnIncreaseVolume(null);
                            toolTip.Show("Volume " + volProgressBar.Value.ToString() + "%", volProgressBar, 20, 7, 1000);
                            //toolTip.Show("Volume " + volProgressBar.Value.ToString() + "%", volProgressBar);
                            //timerVolTT.Start();
                            return true;
                        }
                        else { return false; }
                    }
                case Keys.Subtract:
                case Keys.OemMinus:
                    {
                        if (ActiveControl != cmBxStations)
                        {
                            OnDecreaseVolume(null);
                            toolTip.Show("Volume " + volProgressBar.Value.ToString() + "%", volProgressBar, 20, 7, 1000);
                            //toolTip.Show("Volume " + volProgressBar.Value.ToString() + "%", volProgressBar);
                            //timerVolTT.Start();
                            return true;
                        }
                        else { return false; }
                    }
                case Keys.Home:
                    {
                        if (ActiveControl != cmBxStations)
                        {
                            BtnAOT_Click(null, null);
                            return true;
                        }
                        else { return false; }
                    }
                case Keys.Back:
                    {
                        if (ActiveControl != cmBxStations)
                        {
                            OnPlayerReset(null);
                            return true;
                        }
                        else { return false; }
                    }
                case Keys.Down | Keys.Alt:
                    {
                        cmBxStations.Focus();
                        cmBxStations.DroppedDown = true;
                        return true;
                    }
                case Keys.NumPad1:
                case Keys.D1: { if (cmBxStations.Items.Count >= 1) { cmBxStations.SelectedIndex = 0; OnStationChanged(null); return true; } return false; }
                case Keys.NumPad2:
                case Keys.D2: { if (cmBxStations.Items.Count >= 2) { cmBxStations.SelectedIndex = 1; OnStationChanged(null); return true; } return false; }
                case Keys.NumPad3:
                case Keys.D3: { if (cmBxStations.Items.Count >= 3) { cmBxStations.SelectedIndex = 2; OnStationChanged(null); return true; } return false; }
                case Keys.NumPad4:
                case Keys.D4: { if (cmBxStations.Items.Count >= 4) { cmBxStations.SelectedIndex = 3; OnStationChanged(null); return true; } return false; }
                case Keys.NumPad5:
                case Keys.D5: { if (cmBxStations.Items.Count >= 5) { cmBxStations.SelectedIndex = 4; OnStationChanged(null); return true; } return false; }
                case Keys.NumPad6:
                case Keys.D6: { if (cmBxStations.Items.Count >= 6) { cmBxStations.SelectedIndex = 5; OnStationChanged(null); return true; } return false; }
                case Keys.NumPad7:
                case Keys.D7: { if (cmBxStations.Items.Count >= 7) { cmBxStations.SelectedIndex = 6; OnStationChanged(null); return true; } return false; }
                case Keys.NumPad8:
                case Keys.D8: { if (cmBxStations.Items.Count >= 8) { cmBxStations.SelectedIndex = 7; OnStationChanged(null); return true; } return false; }
                case Keys.NumPad9:
                case Keys.D9: { if (cmBxStations.Items.Count >= 9) { cmBxStations.SelectedIndex = 8; OnStationChanged(null); return true; } return false; }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_KEYDOWN && (Keys)m.WParam == Keys.ShiftKey && !shiftRestoreBtn)  // .Text = "❌"
            {
                shiftRestoreBtn = true;
                btnRestore.Invalidate();
                toolTip.SetToolTip(btnRestore, "Quit Application");
            }
            else if (m.Msg == NativeMethods.WM_KEYUP && (Keys)m.WParam == Keys.ShiftKey && shiftRestoreBtn) // .Text = "🡽"
            {
                shiftRestoreBtn = false;
                btnRestore.Invalidate();
                toolTip.SetToolTip(btnRestore, "Main Window (Esc)");
            }
            return base.ProcessKeyPreview(ref m);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRegion(SystemBrushes.Control, _client); // zeichnet From.Background(-1) => Rahmen ergibt sich aus dem im Designer eingestellten Form.Background
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_MOUSEWHEEL && ActiveControl != cmBxStations)
            {
                int delta = (int)m.WParam >> 16;
                if (delta.GetType() == typeof(int) && delta != 0)
                {
                    OnVolumeMouseWheel(new MouseEventArgs(MouseButtons.None, 0, Cursor.Position.X, Cursor.Position.Y, delta)); //OnVolumeMouseWheelX(new VolumeEventArgs(m.WParam.ToInt32()));
                    toolTip.Show("Volume " + volProgressBar.Value.ToString() + "%", volProgressBar, 20, 7, 1000);
                }
            }
            base.WndProc(ref m);
        }

        //private void VolProgressBar_MouseWheel(object sender, MouseEventArgs e)
        //{
        //    OnVolumeMouseWheel(e); //OnVolumeMouseWheel(new VolumeEventArgs() { Delta = e.Delta < 0 ? 1 : -1 });
        //    toolTip.SetToolTip(volProgressBar, "Volume " + volProgressBar.Value.ToString() + "%");
        //}

        private void VolProgressBar_MouseDown(object sender, MouseEventArgs e) { OnVolumeProgress(null); }

        private void VolProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
                OnVolumeProgress(null);
                if (toolTip.GetToolTip(volProgressBar) != volProgressBar.Value.ToString()) { toolTip.SetToolTip(volProgressBar, "Volume " + volProgressBar.Value.ToString() + "%"); }
            }
        }

        private void VolProgressBar_MouseUp(object sender, MouseEventArgs e)
        {
            toolTip.SetToolTip(volProgressBar, "Volume(+/–)");
            toolTip.Hide(volProgressBar);
        }

        private void BtnAOT_Click(object sender, EventArgs e)
        {
            if (TopMost)
            {
                TopMost = false;
                btnAOT.Image = Properties.Resources.pinout;
                btnAOT.BackColor = SystemColors.ControlDark;
            }
            else
            {
                TopMost = true;
                btnAOT.Image = Properties.Resources.pinpush;
                btnAOT.BackColor = Color.Maroon;
            }
        }

        private void LabelD2_TextChanged(object sender, EventArgs e)
        {
            using Graphics g = CreateGraphics();
            if ((int)g.MeasureString(labelD2.Text, labelD2.Font, 0, StringFormat.GenericTypographic).Width > labelD2.Width)
            {
                if (toolTip.GetToolTip(labelD2) != labelD2.Text) { toolTip.SetToolTip(labelD2, labelD2.Text); }
            }
            else { toolTip.SetToolTip(labelD2, null); } // ToolTip zurücksetzen
        }

        //private void ToolTip_Draw(object sender, DrawToolTipEventArgs e)
        //{
        //    e.DrawBackground();
        //    e.DrawBorder();
        //    e.DrawText();
        //    System.Reflection.PropertyInfo h = toolTip.GetType().GetProperty("Handle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //    IntPtr handle = (IntPtr)h.GetValue(toolTip); // ToolTip t = (ToolTip)sender;
        //    Point location = Cursor.Position;
        //    NativeMethods.MoveWindow(handle, location.X + 20, location.Y - 20, e.Bounds.Width, e.Bounds.Height, false);
        //}

        //private void TimerVolTT_Tick(object sender, EventArgs e)
        //{
        //    if (!NativeMethods.IsKeyDown(Keys.Oemplus) && !NativeMethods.IsKeyDown(Keys.Add) && !NativeMethods.IsKeyDown(Keys.Subtract) && !NativeMethods.IsKeyDown(Keys.OemMinus))
        //    {
        //        toolTip.Hide(volProgressBar);
        //        timerVolTT.Stop();
        //    }
        //}

        private void GoogleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string search = string.Empty;
            if (!string.IsNullOrEmpty(labelD2.Text)) { search = labelD2.Text; }
            if (!string.IsNullOrEmpty(search))
            {
                try
                {
                    ProcessStartInfo psi = new("https://www.google.com/search?q=" + System.Web.HttpUtility.HtmlEncode(Regex.Replace(search.ToLowerInvariant(), @"\s+", "+").Trim().Replace(".", ""))) { UseShellExecute = true };
                    Process.Start(psi);
                }
                catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }

        private void CopyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(labelD2.Text)) { Clipboard.SetText(labelD2.Text, TextDataFormat.UnicodeText); }
        }

        private void PictureBoxLevel_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, pictureBoxLevel.DisplayRectangle, Color.FromArgb(190, 190, 190), ButtonBorderStyle.Solid);
        }

        private void MiniPlayer_MouseDoubleClick(object sender, MouseEventArgs e) { OnFormHide(null); }

        private void Panel_MouseDoubleClick(object sender, MouseEventArgs e) { OnFormHide(null); }

        private void MiniPlayer_Activated(object sender, EventArgs e)
        {
            panelTitle.Invalidate();
            btnRestore.Invalidate();
        }

        private void MiniPlayer_Deactivate(object sender, EventArgs e) { panelTitle.Invalidate(); btnRestore.Invalidate(); }

        private void CmBxStations_TextChanged(object sender, EventArgs e)
        {
            if (cmBxStations.Focused)
            {
                OnStationChanged(null);
                btnPlayPause.Focus();
            }
        }

        private void LabelD2_MouseDoubleClick(object sender, MouseEventArgs e) { OnFormHide(null); }

        private void MiniPlayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                OnFormHide(null);
            }
        }

        private void BtnRestore_MouseEnter(object sender, EventArgs e) { insideRestoreBtn = true; }
        private void BtnRestore_MouseLeave(object sender, EventArgs e) { insideRestoreBtn = false; }
        private void BtnRestore_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && !rightMouseBtnDown)
            {
                rightMouseBtnDown = true;
                btnRestore.Invalidate();
            }
        }

        private void BtnRestore_MouseUp(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Right) && rightMouseBtnDown)
            {
                if (insideRestoreBtn && GetChildAtPoint(e.Location) != null)
                {
                    if (FrmMain.MainClose2Tray)
                    {
                        rightMouseBtnDown = false;
                        Hide();
                    }
                    else { Application.Exit(); }
                }
                else
                {
                    rightMouseBtnDown = false;
                    btnRestore.Invalidate();
                }
            }
        }

        private void MiniPlayer_Shown(object sender, EventArgs e)
        {
            if (TopMost)
            {
                btnAOT.Image = Properties.Resources.pinpush;
                btnAOT.BackColor = Color.Maroon;
            }
            else
            {
                btnAOT.Image = Properties.Resources.pinout;
                btnAOT.BackColor = SystemColors.ControlDark;
            }
        }

        private void PictureBoxLevel_DoubleClick(object sender, EventArgs e) { OnF12_ShowSpectrum(null); }

    }
}
