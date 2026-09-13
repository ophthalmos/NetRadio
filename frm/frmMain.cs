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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;
using System.Xml.Linq;
using Microsoft.Win32;
using NetRadio.cls;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Tags;

namespace NetRadio;

public partial class FrmMain : Form
{
    [GeneratedRegex(@"^\d+\.\d+")]
    private static partial Regex VersionRegex();

    [GeneratedRegex("^[A-Z0-9]$")]
    private static partial Regex HotkeyRegex();

    [GeneratedRegex("^[A-Z]+$")]
    private static partial Regex HotkeyLettersRegex();

    [GeneratedRegex(@"^[/-][1-9][0-5]?$")]
    private static partial Regex CmdStationRegex();

    [GeneratedRegex(@"^[/-](m|mini)$", RegexOptions.IgnoreCase)]
    private static partial Regex CmdMiniRegex();

    [GeneratedRegex(@"^[/-](t|tray)$", RegexOptions.IgnoreCase)]
    private static partial Regex CmdTrayRegex();

    [GeneratedRegex(@"^[/-](l|language)$", RegexOptions.IgnoreCase)]
    private static partial Regex CmdLanguageRegex();

    [GeneratedRegex(@".*(http:\/\/[\S]+).*", RegexOptions.Singleline)]
    private static partial Regex HttpUrlRegex();

    [GeneratedRegex(@".*(https:\/\/[\S]+).*", RegexOptions.Singleline)]
    private static partial Regex HttpsUrlRegex();

    [GeneratedRegex("([0-9])(kHz|bit)")]
    private static partial Regex AudioFormatSpacingRegex();

    [GeneratedRegex("[0-9]+Hz")]
    private static partial Regex HzRegex();

    [GeneratedRegex(@"\d{2}:\d{2}:\d{2}$")]
    private static partial Regex TimeTrailingRegex();

