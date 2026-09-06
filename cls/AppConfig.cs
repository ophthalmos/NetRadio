using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;

namespace NetRadio.cls;

public sealed class AppSettings
{
    public string Language { get; set; } = "en"; // Kultur-Code der Oberflächensprache ("en", "de", "es", "fr")
    public bool HotkeyEnabled
    {
        get; set;
    }
    public string HotkeyLetter { get; set; } = string.Empty;
    public string OutputDevice { get; set; } = "Default";
    public bool AlwaysOnTop
    {
        get; set;
    }
    public bool CloseToTray
    {
        get; set;
    }
    public bool BalloonTips { get; set; } = true;
    public bool LogHistory { get; set; } = true;
    public bool ShowTrayInfo { get; set; } = true;
    public bool AutoStopRecording
    {
        get; set;
    }
    public int Volume { get; set; } = 50;
    public int SaveHistory
    {
        get; set;
    }
    public int StartMode
    {
        get; set;
    } // 0 = Main, 1 = Mini, 2 = Tray
    public int UpdateIndex
    {
        get; set;
    }
    public DateTime LastUpdateSearch
    {
        get; set;
    }
    public int? FormPosX
    {
        get; set;
    }
    public int? FormPosY
    {
        get; set;
    }
    public int? FormWidth
    {
        get; set;
    }
    public int? FormHeight
    {
        get; set;
    }
    public int? MiniPosX
    {
        get; set;
    }
    public int? MiniPosY
    {
        get; set;
    }
    public int AutostartStation
    {
        get; set;
    } // 0 = kein Autostart
    public bool RepeatActionsDaily
    {
        get; set;
    }
    public List<ActionTask> Actions { get; set; } = [];
    public bool ExperimentalFeatures  // false ist Default
    {
        get; set;
    }
}

public sealed class ActionTask
{
    public bool Enabled
    {
        get; set;
    }
    public string Task { get; set; } = string.Empty;
    public string Station { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
}

public sealed class StationEntry
{
    public int Number
    {
        get; set;
    }
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public static class JsonConfig
{
    private static readonly JsonSerializerOptions _options = new()
    {
        WriteIndented = true,
        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping  // URLs/Umlaute lesbar statt \uXXXX
    };

    public static T? Load<T>(string path) where T : class
    {
        if (!File.Exists(path)) { return null; }
        // Frisch geschriebene Dateien können kurzzeitig gesperrt sein (Virenscanner, Cloud-Sync, Indexer) —
        // z. B. beim Sprachwechsel-Neustart, wenn die alte Instanz die Datei Millisekunden zuvor ersetzt hat.
        // -> bis zu 4 Versuche mit wachsender Pause (symmetrisch zu Save); erst danach fliegt die IOException zum Aufrufer.
        const int maxAttempts = 4;
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                return JsonSerializer.Deserialize<T>(File.ReadAllText(path), _options);
            }
            catch (IOException) when (attempt < maxAttempts)
            {
                System.Threading.Thread.Sleep(100 * attempt); // 100/200/300 ms
            }
        }
    }

    public static void Save<T>(string path, T value)
    {
        var dir = Path.GetDirectoryName(path);
        if (!string.IsNullOrEmpty(dir)) { Directory.CreateDirectory(dir); }
        var tmpPath = path + ".tmp";  // Abbruch mitten im Schreiben (z. B. Windows-Shutdown) kann die bestehende Datei nicht zerstören, da File.Move atomar ersetzt.
        File.WriteAllText(tmpPath, JsonSerializer.Serialize(value, _options));

        // Zieldatei kann kurzzeitig gesperrt sein (Virenscanner, Cloud-Sync, Indexer, geöffneter Editor)
        // -> bis zu 4 Versuche mit wachsender Pause; erst danach fliegt die IOException zum Aufrufer (Dialog + Log).
        const int maxAttempts = 4;
        for (var attempt = 1; ; attempt++)
        {
            try
            {
                File.Move(tmpPath, path, overwrite: true); // MoveFileEx(REPLACE_EXISTING) - atomar, robuster als File.Replace
                return;
            }
            catch (IOException) when (attempt < maxAttempts)
            {
                System.Threading.Thread.Sleep(100 * attempt); // 100/200/300 ms
            }
        }
    }
}

