using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using NetRadio.cls;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;

namespace NetRadio;

public partial class FrmMain : Form
{
    //internal static HttpClient? MainHttpClient => httpClient;
    internal static bool MainClose2Tray => close2Tray;

    private readonly string _myUserAgent = "NetRadio";
    [FixedAddressValueType()]
    internal IntPtr _myUserAgentPtr;
    internal delegate void UpdateMessageDelegate(string txt);
    internal delegate void UpdateTagDelegate();
    internal delegate void UpdateStatusDelegate(string txt);
    private int _stream = 0;
    private readonly DOWNLOADPROC myStreamCreateURL;
    private byte[]? _data; // local recording buffer
    private FileStream? _fs = null;
    private bool _recording = false;
    private string? _downloadFileName;
    private string? _channelFilename;
    private TAG_INFO? _tagInfo;
    private SYNCPROC? _connectFail;
    private SYNCPROC? _deviceFail;
    private SYNCPROC? _metaSync;
    private readonly int _hlsPlugIn = 0;
    private readonly int _opusPlugIn = 0;
    private readonly int _flacPlugIn = 0;
    private int _downlaodSize = 0;
    private int _currentButtonNum = 0; // wird im RadioButton_CheckedChanged auf Werte > 0 gesetzt; zurücksetzen auf 0 erfolgt manuell!
    private readonly Version curVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version("0.0.0");
    private readonly string strVersion = "unbekannt";
    private static bool alwaysOnTop;
    private static bool logHistory = true;
    private static bool showTrayInfo = true;
    private static bool autoStopRecording;
    private static bool startMiniCmd; // Miniplayer Command line
    private static bool startTrayCmd; // TrayModus Command line
    private bool mainShown;
    private static bool updateAvailable;
    private bool somethingToSave;
    private bool radioBtnChanged; // ersetzt auf Station-Tab nothingToSave
    private string strCellValue = string.Empty;
    private static readonly string appName = Application.ProductName ?? "NetRadio";
    private static readonly string appPath = Application.ExecutablePath; // EXE-Pfad
    private readonly string xmlPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, appName + ".xml");
    private readonly string bakPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, appName + ".bak");
    private readonly string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, appName + ".log");
    private string hkLetter = string.Empty; // Flag für existierenden Hotkey. AUSNAHME: Programmstart
    private static int lastHotkeyPress;
    private int rowIndexFromMouseDown;
    private int colIndexFromMouseDown;
    private Rectangle dragBoxFromMouseDown;
    private int rowIndexOfItemUnderMouseToDrop;
    private string? autostartStation;
    private bool firstEmptyStart = false;
    private static bool close2Tray = false;
    private bool showBalloonTip = false;
    private bool repeatActionsDaily;
    private float channelVolume = 1.0f;
    private readonly int stationSum = 25; // Rows = stationSum * 2
    //private static HttpClient? httpClient;
    private Control? currentDisplayLabel;
    private int levelLeft, levelRight;
    private readonly RadioButton? autoStartRadioButton = null;
    private int recIncrement = 0;
    private string localSetupFile = string.Empty;
    private string downloadUpdateURL = string.Empty;
    private int intOutputDevice = 0; //  0 = default => Init(-1)
    private string? strOutputDevice;
    private string? prevOutputDevice;
    private bool changeOutputDevice = false;
    private readonly string findNewStations = "Press <Ctrl+F> to find new radio stations.";
    private bool helpRequested = true;
    private readonly MiniPlayer miniPlayer = new();
    private readonly string[] strArrHistory = new string[3];
    private readonly string[] lvSortOrderArray = new string[3];
    private ListViewItem? lvItemHistory;
    private readonly CListViewItemComparer lviComparer = new();      // Sortierer für die ListView
    private readonly BASSTimer spectrumTimer = new(); // Creates a new Timer instance using a default interval of 50ms => 20 Hz.
    private readonly List<byte> spectrumData = [];
    private readonly int spectrumlines = 20;
    private TimeSpan currPlayingTime = TimeSpan.Zero;
    private TimeSpan totalPlayingTime = TimeSpan.Zero;
    private readonly float _netPreBuff;
    private bool _isBuffering = false;
    private bool _playWakeFromSleep = false;
    private long accumulatedTicks;
    private readonly DataTable? tableActions = new();
    private static readonly Timer timerAction1 = new();
    private static readonly Timer timerAction2 = new();
    private static readonly Timer timerAction3 = new();
    private static readonly Timer timerAction4 = new();
    private static readonly Timer timerAction5 = new();
    private static readonly Timer timerAction6 = new();
    private static readonly Timer timerAction7 = new();
    private static readonly Timer timerAction8 = new();
    private static readonly Timer timerAction9 = new();
    private SplashForm? frmSplash = null;
    private int updateIndex = 0; // täglich
    private int startMode = 0; // Main window
    private DateTime lastUpdateTime;
    private readonly string readDateFormat = "yyyy-MM-dd HH:mm:ss:fff"; // wird innerhalb der History-CSV-Dateien verwendet
    private readonly string longDateFormat = "yyyyMMddHHmmssfff";      // lastUpdateTime, ListViewItem.Tag (CListViewItemComparer)
    private readonly string shortDateFormat = "yyyyMMdd-HHmmss";      // LogEvent, _downloadFileName, historyFile
    private Version? updateVersion = null;
    private readonly string? formPosX;
    private readonly string? formPosY;
    private readonly string? formWidth;
    private readonly string? formHeight;
    private readonly string? miniPosX;
    private readonly string? miniPosY;
    private bool doubleClickOccurred = false; // NotifyIcon

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
        _myUserAgentPtr = Marshal.StringToHGlobalAnsi(_myUserAgent);
        CreateLogFile();
        if (curVersion is not null) { strVersion = string.Join('.', new[] { curVersion.Major, curVersion.Minor, curVersion.Build >= 0 ? curVersion.Build : 0 }); }
        LogEvent(appName + ": Version " + strVersion);
        LogEvent("IsUserAdmin: " + NativeMethods.IsUserAnAdmin());  //.IsUserAdminManaged()); 
        var cores = Environment.ProcessorCount;
        LogEvent("ProcessorCount: " + cores);
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
                Utilities.MsgTaskDialog(this, "bassflac.dll", Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode()), TaskDialogIcon.Warning);
                Environment.Exit(0); return;
            }
            _opusPlugIn = Bass.BASS_PluginLoad("bassopus.dll");
            if (_opusPlugIn <= 0)
            {
                Utilities.MsgTaskDialog(this, "bassopus.dll", Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode()), TaskDialogIcon.Warning);
                Environment.Exit(0); return;
            }
            _hlsPlugIn = Bass.BASS_PluginLoad("basshls.dll");
            if (_hlsPlugIn <= 0)
            {
                Utilities.MsgTaskDialog(this, "basshls.dll", Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode()), TaskDialogIcon.Warning);
                Environment.Exit(0); return;
            }
            myStreamCreateURL = new DOWNLOADPROC(MyDownloadProc); // Internet stream download callback function
            LogEvent("BASS_Init: initialized");
        }
        else
        {
            var strError = Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode());
            strError = string.IsNullOrEmpty(strError) ? "bass.dll was not found.\nRe-installing may fix this problem." : strError;
            Utilities.MsgTaskDialog(this, "bass.dll", strError, TaskDialogIcon.Warning);
            Environment.Exit(0); return;
        }

        Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref channelVolume);
        Text = Assembly.GetCallingAssembly().GetName().Name + " " + new Regex(@"^\d+\.\d+").Match(strVersion).Value;
        lblUpdate.Text = "Current version: " + strVersion;
        for (var j = 0; j < stationSum * 4; j++) { dgvStations.Rows.Add("", ""); } // dgvStations.Rows.Add(stationSum); ist wahrscheinlich schlechter, weil Cell.Value = null entsteht

        tableActions.Columns.Add("Enabled", typeof(bool));
        tableActions.Columns.Add("Task", typeof(string));
        tableActions.Columns.Add("Station", typeof(string));
        tableActions.Columns.Add("Time", typeof(string));
        if (!Utilities.IsInnoSetupValid(Path.GetDirectoryName(appPath) ?? string.Empty)) // Portable-Version; prüft auch Debugger.IsAttached
        {
            xmlPath = Path.ChangeExtension(appPath, ".xml");
            bakPath = Path.ChangeExtension(appPath, ".bak");
            logPath = Path.ChangeExtension(appPath, ".log");
            LogEvent("IsInnoSetupValid: Portable version");
        }
        else { LogEvent("IsInnoSetupValid: Setup version"); }

        if (File.Exists(xmlPath) && (!File.Exists(bakPath) || File.GetLastWriteTime(bakPath).Date < File.GetLastWriteTime(xmlPath).Date.AddDays(-1)))
        {
            File.Copy(xmlPath, bakPath, true);
            File.SetLastWriteTime(bakPath, DateTime.Now);
        }

        if (File.Exists(xmlPath))
        {
            using XmlTextReader xtr = new(xmlPath);
            xtr.WhitespaceHandling = WhitespaceHandling.None; // Whitespace zwischen Elementen
            try
            {
                var j = 0;
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
                        if (int.TryParse(xtr.Value, out var intEnabeld)) { cbHotkey.Checked = lblHotkey.Enabled = cmbxHotkey.Enabled = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
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
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "CloseToTray")
                    {
                        xtr.MoveToAttribute("Enabled");
                        if (int.TryParse(xtr.Value, out var intEnabeld)) { cbClose2Tray.Checked = close2Tray = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                        else { cbClose2Tray.Checked = false; }
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "BalloonTips")
                    {
                        xtr.MoveToAttribute("Enabled");
                        if (int.TryParse(xtr.Value, out var intEnabeld)) { cbShowBalloonTip.Checked = showBalloonTip = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                        else { cbShowBalloonTip.Checked = false; }
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "AlwaysOnTop")
                    {
                        xtr.MoveToAttribute("Enabled");
                        if (int.TryParse(xtr.Value, out var intEnabeld)) { miniPlayer.TopMost = cbAlwaysOnTop.Checked = alwaysOnTop = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                        else { miniPlayer.TopMost = cbAlwaysOnTop.Checked = false; } // s. MiniPlayer_Shown-Event in frmMiniPlayer.cs
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "LogHistory")
                    {
                        xtr.MoveToAttribute("Enabled");
                        if (int.TryParse(xtr.Value, out var intEnabeld)) { cbLogHistory.Checked = logHistory = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                        else { cbLogHistory.Checked = false; }
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "ShowTrayInfo")
                    {
                        xtr.MoveToAttribute("Enabled");
                        if (int.TryParse(xtr.Value, out var intEnabeld)) { showTrayInfo = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                        else { showTrayInfo = true; }
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "AutoStopRecording")
                    {
                        xtr.MoveToAttribute("Enabled");
                        if (int.TryParse(xtr.Value, out var intEnabeld)) { cbAutoStopRecording.Checked = autoStopRecording = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                        else { cbAutoStopRecording.Checked = false; }
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "Volume")
                    {
                        xtr.MoveToAttribute("Value");
                        var volume = int.TryParse(xtr.Value, out var intVolume) ? intVolume : (int)channelVolume * 100;
                        volume = volume > 100 ? 100 : volume < 0 ? 0 : volume;
                        channelVolume = Convert.ToSingle(volume) / 100f;
                        Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, channelVolume);
                        miniPlayer.MpVolProgBar.Value = volProgressBar.Value = volume;
                        lblVolume.Text = volProgressBar.Value.ToString();
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "SaveHistory")
                    {
                        xtr.MoveToAttribute("Value");
                        numUpDnSaveHistory.Value = int.TryParse(xtr.Value, out var intHistory) ? intHistory : 0;
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "StartMode")
                    {
                        xtr.MoveToAttribute("Value");
                        startMode = int.TryParse(xtr.Value, out var intMode) ? intMode : 0;
                        if (startMode == 0) { rbStartModeMain.Checked = true; }
                        else if (startMode == 1) { rbStartModeMini.Checked = true; }
                        else if (startMode == 2) { rbStartModeTray.Checked = true; }
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "UpdateIndex")
                    {
                        xtr.MoveToAttribute("Value");
                        updateIndex = int.TryParse(xtr.Value, out var intUpdate) ? intUpdate : 0;
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "UpdateSearch")
                    {
                        xtr.MoveToAttribute("DateTime");
                        lastUpdateTime = DateTime.TryParseExact(xtr.Value, longDateFormat, null, DateTimeStyles.None, out var date) ? date : DateTime.UtcNow;
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "FormLocation")
                    {
                        xtr.MoveToAttribute("PosX");
                        formPosX = xtr.Value;
                        xtr.MoveToAttribute("PosY");
                        formPosY = xtr.Value;
                        xtr.MoveToAttribute("Width");
                        formWidth = xtr.Value;
                        xtr.MoveToAttribute("Height");
                        formHeight = xtr.Value;
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
                        if (int.TryParse(xtr.Value, out var intStation))
                        {
                            if (intStation > 0 && intStation <= stationSum) { cmbxStation.Text = autostartStation = xtr.Value; }
                        }
                    }
                    else if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "RepeatActionsDaily")
                    {
                        xtr.MoveToAttribute("Enabled");
                        if (int.TryParse(xtr.Value, out var intEnabeld)) { repeatActionsDaily = Convert.ToBoolean(Convert.ToInt16(intEnabeld)); }
                    }
                    if (xtr.NodeType == XmlNodeType.Element && xtr.LocalName == "Action")
                    {
                        xtr.MoveToAttribute("Enabled");
                        if (!bool.TryParse(xtr.Value, out var enabled)) { enabled = false; }
                        else if (enabled == true) { cbActions.Checked = true; }
                        xtr.MoveToAttribute("Task");
                        var task = xtr.Value;
                        xtr.MoveToAttribute("Station");
                        var station = xtr.Value;
                        xtr.MoveToAttribute("Time");
                        var time = xtr.Value;
                        tableActions.Rows.Add(enabled, task, station, time);
                    }
                }
            }
            catch (XmlException ex) { Utilities.ErrTaskDialog(null, ex); }
        }
        else
        {
            Directory.CreateDirectory(Path.GetDirectoryName(xmlPath) ?? ""); // If the folder exists already, the line will be ignored.
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?><NetRadio></NetRadio>");
            xmlDoc.Save(xmlPath); // if the specified file exists, this method overwrites it.
            firstEmptyStart = true;
            LogEvent("New config file: " + xmlPath);
        }
        foreach (DataGridViewColumn column in dgvStations.Columns) { column.SortMode = DataGridViewColumnSortMode.NotSortable; }

        if (tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled"))) { cbActions.Checked = true; }

        if (!logHistory) { numUpDnSaveHistory.Value = 0; } // numUpDnSaveHistory.Text = "0";

        string[] args = [.. Environment.GetCommandLineArgs().Skip(1)];
        if (args.Length > 0)
        {
            for (var i = 0; i < args.Length; i++) // Kommandozeilenargumente werden vor Autostart-Einstellungen benutzt
            {
                if (Regex.IsMatch(args[i], @"^[/-][1-9][0-5]?$") && int.TryParse(args[i][1..], out var intStation))
                {
                    var btnName = "rbtn" + intStation.ToString("D2"); // Math.Abs nicht nötig wg. [1..]
                    var controls = tcMain.TabPages[0].Controls.Find(btnName, true);
                    if (controls.Length == 1 && controls[0] is RadioButton button) { autoStartRadioButton = button; } // foundBtn.Checked = true;
                }
                else if (Regex.IsMatch(args[i], @"^[/-](m|mini)$", RegexOptions.IgnoreCase))
                {
                    startMiniCmd = true; // siehe frmMain_Shown-Event
                    Opacity = 0; // sonst wird GUI kurz angezeigt - unschön
                }
                else if (Regex.IsMatch(args[i], @"^[/-](t|tray)$", RegexOptions.IgnoreCase))
                {
                    startTrayCmd = true; // siehe frmMain_Shown-Event
                    Opacity = 0; // sonst wird GUI kurz angezeigt - unschön
                }
            }
        }

        if (startMode == 1 && !startTrayCmd && !startMiniCmd) // Miniplayer
        {
            startMiniCmd = true; // siehe frmMain_Shown-Event
            Opacity = 0; // sonst wird GUI kurz angezeigt - unschön
        }
        else if (startMode == 2 && !startMiniCmd && !startTrayCmd) // tray mode
        {
            startTrayCmd = true; // siehe frmMain_Shown-Event
            Opacity = 0; // sonst wird GUI kurz angezeigt - unschön
        }

        if (autoStartRadioButton == null && !string.IsNullOrEmpty(autostartStation)) // kein Kommandozeilenargumente - dann Autostart-Einstellungen benutzen
        {
            LogEvent("AutostartStation: " + autostartStation);
            var btnName = "rbtn" + autostartStation.PadLeft(2, '0');
            var controls = tcMain.TabPages[0].Controls.Find(btnName, true);
            if (controls.Length == 1 && controls[0] is RadioButton button) { autoStartRadioButton = button; } // löst StartPlaying aus (s. FrmMain_Shown-Event)
        }
        StatusStrip_SingleLabel(true, findNewStations);
        cbAutostart.Checked = Utilities.IsAutoStartEnabled(appName, "\"" + appPath + "\"" + " -min");

        historyLV.ListViewItemSorter = lviComparer;
        spectrumTimer.Tick += SpectrumTick;
        _netPreBuff = Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_PREBUF) / 100f; // 0.75

        timerNotifyIcon.Interval = SystemInformation.DoubleClickTime;
    }

    private void MyDownloadProc(IntPtr buffer, int length, IntPtr user)
    {
        if (buffer != IntPtr.Zero && length == 0)
        {
            var txt = Marshal.PtrToStringAnsi(buffer);
            Invoke(new UpdateMessageDelegate(UpdateMessageDisplay), [txt]);
        }
        else if (buffer != IntPtr.Zero && _recording)
        {
            try
            {
                if (_fs is null)
                {
                    NativeMethods.SHGetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0, IntPtr.Zero, out var downloadPath);
                    _downloadFileName = downloadPath + "\\" + appName + "_" + DateTime.Now.ToString(shortDateFormat) + ".mp3";
                    var info = Bass.BASS_ChannelGetInfo(_stream);
                    switch (info.ctype)
                    {
                        case BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG:
                        case BASSChannelType.BASS_CTYPE_STREAM_OPUS:
                        case BASSChannelType.BASS_CTYPE_STREAM_OGG:
                            _recording = false; // downloadFileName = Path.ChangeExtension(downloadFileName, ".opus");
                            BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { RecordingStop(false); }); //uint uiFlags = /*MB_OK*/ 0x00000000 | /*MB_SETFOREGROUND*/  0x00010000 | /*MB_APPLMODAL*/ 0x00001000 | /*MB_ICONEXCLAMATION*/ 0x00000030;
                            Utilities.MsgTaskDialogTimeout(this, "Format not supported", "Recording is only available for MP3 and AAC streams.", 3, TaskDialogIcon.Information);
                            return;
                        //if (NativeMethods.MessageBoxTimeout(NativeMethods.GetForegroundWindow(), $"Recording is only available for MP3 and AAC streams.", $"NetRadio", 0x00000000 | 0x00010000 | 0x00000000 | 0x00000040, 0, 3000) > 0) { return; }
                        //else { break; }
                        case BASSChannelType.BASS_CTYPE_STREAM_MF:
                            if (Marshal.PtrToStructure<WAVEFORMATEX>(Bass.BASS_ChannelGetTags(_stream, BASSTag.BASS_TAG_WAVEFORMAT))?.wFormatTag == WAVEFormatTag.MPEG_HEAAC) { _downloadFileName = Path.ChangeExtension(_downloadFileName, ".aac"); } // High-Efficiency Advanced Audio Coding (HE-AAC) 
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
                    BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { RecordingStop(); }); // setzt _fs auf null; this code runs on the UI thread!
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
                        BeginInvoke((System.Windows.Forms.MethodInvoker)delegate () { lblD4.Text = "Downloading " + Utilities.GetFileSize(_downlaodSize); });
                    }
                }
            }
            catch (IOException ex)
            {
                _recording = false;
                BeginInvoke((System.Windows.Forms.MethodInvoker)delegate ()// this code runs on the UI thread!
                {
                    RecordingStop(); // setzt _fs auf null; 
                    Utilities.ErrTaskDialog(this, ex); // "An error occurred. Recording has stopped."
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
            lblD3.Text = lblD3.Text.StartsWith('◷') ? "◶" + lblD3.Text.TrimStart('◷') :
                         lblD3.Text.StartsWith('◶') ? "◵" + lblD3.Text.TrimStart('◶') :
                         lblD3.Text.StartsWith('◵') ? "◴" + lblD3.Text.TrimStart('◵') : "◷" + lblD3.Text[1..];
            MiniPlayer.MpLblD2_Text(lblD3.Text);
        }
    }

    private void UpdateMessageDisplay(string txt) => lblD4.Text = txt; // HTTP 0.2 OK

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

    private void ConnectionSync(int handle, int channel, int data, IntPtr user) // BASS_SYNC_DOWNLOAD informs when the connection is closed
    {
        BeginInvoke(() =>
        {
            LogEvent("ConnectionSync: Internet connection disconnected/disabled");
            timerLevel.Stop();
            spectrumTimer.Stop();
            pbLevel.Image = null;
            miniPlayer.MpPBLevel.Image = null;
            foreach (var vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
            if (Bass.BASS_ChannelIsActive(_stream) != BASSActive.BASS_ACTIVE_PLAYING) { lblD3.Text = "⌛Connecting..."; }
            MiniPlayer.MpLblD2_Text("⌛Connecting..."); //.Replace("ERROR:", "⚠"));
            btnPlayStop.Image = Properties.Resources.play_white;
            miniPlayer.MpBtnPlay.Image = Properties.Resources.play_white;
            playPauseToolStripMenuItem.Text = "Play"; // btnPlayStop.Text = 
            playPauseToolStripMenuItem.Image = Properties.Resources.play;
            var info = Bass.BASS_ChannelGetInfo(_stream);
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
            LogEvent("DeviceSync: Output device disconnected or disabled");
            Application.DoEvents(); // damit vorstehender Text angezeigt wird
            System.Threading.Thread.Sleep(1000); // andernfalls werden die gerade entfernten Devices als noch vorhanden angezeigt
            var devices = 0;
            BASS_DEVICEINFO dInfo;
            for (var n = 1; (dInfo = Bass.BASS_GetDeviceInfo(n)) != null; n++) { if (dInfo.IsEnabled) { devices++; } }
            if (devices > 0) // intOutputDevice wurde mit Wert 0 definiert
            {
                timerLevel.Stop();
                spectrumTimer.Stop();
                foreach (var vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
                intOutputDevice = 0;
                strOutputDevice = "Default";
                somethingToSave = true;
                var info = Bass.BASS_ChannelGetInfo(_stream);
                if (info != null && Bass.BASS_ChannelStop(_stream) && Bass.BASS_Stop() && Bass.BASS_Free()) { StartPlaying(info.filename, _currentButtonNum); }
                else
                {
                    var strError = Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode());
                    LogEvent("DeviceSync: " + strError);
                    if (string.IsNullOrEmpty(strError)) { strError = lblD3.Text; }
                    Bass.BASS_ChannelStop(_stream);
                    Bass.BASS_Free();
                    RestorePlayerDefaults();
                    Utilities.MsgTaskDialog(this, strError, string.Empty, TaskDialogIcon.Information);
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
        if (data != 0) { Invoke(new UpdateStatusDelegate(UpdateStatusDisplay), [Marshal.PtrToStringAnsi(new IntPtr(data))]); }
        else
        {
            try
            {
                if (_tagInfo != null && _tagInfo.UpdateFromMETA(Bass.BASS_ChannelGetTags(channel, BASSTag.BASS_TAG_META | BASSTag.BASS_TAG_ID3V2), TAGINFOEncoding.Utf8OrLatin1, true)) { Invoke(new UpdateTagDelegate(UpdateTagDisplay)); }
            }
            catch (ArgumentOutOfRangeException) { } // Wenn Text mehr als 64 Zeichen hat
        }
    }

    private void UpdateStatusDisplay(string txt) => toolStripStatusLabel.Text = txt;

    private void UpdateTagDisplay()
    {
        if (_recording && autoStopRecording)
        {
            btnRecord.PerformClick(); btnRecord.Focus(); // RecordingStop();
            return;
        }
        if (_tagInfo != null)
        {
            lblD2.Text = _tagInfo.ToString().Replace("&", "&&"); // & wird sonst als Akzelerator interpretiert (nächstes Zeichen wird unterstrichen)
            MiniPlayer.MpLblD2_Text(lblD2.Text);
            if (tcMain.SelectedTab == tpSectrum) { StatusStrip_SingleLabel(false, lblD2.Text); }
            if (logHistory) { AddToHistory(_tagInfo.ToString()); }
            lblD4.Text = _tagInfo.filename;
            if (showBalloonTip)
            {
                var foregroundWin = NativeMethods.GetForegroundWindow();
                if (foregroundWin != miniPlayer.Handle && foregroundWin != Handle) { notifyIcon.ShowBalloonTip(2, "Now playing: ", lblD2.Text, ToolTipIcon.Info); }
            }
            //LogEvent("UpdateTagDisplay: " + _tagInfo.title);
        }
        else { lblD4.Text = dgvStations.Rows[_currentButtonNum - 1].Cells[1].Value.ToString(); }
    }

    private void SpectrumTick(object? sender, EventArgs e)
    {
        if (tcMain.SelectedTab != tpSectrum) { return; }
        var _fft = new float[1024];
        var ret = Bass.BASS_ChannelGetData(_stream, _fft, (int)BASSData.BASS_DATA_FFT2048); // get fft data, BASS_ChannelGetData(int handle, float[] buffer, int length)
        if (ret < -1) { return; }
        int x, y;
        var b0 = 0;
        for (x = 0; x < spectrumlines; x++) //computes the spectrum data
        {
            float peak = 0;
            var b1 = (int)Math.Pow(2, x * 10.0 / (spectrumlines - 1));
            if (b1 > 1023) { b1 = 1023; }
            if (b1 <= b0) { b1 = b0 + 1; }
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
        if (string.IsNullOrEmpty(songTitle)) { return; }
        strArrHistory[0] = DateTime.Now.ToString("HH:mm:ss");
        strArrHistory[1] = (tcMain.TabPages.Count > 0 ? tcMain.TabPages[0].Controls["rbtn" + _currentButtonNum.ToString("D2")] as RadioButton : null)?.Text.Replace("&&", "&") ?? string.Empty;
        strArrHistory[2] = songTitle.Replace("&&", "&"); // frühere Umwandlung wg. Akzelerator rückgängig machen
        lvItemHistory = new ListViewItem(strArrHistory)
        {
            Tag = DateTime.Now.ToString(longDateFormat), // DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local).ToString("s")
            ToolTipText = LongSubItemText(songTitle) ? songTitle : ""
        };
        historyLV.Items.Insert(0, lvItemHistory);
        historyExportButton.Enabled = histoyClearButton.Enabled = true;
        HistoryListView_SetDefaultColumnWidth();
        if (tcMain.SelectedIndex == 2) { TPHistory_SetStatusBarText(); }
    }

    private bool LongSubItemText(string songTitle)
    {
        using var g = CreateGraphics();
        return (int)g.MeasureString(songTitle, historyLV.Font, 0, StringFormat.GenericTypographic).Width > historyLV.Columns[2].Width;
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
        var sysMenuHandle = NativeMethods.GetSystemMenu(Handle, false);
        NativeMethods.AppendMenu(sysMenuHandle, NativeMethods.MF_BYPOSITION | NativeMethods.MF_SEPARATOR, 0, string.Empty);
        NativeMethods.AppendMenu(sysMenuHandle, NativeMethods.MF_BYPOSITION, NativeMethods.IDM_CUSTOMITEM1, "Exit\tShift+Esc");

        lblAuthor.Text = "© 2015-" + Utilities.GetBuildDate().ToString("yyyy") + " Wilhelm Happe";
        lbUn4SeenVersion.Text = $"(v{Bass.BASS_GetVersion(4)})";
        lblRadio42Version.Text = $"(v{Utils.GetVersion()})";
        List<string> devicelist = [];
        var defaultDevice = -1;
        BASS_DEVICEINFO info; // = new BASS_DEVICEINFO();
        for (var n = 1; (info = Bass.BASS_GetDeviceInfo(n)) != null; n++) // Device 0 is always the "no sound" device, so you should start at device 1 if you only want to list real output devices.
        {
            if (info.IsEnabled) { devicelist.Add(info.ToString()); }
            if (info.IsDefault && !info.ToString().Equals("Default")) //  .status.Equals(BASSDeviceInfo.BASS_DEVICE_ENABLED))
            {
                defaultDevice = n - 1;
                LogEvent("FrmMain_Load: " + info.ToString() + " is the default device");
            }
        }
        if (devicelist.Count > 0) // intOutputDevice wurde mit Wert 0 definiert
        {
            intOutputDevice = devicelist.IndexOf(strOutputDevice ?? ""); // in dieser Liste ist 0 = Default
            if (intOutputDevice == defaultDevice) { intOutputDevice = 0; } // Sieht wie in Bug aus, ist aber ein Feature, um wenn möglich "Default" zu erzwingen. BASS_GetDeviceInfo gibt niemals Default aus, sondern immer die höhere DeviceID
            if (intOutputDevice <= 0) { intOutputDevice = 0; } // 0 = Default
            strOutputDevice = devicelist[intOutputDevice].ToString();
            LogEvent("FrmMain_Load: " + strOutputDevice + " (" + intOutputDevice + ") is the current device");
        }

        miniPlayer.Show(this);
        miniPlayer.FormExit += new EventHandler(MiniPlayer_AppExit);
        miniPlayer.FormHide += new EventHandler(MiniPlayer_FormHide);
        miniPlayer.FormMove += new EventHandler(MiniPlayer_FormMove);
        miniPlayer.PlayPause += new EventHandler(MiniPlayer_PlayPause);
        miniPlayer.PlayerReset += new EventHandler(MiniPlayer_PlayReset);
        miniPlayer.VolumeProgress += new EventHandler(MiniPlayer_VolumeProgress);
        miniPlayer.VolumeMouseWheel += new MouseEventHandler(MiniPlayer_VolumeMouseWheel);
        miniPlayer.IncreaseVolume += new EventHandler(MiniPlayer_IncreaseVolume);
        miniPlayer.DecreaseVolume += new EventHandler(MiniPlayer_DecreaseVolume);
        miniPlayer.StationChanged += new EventHandler(MiniPlayer_StationChanged);
        miniPlayer.F4_ShowPlayer += new EventHandler(MiniPlayer_F4_ShowPlayer);
        miniPlayer.F5_ShowHistory += new EventHandler(MiniPlayer_F5_ShowHistory);
        miniPlayer.F12_ShowSpectrum += new EventHandler(MiniPlayer_F12_ShowSpectrum);

        SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(PowerMode_Changed);
        RewriteButtonText(); // enthält miniPlayer.MpCmBxStations.Items.Add()-Loop

        if (cbActions.Checked) { PrepareActions(); }
        var vScrollBar = dgvStations.Controls.OfType<VScrollBar>().FirstOrDefault();
        if (vScrollBar != null) { vScrollBar.MouseCaptureChanged += (s, e) => { dgvStations.EndEdit(); }; }

        if (NativeMethods.RegisterMediaKeys() > 0)
        {
            NativeMethods.KeyDown += new KeyEventHandler(GlobalKeyboardHook_KeyDown);
            LogEvent("RegisterMediaKeys: success");
        }
        else
        {
            Console.Beep();
            LogEvent("RegisterMediaKeys: failed");
        }
        //var primaryScreen = Screen.PrimaryScreen;  // Null-Prüfung für PrimaryScreen
        //var screen = primaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1024, 768); // Beispiel-Standardwert
        if (int.TryParse(formPosX, out var fx) && int.TryParse(formPosY, out var fy) && int.TryParse(formWidth, out var fWidth) && int.TryParse(formHeight, out var fHeight))
        {
            var savedBounds = new Rectangle(fx, fy, fWidth, fHeight);
            var isVisibleOnAnyScreen = false;
            foreach (var screen in Screen.AllScreens)
            {
                if (screen.WorkingArea.IntersectsWith(savedBounds)) // Überschneidet sich die gespeicherte Position mit irgendeinem der aktuell verfügbaren Bildschirme?
                {
                    isVisibleOnAnyScreen = true;
                    break;
                }
            }
            if (isVisibleOnAnyScreen)
            {
                StartPosition = FormStartPosition.Manual;
                Bounds = savedBounds;
            }
            else { StartPosition = FormStartPosition.CenterScreen; } // Position ist "verwaist" -> zentrieren.
        }

        var currScreen = Screen.FromControl(this).WorkingArea; // Null-Prüfung für PrimaryScreen
        if (int.TryParse(miniPosX, out var x_Pos) && int.TryParse(miniPosY, out var y_Pos))
        {
            x_Pos = x_Pos < 0 ? 0
                : x_Pos + miniPlayer.Width > currScreen.Width
                ? currScreen.Width - miniPlayer.Width
                : x_Pos;
            y_Pos = y_Pos < 0
                ? 0 // Korrektur wie oben
                : y_Pos + miniPlayer.Height > currScreen.Height
                ? currScreen.Height - miniPlayer.Height // Korrektur wie oben
                : y_Pos;
            miniPlayer.Location = new Point(x_Pos, y_Pos);
        }
        else { miniPlayer.Location = Location; }
    }

    private void GlobalKeyboardHook_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.MediaPlayPause)
        {
            BtnPlayStop_Click(null!, null!);
            LogEvent("GlobalKeyboardHook: MediaPlayPause received");
        }
        else if (e.KeyCode == Keys.MediaStop)
        {
            var iTag = 0;
            foreach (var rb in tcMain.TabPages[0].Controls.OfType<RadioButton>().Where(rb => rb.Checked)) { iTag = Convert.ToInt32(rb.Tag); }
            Bass.BASS_ChannelStop(_stream);
            Bass.BASS_Free();
            RestorePlayerDefaults(iTag);
            LogEvent("GlobalKeyboardHook: MediaStop received");
        }
        else if (e.KeyCode == Keys.MediaNextTrack)
        {
            var iTag = 0;  // 1 bis 25
            var listIndex = 0; // Index in radioButtonTagsList (1 bis <=25)
            List<int> radioButtonTagsList = [];
            for (var i = 0; i < stationSum; i++)
            {
                if (tcMain.TabPages[0].Controls["rbtn" + (i + 1).ToString("D2")] is RadioButton rb)
                {
                    var tag = Convert.ToInt32(rb.Tag);
                    if (!string.IsNullOrEmpty(dgvStations.Rows[tag - 1].Cells[1].Value?.ToString()))
                    {
                        radioButtonTagsList.Add(tag);
                        if (rb.Checked)
                        {
                            iTag = tag;
                            listIndex = radioButtonTagsList.Count;
                        }
                    }
                }
            }
            if (iTag > 0)
            {
                iTag = listIndex < radioButtonTagsList.Count ? radioButtonTagsList[listIndex] : radioButtonTagsList[0];
                if (tcMain.TabPages[0].Controls["rbtn" + iTag.ToString("D2")] is RadioButton rb) { rb.Checked = true; } // löst StartPlaying aus    
            }
            LogEvent("GlobalKeyboardHook: MediaNextTrack received");
        }
        else if (e.KeyCode == Keys.MediaPreviousTrack)
        {
            var iTag = 0;  // 1 bis 25
            var listIndex = 0; // Index in radioButtonTagsList (1 bis <=25)
            List<int> radioButtonTagsList = [];
            for (var i = 0; i < stationSum; i++) //foreach (RadioButton…) // iteriert von 25 nach 1 statt umgekehrt
            {
                if (tcMain.TabPages[0].Controls["rbtn" + (i + 1).ToString("D2")] is RadioButton rb)
                {
                    var tag = Convert.ToInt32(rb.Tag);
                    if (!string.IsNullOrEmpty(dgvStations.Rows[tag - 1].Cells[1].Value?.ToString()))
                    {
                        radioButtonTagsList.Add(tag);
                        if (rb.Checked)
                        {
                            iTag = tag;
                            listIndex = radioButtonTagsList.Count;
                        }
                    }
                }
            }
            if (iTag > 0)
            {
                iTag = listIndex <= 1 ? radioButtonTagsList.Last() : radioButtonTagsList[listIndex - 2];
                if (tcMain.TabPages[0].Controls["rbtn" + iTag.ToString("D2")] is RadioButton rb) { rb.Checked = true; } // löst StartPlaying aus    
            }
            LogEvent("GlobalKeyboardHook: MediaPreviousTrack received");
        }
        e.Handled = true;
    }

    private void MiniPlayer_AppExit(object? sender, EventArgs e)
    {
        SaveConfig();
        Application.Exit();
    }

    private void MiniPlayer_FormHide(object? sender, EventArgs e)
    {
        ShowFullPlayer();
        if (NativeMethods.IsKeyDown(Keys.Escape)) { toolTip.Active = false; } // Workaround for persistent ToolTip display
        else { toolTip.Active = true; }
    }

    private void MiniPlayer_FormMove(object? sender, EventArgs e) => somethingToSave = true;

    private void MiniPlayer_PlayReset(object? sender, EventArgs e) => BtnReset_Click(null!, e!);
    private void MiniPlayer_PlayPause(object? sender, EventArgs e) => BtnPlayStop_Click(null!, e);
    private void MiniPlayer_VolumeProgress(object? sender, EventArgs e) => SetProgressBarValue();
    private void MiniPlayer_VolumeMouseWheel(object? sender, MouseEventArgs e) => SetMouseWheelValue(e);
    private void MiniPlayer_IncreaseVolume(object? sender, EventArgs e) => BtnIncrease_Click(null!, null!);
    private void MiniPlayer_DecreaseVolume(object? sender, EventArgs e)
    {
        BtnDecrease_Click(null!, null!);
    }
    private void MiniPlayer_F4_ShowPlayer(object? sender, EventArgs e)
    {
        ShowFullPlayer(); tcMain.SelectedIndex = 0;
    }
    private void MiniPlayer_F5_ShowHistory(object? sender, EventArgs e)
    {
        ShowFullPlayer(); tcMain.SelectedIndex = 2;
    }
    private void MiniPlayer_F12_ShowSpectrum(object? sender, EventArgs e)
    {
        ShowFullPlayer(); tcMain.SelectedIndex = 6;
    }
    private void MiniPlayer_StationChanged(object? sender, EventArgs e)
    {
        for (var i = 0; i < stationSum; i++)
        {
            if (dgvStations.Rows[i].Cells[0].Value != null)
            {
                if (miniPlayer.MpCmBxStations.Text != null && miniPlayer.MpCmBxStations.Text == Utilities.StationLong(dgvStations.Rows[i].Cells[0].Value.ToString()))
                {
                    var btnName = "rbtn" + (i + 1).ToString().PadLeft(2, '0');
                    var controls = tcMain.TabPages[0].Controls.Find(btnName, true);
                    if (controls.Length == 1 && controls[0] is RadioButton rb) { rb.Checked = true; } // löst StartPlaying aus (BtnReset_Click in RadioButton_CheckedChanged)
                    break;
                }
            }
        }
    }

    private async void PowerMode_Changed(object sender, PowerModeChangedEventArgs e)
    {
        switch (e.Mode)
        {
            case PowerModes.StatusChange: // A power mode status notification event has been raised by the operating system.
                LogEvent("PowerMode_Changed: Notification from the system power supply");
                break;
            case PowerModes.Suspend: // The operating system is about to be suspended
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    _playWakeFromSleep = true;
                    LogEvent("PowerMode_Changed: The OS is about to be suspended (BASS_ACTIVE_PLAYING)");
                    Bass.BASS_ChannelStop(_stream);
                    Bass.BASS_Free();
                    Invoke(new Action(() => RestorePlayerDefaults(_currentButtonNum)));
                }
                else { LogEvent("PowerMode_Changed: The OS is about to be suspended (Netradio not playing)"); }
                break;
            case PowerModes.Resume: // when _playWakeFromSleep: // resume from a suspended state
                if (_playWakeFromSleep)
                {
                    LogEvent("PowerMode_Changed: Resuming from a suspended state (try playing)");
                    try
                    {
                        var max = 20;
                        for (var i = 0; i <= max; i++)
                        {
                            if (!_playWakeFromSleep) { return; } // falls StartPlaying manuell ausgelöst wurde
                            var foo = false; // one second more
                            if (Utilities.PingGoogleSuccess(Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT))) { foo = true; }
                            await Task.Delay(1000).ConfigureAwait(false);
                            if (foo) { break; }
                            else if (i == max)
                            {
                                Invoke(new Action(() =>
                                {
                                    if (tcMain.TabPages[0].Controls["rbtn" + (_currentButtonNum - 1).ToString("D2")] is RadioButton rb) { rb.Checked = false; }
                                }));
                                _playWakeFromSleep = false;
                                return;
                            }
                        }

                        var devices = 0;
                        BASS_DEVICEINFO dInfo;
                        max = 5;
                        for (var i = 0; i <= max; i++)
                        {
                            if (!_playWakeFromSleep) { return; } // falls StartPlaying manuell ausgelöst wurde
                            for (var n = 1; (dInfo = Bass.BASS_GetDeviceInfo(n)) != null; n++) { if (dInfo.IsEnabled) { devices++; } }
                            if (devices > 0) // intOutputDevice wurde mit Wert 0 definiert
                            {
                                LogEvent("PowerMode_Changed: Start playing station no. " + _currentButtonNum);
                                Invoke(new Action(() => StartPlaying(dgvStations.Rows[_currentButtonNum - 1].Cells[1].Value.ToString(), _currentButtonNum))); // switch to the UI thread in an async method
                                _playWakeFromSleep = false;
                                return;
                            }
                            else { await Task.Delay(1000).ConfigureAwait(false); }
                        }
                    }
                    catch { }
                }
                else { LogEvent("PowerMode_Changed: Resuming from a suspended state (nothing to do)"); }
                break;
        }
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == NativeMethods.WM_COPYDATA)
        {
            var copyData = Marshal.PtrToStructure<NativeMethods.COPYDATASTRUCT>(m.LParam);
            if (copyData.dwData == 2)
            {
                var arguments = Marshal.PtrToStringUni(copyData.lpData);
                if (!string.IsNullOrEmpty(arguments))
                {
                    var args = arguments.Split('|');
                    for (var i = 0; i < args.Length; i++)
                    {
                        if (Regex.IsMatch(args[i], @"^[/-][1-9][0-5]?$") && int.TryParse(args[i][1..], out var intStation))
                        {
                            var btnName = "rbtn" + intStation.ToString("D2"); // Math.Abs nicht nötig wg. [1..]
                            var controls = tcMain.TabPages[0].Controls.Find(btnName, true);
                            if (controls.Length == 1 && controls[0] is RadioButton rb && rb.Enabled)
                            {
                                rb.Checked = true; // (controls[0] as RadioButton).Checked = true;  // löst StartPlaying aus (BtnReset_Click in RadioButton_CheckedChanged)
                                var index = miniPlayer.MpCmBxStations.FindStringExact(Utilities.StationLong(dgvStations.Rows[Convert.ToInt32(rb.Tag?.ToString()) - 1].Cells[0].Value.ToString()));
                                if (index >= 0 && miniPlayer.MpCmBxStations.SelectedIndex != index) { miniPlayer.MpCmBxStations.SelectedIndex = index; }
                            }
                        }
                        else if (Regex.IsMatch(args[i], @"^[/-](m|mini)$", RegexOptions.IgnoreCase))
                        {
                            ShowMiniPlayer();
                            Hide(); //ShowInTaskbar = false; verträgt sich nicht mit GlobalHotkey => zerstört Handle
                            tcMain.SelectedIndex = 0;
                        }
                        else if (Regex.IsMatch(args[i], @"^[/-](t|tray)$", RegexOptions.IgnoreCase))
                        {
                            Hide();
                            miniPlayer.Hide();
                        }
                        else if (Regex.IsMatch(args[i], @"^[/-](f|full)$", RegexOptions.IgnoreCase))
                        {
                            ShowFullPlayer();
                            miniPlayer.Hide();
                        }
                        else if (Regex.IsMatch(args[i], @"^[/-](playpause)$", RegexOptions.IgnoreCase))
                        {
                            if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING) { BASSChannelPause(); }
                            else if (_stream != 0 && Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PAUSED) { BASSChannelPlay(); }
                            else if (btnReset.Enabled) { BtnReset_Click(null!, null!); }
                        }
                        else if (Regex.IsMatch(args[i], @"^[/-](p|play)$", RegexOptions.IgnoreCase))
                        {
                            if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING) { return; }
                            else if (_stream != 0 && Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PAUSED) { BASSChannelPlay(); }
                            else if (btnReset.Enabled) { BtnReset_Click(null!, null!); }
                        }
                        else if (Regex.IsMatch(args[i], @"^[/-](s|stop)$", RegexOptions.IgnoreCase))
                        {
                            if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING) { BASSChannelPause(); }
                        }
                        else if (Regex.IsMatch(args[i], @"^[/-](e|exit)$", RegexOptions.IgnoreCase))
                        {
                            SaveConfig();
                            Application.Exit();
                        }
                    }
                }
            }
        }
        else if (m.Msg == NativeMethods.WM_SHOWNETRADIO) { ShowFullPlayer(); } // another instance is started
        else if (m.Msg == NativeMethods.WM_HOTKEY)
        {
            var keyPressTick = Environment.TickCount;
            var elapsed = keyPressTick - lastHotkeyPress;
            lastHotkeyPress = keyPressTick;
            if (!mainShown) { return; } // andernfalls kann es bei 2maligem Drücken zu Anzeige des Hauptfensters und des Miniplayers kommen.
            if (elapsed <= 400 || (ModifierKeys & Keys.Shift) == Keys.Shift) { Close(); }
            else if (miniPlayer.Visible && !miniPlayer.Handle.Equals(NativeMethods.GetForegroundWindow())) { miniPlayer.Activate(); }
            else if (miniPlayer.Visible) { miniPlayer.Activate(); }
            else if (Visible) // heißt nicht, das Form sichtbar bzw. das aktive Fenster sein muss
            {
                if (ActiveForm == null) { Activate(); }
                else
                {
                    if (close2Tray) { miniPlayer.Hide(); }
                    else { ShowMiniPlayer(); }
                    Hide(); //ShowInTaskbar = false; verträgt sich nicht mit GlobalHotkey => zerstört Handle
                    tcMain.SelectedIndex = 0;
                }
            }
            else { ShowFullPlayer(); }  // not visible, d.h. im Tray
            LogEvent("WndProc: Message WM_HOTKEY received");
        }
        else if (m.Msg == NativeMethods.WM_MOUSEWHEEL && tcMain.SelectedTab == tpPlayer)
        {
            var delta = (int)m.WParam >> 16; // OverflowException!
            if (delta.GetType() == typeof(int) && delta != 0) { SetProgressBarVolume(delta); } //SetProgressBarVolume(m.WParam.ToInt32());
        }
        else if (m.Msg == NativeMethods.WM_QUERYENDSESSION) { Close(); }
        else if (m.Msg == NativeMethods.WM_NCLBUTTONDBLCLK) { Hide(); ShowMiniPlayer(); }
        else if (m.Msg == NativeMethods.WM_NCLBUTTONDOWN && tcMain.SelectedTab == tpStations) { dgvStations.EndEdit(); }
        else if ((m.Msg == NativeMethods.WM_SYSCOMMAND) && ((int)m.WParam == NativeMethods.IDM_CUSTOMITEM1))
        {
            SaveConfig();
            Application.Exit();
        }
        base.WndProc(ref m);
    }

    private void ShowMiniPlayer()
    {
        Application.DoEvents(); // für Autostart wichtig, damit GUI im fertigen Zustand angezeigt wird
        miniPlayer.Show();
        miniPlayer.TopMost = true; // make our form jump to the top of everything
        miniPlayer.TopMost = alwaysOnTop; // set it back to whatever it was
        miniPlayer.BringToFront();
        miniPlayer.Activate();
        LogEvent("ShowMiniPlayer: activated");
    }

    private void ShowFullPlayer()
    {
        if (!Visible)
        {
            Show();
            miniPlayer.Hide();
            if (_currentButtonNum > 0 && tcMain.TabPages[0].Controls["rbtn" + _currentButtonNum.ToString("D2")] is RadioButton rb) { rb.Focus(); }
        }
        else if (WindowState == FormWindowState.Minimized) { WindowState = FormWindowState.Normal; } // wahrscheinlich unnötig, kann nicht minimiert werden
        TopMost = true; // make our form jump to the top of everything
        TopMost = alwaysOnTop; // set it back to whatever it was
        BringToFront();
        Activate();
        LogEvent("ShowFullPlayer: activated");
    }

    private void RadioButton_CheckedChanged(object sender, EventArgs e)
    {// the event is fired twice because whenever one RadioButton within a group is checked another will be unchecked
        var rb = (RadioButton)sender; // sender as RadioButton;
        var oldId = _currentButtonNum;
        pbLevel.Image = null;
        miniPlayer.MpPBLevel.Image = null;
        timerLevel.Stop();
        spectrumTimer.Stop();
        timerResume.Stop();
        foreach (var vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
        Application.DoEvents();
        if (rb.Checked)
        {
            rb.ForeColor = Color.White;
            rb.BackColor = SystemColors.Highlight; //.HotTrack; //.ActiveBorder; //.InactiveCaption; //.ControlDark;
            _currentButtonNum = Convert.ToInt32(rb.Tag?.ToString());
        }
        else
        {
            rb.ForeColor = SystemColors.ControlText;
            rb.BackColor = Color.Transparent;
        }
        if (!firstEmptyStart && oldId != _currentButtonNum && _currentButtonNum > 0)
        {
            BtnReset_Click(null!, null!); //  StartPlaying(dgvStations.Rows[Convert.ToInt32(rb.Tag) - 1].Cells[1].Value.ToString()); } // Autostart
            btnReset.Enabled = true; // beim Programmstart deaktiviert
        }
    }

    private void BtnIncrease_MouseDown(object sender, MouseEventArgs e)
    {
        timerVolume.Enabled = true;
        timerVolume.Start();
    }
    private void BtnIncrease_MouseUp(object sender, MouseEventArgs e) => timerVolume.Stop();
    private void BtnDecrease_MouseDown(object sender, MouseEventArgs e)
    {
        timerVolume.Enabled = true;
        timerVolume.Start();
    }
    private void BtnDecrease_MouseUp(object sender, MouseEventArgs e) => timerVolume.Stop();

    private void SetMouseWheelValue(MouseEventArgs e) => SetProgressBarVolume(e.Delta); // MiniPlayer_VolumeMouseWheel

    private void SetProgressBarVolume(int delta)
    {
        var diff = (NativeMethods.GetKeyState(NativeMethods.VK_SHIFT) & 0x8000) == 0 ? 1 : 10;
        if (delta >= 0) { miniPlayer.MpVolProgBar.Value = volProgressBar.Value = volProgressBar.Value >= 100 - diff ? 100 : volProgressBar.Value + diff; }
        else { miniPlayer.MpVolProgBar.Value = volProgressBar.Value = volProgressBar.Value <= diff ? 0 : volProgressBar.Value - diff; }
        Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, volProgressBar.Value / 100f);
        lblVolume.Text = volProgressBar.Value.ToString();
        somethingToSave = true;
    }

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
        var relMouse = absMouse / clcFactor;
        var intMouse = Convert.ToInt32(relMouse);
        miniPlayer.MpVolProgBar.Value = volProgressBar.Value = intMouse > 100 ? 100 : intMouse < 0 ? 0 : intMouse;
        Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, volProgressBar.Value / 100f);
        lblVolume.Text = volProgressBar.Value.ToString();
        somethingToSave = true;
    }

    private void VolProgressBar_MouseDown(object sender, MouseEventArgs e)
    {
        SetProgressBarValue();
    }

    private void VolProgressBar_MouseMove(object sender, MouseEventArgs e)
    {
        if ((e.Button & MouseButtons.Left) == MouseButtons.Left) { SetProgressBarValue(); }
    }

    private void TimerVolume_Tick(object sender, EventArgs e)
    {
        if (btnIncrease.Focused) { BtnIncrease_Click(btnIncrease, EventArgs.Empty); }
        else if (btnDecrease.Focused) { BtnDecrease_Click(btnDecrease, EventArgs.Empty); }
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
                        var urlIsStillInFavorites = false;
                        for (var i = 0; i < stationSum; i++)
                        {
                            if (dgvStations.Rows[i].Cells[1].Value != null && info.filename != null && dgvStations.Rows[i].Cells[1].Value.ToString()!.Equals(info.filename, StringComparison.OrdinalIgnoreCase))
                            {
                                var controlName = "rbtn" + (i + 1).ToString("D2");
                                if (tcMain.TabPages[0].Controls[controlName] is RadioButton foundRadioButton)
                                {
                                    foundRadioButton.Checked = true;
                                    urlIsStillInFavorites = true;
                                }
                                UpdateCaption_lblD1(dgvStations.Rows[i].Cells[0].Value?.ToString() ?? string.Empty);
                                break;
                            }
                        }
                        if (!urlIsStillInFavorites) // || deviceChanged)
                        {
                            var iTag = 0;
                            Bass.BASS_ChannelStop(_stream); // wPlayer.controls.stop();

                            foreach (var rb in tcMain.TabPages[0].Controls.OfType<RadioButton>())
                            {
                                if (rb.Checked)
                                {
                                    var isValid = rb.Tag != null && int.TryParse(rb.Tag.ToString(), out iTag);
                                    if (!isValid || string.IsNullOrEmpty(dgvStations.Rows[iTag - 1].Cells[1].Value.ToString())) { rb.Checked = false; }
                                }
                                else { rb.Checked = false; }
                            }

                            if (iTag > 0) { StartPlaying(dgvStations.Rows[iTag - 1].Cells[1].Value.ToString(), iTag); }
                            else { RestorePlayerDefaults(); }
                        }
                    }
                }
                RewriteButtonText();
                somethingToSave = true; // => SaveConfig() => radioBtnChanged = false;
            }
            foreach (var rb in tcMain.TabPages[0].Controls.OfType<RadioButton>()) { if (rb.Checked) { rb.Focus(); } }
        }
        if (tcMain.SelectedIndex == 1)
        {// Stations
            UpdateStatusLabelStationsList();
            dgvStations.Focus(); // sonst funktioniert F2 nicht sogleich
        }
        else if (tcMain.SelectedIndex == 2)
        {
            TopMost = false; // Workaround, damit Tooltip in Listview im Vordergrund angezeigt wird
            loadHistoryBtn.Enabled = delAllHistoriesBtn.Enabled = Directory.GetFiles(Path.GetDirectoryName(xmlPath) ?? "", appName + "_*.csv").Length > 0;
            TPHistory_SetStatusBarText();
            historyLV.Focus();
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
            var statusLabelText = tcMain.SelectedIndex == 5 ? appPath : findNewStations;
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
        var fullRows = 0;
        foreach (DataGridViewRow row in dgvStations.Rows) { if (!Utilities.IsDGVRowEmpty(row)) { fullRows++; } }
        StatusStrip_SingleLabel(false, fullRows.ToString() + " entries");
    }

    private void BtnPlayStop_Click(object sender, EventArgs e)
    {
        timerResume.Stop();
        if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
        {
            BASSChannelPause();
            notifyIcon.Icon = Properties.Resources.NetRadiX;
        }
        else if (_stream != 0 && Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PAUSED)
        {
            BASSChannelPlay();
            notifyIcon.Icon = Properties.Resources.NetRadio;
        }
        else
        {
            BtnReset_Click(null!, EventArgs.Empty);
            notifyIcon.Icon = Properties.Resources.NetRadio;
        }
    }

    private void BASSChannelPause()
    {
        if (Bass.BASS_ChannelPause(_stream))
        {
            timerLevel.Stop();
            spectrumTimer.Stop();
            foreach (var vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
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

    private void BASSChannelPlay()
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
            NativeMethods.SHGetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0, IntPtr.Zero, out var downloadPath);
            if (lblD4.Text.Contains(downloadPath)) // Recording fand gerade statt, lblD4 enthält DownloadDateinamen
            {
                lblD4.ForeColor = SystemColors.ControlText;
                lblD4.Cursor = Cursors.Default;
                var info = Bass.BASS_ChannelGetInfo(_stream);
                if (info != null) { lblD4.Text = info.filename; }
                else { lblD4.Text = string.Empty; }
            }
        }
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
            MiniPlayer.MpLblD2_Text("NetRadio");
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
            foreach (var rb in tcMain.TabPages[0].Controls.OfType<RadioButton>()) // .Where(rb => rb != rbtn00)
            {
                if (rb.Checked)
                {
                    var isValid = int.TryParse(rb.Tag?.ToString(), out var iTag);
                    if (isValid && iTag > 0 && dgvStations?.Rows.Count >= iTag && dgvStations.Rows[iTag - 1].Cells[1].Value is string cellValue && cellValue.Length > 0)
                    {
                        UpdateCaption_lblD1(dgvStations.Rows[iTag - 1].Cells[0].Value.ToString() ?? string.Empty);
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
        catch (InvalidCastException ex) { Utilities.ErrTaskDialog(this, ex); }
    }

    private void UpdateCaption_lblD1(string caption) // BtnReset_Click | autoStartRadioButton | TcMain_SelectedIndexChanged | 
    {
        miniPlayer.MpCmBxStations.Text = string.IsNullOrEmpty(caption) ? "" : Utilities.StationLong(caption); // Regex.Replace(caption, @"\s+", " "); // doppelte Leerzeichen entfernen
        lblD1.Text = string.IsNullOrEmpty(caption) ? "" : Utilities.StationLong(caption, true); // Regex.Replace(caption, @"\s+", " "); // doppelte Leerzeichen entfernen
    }

    private void RewriteButtonText()
    {
        miniPlayer.MpCmBxStations.Items.Clear();
        if (dgvStations == null) { return; }
        for (var i = 1; i <= stationSum; i++)
        {
            var foundControls = tcMain.TabPages[0].Controls.Find($"rbtn{i:D2}", true); // Schutz vor IndexOutOfRange beim Button-Array
            if (foundControls.Length == 0) { continue; } // Button fehlt auf der Form -> ignorieren
            var foundBtn = (RadioButton)foundControls[0];
            var rowIndex = i - 1;
            if (rowIndex < dgvStations.Rows.Count && dgvStations.Rows[rowIndex].Cells[1].Value is string url && url.Length > 0)
            {
                var cellValue = dgvStations.Rows[rowIndex].Cells[0].Value;
                var stationName = cellValue?.ToString() ?? "-"; // Null-Coalescing
                foundBtn.Text = Utilities.StationShort(stationName, true);
                toolTip.SetToolTip(foundBtn, $"{Utilities.StationLong(stationName)} ({i})");
                miniPlayer.MpCmBxStations.Items.Add(Utilities.StationLong(stationName));
                foundBtn.Enabled = true;
            }
            else
            {
                foundBtn.Text = "-";
                foundBtn.Enabled = false;
            }
        }
        var index = miniPlayer.MpCmBxStations.FindStringExact(lblD1.Text.Replace("&&", "&"));
        if (index >= 0 && miniPlayer.MpCmBxStations.SelectedIndex != index) { miniPlayer.MpCmBxStations.SelectedIndex = index; }
    }

    //private void RewriteButtonText() // initial FrmMain_Load und dann TcMain_SelectedIndexChanged 
    //{
    //    miniPlayer.MpCmBxStations.Items.Clear();
    //    if (dgvStations == null || dgvStations.Rows.Count == 0)
    //    for (var i = 1; i <= stationSum; i++)
    //    {
    //        var foundBtn = (RadioButton)tcMain.TabPages[0].Controls.Find("rbtn" + i.ToString("D2"), true)[0]; // CAVE: Using führt dazu, dass Buttons von GUI verschwinden!
    //        if (dgvStations.Rows[i - 1].Cells[1].Value is string text && text.Length > 0) // column "URL"
    //        {
    //            if (dgvStations.Rows[i - 1].Cells[0].Value != null)
    //            {
    //                foundBtn.Text = Utilities.StationShort(dgvStations.Rows[i - 1].Cells[0].Value.ToString(), true); // true = button
    //                toolTip.SetToolTip(foundBtn, Utilities.StationLong(dgvStations.Rows[i - 1].Cells[0].Value.ToString()) + " (" + i + ")");
    //                miniPlayer.MpCmBxStations.Items.Add(Utilities.StationLong(dgvStations.Rows[i - 1].Cells[0].Value.ToString()));
    //            }
    //            foundBtn.Enabled = true;
    //        }
    //        else
    //        {
    //            foundBtn.Text = "-";
    //            foundBtn.Enabled = false;
    //        }
    //    }
    //    var index = miniPlayer.MpCmBxStations.FindStringExact(lblD1.Text.Replace("&&", "&"));
    //    if (index >= 0 && miniPlayer.MpCmBxStations.SelectedIndex != index) { miniPlayer.MpCmBxStations.SelectedIndex = index; } // erforderlich nach Veränderungen an dgvStations.Rows[i - 1].Cells[0]
    //}

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
        if (sender is RadioButton rb && rb.Checked)
        {
            var borderRectangle = rb.ClientRectangle;
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
        if (sender is DataGridView dGrid)
        {
            var rowText = (e.RowIndex + 1).ToString() + ". ";
            using var centerFormat = new StringFormat(StringFormatFlags.MeasureTrailingSpaces) // default: exclude the space at the end of each line
            {
                Alignment = StringAlignment.Far, // Bei einem Layout mit Ausrichtung von links nach rechts ist die weit entfernte Position rechts.
                LineAlignment = StringAlignment.Center // vertikale Ausrichtung der Zeichenfolge
            };
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, dGrid.RowHeadersWidth, e.RowBounds.Height);
            var rhForeColor = dgvStations.Rows[e.RowIndex].Index >= stationSum ? SystemColors.ControlLightLight : dGrid.RowHeadersDefaultCellStyle.ForeColor;
            using SolidBrush sBrush = new(rhForeColor);
            e.Graphics.DrawString(rowText, e.InheritedRowStyle.Font, sBrush, headerBounds, centerFormat);
        }
    }

    private void LinkPayPal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utilities.StartLink(this, "https://www.paypal.com/donate/?hosted_button_id=3HRQZCUW37BQ6");

    private void LinkHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utilities.StartLink(this, "https://www.netradio.info/app/");

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
    { // 
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
        List<string> devicelist = [];
        BASS_DEVICEINFO info; // = new BASS_DEVICEINFO();
        for (var n = 0; (info = Bass.BASS_GetDeviceInfo(n)) != null; n++) { if (info.IsEnabled) { devicelist.Add(info.ToString()); } }
        if (devicelist[0].Contains("No sound")) { devicelist.RemoveAt(0); } // 0: No sound
        if (devicelist.Count != 0) // if (!list.Any())
        {
            devicelist[0] += " (recommended)";
            cmbxOutput.Items.Clear();
            cmbxOutput.Items.AddRange([.. devicelist]);
            if (_stream != 0)
            {
                var device = Bass.BASS_ChannelGetDevice(_stream); // 0 = no sound, 1 = default
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
            strOutputDevice = cmbxOutput.Items[cmbxOutput.SelectedIndex]?.ToString() ?? string.Empty;
            strOutputDevice = strOutputDevice.StartsWith("Default") ? "Default" : strOutputDevice; // (recommended) entfernen
            if (prevOutputDevice != strOutputDevice) { somethingToSave = true; }
            LogEvent("CmbxOutput_SelectedIndexChanged: " + strOutputDevice + " (" + intOutputDevice + ") is selected");
            if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
            {
                var info = Bass.BASS_ChannelGetInfo(_stream);
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

    private void CmbxOutput_DropDownClosed(object sender, EventArgs e)
    {
        changeOutputDevice = true;
    }

    private void TPHistory_SetStatusBarText()
    {
        //string statusStripText = string.Empty;
        var count = historyLV.Items.Count;
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
        var statusStripText = string.Empty;
        BASS_DEVICEINFO info; // = new BASS_DEVICEINFO();
        for (var n = 1; (info = Bass.BASS_GetDeviceInfo(n)) != null; n++) // n = 1 => Default
        {
            if (n == intOutputDevice + 1) { statusStripText = "Current output: " + info.ToString() + (n == 1 ? " (adjusted by system settings, press F8)" : ""); break; } // info.IsInitialized funkt nicht
        }
        StatusStrip_SingleLabel(false, statusStripText);
    }

    private void CbAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAlwaysOnTop.Focused)
        {
            miniPlayer.TopMost = TopMost = alwaysOnTop = cbAlwaysOnTop.Checked;
            miniPlayer.MpBtnAOT.Image = alwaysOnTop ? Properties.Resources.pinpush : Properties.Resources.pinout;
            miniPlayer.MpBtnAOT.BackColor = alwaysOnTop ? Color.Maroon : SystemColors.ControlDark;
            somethingToSave = true;
            Activate();
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

        if (startMiniCmd || startTrayCmd)
        {
            Hide();
            Opacity = 1; // nach Hide //notifyIcon.ShowBalloonTip(1, Text, "Autostart", ToolTipIcon.Info);
            if (int.TryParse(autoStartRadioButton?.Tag?.ToString(), out var i) && i > 0 && dgvStations?.Rows.Count >= i && dgvStations.Rows[i - 1].Cells[0].Value is string captionText)
            {
                UpdateCaption_lblD1(captionText);
            }
        }
        if (alwaysOnTop) { miniPlayer.TopMost = TopMost = true; }
        if (startMiniCmd) { ShowMiniPlayer(); }
        Application.DoEvents();
        if (autoStartRadioButton != null)
        {
            autoStartRadioButton.Checked = true;
            autoStartRadioButton.Focus();
        }

        if (updateIndex == 0 && (DateTime.UtcNow - lastUpdateTime).TotalDays > 1 ||
            updateIndex == 1 && (DateTime.UtcNow - lastUpdateTime).TotalDays > 7 ||
            updateIndex == 2 && (DateTime.UtcNow - lastUpdateTime).TotalDays > 30)
        {
            BtnUpdate_Click(btnUpdate, EventArgs.Empty);
            if (updateAvailable)
            {
                ShowFullPlayer();
                tcMain.SelectedTab = tpInfo;

            }
            else { lblUpdate.Text = "Current version: " + strVersion; }
        }
        mainShown = true;

        if (tableActions != null && tableActions.Rows.Count > 0)
        {
            foreach (DataRow r in tableActions.Rows)
            {
                if (r.Field<string>("Task") == Utilities.TaskNames[6] && r.Field<bool>("Enabled") &&
                    (DateTime.TryParse(r.Field<string>("Time"), out var parsedTime) &&
                    (parsedTime - DateTime.Now > TimeSpan.Zero || repeatActionsDaily)))
                {
                    var btnCancel = TaskDialogButton.Continue;
                    TaskDialogButton btnAction = new TaskDialogCommandLinkButton("Check the settings");
                    TaskDialogPage taskDialogPage = new()
                    {
                        Icon = TaskDialogIcon.ShieldWarningYellowBar,
                        Caption = appName,
                        Heading = "Following task is active!",
                        Text = "The computer will shutdown at " + r.Field<string>("Time") + ".",
                        AllowCancel = true,
                        Buttons = { btnCancel, btnAction },
                        DefaultButton = btnCancel
                    };
                    if (TaskDialog.ShowDialog(this, taskDialogPage) == btnAction)
                    {
                        if (tcMain.SelectedTab != tpSettings) { tcMain.SelectedIndex = 3; } // Setting
                        BtnActions_Click(null!, null!);
                    }
                    break;
                }
            }
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
                            tcMain.SelectedIndex = 0;
                            if (close2Tray) { miniPlayer.Hide(); }
                            else { ShowMiniPlayer(); }
                            if (NativeMethods.IsKeyDown(Keys.Escape)) { miniPlayer.MpToolTip.Active = false; } // Workaround for persistent ToolTip display
                            else { miniPlayer.MpToolTip.Active = true; }
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
            case Keys.F1 | Keys.Control | Keys.Shift: { helpRequested = false; Utilities.StartFile(this, xmlPath); return true; }
            case Keys.F2 | Keys.Control | Keys.Shift: { Utilities.StartFile(this, logPath); return true; }
            case Keys.F4 | Keys.Control:
                {
                    if (Visible && NativeMethods.HitTest(Bounds, Handle, PointToScreen(Point.Empty))) { Hide(); } // "Tray-Modus"
                }
                return true;
            case Keys.F4 | Keys.Control | Keys.Shift: { Close(); return true; }
            case Keys.F2:
                {
                    if (tcMain.SelectedIndex == 0)
                    {
                        var rbi = 0;
                        foreach (var rb in tcMain.TabPages[0].Controls.OfType<RadioButton>())
                        {
                            if (rb.Checked) { rbi = Convert.ToInt32(rb.Tag) - 1; break; }
                        }
                        tcMain.SelectedIndex = 1;
                        dgvStations.Rows[rbi].Selected = true;
                        dgvStations.CurrentCell = dgvStations.Rows[rbi].Cells[0]; // wg. F2, öffnet sonst 1. Zeile
                    }
                    else if (tcMain.SelectedIndex >= 2)
                    {
                        tcMain.SelectedIndex = 1; // Stations
                        return true;
                    }
                    return false;
                }
            case Keys.F5:
                {
                    if (tcMain.SelectedTab != tpHistory)
                    {
                        tcMain.SelectedIndex = 2; // History
                    }
                    else if (tcMain.SelectedTab == tpHistory && historyLV.Items.Count > 0)
                    {
                        Utilities.SortHistoryNormal(historyLV, lviComparer, lvSortOrderArray);
                    }
                    return true;
                }
            case Keys.F4:
                {
                    if (tcMain.SelectedTab != tpPlayer)
                    {
                        tcMain.SelectedIndex = 0; // Player
                        return true;
                    }
                    return false;
                }
            case Keys.F8:
                {
                    if (tcMain.SelectedTab != tpSettings)
                    {
                        tcMain.SelectedIndex = 3; // Setting
                    }
                    else if (tcMain.SelectedTab == tpSettings)
                    {
                        ControlToolStripMenuItem_Click(null!, null!);
                    }
                    return true;
                }
            case Keys.F9:
                {
                    if (tcMain.SelectedTab != tpHelp)
                    {
                        tcMain.SelectedIndex = 4; // Spectrum
                    }
                    else if (tcMain.SelectedTab == tpHelp)
                    {
                        tcMain.SelectedIndex = 0; // Player
                    }
                    return true;
                }
            case Keys.F11:
                {
                    if (tcMain.SelectedTab != tpInfo)
                    {
                        tcMain.SelectedIndex = 5; // Information
                    }
                    else if (tcMain.SelectedTab == tpInfo)
                    {
                        tcMain.SelectedIndex = 0; // Player
                    }
                    return true;
                }
            case Keys.F12:
                {
                    if (tcMain.SelectedTab != tpSectrum)
                    {
                        tcMain.SelectedIndex = 6; // Spectrum
                    }
                    else if (tcMain.SelectedTab == tpSectrum)
                    {
                        tcMain.SelectedIndex = 0; // Player
                    }
                    return true;
                }
            case Keys.G | Keys.Control:
                {
                    if (tcMain.SelectedIndex == 0 || tcMain.SelectedIndex == 2)
                    {
                        GoogleToolStripMenuItem_Click(null!, null!);
                    }
                    return true;
                }
            case Keys.M | Keys.LWin:
            case Keys.M | Keys.RWin:
                {
                    if (miniPlayer.Visible) { miniPlayer.Hide(); }
                    else { Hide(); }
                    return true;
                }
            case Keys.F | Keys.Control:
            case Keys.F3:
                {
                    if (tcMain.SelectedIndex <= 1)
                    {
                        BtnSearch_Click(null!, null!);
                    }
                    return true;
                }
            case Keys.Oemplus:
                {
                    if (tcMain.SelectedIndex == 0) { btnIncrease.PerformClick(); btnIncrease.Focus(); return true; }
                    return true;
                }
            case Keys.OemMinus:
                {
                    if (tcMain.SelectedIndex == 0) { btnDecrease.PerformClick(); btnDecrease.Focus(); return true; }
                    return true;
                }
            case Keys.Add:
                {
                    if (tcMain.SelectedIndex == 0) { btnIncrease.PerformClick(); btnIncrease.Focus(); return true; }
                    return true;
                }
            case Keys.Subtract:
                {
                    if (tcMain.SelectedIndex == 0) { btnDecrease.PerformClick(); btnDecrease.Focus(); return true; }
                    return true;
                }
            case Keys.Back:
                {
                    if (tcMain.SelectedIndex == 0) { btnReset.PerformClick(); btnReset.Focus(); return true; }
                    return false;
                }
            case Keys.Insert:
                {
                    if (tcMain.SelectedIndex == 0) { btnRecord.PerformClick(); btnRecord.Focus(); return true; }
                    return false;
                }
            case Keys.D1: { if (tcMain.SelectedIndex == 0) { rbtn01.Checked = true; rbtn01.Focus(); return true; } return false; }
            case Keys.D2: { if (tcMain.SelectedIndex == 0) { rbtn02.Checked = true; rbtn02.Focus(); return true; } return false; }
            case Keys.D3: { if (tcMain.SelectedIndex == 0) { rbtn03.Checked = true; rbtn03.Focus(); return true; } return false; }
            case Keys.D4: { if (tcMain.SelectedIndex == 0) { rbtn04.Checked = true; rbtn04.Focus(); return true; } return false; }
            case Keys.D5: { if (tcMain.SelectedIndex == 0) { rbtn05.Checked = true; rbtn05.Focus(); return true; } return false; }
            case Keys.D6: { if (tcMain.SelectedIndex == 0) { rbtn06.Checked = true; rbtn06.Focus(); return true; } return false; }
            case Keys.D7: { if (tcMain.SelectedIndex == 0) { rbtn07.Checked = true; rbtn07.Focus(); return true; } return false; }
            case Keys.D8: { if (tcMain.SelectedIndex == 0) { rbtn08.Checked = true; rbtn08.Focus(); return true; } return false; }
            case Keys.D9: { if (tcMain.SelectedIndex == 0) { rbtn09.Checked = true; rbtn09.Focus(); return true; } return false; }
            case Keys.NumPad1: { if (tcMain.SelectedIndex == 0) { rbtn01.Checked = true; rbtn01.Focus(); return true; } return false; }
            case Keys.NumPad2: { if (tcMain.SelectedIndex == 0) { rbtn02.Checked = true; rbtn02.Focus(); return true; } return false; }
            case Keys.NumPad3: { if (tcMain.SelectedIndex == 0) { rbtn03.Checked = true; rbtn03.Focus(); return true; } return false; }
            case Keys.NumPad4: { if (tcMain.SelectedIndex == 0) { rbtn04.Checked = true; rbtn04.Focus(); return true; } return false; }
            case Keys.NumPad5: { if (tcMain.SelectedIndex == 0) { rbtn05.Checked = true; rbtn05.Focus(); return true; } return false; }
            case Keys.NumPad6: { if (tcMain.SelectedIndex == 0) { rbtn06.Checked = true; rbtn06.Focus(); return true; } return false; }
            case Keys.NumPad7: { if (tcMain.SelectedIndex == 0) { rbtn07.Checked = true; rbtn07.Focus(); return true; } return false; }
            case Keys.NumPad8: { if (tcMain.SelectedIndex == 0) { rbtn08.Checked = true; rbtn08.Focus(); return true; } return false; }
            case Keys.NumPad9: { if (tcMain.SelectedIndex == 0) { rbtn09.Checked = true; rbtn09.Focus(); return true; } return false; }
        }
        return base.ProcessCmdKey(ref msg, keyData);
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
        var pdfPath = Path.ChangeExtension(appPath, ".pdf"); // appPath muss als statische Variable verfügbar sein, da die Methode static ist
        if (File.Exists(pdfPath)) { Utilities.StartFile(null, pdfPath); }
        else
        {
            var (isYes, _, _) = Utilities.YesNo_TaskDialog(null, $"{Path.GetFileName(pdfPath)} was not found in the program directory.", "Would you like to download it from the Internet?");
            if (isYes)
            {
                try
                {
                    using (var fileStream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        using var response = await NetHttpClient.Instance.GetAsync("https://www.netradio.info/download/NetRadio.pdf");
                        response.EnsureSuccessStatusCode(); // Sicherstellen, dass der Download OK war (nicht 404)
                        await response.Content.CopyToAsync(fileStream); // // Da wir hier keine Progressbar haben, reicht CopyToAsync völlig aus.
                    }
                    ShowHelpPDF();
                }
                catch (Exception ex)
                {
                    var activeForm = ActiveForm ?? null;
                    Utilities.ErrTaskDialog(activeForm, ex);
                    if (File.Exists(pdfPath)) { try { File.Delete(pdfPath); } catch { } }
                }
            }
        }
    }

    private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
    {// Application.Exit() => e.CloseReason == CloseReason.ApplicationExitCall
        if (e.CloseReason == CloseReason.UserClosing && close2Tray && (ModifierKeys & Keys.Shift) == 0)
        {
            e.Cancel = true;
            Hide();
            return;
        }
        NativeMethods.UnregisterMediaKeys();
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
        //if (numUpDnSaveHistory.Value > 0) { SaveHistory(true); }
        SaveHistory(true);
    }

    private void SaveConfig() // FrmMain_FormClosing | TcMain_SelectedIndexChanged
    {
        var strVolume = ((int)(channelVolume * 100f)).ToString();
        XmlWriterSettings xwSettings = new()
        {
            IndentChars = "\t",
            NewLineHandling = NewLineHandling.Entitize,
            Indent = true,
            NewLineChars = "\n"
        };
        try
        {
            using var xw = XmlWriter.Create(xmlPath, xwSettings);
            xw.WriteStartDocument();
            xw.WriteStartElement("NetRadio");

            xw.WriteStartElement("Hotkey");
            xw.WriteAttributeString("Enabled", cbHotkey.Checked == true ? "1" : "0");
            xw.WriteAttributeString("Letter", hkLetter); // HIER FEHLT NOCH WAS
            xw.WriteEndElement(); // für HotkeynotifyIcon

            xw.WriteStartElement("Output");
            xw.WriteAttributeString("Device", strOutputDevice);
            xw.WriteEndElement(); // für Output

            xw.WriteStartElement("AlwaysOnTop");
            xw.WriteAttributeString("Enabled", alwaysOnTop == true ? "1" : "0");
            xw.WriteEndElement(); // für AlwaysOnTop

            xw.WriteStartElement("CloseToTray");
            xw.WriteAttributeString("Enabled", close2Tray == true ? "1" : "0");
            xw.WriteEndElement(); // für CloseToTray

            xw.WriteStartElement("BalloonTips");
            xw.WriteAttributeString("Enabled", showBalloonTip == true ? "1" : "0");
            xw.WriteEndElement(); // für MiniPlayer

            xw.WriteStartElement("LogHistory");
            xw.WriteAttributeString("Enabled", logHistory == true ? "1" : "0");
            xw.WriteEndElement(); // für LogHistory

            xw.WriteStartElement("ShowTrayInfo");
            xw.WriteAttributeString("Enabled", showTrayInfo == true ? "1" : "0");
            xw.WriteEndElement(); // für ShowTrayInfo

            xw.WriteStartElement("AutoStopRecording");
            xw.WriteAttributeString("Enabled", autoStopRecording == true ? "1" : "0");
            xw.WriteEndElement(); // für AutoStopRecording

            xw.WriteStartElement("Volume");
            xw.WriteAttributeString("Value", strVolume);
            xw.WriteEndElement(); // für Volume

            xw.WriteStartElement("SaveHistory");
            xw.WriteAttributeString("Value", Convert.ToInt32(numUpDnSaveHistory.Value).ToString());
            xw.WriteEndElement();

            xw.WriteStartElement("StartMode");
            xw.WriteAttributeString("Value", startMode.ToString());
            xw.WriteEndElement();

            xw.WriteStartElement("UpdateIndex");
            xw.WriteAttributeString("Value", updateIndex.ToString());
            xw.WriteEndElement(); // für UpdateIndex

            xw.WriteStartElement("UpdateSearch");
            xw.WriteAttributeString("DateTime", lastUpdateTime.ToString(longDateFormat, CultureInfo.InvariantCulture));
            xw.WriteEndElement(); // für UpdateSearch

            var formBounds = Bounds;
            xw.WriteStartElement("FormLocation"); // RestoreBounds.Location funktioniert nicht richtig
            xw.WriteAttributeString("PosX", formBounds.X.ToString());
            xw.WriteAttributeString("PosY", formBounds.Y.ToString());
            xw.WriteAttributeString("Width", formBounds.Width.ToString());
            xw.WriteAttributeString("Height", formBounds.Height.ToString());
            xw.WriteEndElement(); // für InitialLocation

            xw.WriteStartElement("MiniLocation"); // RestoreBounds.Location funktioniert nicht richtig
            xw.WriteAttributeString("PosX", miniPlayer.Location.X.ToString());
            xw.WriteAttributeString("PosY", miniPlayer.Location.Y.ToString());
            xw.WriteEndElement(); // für InitialLocation

            xw.WriteStartElement("Autostart");
            xw.WriteAttributeString("Station", autostartStation);
            xw.WriteEndElement(); // für Autostart

            xw.WriteStartElement("RepeatActionsDaily");
            xw.WriteAttributeString("Enabled", repeatActionsDaily == true ? "1" : "0");
            xw.WriteEndElement(); // für RepeatActionsDaily

            for (var i = 0; i < tableActions?.Rows.Count; ++i)
            {
                xw.WriteStartElement("Action");
                xw.WriteAttributeString("Enabled", tableActions.Rows[i].Field<bool>("Enabled").ToString());
                xw.WriteAttributeString("Task", tableActions.Rows[i].Field<string>("Task"));
                xw.WriteAttributeString("Station", tableActions.Rows[i].Field<string>("Station"));
                xw.WriteAttributeString("Time", tableActions.Rows[i].Field<string>("Time"));
                xw.WriteEndElement(); // für Action
            }

            for (var i = 0; i < dgvStations.RowCount; ++i)
            {
                xw.WriteStartElement("Station");
                var v = dgvStations.Rows[i].Cells[0].Value;
                xw.WriteAttributeString("Name", v != null ? dgvStations.Rows[i].Cells[0].Value.ToString() : "");
                v = dgvStations.Rows[i].Cells[1].Value;
                xw.WriteAttributeString("URL", v != null ? dgvStations.Rows[i].Cells[1].Value.ToString() : "");
                xw.WriteEndElement(); // für Radio
            }

            xw.WriteEndElement(); // für NetRadio
            xw.WriteEndDocument();
        }
        catch (ArgumentNullException ex) { Utilities.ErrTaskDialog(this, ex); }
        somethingToSave = false;
    }

    private void SaveHistory(bool deleteFiles = false) // LoadHistoryBtn_Click und FrmMain_FormClosing
    {
        Utilities.SortHistoryNormal(historyLV, lviComparer, lvSortOrderArray);
        var folderPath = Path.GetDirectoryName(xmlPath) ?? "";
        var filePath = Path.Combine(folderPath, appName + "_" + DateTime.Now.ToString(shortDateFormat) + ".csv");
        HistoryListView2CsvFile(filePath);
        if (deleteFiles)
        {
            var searchPattern = appName + "_*.csv";
            try
            {
                var filesToDelete = ((List<FileInfo>)[.. Directory.GetFiles(folderPath, searchPattern).Select(f => new FileInfo(f)).
                OrderByDescending(static f => f.LastWriteTime)]).Skip((int)numUpDnSaveHistory.Value).ToList();
                foreach (var file in filesToDelete) { file.Delete(); }
                loadHistoryBtn.Enabled = delAllHistoriesBtn.Enabled = Directory.GetFiles(folderPath, appName + "_*.csv").Length > 0;
            }
            catch (Exception ex) { Utilities.ErrTaskDialog(this, ex); }
            finally { deleteFiles = false; }
        }
    }

    public void HistoryListView2CsvFile(string filePath)
    {
        try
        {
            using StreamWriter sw = new(filePath, false, Encoding.UTF8);
            for (var i = 0; i < historyLV.Columns.Count; i++) // Spaltenüberschriften
            {
                sw.Write($"\"{historyLV.Columns[i].Text}\"");
                if (i < historyLV.Columns.Count - 1) { sw.Write(";"); }
            }
            sw.WriteLine();
            foreach (ListViewItem item in historyLV.Items) // Daten aus jedem ListViewItem
            {
                for (var i = 0; i < item.SubItems.Count; i++)
                {
                    if (i == 0 && item.Tag != null) { sw.Write($"\"{DateTime.ParseExact(item.Tag.ToString() ?? string.Empty, longDateFormat, CultureInfo.InvariantCulture).ToString(readDateFormat)}\""); }
                    else { sw.Write($"\"{item.SubItems[i].Text}\""); }
                    if (i < item.SubItems.Count - 1) { sw.Write(";"); }
                }
                sw.WriteLine();
            }
        }
        catch (Exception ex) { Utilities.ErrTaskDialog(this, ex); }
    }


    private void ShowToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (Visible)
        {
            if (NativeMethods.HitTest(Bounds, Handle, PointToScreen(Point.Empty)))
            {
                Hide();
                ShowMiniPlayer();
            }
            else { ShowFullPlayer(); }
        }
        else if (!Visible)
        {
            if (NativeMethods.HitTest(miniPlayer.Bounds, miniPlayer.Handle, miniPlayer.PointToScreen(Point.Empty))) { ShowFullPlayer(); tcMain.SelectedIndex = 0; }
            else
            {
                if (!miniPlayer.Visible && !Visible) { ShowFullPlayer(); tcMain.SelectedIndex = 0; }
                else { ShowMiniPlayer(); }
            }
        }
    }

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
    {
        SaveConfig();
        Application.Exit();
    }

    internal void BtnSearch_Click(object sender, EventArgs e)
    {
        if (tcMain.SelectedIndex != 1) // && tcMain.SelectedIndex != 0 &&
        {
            tcMain.SelectedIndex = 1;
            for (var row = 0; row < dgvStations.RowCount; row++)
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
            var dgvCellName = dgvStations.SelectedRows[0].Cells[0];
            var currRow = (dgvStations.SelectedRows[0].Index + 1).ToString();
            string currName;
            if (dgvCellName.Value != null && !string.IsNullOrEmpty(dgvCellName.Value.ToString())) { currName = Utilities.StationShort(dgvCellName.Value.ToString()); }
            else { currName = "[empty]"; }
            tcMain.SelectedIndex = 1; // Sendertabelle
            using FrmSearch frmSearch = new(currRow, currName);
            if (alwaysOnTop) { frmSearch.TopMost = true; }
            if (frmSearch.ShowDialog() == DialogResult.OK)
            {
                var searchString = frmSearch.TbString.Text;
                if (searchString.Length > 0)
                {
                    using FrmBrowser frmBrowser = new(searchString.Trim(), Location, curVersion);
                    if (alwaysOnTop) { frmBrowser.TopMost = true; }
                    var result = frmBrowser.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        currName = string.IsNullOrEmpty(currName) ? $"{dgvStations.SelectedRows[0].Index + 1}. row" : currName;

                        // Prüfen, ob bereits eine URL existiert
                        if (dgvStations.SelectedRows[0].Cells[1].Value != null && !string.IsNullOrEmpty(dgvStations.SelectedRows[0].Cells[1].Value.ToString()) &&
                            !Utilities.YesNo_TaskDialog(this, $"Overwrite {currName}?", "This entry already contains a URL. Do you want to replace it?").IsYes)
                        {
                            return;
                        }
                        dgvStations.SelectedRows[0].Cells[0].Value = frmBrowser.SelectedStation;
                        dgvStations.SelectedRows[0].Cells[1].Value = frmBrowser.SelectedURL;
                    }
                }
                if (firstEmptyStart)  // für ein schnelles Erfolgselebnis
                {
                    var url = dgvStations.Rows[0].Cells[1].Value.ToString();
                    if (currRow == "1" && !string.IsNullOrEmpty(url))
                    {
                        tcMain.SelectedIndex = 0; // nach StartPlaying 
                        BeginInvoke(() => { if (tcMain.TabPages[0].Controls["rbtn01"] is RadioButton rb) { rb.Checked = true; } }); // Workaround weil sonst timer nicht funktionieren =>  StartPlaying(url, 1);
                    }
                    firstEmptyStart = false;
                }
            }
        }
        else { Utilities.MsgTaskDialog(this, "Target not selected!"); }

    }

    private void BtnUp_Click(object sender, EventArgs e)
    {
        var idx = dgvStations.SelectedCells[0].OwningRow.Index;
        if (idx != 0)
        {
            var rows = dgvStations.Rows;
            var row = rows[idx];
            rows.Remove(row);
            rows.Insert(idx - 1, row);
            dgvStations.ClearSelection();
            dgvStations.CurrentCell = dgvStations.Rows[idx - 1].Cells[0];
            dgvStations.Rows[idx - 1].Selected = true;
        }
        dgvStations.Focus();
    }

    private void BtnDown_Click(object sender, EventArgs e)
    {
        var totalRows = dgvStations.Rows.Count;
        var idx = dgvStations.SelectedCells[0].OwningRow.Index;
        if (idx != totalRows - 1)
        {// int col = dgvStations.SelectedCells[0].OwningColumn.Index;
            var rows = dgvStations.Rows;
            var row = rows[idx];
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
        if (e.KeyCode == Keys.Insert) { AddToolStripMenuItem_Click(null!, null!); }
        else if (e.KeyCode == Keys.Delete) { DeleteToolStripMenuItem_Click(null!, null!); }
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
        else if (e.KeyCode == Keys.Up && e.Modifiers == Keys.Alt) { BtnUp_Click(null!, null!); e.Handled = true; }
        else if (e.KeyCode == Keys.Down && e.Modifiers == Keys.Alt) { BtnDown_Click(null!, null!); e.Handled = true; }
        else if (e.KeyCode == Keys.PageUp && e.Modifiers == Keys.Alt)
        {
            for (var j = 0; j < 8; j++)
            {
                BtnUp_Click(null!, null!);
                if (dgvStations.SelectedRows[0].Index < 1) { break; }
            }
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.PageDown && e.Modifiers == Keys.Alt)
        {
            for (var j = 0; j < 8; j++)
            {
                BtnDown_Click(null!, null!);
                if (dgvStations.SelectedRows[0].Index >= dgvStations.RowCount - 1) { break; }
            }
            e.Handled = true;
        }
    }

    private void KeyDown_MoveRowAt(int rowIndex, KeyEventArgs? kEA = null)
    {
        if (kEA != null) { kEA.Handled = true; kEA.SuppressKeyPress = true; }
        var idx = dgvStations.SelectedRows[0].Index;
        var rows = dgvStations.Rows;
        var row = rows[idx];
        dgvStations.Rows.RemoveAt(idx);
        dgvStations.Rows.Insert(rowIndex, row);
        dgvStations.Rows[rowIndex].Selected = true;
        dgvStations.CurrentCell = dgvStations.Rows[rowIndex].Cells[0]; // bewirkt Scroll
    }

    private void DgvStations_SelectionChanged(object sender, EventArgs e)
    {
        if (sender is DataGridView dgv)
        {
            var ri = -1;
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
                var dragSize = SystemInformation.DragSize;
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
        var sensitveSpace = 8;
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
    {
        var clientPoint = dgvStations.PointToClient(new Point(e.X, e.Y));
        rowIndexOfItemUnderMouseToDrop = dgvStations.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
        if (e.Effect == DragDropEffects.Move)
        {
            if (rowIndexOfItemUnderMouseToDrop < 0) { return; }
            if (e.Data is not null && e.Data.GetData(typeof(DataGridViewRow)) is DataGridViewRow rowToMove)
            {
                dgvStations.Rows.RemoveAt(rowIndexFromMouseDown);
                if (rowIndexFromMouseDown < rowIndexOfItemUnderMouseToDrop)
                {
                    dgvStations.Rows.Insert(rowIndexOfItemUnderMouseToDrop - 1, rowToMove);
                    rowIndexOfItemUnderMouseToDrop--;
                }
                else { dgvStations.Rows.Insert(rowIndexOfItemUnderMouseToDrop, rowToMove); }
                if (rowIndexOfItemUnderMouseToDrop > 16) { dgvStations.FirstDisplayedScrollingRowIndex += 1; }
                dgvStations.ClearSelection();
                dgvStations.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
                dgvStations.CurrentCell = dgvStations.Rows[rowIndexOfItemUnderMouseToDrop].Cells[0];
            }
        }
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (dgvStations.SelectedRows.Count > 0)
        {
            if (!Utilities.IsDGVRowEmpty(dgvStations.SelectedRows[0]))
            {
                using var dgvc = dgvStations.SelectedRows[0].Cells[0];
                string currName;
                if (dgvc.Value != null && !string.IsNullOrEmpty(dgvc.Value.ToString())) { currName = dgvc.Value.ToString()!; }
                else { currName = (dgvStations.SelectedRows[0].Index + 1) + ". row"; }
                if (!Utilities.YesNo_TaskDialog(this, $"Delete {currName}?", "Are you sure you want to delete this entry?").IsYes) { return; }
            }
            dgvStations.Rows.RemoveAt(dgvStations.SelectedRows[0].Index);
            dgvStations.Rows.Insert(dgvStations.Rows.Count); // -1 entfällt, weil eine Zeile gelöscht wurde!
            dgvStations.CurrentCell = dgvStations.Rows[dgvStations.SelectedRows[0].Index].Cells[0]; // scrollt! //dgvStations.FirstDisplayedScrollingRowIndex = dgvStations.SelectedRows[0].Index;
        }
    }

    private void SearchStationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        BtnSearch_Click(null!, null!);
    }

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var isAdded = false;
        for (var row = dgvStations.RowCount - 1; row >= dgvStations.SelectedRows[0].Index; row--)
        {
            if (Utilities.IsDGVRowEmpty(dgvStations.Rows[row]))
            {
                if (dgvStations.SelectedRows[0].Index != row)
                {
                    dgvStations.Rows.RemoveAt(row--); // deincrement (after the call) since we are removing the row
                    dgvStations.Rows.Insert(dgvStations.SelectedRows[0].Index);
                    dgvStations.Rows[dgvStations.SelectedRows[0].Index - 1].Selected = true;
                    dgvStations.CurrentCell = dgvStations.Rows[dgvStations.SelectedRows[0].Index].Cells[0]; // scrollt! 
                    isAdded = true;
                    break;
                }
            }
        }
        if (!isAdded) { Console.Beep(); } // MessageBox.Show("Sorry!"); }
    }

    private void Row1ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(0);
    }
    private void Row2ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(1);
    }
    private void Row3ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(2);
    }
    private void Row4ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(3);
    }
    private void Row5ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(4);
    }
    private void Row6ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(5);
    }
    private void Row7ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(6);
    }
    private void Row8ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(7);
    }
    private void Row9ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(8);
    }
    private void Row10ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(9);
    }
    private void Row11ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(10);
    }
    private void Row12ToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(11);
    }
    private void UpToolStripMenuItem_Click(object sender, EventArgs e)
    {
        BtnUp_Click(null!, null!);
    }
    private void DownToolStripMenuItem_Click(object sender, EventArgs e)
    {
        BtnDown_Click(null!, null!);
    }
    private void TopToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(0);
    }
    private void EndToolStripMenuItem_Click(object sender, EventArgs e)
    {
        KeyDown_MoveRowAt(dgvStations.RowCount - 1);
    }

    private void PgUpToolStripMenuItem_Click(object sender, EventArgs e)
    {
        for (var j = 0; j < 8; j++)
        {
            BtnUp_Click(null!, null!);
            if (dgvStations.SelectedRows[0].Index < 1) { break; }
        }
    }

    private void PgDnToolStripMenuItem_Click(object sender, EventArgs e)
    {
        for (var j = 0; j < 8; j++)
        {
            BtnDown_Click(null!, null!);
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

    private void PlayPauseToolStripMenuItem_Click(object sender, EventArgs e)
    {
        BtnPlayStop_Click(null!, null!);
    }

    private void CmbxStation_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbxStation.Visible && cmbxStation.Focused)
        {
            autostartStation = cmbxStation.Text;
            somethingToSave = true;
        }
        somethingToSave = true;
    }

    private void PicBoxPayPal_Click(object sender, EventArgs e) => Utilities.StartLink(this, "https://www.paypal.com/donate/?hosted_button_id=3HRQZCUW37BQ6");

    private void PicBoxPayPal_MouseEnter(object sender, EventArgs e)
    {
        picBoxPayPal.Cursor = Cursors.Hand;
    }
    private void PicBoxPayPal_MouseLeave(object sender, EventArgs e)
    {
        picBoxPayPal.Cursor = Cursors.Default;
    }

    private void EditStationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (sender is ToolStripMenuItem tsm && tsm.Owner != null)
        {
            using var rb = ((ContextMenuStrip)tsm.Owner).SourceControl as RadioButton;
            if (rb == null) { return; }
            tcMain.SelectedIndex = 1;
            var rbi = Convert.ToInt32(rb.Tag) - 1;
            dgvStations.Rows[rbi].Selected = true;
            dgvStations.CurrentCell = dgvStations.Rows[rbi].Cells[0]; // wg. F2, öffnet sonst 1. Zeile
        }
    }

    private void GoogleToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var search = string.Empty;
        if (ActiveControl != null && ActiveControl == historyLV)
        {
            if (historyLV.SelectedItems.Count > 0) { search = historyLV.Items[historyLV.SelectedIndices[0]].SubItems[2].Text; }
        }
        else if (!string.IsNullOrEmpty(lblD2.Text)) { search = lblD2.Text; }
        if (!string.IsNullOrEmpty(search)) { Utilities.StartLink(this, "https://www.google.com/search?q=" + System.Web.HttpUtility.UrlEncode(search.Trim())); }
    }

    private void CopyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
    {

        if (ActiveControl != null && ActiveControl == historyLV)
        {
            if (historyLV.SelectedItems.Count > 0)
            {
                var clip = historyLV.SelectedItems[0].SubItems[0].Text + " | " + historyLV.SelectedItems[0].SubItems[1].Text + " | " + historyLV.SelectedItems[0].SubItems[2].Text;
                if (!string.IsNullOrEmpty(clip)) { Utilities.SetClipboardUnicodeText(clip.TrimEnd(['|', ' '])); }
            }

        }
        else if (!string.IsNullOrEmpty(currentDisplayLabel?.Text)) { Utilities.SetClipboardUnicodeText(currentDisplayLabel.Text); }
    }

    private async void StartPlaying(string? _url, int tagID)
    {
        _playWakeFromSleep = false;

        // Netzwerkprüfung
        if (!Utilities.PingGoogleSuccess(Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT)))
        {
            Bass.BASS_StreamFree(_stream);
            RestorePlayerDefaults(tagID);

            TaskDialogButton btnSettings = new("Settings…");
            TaskDialogPage page = new()
            {
                Caption = appName,
                SizeToContent = true,
                Heading = "No Internet Connection!",
                Text = "Check the network connection status.",
                Icon = TaskDialogIcon.ShieldWarningYellowBar,
                Buttons = { btnSettings, TaskDialogButton.Close }
            };

            if (TaskDialog.ShowDialog(miniPlayer.Visible ? miniPlayer : this, page) == btnSettings)
            {
                Utilities.StartLink(this, "ms-settings:network-status");
            }
            return;
        }

        pbVolIcon.Image = Properties.Resources.progress;
        miniPlayer.MpVolProgBar.Value = volProgressBar.Value = 0;
        lblVolume.Text = "0";

        // --- LOGIK FÜR M3U AUFLÖSUNG START ---
        if (!string.IsNullOrEmpty(_url) && _url.EndsWith(".m3u", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var client = NetHttpClient.Instance;

                // Asynchron den Inhalt laden
                var newURL = await client.GetStringAsync(_url);

                // Parsen
                var urlString = Regex.Replace(newURL, @".*(http:\/\/[\S]+).*", "$1", RegexOptions.Singleline);
                var secString = Regex.Replace(newURL, @".*(https:\/\/[\S]+).*", "$1", RegexOptions.Singleline);

                // Priorisierung
                newURL = secString.StartsWith("https", StringComparison.OrdinalIgnoreCase) ? secString :
                         urlString.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? urlString : newURL;

                _url = string.IsNullOrEmpty(newURL) ? _url : newURL;
            }
            catch (Exception ex)
            {
                RestorePlayerDefaults(tagID);
                Utilities.ErrTaskDialog(this, ex);
                return;
            }
        }
        // --- LOGIK FÜR M3U AUFLÖSUNG ENDE --- 
        // HIER fehlte die geschweifte Klammer. Der folgende Code muss für ALLE URLs ausgeführt werden.

        lblD3.Text = "⌛Connecting...";
        MiniPlayer.MpLblD2_Text("⌛Connecting...");

        var windowHandle = Handle;
        // BASS Operationen im Hintergrund-Thread, damit die UI nicht einfriert
        await Task.Run(() =>
        {
            if (_stream != 0)
            {
                Bass.BASS_StreamFree(_stream);
                LogEvent("StartPlaying (_stream != 0): Freeing streams resources, including SYNC");
            }

            LogEvent("StartPlaying (BASS_Init): Output device no. " + intOutputDevice.ToString());

            // Initialisierung
            if (!Bass.BASS_Init(intOutputDevice <= 0 || intOutputDevice >= Bass.BASS_GetDeviceCount() ? -1 : intOutputDevice + 1, 44100, BASSInit.BASS_DEVICE_DEFAULT, windowHandle))
            {
                if (Bass.BASS_ErrorGetCode().Equals(BASSError.BASS_ERROR_ALREADY))
                {
                    LogEvent("StartPlaying (BASS_Init): The device has already been initialized");
                }
            }
            else
            {
                LogEvent("StartPlaying (BASS_Init): " + Bass.BASS_GetDeviceInfo(Bass.BASS_GetDevice()).ToString() + " (" + (Bass.BASS_GetDevice() - 1) + ") successfully (re)initialized");
            }

            var flag = BASSFlag.BASS_DEFAULT | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_STREAM_AUTOFREE;

            // Der Stream wird erstellt (dies dauert oft einen Moment wg. DNS/Connect)
            _stream = Bass.BASS_StreamCreateURL(_url, 0, flag, myStreamCreateURL, IntPtr.Zero);
        });

        // Zurück auf dem UI Thread (wegen await Task.Run)
        if (_stream == 0)
        {
            var errorDescription = Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode());
            RestorePlayerDefaults(tagID);
            Bass.BASS_Free();
            Utilities.MsgTaskDialog(this, "Stream creation failed", errorDescription);
            LogEvent("StartPlaying (BASS_StreamCreateURL): " + errorDescription);
            return;
        }

        _tagInfo = new TAG_INFO(_url);
        if (_tagInfo != null && BassTags.BASS_TAG_GetFromURL(_stream, _tagInfo))
        {
            lblD3.Text = _tagInfo.channelinfo.ToString().Replace("48000Hz", "48kHz").Replace("44100Hz", "44.1kHz").Replace("???, ", _tagInfo.channelinfo.ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG ? "FLAC, " : "");
            lblD3.Text = Regex.Replace(lblD3.Text, "([0-9])(kHz|bit)", "$1 $2");
            lblD3.Text += _tagInfo.bitrate != 0 ? ", " + _tagInfo.bitrate.ToString() + " kbit/s" : string.Empty;
            lblD3.Text += ", 00:00:00";

            if (_tagInfo.channelinfo.ctype == BASSChannelType.BASS_CTYPE_STREAM_MF &&
                Marshal.PtrToStructure<WAVEFORMATEX>(Bass.BASS_ChannelGetTags(_stream, BASSTag.BASS_TAG_WAVEFORMAT))?.wFormatTag == WAVEFormatTag.MPEG_HEAAC)
            {
                lblD3.Text = lblD3.Text.Replace("MF", "AAC");
            }

            lblD2.Text = _tagInfo.ToString().Replace("&", "&&");
            MiniPlayer.MpLblD2_Text(lblD2.Text);
            LogEvent("Channelinfo: " + _tagInfo.channelinfo.ctype + ")");
        }
        else
        {
            lblD3.Text = "00:00:00";
            MiniPlayer.MpLblD2_Text("NetRadio");
        }

        if (tcMain.SelectedTab == tpSectrum) { StatusStrip_SingleLabel(false, lblD2.Text); }
        if (logHistory) { AddToHistory(lblD2.Text); }

        // Syncs setzen
        _connectFail = new SYNCPROC(ConnectionSync);
        if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_DOWNLOAD | BASSSync.BASS_SYNC_ONETIME, 0, _connectFail, IntPtr.Zero) == 0)
        {
            Utilities.MsgTaskDialog(this, "Setting up a download synchronizer failed.", "", TaskDialogIcon.Warning);
        }

        _deviceFail = new SYNCPROC(DeviceSync);
        if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_DEV_FAIL | BASSSync.BASS_SYNC_ONETIME, 0, _deviceFail, IntPtr.Zero) == 0)
        {
            Utilities.MsgTaskDialog(this, "Setting up a device synchronizer failed.", "", TaskDialogIcon.Warning);
        }

        _metaSync = new SYNCPROC(MetaSync);
        if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_META, 0, _metaSync, IntPtr.Zero) == 0)
        {
            Utilities.MsgTaskDialog(this, "Setting up a meta synchronizer failed.", "", TaskDialogIcon.Warning);
        }

        // Playback starten
        Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, channelVolume);
        currPlayingTime = TimeSpan.Zero;
        _isBuffering = true;
        timerLevel.Start();
        spectrumTimer.Start();

        Bass.BASS_ChannelPlay(_stream, false);

        miniPlayer.MpBtnPlay.Enabled = btnPlayStop.Enabled = btnIncrease.Enabled = btnDecrease.Enabled = btnReset.Enabled = btnRecord.Enabled = true;

        // GUI Updates nach erfolgreichem Start
        BASS_CHANNELINFO info = new();
        if (tagID > 0 && Bass.BASS_ChannelGetInfo(_stream, info) && !string.IsNullOrEmpty(info.filename))
        {
            dgvStations.Rows[tagID - 1].Cells[1].Value = info.filename;
            if (lblD3.Text.Length <= 1)
            {
                lblD3.Text = Regex.Replace(info.ToString(), "[0-9]+Hz", ((double)info.freq / 1000).ToString() + "kHz").Replace("???, ", info.ctype == BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG ? "FLAC, " : "");
                lblD3.Text = Regex.Replace(lblD3.Text, "([0-9])(kHz|bit)", "$1 $2");
                lblD3.Text += ", 00:00:00";
            }
            if (lblD4.Text.EndsWith(" OK")) { lblD4.Text = info.filename; }
        }

        btnPlayStop.Image = Properties.Resources.pause_white;
        miniPlayer.MpBtnPlay.Image = Properties.Resources.pause_white;
        playPauseToolStripMenuItem.Text = "Pause";
        playPauseToolStripMenuItem.Image = Properties.Resources.pause;
    }


    private void BtnRecord_Click(object sender, EventArgs e)
    {
        if (_recording)
        {
            RecordingStop(false, Color.Blue); // false = !BASS_StreamFree; recording = false; // muss hier so früh wie möglich erfolgen
            timerLevel.Stop();
            spectrumTimer.Stop();
            foreach (var vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
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
        if (e.Button == MouseButtons.Left && sender is Label lbl && lbl.Text.Equals(_downloadFileName, StringComparison.OrdinalIgnoreCase))
        {
            if (File.Exists(_downloadFileName))
            {
                var dopusrt = @"C:\Program Files\GPSoftware\Directory Opus\dopusrt.exe";
                using Process process = new();
                process.StartInfo.UseShellExecute = false;
                if (File.Exists(dopusrt))
                {
                    process.StartInfo.FileName = dopusrt;
                    process.StartInfo.Arguments = $"/cmd Go \"{_downloadFileName}\"";
                    process.Start();
                }
                else
                {
                    process.StartInfo.FileName = "explorer.exe";
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.Arguments = $"/select,\"{_downloadFileName}\"";
                    process.Start();
                }
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
            var pt = e.Location;
            using var ctrl = tpPlayer.GetChildAtPoint(pt); // Controls die nicht enabled sind, zeigen kein Contextmenu an
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
            BtnSearch_Click(null!, null!);
        }
    }

    private void TimerLevel_Tick(object sender, EventArgs e)
    {
        var everySecond = false;
        var utcNowTicks = DateTime.UtcNow.Ticks; // bei ersten Aufruf ist accumulatedTicks 0
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
            var mainWidth = pbLevel.Height;
            var miniWidth = miniPlayer.MpPBLevel.Width;
            var level = Bass.BASS_ChannelGetLevel(_stream); // The level ranges linearly from 0 (silent) to 32768 (max)
            if (Visible) { DrawLevelMeter(Utils.LowWord32(level) * mainWidth / 32768, Utils.HighWord32(level) * mainWidth / 32768); }
            else if (miniPlayer.Visible) { MiniPlayer.DrawLevelMeter(Utils.LowWord32(level) * miniWidth / 32768, Utils.HighWord32(level) * miniWidth / 32768); }
            if (everySecond)
            {
                if (tcMain.SelectedTab == tpHistory) { TPHistory_SetStatusBarText(); }
                var seconds = currPlayingTime.ToString(@"hh\:mm\:ss");
                if (Regex.Match(lblD3.Text, @"\d{2}:\d{2}:\d{2}$").Success) { lblD3.Text = lblD3.Text[..^8] + seconds; }
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
            var buffProgress = Bass.BASS_StreamGetFilePosition(_stream, BASSStreamFilePosition.BASS_FILEPOS_DOWNLOAD) * (100f / _netPreBuff) / Bass.BASS_StreamGetFilePosition(_stream, BASSStreamFilePosition.BASS_FILEPOS_END);  // percentage of file downloaded
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
        foreach (var vp in tpSectrum.Controls.OfType<VerticalProgressBar>()) { vp.Value = 0; }
        if (currBtnNum == 0)
        {
            foreach (var rb in tcMain.TabPages[0].Controls.OfType<RadioButton>().Where(rb => rb.Checked)) { rb.Checked = false; } // cave: aändert currentButtonNum
            lblD1.Text = "-";
            miniPlayer.MpCmBxStations.Text = string.Empty;
            miniPlayer.MpBtnPlay.Enabled = btnPlayStop.Enabled = btnReset.Enabled = btnRecord.Enabled = false;
        }
        lblD2.Text = "-";
        MiniPlayer.MpLblD2_Text("NetRadio");
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

    private void TimerCloseFinally_Tick(object sender, EventArgs e)
    {
        timerCloseFinally.Stop();
        try { Process.Start(localSetupFile); } 
        catch (Exception ex) // when (ex is ArgumentNullException or InvalidOperationException or Win32Exception)
        {
            Utilities.ErrTaskDialog(this, ex);
            File.Delete(localSetupFile);
        }
        finally { Application.Exit(); }
    }

    private async void BtnUpdate_Click(object sender, EventArgs e)
    {
        if (updateAvailable)
        {
            if (Path.GetDirectoryName(xmlPath) != Path.GetDirectoryName(appPath)) // (Utilities.IsInnoSetupValid(Path.GetDirectoryName(appPath)))
            {
                try
                {
                    // Lokale Datei vorbereiten
                    NativeMethods.SHGetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0, IntPtr.Zero, out var targetDir); // Downloads folder    
                    if (!Directory.Exists(targetDir)) { targetDir = Path.GetTempPath(); }
                    localSetupFile = Path.Combine(targetDir, appName + "Setup.exe");
                    progressBar.Visible = true;
                    Progress<float> progress = new(p => { progressBar.Value = (int)p; });
                    // FileStream in einem Block kapseln, damit er sicher geschlossen ist, 
                    // bevor wir versuchen, die Datei auszuführen.
                    // Das 'using' schließt die Datei am Ende der geschweiften Klammern automatisch.
                    using (FileStream file = new(localSetupFile, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await NetHttpClient.Instance.DownloadDataAsync(downloadUpdateURL, file, progress);
                    }
                    timerCloseFinally.Start();  // Setup wird im Timer-Event gestartet, damit UI Zeit hat, sich zu aktualisieren
                }
                catch (Exception ex) // when (ex is InvalidOperationException or ArgumentNullException or WebException)
                {
                    btnUpdate.Enabled = true;
                    Utilities.ErrTaskDialog(this, ex);
                }
            }
            else { Utilities.StartLink(this, "https://www.netradio.info/app/"); }  // Portable version
        }
        else if (NativeMethods.InternetGetConnectedState(out var flags, 0))
        {
            var xmlURL = "https://www.netradio.info/download/netradio.xml";
            updateVersion = null;
            try
            {
                // NUTZUNG DER ZENTRALEN INSTANZ & ASYNC/AWAIT
                // Wir verwenden GetAsync mit await, damit die UI nicht blockiert.
                using var response = await NetHttpClient.Instance.GetAsync(xmlURL);
                // Prüfen, ob der Server "OK" (200) antwortet
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Inhalt asynchron als String lesen
                    var content = await response.Content.ReadAsStringAsync();

                    var doc = XDocument.Parse(content);
                    if (doc != null && doc.Element("netradio") is XElement x)
                    {
                        updateVersion = new Version(x.Element("version")?.Value ?? "0.0.0");
                        downloadUpdateURL = x.Element("url64")?.Value ?? string.Empty;
                        lastUpdateTime = DateTime.UtcNow;
                        somethingToSave = true;
                    }
                    else
                    {
                        Utilities.MsgTaskDialog(this, "No update information.", appName, TaskDialogIcon.Information);
                        return;
                    }
                }
                // Optional: Behandlung von nicht-OK Statuscodes, falls gewünscht
            }
            catch (Exception ex) // Fängt Netzwerk-, XML- und Argument-Fehler ab
            {
                Utilities.ErrTaskDialog(this, ex);
                return;
            }

            // Ab hier Logik wie gehabt (Versionsvergleich)
            if (updateVersion == null || updateVersion == new Version(0, 0, 0) || curVersion == null)
            {
                Utilities.MsgTaskDialog(this, "No update information.", "", TaskDialogIcon.Information);
            }
            else
            {
                if (updateVersion.CompareTo(curVersion) > 0)
                {
                    lblUpdate.Text = "Update available: v" + updateVersion.ToString();
                    btnUpdate.Text = "Download & Install";
                    updateAvailable = true;
                    btnUpdate.BackColor = SystemColors.MenuHighlight;
                    btnUpdate.ForeColor = SystemColors.Info;
                    btnUpdate.Invalidate();
                }
                else
                {
                    lblUpdate.Text = lblUpdate.Text.Equals("No update available") ? "Current version: " + strVersion : "No update available";
                }
            }
        }
        else { Utilities.MsgTaskDialog(this, "No internet connection.", "", TaskDialogIcon.ShieldWarningYellowBar); }
    }

    private void BtnUpdate_Paint(object sender, PaintEventArgs e)
    {
        if (updateAvailable && sender is Button btn)
        {
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.CompositingMode = CompositingMode.SourceOver;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            var borderRectangle = btn.ClientRectangle;
            borderRectangle.Inflate(-2, -2);
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
            ProcessStartInfo psi = new("mmsys.cpl") { UseShellExecute = true, WorkingDirectory = Environment.SystemDirectory };
            Process.Start(psi);
        }
        catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { Utilities.ErrTaskDialog(this, ex); }
    }

    private void FrmMain_Activated(object sender, EventArgs e) => TopMost = false;  // Workaround, damit Tooltip in Listview im Vordergrund angezeigt wird

    private void FrmMain_Deactivate(object sender, EventArgs e)
    {
        if (alwaysOnTop) { TopMost = true; }
    }

    private void ToolStripStatusLabel1_Click(object sender, EventArgs e)
    {
        if (toolStripStatusLabel.IsLink) { BtnSearch_Click(null!, null!); }
    }

    private void FrmMain_Move(object sender, EventArgs e) => somethingToSave = true;

    private void DrawLevelMeter(int left, int right)
    {
        if (left != levelLeft && right != levelRight)
        {
            levelLeft = left;
            levelRight = right;
            var height = pbLevel.Height - 1;
            Bitmap bm = new(pbLevel.ClientSize.Width, pbLevel.ClientSize.Height);
            using (var g = Graphics.FromImage(bm))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighSpeed;
                using Pen p = new(new LinearGradientBrush(new Point(0, -10), new Point(0, height), Color.Coral, SystemColors.Highlight), 5.0f);
                p.DashCap = DashCap.Round;
                g.DrawLine(p, 8, height, 8, height - levelRight);
                g.DrawLine(p, 1, height, 1, height - levelLeft);
            }
            pbLevel.Image = bm; // pictureBox2.Refresh(); bm.Dispose() führt zu Error
        }
    }

    private void HistoyClearButton_Click(object sender, EventArgs e)
    {
        historyLV.Items.Clear();
        historyExportButton.Enabled = histoyClearButton.Enabled = false;
        HistoryListView_SetDefaultColumnWidth();
        historyLV.Refresh();
        TPHistory_SetStatusBarText();
    }

    private void HistoryExportButton_Click(object sender, EventArgs e)
    {
        if (historyLV.Items.Count == 0) { return; }
        saveFileDialog.FileName = appName + "_" + DateTime.Now.ToString(shortDateFormat) + ".csv";
        if (saveFileDialog.ShowDialog() == DialogResult.OK)
        {
            Utilities.SortHistoryNormal(historyLV, lviComparer, lvSortOrderArray);
            HistoryListView2CsvFile(saveFileDialog.FileName);
            loadHistoryBtn.Enabled = delAllHistoriesBtn.Enabled = true;
        }
    }

    private void CbLogHistory_CheckedChanged(object sender, EventArgs e)
    {
        if (cbLogHistory.Focused)
        {
            if (cbLogHistory.Checked) { logHistory = true; }
            else
            {
                logHistory = false;
                numUpDnSaveHistory.Value = 0;
            }
            somethingToSave = true;
        }
    }

    private void HistoryListView_SetDefaultColumnWidth()
    {
        historyLV.Columns[0].Width = 60;
        historyLV.Columns[1].Width = 80;
        if (!NativeMethods.VerticalScrollbarVisible(historyLV)) { historyLV.Columns[2].Width = historyLV.Width - historyLV.Columns[0].Width - historyLV.Columns[1].Width - 4; }
        else { historyLV.Columns[2].Width = historyLV.Width - historyLV.Columns[0].Width - historyLV.Columns[1].Width - SystemInformation.VerticalScrollBarWidth - 5; }
    }

    private void HistoryListView_ColumnClick(object sender, ColumnClickEventArgs e)
    {
        if (historyLV.Items.Count > 0)
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
            historyLV.Refresh(); // Arrows auf anderen ColumnHeader-Buttons werden entfernt
            historyLV.Sort();
        }
    }

    private void ContextMenuDisplay_Opening(object sender, CancelEventArgs e)
    {
        if (ActiveControl != null && ActiveControl == historyLV && historyLV.SelectedItems.Count <= 0) { e.Cancel = true; }
        else if (ActiveControl != null && ActiveControl != historyLV) { tSMItemListViewDeleteEntry.Visible = tsSepListViewDeleteEntry.Visible = false; }
        else { tSMItemListViewDeleteEntry.Visible = tsSepListViewDeleteEntry.Visible = true; }
    }

    private void HistoryListView_MouseDoubleClick(object sender, MouseEventArgs e) => CopyToClipboardToolStripMenuItem_Click(null!, null!);


    private void HistoryListView_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
    {
        using (SolidBrush backBrush = new(SystemColors.ControlDark)) { e.Graphics.FillRectangle(backBrush, e.Bounds); }
        var rect = e.Bounds; // Do some padding, since these draws right up next to the border for Left/Near. Will need to change this if you use Right/Far
        rect.Inflate(0, -1);
        ControlPaint.DrawBorder3D(e.Graphics, rect, Border3DStyle.RaisedOuter);
        using SolidBrush foreBrush = new(Color.White);
        var font = e.Font ?? SystemFonts.DefaultFont;
        var headerText = e.Header?.Text ?? string.Empty;
        using var stringFormat = e.Header != null ? Utilities.GetStringFormat(e.Header.TextAlign) : new StringFormat();
        rect.X += MouseButtons == MouseButtons.Left && rect.Contains(historyLV.PointToClient(MousePosition)) ? 6 : 4;
        var sortArrow = lviComparer.SortColumn == e.ColumnIndex ? !string.IsNullOrEmpty(lvSortOrderArray[e.ColumnIndex]) && lvSortOrderArray[e.ColumnIndex].Equals("Ascending", StringComparison.Ordinal) ? " ↓" : " ↑" : ""; // ▲▼
        e.Graphics.DrawString(headerText + sortArrow, font, foreBrush, rect, stringFormat);
    }

    private void HistoryListView_DrawItem(object sender, DrawListViewItemEventArgs e) => e.DrawDefault = true;

    private void TpHistory_Leave(object sender, EventArgs e)
    {
        if (logHistory && historyLV.Items.Count > 0) { Utilities.SortHistoryNormal(historyLV, lviComparer, lvSortOrderArray); }
    }
    private void TSMItemListViewDeleteEntry_Click(object sender, EventArgs e)
    {
        var index = historyLV.Items.IndexOf(historyLV.SelectedItems[0]);
        historyLV.Items.RemoveAt(index);
        if (index > 0)
        {
            historyLV.Items[index - 1].Selected = true;
            historyLV.SelectedItems[0].Focused = true;
        }
    }

    private void HistoryListView_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            if (historyLV.FocusedItem != null)
            {
                historyLV.SelectedItems.Clear(); // nur 1 Eintrag soll bei Rechtsklick selected sein
                historyLV.Items[historyLV.FocusedItem.Index].Selected = true;
            }
        }
    }

    private void HistoryListView_KeyDown(object sender, KeyEventArgs e)
    {
        var focussedIndex = historyLV.FocusedItem != null ? historyLV.FocusedItem.Index : -1;
        var selectedCount = historyLV.SelectedItems.Count;
        if (e.KeyCode == Keys.Delete && focussedIndex >= 0)
        {
            if (selectedCount >= 1)
            {
                historyLV.BeginUpdate();
                var lastIndex = historyLV.Items.IndexOf(historyLV.SelectedItems[0]);
                for (var i = selectedCount - 1; i >= 0; i--) { historyLV.Items.RemoveAt(historyLV.SelectedIndices[i]); }
                if (historyLV.Items.Count > 0)
                {
                    historyLV.Items[lastIndex <= historyLV.Items.Count && lastIndex > 0 ? lastIndex - 1 : 0].Selected = true;
                    historyLV.SelectedItems[0].Focused = true;
                }
                else if (historyLV.Items.Count == 0) { histoyClearButton.Enabled = historyExportButton.Enabled = false; }
                historyLV.EndUpdate();
            }
            else { Console.Beep(); }
        }
        else if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
        {
            historyLV.BeginUpdate();
            foreach (ListViewItem item in historyLV.Items) { item.Selected = true; }
            historyLV.EndUpdate();
        }
    }

    private void BtnActions_Click(object sender, EventArgs e)
    {
        using FrmSchedule frmSchedules = new();
        if (alwaysOnTop) { frmSchedules.TopMost = true; }
        for (var i = 0; i < stationSum; i++)
        {
            if (dgvStations.Rows[i].Cells[0].Value != null && !string.IsNullOrEmpty(dgvStations.Rows[i].Cells[0].Value.ToString()))
            {
                frmSchedules.StationsList.Add(Utilities.StationShort(dgvStations.Rows[i].Cells[0].Value.ToString()));
            }
        }
        frmSchedules.ActionListView.Items.Clear();
        for (var j = 0; j < tableActions?.Rows.Count; j++)
        {
            frmSchedules.ActionListView.Items.Add(new ListViewItem(["", tableActions.Rows[j][1].ToString() ?? "", tableActions.Rows[j][2].ToString() ?? "", tableActions.Rows[j][3].ToString() ?? ""]));
            frmSchedules.ActionListView.Items[j].Checked = tableActions.Rows[j].Field<bool>("Enabled");
        }
        for (var l = frmSchedules.ActionListView.Items.Count; l < 9; l++) // mit Leerzeilen auffüllen - erspart Butte "Add" für neue Einträge
        {
            frmSchedules.ActionListView.Items.Add(new ListViewItem(["", "", "", ""]));
        }
        frmSchedules.ActionListView.Items[0].Selected = true;
        frmSchedules.RepeatActionsDaily.Checked = repeatActionsDaily && (tableActions?.AsEnumerable().Any(row => row.Field<bool>("Enabled")) ?? false);
        if (frmSchedules.ShowDialog() == DialogResult.OK)
        {
            StopActions(); // erst jetzt weil alle Zeilen in tableActions auf not enabled (False) gesetzt werden
            tableActions?.Rows.Clear();
            cbActions.Checked = false;
            repeatActionsDaily = frmSchedules.RepeatActionsDaily.Checked;
            foreach (ListViewItem item in frmSchedules.ActionListView.Items) //for (int i = 0; i < frmSchedules.ActionListView.Items.Count; i++)
            {
                var columns = frmSchedules.ActionListView.Columns.Count;
                var notEmpty = false;
                var cells = new object[columns];
                for (var j = 0; j < columns; j++)
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
                if (notEmpty) { tableActions?.Rows.Add(cells); }
            }
            somethingToSave = true;
            if (tableActions != null && tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled")))
            {
                cbActions.Checked = true;
                PrepareActions();
            }
        }
    }

    private void PrepareActions()
    {
        if (tableActions == null || tableActions.Rows.Count == 0) { return; }
        for (var i = 0; i < tableActions.Rows.Count; i++)
        {
            if (tableActions.Rows[i].Field<bool>("Enabled")) //) && rgxValidTime.Match(tableActions.Rows[i].Field<string>("Time")).Success)
            {

                var nowTime = DateTime.Now;
                var timeField = tableActions.Rows[i].Field<string>("Time");
                if (timeField != null)
                {
                    var jobHour = int.TryParse(timeField.Split(':').FirstOrDefault(), out var intH) ? intH : -1;
                    var jobMinu = int.TryParse(timeField.Split(':').LastOrDefault(), out var intM) ? intM : -1;
                    if (jobHour < 0 || jobMinu < 0)
                    {
                        Utilities.MsgTaskDialog(this, "Task #" + i + " is not executed because the time specification is incorrect.");
                        continue;
                    }
                    DateTime jobTime = new(nowTime.Year, nowTime.Month, nowTime.Day, jobHour, jobMinu, 0);
                    if (nowTime > jobTime) { jobTime = jobTime.AddDays(1); }
                    var tickTime = (int)(jobTime - nowTime).TotalMilliseconds; // double
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
    }

    private void OnTimedEvent(object? sender, EventArgs e)
    {
        if (sender == null || tableActions == null) { return; } // || tableActions.Rows.Count == 0
        var num = 0;
        ((Timer)sender).Stop();
        if (sender == timerAction1) { num = 0; }
        else if (sender == timerAction2) { num = 1; }
        else if (sender == timerAction3) { num = 2; }
        else if (sender == timerAction4) { num = 3; }
        else if (sender == timerAction5) { num = 4; }
        else if (sender == timerAction6) { num = 5; }
        else if (sender == timerAction7) { num = 6; }
        else if (sender == timerAction8) { num = 7; }
        else if (sender == timerAction9) { num = 8; }

        if (!repeatActionsDaily) { tableActions.Rows[num][0] = false; } // Aufgabe deaktivieren
        if (!tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled") == true)) { cbActions.Checked = false; }

        if (tcMain.SelectedIndex != 0) { tcMain.SelectedIndex = 0; }

        var tableAction = tableActions.Rows[num][1].ToString();
        if (!string.IsNullOrEmpty(tableAction))
        {
            if (tableAction.Equals(Utilities.TaskNames[0], StringComparison.Ordinal)) // "Start playing"
            {
                using var button = tcMain.TabPages[0].Controls.OfType<RadioButton>().FirstOrDefault(y => y.Text.Equals(tableActions.Rows[num][2].ToString(), StringComparison.Ordinal));
                if (button != null) { button.Checked = true; }
                else { Utilities.MsgTaskDialog(this, "Station not found."); }
            }
            else if (tableAction.Equals(Utilities.TaskNames[1])) // "Stop playing"
            {
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING) { BtnPlayStop_Click(null!, null!); }
            }
            else if (tableAction.Equals(Utilities.TaskNames[2])) // "Start recording"
            {
                if (!_recording)
                {
                    var button = tcMain.TabPages[0].Controls.OfType<RadioButton>().FirstOrDefault(y => y.Text.Equals(tableActions.Rows[num][2].ToString()));
                    if (button != null) { button.Checked = true; } // löst StartPlaying aus
                    BtnRecord_Click(null!, null!);
                }
            }
            else if (tableAction.Equals(Utilities.TaskNames[3])) // "Stop recording"
            {
                if (_recording) { btnRecord.PerformClick(); btnRecord.Focus(); } // RecordingStop();
            }
            else if (tableAction.Equals(Utilities.TaskNames[4])) // "Sleep Mode"
            {
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    BtnPlayStop_Click(null!, null!);
                    _playWakeFromSleep = true;
                }
                //if (NativeMethods.MessageBoxTimeout(Handle, $"The PC will go into sleep mode.", $"NetRadio - Task No. " + num + 1, 0x00000001 | 0x00010000 | 0x00000100 | 0x00000000, 0, 10000) == 2)
                //{
                //    if (_playWakeFromSleep) { BtnPlayStop_Click(null!, null!); }
                //    _playWakeFromSleep = false;
                //    return; // 2: Schaltfläche Cancel wurde ausgewählt
                //}
                if (Utilities.IsActionCancelled(this, $"NetRadio - Task No. {num + 1}", "The PC will go into sleep mode.", 10))
                {
                    if (_playWakeFromSleep) { BtnPlayStop_Click(null!, null!); }
                    _playWakeFromSleep = false;
                    return; // Die Methode gibt true zurück, wenn abgebrochen wurde -> return
                }
                Application.SetSuspendState(PowerState.Suspend, false, true);
            }
            else if (tableAction.Equals(Utilities.TaskNames[5])) // "Hibernate PC"
            {
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    BtnPlayStop_Click(null!, null!);
                    _playWakeFromSleep = true;
                }
                //if (NativeMethods.MessageBoxTimeout(Handle, $"The PC will go into hibernation mode.", $"NetRadio - Task No. " + num + 1, 0x00000001 | 0x00010000 | 0x00000100 | 0x00000040, 0, 10000) == 2)
                //{
                //    if (_playWakeFromSleep) { BtnPlayStop_Click(null!, null!); }
                //    _playWakeFromSleep = false;
                //    return; // 2 = Cancel
                //}
                if (Utilities.IsActionCancelled(this, $"NetRadio - Task No. {num + 1}", "The PC will go into hibernation mode.", 10))
                {
                    if (_playWakeFromSleep) { BtnPlayStop_Click(null!, null!); }
                    _playWakeFromSleep = false;
                    return; // Abbruch durch Benutzer (Klick auf Cancel)
                }
                Application.SetSuspendState(PowerState.Hibernate, false, true); //  put Windows into standby mode. Standby is forced and wake events are ignored:
            }
            else if (tableAction.Equals(Utilities.TaskNames[6])) // "Shut down PC"
            {
                if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
                {
                    BtnPlayStop_Click(null!, null!);
                    _playWakeFromSleep = true;
                }
                //if (NativeMethods.MessageBoxTimeout(Handle, $"The computer will shut down.", $"NetRadio - Task No. " + num + 1, 0x00000001 | 0x00010000 | 0x00000100 | 0x00000030, 0, 10000) == 2)
                //{
                //    if (_playWakeFromSleep) { BtnPlayStop_Click(null!, null!); }
                //    _playWakeFromSleep = false;
                //    return; // 2 = Cancel
                //}
                if (Utilities.IsActionCancelled(this, $"NetRadio - Task No. {num + 1}", "The computer will shut down.", 10, TaskDialogIcon.Warning))
                {
                    if (_playWakeFromSleep) { BtnPlayStop_Click(null!, null!); }
                    _playWakeFromSleep = false;
                    return; // Abbruch durch Benutzer
                }
                Process.Start(new ProcessStartInfo("shutdown", "/s /t 1")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
                Close(); // Application.Exit(); //  if (somethingToSave || radioBtnChanged) { SaveConfig(); }
            }
        }
    }

    private void CbActions_CheckedChanged(object sender, EventArgs e)
    {
        if (tableActions == null) { return; }
        if (!cbActions.Checked && cbActions == ActiveControl) { StopActions(); }
        else if (!tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled"))) { cbActions.Checked = false; }
    }

    private void StopActions()
    {
        if (tableActions == null) { return; }
        for (var i = 0; i < tableActions.Rows.Count; i++) { tableActions.Rows[i][0] = false; }
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

    private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utilities.StartLink(this, "https://github.com/ophthalmos/NetRadio");

    private void PbLevel_Click(object sender, EventArgs e)
    {
        if (timerLevel.Enabled) { tcMain.SelectedIndex = 6; }
    }

    private void LinkLabeGNU_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utilities.StartLink(this, "https://www.gnu.org/licenses/");

    private void DgvStations_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
    {
        strCellValue = dgvStations.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty; // DgvStations_CellValueChanged funktioniert unzuverlässig bzw. zu spät
        if (e.ColumnIndex == 0 && frmSplash == null)
        {
            frmSplash = new(this) { TopMost = true }; // using geht nur mit ShowDialog
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
    private void SplashForm_Activated(object? sender, EventArgs e) => dgvStations.EndEdit();

    private void DgvStations_MouseClick(object sender, MouseEventArgs e)
    {
        var hitInfo = dgvStations.HitTest(e.X, e.Y);
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
        using var g = CreateGraphics();
        if ((int)g.MeasureString(lblD4.Text, lblD4.Font, 0, StringFormat.GenericTypographic).Width > lblD4.Width)
        {
            if (toolTip.GetToolTip(lblD4) != lblD4.Text) { toolTip.SetToolTip(lblD4, lblD4.Text); }
        }
        else { toolTip.SetToolTip(lblD4, null); } // ToolTip zurücksetzen
    }

    private void LblD2_TextChanged(object sender, EventArgs e)
    {
        using var g = CreateGraphics();
        if ((int)g.MeasureString(lblD2.Text, lblD2.Font, 0, StringFormat.GenericTypographic).Width > lblD2.Width)
        {
            if (toolTip.GetToolTip(lblD2) != lblD2.Text) { toolTip.SetToolTip(lblD2, lblD2.Text); }
        }
        else { toolTip.SetToolTip(lblD2, null); } // ToolTip zurücksetzen
    }

    private void PanelLevel_Paint(object sender, PaintEventArgs e)
    {
        if (sender is Panel p && p.Parent != null)
        {
            LinearGradientBrush myBrush = new(p.PointToClient(p.Parent.PointToScreen(p.Location)), new Point(p.Width, p.Height), Color.AliceBlue, Color.LightSteelBlue);
            e.Graphics.FillRectangle(myBrush, ClientRectangle);
        }
    }

    private void CbClose2Tray_CheckedChanged(object sender, EventArgs e)
    {
        if (cbClose2Tray.Focused)
        {
            if (cbClose2Tray.Checked)
            {
                close2Tray = true;
                if (showTrayInfo)
                {
                    TaskDialogPage page = new()
                    {
                        Heading = "You still have the following options to exit:",
                        Text = "1. Right-click on the NetRadio icon in the system tray and Exit.\n\n2. Press the Shift key while clicking on the Close button [🗙].",
                        Caption = appName + " - Tray mode",
                        Icon = TaskDialogIcon.None,
                        AllowCancel = true,
                        Verification = new TaskDialogVerificationCheckBox() { Text = "Do not show again" },
                        Buttons = { TaskDialogButton.OK },
                        Footnote = new TaskDialogFootnote()
                        {
                            Text = "If the NetRadio icon is unvisible: Click on the ˄ arrow in the taskbar to show all icons and drag the icon to the system tray.\nIn this mode, pressing the Escape key in the main window minimizes the program to the taskbar.",
                        }
                    };
                    if (TaskDialog.ShowDialog(this, page) == TaskDialogButton.OK)
                    {
                        if (page.Verification.Checked) { showTrayInfo = false; }
                    }
                }
            }
            else { close2Tray = false; }
            somethingToSave = true;
        }
    }

    private void BtnUpdateSettings_Click(object sender, EventArgs e)
    {
        var prevUpdateIndex = updateIndex;
        TaskDialogPage pageUpdate = new()
        {
            Caption = appName,
            Heading = "Automatic Updates",
            Text = "You will be notified that an update is available to download.\n\nDetection frequency:",
            AllowCancel = true,
            SizeToContent = true,
            Buttons = { TaskDialogButton.OK, TaskDialogButton.Cancel },
        };
        var rbn0 = pageUpdate.RadioButtons.Add("Every day");
        var rbn1 = pageUpdate.RadioButtons.Add("Every week");
        var rbn2 = pageUpdate.RadioButtons.Add("Every month");
        var rbn3 = pageUpdate.RadioButtons.Add("Never");
        if (updateIndex == 1) { rbn1.Checked = true; }
        else if (updateIndex == 2) { rbn2.Checked = true; }
        else if (updateIndex == 3) { rbn3.Checked = true; }
        else { rbn0.Checked = true; }
        if (TaskDialog.ShowDialog(this, pageUpdate) == TaskDialogButton.OK)
        {
            updateIndex = rbn3.Checked ? 3 : rbn2.Checked ? 2 : rbn1.Checked ? 1 : 0;
            if (updateIndex != prevUpdateIndex) { somethingToSave = true; }
        }
    }

    private void RbStartMode_CheckedChanged(object sender, EventArgs e)
    {
        if (rbStartModeMain.Focused || rbStartModeMini.Focused || rbStartModeTray.Focused)
        {
            startMode = rbStartModeTray.Checked ? 2 : rbStartModeMini.Checked ? 1 : 0;
            somethingToSave = true;
        }
    }

    public void CreateLogFile()
    {
        try
        {
            using StreamWriter writer = new(logPath);
            writer.Write(""); // Datei leeren
        }
        catch { }
    }

    public void LogEvent(string message)
    {
        try
        {
            using StreamWriter writer = new(logPath, true, Encoding.UTF8); // Datei erstellen oder öffnen
            writer.WriteLine(DateTime.Now.ToString(shortDateFormat) + " | " + message); // Ereignisprotokollieren
            writer.Flush();
        }
        catch { }
    }

    private void LoadHistoryBtn_Click(object sender, EventArgs e)
    {
        //SaveHistory(); // Sortiert Liste normal (nach Datum)
        openFileDialog.InitialDirectory = Path.GetDirectoryName(xmlPath);
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                historyLV.Items.Clear();
                foreach (var line in File.ReadLines(openFileDialog.FileName).Skip(1)) // Alle Zeilen einlesen, erste (Header) überspringen
                {
                    var values = line.Split(';');
                    var time = DateTime.ParseExact(values[0].Trim('"'), readDateFormat, CultureInfo.InvariantCulture);
                    var item = new ListViewItem(time.ToString("HH:mm:ss"))
                    {
                        Tag = time.ToString(longDateFormat),
                        ToolTipText = values.Length > 1 ? values[1].Trim('"') : string.Empty
                    };
                    for (var i = 1; i < values.Length; i++) { item.SubItems.Add(values[i].Trim('"')); }
                    historyLV.Items.Add(item);
                }
                histoyClearButton.Enabled = historyExportButton.Enabled = historyLV.Items.Count > 0;

            }
            catch (Exception ex) { Utilities.ErrTaskDialog(this, ex); }
            finally { TPHistory_SetStatusBarText(); }
        }
    }

    private void DelAllHistoriesBtn_Click(object sender, EventArgs e)
    {
        try
        {
            var files = Directory.GetFiles(Path.GetDirectoryName(xmlPath) ?? "", appName + "_*.csv");
            if (files.Length > 0)
            {
                TaskDialogButton deleteButton = new("&Delete");
                var heading = "Do you want to delete " + (files.Length > 1 ? "these files?" : "this file?");
                if (TaskDialog.ShowDialog(this, new TaskDialogPage()
                {
                    Caption = appName,
                    Heading = heading,
                    Text = string.Join(Environment.NewLine, files),
                    Buttons = { TaskDialogButton.Cancel, deleteButton }
                }) == deleteButton)
                {
                    foreach (var file in files) { File.Delete(file); }
                    loadHistoryBtn.Enabled = delAllHistoriesBtn.Enabled = false;
                }
            }
        }
        catch (Exception ex) { Utilities.ErrTaskDialog(this, ex); }
    }

    private void NumUpDnSaveHistory_ValueChanged(object sender, EventArgs e)
    {
        if (numUpDnSaveHistory.Focused) { somethingToSave = true; }
        if (numUpDnSaveHistory.Value > 0) { cbLogHistory.Checked = logHistory = true; }
    }

    private void LinkLblUn4Seen_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utilities.StartLink(this, "https://www.un4seen.com/");

    private void LinkLblRadio42_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => Utilities.StartLink(this, "https://www.radio42.com/bass/");


    private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            if (!doubleClickOccurred) { timerNotifyIcon.Start(); }
        }
    }

    private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left)
        {
            doubleClickOccurred = true;
            timerNotifyIcon.Stop();
            ShowFullPlayer();
            tcMain.SelectedIndex = 0;
            doubleClickOccurred = false;
        }
    }

    private void TimerNotifyIcon_Tick(object sender, EventArgs e)
    {
        timerNotifyIcon.Stop();
        if (!doubleClickOccurred) { BtnPlayStop_Click(null!, EventArgs.Empty); }
    }
}