    [GeneratedRegex(@"^-, ")]
    private static partial Regex LeadingDashCommaRegex();

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
    private SYNCPROC? _oggSync;
    private readonly int _hlsPlugIn = 0;
    private readonly int _opusPlugIn = 0;
    private readonly int _flacPlugIn = 0;
    private int _downlaodSize = 0;
    private int _currentButtonNum = 0;  // Wert wird in SelectStation() synchron gehalten mit _selectedStation.Number
    private readonly Version curVersion = Assembly.GetExecutingAssembly().GetName().Version ?? new Version("0.0.0");
    private readonly string strVersion = "unbekannt";
    private static bool startMiniCmd; // Miniplayer Command line
    private static bool startTrayCmd; // TrayModus Command line
    private bool mainShown;
    private static bool updateAvailable;
    private bool somethingToSave;
    private bool radioBtnChanged; // ersetzt auf Station-Tab nothingToSave
    private static readonly string appName = Application.ProductName ?? "NetRadio";
    private static readonly string appPath = Application.ExecutablePath; // EXE-Pfad
    private readonly string settingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, appName + ".settings.json");
    private readonly string stationsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, appName + ".stations.json");
    private readonly string logPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), appName, appName + ".log");
    private string hkLetter = string.Empty; // Flag für existierenden Hotkey. AUSNAHME: Programmstart
    private static int lastHotkeyPress;
    private int rowIndexFromMouseDown;
    private int colIndexFromMouseDown;
    private Rectangle dragBoxFromMouseDown;
    private int rowIndexOfItemUnderMouseToDrop;
    private int _dropIndicatorRowIndex = -1;  // Zeile, über der die Einfügelinie gezeichnet wird (-1 = keine)
    private bool firstEmptyStart = false;
    private readonly AppSettings _settings = new(); // geladene Einstellungen – Single Source of Truth für CloseToTray (auch vom MiniPlayer referenziert); Initializer schützt frühe WndProc-Aufrufe
    private float channelVolume = 1.0f;
    private readonly int stationSum = 25; // Rows = stationSum * 2
    private Control? currentDisplayLabel;
    private int levelLeft, levelRight;
    private int recIncrement = 0;
    private string localSetupFile = string.Empty;
    private string downloadUpdateURL = string.Empty;
    private int intOutputDevice = 0; //  0 = default => Init(-1)
    private string? prevOutputDevice;
    private bool changeOutputDevice = false;
    private readonly string findNewStations = "Press <Ctrl+F> to find new radio stations.";
    private static readonly string[] _languageCodes = ["en", "de", "es", "fr"]; // Reihenfolge = Items von cbUiLanguage
    private readonly bool _setupVersion; // false = Portable-Modus (Config neben der EXE statt in AppData)
    private bool helpRequested = true;
    private readonly MiniPlayer miniPlayer = new();
    private readonly string[] lvSortOrderArray = new string[3];
    private ListViewItem? lvItemHistory;
    private readonly CListViewItemComparer lviComparer = new();      // Sortierer für die ListView
    private readonly BASSTimer spectrumTimer = new(); // Creates a new Timer instance using a default interval of 50ms => 20 Hz.
    private TimeSpan currPlayingTime = TimeSpan.Zero;
    private TimeSpan totalPlayingTime = TimeSpan.Zero;
    private readonly float _netPreBuff;
    private bool _isBuffering = false;
    private bool _playWakeFromSleep = false;
    private long accumulatedTicks;
    private readonly DataTable? tableActions = new();
    private static readonly System.Windows.Forms.Timer timerAction1 = new();
    private static readonly System.Windows.Forms.Timer timerAction2 = new();
    private static readonly System.Windows.Forms.Timer timerAction3 = new();
    private static readonly System.Windows.Forms.Timer timerAction4 = new();
    private static readonly System.Windows.Forms.Timer timerAction5 = new();
    private static readonly System.Windows.Forms.Timer timerAction6 = new();
    private static readonly System.Windows.Forms.Timer timerAction7 = new();
    private static readonly System.Windows.Forms.Timer timerAction8 = new();
    private static readonly System.Windows.Forms.Timer timerAction9 = new();
    private SplashForm? frmSplash = null;
    private readonly string readDateFormat = "yyyy-MM-dd HH:mm:ss:fff"; // wird innerhalb der History-CSV-Dateien verwendet
    private readonly string longDateFormat = "yyyyMMddHHmmssfff";      // _settings.LastUpdateSearch, ListViewItem.Tag (CListViewItemComparer)
    private readonly string shortDateFormat = "yyyyMMdd-HHmmss";      // LogEvent, _downloadFileName, historyFile
    private Version? updateVersion = null;
    private bool doubleClickOccurred = false; // NotifyIcon
    private CancellationTokenSource? _startPlayingCts;
    private readonly RadioButton[] _stationButtons = new RadioButton[25]; // stationSum
    private Station? _selectedStation;  // Ersetzt die Kombination aus _currentButtonNum + checked RadioButton + ComboBox-Text.
    private bool _suppressStationEvents;  // Verhindert Event-Loops beim programmatischen Setzen von rb.Checked / cmBxStations.SelectedIndex.
    private readonly int _autoStartStationNumber; // 0 = kein Autostart // Ersetzt private readonly RadioButton? autoStartRadioButton.
    private readonly BindingList<StationRow> _stationData = new([.. Enumerable.Range(0, 100).Select(static _ => new StationRow())]) { AllowNew = false, AllowRemove = false, AllowEdit = true, };

    private ITaskbarList3? _taskbarList;
    private uint _taskbarButtonCreatedMsg;
    private const uint PLAY_BUTTON_ID = 101; // Beliebige eindeutige ID
    private readonly Icon _playIcon = Properties.Resources.NetRadio;
    private readonly Icon _pauseIcon = Properties.Resources.NetRadiX;

    public FrmMain()
    {
        InitializeComponent();
        // DataGridView hat kein öffentliches DoubleBuffered-Property → per Reflection aktivieren
        typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dgvStations, (object[])[true]);
        for (var i = 0; i < stationSum; i++)
        {
            var name = $"rbtn{i + 1:D2}";
            if (tcMain.TabPages[0].Controls[name] is RadioButton rb) { _stationButtons[i] = rb; }
        }
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
        if (curVersion is not null) { strVersion = string.Join('.', [curVersion.Major, curVersion.Minor, curVersion.Build >= 0 ? curVersion.Build : 0]); }
        LogEvent(appName + ": Version " + strVersion);
        LogEvent("IsUserAdmin: " + NativeMethods.IsUserAnAdmin());  //.IsUserAdminManaged()); 
        var cores = Environment.ProcessorCount;
        LogEvent("ProcessorCount: " + cores);
        Bass.BASS_SetConfigPtr(BASSConfig.BASS_CONFIG_NET_AGENT, _myUserAgentPtr);
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_BUFFER, 2000); // The buffer length in milliseconds (default 5000)
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PREBUF, 50); // Percentage of the download buffer length(BASS_CONFIG_NET_BUFFER) should be filled before starting playback. The default is 75 %
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_BUFFER, 2000); // The playback buffer length for HSTREAM and HMUSIC channels (default 500), maximum is 5000 milliseconds
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATEPERIOD, 50); // The update period of HSTREAM and HMUSIC channel playback buffers (default 100)
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_UPDATETHREADS, cores >= 4 ? 2 : 1); // The number of threads to use for updating playback buffers. Default is to use a single thread.
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT, 3000); // The default timeout is 5000 milliseconds 
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_NET_PLAYLIST, 1); // When enabled, BASS will process PLS, M3U, WPL and ASX playlists, going through each entry until it finds a URL that it can play. By default, playlist procesing is disabled.
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_DEV_DEFAULT, 1); // enable "Default" device
        Bass.BASS_SetConfig(BASSConfig.BASS_CONFIG_HLS_DOWNLOAD_TAGS, 1); // stream's DOWNLOADPROC callback function will receive any ID3v2 tags that the stream contains

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
        Text = $"{Assembly.GetCallingAssembly().GetName().Name} {VersionRegex().Match(strVersion).Value}";
        lblUpdate.Text = "Current version: " + strVersion;
        tableActions.Columns.Add("Enabled", typeof(bool));
        tableActions.Columns.Add("Task", typeof(string));
        tableActions.Columns.Add("Station", typeof(string));
        tableActions.Columns.Add("Time", typeof(string));
        _setupVersion = Utilities.IsInnoSetupValid(Path.GetDirectoryName(appPath) ?? string.Empty) // prüft auch Debugger.IsAttached
            || Environment.GetEnvironmentVariable("NETRADIO_SETUPMODE") == "1"; // vom Sprachwechsel-Neustart vererbt: Ohne Debugger fiele eine VS-Instanz sonst in den Portable-Modus und läse eine andere Config
        if (!_setupVersion) // Portable-Version
        {
            settingsPath = Path.ChangeExtension(appPath, ".settings.json");
            stationsPath = Path.ChangeExtension(appPath, ".stations.json");
            logPath = Path.ChangeExtension(appPath, ".log");
            CreateLogFile(); // logPath hat sich geändert - portable Log leeren/anlegen (.log.bak der Vorsitzung inklusive)
            LogEvent($"{appName}: Version {strVersion} (portable)");
            LogEvent("IsInnoSetupValid: Portable version");
        }
        else { LogEvent("IsInnoSetupValid: Setup version"); }

        Directory.CreateDirectory(Path.GetDirectoryName(settingsPath) ?? ""); // If the folder exists already, the line will be ignored.
        BackupDaily(settingsPath); // → NetRadio.settings.bak
        BackupDaily(stationsPath); // → NetRadio.stations.bak

        AppSettings? settings = null;
        List<StationEntry>? stationList = null;
        try
        {
            settings = JsonConfig.Load<AppSettings>(settingsPath);
            stationList = JsonConfig.Load<List<StationEntry>>(stationsPath);
        }
        catch (Exception ex) when (ex is JsonException or IOException)
        {
            LogEvent($"Config load FAILED: {ex.GetType().Name} - {ex.Message}");
            Utilities.ErrTaskDialog(null, ex);
        }

        if (settings is null && stationList is null)
        {
            // Einmalige Migration der alten NetRadio.xml (die XML-Datei bleibt als Sicherung liegen, wird aber nie wieder gelesen)
            var oldXmlPath = Path.Combine(Path.GetDirectoryName(settingsPath) ?? "", appName + ".xml");
            if (ConfigMigration.FromXml(oldXmlPath) is ({ } migratedSettings, { } migratedStations))
            {
                settings = migratedSettings;
                stationList = migratedStations;
                try
                {
                    JsonConfig.Save(settingsPath, settings);
                    JsonConfig.Save(stationsPath, stationList);
                    LogEvent("Config migrated from XML to JSON: " + oldXmlPath);
                }
                catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
                {
                    LogEvent($"Config migration save FAILED: {ex.GetType().Name} - {ex.Message}");
                    Utilities.ErrTaskDialog(null, ex);
                }
            }
            else
            {
                firstEmptyStart = true;
                LogEvent("New config: " + settingsPath);
            }
        }
        else { LogEvent($"Config loaded: settings={(settings is null ? "MISSING (defaults)" : "ok")}, stations={(stationList is null ? "MISSING" : $"{stationList.Count(static st => st.Name.Length > 0 || st.Url.Length > 0)}/{stationList.Count}")}"); } // gefüllte/gesamte Listeneinträge

        _settings = settings ?? new AppSettings();
        miniPlayer.Settings = _settings; // MiniPlayer liest daraus CloseToTray

        if (settings is null && GetSystemLanguage() is { } systemLanguage) // echter Erststart ohne settings.json: Windows-Anzeigesprache als Vorgabe, sonst bliebe es bei Englisch (z. B. wenn der Installer das Programm nicht selbst startet)
        {
            _settings.Language = systemLanguage;
            somethingToSave = true; // dauerhaft übernehmen
            LogEvent("System language: " + systemLanguage);
        }
        if (GetCmdLineLanguage() is { } cmdLanguage && cmdLanguage != _settings.Language) // "/l xx" bzw. "/language xx" — so übergibt z. B. der Installer seine Sprachauswahl an den ersten Start
        {
            _settings.Language = cmdLanguage;
            somethingToSave = true; // dauerhaft übernehmen
            LogEvent("Command line language: " + cmdLanguage);
        }
        Lng.Initialize(_settings.Language); // vor allen weiteren Dialogen und vor Lng.Apply
        Lng.Apply(this, toolTip); // übersetzt alle Designer-Texte samt ToolTips, falls nicht Englisch eingestellt ist
        Lng.Apply(contextMenuPlayer); // Kontextmenüs hängen nicht im Control-Baum
        Lng.Apply(contextMenuStations);
        Lng.Apply(contextMenuDisplay);
        Lng.Apply(contextMenuTrayIcon);
        miniPlayer.ApplyLanguage();
        saveFileDialog.Filter = Lng.T(saveFileDialog.Filter); // Dialog-Komponenten sind keine Controls, Lng.Apply erreicht sie nicht
        openFileDialog.Filter = Lng.T(openFileDialog.Filter);
        lblUpdate.Text = Lng.T("Current version:") + " " + strVersion; // die Zuweisung aus dem Konstruktoranfang lag vor der Sprachinitialisierung
        cbUiLanguage.SelectedIndex = Math.Max(0, Array.IndexOf(_languageCodes, _settings.Language));

        // Wird immer angewendet: Beim ersten Start (keine Config-Dateien) gelten damit die Defaults aus AppSettings
        cbHotkey.Checked = lblHotkey.Enabled = cmbxHotkey.Enabled = _settings.HotkeyEnabled;
        if (string.IsNullOrEmpty(_settings.HotkeyLetter)) { lblHotkey.Enabled = cmbxHotkey.Enabled = cbHotkey.Checked = false; }
        else if (cbHotkey.Checked && HotkeyRegex().IsMatch(_settings.HotkeyLetter)) { cmbxHotkey.Text = hkLetter = _settings.HotkeyLetter; } // You won't be able to register a hotkey before the window is created

        if (string.IsNullOrEmpty(_settings.OutputDevice)) { _settings.OutputDevice = "Default"; }
        cbClose2Tray.Checked = _settings.CloseToTray;
        cbShowBalloonTip.Checked = _settings.BalloonTips;
        miniPlayer.TopMost = cbAlwaysOnTop.Checked = _settings.AlwaysOnTop; // s. MiniPlayer_Shown-Event in frmMiniPlayer.cs
        cbLogHistory.Checked = _settings.LogHistory;
        cbAutoStopRecording.Checked = _settings.AutoStopRecording;

        var volume = Math.Clamp(_settings.Volume, 0, 100);
        channelVolume = volume / 100f;
        Bass.BASS_ChannelSetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, channelVolume);
        miniPlayer.MpVolProgBar.Value = volProgressBar.Value = volume;
        lblVolume.Text = volProgressBar.Value.ToString();

        numUpDnSaveHistory.Value = Math.Clamp(_settings.SaveHistory, (int)numUpDnSaveHistory.Minimum, (int)numUpDnSaveHistory.Maximum);
        if (_settings.StartMode == 0) { rbStartModeMain.Checked = true; }
        else if (_settings.StartMode == 1) { rbStartModeMini.Checked = true; }
        else if (_settings.StartMode == 2) { rbStartModeTray.Checked = true; }

        if (_settings.LastUpdateSearch == default) { _settings.LastUpdateSearch = DateTime.UtcNow; }
        if (_settings.AutostartStation > 0 && _settings.AutostartStation <= stationSum) { cmbxStation.Text = _settings.AutostartStation.ToString(); }
        foreach (var action in _settings.Actions)
        {
            if (action.Enabled) { cbActions.Checked = true; }
            tableActions.Rows.Add(action.Enabled, action.Task, action.Station, action.Time);
        }

        if (stationList is not null)
        {
            var legacyFormat = stationList.All(static st => st.Number == 0); // Altformat ohne "Number": Position = Stationsnummer
            for (var j = 0; j < stationList.Count; j++)
            {
                var idx = legacyFormat ? j : stationList[j].Number - 1;
                if (idx < 0 || idx >= _stationData.Count) { continue; }
                _stationData[idx].Name = stationList[j].Name;
                _stationData[idx].Url = stationList[j].Url;
            }
        }
        foreach (DataGridViewColumn column in dgvStations.Columns) { column.SortMode = DataGridViewColumnSortMode.NotSortable; }
        // DataPropertyName muss VOR DataSource gesetzt werden, damit keine Spalten auto-generiert werden
        dgvStations.AutoGenerateColumns = false;
        dgvStations.Columns[0].DataPropertyName = nameof(StationRow.Name);
        dgvStations.Columns[1].DataPropertyName = nameof(StationRow.Url);
        dgvStations.DataSource = _stationData;

        if (tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled"))) { cbActions.Checked = true; }

        if (!_settings.LogHistory) { numUpDnSaveHistory.Value = 0; } // numUpDnSaveHistory.Text = "0";

        string[] args = [.. Environment.GetCommandLineArgs().Skip(1)];
        if (args.Length > 0)
        {
            for (var i = 0; i < args.Length; i++) // Kommandozeilenargumente werden vor Autostart-Einstellungen benutzt
            {
                if (CmdStationRegex().IsMatch(args[i]) && int.TryParse(args[i][1..], out var intStation))
                {
                    if (intStation >= 1 && intStation <= stationSum) { _autoStartStationNumber = intStation; }
                }
                else if (CmdMiniRegex().IsMatch(args[i]))
                {
                    startMiniCmd = true; // siehe frmMain_Shown-Event
                    Opacity = 0; // sonst wird GUI kurz angezeigt - unschön
                }
                else if (CmdTrayRegex().IsMatch(args[i]))
                {
                    startTrayCmd = true; // siehe frmMain_Shown-Event
                    Opacity = 0; // sonst wird GUI kurz angezeigt - unschön
                }
            }
        }
        if (_settings.StartMode == 1 && !startTrayCmd && !startMiniCmd) // Miniplayer
        {
            startMiniCmd = true; // siehe frmMain_Shown-Event
            Opacity = 0; // sonst wird GUI kurz angezeigt - unschön
        }
        else if (_settings.StartMode == 2 && !startMiniCmd && !startTrayCmd) // tray mode
        {
            startTrayCmd = true; // siehe frmMain_Shown-Event
            Opacity = 0; // sonst wird GUI kurz angezeigt - unschön
        }
        if (_settings.AutostartStation >= 1 && _settings.AutostartStation <= stationSum &&
            string.IsNullOrEmpty(Environment.GetCommandLineArgs().Skip(1).FirstOrDefault())) { _autoStartStationNumber = _settings.AutostartStation; }   // kein Kommandozeilenargumente - dann Autostart-Einstellungen benutzen
        StatusStrip_SingleLabel(true, Lng.T(findNewStations));
        cbAutostart.Checked = Utilities.IsAutoStartEnabled(appName, "\"" + appPath + "\"" + " -min");
        historyLV.ListViewItemSorter = lviComparer;
        _stationData.ListChanged += StationData_ListChanged;
        spectrumTimer.Tick += SpectrumTick;
        spectrumTimer.Interval = 47; // ~21 fps; 3 × ~15,6 ms (System-Clock-Tick): feuert dadurch gleichmäßig, egal ob die Timer-Auflösung erhöht ist oder nicht
        _netPreBuff = Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_PREBUF) / 100f; // 0.75
        timerNotifyIcon.Interval = SystemInformation.DoubleClickTime;
    }

    private Station ReadStationFromDgv(int number) => _stationData[number - 1].ToStation(number);

    private void SelectStation(Station? station, bool forcePlay = false)
    {
        if (_suppressStationEvents) { return; }
        var isNewStation = station != _selectedStation;
        if (!isNewStation && !forcePlay) { return; }  // nichts zu tun
        _selectedStation = station;
        _currentButtonNum = station?.Number ?? 0;  // Abwärtskompatibilität für alle Lesestellen
        _suppressStationEvents = true;
        try
        {
            // ── 1. RadioButtons ───────────────────────────────────────────────
            for (var i = 0; i < stationSum; i++)
            {
                var rb = _stationButtons[i];
                var active = station?.Number == i + 1;
                rb.Checked = active;
                rb.ForeColor = active ? Color.White : SystemColors.ControlText;
                rb.BackColor = active ? SystemColors.Highlight : Color.Transparent;
            }

            // ── 2. Haupt-Labels ───────────────────────────────────────────────
            lblD1.Text = station?.LabelName ?? string.Empty;
            // lblD4 zeigt während des Streamaufbaus die URL – bleibt im StartPlaying-Flow

            // ── 3. MiniPlayer-ComboBox ────────────────────────────────────────
            SyncMiniPlayerComboBox(station);

            // ── 4. Level-/Spektrum-Timer zurücksetzen + Anzeige löschen ─────────
            if (isNewStation || forcePlay)
            {
                pbLevel.Image = miniPlayer.MpPBLevel.Image = null;
                timerLevel.Stop();
                spectrumTimer.Stop();
                timerResume.Stop();
                spectrumDisplay.Clear();
                // Song-Titel löschen – neue Metadaten kommen via MetaSync / UpdateTagDisplay
                lblD2.Text = "-";
                MiniPlayer.MpLblD2_Text("NetRadio");
                if (tcMain.SelectedTab == tpSectrum) { StatusStrip_SingleLabel(false, lblD2.Text); }
                Application.DoEvents();
            }

            // ── 5. Wiedergabe ─────────────────────────────────────────────────
            if ((isNewStation || forcePlay) && !firstEmptyStart && station is { IsValid: true })
            {
                // Stream freigeben wie in BtnReset_Click
                currPlayingTime = TimeSpan.Zero;
                playPauseToolStripMenuItem.Enabled = true;
                if (_stream != 0)
                {
                    Bass.BASS_ChannelGetAttribute(_stream, BASSAttribute.BASS_ATTRIB_VOL, ref channelVolume);
                    Bass.BASS_StreamFree(_stream);
                }
                StartPlaying(station.Url, station.Number);
            }
        }
        finally { _suppressStationEvents = false; }
    }

    private void SyncMiniPlayerComboBox(Station? station)  // Synchronisiert den MiniPlayer-ComboBox-SelectedIndex auf <paramref name="station"
    {
        var target = station?.LongName ?? string.Empty;
        var idx = miniPlayer.MpCmBxStations.FindStringExact(target);
        if (miniPlayer.MpCmBxStations.SelectedIndex != idx) { miniPlayer.MpCmBxStations.SelectedIndex = idx; }
    }



    private void MyDownloadProc(IntPtr buffer, int length, IntPtr user)
    {
        if (buffer == IntPtr.Zero)
        {
            // Stream-Ende oder Abbruch
            if (_recording)
            {
                BeginInvoke(() => RecordingStop());
            }
            return;
        }

        if (length == 0)
        {
            var txt = Marshal.PtrToStringUTF8(buffer) ?? string.Empty;  // PtrToStringUTF8
            // Moderner Lambda-Aufruf statt explizitem Delegate-Typ
            BeginInvoke(() => UpdateMessageDisplay(txt));
            return;
        }

        if (_recording)
        {
            try
            {
                if (_fs is null)
                {
                    var downloadPath = NativeMethods.GetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"));

                    var timestamp = DateTime.Now.ToString(shortDateFormat);
                    _downloadFileName = Path.Combine(downloadPath, $"{appName}_{timestamp}.mp3");

                    var info = Bass.BASS_ChannelGetInfo(_stream);
                    switch (info.ctype)
                    {
                        case BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG:
                        case BASSChannelType.BASS_CTYPE_STREAM_OPUS:
                        case BASSChannelType.BASS_CTYPE_STREAM_OGG:
                            _recording = false;
                            BeginInvoke(() =>
                            {
                                RecordingStop(false);
                                Utilities.MsgTaskDialogTimeout(this, Lng.T("Format not supported"), Lng.T("Recording is only available for MP3 and AAC streams."), 3, TaskDialogIcon.Information);
                            });
                            return;

                        case BASSChannelType.BASS_CTYPE_STREAM_MF:
                            var tags = Bass.BASS_ChannelGetTags(_stream, BASSTag.BASS_TAG_WAVEFORMAT);
                            if (tags != IntPtr.Zero && Marshal.PtrToStructure<WAVEFORMATEX>(tags)?.wFormatTag == WAVEFormatTag.MPEG_HEAAC)
                            {
                                _downloadFileName = Path.ChangeExtension(_downloadFileName, ".aac");
                            }
                            break;
                    }

                    _fs = new FileStream(_downloadFileName, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
                    _downlaodSize = 0;
                }

                // Puffer-Management
                if (_data == null || _data.Length < length)
                {
                    _data = new byte[length + 1024]; // Kleiner Puffer-Vorrat
                }

                Marshal.Copy(buffer, _data, 0, length);
                _fs.Write(_data, 0, length);
                _downlaodSize += length;

                // GUI-Update drosseln
                recIncrement++;
                if (recIncrement % 5 == 0) // Nur jedes 5. Mal spart CPU-Last
                {
                    var sizeText = Utilities.GetFileSize(_downlaodSize);
                    BeginInvoke(() => lblD4.Text = Lng.T("Downloading") + " " + sizeText);
                }
            }
            catch (IOException ex)
            {
                _recording = false;
                BeginInvoke(() =>
                {
                    RecordingStop();
                    Utilities.ErrTaskDialog(this, ex);
                });
            }
        }
    }
    private async void TimerResume_Tick(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(_channelFilename) && await Utilities.PingGoogleSuccessAsync(timerResume.Interval))  //_stream != 0 && 
        {
            timerResume.Enabled = false;
            StartPlaying(_channelFilename, _currentButtonNum);
            _channelFilename = string.Empty;
        }
        else if (_stream == 0) { timerResume.Enabled = false; }
        else
        {
            lblD3.Text = !lblD3.Text.EndsWith(Lng.T("Trying to reconnect...")) ? "◴" + Lng.T("Trying to reconnect...") : lblD3.Text;
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
            spectrumDisplay.Clear();
            if (Bass.BASS_ChannelIsActive(_stream) != BASSActive.BASS_ACTIVE_PLAYING) { lblD3.Text = "⌛" + Lng.T("Connecting..."); }
            MiniPlayer.MpLblD2_Text("⌛" + Lng.T("Connecting...")); //.Replace("ERROR:", "⚠"));
            btnPlayStop.Image = Properties.Resources.play_white;
            UpdateTaskbarIcon(false);
            miniPlayer.MpBtnPlay.Image = Properties.Resources.play_white;
            playPauseToolStripMenuItem.Text = Lng.T("Play"); // btnPlayStop.Text = 
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
            lblD3.Text = Lng.T("Output device is disconnected or disabled.");
            LogEvent("DeviceSync: Output device disconnected or disabled");
            Application.DoEvents(); // damit vorstehender Text angezeigt wird
            Thread.Sleep(1000); // andernfalls werden die gerade entfernten Devices als noch vorhanden angezeigt
            var devices = 0;
            BASS_DEVICEINFO dInfo;
            for (var n = 1; (dInfo = Bass.BASS_GetDeviceInfo(n)) != null; n++) { if (dInfo.IsEnabled) { devices++; } }
            if (devices > 0) // intOutputDevice wurde mit Wert 0 definiert
            {
                timerLevel.Stop();
                spectrumTimer.Stop();
                spectrumDisplay.Clear();
                intOutputDevice = 0;
                _settings.OutputDevice = "Default";
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
        if (data != 0) { BeginInvoke(() => UpdateStatusDisplay(Marshal.PtrToStringAnsi(new IntPtr(data)) ?? string.Empty)); }
        else
        {
            try
            {
                if (_tagInfo != null && _tagInfo.UpdateFromMETA(Bass.BASS_ChannelGetTags(channel, BASSTag.BASS_TAG_META | BASSTag.BASS_TAG_ID3V2), TAGINFOEncoding.Utf8OrLatin1, true))
                {
                    BeginInvoke(UpdateTagDisplay);
                }
            }
            catch (ArgumentOutOfRangeException) { }
        }
    }

    private void OggSync(int handle, int channel, int data, IntPtr user) // BASS_SYNC_OGG_CHANGE: neuer logischer Bitstream in einem OGG-Stream (Vorbis/Opus/FLAC-in-OGG) = neuer Titel
    {
        // OGG-Streams liefern keine ICY-Metadaten (BASS_SYNC_META bleibt stumm), sondern Vorbis-Kommentare je Bitstream;
        // BASS_TAG_GetFromURL liest diese (BASS_TAG_OGG) wie beim Start und aktualisiert _tagInfo
        try
        {
            if (_tagInfo != null && BassTags.BASS_TAG_GetFromURL(channel, _tagInfo)) { BeginInvoke(UpdateTagDisplay); }
        }
        catch (ArgumentOutOfRangeException) { }
    }

    private void UpdateStatusDisplay(string txt) => toolStripStatusLabel.Text = txt;

    private void UpdateTagDisplay()
    {
        if (_recording && _settings.AutoStopRecording)
        {
            btnRecord.PerformClick();
            btnRecord.Focus(); // RecordingStop();
            return;
        }
        if (_tagInfo != null)
        {
            lblD2.Text = _tagInfo.ToString().Replace("&", "&&"); // & wird sonst als Akzelerator interpretiert (nächstes Zeichen wird unterstrichen)
            MiniPlayer.MpLblD2_Text(lblD2.Text);
            if (tcMain.SelectedTab == tpSectrum) { StatusStrip_SingleLabel(false, lblD2.Text); }
            if (_settings.LogHistory) { AddToHistory(_tagInfo.ToString()); }
            lblD4.Text = _tagInfo.filename;
            if (_settings.BalloonTips)
            {
                var foregroundWin = NativeMethods.GetForegroundWindow();
                if (foregroundWin != miniPlayer.Handle && foregroundWin != Handle) { notifyIcon.ShowBalloonTip(2, Lng.T("Now playing: "), lblD2.Text, ToolTipIcon.Info); }
            }
            LogEvent("UpdateTagDisplay: " + _tagInfo.title);
        }
        else { lblD4.Text = dgvStations.Rows[_currentButtonNum - 1].Cells[1].Value?.ToString(); }
    }

    private void SpectrumTick(object? sender, EventArgs e)
    {
        if (tcMain.SelectedTab != tpSectrum) { return; }
        var fft = new float[1024];
        if (Bass.BASS_ChannelGetData(_stream, fft, (int)BASSData.BASS_DATA_FFT2048) < 0) { return; } // -1 = Fehler (z. B. kein Stream)
        spectrumDisplay.SetFft(fft, Bass.BASS_ChannelGetInfo(_stream)?.freq ?? 44100);
    }

    private void AddToHistory(string songTitle)
    {
        if (string.IsNullOrEmpty(songTitle)) { return; }
        var strArrHistory = new string[3]; // Lokale Instanzierung
        strArrHistory[0] = DateTime.Now.ToString("HH:mm:ss");
        strArrHistory[1] = _selectedStation?.ShortName ?? string.Empty;
        strArrHistory[2] = songTitle.Replace("&&", "&");
        lvItemHistory = new ListViewItem(strArrHistory)
        {
            Tag = DateTime.Now.ToString(longDateFormat),
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
        NativeMethods.AppendMenu(sysMenuHandle, NativeMethods.MF_BYPOSITION, NativeMethods.IDM_CUSTOMITEM1, Lng.T("Exit") + "\tShift+Esc");
        if (_settings.ExperimentalFeatures) { _taskbarButtonCreatedMsg = NativeMethods.RegisterWindowMessage("TaskbarButtonCreated"); }

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
            intOutputDevice = devicelist.IndexOf(_settings.OutputDevice); // in dieser Liste ist 0 = Default
            if (intOutputDevice == defaultDevice) { intOutputDevice = 0; } // Sieht wie in Bug aus, ist aber ein Feature, um wenn möglich "Default" zu erzwingen. BASS_GetDeviceInfo gibt niemals Default aus, sondern immer die höhere DeviceID
            if (intOutputDevice <= 0) { intOutputDevice = 0; } // 0 = Default
            _settings.OutputDevice = devicelist[intOutputDevice].ToString();
            LogEvent("FrmMain_Load: " + _settings.OutputDevice + " (" + intOutputDevice + ") is the current device");
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
        RefreshStationButtons();
        if (cbActions.Checked) { PrepareActions(); }
        var vScrollBar = dgvStations.Controls.OfType<VScrollBar>().FirstOrDefault();
        vScrollBar?.MouseCaptureChanged += (s, e) => { dgvStations.EndEdit(); };

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
        if (_settings.FormPosX is int fx && _settings.FormPosY is int fy && _settings.FormWidth is int fWidth && _settings.FormHeight is int fHeight)
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
        if (_settings.MiniPosX is int x_Pos && _settings.MiniPosY is int y_Pos)
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
            Bass.BASS_ChannelStop(_stream);
            Bass.BASS_Free();
            RestorePlayerDefaults(_selectedStation?.Number ?? 0);
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

    private void MiniPlayer_AppExit(object? sender, EventArgs e) =>
        Application.Exit(); // löst FrmMain_FormClosing mit CloseReason.ApplicationExitCall aus → SaveConfig/Cleanup dort

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
    private void MiniPlayer_IncreaseVolume(object? sender, EventArgs e) => BtnIncrease_Click(null!, EventArgs.Empty);
    private void MiniPlayer_DecreaseVolume(object? sender, EventArgs e) => BtnDecrease_Click(null!, EventArgs.Empty);
    private void MiniPlayer_F4_ShowPlayer(object? sender, EventArgs e)
    {
        ShowFullPlayer();
        tcMain.SelectedIndex = 0;
    }
    private void MiniPlayer_F5_ShowHistory(object? sender, EventArgs e)
    {
        ShowFullPlayer();
        tcMain.SelectedIndex = 2;
    }
    private void MiniPlayer_F12_ShowSpectrum(object? sender, EventArgs e)
    {
        ShowFullPlayer();
        tcMain.SelectedIndex = 6;
    }

    private void MiniPlayer_StationChanged(object? sender, EventArgs e)
    {
        if (_suppressStationEvents) { return; }
        var selectedText = miniPlayer.MpCmBxStations.Text;
        if (string.IsNullOrEmpty(selectedText)) { return; }
        for (var i = 0; i < stationSum; i++)
        {
            var station = ReadStationFromDgv(i + 1);
            if (station.IsValid && station.LongName == selectedText)
            {
                SelectStation(station);
                return;
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
                            if (await Utilities.PingGoogleSuccessAsync(Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT))) { foo = true; }
                            await Task.Delay(1000).ConfigureAwait(false);
                            if (foo) { break; }
                            else if (i == max && _selectedStation != null)
                            {
                                Invoke(new Action(() =>
                                {
                                    _suppressStationEvents = true;
                                    _stationButtons[_selectedStation.Number - 1].Checked = false;
                                    _suppressStationEvents = false;
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
                                RestartCurrentStationOnUiThread();
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

    private void RestartCurrentStationOnUiThread()
    {
        Invoke(() =>
        {
            if (_selectedStation is { IsValid: true } s) { StartPlaying(s.Url, s.Number); }
        });
    }

    private void InitializeTaskbarButtons()
    {
        try
        {
            var clsid = new Guid("56FDF344-FD6D-11d0-958A-006097C9A090");
            var iid = new Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf");
            _ = NativeMethods.CoCreateInstance(in clsid, nint.Zero, 1, in iid, out _taskbarList);  // Objekt via nativer API erstellen
            if (_taskbarList != null)
            {
                _taskbarList.HrInit();
                var isPlaying = Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING;
                var iconHandle = isPlaying ? _pauseIcon.Handle : _playIcon.Handle;
                var button = new THUMBBUTTON
                {
                    dwMask = THUMBBUTTONMASK.THB_ICON | THUMBBUTTONMASK.THB_TOOLTIP | THUMBBUTTONMASK.THB_FLAGS,
                    iId = PLAY_BUTTON_ID,
                    hIcon = iconHandle,
                    szTip = "Play/Pause",
                    dwFlags = THUMBBUTTONFLAGS.THBF_ENABLED
                };
                var buttons = new[] { button };
                _taskbarList.ThumbBarAddButtons(Handle, 1, buttons);
            }
        }
        catch (Exception ex) { Console.WriteLine($"Fehler beim Erstellen des Taskbar-Buttons: {ex.Message}"); }
    }

    public void UpdateTaskbarIcon(bool isPlaying)
    {
        if (_taskbarList != null)
        {
            var iconHandle = isPlaying ? _pauseIcon.Handle : _playIcon.Handle;
            var button = new THUMBBUTTON { dwMask = THUMBBUTTONMASK.THB_ICON, iId = PLAY_BUTTON_ID, hIcon = iconHandle };
            var buttons = new[] { button };
            _taskbarList.ThumbBarUpdateButtons(Handle, 1, buttons);
        }
    }

    protected override unsafe void WndProc(ref Message m)
    {
        if (m.Msg == NativeMethods.WM_SHOWNETRADIO)  // 1. Dynamisch vergebene Nachrichten (Laufzeit) müssen per 'if' geprüft werden
        {
            ShowFullPlayer();
            base.WndProc(ref m);
            return;
        }

        if (m.Msg == _taskbarButtonCreatedMsg) { InitializeTaskbarButtons(); }
        else if (m.Msg == NativeMethods.WM_COMMAND && _settings.ExperimentalFeatures)
        {
            var hiWord = (int)((m.WParam >> 16) & 0xFFFF);
            var loWord = (int)(m.WParam & 0xFFFF);
            if (hiWord == NativeMethods.THBN_CLICKED && loWord == PLAY_BUTTON_ID)
            {
                if (btnPlayStop.Enabled) { BtnPlayStop_Click(this, EventArgs.Empty); }
            }
        }

        switch ((uint)m.Msg)  // 2. Alle festen (konstanten) Nachrichten kommen in den schnellen 'switch'
        {
            case NativeMethods.WM_COPYDATA:
                {
                    var copyData = (NativeMethods.COPYDATASTRUCT*)m.LParam;
                    if (copyData->dwData == 2 && copyData->lpData != nint.Zero)
                    {
                        var argumentsSpan = MemoryMarshal.CreateReadOnlySpanFromNullTerminated((char*)copyData->lpData);
                        var remaining = argumentsSpan;
                        while (!remaining.IsEmpty)  // Allokationsfreies Splitten durch den Span
                        {
                            var idx = remaining.IndexOf('|');
                            var arg = idx == -1 ? remaining : remaining[..idx];
                            remaining = idx == -1 ? default : remaining[(idx + 1)..];
                            if (arg.Length < 2) { continue; }

                            var prefix = arg[0];
                            if (prefix == '/' || prefix == '-')
                            {
                                var cmd = arg[1..];
                                if (int.TryParse(cmd, out var intStation) && intStation is > 0)
                                {
                                    var btnName = "rbtn" + intStation.ToString("D2");
                                    var controls = tcMain.TabPages[0].Controls.Find(btnName, true);
                                    if (controls.Length == 1 && controls[0] is RadioButton rb && rb.Enabled)
                                    {
                                        rb.Checked = true;
                                        var tagValue = rb.Tag?.ToString();
                                        if (tagValue != null && int.TryParse(tagValue.ToString(), out var tagInt) && tagInt >= 1 && tagInt <= _stationData.Count)
                                        {
                                            var stationName = _stationData[tagInt - 1].Name;
                                            if (stationName != null)
                                            {
                                                var index = miniPlayer.MpCmBxStations.FindStringExact(Utilities.StationLong(stationName));
                                                if (index >= 0 && miniPlayer.MpCmBxStations.SelectedIndex != index) { miniPlayer.MpCmBxStations.SelectedIndex = index; }
                                            }
                                        }
                                    }
                                }
                                else if (cmd.Equals("m", StringComparison.OrdinalIgnoreCase) || cmd.Equals("mini", StringComparison.OrdinalIgnoreCase))
                                {
                                    ShowMiniPlayer();
                                    Hide();
                                    tcMain.SelectedIndex = 0;
                                }
                                else if (cmd.Equals("t", StringComparison.OrdinalIgnoreCase) || cmd.Equals("tray", StringComparison.OrdinalIgnoreCase))
                                {
                                    Hide();
                                    miniPlayer.Hide();
                                }
                                else if (cmd.Equals("f", StringComparison.OrdinalIgnoreCase) || cmd.Equals("full", StringComparison.OrdinalIgnoreCase))
                                {
                                    ShowFullPlayer();
                                    miniPlayer.Hide();
                                }
                                else if (cmd.Equals("playpause", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING) { BASSChannelPause(); }
                                    else if (_stream != 0 && Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PAUSED) { BASSChannelPlay(); }
                                    else if (btnReset.Enabled) { BtnReset_Click(null!, null!); }
                                }
                                else if (cmd.Equals("p", StringComparison.OrdinalIgnoreCase) || cmd.Equals("play", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING) { continue; }  // Anstelle von 'return', um eventuell weitere Argumente im Span zu verarbeiten
                                    else if (_stream != 0 && Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PAUSED) { BASSChannelPlay(); }
                                    else if (btnReset.Enabled) { BtnReset_Click(null!, null!); }
                                }
                                else if (cmd.Equals("s", StringComparison.OrdinalIgnoreCase) || cmd.Equals("stop", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING) { BASSChannelPause(); }
                                }
                                else if (cmd.Equals("e", StringComparison.OrdinalIgnoreCase) || cmd.Equals("exit", StringComparison.OrdinalIgnoreCase))
                                {
                                    Application.Exit(); // SaveConfig/Cleanup in FrmMain_FormClosing
                                }
                            }
                        }
                    }
                    break;
                }

            case NativeMethods.WM_HOTKEY:
                {
                    var keyPressTick = Environment.TickCount;
                    var elapsed = keyPressTick - lastHotkeyPress;
                    lastHotkeyPress = keyPressTick;
                    if (!mainShown) { break; }
                    if (elapsed <= 400)
                    {
                        Application.Exit(); // SaveConfig/Cleanup in FrmMain_FormClosing
                    }
                    else if (miniPlayer.Visible && miniPlayer.Handle.Equals(NativeMethods.GetForegroundWindow())) { ShowFullPlayer(); }
                    else if (miniPlayer.Visible) { miniPlayer.Activate(); }
                    else if (Visible)
                    {
                        if (ActiveForm == null) { Activate(); }
                        else
                        {
                            if ((ModifierKeys & Keys.Shift) == Keys.Shift)
                            {
                                Application.Exit();
                                break;
                            }
                            if (_settings.CloseToTray) { miniPlayer.Hide(); }
                            else { ShowMiniPlayer(); }
                            Hide();
                            tcMain.SelectedIndex = 0;
                        }
                    }
                    else { ShowFullPlayer(); }
                    LogEvent("WndProc: Message WM_HOTKEY received");
                    break;
                }

            case NativeMethods.WM_MOUSEWHEEL:
                {
                    if (tcMain.SelectedTab == tpPlayer)
                    {
                        var delta = (short)((long)m.WParam >> 16);
                        if (delta != 0) { SetProgressBarVolume(delta); }
                    }
                    break;
                }
            case NativeMethods.WM_NCLBUTTONDBLCLK:
                {
                    Hide();
                    ShowMiniPlayer();
                    break;
                }
            case NativeMethods.WM_NCLBUTTONDOWN:
                {
                    if (tcMain.SelectedTab == tpStations) { dgvStations.EndEdit(); }
                    break;
                }
            case NativeMethods.WM_SYSCOMMAND:
                {
                    if ((int)m.WParam == NativeMethods.IDM_CUSTOMITEM1)
                    {
                        Application.Exit(); // SaveConfig/Cleanup in FrmMain_FormClosing
                    }
                    break;
                }
        }
        base.WndProc(ref m);
    }

    private void ShowMiniPlayer()
    {
        Application.DoEvents(); // für Autostart wichtig, damit GUI im fertigen Zustand angezeigt wird
        miniPlayer.Show();
        miniPlayer.TopMost = true; // make our form jump to the top of everything
        miniPlayer.TopMost = _settings.AlwaysOnTop; // set it back to whatever it was
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
        TopMost = _settings.AlwaysOnTop; // set it back to whatever it was
        BringToFront();
        Activate();
        LogEvent("ShowFullPlayer: activated");
    }

    private void RadioButton_CheckedChanged(object sender, EventArgs e)
    {
        if (_suppressStationEvents) { return; }  // programmatisches Setzen → ignorieren
        var rb = (RadioButton)sender;
        if (!rb.Checked) { return; }  // "Abwählen"-Hälfte des Doppel-Fire → ignorieren
        var number = Convert.ToInt32(rb.Tag);
        var station = ReadStationFromDgv(number);
        SelectStation(station.IsValid ? station : null);
        btnReset.Enabled = true;
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
        if (tcMain.SelectedIndex == 0 && !Utilities.IsDGVEmpty(dgvStations)) { OnSwitchBackToPlayerTab(); }
        if (tcMain.SelectedIndex == 1)
        {// Stations
            UpdateStatusLabelStationsList();
            dgvStations.Focus(); // sonst funktioniert F2 nicht sogleich
        }
        else if (tcMain.SelectedIndex == 2)
        {
            TopMost = false; // Workaround, damit Tooltip in Listview im Vordergrund angezeigt wird
            loadHistoryBtn.Enabled = delAllHistoriesBtn.Enabled = Directory.GetFiles(Path.GetDirectoryName(settingsPath) ?? "", appName + "_*.csv").Length > 0;
            TPHistory_SetStatusBarText();
            historyLV.Focus();
        }
        else if (tcMain.SelectedIndex == 3)
        {
            CmbxOutput_CreateContent();
            TPSettings_SetStatusBarText();
        }
        else if (tcMain.SelectedIndex == 6) { StatusStrip_SingleLabel(false, lblD2.Text); }  // Spectrum
        else if (tcMain.SelectedIndex == 7) // Miniplayer
        {
            Hide(); //ShowInTaskbar = false; verträgt sich nicht mit GlobalHotkey => zerstört Handle
            ShowMiniPlayer();
            tcMain.SelectedIndex = 0;
        }
        else
        {
            var statusLabelText = tcMain.SelectedIndex == 5 ? appPath : Lng.T(findNewStations);
            StatusStrip_SingleLabel(!statusLabelText.Equals(appPath), statusLabelText);
        }
        if (somethingToSave) { SaveConfig(); }
    }

    private void OnSwitchBackToPlayerTab()
    {
        if (radioBtnChanged) // nur wenn Stationsdaten seit dem letzten Speichern geändert wurden - sonst ist nichts neu zu beschriften/speichern (F12-Wechsel bleibt dadurch reines Zeichnen)
        {
            RefreshStationButtons();              // Buttons aus aktualisiertem DGV neu beschriften
            somethingToSave = true;               // → SaveConfig() → radioBtnChanged = false
        }
        BASS_CHANNELINFO info = new();  // Prüfen, ob die aktuell spielende URL noch in den Favoriten ist
        if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING && Bass.BASS_ChannelGetInfo(_stream, info))
        {
            var playingUrl = info.filename;
            Station? matchingStation = null;
            for (var i = 0; i < stationSum; i++)
            {
                var s = ReadStationFromDgv(i + 1);
                if (s.IsValid && s.Url.Equals(playingUrl, StringComparison.OrdinalIgnoreCase))
                {
                    matchingStation = s;
                    break;
                }
            }
            if (matchingStation != null)  // URL noch vorhanden → nur UI-Sync, kein Neustart
            {
                _suppressStationEvents = true;
                _selectedStation = matchingStation;  // für Focus (siehe ein paar Zeilen weiter unten)
                try { SelectRadioButtonOnly(matchingStation); }
                finally { _suppressStationEvents = false; }
                UpdateCaption_lblD1(matchingStation.Name);
            }
            else  // URL nicht mehr vorhanden → aktuell ausgewählten Sender neu starten
            {
                Bass.BASS_ChannelStop(_stream);
                if (_selectedStation is { IsValid: true } sel) { SelectStation(ReadStationFromDgv(sel.Number), forcePlay: true); }
                else { RestorePlayerDefaults(); }
            }
        }
        else if (_selectedStation != null)  // Stream noch nicht aktiv (z. B. verbindet noch) → visuellen Zustand sicherstellen
        {
            _suppressStationEvents = true;
            try { SelectRadioButtonOnly(_selectedStation); }
            finally { _suppressStationEvents = false; }
        }
        if (_selectedStation != null) { _stationButtons[_selectedStation.Number - 1].Focus(); }  // Fokus auf aktiven RadioButton
        else { FocusStationButton(); } // auch ohne aktive Station soll immer ein Button den Fokus haben (Leertaste = Play)
    }

    private void FocusStationButton()  // Fokus auf aktiven (checked) Stations-Button, sonst Play-Button; Leertaste soll in jedem Fall Play auslösen (BtnReset-Fallback)
    {
        var rb = Array.Find(_stationButtons, static rb => rb is { Checked: true });
        if (rb is not null) { rb.Focus(); return; } // checked Button: OnEnter setzt Checked=true erneut, kein CheckedChanged
        if (btnPlayStop.Enabled) { btnPlayStop.Focus(); }
    }

    private void SelectRadioButtonOnly(Station station)
    {
        for (var i = 0; i < stationSum; i++)
        {
            var active = station.Number == i + 1;
            _stationButtons[i].Checked = active;
            _stationButtons[i].ForeColor = active ? Color.White : SystemColors.ControlText;
            _stationButtons[i].BackColor = active ? SystemColors.Highlight : Color.Transparent;
        }
    }

    private void StatusStrip_SingleLabel(bool isLink, string text)
    {
        toolStripStatusLabel.IsLink = isLink; // .Width = 388;
        toolStripStatusLabel.Text = text;
    }

    private void UpdateStatusLabelStationsList()
    {
        var fullRows = _stationData.Count(r => !r.IsEmpty);
        StatusStrip_SingleLabel(false, fullRows.ToString() + " " + Lng.T("entries"));
    }

    private void BtnPlayStop_Click(object sender, EventArgs e)
    {
        timerResume.Stop();
        if (Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PLAYING)
        {
            BASSChannelPause();
            notifyIcon.Icon = _pauseIcon;
        }
        else if (_stream != 0 && Bass.BASS_ChannelIsActive(_stream) == BASSActive.BASS_ACTIVE_PAUSED)
        {
            BASSChannelPlay();
            notifyIcon.Icon = _playIcon;
        }
        else
        {
            BtnReset_Click(null!, EventArgs.Empty);
            notifyIcon.Icon = _playIcon;
        }
    }

    private void BASSChannelPause()
    {
        if (Bass.BASS_ChannelPause(_stream))
        {
            timerLevel.Stop();
            spectrumTimer.Stop();
            spectrumDisplay.ClearBars(); // Pause: Balken ausblenden, Peak-Hold-Striche bleiben stehen (wie foobar2000)
            pbLevel.Image = null;
            miniPlayer.MpPBLevel.Image = null;
            btnPlayStop.Image = Properties.Resources.play_white;
            UpdateTaskbarIcon(false);
            miniPlayer.MpBtnPlay.Image = Properties.Resources.play_white;
            timerPause.Enabled = true;
            playPauseToolStripMenuItem.Text = Lng.T("Play"); // btnPlayStop.Text = 
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
            UpdateTaskbarIcon(true);
            miniPlayer.MpBtnPlay.Image = Properties.Resources.pause_white;
            timerLevel.Start();
            spectrumTimer.Start();
            timerPause.Enabled = false;
            playPauseToolStripMenuItem.Text = Lng.T("Pause"); // btnPlayStop.Text = 
            playPauseToolStripMenuItem.Image = Properties.Resources.pause;
            btnPlayStop.BackColor = SystemColors.ControlDark;
            miniPlayer.MpBtnPlay.BackColor = SystemColors.ControlDark;
            var downloadPath = NativeMethods.GetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B")); //   NativeMethods.SHGetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0, IntPtr.Zero, out var downloadPath);
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
        var station = _selectedStation ?? ScanForCheckedStation() ?? ScanForFirstValidStation();  // _selectedStation kann null sein (z.B. direkt nach Programmstart ohne Autostart-Station). Dann Fallback auf checked Button, sonst erste gültige Station.
        if (station is not { IsValid: true })  // Station nicht spielbar → DGV-Daten erneut prüfen (könnten geändert worden sein)
        {
            if (station != null) { station = ReadStationFromDgv(station.Number); }

            if (station is not { IsValid: true })
            {
                lblD1.Text = Lng.T("ERROR");
                lblD4.Text = Lng.T("No URL is defined.");
                return;
            }
        }
        station = ReadStationFromDgv(station.Number);  // Frisch aus DGV lesen – der Nutzer könnte URL auf dem Stations-Tab geändert haben
        SelectStation(station, forcePlay: true);
    }

    private Station? ScanForCheckedStation()
    {
        for (var i = 0; i < stationSum; i++)
        {
            if (_stationButtons[i].Checked) { return ReadStationFromDgv(i + 1); }
        }
        return null;
    }

    /// <summary>Liefert die erste gültige Station (für Play direkt nach Programmstart, wenn noch nichts checked ist).</summary>
    private Station? ScanForFirstValidStation()
    {
        for (var i = 1; i <= stationSum; i++)
        {
            var station = ReadStationFromDgv(i);
            if (station.IsValid) { return station; }
        }
        return null;
    }

    private void UpdateCaption_lblD1(string caption) // BtnReset_Click | autoStartRadioButton | TcMain_SelectedIndexChanged | 
    {
        miniPlayer.MpCmBxStations.Text = string.IsNullOrEmpty(caption) ? "" : Utilities.StationLong(caption); // Regex.Replace(caption, @"\s+", " "); // doppelte Leerzeichen entfernen
        lblD1.Text = string.IsNullOrEmpty(caption) ? "" : Utilities.StationLong(caption, true); // Regex.Replace(caption, @"\s+", " "); // doppelte Leerzeichen entfernen
    }

    private void RefreshStationButtons()
    {
        _suppressStationEvents = true;
        try
        {
            miniPlayer.MpCmBxStations.Items.Clear();
            for (var i = 0; i < stationSum; i++)
            {
                var rb = _stationButtons[i];
                var station = ReadStationFromDgv(i + 1);

                if (station.IsValid)
                {
                    rb.Text = station.ShortName;
                    rb.Enabled = true;
                    toolTip.SetToolTip(rb, $"{station.LongName} ({station.Number})");
                    miniPlayer.MpCmBxStations.Items.Add(station.LongName);
                }
                else
                {
                    rb.Text = "-";
                    rb.Enabled = false;
                    toolTip.SetToolTip(rb, string.Empty);
                }
            }
            SyncMiniPlayerComboBox(_selectedStation);  // Nach dem Neuaufbau der Items den aktuellen Sender wieder markieren

            // Player-Buttons aktivieren, sobald mindestens eine gültige Station existiert - sonst lässt sich
            // die Wiedergabe nach dem Programmstart (ohne Autostart-Station) nicht per Play-Button starten.
            if (!btnPlayStop.Enabled && Enumerable.Range(1, stationSum).Any(n => ReadStationFromDgv(n).IsValid))
            {
                miniPlayer.MpBtnPlay.Enabled = btnPlayStop.Enabled = btnIncrease.Enabled = btnDecrease.Enabled = btnReset.Enabled = btnRecord.Enabled = true;
            }
        }
        finally { _suppressStationEvents = false; }
    }

    private string GetStreamFormatName()  // Streamformatkurzname; ersetzt "???" in Kanalinfo (lblD3), wenn Bass.Net den Typ nicht kennt (z. B. OPUS, FLAC-in-OGG, HLS).
    {
        if (_stream == 0 || Bass.BASS_ChannelGetInfo(_stream) is not { } info) { return string.Empty; }
        return info.ctype switch
        {
            BASSChannelType.BASS_CTYPE_STREAM_MP3 => "MP3",
            BASSChannelType.BASS_CTYPE_STREAM_MP2 => "MP2",
            BASSChannelType.BASS_CTYPE_STREAM_MP1 => "MP1",
            BASSChannelType.BASS_CTYPE_STREAM_OGG => "OGG",
            BASSChannelType.BASS_CTYPE_STREAM_OPUS => "OPUS",
            BASSChannelType.BASS_CTYPE_STREAM_FLAC => "FLAC",
            BASSChannelType.BASS_CTYPE_STREAM_FLAC_OGG => "FLAC",
            _ when (int)info.ctype == 0x10300 => "HLS", // basshls (Enum-Name je nach Bass.Net-Version nicht vorhanden)
            _ => info.ctype.ToString().Replace("BASS_CTYPE_STREAM_", string.Empty),
        };
    }

    private void TpPlayer_MouseDown(object? sender, MouseEventArgs e)  // Klicks auf deaktivierte (leere) Stations-Buttons abfangen
    {
        if (e.Button != MouseButtons.Left || sender is not Control parent) { return; }
        if (parent.GetChildAtPoint(e.Location, GetChildAtPointSkip.None) is not RadioButton { Enabled: false } rb) { return; }
        var idx = Array.IndexOf(_stationButtons, rb); // rbtn N <-> dgv-Zeile N-1
        if (idx < 0 || idx >= dgvStations.Rows.Count) { return; }
        tcMain.SelectedTab = tpStations;
        dgvStations.ClearSelection();
        dgvStations.CurrentCell = dgvStations.Rows[idx].Cells[0]; // setzt die aktuelle Zelle und scrollt die Zeile in den sichtbaren Bereich
        dgvStations.Rows[idx].Selected = true;
        dgvStations.Focus();
    }

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
            var font = e.InheritedRowStyle.Font ?? dGrid.Font;
            var rhForeColor = dgvStations.Rows[e.RowIndex].Index >= stationSum ? SystemColors.ControlLightLight : dGrid.RowHeadersDefaultCellStyle.ForeColor;
            using SolidBrush sBrush = new(rhForeColor);
            e.Graphics.DrawString(rowText, font, sBrush, headerBounds, centerFormat);
        }
        if (e.RowIndex == _dropIndicatorRowIndex)   // Einfügelinie beim Drag&Drop
        {
            const int arrowSize = 5;
            var lineY = e.RowBounds.Top + 1;
            var left = e.RowBounds.Left;
            var right = e.RowBounds.Right;
            using var pen = new Pen(SystemColors.GradientActiveCaption, 2);
            e.Graphics.DrawLine(pen, left + arrowSize + 1, lineY, right - arrowSize - 1, lineY);
            using var brush = new SolidBrush(SystemColors.Highlight);
            e.Graphics.FillPolygon(brush, (Point[])[new(left, lineY - arrowSize), new(left, lineY + arrowSize), new(left + arrowSize + 1, lineY)]);
            e.Graphics.FillPolygon(brush, (Point[])[new(right, lineY - arrowSize), new(right, lineY + arrowSize), new(right - arrowSize - 1, lineY)]);
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
                    StatusStrip_SingleLabel(false, Lng.T("Autorun written to Registry"));
                }
            }
            else
            {
                if (Utilities.IsAutoStartEnabled(appName, "\"" + appPath + "\"" + " -min"))
                {
                    Utilities.UnSetAutoStart(appName);
                    StatusStrip_SingleLabel(false, Lng.T("Autorun deleted from Registry"));
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
                    StatusStrip_SingleLabel(false, Lng.T("Hotkey unregistered"));
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
            { // 1. Schritt: vorhanden Hotkey löschen
                StatusStrip_SingleLabel(false, Lng.T("Hotkey unregistered"));
                hkLetter = string.Empty;
            }
            if (string.IsNullOrEmpty(hkLetter) && cbHotkey.Checked && HotkeyLettersRegex().IsMatch(cmbxHotkey.Text))
            { // 2. Schritt: neuen Hotkey registrieren
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
            StatusStrip_SingleLabel(false, Lng.T("Hotkey registered") + " (Ctrl+Win+" + hkString + ")");
            toolStripStatusLabel.IsLink = false;
            hkLetter = hkString;
        }
        else
        {
            hkLetter = string.Empty;
            cbHotkey.Checked = false;
            cmbxHotkey.SelectedIndex = 0;
            tcMain.SelectedIndex = 3; // Hotkey-Dialog anzeigen
            StatusStrip_SingleLabel(false, Lng.T("Sorry, another application is using this hotkey!"));
            cmbxHotkey.Enabled = false;
            lblHotkey.Enabled = false;
        }
    }

    private void CmbxOutput_CreateContent()
    {
        List<string> devicelist = [];
        BASS_DEVICEINFO info; // = new BASS_DEVICEINFO();
        for (var n = 0; (info = Bass.BASS_GetDeviceInfo(n)) != null; n++) { if (info.IsEnabled) { devicelist.Add(info.ToString()); } }
        if (devicelist.Count > 0 && devicelist[0].Contains("No sound")) { devicelist.RemoveAt(0); } // 0: No sound
        if (devicelist.Count != 0) // if (!list.Any())
        {
            devicelist[0] += " " + Lng.T("(recommended)");
            cmbxOutput.Items.Clear();
            cmbxOutput.Items.AddRange([.. devicelist]);
            if (_stream != 0)
            {
                var device = Bass.BASS_ChannelGetDevice(_stream); // 0 = no sound, 1 = default
                intOutputDevice = device <= 1 || device == 0x20000 ? 0 : device - 1; // const int bass_nodevice = 0x20000;
            }
            else { intOutputDevice = cmbxOutput.FindString(_settings.OutputDevice); } // Index des Elements, das mit der Zeichenfolge beginnt...
            cmbxOutput.SelectedIndex = intOutputDevice > 0 && devicelist.Count >= intOutputDevice ? intOutputDevice : 0;
        }
    }

    private void CmbxOutput_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (changeOutputDevice && cmbxOutput.Visible && cmbxOutput.Focused)
        {
            intOutputDevice = cmbxOutput.SelectedIndex;
            _settings.OutputDevice = cmbxOutput.Items[cmbxOutput.SelectedIndex]?.ToString() ?? string.Empty;
            _settings.OutputDevice = _settings.OutputDevice.StartsWith("Default") ? "Default" : _settings.OutputDevice; // (recommended) entfernen
            if (prevOutputDevice != _settings.OutputDevice) { somethingToSave = true; }
            LogEvent("CmbxOutput_SelectedIndexChanged: " + _settings.OutputDevice + " (" + intOutputDevice + ") is selected");
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
            prevOutputDevice = _settings.OutputDevice; //somethingToSave = false; s. o.
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
        var count = historyLV.Items.Count;
        if (count > 0) { StatusStrip_SingleLabel(false, count + " " + (count == 1 ? Lng.T("entry") : Lng.T("entries")) + " (" + totalPlayingTime.ToString(@"hh\:mm\:ss") + ")"); }
        else { StatusStrip_SingleLabel(false, string.Empty); }
    }

    private void TPSettings_SetStatusBarText()
    {
        var statusStripText = string.Empty;
        BASS_DEVICEINFO info; // = new BASS_DEVICEINFO();
        for (var n = 1; (info = Bass.BASS_GetDeviceInfo(n)) != null; n++) // n = 1 => Default
        {
            if (n == intOutputDevice + 1) { statusStripText = Lng.T("Current output: ") + info.ToString() + (n == 1 ? " " + Lng.T("(adjusted by system settings, press F8)") : ""); break; } // info.IsInitialized funkt nicht
        }
        StatusStrip_SingleLabel(false, statusStripText);
    }

    private void CbAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAlwaysOnTop.Focused)
        {
            miniPlayer.TopMost = TopMost = _settings.AlwaysOnTop = cbAlwaysOnTop.Checked;
            miniPlayer.MpBtnAOT.Image = _settings.AlwaysOnTop ? Properties.Resources.pinpush : Properties.Resources.pinout;
            miniPlayer.MpBtnAOT.BackColor = _settings.AlwaysOnTop ? Color.Maroon : SystemColors.ControlDark;
            somethingToSave = true;
            Activate();
        }
    }

    private void CbAutoStopRecording_CheckedChanged(object sender, EventArgs e)
    {
        if (cbAutoStopRecording.Focused)
        {
            if (cbAutoStopRecording.Checked) { _settings.AutoStopRecording = true; }
            else { _settings.AutoStopRecording = false; }
            somethingToSave = true;
        }
    }

    private void CbShowBalloonTip_CheckedChanged(object sender, EventArgs e)
    {
        if (cbShowBalloonTip.Focused)
        {
            if (cbShowBalloonTip.Checked) { _settings.BalloonTips = true; }
            else { _settings.BalloonTips = false; }
            somethingToSave = true;
        }
    }

    /// <summary>Liest "/l xx" bzw. "/language xx" (auch mit Bindestrich) aus der Kommandozeile; xx = en/de/es/fr.
    /// Der Installer übergibt so die Setup-Sprachauswahl, der Schalter taugt aber auch für Verknüpfungen/Skripte.</summary>
    private static string? GetCmdLineLanguage()
    {
        var args = Environment.GetCommandLineArgs();
        for (var i = 1; i < args.Length - 1; i++)
        {
            if (CmdLanguageRegex().IsMatch(args[i]))
            {
                var code = args[i + 1].ToLowerInvariant();
                if (Array.IndexOf(_languageCodes, code) >= 0) { return code; }
            }
        }
        return null;
    }

    /// <summary>Windows-Anzeigesprache als en/de/es/fr, sofern unterstützt — sonst null (Englisch bleibt).
    /// Wird nur beim allerersten Start ohne Konfiguration herangezogen; "/l" hat Vorrang.</summary>
    private static string? GetSystemLanguage()
    {
        var code = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
        return Array.IndexOf(_languageCodes, code) >= 0 ? code : null;
    }

    private void CbUiLanguage_SelectionChangeCommitted(object sender, EventArgs e)
    {
        var code = _languageCodes[Math.Max(0, cbUiLanguage.SelectedIndex)];
        if (code == _settings.Language) { return; }
        _settings.Language = code;
        somethingToSave = true;
        if (Utilities.YesNo_TaskDialog(this, Lng.T("The language change takes effect after restarting the program."), Lng.T("Restart now?")).IsYes)
        {
            if (_setupVersion) { Environment.SetEnvironmentVariable("NETRADIO_SETUPMODE", "1"); } // vererbt sich auf den Neustart-Prozess und hält ihn im Setup-Modus (wichtig für per Debugger gestartete Instanzen)
            Program.ReleaseSingleInstanceMutex(); // sonst hält sich die neue Instanz für eine Zweitinstanz und beendet sich sofort
            Application.Restart(); // löst FormClosing aus, die Einstellungen werden dort gespeichert
        }
    }

    private void FrmMain_Shown(object sender, EventArgs e)
    {
        miniPlayer.Hide();
        miniPlayer.Opacity = 1;
        if (!string.IsNullOrEmpty(hkLetter) && HotkeyLettersRegex().IsMatch(hkLetter)) { RegisterHK(hkLetter); } // Hotkey kann erst registriert werden, wenn das Fenster erstellt wurde
        if (startMiniCmd || startTrayCmd)
        {
            Hide();
            Opacity = 1; // nach Hide //notifyIcon.ShowBalloonTip(1, Text, "Autostart", ToolTipIcon.Info);
        }
        if (_settings.AlwaysOnTop) { miniPlayer.TopMost = TopMost = true; }
        if (startMiniCmd) { ShowMiniPlayer(); }
        ExecuteAutoStart();   // statt: autoStartRadioButton.Checked = true;
        if (_autoStartStationNumber < 1) { FocusStationButton(); } // Es soll nie vorkommen, dass kein Button den Fokus hat (Leertaste = Play)


        if (_settings.UpdateIndex == 0 && (DateTime.UtcNow - _settings.LastUpdateSearch).TotalDays > 1 ||
            _settings.UpdateIndex == 1 && (DateTime.UtcNow - _settings.LastUpdateSearch).TotalDays > 7 ||
            _settings.UpdateIndex == 2 && (DateTime.UtcNow - _settings.LastUpdateSearch).TotalDays > 30)
        {
            BtnUpdate_Click(btnUpdate, EventArgs.Empty);
            if (updateAvailable)
            {
                ShowFullPlayer();
                tcMain.SelectedTab = tpInfo;

            }
            else { lblUpdate.Text = Lng.T("Current version:") + " " + strVersion; }
        }
        mainShown = true;

        if (tableActions != null && tableActions.Rows.Count > 0)
        {
            foreach (DataRow r in tableActions.Rows)
            {
                if (r.Field<string>("Task") == Utilities.TaskNames[6] && r.Field<bool>("Enabled") &&
                    (DateTime.TryParse(r.Field<string>("Time"), out var parsedTime) &&
                    (parsedTime - DateTime.Now > TimeSpan.Zero || _settings.RepeatActionsDaily)))
                {
                    var btnCancel = TaskDialogButton.Continue;
                    TaskDialogButton btnAction = new TaskDialogCommandLinkButton(Lng.T("Check the settings"));
                    TaskDialogPage taskDialogPage = new()
                    {
                        Icon = TaskDialogIcon.ShieldWarningYellowBar,
                        Caption = appName,
                        Heading = Lng.T("Following task is active!"),
                        Text = string.Format(Lng.T("The computer will shut down at {0}."), r.Field<string>("Time")),
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

    private void ExecuteAutoStart()
    {
        if (_autoStartStationNumber < 1) { return; }
        LogEvent($"ExecuteAutoStart: station no. {_autoStartStationNumber} (settings: {_settings.AutostartStation}, per Kommandozeile übersteuerbar)");
        var station = ReadStationFromDgv(_autoStartStationNumber);
        if (!string.IsNullOrEmpty(station.Name))
        {
            lblD1.Text = station.LabelName;  // Label vorab setzen (für Mini/Tray-Modus – sichtbar bevor StartPlaying abgeschlossen ist)
            miniPlayer.MpCmBxStations.Text = station.LongName;
        }
        SelectStation(station.IsValid ? station : null);
        if (_autoStartStationNumber >= 1 && _autoStartStationNumber <= stationSum) { _stationButtons[_autoStartStationNumber - 1].Focus(); }
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
                            if (_settings.CloseToTray) { miniPlayer.Hide(); }
                            else { ShowMiniPlayer(); }
                            if (NativeMethods.IsKeyDown(Keys.Escape)) { miniPlayer.MpToolTip.Active = false; } // Workaround for persistent ToolTip display
                            else { miniPlayer.MpToolTip.Active = true; }
                        }
                    }
                    return true;
                }
            case Keys.Space:
                {
                    if ((tcMain.SelectedIndex is 0 or 6) && !btnPlayStop.Focused) // Player- und Spectrum-Tab: Leertaste = Play/Pause
                    {
                        if (btnPlayStop.Enabled) { BtnPlayStop_Click(btnPlayStop, EventArgs.Empty); } // direkter Aufruf: PerformClick wirkt nicht auf unsichtbarer TabPage
                        if (tcMain.SelectedIndex == 0) { NativeMethods.SetFocus(btnPlayStop.Handle); }
                        return true;
                    }
                    else { return false; }
                }
            case Keys.Q | Keys.Control: { Application.Exit(); return true; } // beendet immer, auch bei close2Tray (SaveConfig/Cleanup in FrmMain_FormClosing)
            case Keys.F1 | Keys.Control | Keys.Shift: { helpRequested = false; Utilities.StartFile(this, settingsPath); return true; }
            case Keys.F2 | Keys.Control | Keys.Shift: { Utilities.StartFile(this, stationsPath); return true; }
            case Keys.F3 | Keys.Control | Keys.Shift: { Utilities.StartFile(this, logPath); return true; }
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
            // Hinweis: PerformClick()/Focus() wirken nur auf SICHTBARE Buttons (CanSelect) - auf dem Spectrum-Tab
            // liegen die Player-Buttons auf der unsichtbaren tpPlayer. Deshalb werden die Click-Handler direkt
            // aufgerufen (mit Enabled-Guard) und der Fokus nur auf dem Player-Tab gesetzt.
            case Keys.Oemplus:
            case Keys.Add:
                {
                    if (tcMain.SelectedIndex is 0 or 6)
                    {
                        if (btnIncrease.Enabled) { BtnIncrease_Click(btnIncrease, EventArgs.Empty); }
                        if (tcMain.SelectedIndex == 0) { btnIncrease.Focus(); }
                    }
                    return true;
                }
            case Keys.OemMinus:
            case Keys.Subtract:
                {
                    if (tcMain.SelectedIndex is 0 or 6)
                    {
                        if (btnDecrease.Enabled) { BtnDecrease_Click(btnDecrease, EventArgs.Empty); }
                        if (tcMain.SelectedIndex == 0) { btnDecrease.Focus(); }
                    }
                    return true;
                }
            case Keys.Back:
                {
                    if (tcMain.SelectedIndex is 0 or 6)
                    {
                        if (btnReset.Enabled) { BtnReset_Click(btnReset, EventArgs.Empty); }
                        if (tcMain.SelectedIndex == 0) { btnReset.Focus(); }
                        return true;
                    }
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
            var (isYes, _, _) = Utilities.YesNo_TaskDialog(null, string.Format(Lng.T("{0} was not found in the program directory."), Path.GetFileName(pdfPath)), Lng.T("Would you like to download it from the Internet?"));
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
    {
        if (e.CloseReason == CloseReason.UserClosing && _settings.CloseToTray && (ModifierKeys & Keys.Shift) == 0)
        {
            e.Cancel = true;
            Hide();
            return;
        }
        LogEvent($"FrmMain_FormClosing: CloseReason={e.CloseReason}"); // kritisch für die Diagnose der Beenden-Pfade
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
        // History-CSV nur schreiben, wenn der Anwender das aktiv eingestellt hat (Wert > 0)
        // und es überhaupt Einträge gibt. Bestehende Dateien bleiben unangetastet (Löschen über delAllHistoriesBtn).
        if (numUpDnSaveHistory.Value > 0 && historyLV.Items.Count > 0) { SaveHistoryFile(true); }
    }

    private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
    {
        _playIcon.Dispose();  // siehe auch "notifyIcon.Visible = false;" in FrmMain_FormClosing, dort kann
        _pauseIcon.Dispose();  // NotifyIcon kann beim Ausblenden noch sicher auf die Handles zugreifen
    }

    private void SaveConfig() // FrmMain_FormClosing | TcMain_SelectedIndexChanged
    {
        try
        {
            JsonConfig.Save(settingsPath, CollectSettings()); // atomar (Temp + File.Replace)
            if (radioBtnChanged) { JsonConfig.Save(stationsPath, CollectStations()); } // Stationen nur schreiben, wenn geändert
            LogEvent("SaveConfig: settings" + (radioBtnChanged ? " + stations" : "") + " saved");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            LogEvent($"SaveConfig FAILED: {ex.GetType().Name} - {ex.Message}");
            Utilities.ErrTaskDialog(this, ex);
        }
        somethingToSave = false;
        radioBtnChanged = false;
    }

    private AppSettings CollectSettings() // ergänzt in _settings nur die Werte, die nicht ohnehin direkt dort gepflegt werden
    {
        _settings.HotkeyEnabled = cbHotkey.Checked;
        _settings.HotkeyLetter = hkLetter;
        _settings.Volume = (int)(channelVolume * 100f);
        _settings.SaveHistory = Convert.ToInt32(numUpDnSaveHistory.Value);
        _settings.FormPosX = Bounds.X; // RestoreBounds.Location funktioniert nicht richtig
        _settings.FormPosY = Bounds.Y;
        _settings.FormWidth = Bounds.Width;
        _settings.FormHeight = Bounds.Height;
        _settings.MiniPosX = miniPlayer.Location.X;
        _settings.MiniPosY = miniPlayer.Location.Y;
        _settings.Actions = tableActions is null ? [] : [.. tableActions.AsEnumerable().Select(static row => new ActionTask
        {
            Enabled = row.Field<bool>("Enabled"),
            Task = row.Field<string>("Task") ?? string.Empty,
            Station = row.Field<string>("Station") ?? string.Empty,
            Time = row.Field<string>("Time") ?? string.Empty,
        })];
        return _settings;
    }

    /// <summary>Nur belegte Stationen mit ihrer 1-basierten Nummer - leere Positionen landen nicht in stations.json.</summary>
    private List<StationEntry> CollectStations() => [.. _stationData
        .Select(static (row, i) => new StationEntry { Number = i + 1, Name = row.Name, Url = row.Url })
        .Where(static st => st.Name.Length > 0 || st.Url.Length > 0)];

    private void BackupDaily(string filePath) // tägliche Sicherungskopie (.bak)
    {
        var bakPath = Path.ChangeExtension(filePath, ".bak");
        if (File.Exists(filePath) && (!File.Exists(bakPath) || File.GetLastWriteTime(bakPath).Date < File.GetLastWriteTime(filePath).Date.AddDays(-1)))
        {
            File.Copy(filePath, bakPath, true);
            File.SetLastWriteTime(bakPath, DateTime.Now);
            LogEvent("BackupDaily: " + bakPath);
        }
    }

    private void SaveHistoryFile(bool deleteFiles = false) // LoadHistoryBtn_Click und FrmMain_FormClosing
    {
        Utilities.SortHistoryNormal(historyLV, lviComparer, lvSortOrderArray);
        var folderPath = Path.GetDirectoryName(settingsPath) ?? "";
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

    private void ExitToolStripMenuItem_Click(object sender, EventArgs e) =>
        Application.Exit(); // SaveConfig/Cleanup in FrmMain_FormClosing (CloseReason.ApplicationExitCall)

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
            else { currName = Lng.T("[empty]"); }
            tcMain.SelectedIndex = 1; // Sendertabelle
            using FrmSearch frmSearch = new(currRow, currName);
            if (_settings.AlwaysOnTop) { frmSearch.TopMost = true; }
            if (frmSearch.ShowDialog() == DialogResult.OK)
            {
                var searchString = frmSearch.TbString.Text;
                if (searchString.Length > 0)
                {
                    using FrmBrowser frmBrowser = new(searchString.Trim(), Location, curVersion);
                    if (_settings.AlwaysOnTop) { frmBrowser.TopMost = true; }
                    var result = frmBrowser.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        currName = string.IsNullOrEmpty(currName) ? string.Format(Lng.T("row {0}"), dgvStations.SelectedRows[0].Index + 1) : currName;

                        // Prüfen, ob bereits eine URL existiert
                        if (!string.IsNullOrEmpty(_stationData[dgvStations.SelectedRows[0].Index].Url) &&
                            !Utilities.YesNo_TaskDialog(this, string.Format(Lng.T("Overwrite {0}?"), currName), Lng.T("This entry already contains a URL. Do you want to replace it?")).IsYes)
                        {
                            return;
                        }
                        dgvStations.EndEdit(); // laufende Bearbeitung abschliessen
                        var rowIdx = dgvStations.SelectedRows[0].Index;
                        _stationData[rowIdx].Name = frmBrowser.SelectedStation ?? string.Empty;
                        _stationData[rowIdx].Url = frmBrowser.SelectedURL ?? string.Empty;
                    }
                }
                if (firstEmptyStart)  // für ein schnelles Erfolgselebnis
                {
                    var url = _stationData[0].Url;
                    if (currRow == "1" && !string.IsNullOrEmpty(url))
                    {
                        tcMain.SelectedIndex = 0; // nach StartPlaying 
                        StartFirstStationAfterImport();
                    }
                    firstEmptyStart = false;
                }
            }
        }
        else { Utilities.MsgTaskDialog(this, Lng.T("Target not selected!")); }

    }

    private void StartFirstStationAfterImport()
    {
        var station = ReadStationFromDgv(1);
        if (station.IsValid)
        {
            firstEmptyStart = false;
            SelectStation(station);
        }
    }

    private void BtnUp_Click(object sender, EventArgs e)
    {
        if (dgvStations.SelectedCells.Count == 0) { return; }
        var idx = dgvStations.SelectedCells[0].OwningRow?.Index ?? 0;
        if (idx < 1) { return; }
        MoveStationData(idx, idx - 1);
        dgvStations.ClearSelection();
        dgvStations.Rows[idx - 1].Selected = true;
        dgvStations.CurrentCell = dgvStations.Rows[idx - 1].Cells[0];
        dgvStations.Focus();
    }

    private void BtnDown_Click(object sender, EventArgs e)
    {
        if (dgvStations.SelectedCells.Count == 0) { return; }
        var idx = dgvStations.SelectedCells[0].OwningRow?.Index ?? dgvStations.Rows.Count;
        if (idx >= dgvStations.Rows.Count - 1) { return; }
        MoveStationData(idx, idx + 1);
        dgvStations.ClearSelection();
        dgvStations.Rows[idx + 1].Selected = true;
        dgvStations.CurrentCell = dgvStations.Rows[idx + 1].Cells[0];
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
        MoveStationData(idx, rowIndex);
        dgvStations.Rows[rowIndex].Selected = true;
        dgvStations.CurrentCell = dgvStations.Rows[rowIndex].Cells[0];
    }

    private void MoveStationData(int from, int to)  // ListChanged feuert automatisch → radioBtnChanged wird gesetzt
    {
        if (from == to || from < 0 || to < 0 || from >= _stationData.Count || to >= _stationData.Count) { return; }
        var (name, url) = (_stationData[from].Name, _stationData[from].Url);
        if (from < to) { for (var i = from; i < to; i++) { _stationData[i].CopyFrom(_stationData[i + 1]); } }
        else { for (var i = from; i > to; i--) { _stationData[i].CopyFrom(_stationData[i - 1]); } }
        _stationData[to].Name = name;
        _stationData[to].Url = url;
    }

    private void DgvStations_SelectionChanged(object sender, EventArgs e)
    {
        if (sender is DataGridView dgv)
        {
            var ri = -1;
            foreach (DataGridViewCell cell in dgv.SelectedCells) { ri = cell.RowIndex; }
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
    {
        if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
        { //If the mouse moves outside the rectangle, start the drag.
            if (dragBoxFromMouseDown != Rectangle.Empty && !dragBoxFromMouseDown.Contains(e.X, e.Y))
            {
                dgvStations.DoDragDrop(rowIndexFromMouseDown, DragDropEffects.Move);
            }
        }
    }

    private void DgvStations_MouseDown(object sender, MouseEventArgs e)
    { //Get the index of the item the mouse is below
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
            { //Remember the point where the mouse down occurred. The DragSize indicates the size that the mouse can move before a drag event should be started.
                var dragSize = SystemInformation.DragSize;
                dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else { dragBoxFromMouseDown = Rectangle.Empty; }  // Reset the rectangle if the mouse is not over an item
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

        var clientPt = dgvStations.PointToClient(new Point(e.X, e.Y));  // Einfügelinie: Zielzeile ermitteln und bei Änderung neu zeichnen
        var newTarget = e.Effect == DragDropEffects.Move ? dgvStations.HitTest(clientPt.X, clientPt.Y).RowIndex : -1;
        if (newTarget != _dropIndicatorRowIndex)
        {
            if (_dropIndicatorRowIndex >= 0) { dgvStations.InvalidateRow(_dropIndicatorRowIndex); }
            _dropIndicatorRowIndex = newTarget;
            if (_dropIndicatorRowIndex >= 0) { dgvStations.InvalidateRow(_dropIndicatorRowIndex); }
        }
    }

    private void DgvStations_DragDrop(object sender, DragEventArgs e)
    {
        if (_dropIndicatorRowIndex >= 0) { dgvStations.InvalidateRow(_dropIndicatorRowIndex); }
        _dropIndicatorRowIndex = -1;  // Einfügelinie entfernen
        var clientPoint = dgvStations.PointToClient(new Point(e.X, e.Y));
        rowIndexOfItemUnderMouseToDrop = dgvStations.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
        if (e.Effect != DragDropEffects.Move || rowIndexOfItemUnderMouseToDrop < 0) { return; }
        if (rowIndexFromMouseDown < rowIndexOfItemUnderMouseToDrop) { rowIndexOfItemUnderMouseToDrop--; }
        MoveStationData(rowIndexFromMouseDown, rowIndexOfItemUnderMouseToDrop);
        if (rowIndexOfItemUnderMouseToDrop > 16) { dgvStations.FirstDisplayedScrollingRowIndex += 1; }
        dgvStations.ClearSelection();
        dgvStations.Rows[rowIndexOfItemUnderMouseToDrop].Selected = true;
        dgvStations.CurrentCell = dgvStations.Rows[rowIndexOfItemUnderMouseToDrop].Cells[0];
    }

    private void DgvStations_DragLeave(object sender, EventArgs e)
    {
        if (_dropIndicatorRowIndex >= 0) { dgvStations.InvalidateRow(_dropIndicatorRowIndex); }
        _dropIndicatorRowIndex = -1;
    }

    private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (dgvStations.SelectedRows.Count == 0) { return; }
        var row = dgvStations.SelectedRows[0];
        if (!_stationData[row.Index].IsEmpty)
        {
            var name = _stationData[row.Index].Name is { Length: > 0 } n
                ? n : string.Format(Lng.T("row {0}"), row.Index + 1);
            if (!Utilities.YesNo_TaskDialog(this,
                    string.Format(Lng.T("Delete {0}?"), name),
                    Lng.T("Are you sure you want to delete this entry?")).IsYes) { return; }
        }
        var selectedIdx = row.Index;
        DeleteStationData(selectedIdx);
        var newSel = Math.Min(selectedIdx, dgvStations.Rows.Count - 1);
        dgvStations.Rows[newSel].Selected = true;
        dgvStations.CurrentCell = dgvStations.Rows[newSel].Cells[0];
    }

    private void DeleteStationData(int index)
    {
        if (index < 0 || index >= _stationData.Count) { return; }
        for (var i = index; i < _stationData.Count - 1; i++)
        {
            _stationData[i].CopyFrom(_stationData[i + 1]);
        }
        _stationData[^1].Clear();
    }


    private void SearchStationToolStripMenuItem_Click(object sender, EventArgs e)
    {
        BtnSearch_Click(null!, null!);
    }

    private void AddToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (dgvStations.SelectedRows.Count == 0) { return; }
        InsertStationData(dgvStations.SelectedRows[0].Index);
    }

    private void InsertStationData(int at)
    {
        var emptyIdx = -1;
        for (var i = _stationData.Count - 1; i >= at; i--)
        {
            if (_stationData[i].IsEmpty) { emptyIdx = i; break; }
        }
        if (emptyIdx < 0) { Console.Beep(); return; } // kein Platz

        for (var i = emptyIdx; i > at; i--) { _stationData[i].CopyFrom(_stationData[i - 1]); }
        _stationData[at].Clear();
    }


    private void Row1ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(0);
    private void Row2ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(1);
    private void Row3ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(2);
    private void Row4ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(3);
    private void Row5ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(4);
    private void Row6ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(5);
    private void Row7ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(6);
    private void Row8ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(7);
    private void Row9ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(8);
    private void Row10ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(9);
    private void Row11ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(10);
    private void Row12ToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(11);
    private void UpToolStripMenuItem_Click(object sender, EventArgs e) => BtnUp_Click(null!, null!);
    private void DownToolStripMenuItem_Click(object sender, EventArgs e) => BtnDown_Click(null!, null!);
    private void TopToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(0);
    private void EndToolStripMenuItem_Click(object sender, EventArgs e) => KeyDown_MoveRowAt(dgvStations.RowCount - 1);

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
            _settings.AutostartStation = int.TryParse(cmbxStation.Text, out var autoNum) ? autoNum : 0;
            somethingToSave = true;
        }
        somethingToSave = true;
    }

    private void PicBoxPayPal_Click(object sender, EventArgs e) => Utilities.StartLink(this, "https://www.paypal.com/donate/?hosted_button_id=3HRQZCUW37BQ6");

    private void PicBoxPayPal_MouseEnter(object sender, EventArgs e) => picBoxPayPal.Cursor = Cursors.Hand;
    private void PicBoxPayPal_MouseLeave(object sender, EventArgs e) => picBoxPayPal.Cursor = Cursors.Default;

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
        _startPlayingCts?.Cancel();
        _startPlayingCts?.Dispose();
        var cts = _startPlayingCts = new CancellationTokenSource();
        var token = cts.Token;

        _playWakeFromSleep = false;

        // 1. OPTIMIERUNG: Sofortiges Audio-Feedback beim Klick!
        if (_stream != 0)
        {
            Bass.BASS_ChannelStop(_stream);
            Bass.BASS_StreamFree(_stream);
            _stream = 0;
            LogEvent("StartPlaying: Old stream freed on UI thread");
        }

        // Netzwerkprüfung
        if (!await Utilities.PingGoogleSuccessAsync(Bass.BASS_GetConfig(BASSConfig.BASS_CONFIG_NET_TIMEOUT)))
        {
            if (token.IsCancellationRequested) { return; }  // schon veraltet

            // _stream ist bereits 0, BASS_StreamFree(_stream) ist hier nicht mehr nötig
            RestorePlayerDefaults(tagID);

            TaskDialogButton btnSettings = new(Lng.T("NetworkSettings", "Settings…")); // eigener Schlüssel: im Dialog darf die Übersetzung länger sein als auf btnActions
            TaskDialogPage page = new()
            {
                Caption = appName,
                SizeToContent = true,
                Heading = Lng.T("No Internet Connection!"),
                Text = Lng.T("Check the network connection status."),
                Icon = TaskDialogIcon.ShieldWarningYellowBar,
                Buttons = { btnSettings, TaskDialogButton.Close }
            };

            if (TaskDialog.ShowDialog(miniPlayer.Visible ? miniPlayer : this, page) == btnSettings)
            {
                Utilities.StartLink(this, "ms-settings:network-status");
            }
            return;
        }
        if (token.IsCancellationRequested) { return; }

        pbVolIcon.Image = Properties.Resources.progress;
        miniPlayer.MpVolProgBar.Value = volProgressBar.Value = 0;
        lblVolume.Text = "0";

        if (!string.IsNullOrEmpty(_url) && _url.EndsWith(".m3u", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                var client = NetHttpClient.Instance;

                // 2. OPTIMIERUNG: Token an GetStringAsync übergeben!
                var newURL = await client.GetStringAsync(_url, token);

                var urlString = HttpUrlRegex().Replace(newURL, "$1");
                var secString = HttpsUrlRegex().Replace(newURL, "$1");

                newURL = secString.StartsWith("https", StringComparison.OrdinalIgnoreCase) ? secString :
                         urlString.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? urlString : newURL;

                _url = string.IsNullOrEmpty(newURL) ? _url : newURL;
                if (token.IsCancellationRequested) { return; }
            }
            catch (OperationCanceledException)
            {
                return; // Lautlos beenden bei Abbruch
            }
            catch (Exception ex)
            {
                if (token.IsCancellationRequested) { return; }
                RestorePlayerDefaults(tagID);
                Utilities.ErrTaskDialog(this, ex);
                return;
            }
        }

        lblD3.Text = "⌛" + Lng.T("Connecting...");
        MiniPlayer.MpLblD2_Text("⌛" + Lng.T("Connecting..."));

        var windowHandle = Handle;

        try
        {
            // 3. OPTIMIERUNG: Rückgabewert nutzen und Token an Task.Run übergeben
            var newStream = await Task.Run(() =>
            {
                if (token.IsCancellationRequested) { return 0; }

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

                if (token.IsCancellationRequested) { return 0; }

                var flag = BASSFlag.BASS_DEFAULT | BASSFlag.BASS_STREAM_STATUS | BASSFlag.BASS_STREAM_AUTOFREE | BASSFlag.BASS_STREAM_BLOCK;

                return Bass.BASS_StreamCreateURL(_url, 0, flag, myStreamCreateURL, IntPtr.Zero);
            }, token);

            // Zurück auf dem UI Thread 
            if (token.IsCancellationRequested)
            {
                if (newStream != 0)
                {
                    Bass.BASS_StreamFree(newStream);
                    LogEvent("StartPlaying: Abgebrochen nach Stream-Erstellung, Stream freigegeben");
                }
                return;
            }

            _stream = newStream;

            if (_stream == 0)
            {
                var errorDescription = Utilities.GetErrorDescription(Bass.BASS_ErrorGetCode());
                RestorePlayerDefaults(tagID);
                Bass.BASS_Free();
                Utilities.MsgTaskDialog(this, Lng.T("Stream creation failed"), errorDescription);
                LogEvent("StartPlaying (BASS_StreamCreateURL): " + errorDescription);
                return;
            }

            _tagInfo = new TAG_INFO(_url);
            if (_tagInfo != null && BassTags.BASS_TAG_GetFromURL(_stream, _tagInfo))
            {
                var streamFormat = GetStreamFormatName(); // ersetzt "???", wenn Bass.Net den Channeltype nicht kennt (OPUS, FLAC-in-OGG, HLS, ...)
                lblD3.Text = _tagInfo.channelinfo.ToString().Replace("48000Hz", "48kHz").Replace("44100Hz", "44.1kHz").Replace("???, ", streamFormat.Length > 0 ? streamFormat + ", " : "");

                lblD3.Text = AudioFormatSpacingRegex().Replace(lblD3.Text, "$1 $2");

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
            if (_settings.LogHistory) { AddToHistory(lblD2.Text); }

            // Syncs setzen
            _connectFail = new SYNCPROC(ConnectionSync);
            if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_DOWNLOAD | BASSSync.BASS_SYNC_ONETIME, 0, _connectFail, IntPtr.Zero) == 0)
            {
                Utilities.MsgTaskDialog(this, Lng.T("Setting up a download synchronizer failed."), "", TaskDialogIcon.Warning);
            }

            _deviceFail = new SYNCPROC(DeviceSync);
            if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_DEV_FAIL | BASSSync.BASS_SYNC_ONETIME, 0, _deviceFail, IntPtr.Zero) == 0)
            {
                Utilities.MsgTaskDialog(this, Lng.T("Setting up a device synchronizer failed."), "", TaskDialogIcon.Warning);
            }

            _metaSync = new SYNCPROC(MetaSync);
            if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_META, 0, _metaSync, IntPtr.Zero) == 0)
            {
                Utilities.MsgTaskDialog(this, Lng.T("Setting up a meta synchronizer failed."), "", TaskDialogIcon.Warning);
            }

            _oggSync = new SYNCPROC(OggSync);
            if (Bass.BASS_ChannelSetSync(_stream, BASSSync.BASS_SYNC_OGG_CHANGE, 0, _oggSync, IntPtr.Zero) == 0)
            {
                LogEvent("Setting up an OGG synchronizer failed: " + Bass.BASS_ErrorGetCode()); // nur relevant für OGG-Streams, daher kein Dialog
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
            var info = new BASS_CHANNELINFO();
            if (tagID > 0 && tagID <= _stationData.Count && Bass.BASS_ChannelGetInfo(_stream, info) && !string.IsNullOrEmpty(info.filename))
            {
                //dgvStations.Rows[tagID - 1].Cells[1].Value = info.filename;
                _stationData[tagID - 1].Url = info.filename;

                if (lblD3.Text.Length <= 1 || lblD3.Text == "00:00:00") // BASS_TAG_GetFromURL lieferte nichts (z. B. OPUS/AAC-Streams ohne ICY-Tags) -> Kanalinfo aus BASS_CHANNELINFO aufbauen
                {
                    var streamFormat = GetStreamFormatName(); // ersetzt "???", wenn Bass.Net den Channeltype nicht kennt (OPUS, FLAC-in-OGG, HLS, ...)
                    lblD3.Text = HzRegex().Replace(info.ToString(), ((double)info.freq / 1000).ToString() + "kHz").Replace("???, ", streamFormat.Length > 0 ? streamFormat + ", " : "");

                    lblD3.Text = AudioFormatSpacingRegex().Replace(lblD3.Text, "$1 $2");

                    if (info.ctype == BASSChannelType.BASS_CTYPE_STREAM_MF &&
                        Marshal.PtrToStructure<WAVEFORMATEX>(Bass.BASS_ChannelGetTags(_stream, BASSTag.BASS_TAG_WAVEFORMAT))?.wFormatTag == WAVEFormatTag.MPEG_HEAAC)
                    {
                        lblD3.Text = lblD3.Text.Replace("MF", "AAC"); // Media-Foundation-Decoder = AAC (wie im TagInfo-Zweig)
                    }

                    lblD3.Text += ", 00:00:00";
                }

                if (lblD4.Text.EndsWith(" OK")) { lblD4.Text = info.filename; }
            }

            btnPlayStop.Image = Properties.Resources.pause_white;
            UpdateTaskbarIcon(true);
            miniPlayer.MpBtnPlay.Image = Properties.Resources.pause_white;
            playPauseToolStripMenuItem.Text = Lng.T("Pause");
            playPauseToolStripMenuItem.Image = Properties.Resources.pause;
        }
        catch (OperationCanceledException)
        {
            // Task wurde vor der Ausführung storniert
            LogEvent("StartPlaying Error: Task storniert.");
        }
        catch (Exception ex) when (!token.IsCancellationRequested)
        {
            if (_stream != 0) { Bass.BASS_StreamFree(_stream); _stream = 0; }
            RestorePlayerDefaults(tagID);
            Utilities.ErrTaskDialog(this, ex);
            LogEvent("StartPlaying Error: " + ex.Message);
        }
        catch (Exception)
        {
            // Abbruch durch neuen Sender-Klick – kein Fehler
            if (_stream != 0) { Bass.BASS_StreamFree(_stream); _stream = 0; }
        }
    }

    private void BtnRecord_Click(object sender, EventArgs e)
    {
        if (_recording)
        {
            RecordingStop(false, Color.Blue); // false = !BASS_StreamFree; recording = false; // muss hier so früh wie möglich erfolgen
            timerLevel.Stop();
            spectrumTimer.Stop();
            spectrumDisplay.Clear();
            pbLevel.Image = null;
            miniPlayer.MpPBLevel.Image = null;
            Bass.BASS_ChannelPause(_stream);
            playPauseToolStripMenuItem.Text = Lng.T("Play"); // btnPlayStop.Text = 
            btnPlayStop.Image = Properties.Resources.play_white;
            UpdateTaskbarIcon(false);
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
                if (_selectedStation == null) { return; }
                var rowIndex = _selectedStation.Number - 1;
                dgvStations.Rows[rowIndex].Selected = true;
                dgvStations.CurrentCell = dgvStations.Rows[rowIndex].Cells[0];
                if (_selectedStation.Number > 12) { dgvStations.FirstDisplayedScrollingRowIndex = dgvStations.SelectedRows[0].Index; }
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
                if (tcMain.SelectedTab == tpHistory)
                {
                    TPHistory_SetStatusBarText();
                }

                var seconds = currPlayingTime.ToString(@"hh\:mm\:ss");

                // IsMatch ist performanter als Match().Success
                if (TimeTrailingRegex().IsMatch(lblD3.Text))
                {
                    lblD3.Text = lblD3.Text[..^8] + seconds;
                }
                else
                {
                    if (lblD3.Text.Length > 0)
                    {
                        lblD3.Text += ", " + seconds;
                        lblD3.Text = LeadingDashCommaRegex().Replace(lblD3.Text, ""); // Unnötige Zeichen am Anfang entfernen
                    }
                    else
                    {
                        lblD3.Text = seconds;
                    }
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
                Thread.Sleep(timerLevel.Interval / 2);
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
        spectrumDisplay.Clear();
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
        UpdateTaskbarIcon(false);
        btnPlayStop.BackColor = SystemColors.ControlDark;
        miniPlayer.MpBtnPlay.Image = Properties.Resources.play_white;
        miniPlayer.MpBtnPlay.BackColor = SystemColors.ControlDark;
        playPauseToolStripMenuItem.Text = Lng.T("Play"); // btnPlayStop.Text = 
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
            if (Path.GetDirectoryName(settingsPath) != Path.GetDirectoryName(appPath)) // (Utilities.IsInnoSetupValid(Path.GetDirectoryName(appPath)))
            {
                try
                {
                    // Lokale Datei vorbereiten
                    var targetDir = NativeMethods.GetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"));  // NativeMethods.SHGetKnownFolderPath(new Guid("374DE290-123F-4565-9164-39C4925E467B"), 0, IntPtr.Zero, out var targetDir); // Downloads folder    
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
                        _settings.LastUpdateSearch = DateTime.UtcNow;
                        somethingToSave = true;
                    }
                    else
                    {
                        Utilities.MsgTaskDialog(this, Lng.T("No update information."), appName, TaskDialogIcon.Information);
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
                Utilities.MsgTaskDialog(this, Lng.T("No update information."), "", TaskDialogIcon.Information);
            }
            else
            {
                if (updateVersion.CompareTo(curVersion) > 0)
                {
                    lblUpdate.Text = Lng.T("Update available:") + " v" + updateVersion.ToString();
                    btnUpdate.Text = Lng.T("Download & Install");
                    updateAvailable = true;
                    btnUpdate.BackColor = SystemColors.MenuHighlight;
                    btnUpdate.ForeColor = SystemColors.Info;
                    btnUpdate.Invalidate();
                }
                else
                {
                    lblUpdate.Text = lblUpdate.Text.Equals(Lng.T("No update available")) ? Lng.T("Current version:") + " " + strVersion : Lng.T("No update available");
                }
            }
        }
        else { Utilities.MsgTaskDialog(this, Lng.T("No internet connection."), "", TaskDialogIcon.ShieldWarningYellowBar); }
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
        if (_settings.AlwaysOnTop) { TopMost = true; }
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
            if (cbLogHistory.Checked) { _settings.LogHistory = true; }
            else
            {
                _settings.LogHistory = false;
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
        if (_settings.LogHistory && historyLV.Items.Count > 0) { Utilities.SortHistoryNormal(historyLV, lviComparer, lvSortOrderArray); }
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
        if (_settings.AlwaysOnTop) { frmSchedules.TopMost = true; }
        for (var i = 0; i < stationSum; i++)
        {
            if (dgvStations.Rows[i].Cells[0].Value != null && !string.IsNullOrEmpty(dgvStations.Rows[i].Cells[0].Value?.ToString()))
            {
                frmSchedules.StationsList.Add(Utilities.StationShort(dgvStations.Rows[i].Cells[0].Value?.ToString()));
            }
        }
        frmSchedules.ActionListView.Items.Clear();
        for (var j = 0; j < tableActions?.Rows.Count; j++)
        {
            frmSchedules.ActionListView.Items.Add(new ListViewItem(["", Lng.T(tableActions.Rows[j][1].ToString() ?? ""), tableActions.Rows[j][2].ToString() ?? "", tableActions.Rows[j][3].ToString() ?? ""])); // Task-Namen übersetzt anzeigen, gespeichert wird englisch

            frmSchedules.ActionListView.Items[j].Checked = tableActions.Rows[j].Field<bool>("Enabled");
        }
        for (var l = frmSchedules.ActionListView.Items.Count; l < 9; l++) // mit Leerzeilen auffüllen - erspart Butte "Add" für neue Einträge
        {
            frmSchedules.ActionListView.Items.Add(new ListViewItem(["", "", "", ""]));
        }
        frmSchedules.ActionListView.Items[0].Selected = true;
        frmSchedules.RepeatActionsDaily.Checked = _settings.RepeatActionsDaily && (tableActions?.AsEnumerable().Any(row => row.Field<bool>("Enabled")) ?? false);
        if (frmSchedules.ShowDialog() == DialogResult.OK)
        {
            StopActions(); // erst jetzt weil alle Zeilen in tableActions auf not enabled (False) gesetzt werden
            tableActions?.Rows.Clear();
            cbActions.Checked = false;
            _settings.RepeatActionsDaily = frmSchedules.RepeatActionsDaily.Checked;
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
                        if (!string.IsNullOrEmpty(item.SubItems[j].Text)) { cells[j] = j == 1 ? Utilities.TaskNameFromDisplay(item.SubItems[j].Text) : item.SubItems[j].Text; } // Task-Spalte: übersetzte Anzeige auf den englischen Speichernamen zurückführen
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
                        Utilities.MsgTaskDialog(this, string.Format(Lng.T("Task #{0} is not executed because the time specification is incorrect."), i));
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
        ((System.Windows.Forms.Timer)sender).Stop();
        if (sender == timerAction1) { num = 0; }
        else if (sender == timerAction2) { num = 1; }
        else if (sender == timerAction3) { num = 2; }
        else if (sender == timerAction4) { num = 3; }
        else if (sender == timerAction5) { num = 4; }
        else if (sender == timerAction6) { num = 5; }
        else if (sender == timerAction7) { num = 6; }
        else if (sender == timerAction8) { num = 7; }
        else if (sender == timerAction9) { num = 8; }

        if (!_settings.RepeatActionsDaily) { tableActions.Rows[num][0] = false; } // Aufgabe deaktivieren
        if (!tableActions.AsEnumerable().Any(row => row.Field<bool>("Enabled") == true)) { cbActions.Checked = false; }

        if (tcMain.SelectedIndex != 0) { tcMain.SelectedIndex = 0; }

        var tableAction = tableActions.Rows[num][1].ToString();
        if (!string.IsNullOrEmpty(tableAction))
        {
            if (tableAction.Equals(Utilities.TaskNames[0], StringComparison.Ordinal)) // "Start playing"
            {
                using var button = tcMain.TabPages[0].Controls.OfType<RadioButton>().FirstOrDefault(y => y.Text.Equals(tableActions.Rows[num][2].ToString(), StringComparison.Ordinal));
                if (button != null) { button.Checked = true; }
                else { Utilities.MsgTaskDialog(this, Lng.T("Station not found.")); }
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
                    button?.Checked = true; // löst StartPlaying aus
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
                if (Utilities.IsActionCancelled(this, string.Format(Lng.T("NetRadio - Task No. {0}"), num + 1), Lng.T("The PC will go into sleep mode."), 10))
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
                if (Utilities.IsActionCancelled(this, string.Format(Lng.T("NetRadio - Task No. {0}"), num + 1), Lng.T("The PC will go into hibernation mode."), 10))
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
                if (Utilities.IsActionCancelled(this, string.Format(Lng.T("NetRadio - Task No. {0}"), num + 1), Lng.T("The computer will shut down."), 10, TaskDialogIcon.Warning))
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
                Application.Exit(); // nicht Close(): das ergäbe CloseReason.UserClosing und bliebe bei close2Tray im Tray hängen
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
        if (e.ColumnIndex == 0 && frmSplash == null)
        {
            frmSplash = new(this) { TopMost = true }; // using geht nur mit ShowDialog
            frmSplash.SplashActivated += new EventHandler(SplashForm_Activated);
            NativeMethods.ShowWindow(frmSplash.Handle, NativeMethods.SW_SHOWNOACTIVATE); // ohne TopMost!
        }
    }

    private void DgvStations_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
        if (e.ColumnIndex == 0 && frmSplash != null)
        {
            frmSplash.Close();
            frmSplash.Dispose();
            frmSplash = null;
        }
    }

    private void StationData_ListChanged(object? sender, ListChangedEventArgs e)  // statt radioBtnChanged
    {
        if (e.ListChangedType != ListChangedType.ItemChanged) { return; }
        radioBtnChanged = true;
        somethingToSave = true;
        UpdateStatusLabelStationsList();
        if (e.NewIndex < stationSum && e.PropertyDescriptor?.Name == nameof(StationRow.Name)) { RefreshStationButtons(); }  // nicht bei URL-Normalisierung durch StartPlaying
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
                _settings.CloseToTray = true;
                if (_settings.ShowTrayInfo)
                {
                    TaskDialogPage page = new()
                    {
                        Heading = Lng.T("You still have the following options to exit:"),
                        Text = Lng.T("TrayModeOptions", "1. Right-click on the NetRadio icon in the system tray and Exit.\n\n2. Press the Shift key while clicking on the Close button [🗙]."),
                        Caption = appName + " - " + Lng.T("Tray mode"),
                        Icon = TaskDialogIcon.None,
                        AllowCancel = true,
                        Verification = new TaskDialogVerificationCheckBox() { Text = Lng.T("Do not show again") },
                        Buttons = { TaskDialogButton.OK },
                        Footnote = new TaskDialogFootnote()
                        {
                            Text = Lng.T("TrayModeFootnote", "If the NetRadio icon is unvisible: Click on the ˄ arrow in the taskbar to show all icons and drag the icon to the system tray.\nIn this mode, pressing the Escape key in the main window minimizes the program to the taskbar."),
                        }
                    };
                    if (TaskDialog.ShowDialog(this, page) == TaskDialogButton.OK)
                    {
                        if (page.Verification.Checked) { _settings.ShowTrayInfo = false; }
                    }
                }
            }
            else { _settings.CloseToTray = false; }
            somethingToSave = true;
        }
    }

    private void BtnUpdateSettings_Click(object sender, EventArgs e)
    {
        var prevUpdateIndex = _settings.UpdateIndex;
        TaskDialogPage pageUpdate = new()
        {
            Caption = appName,
            Heading = Lng.T("Automatic Updates"),
            Text = Lng.T("AutomaticUpdatesText", "You will be notified that an update is available to download.\n\nDetection frequency:"),
            AllowCancel = true,
            SizeToContent = true,
            Buttons = { TaskDialogButton.OK, TaskDialogButton.Cancel },
        };
        var rbn0 = pageUpdate.RadioButtons.Add(Lng.T("Every day"));
        var rbn1 = pageUpdate.RadioButtons.Add(Lng.T("Every week"));
        var rbn2 = pageUpdate.RadioButtons.Add(Lng.T("Every month"));
        var rbn3 = pageUpdate.RadioButtons.Add(Lng.T("Never"));
        if (_settings.UpdateIndex == 1) { rbn1.Checked = true; }
        else if (_settings.UpdateIndex == 2) { rbn2.Checked = true; }
        else if (_settings.UpdateIndex == 3) { rbn3.Checked = true; }
        else { rbn0.Checked = true; }
        if (TaskDialog.ShowDialog(this, pageUpdate) == TaskDialogButton.OK)
        {
            _settings.UpdateIndex = rbn3.Checked ? 3 : rbn2.Checked ? 2 : rbn1.Checked ? 1 : 0;
            if (_settings.UpdateIndex != prevUpdateIndex) { somethingToSave = true; }
        }
    }

    private void RbStartMode_CheckedChanged(object sender, EventArgs e)
    {
        if (rbStartModeMain.Focused || rbStartModeMini.Focused || rbStartModeTray.Focused)
        {
            _settings.StartMode = rbStartModeTray.Checked ? 2 : rbStartModeMini.Checked ? 1 : 0;
            somethingToSave = true;
        }
    }

    private static readonly Lock _logLock = new(); // LogEvent wird auch aus BASS-Callbacks/async-Kontexten aufgerufen

    public void CreateLogFile()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(logPath) ?? "");
            if (File.Exists(logPath)) { File.Copy(logPath, Path.ChangeExtension(logPath, ".log.bak"), true); } // Log der letzten Sitzung aufheben
            File.WriteAllText(logPath, string.Empty, Encoding.UTF8); // Datei leeren/anlegen
        }
        catch { /* Logging darf das Programm nie beeinträchtigen */ }
    }

    public void LogEvent(string message)
    {
        try
        {
            lock (_logLock) // parallele Aufrufe (UI-Thread, BASS-Callbacks, PowerMode-Events) serialisieren
            {
                File.AppendAllText(logPath, $"{DateTime.Now:yyyyMMdd-HHmmss.fff} | {message}{Environment.NewLine}", Encoding.UTF8);
            }
        }
        catch { /* Logging darf das Programm nie beeinträchtigen */ }
    }

    private void LoadHistoryBtn_Click(object sender, EventArgs e)
    {
        //SaveHistory(); // Sortiert Liste normal (nach Datum)
        openFileDialog.InitialDirectory = Path.GetDirectoryName(settingsPath);
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
            var files = Directory.GetFiles(Path.GetDirectoryName(settingsPath) ?? "", appName + "_*.csv");
            if (files.Length > 0)
            {
                TaskDialogButton deleteButton = new(Lng.T("&Delete"));
                var heading = files.Length > 1 ? Lng.T("Do you want to delete these files?") : Lng.T("Do you want to delete this file?");
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
        if (numUpDnSaveHistory.Value > 0) { cbLogHistory.Checked = _settings.LogHistory = true; }
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