public static class ConfigMigration  // Einmalige Migration der alten NetRadio.xml nach JSON. Kann entfernt werden, sobald keine XML-Configs mehr im Umlauf sind.
{
    private const string LongDateFormat = "yyyyMMddHHmmssfff"; // Format der alten XML (lastUpdateTime)

    public static (AppSettings Settings, List<StationEntry> Stations)? FromXml(string xmlPath)  // wenn JSON-Datei noch nicht existiert oder defkt ist
    {
        if (!File.Exists(xmlPath)) { return null; }
        XElement? root;
        try { root = XDocument.Load(xmlPath).Root; }
        catch (System.Xml.XmlException) { return null; }
        if (root is null) { return null; }

        static string? Str(XElement? el, string attr) => (string?)el?.Attribute(attr);
        static bool Flag(XElement? el) => Str(el, "Enabled") == "1";
        static int? Int(XElement? el, string attr) => int.TryParse(Str(el, attr), out var i) ? i : null;

        var hotkey = root.Element("Hotkey");
        var formLoc = root.Element("FormLocation");
        var miniLoc = root.Element("MiniLocation");
        AppSettings settings = new()
        {
            HotkeyEnabled = Flag(hotkey),
            HotkeyLetter = Str(hotkey, "Letter") ?? string.Empty,
            OutputDevice = Str(root.Element("Output"), "Device") is { Length: > 0 } device ? device : "Default",
            AlwaysOnTop = Flag(root.Element("AlwaysOnTop")),
            CloseToTray = Flag(root.Element("CloseToTray")),
            BalloonTips = Flag(root.Element("BalloonTips")),
            LogHistory = !(root.Element("LogHistory") is { } logHistory) || Flag(logHistory),
            ShowTrayInfo = !(root.Element("ShowTrayInfo") is { } trayInfo) || Flag(trayInfo),
            AutoStopRecording = Flag(root.Element("AutoStopRecording")),
            Volume = Math.Clamp(Int(root.Element("Volume"), "Value") ?? 100, 0, 100),
            SaveHistory = Int(root.Element("SaveHistory"), "Value") ?? 0,
            StartMode = Int(root.Element("StartMode"), "Value") ?? 0,
            UpdateIndex = Int(root.Element("UpdateIndex"), "Value") ?? 0,
            LastUpdateSearch = DateTime.TryParseExact(Str(root.Element("UpdateSearch"), "DateTime"), LongDateFormat, null, DateTimeStyles.None, out var date) ? date : DateTime.UtcNow,
            FormPosX = Int(formLoc, "PosX"),
            FormPosY = Int(formLoc, "PosY"),
            FormWidth = Int(formLoc, "Width"),
            FormHeight = Int(formLoc, "Height"),
            MiniPosX = Int(miniLoc, "PosX"),
            MiniPosY = Int(miniLoc, "PosY"),
            AutostartStation = Int(root.Element("Autostart"), "Station") ?? 0,
            RepeatActionsDaily = Flag(root.Element("RepeatActionsDaily")),
            Actions = [.. root.Elements("Action").Select(static a => new ActionTask
            {
                Enabled = bool.TryParse(Str(a, "Enabled"), out var enabled) && enabled, // alte XML: "True"/"False"
                Task = Str(a, "Task") ?? string.Empty,
                Station = Str(a, "Station") ?? string.Empty,
                Time = Str(a, "Time") ?? string.Empty,
            })],
        };

        List<StationEntry> stations = [.. root.Elements("Station")
            .Select(static (st, i) => new StationEntry
            {
                Number = i + 1, // alte XML: Position = Stationsnummer
                Name = Str(st, "Name") ?? string.Empty,
                Url = Str(st, "URL") ?? string.Empty,
            })
            .Where(static st => st.Name.Length > 0 || st.Url.Length > 0)]; // leere Positionen nicht übernehmen

        return (settings, stations);
    }
}
