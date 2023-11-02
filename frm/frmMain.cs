using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;

namespace NetRadio
{
    public partial class FrmMain : Form
    {
        internal static HttpClient MainHttpClient { get { return httpClient; } }
        private readonly string _myUserAgent = "NetRadio";
        [FixedAddressValueType()]
        internal IntPtr _myUserAgentPtr;
        internal delegate void UpdateMessageDelegate(string txt);
        internal delegate void UpdateTagDelegate();
        internal delegate void UpdateStatusDelegate(string txt);
        private int _stream = 0;
        private readonly DOWNLOADPROC myStreamCreateURL;
        private byte[] _data; // local recording buffer
        private FileStream _fs = null;
        private bool _recording = false;
        private readonly string _downloads = NativeMethods.SHGetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0);
        private string _downloadFileName;
        private string _channelFilename;
        private TAG_INFO _tagInfo;
        //private SYNCPROC _hlsChange;
        private SYNCPROC _connectFail;
        private SYNCPROC _deviceFail;
        private SYNCPROC _metaSync;
        private readonly int _hlsPlugIn = 0;
        private readonly int _opusPlugIn = 0;
        private readonly int _flacPlugIn = 0;
        private int _downlaodSize = 0;
        private int _currentButtonNum = 0; // wird im RadioButton_CheckedChanged auf Werte > 0 gesetzt; zurücksetzen auf 0 erfolgt manuell!
        private readonly Version _curVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private static bool alwaysOnTop;
        private static bool logHistory = true;
        private static bool autoStopRecording;
        private static bool startMin;
        private static bool updateAvailable;
        private bool somethingToSave;
        private bool radioBtnChanged; // ersetzt auf Station-Tab nothingToSave
        private string strCellValue = string.Empty;
        private static readonly string appName = Application.ProductName; // "NetRadio";
        private static readonly string appPath = Application.ExecutablePath; // EXE-Pfad
        private readonly string xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, appName + ".xml");
        private string hkLetter = string.Empty; // Flag für existierenden Hotkey. AUSNAHME: Programmstart
        private static int lastHotkeyPress;
        private int rowIndexFromMouseDown;
        private int colIndexFromMouseDown;
        private Rectangle dragBoxFromMouseDown;
        private int rowIndexOfItemUnderMouseToDrop;
        private string autostartStation;
        private bool firstEmptyStart = false;
        private bool showBalloonTip = true;
        private bool keepActionsActive;
        private float channelVolume = 1.0f;
        private readonly int stationSum = 25; // Rows = stationSum * 2
        private static HttpClient httpClient;
        private Control currentDisplayLabel;
        private int levelLeft, levelRight;
        private readonly RadioButton autoStartRadioButton = null;
        private int recIncrement = 0;
        private string localSetupFile = string.Empty;
        private string downloadUpdateURL = string.Empty;
        private int intOutputDevice = 0; //  0 = default => Init(-1)
        private string strOutputDevice;
        private string prevOutputDevice;
        private bool changeOutputDevice = false;
        //private Button newButton;
        private readonly string findNewStations = "Press <Ctrl+F> to find new radio stations.";
        private bool helpRequested = true;
        private readonly MiniPlayer miniPlayer = new();
        private readonly string[] strArrHistory = new string[3];
        private readonly string[] lvSortOrderArray = new string[3];
        private ListViewItem lvItemHistory;
        private readonly CListViewItemComparer lviComparer = new();      // Sortierer für die ListView
        private readonly BASSTimer spectrumTimer = new(); // Creates a new Timer instance using a default interval of 50ms => 20 Hz.
        private readonly List<byte> spectrumData = new();
        private readonly int spectrumlines = 20;
        private TimeSpan currPlayingTime = TimeSpan.Zero;
        private TimeSpan totalPlayingTime = TimeSpan.Zero;
        private readonly float _netPreBuff;
        private bool _isBuffering = false;
        private bool _playWakeFromSleep = false;
        private long accumulatedTicks;
        private readonly DataTable tableActions = new();
        private static readonly Timer timerAction1 = new();
        private static readonly Timer timerAction2 = new();
        private static readonly Timer timerAction3 = new();
        private static readonly Timer timerAction4 = new();
        private static readonly Timer timerAction5 = new();
        private static readonly Timer timerAction6 = new();
        private static readonly Timer timerAction7 = new();
        private static readonly Timer timerAction8 = new();
        private static readonly Timer timerAction9 = new();
        private SplashForm frmSplash = null;

        public FrmMain()
        {
            InitializeComponent();
            timerAction1.Tick += new EventHandler(OnTimedEvent);
            timerAction2.Tick += new EventHandler(OnTimedEvent);
            timerAction3.Tick += new EventHandler(OnTimedEvent);
            timerAction4.Tick += new EventHandler(OnTimedEvent);
            timerAction5.Tick += new EventHandler(OnTimedEvent);
            timerAction6.Tick += new EventHandler(OnTimedEvent);
            timerAction7.Tick += new EventHandler(OnTimedEvent);
            timerAction8.Tick += new EventHandler(OnTimedEvent);
            statusStrip.Renderer = new AutoEllipsisToolStripRenderer();
            string formPosX = string.Empty;
            string formPosY = string.Empty;
            string miniPosX = string.Empty;
            string miniPosY = string.Empty;
            _myUserAgentPtr = Marshal.StringToHGlobalAnsi(_myUserAgent);
            int cores = Environment.ProcessorCount;
            Bass.BASS_SetConfigPtr(BASSConfig.BASS_CONFIG_NET_AGENT, _myUserAgentPtr);
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_BUFFER, 4000); // The buffer length in milliseconds (default 5000)
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PREBUF, 80); // Percentage of the download buffer length(BASS_CONFIG_NET_BUFFER) should be filled before starting playback. The default is 75 %
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 2000); // The playback buffer length for HSTREAM and HMUSIC channels (default 500), maximum is 5000 milliseconds
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 50); // The update period of HSTREAM and HMUSIC channel playback buffers (default 100)
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, cores >= 4 ? 2 : 1); // The number of threads to use for updating playback buffers. Default is to use a single thread.
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT, 4000); // The default timeout is 5000 milliseconds 
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 1); // When enabled, BASS will process PLS, M3U, WPL and ASX playlists, going through each entry until it finds a URL that it can play. By default, playlist procesing is disabled.
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_DEV_DEFAULT, true); // enable "Default" device
            Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_HLS_DOWNLOAD_TAGS, true); // stream's DOWNLOADPROC callback function will receive any ID3v2 tags that the stream contains
            if (Bass.BASS_Init(-1, 48000, BASSInit.BASS_DEVICE_DEFAULT, Handle)) // BASS_DEVICE_DEFAULT	0 = 16 bit, stereo, no 3D, no Latency calc, no Speaker Assignments
            {   //  The sample format specified in the freq and flags parameters has no effect on the output - the device's native sample format is automatically used.
                _flacPlugIn = Bass.BASS_PluginLoad("bassflac.dll");
                if (_flacPlugIn <= 0)
                {
                    MessageBox.Show(Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode()), "bassflac.dll", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _opusPlugIn = Bass.BASS_PluginLoad("bassopus.dll");
                if (_opusPlugIn <= 0)
                {
                    MessageBox.Show(Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode()), "bassopus.dll", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                _hlsPlugIn = Bass.BASS_PluginLoad("basshls.dll");
                if (_hlsPlugIn <= 0)
                {
                    MessageBox.Show(Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode()), "basshls.dll", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                myStreamCreateURL = new DOWNLOADPROC(MyDownloadProc); // Internet stream download callback function
            }
            else
            {
                MessageBox.Show("This application failed to start because bass.dll was not found.\nRe-installing the application may fix this problem.", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref channelVolume);
            //Text = Utilities.GetDescription() + " " + new Regex(@"^\d+\.\d+").Match(curVersion.ToString()).Value;
            Text = Assembly.GetCallingAssembly().GetName().Name + " " + new Regex(@"^\d+\.\d+").Match(_curVersion.ToString()).Value;
            lblUpdate.Text = "Current version: " + _curVersion.ToString();
            for (int j = 0; j < stationSum * 4; j++) { dgvStations.Rows.Add("", ""); } // dgvStations.Rows.Add(stationSum); ist wahrscheinlich schlechter, weil Cell.Value = null entsteht

            xmlPath = Utilities.IsInnoSetupValid(Path.GetDirectoryName(appPath)) ? xmlPath : Path.ChangeExtension(appPath, ".xml"); // Portable-Version; prüft auch Debugger.IsAttached

            tableActions.Columns.Add("Enabled", typeof(bool));
            tableActions.Columns.Add("Task", typeof(string));
            tableActions.Columns.Add("Station", typeof(string));
            tableActions.Columns.Add("Time", typeof(string));


            if (File.Exists(xmlPath))
            {
                using (XmlTextReader xtr = new(xmlPath))
                {
                    xtr.WhitespaceHandling = WhitespaceHandling.None; // Whitespace zwischen Elementen
                    try
                    {
                        int j = 0;
                        while (xtr.Read())
                        {
                            if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "Station")
                            {
                                if (j < dgvStations.RowCount)
                                {// tritt ein wenn der User die Datei außerhalb des Programms editiert oder der Programmier die Zeilenzahl reduziert
                                    xtr.MoveToAttribute("Name");
                                    dgvStations.Rows[j].Cells[0].Value = xtr.Value;
                                    xtr.MoveToAttribute("URL");
                                    dgvStations.Rows[j].Cells[1].Value = xtr.Value;
                                }
                                j++;
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "Hotkey")
                            {
                                xtr.MoveToAttribute("Enabled");
                                if (int.TryParse(xtr.Value, out int intEnabeld)) { cbHotkey.Checked = lblHotkey.Enabled = cmbxHotkey.Enabled = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                                else { cbHotkey.Checked = lblHotkey.Enabled = cmbxHotkey.Enabled = false; }
                                xtr.MoveToAttribute("Letter");
                                if (string.IsNullOrEmpty(xtr.Value)) { lblHotkey.Enabled = cmbxHotkey.Enabled = cbHotkey.Checked = false; }
                                else { if (cbHotkey.Checked && new Regex("^[A-Z0-9]$").IsMatch(xtr.Value)) { cmbxHotkey.Text = hkLetter = xtr.Value; } } // You won't be able to register a hotkey before the window is created
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "Output")
                            {
                                xtr.MoveToAttribute("Device");
                                if (string.IsNullOrEmpty(xtr.Value)) { strOutputDevice = "Default"; }
                                else { strOutputDevice = xtr.Value; }
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "MiniPlayer")
                            {
                                xtr.MoveToAttribute("Enabled");
                                if (int.TryParse(xtr.Value, out int intEnabeld)) { cbShowBalloonTip.Checked = showBalloonTip = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                                else { cbShowBalloonTip.Checked = false; }
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "AlwaysOnTop")
                            {
                                xtr.MoveToAttribute("Enabled");
                                if (int.TryParse(xtr.Value, out int intEnabeld)) { cbAlwaysOnTop.Checked = alwaysOnTop = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                                else { cbAlwaysOnTop.Checked = false; }
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "LogHistory")
                            {
                                xtr.MoveToAttribute("Enabled");
                                if (int.TryParse(xtr.Value, out int intEnabeld)) { cbLogHistory.Checked = logHistory = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                                else { cbLogHistory.Checked = false; }
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "AutoStopRecording")
                            {
                                xtr.MoveToAttribute("Enabled");
                                if (int.TryParse(xtr.Value, out int intEnabeld)) { cbAutoStopRecording.Checked = autoStopRecording = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                                else { cbAutoStopRecording.Checked = false; }
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "Volume")
                            {
                                xtr.MoveToAttribute("Value");
                                int volume = int.TryParse(xtr.Value, out int intVolume) ? intVolume : (int)channelVolume * 100;
                                volume = volume > 100 ? 100 : volume < 0 ? 0 : volume;
                                channelVolume = Convert.ToSingle(volume) / 100f;
                                Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, channelVolume);
                                miniPlayer.MpVolProgBar.Value = volProgressBar.Value = volume;
                                lblVolume.Text = volProgressBar.Value.ToString();
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "FormLocation")
                            {
                                xtr.MoveToAttribute("PosX");
                                formPosX = xtr.Value;
                                xtr.MoveToAttribute("PosY");
                                formPosY = xtr.Value;
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "MiniLocation")
                            {
                                xtr.MoveToAttribute("PosX");
                                miniPosX = xtr.Value;
                                xtr.MoveToAttribute("PosY");
                                miniPosY = xtr.Value;
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "Autostart")
                            {
                                xtr.MoveToAttribute("Station");
                                if (int.TryParse(xtr.Value, out int intStation))
                                {
                                    if (intStation > 0 && intStation <= stationSum) { cmbxStation.Text = autostartStation = xtr.Value; }
                                }
                            }
                            else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "KeepActionsActive")
                            {
                                xtr.MoveToAttribute("Enabled");
                                if (int.TryParse(xtr.Value, out int intEnabeld)) { keepActionsActive = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                            }
                            if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "Action")
                            {
                                xtr.MoveToAttribute("Enabled");
                                if (!bool.TryParse(xtr.Value, out bool enabled)) { enabled = false; }
                                else if (enabled == true) { cbActions.Checked = true; }
                                xtr.MoveToAttribute("Task");
                                string task = xtr.Value;
                                xtr.MoveToAttribute("Station");
                                string station = xtr.Value;
                                xtr.MoveToAttribute("Time");
                                string time = xtr.Value;
                                tableActions.Rows.Add(enabled, task, station, time);
                            }
                        }
                    }
                    catch (XmlException ex) { MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
            }
            else
            {// MessageBox.Show("\"" + xmlPath + "\"  is not found.");
                Directory.CreateDirectory(Path.GetDirectoryName(xmlPath)); // If the folder exists already, the line will be ignored.
                XmlDocument xmlDoc = new();
                xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?><NetRadio></NetRadio>");
                xmlDoc.Save(xmlPath); // if the specified file exists, this method overwrites it.
                firstEmptyStart = true;
            }
            foreach (DataGridViewColumn column in dgvStations.Columns) { column.SortMode = DataGridViewColumnSortMode.NotSortable; }
            //bool activateScheduler = false;
            string[] args = Environment.GetCommandLineArgs();
            foreach (string x in args)
            {
                if (x.Contains("min"))
                {
                    startMin = true; // siehe frmMain_Shown-Event
                    Opacity = 0; // sonst wird GUI kurz angezeigt - unschön
                }
                //else if (x.Contains("schedule") || x.Contains("activate")) { activateScheduler = true; }
            }
            if (keepActionsActive && tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled") == true)) { cbActions.Checked = true; }
            for (int i = 1; i < args.Length; i++) // Kommandozeilenargumente werden vor Autostart-Einstellungen benutzt
            {
                if (!args[i].Contains(':') && int.TryParse(args[i], out int intStation))
                {
                    string btnName = "rbtn" + Math.Abs(intStation).ToString("D2");
                    Control[] controls = tcMain.TabPages[0].Controls.Find(btnName, true);
                    if (controls.Length == 1 && controls[0] is RadioButton)
                    {
                        autoStartRadioButton = controls[0] as RadioButton; // foundBtn.Checked = true;
                        break;
                    }
                }
            }

            //Regex checkTime = new(@"^(?i)(0?[1-9]|1[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?( AM| PM)?$"); // This will work for these format (case insensitive): 08:09:00 AM, 08:09 AM, 8:30 AM, 8:30:59
            //string commandLine = string.Join(" ", args.Select(s => s.Contains(' ') ? "\"" + s + "\"" : s));
            //if (checkTime.IsMatch(commandLine))
            //{
            //    /* Timer für AutoRecording einbauen und Zeitdifferenz berechnen  */
            //    /* Recording-Button mit Uhrzeit beschriften (z.B. statt Grafik)  */
            //} 

            if (autoStartRadioButton == null && !string.IsNullOrEmpty(autostartStation)) // kein Kommandozeilenargumente - dann Autostart-Einstellungen benutzen
            { //MessageBox.Show(autostartStation);
                string btnName = "rbtn" + autostartStation.PadLeft(2, '0');
                Control[] controls = tcMain.TabPages[0].Controls.Find(btnName, true);
                if (controls.Length == 1 && controls[0] is RadioButton) { autoStartRadioButton = controls[0] as RadioButton; } // löst StartPlaying aus (s. FrmMain_Shown-Event)
            }
            StatusStrip_SingleLabel(true, findNewStations);

            cbAutostart.Checked = Utilities.IsAutoStartEnabled(appName, "\"" + appPath + "\"" + " -min");
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            if (int.TryParse(formPosX, out int xPos) && int.TryParse(formPosY, out int yPos))
            {// MainForm komplett innerhalb der WorkingArea angezeigt werden
                xPos = xPos < 0 ? 0
                    : xPos + Width > screen.Width
                    ? screen.Width - Width
                    : xPos;
                yPos = yPos < 0
                    ? yPos = 0
                    : yPos + Height > screen.Height
                    ? yPos = screen.Height - Height
                    : yPos;
                Location = new Point(xPos, yPos);
            } // else StartPosition = FormStartPosition.CenterScreen => s. Designer

            if (int.TryParse(miniPosX, out int x_Pos) && int.TryParse(miniPosY, out int y_Pos))
            {// MiniPlayer komplett innerhalb der WorkingArea angezeigt werden
                x_Pos = x_Pos < 0 ? 0
                    : x_Pos + miniPlayer.Width > screen.Width
                    ? screen.Width - miniPlayer.Width
                    : x_Pos;
                y_Pos = y_Pos < 0
                    ? y_Pos = 0
                    : y_Pos + miniPlayer.Height > screen.Height
                    ? y_Pos = screen.Height - miniPlayer.Height
                    : y_Pos;
                miniPlayer.Location = new Point(x_Pos, y_Pos);
            }
            else { miniPlayer.Location = Location; }
            historyListView.ListViewItemSorter = lviComparer;
            spectrumTimer.Tick += SpectrumTick;
            _netPreBuff = Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_PREBUF) / 100f; // 0.75
        }

        private void MyDownloadProc(IntPtr buffer, int length, IntPtr user)
        {
            if (buffer != IntPtr.Zero && length == 0)
            {
                string txt = Marshal.PtrToStringAnsi(buffer);
                Invoke(new UpdateMessageDelegate(UpdateMessageDisplay), new object[] { txt });
            }
            else if (buffer != IntPtr.Zero && _recording)
            {
                try
                {
                    if (_fs is null)
                    {
                        _downloadFileName = _downloads + "\\" + appName + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".mp3";
                        BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_stream);
                        switch (info.ctype)
                        {
                            case BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG:
                            case BASSChannelType.BASS_CTYPE_STREAM_OPUS:
                            case BASSChannelType.BASS_CTYPE_STREAM_OGG:
                                _recording = false; // downloadFileName = Path.ChangeExtension(downloadFileName, ".opus");
                                BeginInvoke((MethodInvoker)delegate () { RecordingStop(false); }); //uint uiFlags = /*MB_OK*/ 0x00000000 | /*MB_SETFOREGROUND*/  0x00010000 | /*MB_APPLMODAL*/ 0x00001000 | /*MB_ICONEXCLAMATION*/ 0x00000030;
                                if (NativeMethods.MessageBoxTimeout(NativeMethods.GetForegroundWindow(), $"Recording is only available for MP3 and AAC streams.", $"NetRadio", 0x00000000 | 0x00010000 | 0x00000000 | 0x00000040, 0, 3000) > 0)
                                { return; }
                                else { break; }
                            case BASSChannelType.BASS_CTYPE_STREAM_MF:
                                //WAVEFORMATEX w = (WAVEFORMATEX)Marshal.PtrToStructure(Bass.BASS_ChannelGetTags(_stream, BASSTag.BASS_TAG_WAVEFORMAT), typeof(WAVEFORMATEX));
                                //MessageBox.Show(w.nAvgBytesPerSec.ToString() + Environment.NewLine +
                                //    w.nChannels.ToString() + Environment.NewLine +
                                //    w.nSamplesPerSec.ToString() + Environment.NewLine +
                                //    w.wBitsPerSample.ToString());
                                if (((WAVEFORMATEX)Marshal.PtrToStructure(Bass.BASS_ChannelGetTags(_stream, BASSTag.BASS_TAG_WAVEFORMAT), typeof(WAVEFORMATEX))).wFormatTag == WAVEFormatTag.MPEG_HEAAC) { _downloadFileName = Path.ChangeExtension(_downloadFileName, ".aac"); } // High-Efficiency Advanced Audio Coding (HE-AAC) 
                                else { _downloadFileName = Path.ChangeExtension(_downloadFileName, ".mp3"); }
                                break;
                            default:
                                _downloadFileName = Path.ChangeExtension(_downloadFileName, ".mp3");
                                break;
                        }
                        _fs = new FileStream(_downloadFileName, FileMode.CreateNew);
                        _downlaodSize = 0;
                    }
                    if (buffer == IntPtr.Zero)
                    {
                        BeginInvoke((MethodInvoker)delegate () { RecordingStop(); }); // setzt _fs auf null; this code runs on the UI thread!
                    }
                    else
                    {
                        if ((_data == null) || (_data.Length < length)) { _data = new byte[length + 1]; }
                        Marshal.Copy(buffer, _data, 0, length); // Speicher in Buffer kopieren
                        _fs.Write(_data, 0, length); // In Datei schreiben
                        _downlaodSize += length;
                        recIncrement++;
                        if (recIncrement % 2 == 0) // Anzeige nur jedes 2te mal aktualisieren
                        {
                            BeginInvoke((MethodInvoker)delegate () { lblD4.Text = "Downloading " + Utilities.GetFileSize(_downlaodSize); });
                        }
                    }
                }
                catch (IOException ex)
                {
                    _recording = false;
                    BeginInvoke((MethodInvoker)delegate ()// this code runs on the UI thread!
                    {
                        RecordingStop(); // setzt _fs auf null; 
                        MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Error); // "An error occurred. Recording has stopped."
                    });
                }
            }
        }

        private void TimerResume_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_channelFilename) && Utilities.PingGoogleSuccess(timerResume.Interval))  //_stream != 0 && 
            {
                timerResume.Enabled = false;
                StartPlaying(_channelFilename, _currentButtonNum);
                _channelFilename = string.Empty;
            }
            else if (_stream == 0) { timerResume.Enabled = false; }
            else
            {
                lblD3.Text = !lblD3.Text.EndsWith("Trying to reconnect...") ? "◴Trying to reconnect..." : lblD3.Text;
                lblD3.Text = lblD3.Text.StartsWith("◷") ? "◶" + lblD3.Text.TrimStart('◷') :
                             lblD3.Text.StartsWith("◶") ? "◵" + lblD3.Text.TrimStart('◶') :
                             lblD3.Text.StartsWith("◵") ? "◴" + lblD3.Text.TrimStart('◵') : "◷" + lblD3.Text[1..];
                MiniPlayer.MpLblD2_Text(lblD3.Text);
            }
        }

        private void UpdateMessageDisplay(string txt) { lblD4.Text = txt; } // HTTP 0.2 OK

        private void RecordingStop(bool freeStream = true, Color? color = null)
        {
            _recording = false;
            recIncrement = 0;
            if (_fs != null)
            {
                _fs.Flush();
                _fs.Close();
                _fs = null;
            }
            if (freeStream) { Bass.BASS_StreamFree(_stream); } // syncs are automatically removed when the channel is freed 
            lblD4.ForeColor = color ?? SystemColors.ControlText;
            btnRecord.BackColor = SystemColors.ControlDark;
            btnRecord.Image = Properties.Resources.mic_white;
        }

        //private void HLSChangeSync(int handle, int channel, int data, IntPtr user) // The updated SDT is available from BASS_ChannelGetTags(Int32, BASSTag) (type BASS_TAG_HLS_SDT).
        //{
        //    BeginInvoke(() => // https://www.un4seen.com/forum/?topic=16905.msg121901#msg121901
        //    {
        //        IntPtr p = Bass.BASS_ChannelGetTags(channel, BASSTag.BASS_TAG_HLS_EXTINF); // get new segment's EXTINF tag
        //        //The current segment's EXTINF tag in the media playlist. A single UTF-8 string is returned,
        //        //containing the duration of the segment and possibly a title too (separated by a comma).
        //        //This will be updated every time a new segment begins downloading
        //        if (p != IntPtr.Zero) // the current segment's EXTINF tag in the media playlist is available from BASS_ChannelGetTags with the BASS_TAG_HLS_EXTINF tag type
        //        {
        //            string myString = Utils.IntPtrAsStringUtf8(p);
        //            if (myString != null) { myString = Regex.Replace(myString, @"^[\d.-]*\,?\s?", ""); } // look for duration/title separator
        //            lblD2.Text = string.IsNullOrEmpty(myString) ? lblD2.Text : myString;
        //        }
        //        string[] tags = Bass.BASS_ChannelGetTagsID3V2(channel); //If present, the ID3v2 tags can/will change with each segment.
        //        if (tags != null)
        //        {
        //            lblD2.Text = string.Join(" ", tags);
        //            //String title = Bass.TAGS.TAGS_Read(_stream, ARTI + " - " TITL); // That stream does also include ID3v2 tags (title and artist). You can use the Tags add-on to process them.
        //        }
        //    });
        //}

        private void ConnectionSync(int handle, int channel, int data, IntPtr user) // BASS_SYNC_DOWNLOAD informs when the connection is closed
        {
            BeginInvoke(() =>
            {
                timerLevel.Stop();
                spectrumTimer.Stop();
                pbLevel.Image = null;
                miniPlayer.MpPBLevel.Image = null;
                foreach (VerticalProgressBar vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
                if (Bass.BASS_ChannelIsActive(_stream) != BASSActive.BASS_ACTIVE_PLAYING) { lblD3.Text = "⌛Connecting..."; }
                MiniPlayer.MpLblD2_Text("⌛Connecting..."); //.Replace("ERROR:", "⚠"));
                btnPlayStop.Image = Properties.Resources.play_white;
                miniPlayer.MpBtnPlay.Image = Properties.Resources.play_white;
                playPauseToolStripMenuItem.Text = "Play"; // btnPlayStop.Text = 
                playPauseToolStripMenuItem.Image = Properties.Resources.play;
                BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_stream);
                if (info != null)
                { //It will not be possible to resume the recording channel then; a new recording channel will need to be created.
                    Bass.BASS_ChannelStop(_stream);
                    Bass.BASS_Free();
                    _channelFilename = info.filename;
                    Application.DoEvents();
                    timerResume.Enabled = true;
                }
            });
        }

        private void DeviceSync(int handle, int channel, int data, IntPtr user) // BASS_SYNC_DEV_FAIL is triggered on device stops (eg. if it is disconnected/disabled)
        {
            BeginInvoke(() =>
            {
                lblD3.Text = "Output device is disconnected or disabled.";
                Application.DoEvents(); // damit vorstehender Text angezeigt wird
                System.Threading.Thread.Sleep(1000); // andernfalls werden die gerade entfernten Devices als noch vorhanden angezeigt
                int devices = 0;
                BASS_DEVICEINFO dInfo;
                for (int n = 1; (dInfo = Bass.BASS_GetDeviceInfo(n)) != null; n++) { if (dInfo.IsEnabled) { devices++; } }
                if (devices > 0) // intOutputDevice wurde mit Wert 0 definiert
                {
                    timerLevel.Stop();
                    spectrumTimer.Stop();
                    foreach (VerticalProgressBar vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
                    intOutputDevice = 0;
                    strOutputDevice = "Default";
                    somethingToSave = true;
                    BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_stream);
                    if (info != null && Bass.BASS_ChannelStop(_stream) && Bass.BASS_Stop() && Bass.BASS_Free()) { StartPlaying(info.filename, _currentButtonNum); }
                    else
                    {
                        string strError = Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode());
                        if (string.IsNullOrEmpty(strError)) { strError = lblD3.Text; }
                        Bass.BASS_ChannelStop(_stream);
                        Bass.BASS_Free();
                        RestorePlayerDefaults();
                        MessageBox.Show(strError, appName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (tcMain.SelectedIndex == 3) { CmbxOutput_CreateContent(); }
                }
                else
                {
                    Bass.BASS_ChannelStop(_stream);
                    Bass.BASS_Free();
                    RestorePlayerDefaults();
                }
            });
        }

        private void MetaSync(int handle, int channel, int data, IntPtr user) // BASS_SYNC_META is triggered on meta changes of SHOUTcast streams
        {
            if (data != 0) { Invoke(new UpdateStatusDelegate(UpdateStatusDisplay), new object[] { Marshal.PtrToStringAnsi(new IntPtr(data)) }); }
            else
            {
                try
                {
                    if (_tagInfo.UpdateFromMETA(Bass.BASS_ChannelGetTags(channel, BASSTag.BASS_TAG_META | BASSTag.BASS_TAG_ID3V2), TAGINFOEncoding.Utf8OrLatin1, true)) { Invoke(new UpdateTagDelegate(UpdateTagDisplay)); }
                }
                catch (ArgumentOutOfRangeException) { } // Wenn Text mehr als 64 Zeichen hat
            }
        }

        private void UpdateStatusDisplay(string txt) { toolStripStatusLabel.Text = txt; }

        private void UpdateTagDisplay()
        {
            if (_recording && autoStopRecording)
            {
                btnRecord.PerformClick(); btnRecord.Focus(); // RecordingStop();
                return;
            }
            if (_tagInfo != null)
            {
                lblD2.Text = _tagInfo.ToString(); //MessageBox.Show(_tagInfo.title);
                MiniPlayer.MpLblD2_Text(lblD2.Text);
                if (tcMain.SelectedTab == tpSectrum) { StatusStrip_SingleLabel(false, lblD2.Text); }
                if (logHistory) { AddToHistory(_tagInfo.ToString()); }
                lblD4.Text = _tagInfo.filename;
                if (showBalloonTip && ActiveForm != miniPlayer && ActiveForm != this) { notifyIcon.ShowBalloonTip(2, "Now playing: ", lblD2.Text, ToolTipIcon.None); } // && minClose
            }
            else { lblD4.Text = dgvStations.Rows[_currentButtonNum - 1].Cells[1].Value.ToString(); }
        }

        private void SpectrumTick(object sender, EventArgs e)
        {
            if (tcMain.SelectedTab != tpSectrum) { return; }
            //int length = (int)Bass.BASS_ChannelSeconds2Bytes(_stream, 0.03); // a 30ms window in bytes to be filled with sample data
            //float[] data = new float[length / 4]; // first we need a mananged object where the sample data should be placed length is in bytes, so the number of floats to process is length/4 
            float[] _fft = new float[1024];
            int ret = Bass.BASS_ChannelGetData(_stream, _fft, (int)BASSData.BASS_DATA_FFT2048); // get fft data, BASS_ChannelGetData(int handle, float[] buffer, int length)
            if (ret < -1) { return; }
            int x, y;
            int b0 = 0;
            for (x = 0; x < spectrumlines; x++) //computes the spectrum data
            {
                float peak = 0;
                int b1 = (int)Math.Pow(2, x * 10.0 / (spectrumlines - 1));
                if (b1 > 1023) b1 = 1023;
                if (b1 <= b0) b1 = b0 + 1;
                for (; b0 < b1; b0++)
                {
                    if (peak < _fft[1 + b0]) { peak = _fft[1 + b0]; }
                }
                y = (int)(Math.Sqrt(peak) * 3 * 255 - 4);
                if (y > 255) { y = 255; }
                if (y < 0) { y = 0; }
                spectrumData.Add((byte)y);  // spectrumData[x + 1] = (byte)y; // 
            }
            SetSpectrum(spectrumData);
            spectrumData.Clear();
        }

        public void SetSpectrum(List<byte> data)
        {
            if (data.Count < spectrumlines) { return; }
            Bar01.Value = data[0];
            Bar02.Value = data[1];
            Bar03.Value = data[2];
            Bar04.Value = data[3];
            Bar05.Value = data[4];
            Bar06.Value = data[5];
            Bar07.Value = data[6];
            Bar08.Value = data[7];
            Bar09.Value = data[8];
            Bar10.Value = data[9];
            Bar11.Value = data[10];
            Bar12.Value = data[11];
            Bar13.Value = data[12];
            Bar14.Value = data[13];
            Bar15.Value = data[14];
            Bar16.Value = data[15];
            Bar17.Value = data[16];
            Bar18.Value = data[17];
            Bar19.Value = data[18];
            Bar20.Value = data[19];
        }

        private void AddToHistory(string songTitle)
        {
            strArrHistory[0] = DateTime.Now.ToString("HH:mm:ss");
            strArrHistory[1] = (tcMain.TabPages[0].Controls["rbtn" + _currentButtonNum.ToString("D2")] as RadioButton).Text;
            strArrHistory[2] = songTitle;
            lvItemHistory = new ListViewItem(strArrHistory)
            {
                Tag = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc).ToString("o"),
                ToolTipText = LongSubItemText(songTitle) ? songTitle : ""
            };

            // ToolTipText = LongSubItemText(songTitle) ? songTitle : "" // vorausgesetzt historyListView.ShowItemToolTips = true;
            historyListView.Items.Insert(0, lvItemHistory);
            historyExportButton.Enabled = histoyClearButton.Enabled = true;
            HistoryListView_SetDefaultColumnWidth();
            if (tcMain.SelectedIndex == 2) { TPHistory_SetStatusBarText(); }
        }

        private bool LongSubItemText(string songTitle)
        {
            using Graphics g = CreateGraphics();
            return (int)g.MeasureString(songTitle, historyListView.Font, 0, StringFormat.GenericTypographic).Width > historyListView.Columns[2].Width;
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            //FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Path.Combine(Path.GetDirectoryName(appPath), "bass_aac.dll"));
            lblCredits.Text = "Acknowledgments" + Environment.NewLine +
                "NetRadio uses libraries for streaming and audio playback:" + Environment.NewLine +
                "bass.dll © 1999-2022, Un4seen Developments Ltd. (" + Bass.BASS_GetVersion(4) + ")" + Environment.NewLine +
                //"bass_aac.dll © 2002-2012, Sebastian Andersson (" + fvi.FileMajorPart + "." + fvi.FileMinorPart + "." + fvi.FileBuildPart + "." + fvi.FilePrivatePart + ")" + Environment.NewLine +
                "bass.net.dll © 2005-2022, radio42, B. Niedergesaess " + "(" + Utils.GetVersion() + ")";
            List<string> devicelist = new();
            int defaultDevice = -1;
            BASS_DEVICEINFO info; // = new BASS_DEVICEINFO();
            for (int n = 1; (info = Bass.BASS_GetDeviceInfo(n)) != null; n++) // Device 0 is always the "no sound" device, so you should start at device 1 if you only want to list real output devices.
            {
                if (info.IsEnabled) { devicelist.Add(info.ToString()); }
                if (info.IsDefault) { defaultDevice = n - 1; }
            }
            if (devicelist.Count > 0) // intOutputDevice wurde mit Wert 0 definiert
            {
                intOutputDevice = devicelist.IndexOf(strOutputDevice); // in dieser Liste ist 0 = Default
                if (intOutputDevice == defaultDevice) { intOutputDevice = 0; } // Sieht wie in Bug aus, ist aber ein Feature, um wenn möglich "Default" zu erzwingen. BASS_GetDeviceInfo gibt niemals Default aus, sondern immer die höhere DeviceID
                if (intOutputDevice <= 0) { intOutputDevice = 0; } // 0 = Default
                strOutputDevice = devicelist[intOutputDevice].ToString();
            }
            //newButton = new()
            //{
            //    Text = "🡗", //  South West Sans-Serif Arrow (U+1F857)                ⭹", // ❌,
            //    Font = new Font("Segoe UI", 14, FontStyle.Regular),
            //    Location = new Point(362, -1),
            //    Size = new Size(45, 26),
            //    FlatStyle = FlatStyle.Flat,
            //    TextImageRelation = TextImageRelation.TextAboveImage,
            //    Image = null,
            //    //Image = Properties.Resources.send2tray,
            //};
            //newButton.FlatAppearance.BorderSize = 0;
            //newButton.FlatAppearance.MouseOverBackColor = Color.SkyBlue;
            //newButton.FlatAppearance.MouseDownBackColor = SystemColors.ActiveCaption;
            //toolTip.SetToolTip(newButton, "MiniPlayer (Esc)");
            //Controls.Add(newButton);
            //newButton.BringToFront();
            //newButton.Click += new EventHandler(NewButton_Click);
            //newButton.MouseEnter += new EventHandler(NewButton_MouseEnter);
            //newButton.MouseLeave += new EventHandler(NewButton_MouseLeave);

            miniPlayer.Show(this);
            miniPlayer.FormHide += new EventHandler(MiniPlayer_FormHide);
            miniPlayer.PlayPause += new EventHandler(MiniPlayer_PlayPause);
            miniPlayer.PlayerReset += new EventHandler(MiniPlayer_PlayReset);
            miniPlayer.VolumeProgress += new EventHandler(MiniPlayer_VolumeProgress);
            miniPlayer.IncreaseVolume += new EventHandler(MiniPlayer_IncreaseVolume);
            miniPlayer.DecreaseVolume += new EventHandler(MiniPlayer_DecreaseVolume);
            miniPlayer.StationChanged += new EventHandler(MiniPlayer_StationChanged);
            miniPlayer.F4_ShowPlayer += new EventHandler(MiniPlayer_F4_ShowPlayer);
            miniPlayer.F5_ShowHistory += new EventHandler(MiniPlayer_F5_ShowHistory);
            miniPlayer.F12_ShowSpectrum += new EventHandler(MiniPlayer_F12_ShowSpectrum);

            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(PowerMode_Changed);
            RewriteButtonText(); // enthält miniPlayer.MpCmBxStations.Items.Add()-Loop


            //for (int i = 0; i < stationSum; i++)
            //{
            //    if (dgvStations.Rows[i].Cells[0].Value != null && !string.IsNullOrEmpty(dgvStations.Rows[i].Cells[0].Value.ToString()))
            //    {
            //        miniPlayer.MpCmBxStations.Items.Add(Utilities.StationLong(dgvStations.Rows[i].Cells[0].Value.ToString()));
            //    }
            //}
            if (cbActions.Checked) { PrepareActions(); }
            ((ScrollBar)dgvStations.GetType().GetProperty("VerticalScrollBar", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(dgvStations, null)).MouseCaptureChanged += (s, e) => { dgvStations.EndEdit(); };
        }

        private void MiniPlayer_FormHide(object sender, EventArgs e) { ShowMe(); }
        private void MiniPlayer_PlayReset(object sender, EventArgs e) { BtnReset_Click(null, null); }
        private void MiniPlayer_PlayPause(object sender, EventArgs e) { BtnPlayStop_Click(null, null); }
        private void MiniPlayer_VolumeProgress(object sender, EventArgs e) { SetProgressBarValue(); }
        private void MiniPlayer_IncreaseVolume(object sender, EventArgs e) { BtnIncrease_Click(null, null); }
        private void MiniPlayer_DecreaseVolume(object sender, EventArgs e) { BtnDecrease_Click(null, null); }
        private void MiniPlayer_F4_ShowPlayer(object sender, EventArgs e) { ShowMe(); tcMain.SelectedIndex = 0; }
        private void MiniPlayer_F5_ShowHistory(object sender, EventArgs e) { ShowMe(); tcMain.SelectedIndex = 2; }
        private void MiniPlayer_F12_ShowSpectrum(object sender, EventArgs e) { ShowMe(); tcMain.SelectedIndex = 6; }
        private void MiniPlayer_StationChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < stationSum; i++)
            {
                if (dgvStations.Rows[i].Cells[0].Value != null)
                {
                    if (miniPlayer.MpCmBxStations.Text != null && miniPlayer.MpCmBxStations.Text == Utilities.StationLong(dgvStations.Rows[i].Cells[0].Value.ToString()))
                    {
                        string btnName = "rbtn" + (i + 1).ToString().PadLeft(2, '0');
                        Control[] controls = tcMain.TabPages[0].Controls.Find(btnName, true);
                        if (controls.Length == 1 && controls[0] is RadioButton) { (controls[0] as RadioButton).Checked = true; } // löst StartPlaying aus (BtnReset_Click in RadioButton_CheckedChanged)
                        break;
                    }
                }
            }
        }

        //private void NewButton_Click(object sender, EventArgs e)
        //{
        //    Hide(); //ShowInTaskbar = false; verträgt sich nicht mit GlobalHotkey => zerstört Handle
        //    ShowMiniPlayer();
        //    tcMain.SelectedIndex = 0;
        //}

        //private void NewButton_MouseEnter(object sender, EventArgs e) { newButton.ForeColor = Color.White; }
        //private void NewButton_MouseLeave(object sender, EventArgs e) { newButton.ForeColor = Color.Black; }

        private async void PowerMode_Changed(Object sender, PowerModeChangedEventArgs e)
        {
            switch (e.Mode)
            {
                case PowerModes.Suspend: // The operating system is about to be suspended
                    if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                    {
                        _playWakeFromSleep = true;
                        Bass.BASS_ChannelStop(_stream);
                        Bass.BASS_Free();
                        Invoke(new Action(() => RestorePlayerDefaults(_currentButtonNum)));
                    }
                    break;
                case PowerModes.Resume when _playWakeFromSleep: // resume from a suspended state
                    _playWakeFromSleep = false;
                    try
                    {
                        int max = 20;
                        for (int i = 0; i <= max; i++)
                        {
                            bool foo = false; // one second more
                            if (Utilities.PingGoogleSuccess(Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT))) { foo = true; }
                            await System.Threading.Tasks.Task.Delay(1000).ConfigureAwait(false);
                            if (foo) { break; }
                            else if (i == max)
                            {
                                Invoke(new Action(() => (tcMain.TabPages[0].Controls["rbtn" + (_currentButtonNum - 1).ToString("D2")] as RadioButton).Checked = false));
                                return;
                            }
                        }
                        Invoke(new Action(() => StartPlaying(dgvStations.Rows[_currentButtonNum - 1].Cells[1].Value.ToString(), _currentButtonNum))); // switch to the UI thread in an async method

                    }
                    catch { }
                    break;
            }
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeMethods.WM_SHOWME) { ShowMe(); } // another instance is started
            else if (m.Msg == NativeMethods.WM_HOTKEY)
            {
                int keyPressTick = Environment.TickCount;
                int elapsed = keyPressTick - lastHotkeyPress;
                lastHotkeyPress = keyPressTick;
                if (elapsed <= 400) { Close(); }
                else if (miniPlayer.Visible && !miniPlayer.Handle.Equals(NativeMethods.GetForegroundWindow())) { miniPlayer.Activate(); }
                else if (Visible) // heißt nicht, das Form sichtbar bzw. das aktive Fenster sein muss
                {
                    if (ActiveForm == null) { Activate(); }
                    else
                    {
                        ShowMiniPlayer();
                        Hide(); //ShowInTaskbar = false; verträgt sich nicht mit GlobalHotkey => zerstört Handle
                        tcMain.SelectedIndex = 0;
                    }
                }
                else { ShowMe(); }  // not visible, d.h. im Tray
            }
            else if (m.Msg == NativeMethods.WM_QUERYENDSESSION) { Close(); }
            else if (m.Msg == NativeMethods.WM_NCLBUTTONDBLCLK) { Hide(); ShowMiniPlayer(); }
            else if (m.Msg == NativeMethods.WM_NCLBUTTONDOWN && tcMain.SelectedTab == tpStations) { dgvStations.EndEdit(); }
            //else if (m.Msg == NativeMethods.WM_SETCURSOR) // does work if there are no controls covering the form where the cursor is; every control has its own WndProc
            //{
            //    int lowWord = (m.LParam.ToInt32() << 16) >> 16;
            //    if (lowWord == NativeMethods.HTCLIENT) { Console.Beep(); } //  m.Result = (IntPtr)1; // return TRUE; equivalent in C++
            //}
            base.WndProc(ref m);
        }

        private void ShowMiniPlayer()
        {
            int index = miniPlayer.MpCmBxStations.FindStringExact(lblD1.Text);
            if (index >= 0 && miniPlayer.MpCmBxStations.SelectedIndex != index) { miniPlayer.MpCmBxStations.SelectedIndex = index; }
            miniPlayer.MpCmBxStations.Select(0, 0);
            miniPlayer.MpPBLevel.Focus(); // Focus von MpCmBxStations weg nehmen 
            miniPlayer.Show();
            bool top = miniPlayer.TopMost; // get our current "TopMost" value (ours will always be false though)
            miniPlayer.TopMost = true; // make our form jump to the top of everything
            miniPlayer.TopMost = top; // set it back to whatever it was
        }

        private void ShowMe()
        {
            if (!Visible)
            {
                Show();
                miniPlayer.Hide();
            }
            else if (WindowState == FormWindowState.Minimized) { WindowState = FormWindowState.Normal; } // wahrscheinlich unnötig, kann nicht minimiert werden
            bool top = TopMost; // get our current "TopMost" value (ours will always be false though)
            TopMost = true; // make our form jump to the top of everything
            TopMost = top; // set it back to whatever it was
            BringToFront();
            Activate();
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {// the event is fired twice because whenever one RadioButton within a group is checked another will be unchecked
            RadioButton rb = (RadioButton)sender; // sender as RadioButton;
            int oldId = _currentButtonNum;
            pbLevel.Image = null;
            miniPlayer.MpPBLevel.Image = null;
            timerLevel.Stop();
            spectrumTimer.Stop();
            timerResume.Stop();
            foreach (VerticalProgressBar vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
            Application.DoEvents();
            if (rb.Checked)
            {
                rb.ForeColor = Color.White;
                rb.BackColor = SystemColors.Highlight; //.HotTrack; //.ActiveBorder; //.InactiveCaption; //.ControlDark;
                _currentButtonNum = Convert.ToInt32(rb.Tag.ToString());
            }
            else
            {
                rb.ForeColor = SystemColors.ControlText;
                rb.BackColor = Color.Transparent;
            }
            if (!firstEmptyStart && oldId != _currentButtonNum && _currentButtonNum > 0)
            {
                BtnReset_Click(null, null); //  StartPlaying(dgvStations.Rows[Convert.ToInt32(rb.Tag) - 1].Cells[1].Value.ToString()); } // Autostart
                btnReset.Enabled = true; // beim Programmstart deaktiviert
            }
        }

        private void BtnIncrease_MouseDown(object sender, MouseEventArgs e) { timerVolume.Enabled = true; timerVolume.Start(); }
        private void BtnIncrease_MouseUp(object sender, MouseEventArgs e) { timerVolume.Stop(); }
        private void BtnDecrease_MouseDown(object sender, MouseEventArgs e) { timerVolume.Enabled = true; timerVolume.Start(); }
        private void BtnDecrease_MouseUp(object sender, MouseEventArgs e) { timerVolume.Stop(); }

        private void SetProgressBarValue()
        {
            float absMouse;
            float clcFactor;
            if (miniPlayer.Visible)
            {
                absMouse = (miniPlayer.PointToClient(MiniPlayer.MpMousePos).X - miniPlayer.MpVolProgBar.Bounds.X);
                clcFactor = miniPlayer.MpVolProgBar.Width / (float)100;
            }
            else
            {
                absMouse = (PointToClient(MousePosition).X - volProgressBar.Bounds.X);
                clcFactor = volProgressBar.Width / (float)100;
            }
            float relMouse = absMouse / clcFactor;
            int intMouse = Convert.ToInt32(relMouse);
            miniPlayer.MpVolProgBar.Value = volProgressBar.Value = intMouse > 100 ? 100 : intMouse < 0 ? 0 : intMouse;
            Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, volProgressBar.Value / 100f);
            lblVolume.Text = volProgressBar.Value.ToString();
            somethingToSave = true;
        }

        private void VolProgressBar_MouseDown(object sender, MouseEventArgs e) { SetProgressBarValue(); }

        private void VolProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) { SetProgressBarValue(); }
        }

        private void TimerVolume_Tick(object sender, EventArgs e)
        {
            if (btnIncrease.Focused) { BtnIncrease_Click(btnIncrease, null); }
            else if (btnDecrease.Focused) { BtnDecrease_Click(btnDecrease, null); }
        }

        private void TcMain_SelectedIndexChanged(object sender, EventArgs e)
        {// 0 = Player, 1 = Stations, 2 = History, 3 = Settings, 4 = Help, 5 = Information
            if (tcMain.SelectedIndex == 0 && !Utilities.IsDGVEmpty(dgvStations))
            {
                if (radioBtnChanged) // d.h. es wurden Änderungen vorgenommen
                {
                    if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                    {
                        BASS_CHANNELINFO info = new();
                        if (Bass.BASS_ChannelGetInfo(_stream, info)) // get current url => info.filename
                        {
                            bool urlIsStillInFavorites = false;
                            for (int i = 0; i < stationSum; i++)
                            {
                                if (dgvStations.Rows[i].Cells[1].Value != null && dgvStations.Rows[i].Cells[1].Value.ToString().ToLower() == info.filename.ToLower())
                                {
                                    urlIsStillInFavorites = (tcMain.TabPages[0].Controls["rbtn" + (i + 1).ToString("D2")] as RadioButton).Checked = true; // tcMain.TabPages[0].Controls.Find("rbtn" + (i + 1).ToString("D2"), true)[0] as RadioButton; //  cast to the control type and take the first element 
                                    UpdateCaption_lblD1(dgvStations.Rows[i].Cells[0].Value.ToString());
                                    break;
                                }
                            }
                            if (!urlIsStillInFavorites) // || deviceChanged)
                            {
                                int iTag = 0;
                                Bass.BASS_ChannelStop(_stream); // wPlayer.controls.stop();

                                foreach (RadioButton rb in tcMain.TabPages[0].Controls.OfType<RadioButton>())
                                {
                                    if (rb.Checked)
                                    {
                                        bool isValid = int.TryParse(rb.Tag.ToString(), out iTag);
                                        if (!isValid || string.IsNullOrEmpty(dgvStations.Rows[iTag - 1].Cells[1].Value.ToString())) { rb.Checked = false; }
                                    }
                                    else { rb.Checked = false; }
                                }

                                if (iTag > 0)
                                {
                                    StartPlaying(dgvStations.Rows[iTag - 1].Cells[1].Value.ToString(), iTag);
                                    UpdateCaption_lblD1(dgvStations.Rows[iTag - 1].Cells[0].Value.ToString());
                                    int index = miniPlayer.MpCmBxStations.FindStringExact(lblD1.Text);
                                    if (index >= 0 && miniPlayer.MpCmBxStations.SelectedIndex != index) { miniPlayer.MpCmBxStations.SelectedIndex = index; }
                                }
                                else
                                {
                                    RestorePlayerDefaults();
                                    //miniPlayer.MpBtnPlay.Enabled = btnPlayStop.Enabled = btnReset.Enabled = btnRecord.Enabled = false;
                                    //btnPlayStop.Image = Properties.Resources.play_white;
                                    //miniPlayer.MpBtnPlay.Image = Properties.Resources.play_white;
                                    //btnPlayStop.BackColor = SystemColors.ControlDark;
                                    //miniPlayer.MpBtnPlay.BackColor = SystemColors.ControlDark;
                                    //playPauseToolStripMenuItem.Text = "Play"; // btnPlayStop.Text = 
                                    //playPauseToolStripMenuItem.Image = Properties.Resources.play;
                                    //playPauseToolStripMenuItem.Enabled = false;
                                }
                            }
                        }
                    }
                    RewriteButtonText();
                    somethingToSave = true; // => SaveConfig() => radioBtnChanged = false;
                }
                foreach (RadioButton rb in tcMain.TabPages[0].Controls.OfType<RadioButton>()) { if (rb.Checked) { rb.Focus(); } }
            }
            if (tcMain.SelectedIndex == 1)
            {// Stations
                UpdateStatusLabelStationsList();
                dgvStations.Focus(); // sonst funktioniert F2 nicht sogleich
            }
            else if (tcMain.SelectedIndex == 2)
            {
                TopMost = false; // Workaround, damit Tooltip in Listview im Vordergrund angezeigt wird
                TPHistory_SetStatusBarText();
                historyListView.Focus();
            }
            else if (tcMain.SelectedIndex == 3)
            {
                CmbxOutput_CreateContent();
                TPSettings_SetStatusBarText();
            }
            else if (tcMain.SelectedIndex == 6) // Spectrum
            {
                StatusStrip_SingleLabel(false, lblD2.Text);
            }
            else if (tcMain.SelectedIndex == 7) // Miniplayer
            {
                Hide(); //ShowInTaskbar = false; verträgt sich nicht mit GlobalHotkey => zerstört Handle
                ShowMiniPlayer();
                tcMain.SelectedIndex = 0;
            }
            else
            {
                string statusLabelText = tcMain.SelectedIndex == 5 ? appPath : findNewStations;
                StatusStrip_SingleLabel(statusLabelText.Equals(findNewStations), statusLabelText);
            }
            if (somethingToSave) { SaveConfig(); } // siehe 14 Zeilen weiter oben
        }

        private void StatusStrip_SingleLabel(bool isLink, string text)
        {
            toolStripStatusLabel.IsLink = isLink; // .Width = 388;
            toolStripStatusLabel.Text = text;
        }

        //private void StatusStrip_TaskInformation()
        //{
        //    for (int i = 0; i < tableActions.Rows.Count; i++)
        //    {
        //        if (tableActions.Rows[i].Field<bool>("Enabled")) //) && rgxValidTime.Match(tableActions.Rows[i].Field<string>("Time")).Success)
        //        {

        //            DateTime nowTime = DateTime.Now;
        //            int jobHour = int.TryParse(tableActions.Rows[i].Field<string>("Time").Split(':').FirstOrDefault(), out int intH) ? intH : -1;
        //            int jobMinu = int.TryParse(tableActions.Rows[i].Field<string>("Time").Split(':').LastOrDefault(), out int intM) ? intM : -1;
        //            if (jobHour < 0 || jobMinu < 0) { continue; }
        //            DateTime jobTime = new(nowTime.Year, nowTime.Month, nowTime.Day, jobHour, jobMinu, 0);
        //            if (nowTime > jobTime) { jobTime = jobTime.AddDays(1); }
        //            StatusStrip_SingleLabel(false, tableActions.Rows[i].Field<string>("Task") + " at " + jobTime.ToString("H:mm"));
        //            break;
        //        }
        //    }
        //}

        private void UpdateStatusLabelStationsList()
        {
            int fullRows = 0;
            foreach (DataGridViewRow row in dgvStations.Rows) { if (!Utilities.IsDGVRowEmpty(row)) { fullRows++; } }
            StatusStrip_SingleLabel(false, fullRows.ToString() + " entries");
        }

        private void BtnPlayStop_Click(object sender, EventArgs e)
        {
            timerResume.Stop();
            if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                if (Bass.BASS_ChannelPause(_stream))
                {
                    timerLevel.Stop();
                    spectrumTimer.Stop();
                    foreach (VerticalProgressBar vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
                    pbLevel.Image = null;
                    miniPlayer.MpPBLevel.Image = null;
                    btnPlayStop.Image = Properties.Resources.play_white;
                    miniPlayer.MpBtnPlay.Image = Properties.Resources.play_white;
                    timerPause.Enabled = true;
                    playPauseToolStripMenuItem.Text = "Play"; // btnPlayStop.Text = 
                    playPauseToolStripMenuItem.Image = Properties.Resources.play;
                    btnPlayStop.BackColor = Color.Maroon;
                    miniPlayer.MpBtnPlay.BackColor = Color.Maroon;
                }
            }
            else if (_stream != 0 && Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PAUSED)
            {
                if (Bass.BASS_ChannelPlay(_stream, false)) // false: Song beginnt von neuem, true: Spielt von aktueller position weiter
                {
                    btnPlayStop.Image = Properties.Resources.pause_white;
                    miniPlayer.MpBtnPlay.Image = Properties.Resources.pause_white;
                    timerLevel.Start();
                    spectrumTimer.Start();
                    timerPause.Enabled = false;
                    playPauseToolStripMenuItem.Text = "Pause"; // btnPlayStop.Text = 
                    playPauseToolStripMenuItem.Image = Properties.Resources.pause;
                    btnPlayStop.BackColor = SystemColors.ControlDark;
                    miniPlayer.MpBtnPlay.BackColor = SystemColors.ControlDark;
                    if (lblD4.Text.Contains(_downloads)) // Recording fand gerade statt, lblD4 enthält DownloadDateinamen
                    {
                        lblD4.ForeColor = SystemColors.ControlText;
                        lblD4.Cursor = Cursors.Default;
                        BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_stream);
                        if (info != null) { lblD4.Text = info.filename; }
                        else { lblD4.Text = string.Empty; }
                    }
                }
            }
            else { BtnReset_Click(null, null); }
        }

        private void TimerPause_Tick(object sender, EventArgs e)
        { //Server unterbricht nach ca. 30 Sekunden die Verbindung (ohne Info), dem kommen wir zuvor mittels zwangsweisem ReLoad 
            if (Bass.BASS_ChannelStop(_stream)) // BASS_STREAM_AUTOFREE: Automatically free the stream's resources when BASS_ChannelStop(Int32) is called.
            {
                btnPlayStop.BackColor = SystemColors.ControlDark;
                miniPlayer.MpBtnPlay.BackColor = SystemColors.ControlDark;
                btnPlayStop.Invalidate();
                miniPlayer.MpBtnPlay.Invalidate();
                lblD2.Text = "-";
                MiniPlayer.MpLblD2_Text(lblD2.Text);
            }
            timerPause.Enabled = false;
        }

        private void BtnIncrease_Click(object sender, EventArgs e)
        {
            Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref channelVolume);
            if (channelVolume < 1.0f)
            {
                channelVolume += 0.01f;
                channelVolume = channelVolume > 1.0f ? 1.0f : channelVolume;
                Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, channelVolume);
            }
            miniPlayer.MpVolProgBar.Value = volProgressBar.Value = (int)(channelVolume * 100f);
            lblVolume.Text = volProgressBar.Value.ToString();
            somethingToSave = true;
        }

        private void BtnDecrease_Click(object sender, EventArgs e)
        {
            Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref channelVolume);
            if (channelVolume > 0f)
            {
                channelVolume -= 0.01f;
                channelVolume = channelVolume <= 0f ? 0f : channelVolume;
                Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, channelVolume);
            }
            miniPlayer.MpVolProgBar.Value = volProgressBar.Value = (int)(channelVolume * 100f);
            lblVolume.Text = volProgressBar.Value.ToString();
            somethingToSave = true;
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            timerResume.Stop();
            RestorePlayerDefaults(_currentButtonNum); // currentButtonNum kann auch 0 sein - macht nichts
            currPlayingTime = TimeSpan.Zero;
            playPauseToolStripMenuItem.Enabled = true;
            if (_stream != 0)
            {
                Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref channelVolume);
                Bass.BASS_StreamFree(_stream);
            }
            try
            {
                foreach (RadioButton rb in tcMain.TabPages[0].Controls.OfType<RadioButton>()) // .Where(rb => rb != rbtn00)
                {
                    if (rb.Checked)
                    {
                        bool isValid = int.TryParse(rb.Tag.ToString(), out int iTag);
                        if (isValid && iTag > 0 && dgvStations.Rows.Count >= iTag && dgvStations.Rows[iTag - 1].Cells[1].Value.ToString().Length > 0)
                        {
                            UpdateCaption_lblD1(dgvStations.Rows[iTag - 1].Cells[0].Value.ToString());
                            StartPlaying(dgvStations.Rows[iTag - 1].Cells[1].Value.ToString(), iTag);
                        }
                        else
                        {
                            lblD1.Text = "ERROR";
                            lblD4.Text = "No URL is defined.";
                        }
                        break;
                    }
                }
            }
            catch (InvalidCastException ex) { MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void UpdateCaption_lblD1(string caption)
        {
            lblD1.Text = Utilities.StationLong(caption); // Regex.Replace(caption, @"\s+", " "); // doppelte Leerzeichen entfernen
            MiniPlayer.MpLblD1_Text(lblD1.Text);
        }

        private void RewriteButtonText() // initial FrmMain_Load und dann TcMain_SelectedIndexChanged 
        {
            miniPlayer.MpCmBxStations.Items.Clear();
            for (int i = 1; i <= stationSum; i++)
            {
                RadioButton foundBtn = tcMain.TabPages[0].Controls.Find("rbtn" + i.ToString("D2"), true)[0] as RadioButton;
                if (dgvStations.Rows[i - 1].Cells[1].Value != null && dgvStations.Rows[i - 1].Cells[1].Value.ToString().Length > 0) // column "URL"
                {
                    foundBtn.Text = Utilities.StationShort(dgvStations.Rows[i - 1].Cells[0].Value.ToString());
                    toolTip.SetToolTip(foundBtn, Utilities.StationLong(dgvStations.Rows[i - 1].Cells[0].Value.ToString()));
                    miniPlayer.MpCmBxStations.Items.Add(Utilities.StationLong(dgvStations.Rows[i - 1].Cells[0].Value.ToString()));
                    foundBtn.Enabled = true;
                }
                else
                {
                    foundBtn.Text = "-";
                    foundBtn.Enabled = false;
                }
            }
            //int index = miniPlayer.MpCmBxStations.FindStringExact(lblD1.Text);
            //if (index >= 0 && miniPlayer.MpCmBxStations.SelectedIndex != index) { miniPlayer.MpCmBxStations.SelectedIndex = index; }
        }

        //private static string DistillButtonText(string s)
        //{
        //    s = Regex.Replace(s, @"[\[{]([^\]|}]*)\|.*[]}]", "$1"); // innerhalb geschweifter Klammern wird Part1 genommen
        //    s = Regex.Replace(s, @"[\[{][^\]|}]*[]}]", string.Empty); // Text innerhalb eckiger Klammern wird entfernt, Zwischebereich darf keine schließende Klammer enthalten [^\]]*; [ muss maskiert werden, ] nicht
        //    s = Regex.Replace(s, @"\s+", " "); // doppelte Leerzeichen entfernen
        //    Match m = Regex.Match(s, @"(.+? .+?) ");
        //    if (m.Success) { s = m.Groups[1].Value; } // Text nach dem 2. Leerzeichen wird abgeschnitten
        //    return s.Trim();
        //}

        private void RadioButton_Paint(object sender, PaintEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                Rectangle borderRectangle = rb.ClientRectangle;
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.CompositingMode = CompositingMode.SourceOver;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
                borderRectangle.Inflate(-2, -2); //ControlPaint.DrawBorder3D(e.Graphics, borderRectangle, Border3DStyle.Flat);
                ControlPaint.DrawBorder(e.Graphics, borderRectangle,
                    SystemColors.Highlight, 1, ButtonBorderStyle.Solid, // left
                    SystemColors.Highlight, 1, ButtonBorderStyle.Solid, // top
                    SystemColors.HotTrack, 1, ButtonBorderStyle.Solid,  // right
                    SystemColors.HotTrack, 1, ButtonBorderStyle.Solid); // bottom
            }
        }

        private void DgvStations_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dGrid = sender as DataGridView;
            string rowText = (e.RowIndex + 1).ToString() + ". ";
            StringFormat centerFormat = new StringFormat(StringFormatFlags.MeasureTrailingSpaces) // default: exclude the space at the end of each line
            {
                Alignment = StringAlignment.Far, // Bei einem Layout mit Ausrichtung von links nach rechts ist die weit entfernte Position rechts.
                LineAlignment = StringAlignment.Center // vertikale Ausrichtung der Zeichenfolge
            };
            Rectangle headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dGrid.RowHeadersWidth, e.RowBounds.Height);
            Color rhForeColor = dgvStations.Rows[e.RowIndex].Index >= stationSum ? SystemColors.ControlLightLight : dGrid.RowHeadersDefaultCellStyle.ForeColor;
            using SolidBrush sBrush = new(rhForeColor);
            e.Graphics.DrawString(rowText, e.InheritedRowStyle.Font, sBrush, headerBounds, centerFormat);
        }

        private void LinkPayPal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=DK9WYLVBN7K4Y") { UseShellExecute = true };
                Process.Start(psi);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void LinkHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new("https://www.ophthalmostar.de/freeware/#netradio") { UseShellExecute = true };
                Process.Start(psi);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void PnlDisplay_Paint(object sender, PaintEventArgs e)
        {
            LinearGradientBrush myBrush = new(new Point(0, 0), new Point(Width, Height), Color.AliceBlue, Color.LightSteelBlue);
            e.Graphics.FillRectangle(myBrush, ClientRectangle);
        }

        private void CbAutostart_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutostart.Focused)
            {
                if (cbAutostart.Checked)
                {
                    if (!Utilities.IsAutoStartEnabled(appName, "\"" + appPath + "\"" + " -min"))
                    {
                        Utilities.SetAutoStart(appName, "\"" + appPath + "\"" + " -min");
                        StatusStrip_SingleLabel(false, "Autorun written to Registy");
                    }
                }
                else
                {
                    if (Utilities.IsAutoStartEnabled(appName, "\"" + appPath + "\"" + " -min"))
                    {
                        Utilities.UnSetAutoStart(appName);
                        StatusStrip_SingleLabel(false, "Autorun deleted from Registry");
                    }
                }
                somethingToSave = true;
            }
        }

        private void CbHotkey_CheckedChanged(object sender, EventArgs e)
        {
            if (cbHotkey.Focused)
            {
                if (cbHotkey.Checked)
                {// Liste automatisch öffnen (besser nicht)
                    lblHotkey.Enabled = true;
                    cmbxHotkey.Enabled = true;
                    cmbxHotkey.Focus(); //cmbxHotkey.SelectedItem = "A";
                    if (cmbxHotkey.SelectedText.Length == 1 && string.IsNullOrEmpty(hkLetter))
                    {// Regex("^[A-Z].$").IsMatch ist hier nicht erforderlich, da bereits
                        RegisterHK(cmbxHotkey.SelectedText);
                    }
                }
                else // unChecked
                {
                    if (!string.IsNullOrEmpty(hkLetter) && cmbxHotkey.Enabled && NativeMethods.UnregisterHotKey(Handle, NativeMethods.HOTKEY_ID))
                    {
                        StatusStrip_SingleLabel(false, "Hotkey unregistered");
                        hkLetter = string.Empty;
                    }
                    lblHotkey.Enabled = false;
                    cmbxHotkey.Enabled = false;
                }
                somethingToSave = true;
            }
        }

        private void CmbxHotkey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbxHotkey.Visible && cmbxHotkey.Focused && cmbxHotkey.Enabled)
            {
                if (!string.IsNullOrEmpty(hkLetter) && NativeMethods.UnregisterHotKey(Handle, NativeMethods.HOTKEY_ID))
                {// 1. Schritt: vorhanden Hotkey löschen
                    StatusStrip_SingleLabel(false, "Hotkey unregistered");
                    hkLetter = string.Empty;
                }
                if (string.IsNullOrEmpty(hkLetter) && cbHotkey.Checked && new Regex("^[A-Z]+$").IsMatch(cmbxHotkey.Text))
                {// 2. Schritt: neuen Hotkey registrieren
                    RegisterHK(cmbxHotkey.Text); //  MessageBox.Show("RegisterHK(cmbxHotkey.Text)");
                }
                somethingToSave = true;
            }
        }

        private void CmbxHotkey_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(cmbxHotkey.Text))
            {
                lblHotkey.Enabled = false;
                cmbxHotkey.Enabled = false;
            }
        }

        private void RegisterHK(string hkString)
        {
            if (NativeMethods.RegisterHotKey(Handle, NativeMethods.HOTKEY_ID, (uint)(NativeMethods.Modifiers.Control | NativeMethods.Modifiers.Win), (uint)(Keys)Convert.ToChar(hkString)) == true)
            {
                StatusStrip_SingleLabel(false, "Hotkey registered (Ctrl+Win+" + hkString + ")");
                toolStripStatusLabel.IsLink = false;
                hkLetter = hkString;
            }
            else
            {
                hkLetter = string.Empty;
                cbHotkey.Checked = false;
                cmbxHotkey.SelectedIndex = 0;
                tcMain.SelectedIndex = 3; // Hotkey-Dialog anzeigen
                StatusStrip_SingleLabel(false, "Sorry, another application is using this hotkey!");
                cmbxHotkey.Enabled = false;
                lblHotkey.Enabled = false;
            }
        }

        private void CmbxOutput_CreateContent()
        {
            List<string> devicelist = new();
            BASS_DEVICEINFO info; // = new BASS_DEVICEINFO();
            for (int n = 0; (info = Bass.BASS_GetDeviceInfo(n)) != null; n++) { if (info.IsEnabled) { devicelist.Add(info.ToString()); } }
            if (devicelist[0].Contains("No sound")) { devicelist.RemoveAt(0); } // 0: No sound
            if (devicelist.Count != 0) // if (!list.Any())
            {
                devicelist[0] += " (recommended)";
                cmbxOutput.Items.Clear();
                cmbxOutput.Items.AddRange(devicelist.ToArray());
                if (_stream != 0)
                {
                    int device = Bass.BASS_ChannelGetDevice(_stream); // 0 = no sound, 1 = default
                    intOutputDevice = device <= 1 || device == 0x20000 ? 0 : device - 1; // const int bass_nodevice = 0x20000;
                }
                else { intOutputDevice = cmbxOutput.FindString(strOutputDevice); } // Index des Elements, das mit der Zeichenfolge beginnt...
                cmbxOutput.SelectedIndex = intOutputDevice > 0 && devicelist.Count >= intOutputDevice ? intOutputDevice : 0;
            }
        }

        private void CmbxOutput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (changeOutputDevice && cmbxOutput.Visible && cmbxOutput.Focused)
            {
                intOutputDevice = cmbxOutput.SelectedIndex;
                strOutputDevice = cmbxOutput.Items[cmbxOutput.SelectedIndex].ToString();
                strOutputDevice = strOutputDevice.StartsWith("Default") ? "Default" : strOutputDevice; // (recommended) entfernen
                if (prevOutputDevice != strOutputDevice) { somethingToSave = true; }
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    BASS_CHANNELINFO info = Bass.BASS_ChannelGetInfo(_stream);
                    if (info != null)
                    {
                        Bass.BASS_ChannelStop(_stream);
                        Bass.BASS_Free();
                        StartPlaying(info.filename, _currentButtonNum);
                        TPSettings_SetStatusBarText();
                    }
                }
                prevOutputDevice = strOutputDevice; //somethingToSave = false; s. o.
            }
        }

        private void CmbxOutput_DropDown(object sender, EventArgs e)
        {
            changeOutputDevice = false;
            if (Bass.BASS_GetDeviceCount() != cmbxOutput.Items.Count) { CmbxOutput_CreateContent(); }
        }

        private void CmbxOutput_DropDownClosed(object sender, EventArgs e) { changeOutputDevice = true; }

        private void TPHistory_SetStatusBarText()
        {
            //string statusStripText = string.Empty;
            int count = historyListView.Items.Count;
            if (count > 0)
            {
                //DateTime min = (from m in historyListView.Items.Cast<ListViewItem>() select DateTime.Parse(m.Tag.ToString(), null, DateTimeStyles.RoundtripKind)).Min(); // Tag enthält DateTime (Tag und Zeit)
                //DateTime max = (from m in historyListView.Items.Cast<ListViewItem>() select DateTime.Parse(m.Tag.ToString(), null, DateTimeStyles.RoundtripKind)).Max(); // dadurch stimmt Anzeige auch um 24:00 Uhr
                //StatusStrip_SingleLabel(false, count + (count == 1 ? " entry (" : " entries (") + (max - min).ToString(@"hh\:mm\:ss") + ")");
                StatusStrip_SingleLabel(false, count + (count == 1 ? " entry (" : " entries (") + totalPlayingTime.ToString(@"hh\:mm\:ss") + ")");
            }
            else { StatusStrip_SingleLabel(false, string.Empty); }
        }

        private void TPSettings_SetStatusBarText()
        {
            string statusStripText = string.Empty;
            BASS_DEVICEINFO info; // = new BASS_DEVICEINFO();
            for (int n = 1; (info = Bass.BASS_GetDeviceInfo(n)) != null; n++) // n = 1 => Default
            {
                if (n == intOutputDevice + 1) { statusStripText = "Current output: " + info.ToString() + (n == 1 ? " (adjusted by system settings, press F8)" : ""); break; } // info.IsInitialized funkt nicht
            }
            StatusStrip_SingleLabel(false, statusStripText);
        }

        private void CbAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAlwaysOnTop.Focused)
            {
                if (cbAlwaysOnTop.Checked) { alwaysOnTop = true; }
                else { alwaysOnTop = false; }
                TopMost = alwaysOnTop;
                somethingToSave = true;
            }
        }

        private void CbAutoStopRecording_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAutoStopRecording.Focused)
            {
                if (cbAutoStopRecording.Checked) { autoStopRecording = true; }
                else { autoStopRecording = false; }
                somethingToSave = true;
            }
        }

        private void CbShowBalloonTip_CheckedChanged(object sender, EventArgs e)
        {
            if (cbShowBalloonTip.Focused)
            {
                if (cbShowBalloonTip.Checked) { showBalloonTip = true; }
                else { showBalloonTip = false; }
                somethingToSave = true;
            }
        }

        private void FrmMain_Shown(object sender, EventArgs e)
        {
            miniPlayer.Hide();
            miniPlayer.Opacity = 1;
            if (!string.IsNullOrEmpty(hkLetter) && new Regex("^[A-Z]+$").IsMatch(hkLetter)) { RegisterHK(hkLetter); } // Hotkey kann erst registriert werden, wenn das Fenster erstellt wurde
            if (startMin)
            {
                startMin = false;
                Hide();
                Opacity = 1; // nach Hide //notifyIcon.ShowBalloonTip(1, Text, "Autostart", ToolTipIcon.Info);
                if (int.TryParse(autostartStation, out int i) && i > 0) { UpdateCaption_lblD1(dgvStations.Rows[i - 1].Cells[0].Value.ToString()); }
                ShowMiniPlayer();
            }
            if (alwaysOnTop) { TopMost = true; }
            Application.DoEvents();
            if (autoStartRadioButton != null)
            {
                autoStartRadioButton.Checked = true;
                autoStartRadioButton.Focus();
            } //else { MessageBox.Show(this.ActiveControl.Name); }; tcMa
        }

        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F2 && tcMain.SelectedIndex == 0)
            {
                int rbi = 0;
                foreach (RadioButton rb in tcMain.TabPages[0].Controls.OfType<RadioButton>())
                {
                    if (rb.Checked) { rbi = Convert.ToInt32(rb.Tag) - 1; break; }
                }
                tcMain.SelectedIndex = 1;
                dgvStations.Rows[rbi].Selected = true;
                dgvStations.CurrentCell = dgvStations.Rows[rbi].Cells[0]; // wg. F2, öffnet sonst 1. Zeile
            }
            else if (e.KeyCode == Keys.F2 && tcMain.SelectedIndex >= 2)
            {
                tcMain.SelectedIndex = 1; // Stations
            }
            else if (e.KeyCode == Keys.F5 && tcMain.SelectedTab != tpHistory)
            {
                tcMain.SelectedIndex = 2; // History
            }
            else if (e.KeyCode == Keys.F4 && tcMain.SelectedTab != tpPlayer)
            {
                tcMain.SelectedIndex = 0; // Player
            }
            else if (e.KeyCode == Keys.F5 && tcMain.SelectedTab == tpHistory && historyListView.Items.Count > 0)
            {
                lviComparer.SortColumn = 0;
                lviComparer.Order = SortOrder.Descending;
                historyListView.Refresh(); // Arrows auf anderen ColumnHeader-Buttons werden entfernt
                historyListView.Sort(); // MessageBox.Show(historyListView.TopItem.Tag.ToString()); // 2023-04-14T12:59:06.3463796Z
                lvSortOrderArray[0] = "Descending";
            }
            else if (e.KeyCode == Keys.F8 && tcMain.SelectedTab != tpSettings)
            {
                tcMain.SelectedIndex = 3; // Setting
            }
            else if (e.KeyCode == Keys.F8 && tcMain.SelectedTab == tpSettings)
            {
                ControlToolStripMenuItem_Click(null, null);
            }
            else if (e.KeyCode == Keys.F9 && tcMain.SelectedTab != tpHelp)
            {
                tcMain.SelectedIndex = 4; // Spectrum
            }
            else if (e.KeyCode == Keys.F9 && tcMain.SelectedTab == tpHelp)
            {
                tcMain.SelectedIndex = 0; // Player
            }
            else if (e.KeyCode == Keys.F11 && tcMain.SelectedTab != tpInfo)
            {
                tcMain.SelectedIndex = 5; // Information
            }
            else if (e.KeyCode == Keys.F11 && tcMain.SelectedTab == tpInfo)
            {
                tcMain.SelectedIndex = 0; // Player
            }
            else if (e.KeyCode == Keys.F12 && tcMain.SelectedTab != tpSectrum)
            {
                tcMain.SelectedIndex = 6; // Spectrum
            }
            else if (e.KeyCode == Keys.F12 && tcMain.SelectedTab == tpSectrum)
            {
                tcMain.SelectedIndex = 0; // Player
            }
            else if (e.KeyCode == Keys.G && e.Modifiers == Keys.Control && (tcMain.SelectedIndex == 0 || tcMain.SelectedIndex == 2))
            {
                GoogleToolStripMenuItem_Click(null, null);
            }
            else if (((e.KeyCode == Keys.F && e.Modifiers == Keys.Control) || e.KeyCode == Keys.F3) && tcMain.SelectedIndex <= 1)
            {
                e.Handled = true;
                BtnSearch_Click(null, null);
            }
            else if (tcMain.SelectedIndex == 0)
            {
                if (e.KeyCode == Keys.Oemplus) { btnIncrease.PerformClick(); btnIncrease.Focus(); }
                else if (e.KeyCode == Keys.OemMinus) { btnDecrease.PerformClick(); btnDecrease.Focus(); }
                else if (e.KeyCode == Keys.Add) { btnIncrease.PerformClick(); btnIncrease.Focus(); }
                else if (e.KeyCode == Keys.Subtract) { btnDecrease.PerformClick(); btnDecrease.Focus(); }
                else if (e.KeyCode == Keys.Back) { btnReset.PerformClick(); btnReset.Focus(); }
                else if (e.KeyCode == Keys.Insert) { btnRecord.PerformClick(); btnRecord.Focus(); }
                else if (e.KeyCode == Keys.D1) { rbtn01.Checked = true; rbtn01.Focus(); }
                else if (e.KeyCode == Keys.D2) { rbtn02.Checked = true; rbtn02.Focus(); }
                else if (e.KeyCode == Keys.D3) { rbtn03.Checked = true; rbtn03.Focus(); }
                else if (e.KeyCode == Keys.D4) { rbtn04.Checked = true; rbtn04.Focus(); }
                else if (e.KeyCode == Keys.D5) { rbtn05.Checked = true; rbtn05.Focus(); }
                else if (e.KeyCode == Keys.D6) { rbtn06.Checked = true; rbtn06.Focus(); }
                else if (e.KeyCode == Keys.D7) { rbtn07.Checked = true; rbtn07.Focus(); }
                else if (e.KeyCode == Keys.D8) { rbtn08.Checked = true; rbtn08.Focus(); }
                else if (e.KeyCode == Keys.D9) { rbtn09.Checked = true; rbtn09.Focus(); }
                else if (e.KeyCode == Keys.NumPad1) { rbtn01.Checked = true; rbtn01.Focus(); }
                else if (e.KeyCode == Keys.NumPad2) { rbtn02.Checked = true; rbtn02.Focus(); }
                else if (e.KeyCode == Keys.NumPad3) { rbtn03.Checked = true; rbtn03.Focus(); }
                else if (e.KeyCode == Keys.NumPad4) { rbtn04.Checked = true; rbtn04.Focus(); }
                else if (e.KeyCode == Keys.NumPad5) { rbtn05.Checked = true; rbtn05.Focus(); }
                else if (e.KeyCode == Keys.NumPad6) { rbtn06.Checked = true; rbtn06.Focus(); }
                else if (e.KeyCode == Keys.NumPad7) { rbtn07.Checked = true; rbtn07.Focus(); }
                else if (e.KeyCode == Keys.NumPad8) { rbtn08.Checked = true; rbtn08.Focus(); }
                else if (e.KeyCode == Keys.NumPad9) { rbtn09.Checked = true; rbtn09.Focus(); }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Escape:
                case Keys.Escape | Keys.Shift:
                    {
                        if (dgvStations.CurrentCell != null && dgvStations.IsCurrentCellInEditMode)
                        {
                            dgvStations.EndEdit();
                            dgvStations.CurrentCell.Selected = true;
                        }
                        else if (tcMain.SelectedIndex != 0) { tcMain.SelectedIndex = 0; }
                        else
                        {
                            if ((ModifierKeys & Keys.Shift) == Keys.Shift) { Close(); } // Ctrl + Esc: Open Start.
                            else
                            {
                                Hide(); //ShowInTaskbar = false; verträgt sich nicht mit GlobalHotkey => zerstört Handle
                                ShowMiniPlayer();
                                tcMain.SelectedIndex = 0;
                            }
                        }
                        return true;
                    }
                case Keys.Space:
                    {
                        if (tcMain.SelectedIndex == 0 && !btnPlayStop.Focused)
                        {
                            btnPlayStop.PerformClick(); NativeMethods.SetFocus(btnPlayStop.Handle); return true;
                        }
                        else { return false; }
                    }
                case Keys.Q | Keys.Control: { Close(); return true; } // exitFlag = true;
                case Keys.F1 | Keys.Control | Keys.Shift: { helpRequested = false; StartXMLFile(xmlPath); return true; }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private static void StartXMLFile(string filePath)
        {
            try
            {
                ProcessStartInfo psi = new(filePath) { UseShellExecute = true }; // for non-executables
                Process.Start(psi);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void FrmMain_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            ShowHelpPDF();
        }

        private void FrmMain_HelpRequested(object sender, HelpEventArgs hlpevent)
        {// Das HelpRequested-Ereignis wird ausgelöst, wenn der Benutzer F1 drückt
            if (helpRequested)
            {
                hlpevent.Handled = true;
                ShowHelpPDF();
            }
            else { helpRequested = true; } // ist der Fall, wenn Strg+Shift+F1 gedrückt wurde
        }

        private static async void ShowHelpPDF()
        {
            string pdfPath = Path.ChangeExtension(appPath, ".pdf"); // Path.Combine(Path.GetDirectoryName(appPath), appName + ".pdf")
            if (File.Exists(pdfPath))
            {
                Process pdfProcess = new();
                pdfProcess.StartInfo.FileName = pdfPath;
                pdfProcess.StartInfo.UseShellExecute = true;
                pdfProcess.Start();
            }
            else
            {
                if (MessageBox.Show(Path.GetFileName(pdfPath) + " was not found in the program directory.\nWould you like to download it from the Internet?", appName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    httpClient ??= new HttpClient();
                    using (HttpResponseMessage response = await httpClient.GetAsync("https://www.ophthalmostar.de/NetRadio.pdf"))
                    {
                        using FileStream streamToWriteTo = new("NetRadio.pdf", FileMode.CreateNew);
                        await response.Content.CopyToAsync(streamToWriteTo);
                    }
                    ShowHelpPDF();
                }
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {// Form.Closed and Form.Closing events are not raised when the Application.Exit method is called; use Form.Close
            SystemEvents.PowerModeChanged -= new PowerModeChangedEventHandler(PowerMode_Changed);
            timerLevel.Stop();
            spectrumTimer.Stop();
            Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref channelVolume); // muss vor BASS_StreamFree
            RecordingStop(); // enthält BASS_StreamFree! - channelVolume muss vorher gespeichert werden!
            notifyIcon.Visible = false; // keine komische Meldungen an Windows-Nachrichtenzentrale
            if (!string.IsNullOrEmpty(hkLetter)) { NativeMethods.UnregisterHotKey(Handle, NativeMethods.HOTKEY_ID); }
            Bass.BASS_PluginFree(_hlsPlugIn);
            Bass.BASS_PluginFree(_flacPlugIn);
            Bass.BASS_PluginFree(_opusPlugIn);
            Bass.BASS_Stop();
            Bass.BASS_Free();
            if (somethingToSave || radioBtnChanged) { SaveConfig(); }
        }

        private void SaveConfig()
        {
            string strVolume = ((int)(channelVolume * 100f)).ToString();
            XmlWriterSettings xwSettings = new()
            {
                IndentChars = "\t",
                NewLineHandling = NewLineHandling.Entitize,
                Indent = true,
                NewLineChars = "\n"
            };
            try
            {
                using XmlWriter xw = XmlWriter.Create(xmlPath, xwSettings);
                xw.WriteStartDocument();
                xw.WriteStartElement("NetRadio");

                for (int i = 0; i < dgvStations.RowCount; ++i)
                {
                    xw.WriteStartElement("Station");
                    object v = dgvStations.Rows[i].Cells[0].Value;
                    xw.WriteAttributeString("Name", v != null ? dgvStations.Rows[i].Cells[0].Value.ToString() : "");
                    v = dgvStations.Rows[i].Cells[1].Value;
                    xw.WriteAttributeString("URL", v != null ? dgvStations.Rows[i].Cells[1].Value.ToString() : "");
                    xw.WriteEndElement(); // für Radio
                }

                xw.WriteStartElement("Hotkey");
                xw.WriteAttributeString("Enabled", cbHotkey.Checked == true ? "1" : "0");
                xw.WriteAttributeString("Letter", hkLetter); // HIER FEHLT NOCH WAS
                xw.WriteEndElement(); // für HotkeynotifyIcon

                xw.WriteStartElement("Output");
                xw.WriteAttributeString("Device", strOutputDevice);
                xw.WriteEndElement(); // für Output

                xw.WriteStartElement("MiniPlayer");
                xw.WriteAttributeString("Enabled", showBalloonTip == true ? "1" : "0");
                xw.WriteEndElement(); // für MiniPlayer

                xw.WriteStartElement("AlwaysOnTop");
                xw.WriteAttributeString("Enabled", alwaysOnTop == true ? "1" : "0");
                xw.WriteEndElement(); // für AlwaysOnTop

                xw.WriteStartElement("LogHistory");
                xw.WriteAttributeString("Enabled", logHistory == true ? "1" : "0");
                xw.WriteEndElement(); // für LogHistory

                xw.WriteStartElement("AutoStopRecording");
                xw.WriteAttributeString("Enabled", autoStopRecording == true ? "1" : "0");
                xw.WriteEndElement(); // für AutoStopRecording

                xw.WriteStartElement("Volume");
                xw.WriteAttributeString("Value", strVolume);
                xw.WriteEndElement(); // für Volume

                xw.WriteStartElement("FormLocation"); // RestoreBounds.Location funktioniert nicht richtig
                xw.WriteAttributeString("PosX", Location.X.ToString());
                xw.WriteAttributeString("PosY", Location.Y.ToString());
                xw.WriteEndElement(); // für InitialLocation

                xw.WriteStartElement("MiniLocation"); // RestoreBounds.Location funktioniert nicht richtig
                xw.WriteAttributeString("PosX", miniPlayer.Location.X.ToString());
                xw.WriteAttributeString("PosY", miniPlayer.Location.Y.ToString());
                xw.WriteEndElement(); // für InitialLocation

                xw.WriteStartElement("Autostart");
                xw.WriteAttributeString("Station", autostartStation);
                xw.WriteEndElement(); // für Autostart

                xw.WriteStartElement("KeepActionsActive");
                xw.WriteAttributeString("Enabled", keepActionsActive == true ? "1" : "0");
                xw.WriteEndElement(); // für KeepActionsActive

                for (int i = 0; i < tableActions.Rows.Count; ++i)
                {
                    xw.WriteStartElement("Action");
                    xw.WriteAttributeString("Enabled", tableActions.Rows[i].Field<bool>("Enabled").ToString());
                    xw.WriteAttributeString("Task", tableActions.Rows[i].Field<string>("Task"));
                    xw.WriteAttributeString("Station", tableActions.Rows[i].Field<string>("Station"));
                    xw.WriteAttributeString("Time", tableActions.Rows[i].Field<string>("Time"));
                    xw.WriteEndElement(); // für Action
                }

                xw.WriteEndElement(); // für NetRadio
                xw.WriteEndDocument();
            }
            catch (ArgumentNullException ex) { MessageBox.Show(ex.Message); }
            somethingToSave = false;
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) { ShowToolStripMenuItem_Click(null, null); }
        }

        //private void ContextMenuTrayIcon_Opening(object sender, CancelEventArgs e) { showToolStripMenuItem.Text = Visible ? "Hide" : "Show"; }

        private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (NativeMethods.HitTest(Bounds, Handle, PointToScreen(Point.Empty)))
                {
                    Hide();
                    ShowMiniPlayer();
                }
                else { ShowMe(); }
            }
            else if (!Visible)
            {
                if (NativeMethods.HitTest(miniPlayer.Bounds, miniPlayer.Handle, miniPlayer.PointToScreen(Point.Empty))) { ShowMe(); }
                else { miniPlayer.Activate(); }
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e) { Close(); }

        internal void BtnSearch_Click(object sender, EventArgs e)
        {
            if (tcMain.SelectedIndex != 1) // && tcMain.SelectedIndex != 0 &&
            {
                tcMain.SelectedIndex = 1;
                for (int row = 0; row < dgvStations.RowCount; row++)
                {
                    if (Utilities.IsDGVRowEmpty(dgvStations.Rows[row]))
                    {
                        dgvStations.Rows[row].Selected = true;
                        dgvStations.CurrentCell = dgvStations.Rows[row].Cells[0]; // wg. F2, öffnet sonst 1. Zeile
                        dgvStations.FirstDisplayedScrollingRowIndex = dgvStations.SelectedRows[0].Index;
                        break;
                    }
                }
            }
            if (dgvStations.SelectedRows.Count > 0)
            {
                DataGridViewCell dgvCellName = dgvStations.SelectedRows[0].Cells[0];
                string currRow = (dgvStations.SelectedRows[0].Index + 1).ToString();
                string currName;
                if (dgvCellName.Value != null && !string.IsNullOrEmpty(dgvCellName.Value.ToString())) { currName = Utilities.StationShort(dgvCellName.Value.ToString()); }
                else { currName = "[empty]"; }
                tcMain.SelectedIndex = 1; // Sendertabelle
                using FrmSearch frmSearch = new(currRow, currName);
                if (alwaysOnTop) { frmSearch.TopMost = true; }
                if (frmSearch.ShowDialog() == DialogResult.OK)
                {
                    string searchString = frmSearch.TbString.Text;
                    if (searchString.Length > 0)
                    {
                        using FrmBrowser frmBrowser = new(searchString.Trim(), Location);
                        if (alwaysOnTop) { frmBrowser.TopMost = true; }
                        DialogResult result = frmBrowser.ShowDialog();
                        if (result == DialogResult.OK)
                        {
                            currName = string.IsNullOrEmpty(currName) ? (dgvStations.SelectedRows[0].Index + 1) + ". row" : currName;
                            if (dgvStations.SelectedRows[0].Cells[1].Value != null && !string.IsNullOrEmpty(dgvStations.SelectedRows[0].Cells[1].Value.ToString()) &&
                                MessageBox.Show("Overwrite " + currName + "?", appName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
                            dgvStations.SelectedRows[0].Cells[0].Value = frmBrowser.SelectedStation;
                            dgvStations.SelectedRows[0].Cells[1].Value = frmBrowser.SelectedURL;
                        }
                    }
                    if (firstEmptyStart)  // für ein schnelles Erfolgselebnis
                    {
                        string url = dgvStations.Rows[0].Cells[1].Value.ToString();
                        if (currRow == "1" && !string.IsNullOrEmpty(url))
                        {
                            tcMain.SelectedIndex = 0; // nach StartPlaying 
                            BeginInvoke(() => { (tcMain.TabPages[0].Controls["rbtn01"] as RadioButton).Checked = true; }); // Workaround weil sonst timer nicht funktionieren =>  StartPlaying(url, 1);
                        }
                        firstEmptyStart = false;
                    }
                }
            }
            else { MessageBox.Show("Target not selected!", appName, MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            int idx = dgvStations.SelectedCells[0].OwningRow.Index;
            if (idx != 0)
            {
                DataGridViewRowCollection rows = dgvStations.Rows;
                DataGridViewRow row = rows[idx];
                rows.Remove(row);
                rows.Insert(idx - 1, row);
                dgvStations.ClearSelection();
                dgvStations.CurrentCell = dgvStations.Rows[idx - 1].Cells[0];
                dgvStations.Rows[idx - 1].Selected = true;
                //if (dgvStations.FirstDisplayedScrollingRowIndex == 15 && idx > 16) { dgvStations.FirstDisplayedScrollingRowIndex = 16; } // MessageBox.Show("FDSRI:\t" + dgvStations.FirstDisplayedScrollingRowIndex.ToString() + Environment.NewLine + "idx:\t" + idx.ToString());
                //else if (dgvStations.FirstDisplayedScrollingRowIndex >= idx - 1) { dgvStations.FirstDisplayedScrollingRowIndex = idx - 1; } // MessageBox.Show("FDSRI:\t" + dgvStations.FirstDisplayedScrollingRowIndex.ToString() + Environment.NewLine + "idx:\t" + idx.ToString());
            }
            dgvStations.Focus();
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            int totalRows = dgvStations.Rows.Count;
            int idx = dgvStations.SelectedCells[0].OwningRow.Index;
            if (idx != totalRows - 1)
            {// int col = dgvStations.SelectedCells[0].OwningColumn.Index;
                DataGridViewRowCollection rows = dgvStations.Rows;
                DataGridViewRow row = rows[idx];
                rows.Remove(row);
                rows.Insert(idx + 1, row);
                dgvStations.ClearSelection();
                dgvStations.CurrentCell = dgvStations.Rows[idx + 1].Cells[0];
                dgvStations.Rows[idx + 1].Selected = true;
                //if (dgvStations.FirstDisplayedScrollingRowIndex < idx - 6) { dgvStations.FirstDisplayedScrollingRowIndex = idx - 6; }
            }
            dgvStations.Focus();
        }

        private void DgvStations_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Insert) { AddToolStripMenuItem_Click(null, null); }
            else if (e.KeyCode == Keys.Delete) { DeleteToolStripMenuItem_Click(null, null); }
            else if (e.KeyCode == Keys.D1 && e.Modifiers == Keys.Alt || e.KeyCode == Keys.NumPad1 && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(0, e); }
            else if (e.KeyCode == Keys.D2 && e.Modifiers == Keys.Alt || e.KeyCode == Keys.NumPad2 && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(1, e); }
            else if (e.KeyCode == Keys.D3 && e.Modifiers == Keys.Alt || e.KeyCode == Keys.NumPad3 && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(2, e); }
            else if (e.KeyCode == Keys.D4 && e.Modifiers == Keys.Alt || e.KeyCode == Keys.NumPad4 && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(3, e); }
            else if (e.KeyCode == Keys.D5 && e.Modifiers == Keys.Alt || e.KeyCode == Keys.NumPad5 && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(4, e); }
            else if (e.KeyCode == Keys.D6 && e.Modifiers == Keys.Alt || e.KeyCode == Keys.NumPad6 && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(5, e); }
            else if (e.KeyCode == Keys.D7 && e.Modifiers == Keys.Alt || e.KeyCode == Keys.NumPad7 && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(6, e); }
            else if (e.KeyCode == Keys.D8 && e.Modifiers == Keys.Alt || e.KeyCode == Keys.NumPad8 && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(7, e); }
            else if (e.KeyCode == Keys.D9 && e.Modifiers == Keys.Alt || e.KeyCode == Keys.NumPad9 && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(8, e); }
            else if (e.KeyCode == Keys.Home && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(0, e); }
            else if (e.KeyCode == Keys.End && e.Modifiers == Keys.Alt) { KeyDown_MoveRowAt(dgvStations.RowCount - 1, e); }
            else if (e.KeyCode == Keys.Up && e.Modifiers == Keys.Alt) { BtnUp_Click(null, null); e.Handled = true; }
            else if (e.KeyCode == Keys.Down && e.Modifiers == Keys.Alt) { BtnDown_Click(null, null); e.Handled = true; }
            else if (e.KeyCode == Keys.PageUp && e.Modifiers == Keys.Alt)
            {
                for (int j = 0; j < 8; j++)
                {
                    BtnUp_Click(null, null);
                    if (dgvStations.SelectedRows[0].Index < 1) { break; }
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.PageDown && e.Modifiers == Keys.Alt)
            {
                for (int j = 0; j < 8; j++)
                {
                    BtnDown_Click(null, null);
                    if (dgvStations.SelectedRows[0].Index >= dgvStations.RowCount - 1) { break; }
                }
                e.Handled = true;
            }
        }

        private void KeyDown_MoveRowAt(int rowIndex, KeyEventArgs kEA = null)
        {
            if (kEA != null) { kEA.Handled = true; kEA.SuppressKeyPress = true; }
            int idx = dgvStations.SelectedRows[0].Index;
            DataGridViewRowCollection rows = dgvStations.Rows;
            DataGridViewRow row = rows[idx];
            dgvStations.Rows.RemoveAt(idx);
            dgvStations.Rows.Insert(rowIndex, row);
            dgvStations.Rows[rowIndex].Selected = true;
            dgvStations.CurrentCell = dgvStations.Rows[rowIndex].Cells[0]; // bewirkt Scroll
        }

        private void DgvStations_SelectionChanged(object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            int ri = -1;
            foreach (DataGridViewCell cell in dgv.SelectedCells)
            {
                ri = cell.RowIndex;
            }
            if (ri == 0)
            {
                btnUp.Enabled = false;
                btnDown.Enabled = true;
            }
            else if (ri == dgvStations.Rows.Count - 1)
            {
                btnUp.Enabled = true;
                btnDown.Enabled = false;
            }
            else
            {
                btnUp.Enabled = true;
                btnDown.Enabled = true;
            }
        }

        private void DgvStations_MouseMove(object sender, MouseEventArgs e)
        {// if (e.Button == MouseButtons.Left)
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {// If the mouse moves outside the rectangle, start the drag.
                if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
                {
                    //DragDropEffects dropEffect = 
                    dgvStations.DoDragDrop(dgvStations.Rows[rowIndexFromMouseDown], DragDropEffects.Move);
                }
            }
        }

        private void DgvStations_MouseDown(object sender, MouseEventArgs e)
        {// Get the index of the item the mouse is below
            rowIndexFromMouseDown = dgvStations.HitTest(e.X, e.Y).RowIndex;
            colIndexFromMouseDown = dgvStations.HitTest(e.X, e.Y).ColumnIndex;
            if (e.Button == MouseButtons.Right)
            {
                dgvStations.ClearSelection();
                dgvStations.Rows[rowIndexFromMouseDown].Selected = true;
            }
            else
            {
                if (rowIndexFromMouseDown != -1)
                {// Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.
                    Size dragSize = SystemInformation.DragSize;
                    dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
                }
                else
                {// Reset the rectangle if the mouse is not over an item
                    dragBoxFromMouseDown = Rectangle.Empty;
                }
            }
        }

        private void DataGridView_DragOver(object sender, DragEventArgs e)
        {// The PointToScreen conversion as dgvStations.Location.X will give co-ordinates relative to the hosted form and e.Y gives co-ordinates relative to the screen.
            if (e.Y <= PointToScreen(new Point(dgvStations.Location.X, dgvStations.Location.Y)).Y + dgvStations.Columns[0].HeaderCell.Size.Height * 2) { e.Effect = DragDropEffects.None; }
            else { e.Effect = DragDropEffects.Move; }
            int sensitveSpace = 8;
            if (e.Y <= PointToScreen(new Point(dgvStations.Location.X, dgvStations.Location.Y)).Y + dgvStations.Columns[0].HeaderCell.Size.Height * 2 + sensitveSpace)
            {// Maus nach oben
                if (dgvStations.FirstDisplayedScrollingRowIndex > 0) { dgvStations.FirstDisplayedScrollingRowIndex -= 1; }
            }
            else if (e.Y >= PointToScreen(new Point(dgvStations.Location.X + dgvStations.Width, dgvStations.Location.Y + dgvStations.Height)).Y + dgvStations.Rows[0].Height - sensitveSpace)
            {// Maus nach unten
                if (dgvStations.FirstDisplayedScrollingRowIndex <= dgvStations.RowCount) { dgvStations.FirstDisplayedScrollingRowIndex += 1; }
            }
        }

        private void DgvStations_DragDrop(object sender, DragEventArgs e)
        {// The mouse locations are relative to the screen, so they must be converted to client coordinates.
            Point clientPoint = dgvStations.PointToClient(new Point(e.X, e.Y));
            rowIndexOfItemUnderMouseToDrop = dgvStations.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            if (e.Effect == DragDropEffects.Move)
            {// If the drag operation was a move then remove and insert the row.
                DataGridViewRow rowToMove = e.Data.GetData(typeof(DataGridViewRow)) as DataGridViewRow;
                dgvStations.Rows.RemoveAt(rowIndexFromMouseDown);
                dgvStations.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove);
                if (rowIndexOfItemUnderMouseToDrop > 16) { dgvStations.FirstDisplayedScrollingRowIndex += 1; }
                //if (rowIndexOfItemUnderMouseToDrop >= 0) { dgvStations.ClearSelection(); }
                dgvStations.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
                dgvStations.CurrentCell = dgvStations.Rows[rowIndexOfItemUnderMouseToDrop].Cells[0];
            }
        }

        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvStations.SelectedRows.Count > 0)
            {
                if (!Utilities.IsDGVRowEmpty(dgvStations.SelectedRows[0]))
                {
                    DataGridViewCell dgvc = dgvStations.SelectedRows[0].Cells[0];
                    string currName;
                    if (dgvc.Value != null && !string.IsNullOrEmpty(dgvc.Value.ToString())) { currName = dgvc.Value.ToString(); }
                    else { currName = (dgvStations.SelectedRows[0].Index + 1) + ". row"; }
                    if (MessageBox.Show("Do you want to delete " + currName + "?", appName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) { return; }
                }
                dgvStations.Rows.RemoveAt(dgvStations.SelectedRows[0].Index);
                dgvStations.Rows.Insert(dgvStations.Rows.Count); // -1 entfällt, weil eine Zeile gelöscht wurde!
                dgvStations.CurrentCell = dgvStations.Rows[dgvStations.SelectedRows[0].Index].Cells[0]; // scrollt! //dgvStations.FirstDisplayedScrollingRowIndex = dgvStations.SelectedRows[0].Index;
            }
        }

        private void SearchStationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void AddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bool isAdded = false;
            for (int row = dgvStations.RowCount - 1; row >= dgvStations.SelectedRows[0].Index; row--)
            {
                if (Utilities.IsDGVRowEmpty(dgvStations.Rows[row]))
                {
                    if (dgvStations.SelectedRows[0].Index != row)
                    {
                        dgvStations.Rows.RemoveAt(row--); // deincrement (after the call) since we are removing the row
                        dgvStations.Rows.Insert(dgvStations.SelectedRows[0].Index);
                        dgvStations.Rows[dgvStations.SelectedRows[0].Index - 1].Selected = true;
                        dgvStations.CurrentCell = dgvStations.Rows[dgvStations.SelectedRows[0].Index].Cells[0]; // scrollt!                //dgvStations.FirstDisplayedScrollingRowIndex = dgvStations.SelectedRows[0].Index;
                        isAdded = true;
                        break;
                    }
                }
            }
            if (!isAdded) { Console.Beep(); } // MessageBox.Show("Sorry!"); }
        }

        private void Row1ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(0); }
        private void Row2ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(1); }
        private void Row3ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(2); }
        private void Row4ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(3); }
        private void Row5ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(4); }
        private void Row6ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(5); }
        private void Row7ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(6); }
        private void Row8ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(7); }
        private void Row9ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(8); }
        private void Row10ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(9); }
        private void Row11ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(10); }
        private void Row12ToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(11); }
        private void UpToolStripMenuItem_Click(object sender, EventArgs e) { BtnUp_Click(null, null); }
        private void DownToolStripMenuItem_Click(object sender, EventArgs e) { BtnDown_Click(null, null); }
        private void TopToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(0); }
        private void EndToolStripMenuItem_Click(object sender, EventArgs e) { KeyDown_MoveRowAt(dgvStations.RowCount - 1); }

        private void PgUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < 8; j++)
            {
                BtnUp_Click(null, null);
                if (dgvStations.SelectedRows[0].Index < 1) { break; }
            }
        }

        private void PgDnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < 8; j++)
            {
                BtnDown_Click(null, null);
                if (dgvStations.SelectedRows[0].Index >= dgvStations.RowCount - 1) { break; }
            }
        }

        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (rowIndexFromMouseDown >= 0 && colIndexFromMouseDown >= 0)
            {
                dgvStations.CurrentCell = dgvStations.Rows[rowIndexFromMouseDown].Cells[colIndexFromMouseDown];
                dgvStations.BeginEdit(true);
            }
        }

        private void DgvStations_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            radioBtnChanged = true;
            UpdateStatusLabelStationsList();
        }

        private void LinkLblWebService_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new("https://www.radio-browser.info/") { UseShellExecute = true };
                Process.Start(psi);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void PlayPauseToolStripMenuItem_Click(object sender, EventArgs e) { BtnPlayStop_Click(null, null); }

        private void CmbxStation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbxStation.Visible && cmbxStation.Focused)
            {
                autostartStation = cmbxStation.Text;
                somethingToSave = true;
            }
            somethingToSave = true;
        }

        private void PicBoxPayPal_Click(object sender, EventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new("https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=DK9WYLVBN7K4Y") { UseShellExecute = true };
                Process.Start(psi);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void PicBoxPayPal_MouseEnter(object sender, EventArgs e) { picBoxPayPal.Cursor = Cursors.Hand; }
        private void PicBoxPayPal_MouseLeave(object sender, EventArgs e) { picBoxPayPal.Cursor = Cursors.Default; }

        private void EditStationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RadioButton rb = ((ContextMenuStrip)(((ToolStripMenuItem)sender).Owner)).SourceControl as RadioButton;
            int rbi = Convert.ToInt32(rb.Tag) - 1;
            tcMain.SelectedIndex = 1;
            dgvStations.Rows[rbi].Selected = true;
            dgvStations.CurrentCell = dgvStations.Rows[rbi].Cells[0]; // wg. F2, öffnet sonst 1. Zeile
        }

        private void GoogleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string search = string.Empty;
            if (ActiveControl != null && ActiveControl == historyListView)
            {
                if (historyListView.SelectedItems.Count > 0) { search = historyListView.Items[historyListView.SelectedIndices[0]].SubItems[2].Text; }
            }
            else if (!string.IsNullOrEmpty(lblD2.Text)) { search = lblD2.Text; }
            if (!string.IsNullOrEmpty(search))
            {
                try { Process.Start(new ProcessStartInfo("https://www.google.com/search?q=" + System.Web.HttpUtility.UrlEncode(search.Trim())) { UseShellExecute = true }); }
                catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            }
        }

        private void CopyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (ActiveControl != null && ActiveControl == historyListView)
            {
                if (historyListView.SelectedItems.Count > 0)
                {
                    string clip = historyListView.SelectedItems[0].SubItems[0].Text + " | " + historyListView.SelectedItems[0].SubItems[1].Text + " | " + historyListView.SelectedItems[0].SubItems[2].Text;
                    if (!string.IsNullOrEmpty(clip)) { Utilities.SetClipboardUnicodeText(clip.TrimEnd(new char[] { '|', ' ' })); }
                }

            }
            else if (!string.IsNullOrEmpty(currentDisplayLabel.Text)) { Utilities.SetClipboardUnicodeText(currentDisplayLabel.Text); }
        }

        private void StartPlaying(string _url, int tagID)
        {
            if (!Utilities.PingGoogleSuccess(Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT))) // 18: binary 0001 0010, which would map to LAN(0x2) | RasInstalled(0x10). This code only checks if the network cable is plugged in
            { //  && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()
                Bass.BASS_StreamFree(_stream);
                RestorePlayerDefaults(tagID); // (tagID)
                MessageBox.Show("No internet connection!", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);  // MB_TOPMOST
                return;
            }
            pbVolIcon.Image = Properties.Resources.progress;
            miniPlayer.MpVolProgBar.Value = volProgressBar.Value = 0;
            lblVolume.Text = "0";
            //Application.DoEvents();



            if (_url != string.Empty)
            {
                if (_url.ToLower().EndsWith(".m3u")) // || _url.ToLower().EndsWith(".m3u8")) // && (_url.ToLower().Contains("aac") || _url.ToLower().Contains("opus") || _url.ToLower().Contains("flac")))
                { //https://www.ndr.de/resources/metadaten/audio/aac/ndr1wellenord.m3u führt andernfalls zu Fehlermeldung - BASS_AAC_StreamCreateURL kommt damit nicht klar
                    try
                    {
                        string newURL = string.Empty;
                        httpClient ??= new HttpClient();
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
                        HttpResponseMessage response = httpClient.GetAsync(_url).Result; // http://www.ndr.de/resources/metadaten/audio/aac/n-joy.m3u
                        response.EnsureSuccessStatusCode();
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            newURL = response.Content.ReadAsStringAsync().Result.Trim();
                            string urlString = Regex.Replace(newURL, @".*(http:\/\/[\S]+).*", "$1", RegexOptions.Singleline);
                            string secString = Regex.Replace(newURL, @".*(https:\/\/[\S]+).*", "$1", RegexOptions.Singleline);
                            newURL = secString.StartsWith("https") ? secString : urlString.StartsWith("http") ? urlString : newURL; // weil Regex.Replace ganzen string zurückgibt, wenn nichts gefunden wird
                        }
                        else { MessageBox.Show("No Response from Server.", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000); return; }
                        _url = string.IsNullOrEmpty(newURL) ? _url : newURL;
                    }
                    catch (Exception ex)
                    {
                        RestorePlayerDefaults(tagID);
                        MessageBox.Show(ex.ToString(), appName, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000); // MB_TOPMOST);
                        return;
                    }
                }

                lblD3.Text = "⌛Connecting...";
                MiniPlayer.MpLblD2_Text("⌛Connecting...");
                Application.DoEvents();


                TaskScheduler uiSched;
                System.Threading.CancellationToken token = Task.Factory.CancellationToken;
                uiSched = TaskScheduler.FromCurrentSynchronizationContext();
                Task.Factory.StartNew(() =>
                {


                    if (_stream != 0) { Bass.BASS_StreamFree(_stream); } // mehrere Sender gleichzeitig zu hören wäre grundsätzlich möglich
                                                                         //MessageBox.Show(intOutputDevice.ToString() + " | " + Bass.BASS_GetDeviceCount(), appName, MessageBoxButtons.OK);
                    Bass.BASS_Init(intOutputDevice <= 0 || intOutputDevice >= Bass.BASS_GetDeviceCount() ? -1 : intOutputDevice + 1, 44100, BASSInit.BASS_DEVICE_DEFAULT, Handle);
                    //Parameter device: - 1 = default device, 0 = no sound, 1 = first real output device (Default)
                    //bool m3u8 = false;
                    BASSFlag flag = BASSFlag.BASS_DEFAULT | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_STREAM_AUTOFREE; // BASSFlag.BASS_STREAM_DECODE = kein Mithören / | BASSFlag.BASS_UNICODE ändert nichts an Fehlcodierung - eigentlich überflüssig
                                                                                                                         //if (_url.ToLower().Contains("aac")) { _stream = BassAac.BASS_AAC_StreamCreateURL(_url, 0, flag, myStreamCreateURL, IntPtr.Zero); }
                                                                                                                         //if (_url.ToLower().Contains("opus")) { _stream = BassOpus.BASS_OPUS_StreamCreateURL(_url, 0, flag, myStreamCreateURL, IntPtr.Zero); }
                                                                                                                         //else if (_url.ToLower().Contains("flac")) { _stream = BassFlac.BASS_FLAC_StreamCreateURL(_url, 0, flag, myStreamCreateURL, IntPtr.Zero); }
                                                                                                                         //else if (_url.ToLower().EndsWith("m3u8")) { _stream = BassHls.BASS_HLS_StreamCreateURL(_url, flag, myStreamCreateURL, IntPtr.Zero); m3u8 = true; }
                                                                                                                         //else { _stream = Bass.BASS_StreamCreateURL(_url, 0, flag, myStreamCreateURL, IntPtr.Zero); } // create the stream    

                    _stream = Bass.BASS_StreamCreateURL(_url, 0, flag, myStreamCreateURL, IntPtr.Zero);
                    if (_stream == 0)
                    {
                        string errorDescription = Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode());
                        RestorePlayerDefaults(tagID); // (tagID)
                        Bass.BASS_Free();
                        MessageBox.Show(errorDescription, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);  // MB_TOPMOST);
                    }
                    else
                    {
                        _tagInfo = new TAG_INFO(_url);
                        if (_tagInfo != null)
                        {
                            if (BassTags.BASS_TAG_GetFromURL(_stream, _tagInfo)) // This method first tries to get streaming header information via BASS_TAG_ICY and BASS_TAG_HTTP. 
                            {                                                   // Then it tries the following tags in that order: BASS_TAG_META, BASS_TAG_OGG, BASS_TAG_APE and BASS_TAG_WMA.
                                lblD3.Text = _tagInfo.channelinfo.ToString().Replace("48000Hz", "48kHz").Replace("44100Hz", "44.1kHz").Replace("???, ", _tagInfo.channelinfo.ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG ? "FLAC, " : "");
                                lblD3.Text = Regex.Replace(lblD3.Text, "([0-9])(kHz|bit)", "$1 $2"); // Leerzeichen einfügen 48kHz, 16bit
                                lblD3.Text += _tagInfo.bitrate != 0 ? ", " + _tagInfo.bitrate.ToString() + " kbit/s" : string.Empty;
                                lblD3.Text += ", 00:00:00";
                                if (_tagInfo.channelinfo.ctype == BASSChannelType.BASS_CTYPE_STREAM_MF && ((WAVEFORMATEX)Marshal.PtrToStructure(Bass.BASS_ChannelGetTags(_stream, BASSTag.BASS_TAG_WAVEFORMAT), typeof(WAVEFORMATEX))).wFormatTag
                                    == WAVEFormatTag.MPEG_HEAAC) { lblD3.Text = lblD3.Text.Replace("MF", "AAC"); }
                                lblD2.Text = _tagInfo.ToString();
                                MiniPlayer.MpLblD2_Text(lblD2.Text);
                                //if (_tagInfo.channelinfo.ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG) { MessageBox.Show("FLAC_OGG"); }
                            }
                            else { lblD3.Text = "00:00:00"; }

                            if (tcMain.SelectedTab == tpSectrum) { StatusStrip_SingleLabel(false, lblD2.Text); }
                            if (logHistory) { AddToHistory(lblD2.Text); } // _tagInfo.ToString() könnte null sein?!

                            // (Bass.BASS_ChannelGetInfo(_stream).plugin == 1) // 0 = not using a plugin
                            //{
                            //BASS_PLUGININFO pInfo = Bass.BASS_PluginGetInfo(1);
                            //if (pInfo != null)
                            //{
                            //    int a = 0;
                            //    string text = string.Empty;
                            //    for (a = 0; a < pInfo.formatc; a++) { text += pInfo.formats[a].ctype + " " + pInfo.formats[a].name + " " + pInfo.formats[a].exts; }
                            //    MessageBox.Show(text);
                            //}
                            //}

                            // Multiple synchronizers may be used per channel, and they can be set before and while playing.
                            // Equally, synchronizers can also be removed at any time, using BASS_ChannelRemoveSync(Int32, Int32).
                            // If the BASS_SYNC_ONETIME flag is used, then the sync is automatically removed after its first occurrence.
                            //if (m3u8)
                            //{
                            //    _hlsChange = new SYNCPROC(HLSChangeSync); // Sync when a new MPEG-TS/g (SDT) has been received. 
                            //    if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_HLS_SEGMENT, 0, _hlsChange, IntPtr.Zero) == 0) { MessageBox.Show("Setting up a HLS_Segment synchronizer failed.", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); };
                            //}
                            _connectFail = new SYNCPROC(ConnectionSync); // Sync for when a device stops unexpectedly (eg. if it is disconnected/disabled) 
                            if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_DOWNLOAD | BASSSync.BASS_SYNC_ONETIME, 0, _connectFail, IntPtr.Zero) == 0) { MessageBox.Show("Setting up a download synchronizer failed.", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); };

                            _deviceFail = new SYNCPROC(DeviceSync); // Sync for when a device stops unexpectedly (eg. if it is disconnected/disabled) 
                            if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_DEV_FAIL | BASSSync.BASS_SYNC_ONETIME, 0, _deviceFail, IntPtr.Zero) == 0) { MessageBox.Show("Setting up a device synchronizer failed.", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); };

                            _metaSync = new SYNCPROC(MetaSync); // set a sync to get the title updates out of the meta data...
                            if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_META, 0, _metaSync, IntPtr.Zero) == 0) { MessageBox.Show("Setting up a meta synchronizer failed.", appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); };
                            //Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_HLS_SEGMENT, 0, _metaSync, IntPtr.Zero);
                            //Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_OGG_CHANGE, 0, _metaSync, IntPtr.Zero);

                            Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, channelVolume); // ist erforderlich, startet sonst mit voller Lautstärke (Default)
                            currPlayingTime = TimeSpan.Zero; // wird im nachfolgenden timerLevel genutzt
                            _isBuffering = true;
                            timerLevel.Start();
                            spectrumTimer.Start();
                            Bass.BASS_ChannelPlay(_stream, false);
                            //SpecialGUITasks();
                            miniPlayer.MpBtnPlay.Enabled = btnPlayStop.Enabled = btnIncrease.Enabled = btnDecrease.Enabled = btnReset.Enabled = btnRecord.Enabled = true;
                            BASS_CHANNELINFO info = new(); // extrahiert m3u-Adressen! Voraussetzung für Vergleich in TcMain_SelectedIndexChanged
                            if (tagID > 0 && Bass.BASS_ChannelGetInfo(_stream, info) && !string.IsNullOrEmpty(info.filename))
                            {  //string url = _tagInfo.filename; // extrahiert NICHT m3u!
                                dgvStations.Rows[tagID - 1].Cells[1].Value = info.filename; // m3u mit aac ersetzen
                                if (lblD3.Text.Length <= 1) // z.B. Offener Kanal Kiel
                                {
                                    lblD3.Text = Regex.Replace(info.ToString(), "[0-9]+Hz", ((double)info.freq / 1000).ToString() + "kHz").Replace("???, ", info.ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG ? "FLAC, " : "");
                                    lblD3.Text = Regex.Replace(lblD3.Text, "([0-9])(kHz|bit)", "$1 $2"); // Leerzeichen einfügen 48kHz, 16bit
                                    lblD3.Text += ", 00:00:00";
                                }
                                if (lblD4.Text.EndsWith(" OK")) { lblD4.Text = info.filename; }
                            }
                            btnPlayStop.Image = Properties.Resources.pause_white;
                            miniPlayer.MpBtnPlay.Image = Properties.Resources.pause_white;
                            playPauseToolStripMenuItem.Text = "Pause"; // btnPlayStop.Text = 
                            playPauseToolStripMenuItem.Image = Properties.Resources.pause;
                        }
                        else { RestorePlayerDefaults(tagID); } // _tag.Info null
                    }


                }, token, TaskCreationOptions.DenyChildAttach, uiSched);

            }
        }

        private void BtnRecord_Click(object sender, EventArgs e)
        {
            if (_recording)
            {
                RecordingStop(false, Color.Blue); // false = !BASS_StreamFree; recording = false; // muss hier so früh wie möglich erfolgen
                timerLevel.Stop();
                spectrumTimer.Stop();
                foreach (VerticalProgressBar vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
                pbLevel.Image = null;
                miniPlayer.MpPBLevel.Image = null;
                Bass.BASS_ChannelPause(_stream);
                playPauseToolStripMenuItem.Text = "Play"; // btnPlayStop.Text = 
                btnPlayStop.Image = Properties.Resources.play_white;
                miniPlayer.MpBtnPlay.Image = Properties.Resources.play_white;
                lblD4.Text = _downloadFileName;
                lblD4.Cursor = Cursors.Hand;
            }
            else if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                _recording = true;
                currPlayingTime = TimeSpan.Zero; //  Bass.BASS_ChannelSetPosition(_stream, Bass.BASS_ChannelSeconds2Bytes(_stream, 00.00));
                btnRecord.Image = Properties.Resources.stop_white;
                lblD4.ForeColor = btnRecord.BackColor = Color.Maroon;
            }
            else { Console.Beep(); }
        }

        private void DisplayLabel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && (sender as Label).Text.Equals(_downloadFileName, StringComparison.InvariantCultureIgnoreCase))
            {
                if (File.Exists(_downloadFileName))
                {
                    Process explorer = new();
                    explorer.StartInfo.FileName = "explorer.exe";
                    explorer.StartInfo.Arguments = "/e, /select,\"" + _downloadFileName + "\""; // /e:  Open in its default view.
                    explorer.Start();
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                currentDisplayLabel = sender as Control;
                contextMenuDisplay.Show(this, tcMain.PointToClient(Cursor.Position));
            }
        }

        private void TpPlayer_MouseUp(object sender, MouseEventArgs e) // ContextMenu nur für die RadioButtos, für die noch keine Station definiert wurde
        {
            if (e.Button == MouseButtons.Right)
            {
                Point pt = e.Location;
                Control ctrl = tpPlayer.GetChildAtPoint(pt); // Controls die nicht enabled sind, zeigen kein Contextmenu an
                if (ctrl != null && ctrl is RadioButton && ctrl.Enabled == false) { contextMenuPlayer.Show(ctrl, new Point(10, 10)); }
            }
        }

        private void SearchNewStationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tcMain.SelectedIndex != 1)
            {
                tcMain.SelectedIndex = 1;
                if (_currentButtonNum > 0)
                {
                    dgvStations.Rows[_currentButtonNum - 1].Selected = true;
                    dgvStations.CurrentCell = dgvStations.Rows[_currentButtonNum - 1].Cells[0]; // wg. F2, öffnet sonst 1. Zeile
                    if (_currentButtonNum > 12) { dgvStations.FirstDisplayedScrollingRowIndex = dgvStations.SelectedRows[0].Index; }
                }
                BtnSearch_Click(null, null);
            }
        }

        private void TimerLevel_Tick(object sender, EventArgs e)
        {
            bool everySecond = false;
            long utcNowTicks = DateTime.UtcNow.Ticks; // bei ersten Aufruf ist accumulatedTicks 0
            accumulatedTicks = accumulatedTicks == 0 ? utcNowTicks : accumulatedTicks <= utcNowTicks - 20000000 ? utcNowTicks - 10000000 : accumulatedTicks; // 2. Statement: wenn Wiedergabe/Timer pausiert wurde
            if (utcNowTicks >= accumulatedTicks + 10000000) // 1 second = 10.000.000 ticks
            {
                everySecond = true;
                currPlayingTime += TimeSpan.FromTicks(utcNowTicks - accumulatedTicks); // TimeSpan.FromSeconds(1);
                totalPlayingTime += TimeSpan.FromTicks(utcNowTicks - accumulatedTicks);
                accumulatedTicks = utcNowTicks;
            }
            if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING && (Visible || miniPlayer.Visible))
            {
                int mainWidth = pbLevel.Height;
                int miniWidth = miniPlayer.MpPBLevel.Width;
                int level = Bass.BASS_ChannelGetLevel(_stream); // The level ranges linearly from 0 (silent) to 32768 (max)
                if (Visible) { DrawLevelMeter(Utils.LowWord32(level) * mainWidth / 32768, Utils.HighWord32(level) * mainWidth / 32768); }
                else if (miniPlayer.Visible) { MiniPlayer.DrawLevelMeter(Utils.LowWord32(level) * miniWidth / 32768, Utils.HighWord32(level) * miniWidth / 32768); }
                if (everySecond)
                {
                    if (tcMain.SelectedTab == tpHistory) { TPHistory_SetStatusBarText(); }
                    string seconds = currPlayingTime.ToString(@"hh\:mm\:ss");
                    if (Regex.Match(lblD3.Text, @"\d{2}:\d{2}:\d{2}$").Success) { lblD3.Text = lblD3.Text.Remove(lblD3.Text.Length - 8) + seconds; }
                    else
                    {
                        if (lblD3.Text.Length > 0)
                        {
                            lblD3.Text += ", " + seconds;
                            lblD3.Text = Regex.Replace(lblD3.Text, @"^-, ", ""); // Unnötige Zeichen am Anfang entfernen
                        }
                        else { lblD3.Text = seconds; }
                    }
                }
            }

            if (_isBuffering) // scheint beste/einfachste Lösung zu sein; while-loop kann einfrieren, Threads/(Background)Tasks ausserhalb GUI
            {
                miniPlayer.MpVolProgBar.ForeColor = volProgressBar.ForeColor = Color.MediumSeaGreen;
                float buffProgress = Bass.BASS_StreamGetFilePosition(_stream, BASSStreamFilePosition.BASS_FILEPOS_DOWNLOAD) * (100f / _netPreBuff) / Bass.BASS_StreamGetFilePosition(_stream, BASSStreamFilePosition.BASS_FILEPOS_END);  // percentage of file downloaded
                if (buffProgress < 100)
                {
                    buffProgress = buffProgress > 100 ? 100 : buffProgress;
                    miniPlayer.MpVolProgBar.Value = volProgressBar.Value = (int)Math.Round(buffProgress);
                    lblVolume.Text = volProgressBar.Value.ToString();
                }
                else
                {
                    miniPlayer.MpVolProgBar.Value = volProgressBar.Value = 100;
                    lblVolume.Text = "100";
                    System.Threading.Thread.Sleep(timerLevel.Interval / 2);
                    miniPlayer.MpVolProgBar.ForeColor = volProgressBar.ForeColor = SystemColors.ActiveCaption;
                    Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref channelVolume);
                    miniPlayer.MpVolProgBar.Value = volProgressBar.Value = (int)(channelVolume * 100f);
                    lblVolume.Text = volProgressBar.Value.ToString();
                    pbVolIcon.Image = Properties.Resources.volume;
                    _isBuffering = false;
                }
            }
        }

        private void RestorePlayerDefaults(int currBtnNum = 0) // eventuell soll gewähle Station aktiv bleiben (currBtnNum > 0)
        {
            timerLevel.Stop();
            spectrumTimer.Stop();
            foreach (VerticalProgressBar vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
            if (currBtnNum == 0)
            {
                foreach (RadioButton rb in tcMain.TabPages[0].Controls.OfType<RadioButton>().Where(rb => rb.Checked)) { rb.Checked = false; } // cave: aändert currentButtonNum
                lblD1.Text = "-";
                MiniPlayer.MpLblD1_Text(lblD1.Text);
                miniPlayer.MpBtnPlay.Enabled = btnPlayStop.Enabled = btnReset.Enabled = btnRecord.Enabled = false;
            }
            lblD2.Text = "-";
            MiniPlayer.MpLblD2_Text(lblD2.Text);
            if (tcMain.SelectedTab == tpSectrum) { StatusStrip_SingleLabel(false, lblD2.Text); }
            lblD3.Text = "-";
            lblD4.Text = "-";
            lblD4.ForeColor = SystemColors.ControlText;
            lblD4.Cursor = Cursors.Default;
            pbLevel.Image = null; // LevelMeter löschen
            miniPlayer.MpPBLevel.Image = null;
            btnPlayStop.Image = Properties.Resources.play_white;
            btnPlayStop.BackColor = SystemColors.ControlDark;
            miniPlayer.MpBtnPlay.Image = Properties.Resources.play_white;
            miniPlayer.MpBtnPlay.BackColor = SystemColors.ControlDark;
            playPauseToolStripMenuItem.Text = "Play"; // btnPlayStop.Text = 
            playPauseToolStripMenuItem.Image = Properties.Resources.play;

            pbVolIcon.Image = Properties.Resources.volume;
            miniPlayer.MpVolProgBar.ForeColor = volProgressBar.ForeColor = SystemColors.ActiveCaption;
            Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref channelVolume);
            miniPlayer.MpVolProgBar.Value = volProgressBar.Value = (int)(channelVolume * 100f);
            lblVolume.Text = volProgressBar.Value.ToString();
        }

        private void Progress_ProgressChanged(object sender, float progress)
        {
            progressBar.Value = (int)progress; //if (Environment.OSVersion.Version.Major >= 6) { TaskbarProgress.SetValue(Handle, (int)progress, 100); } // Vista und höher
            if (progressBar.Value >= progressBar.Maximum) { timerCloseFinally.Start(); } // nach 3 Sekunden - wenn nicht FileSystemWatcher vorher beendet hat
        }

        private void TimerCloseFinally_Tick(object sender, EventArgs e)
        {
            try { Process.Start(localSetupFile, "/deleteSetup=true"); } // /SILENT
            catch (Exception ex) // when (ex is ArgumentNullException or InvalidOperationException or Win32Exception)
            {
                MessageBox.Show(ex.Message, Application.ProductName);
                File.Delete(localSetupFile);
            }
            finally { Close(); } // Application.Exit();
        }

        private async void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (updateAvailable)
            {
                if (Path.GetDirectoryName(xmlPath) != Path.GetDirectoryName(appPath)) // (Utilities.IsInnoSetupValid(Path.GetDirectoryName(appPath)))
                {
                    try
                    {
                        localSetupFile = Path.Combine(Path.GetTempPath(), appName + ((IntPtr.Size == 4) ? "32" : "") + "Setup.exe");
                        btnUpdate.Enabled = false;
                        progressBar.Visible = true;
                        progressBar.BringToFront();
                        Progress<float> progress = new(); // Setup your progress reporter 
                        progress.ProgressChanged += Progress_ProgressChanged;
                        httpClient ??= new HttpClient();
                        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/octet-stream");
                        using FileStream file = new(localSetupFile, FileMode.Create, FileAccess.Write, FileShare.None);
                        await httpClient.DownloadDataAsync(downloadUpdateURL, file, progress);  // s. Class HttpClientProgressExtensions
                    }
                    catch (Exception ex) // when (ex is InvalidOperationException or ArgumentNullException or WebException)
                    {
                        btnUpdate.Enabled = true;
                        MessageBox.Show(ex.Message, appName);
                    }
                }
                else // Portable version
                {
                    try
                    {
                        ProcessStartInfo psi = new("https://www.ophthalmostar.de/freeware/#netradio") { UseShellExecute = true };
                        Process.Start(psi);
                    }
                    catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                }
            }
            else if (NativeMethods.InternetGetConnectedState(out int flags, 0)) // 18: binary 0001 0010, which would map to LAN(0x2) | RasInstalled(0x10)
            {
                string xmlURL = "https://www.ophthalmostar.de/netradio.xml";
                Version newVersion = null;
                try
                {
                    httpClient ??= new HttpClient();
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/xml; charset=utf-8");
                    HttpResponseMessage response = httpClient.GetAsync(xmlURL).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        XDocument doc = XDocument.Parse(response.Content.ReadAsStringAsync().Result);
                        if (doc != null)
                        {
                            newVersion = new Version(doc.Element("netradio").Element("version").Value); // doc.XPathSelectElement("/pdfmover/version").Value
                            downloadUpdateURL = doc.Element("netradio").Element("url64").Value;
                        }
                        else { MessageBox.Show("No update information", appName, MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
                    }
                }
                catch (Exception ex) // when (ex is WebException or NullReferenceException or ArgumentNullException or XmlException or ArgumentException or IOException)
                {
                    MessageBox.Show(ex.ToString(), appName); // MsgBoxOK("Die Downloadseite wurde nicht erreicht.", "Error");
                    return;
                }
                if (newVersion == null) { MessageBox.Show("Sorry, no update information available.", appName, MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else
                {
                    if (newVersion.CompareTo(_curVersion) > 0) // größer 0 → NEWVersion vorhanden; 0 → beide Versionen identisch
                    {
                        lblUpdate.Text = "Update available: v" + newVersion.ToString();
                        btnUpdate.Text = "Download & Install"; // UseMnemonic = false
                        updateAvailable = true;
                        btnUpdate.BackColor = SystemColors.MenuHighlight;
                        btnUpdate.ForeColor = SystemColors.Info;
                        btnUpdate.Invalidate(); // s. BtnUpdate_Paint()
                    }
                    else { lblUpdate.Text = lblUpdate.Text.Equals("No update available") ? "Current version: " + _curVersion.ToString() : "No update available"; } // newVersion == currVersion; SaveConfigValue(updateTime, DateTime.Now.ToString()); wird im FormClosing-Event erledigt
                }
            }
            else { MessageBox.Show("No internet connection.", appName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void BtnUpdate_Paint(object sender, PaintEventArgs e)
        {
            if (updateAvailable)
            {
                e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.CompositingMode = CompositingMode.SourceOver;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
                Button btn = sender as Button;
                Rectangle borderRectangle = btn.ClientRectangle;
                borderRectangle.Inflate(-2, -2); //ControlPaint.DrawBorder3D(e.Graphics, borderRectangle, Border3DStyle.Flat);
                ControlPaint.DrawBorder(e.Graphics, borderRectangle,
                    SystemColors.Highlight, 1, ButtonBorderStyle.Solid, // left
                    SystemColors.Highlight, 1, ButtonBorderStyle.Solid, // top
                    SystemColors.HotTrack, 1, ButtonBorderStyle.Solid,  // right
                    SystemColors.HotTrack, 1, ButtonBorderStyle.Solid); // bottom
            }
        }

        private void ControlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Environment.OSVersion.Version > new Version("6.2")) { Process.Start("ms-settings:sound-devices"); }
                //else { Process.Start("mmsys.cpl"); }

                ProcessStartInfo psi = new("mmsys.cpl") { UseShellExecute = true, WorkingDirectory = Environment.SystemDirectory };
                Process.Start(psi);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void FrmMain_Activated(object sender, EventArgs e)
        {
            //newButton.ForeColor = SystemColors.ControlText;
            TopMost = false;  // Workaround, damit Tooltip in Listview im Vordergrund angezeigt wird
        }

        private void FrmMain_Deactivate(object sender, EventArgs e)
        {
            //newButton.ForeColor = SystemColors.ControlDark;
            if (alwaysOnTop) { TopMost = true; }
        }

        private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabel.IsLink) { BtnSearch_Click(null, null); }
        }

        private void FrmMain_Move(object sender, EventArgs e) { somethingToSave = true; }

        private void DrawLevelMeter(int left, int right)
        {
            if (left != levelLeft && right != levelRight)
            {
                levelLeft = left;
                levelRight = right;
                int height = pbLevel.Height - 1;
                Bitmap bm = new(pbLevel.ClientSize.Width, pbLevel.ClientSize.Height);
                using (Graphics g = Graphics.FromImage(bm))
                {
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                    //g.Clear(pbLevel.BackColor);
                    using Pen p = new(SystemColors.Highlight, 5.0f); // ActiveCaption
                    p.DashCap = DashCap.Round;
                    //p.DashPattern = new float[] { 1.0f, 1.0f };
                    g.DrawLine(p, 8, height, 8, height - levelRight);
                    g.DrawLine(p, 1, height, 1, height - levelLeft);
                }
                pbLevel.Image = bm; // pictureBox2.Refresh(); bm.Dispose() führt zu Error
            }
        }

        private void HistoyClearButton_Click(object sender, EventArgs e)
        {
            historyListView.Items.Clear();
            historyExportButton.Enabled = histoyClearButton.Enabled = false;
            HistoryListView_SetDefaultColumnWidth();
            historyListView.Refresh();
            TPHistory_SetStatusBarText();
        }

        private void HistoryExportButton_Click(object sender, EventArgs e)
        {
            saveFileDialog.FileName = appName + "_" + DateTime.Now.ToString("yyyyMMdd-hhmmss") + ".csv";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StringBuilder result = new();
                Utilities.WriteCSVRow(result, historyListView.Columns.Count, i => historyListView.Columns[i].Width > 0, i => historyListView.Columns[i].Text);
                foreach (ListViewItem listItem in historyListView.Items) { Utilities.WriteCSVRow(result, historyListView.Columns.Count, i => historyListView.Columns[i].Width > 0, i => listItem.SubItems[i].Text); }
                File.WriteAllText(saveFileDialog.FileName, result.ToString());
            }
        }

        private void CbLogHistory_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLogHistory.Focused)
            {
                if (cbLogHistory.Checked) { logHistory = true; }
                else { logHistory = false; }
                somethingToSave = true;
            }
        }

        private void HistoryListView_SetDefaultColumnWidth()
        {
            historyListView.Columns[0].Width = 60;
            historyListView.Columns[1].Width = 80;
            if (!NativeMethods.VerticalScrollbarVisible(historyListView)) { historyListView.Columns[2].Width = historyListView.Width - historyListView.Columns[0].Width - historyListView.Columns[1].Width - 4; }
            else { historyListView.Columns[2].Width = historyListView.Width - historyListView.Columns[0].Width - historyListView.Columns[1].Width - SystemInformation.VerticalScrollBarWidth - 5; }
        }

        private void HistoryListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (historyListView.Items.Count > 0)
            {
                if (!string.IsNullOrEmpty(lvSortOrderArray[e.Column]) && lvSortOrderArray[e.Column].Equals("Ascending"))
                {
                    lviComparer.Order = SortOrder.Descending;
                    lvSortOrderArray[e.Column] = "Descending";
                }
                else if (!string.IsNullOrEmpty(lvSortOrderArray[e.Column]) && lvSortOrderArray[e.Column].Equals("Descending"))
                {
                    lviComparer.Order = SortOrder.Ascending;
                    lvSortOrderArray[e.Column] = "Ascending";
                }
                else // die Spalte ist unsortiert => standardmäßig Sortierrichtung 
                {
                    lviComparer.Order = SortOrder.Ascending;
                    lvSortOrderArray[e.Column] = "Ascending";
                }
                lviComparer.SortColumn = e.Column;
                historyListView.Refresh(); // Arrows auf anderen ColumnHeader-Buttons werden entfernt
                historyListView.Sort();
            }
        }

        private void ContextMenuDisplay_Opening(object sender, CancelEventArgs e)
        {
            if (ActiveControl != null && ActiveControl == historyListView && historyListView.SelectedItems.Count <= 0) { e.Cancel = true; }
            else if (ActiveControl != null && ActiveControl != historyListView) { tSMItemListViewDeleteEntry.Visible = tsSepListViewDeleteEntry.Visible = false; }
            else { tSMItemListViewDeleteEntry.Visible = tsSepListViewDeleteEntry.Visible = true; }
        }

        private void HistoryListView_MouseDoubleClick(object sender, MouseEventArgs e) { CopyToClipboardToolStripMenuItem_Click(null, null); }


        private void HistoryListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
        {
            using (SolidBrush backBrush = new(SystemColors.ControlDark)) { e.Graphics.FillRectangle(backBrush, e.Bounds); }
            Rectangle rect = e.Bounds; // Do some padding, since these draws right up next to the border for Left/Near. Will need to change this if you use Right/Far
            rect.Inflate(0, -1);
            ControlPaint.DrawBorder3D(e.Graphics, rect, Border3DStyle.RaisedOuter);
            using SolidBrush foreBrush = new(Color.White);
            StringFormat stringFormat = Utilities.GetStringFormat(e.Header.TextAlign); // Translate e.Header.TextAlign value ('HorizontalAlignment' with values of Right, Center, Left).
            rect.X += MouseButtons == MouseButtons.Left && rect.Contains(historyListView.PointToClient(MousePosition)) ? 6 : 4;
            string sortArrow = lviComparer.SortColumn == e.ColumnIndex ? !string.IsNullOrEmpty(lvSortOrderArray[e.ColumnIndex]) && lvSortOrderArray[e.ColumnIndex].Equals("Ascending") ? " ↓" : " ↑" : ""; // ▲▼
                                                                                                                                                                                                           //string sortArrow = string.Empty;
            e.Graphics.DrawString(e.Header.Text + sortArrow, e.Font, foreBrush, rect, stringFormat);
        }

        private void HistoryListView_DrawItem(object sender, DrawListViewItemEventArgs e) { e.DrawDefault = true; }

        private void TpHistory_Leave(object sender, EventArgs e)
        {
            if (logHistory && historyListView.Items.Count > 0)
            {
                HistoryListView_SetDefaultColumnWidth(); // wg. Tooltip LongSubItemText
                lviComparer.SortColumn = 0;
                lviComparer.Order = SortOrder.Descending;
                historyListView.Sort(); // MessageBox.Show(historyListView.TopItem.Tag.ToString()); // 2023-04-14T12:59:06.3463796Z
                lvSortOrderArray[0] = "Descending";
            }
        }
        private void TSMItemListViewDeleteEntry_Click(object sender, EventArgs e)
        {
            int index = historyListView.Items.IndexOf(historyListView.SelectedItems[0]);
            historyListView.Items.RemoveAt(index);
            if (index > 0)
            {
                historyListView.Items[index - 1].Selected = true;
                historyListView.SelectedItems[0].Focused = true;
            }
        }

        private void HistoryListView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (historyListView.FocusedItem != null)
                {
                    historyListView.SelectedItems.Clear(); // nur 1 Eintrag soll bei Rechtsklick selected sein
                    historyListView.Items[historyListView.FocusedItem.Index].Selected = true;
                }
            }
        }

        private void HistoryListView_KeyDown(object sender, KeyEventArgs e)
        {
            int focussedIndex = historyListView.FocusedItem != null ? historyListView.FocusedItem.Index : -1;
            int selectedCount = historyListView.SelectedItems.Count;
            if (e.KeyCode == Keys.Delete && focussedIndex >= 0)
            {
                if (selectedCount >= 1)
                {
                    historyListView.BeginUpdate();
                    int lastIndex = historyListView.Items.IndexOf(historyListView.SelectedItems[0]);
                    for (int i = selectedCount - 1; i >= 0; i--) { historyListView.Items.RemoveAt(historyListView.SelectedIndices[i]); }
                    if (historyListView.Items.Count > 0)
                    {
                        historyListView.Items[lastIndex <= historyListView.Items.Count && lastIndex > 0 ? lastIndex - 1 : 0].Selected = true;
                        historyListView.SelectedItems[0].Focused = true;
                    }
                    else if (historyListView.Items.Count == 0) { histoyClearButton.Enabled = historyExportButton.Enabled = false; }
                    historyListView.EndUpdate();
                }
                else { Console.Beep(); }
            }
            else if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
            {
                historyListView.BeginUpdate();
                foreach (ListViewItem item in historyListView.Items) { item.Selected = true; }
                historyListView.EndUpdate();
            }
        }

        private void BtnActions_Click(object sender, EventArgs e)
        {
            using FrmSchedule frmSchedules = new();
            if (alwaysOnTop) { frmSchedules.TopMost = true; }
            for (int i = 0; i < stationSum; i++)
            {
                if (dgvStations.Rows[i].Cells[0].Value != null && !string.IsNullOrEmpty(dgvStations.Rows[i].Cells[0].Value.ToString()))
                {
                    frmSchedules.StationsList.Add(Utilities.StationShort(dgvStations.Rows[i].Cells[0].Value.ToString()));
                }
            }
            frmSchedules.ActionListView.Items.Clear();
            for (int j = 0; j < tableActions.Rows.Count; j++)
            {
                frmSchedules.ActionListView.Items.Add(new ListViewItem(new string[] { "", tableActions.Rows[j][1].ToString(), tableActions.Rows[j][2].ToString(), tableActions.Rows[j][3].ToString() }));
                frmSchedules.ActionListView.Items[j].Checked = tableActions.Rows[j].Field<bool>("Enabled");
            }
            for (int l = frmSchedules.ActionListView.Items.Count; l < 9; l++) // mit Leerzeilen auffüllen - erspart Butte "Add" für neue Einträge
            {
                frmSchedules.ActionListView.Items.Add(new ListViewItem(new string[] { "", "", "", "" }));
            }
            frmSchedules.ActionListView.Items[0].Selected = true;
            frmSchedules.KeepActionsActive.Checked = keepActionsActive && tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled") == true);
            if (frmSchedules.ShowDialog() == DialogResult.OK)
            {
                StopActions(); // erst jetzt weil alle Zeilen in tableActions auf not enabled (False) gesetzt werden
                tableActions.Rows.Clear();
                cbActions.Checked = false;
                keepActionsActive = frmSchedules.KeepActionsActive.Checked;
                foreach (ListViewItem item in frmSchedules.ActionListView.Items) //for (int i = 0; i < frmSchedules.ActionListView.Items.Count; i++)
                {
                    int columns = frmSchedules.ActionListView.Columns.Count;
                    bool notEmpty = false;
                    object[] cells = new object[columns];
                    for (int j = 0; j < columns; j++)
                    {
                        if (j == 0)
                        {
                            if (item.Checked) { cells[0] = true; }
                            else { cells[0] = false; }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(item.SubItems[j].Text)) { cells[j] = item.SubItems[j].Text; }
                            if (cells[j] != null) { notEmpty = true; }
                        }
                    }
                    if (notEmpty) { tableActions.Rows.Add(cells); }
                }
                somethingToSave = true;
                if (tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled") == true))
                {
                    cbActions.Checked = true;
                    PrepareActions();
                }
            }
        }

        private void PrepareActions()
        {
            for (int i = 0; i < tableActions.Rows.Count; i++)
            {
                if (tableActions.Rows[i].Field<bool>("Enabled")) //) && rgxValidTime.Match(tableActions.Rows[i].Field<string>("Time")).Success)
                {

                    DateTime nowTime = DateTime.Now;
                    int jobHour = int.TryParse(tableActions.Rows[i].Field<string>("Time").Split(':').FirstOrDefault(), out int intH) ? intH : -1;
                    int jobMinu = int.TryParse(tableActions.Rows[i].Field<string>("Time").Split(':').LastOrDefault(), out int intM) ? intM : -1;
                    if (jobHour < 0 || jobMinu < 0)
                    {
                        MessageBox.Show("Task #" + i + " is not executed because the time specification is incorrect.", appName);
                        continue;
                    }
                    DateTime jobTime = new(nowTime.Year, nowTime.Month, nowTime.Day, jobHour, jobMinu, 0);
                    if (nowTime > jobTime) { jobTime = jobTime.AddDays(1); }
                    int tickTime = (int)(jobTime - nowTime).TotalMilliseconds; // double
                    switch (i)
                    {
                        case 0:
                            timerAction1.Interval = tickTime;
                            timerAction1.Start();
                            break;
                        case 1:
                            timerAction2.Interval = tickTime;
                            timerAction2.Start();
                            break;
                        case 2:
                            timerAction3.Interval = tickTime;
                            timerAction3.Start();
                            break;
                        case 3:
                            timerAction4.Interval = tickTime;
                            timerAction4.Start();
                            break;
                        case 4:
                            timerAction5.Interval = tickTime;
                            timerAction5.Start();
                            break;
                        case 5:
                            timerAction6.Interval = tickTime;
                            timerAction6.Start();
                            break;
                        case 6:
                            timerAction7.Interval = tickTime;
                            timerAction7.Start();
                            break;
                        case 7:
                            timerAction8.Interval = tickTime;
                            timerAction8.Start();
                            break;
                        case 8:
                            timerAction9.Interval = tickTime;
                            timerAction9.Start();
                            break;
                    }
                }
            }
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            int num = 0;
            (sender as Timer).Stop();
            if (sender == timerAction1) { num = 0; }
            else if (sender == timerAction2) { num = 1; }
            else if (sender == timerAction3) { num = 2; }
            else if (sender == timerAction4) { num = 3; }
            else if (sender == timerAction5) { num = 4; }
            else if (sender == timerAction6) { num = 5; }
            else if (sender == timerAction7) { num = 6; }
            else if (sender == timerAction8) { num = 7; }
            else if (sender == timerAction9) { num = 8; }

            tableActions.Rows[num][0] = false; // Aufgabe deaktivieren
            if (!tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled") == true)) { cbActions.Checked = false; }

            if (tcMain.SelectedIndex != 0) { tcMain.SelectedIndex = 0; }

            if (tableActions.Rows[num][1].ToString().Equals(Utilities.TaskNames[0])) // "Start playing"
            {
                RadioButton button = tcMain.TabPages[0].Controls.OfType<RadioButton>().FirstOrDefault(y => y.Text.Equals(tableActions.Rows[num][2].ToString()));
                if (button != null) { button.Checked = true; }
                else MessageBox.Show("Station not found.", appName);
            }
            else if (tableActions.Rows[num][1].ToString().Equals(Utilities.TaskNames[1])) // "Stop playing"
            {
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING) { BtnPlayStop_Click(null, null); }
            }
            else if (tableActions.Rows[num][1].ToString().Equals(Utilities.TaskNames[2])) // "Start recording"
            {
                if (!_recording)
                {
                    RadioButton button = tcMain.TabPages[0].Controls.OfType<RadioButton>().FirstOrDefault(y => y.Text.Equals(tableActions.Rows[num][2].ToString()));
                    if (button != null) { button.Checked = true; } // löst StartPlaying aus
                    BtnRecord_Click(null, null);
                }
            }
            else if (tableActions.Rows[num][1].ToString().Equals(Utilities.TaskNames[3])) // "Stop recording"
            {
                if (_recording) { btnRecord.PerformClick(); btnRecord.Focus(); } // RecordingStop();
            }
            else if (tableActions.Rows[num][1].ToString().Equals(Utilities.TaskNames[4])) // "Sleep Mode"
            {
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    BtnPlayStop_Click(null, null);
                    _playWakeFromSleep = true;
                }
                if (NativeMethods.MessageBoxTimeout(Handle, $"The PC will go into sleep mode.", $"NetRadio - Task No. " + num + 1, 0x00000001 | 0x00010000 | 0x00000100 | 0x00000000, 0, 10000) == 2)
                {
                    if (_playWakeFromSleep) { BtnPlayStop_Click(null, null); }
                    _playWakeFromSleep = false;
                    return; // 2: Schaltfläche Cancel wurde ausgewählt
                }
                Application.SetSuspendState(PowerState.Suspend, false, true);
            }
            else if (tableActions.Rows[num][1].ToString().Equals(Utilities.TaskNames[5])) // "Hibernate PC"
            {
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    BtnPlayStop_Click(null, null);
                    _playWakeFromSleep = true;
                }
                if (NativeMethods.MessageBoxTimeout(Handle, $"The PC will go into hibernation mode.", $"NetRadio - Task No. " + num + 1, 0x00000001 | 0x00010000 | 0x00000100 | 0x00000040, 0, 10000) == 2)
                {
                    if (_playWakeFromSleep) { BtnPlayStop_Click(null, null); }
                    _playWakeFromSleep = false;
                    return; // 2 = Cancel
                }
                Application.SetSuspendState(PowerState.Hibernate, false, true); //  put Windows into standby mode. Standby is forced and wake events are ignored:
            }
            else if (tableActions.Rows[num][1].ToString().Equals(Utilities.TaskNames[6])) // "Shut down PC"
            {
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    BtnPlayStop_Click(null, null);
                    _playWakeFromSleep = true;
                }
                if (NativeMethods.MessageBoxTimeout(Handle, $"The computer will shut down.", $"NetRadio - Task No. " + num + 1, 0x00000001 | 0x00010000 | 0x00000100 | 0x00000030, 0, 10000) == 2)
                {
                    if (_playWakeFromSleep) { BtnPlayStop_Click(null, null); }
                    _playWakeFromSleep = false;
                    return; // 2 = Cancel
                }
                Process.Start(new ProcessStartInfo("shutdown", "/s /t 1")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
                Close(); // Application.Exit(); //  if (somethingToSave || radioBtnChanged) { SaveConfig(); }
            }
        }

        private void CbActions_CheckedChanged(object sender, EventArgs e)
        {
            if (!cbActions.Checked && cbActions == ActiveControl) { StopActions(); }
            else if (!tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled") == true)) { cbActions.Checked = false; }
        }

        private void StopActions()
        {
            for (int i = 0; i < tableActions.Rows.Count; i++) { tableActions.Rows[i][0] = false; }
            timerAction1?.Stop();
            timerAction2?.Stop();
            timerAction3?.Stop();
            timerAction4?.Stop();
            timerAction5?.Stop();
            timerAction6?.Stop();
            timerAction7?.Stop();
            timerAction8?.Stop();
            timerAction9?.Stop();
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new("https://github.com/ophthalmos/NetRadio") { UseShellExecute = true };
                Process.Start(psi);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void PbLevel_Click(object sender, EventArgs e) { if (timerLevel.Enabled) { tcMain.SelectedIndex = 6; } }

        private void LinkLabeGNU_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new("https://www.gnu.org/licenses/") { UseShellExecute = true };
                Process.Start(psi);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, appName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        private void DgvStations_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            strCellValue = dgvStations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null ? dgvStations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() : string.Empty;  // DgvStations_CellValueChanged funktioniert unzuverlässig bzw. zu spät
            if (e.ColumnIndex == 0 && frmSplash == null)
            {
                frmSplash = new() { TopMost = true }; // using geht nur mit ShowDialog
                frmSplash.SplashActivated += new EventHandler(SplashForm_Activated);

                NativeMethods.ShowWindow(frmSplash.Handle, NativeMethods.SW_SHOWNOACTIVATE); // ohne TopMost!
            }
        }

        private void DgvStations_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            radioBtnChanged = true || strCellValue != dgvStations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            if (e.ColumnIndex == 0 && frmSplash != null)
            {
                frmSplash.Close();
                frmSplash.Dispose();
                frmSplash = null;
            }
        }

        private void DgvStations_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (ActiveControl != null && ActiveControl == dgvStations && dgvStations.ContainsFocus && !radioBtnChanged)
            {
                radioBtnChanged = true;
                UpdateStatusLabelStationsList();
            }
        }
        private void SplashForm_Activated(object sender, EventArgs e) { dgvStations.EndEdit(); }

        private void DgvStations_MouseClick(object sender, MouseEventArgs e)
        {
            DataGridView.HitTestInfo hitInfo = dgvStations.HitTest(e.X, e.Y);
            if (hitInfo.Type == DataGridViewHitTestType.RowHeader || hitInfo.Type == DataGridViewHitTestType.TopLeftHeader) { dgvStations.EndEdit(); }
        }

        private void FrmMain_Click(object sender, EventArgs e)
        {
            if (tcMain.SelectedTab == tpStations) { dgvStations.EndEdit(); }
        }

        private void StatusStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (tcMain.SelectedTab == tpStations) { dgvStations.EndEdit(); }

        }

        private void LblD4_TextChanged(object sender, EventArgs e)
        {
            using Graphics g = CreateGraphics();
            if ((int)g.MeasureString(lblD4.Text, lblD4.Font, 0, StringFormat.GenericTypographic).Width > lblD4.Width)
            {
                if (toolTip.GetToolTip(lblD4) != lblD4.Text) { toolTip.SetToolTip(lblD4, lblD4.Text); }
            }
            else { toolTip.SetToolTip(lblD4, null); } // ToolTip zurücksetzen
        }

        private void LblD2_TextChanged(object sender, EventArgs e)
        {
            using Graphics g = CreateGraphics();
            if ((int)g.MeasureString(lblD2.Text, lblD2.Font, 0, StringFormat.GenericTypographic).Width > lblD2.Width)
            {
                if (toolTip.GetToolTip(lblD2) != lblD2.Text) { toolTip.SetToolTip(lblD2, lblD2.Text); }
            }
            else { toolTip.SetToolTip(lblD2, null); } // ToolTip zurücksetzen
        }

        private void PanelLevel_Paint(object sender, PaintEventArgs e)
        {
            Panel p = sender as Panel;
            LinearGradientBrush myBrush = new(p.PointToClient(p.Parent.PointToScreen(p.Location)), new Point(p.Width, p.Height), Color.AliceBlue, Color.LightSteelBlue);
            e.Graphics.FillRectangle(myBrush, ClientRectangle);
        }

        //private void tcMain_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    if (e.Index == 7)
        //    {
        //        Color cColor = Color.LightPink;
        //        using (Brush br = new SolidBrush(cColor))
        //        {
        //            e.Graphics.FillRectangle(br, e.Bounds);
        //            SizeF sz = e.Graphics.MeasureString(tcMain.TabPages[e.Index].Text, e.Font);
        //            e.Graphics.DrawString(tcMain.TabPages[e.Index].Text, e.Font, Brushes.Black, e.Bounds.Left + (e.Bounds.Width - sz.Width) / 2, e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1);

        //            Rectangle rect = e.Bounds;
        //            rect.Offset(0, 1);
        //            rect.Inflate(0, -1);
        //            e.Graphics.DrawRectangle(Pens.DarkGray, rect);
        //            e.DrawFocusRectangle();
        //        }
        //    }
        //    else
        //    {
        //        e.DrawBackground();
        //        Color color; Color tabTextColor;
        //        if (e.Index == tcMain.SelectedIndex)
        //            color = Color.White;
        //        else
        //        {
        //            tabTextColor = Color.FromArgb(0x000001);
        //            color = Color.FromArgb(tabTextColor.R, tabTextColor.G, tabTextColor.B);
        //        }
        //        TextRenderer.DrawText(e.Graphics, tcMain.TabPages[e.Index].Text, e.Font, e.Bounds, color);
        //    }
        //}
    }
}