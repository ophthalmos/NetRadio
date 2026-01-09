using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32; // Registry
using Un4seen.Bass;

namespace NetRadio.cls;

internal class Utilities // internal is standard
{
    private const string runLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";

    public static readonly List<string> TaskNames = ["Start playing", "Stop playing", "Start recording", "Stop recording", "Put PC to sleep", "Hibernate PC", "Shut down PC"];

    internal static void MsgTaskDialog(IWin32Window? owner, string heading, string message = "", TaskDialogIcon? icon = null)
    {
        icon ??= TaskDialogIcon.Error;
        TaskDialogPage page = new() { Caption = Application.ProductName, Heading = heading, SizeToContent = true, Text = message, Icon = icon, AllowCancel = true, Buttons = { TaskDialogButton.OK } };
        if (owner != null) { TaskDialog.ShowDialog(owner, page); }
        else { TaskDialog.ShowDialog(page); }
    }

    internal static void MsgTaskDialogTimeout(IWin32Window? owner, string heading, string text, int seconds, TaskDialogIcon? icon = null)
    {
        icon ??= TaskDialogIcon.Information;
        var btnOK = TaskDialogButton.OK;
        TaskDialogPage page = new()
        {
            Caption = Application.ProductName,
            Heading = heading,
            Text = $"{text}\n\n(Closing in {seconds} s)", // Initialer Text
            Icon = icon,
            Buttons = { btnOK },
            DefaultButton = btnOK,
            SizeToContent = true
        };
        using System.Windows.Forms.Timer timer = new() { Interval = 1000 };
        var remainingSeconds = seconds;
        page.Created += (s, e) => { timer.Start(); };
        page.Destroyed += (s, e) => { timer.Stop(); };
        timer.Tick += (s, e) =>
        {
            remainingSeconds--;
            if (remainingSeconds <= 0)
            {
                timer.Stop();
                if (page.BoundDialog != null) { btnOK.PerformClick(); }
            }
            else { page.Text = $"{text}\n\n(Closing in {remainingSeconds} s)"; }
        };
        if (owner == null) { TaskDialog.ShowDialog(page); }
        else { TaskDialog.ShowDialog(owner, page); }
    }

    internal static bool IsActionCancelled(IWin32Window? owner, string heading, string text, int seconds, TaskDialogIcon? icon = null)
    {
        icon ??= TaskDialogIcon.Information;
        var btnOK = TaskDialogButton.OK;
        var btnCancel = TaskDialogButton.Cancel;
        TaskDialogPage page = new()
        {
            Caption = Application.ProductName,
            Heading = heading,
            Text = $"{text}\n\n(Auto-confirm in {seconds} s)",
            Icon = icon, // Hier wird das Icon gesetzt
            Buttons = { btnOK, btnCancel },
            DefaultButton = btnCancel,
            SizeToContent = true
        };
        using System.Windows.Forms.Timer timer = new() { Interval = 1000 };
        var remaining = seconds;
        page.Created += (s, e) => timer.Start();
        page.Destroyed += (s, e) => timer.Stop();
        timer.Tick += (s, e) =>
        {
            remaining--;
            if (remaining <= 0)
            {
                timer.Stop();
                if (page.BoundDialog != null) { btnOK.PerformClick(); }
            }
            else { page.Text = $"{text}\n\n(Auto-confirm in {remaining} s)"; }
        };
        var result = owner != null ? TaskDialog.ShowDialog(owner, page) : TaskDialog.ShowDialog(page);
        return result == btnCancel;
    }

    public static void ErrTaskDialog(IWin32Window? owner, Exception error, TaskDialogIcon? icon = null)
    {
        icon ??= TaskDialogIcon.Error;
        TaskDialogButton copyButton = new("Copy Details");
        TaskDialogPage page = new()
        {
            Caption = Application.ProductName,
            Heading = error.GetType().ToString(),
            Text = error.Message,
            Icon = icon,
            Buttons = { TaskDialogButton.OK, copyButton },
            Expander = new TaskDialogExpander()
            {
                Text = $"--- StackTrace ---\n{error}\n\n--- System ---\nOS: {Environment.OSVersion}\nRuntime: {RuntimeInformation.FrameworkDescription}",
                CollapsedButtonText = "Show technical details",
                ExpandedButtonText = "Hide details",
                Position = TaskDialogExpanderPosition.AfterFootnote
            }
        };
        copyButton.Click += (s, e) => { Clipboard.SetText(page.Expander.Text); };
        if (owner is null) { TaskDialog.ShowDialog(page); }
        else { TaskDialog.ShowDialog(owner, page); }
    }

    internal static (bool IsYes, bool IsNo, bool IsCancelled) YesNo_TaskDialog(IWin32Window? owner, string heading, string text, string yes = "", string no = "", bool defBtn = true)
    {
        var yesButton = string.IsNullOrEmpty(yes) ? TaskDialogButton.Yes : new TaskDialogButton(yes);
        var noButton = string.IsNullOrEmpty(no) ? TaskDialogButton.No : new TaskDialogButton(no);
        var page = new TaskDialogPage
        {
            Caption = Application.ProductName,
            Heading = heading,
            Text = text,
            Icon = new TaskDialogIcon(Properties.Resources.question32),
            Buttons = { yesButton, noButton },
            DefaultButton = defBtn ? yesButton : noButton,
            AllowCancel = true,
            SizeToContent = true
        };
        var result = owner is not null ? TaskDialog.ShowDialog(owner, page) : TaskDialog.ShowDialog(page);
        var isYes = result == yesButton;
        var isNo = result == noButton;
        var isCancelled = result == TaskDialogButton.Cancel || (!isYes && !isNo);
        return (isYes, isNo, isCancelled);
    }

    public static void StartFile(IWin32Window? iWin, string filePath)
    {
        try
        {
            ProcessStartInfo psi = new(filePath) { UseShellExecute = true }; // for non-executables
            Process.Start(psi);
        }
        catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { ErrTaskDialog(iWin, ex); }
    }


    public static void StartLink(IWin32Window? iWin, string url)
    {
        try
        {
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                ProcessStartInfo psi = new(url) { UseShellExecute = true };
                Process.Start(psi);
            }
            else { MsgTaskDialog(iWin ?? null, "Ungültiger Link!", "'" + url + "' ist keine gültige URL.", TaskDialogIcon.ShieldWarningYellowBar); }
        }
        catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { ErrTaskDialog(iWin, ex); }
    }

    public static bool PingGoogleSuccess(int timeout)
    { // InternetGetConnectedState: This code only checks if the network cable is plugged in
        try { return NativeMethods.InternetGetConnectedState(out _, 0) && new Ping().Send(new IPAddress([8, 8, 8, 8]), timeout).Status == IPStatus.Success; }
        catch { return false; } // erforderlich //  && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()
    }

    public static void SetClipboardUnicodeText(string text)
    {
        if (string.IsNullOrEmpty(text)) { return; }
        try { Clipboard.SetText(text, TextDataFormat.UnicodeText); }
        catch (ExternalException)
        {
            Thread.Sleep(100);
            Clipboard.SetText(text, TextDataFormat.UnicodeText);
        }
    }

    public static StringFormat GetStringFormat(HorizontalAlignment ha)
    {
        var align = ha switch
        {
            HorizontalAlignment.Right => StringAlignment.Far,
            HorizontalAlignment.Center => StringAlignment.Center,
            _ => StringAlignment.Near,
        };
        return new StringFormat() { Alignment = align, LineAlignment = StringAlignment.Center };
    }

    public static string StationLong(string? caption, bool button = false)
    {
        if (string.IsNullOrEmpty(caption)) { return string.Empty; }
        caption = Regex.Replace(caption, @"[\[{].*\||[\[{](.*)[]}]", "$1");
        caption = caption.Replace("[", string.Empty).Replace("]", string.Empty);
        caption = caption.Replace("{", string.Empty).Replace("}", string.Empty);
        if (button) { caption = caption.Replace("&", "&&"); } // & wird sonst als Akzelerator interpretiert (nächstes Zeichen wird unterstrichen)
        return Regex.Replace(caption, @"\s+", " "); // doppelte Leerzeichen entfernen
    }

    public static string StationShort(string? s, bool button = false)
    {
        if (string.IsNullOrEmpty(s)) { return string.Empty; }
        s = Regex.Replace(s, @"[\[{]([^\]|}]*)\|.*[]}]", "$1"); // innerhalb geschweifter Klammern wird Part1 genommen
        s = Regex.Replace(s, @"[\[{][^\]|}]*[]}]", string.Empty); // Text innerhalb eckiger Klammern wird entfernt, Zwischebereich darf keine schließende Klammer enthalten [^\]]*; [ muss maskiert werden, ] nicht
        s = Regex.Replace(s, @"\s+", " "); // doppelte Leerzeichen entfernen
        if (button) { s = s.Replace("&", "&&"); } // & wird sonst als Akzelerator interpretiert (nächstes Zeichen wird unterstrichen)
        var m = Regex.Match(s, @"(.+? .+?) ");
        if (m.Success) { s = m.Groups[1].Value; } // Text nach dem 2. Leerzeichen wird abgeschnitten
        return s.Trim();
    }

    internal static string GetErrorDescription(BASSError error)
    {
        return error switch
        {
            BASSError.BASS_ERROR_INIT => "BASS_Init has not been successfully called.",
            BASSError.BASS_ERROR_NOTAVAIL => "The BASS_STREAM_AUTOFREE flag cannot be combined with the BASS_STREAM_DECODE flag.",
            BASSError.BASS_ERROR_PROTOCOL => "The protocol in url is not supported",
            BASSError.BASS_ERROR_SSL => "SSL/HTTPS support is not available.See BASS_CONFIG_LIBSSL",
            BASSError.BASS_ERROR_DENIED => "A valid username/ password is required.",
            BASSError.BASS_ERROR_FILEOPEN => "The file could not be opened.",
            BASSError.BASS_ERROR_FILEFORM => "The file's format is not recognised/supported.",
            BASSError.BASS_ERROR_UNSTREAMABLE => "The file cannot be streamed. This could be because an MP4 file's 'mdat' atom comes before its 'moov' atom.",
            BASSError.BASS_ERROR_NOTAUDIO => "The file does not contain audio, or it also contains video and videos are disabled.",
            BASSError.BASS_ERROR_CODEC => "The file uses a codec that is not available/supported.", // This can apply to WAV and AIFF files.
            BASSError.BASS_ERROR_FORMAT => "The specified format is not supported by the device. Try changing the freq and flags parameters.",
            BASSError.BASS_ERROR_SPEAKER => "The specified SPEAKER flags are invalid.",
            BASSError.BASS_ERROR_MEM => "There is insufficient memory.",
            BASSError.BASS_ERROR_NO3D => "Could not initialize 3D support. The device has no 3D support.",
            BASSError.BASS_ERROR_NONET => "No internet connection could be opened.",
            BASSError.BASS_ERROR_TIMEOUT => "The server did not respond to the request.",// within the timeout period.";
            BASSError.BASS_ERROR_ILLPARAM => "Illegal Parameter. Url is not a valid URL.",
            BASSError.BASS_ERROR_UNKNOWN => "Unkown Error!",
            BASSError.BASS_ERROR_DRIVER => "There is no available device driver... the device may already be in use.",
            BASSError.BASS_ERROR_BUFLOST => "The sample buffer was lost",
            BASSError.BASS_ERROR_HANDLE => "Invalid handle",
            BASSError.BASS_ERROR_POSITION => "Invalid playback position",
            BASSError.BASS_ERROR_START => "BASS_Start has not been successfully called",
            BASSError.BASS_ERROR_REINIT => "device needs to be reinitialized",
            BASSError.BASS_ERROR_ALREADY => "The device has already been initialized. You must call BASS_Free() before you can initialize it again.",
            BASSError.BASS_ERROR_NOPAUSE => "Not paused",
            BASSError.BASS_ERROR_NOCHAN => "Can't get a free channel",
            BASSError.BASS_ERROR_ILLTYPE => "An illegal type was specified",
            BASSError.BASS_ERROR_NOEAX => "No EAX support",
            BASSError.BASS_ERROR_DEVICE => "Illegal device number",
            BASSError.BASS_ERROR_NOPLAY => "Not playing",
            BASSError.BASS_ERROR_FREQ => "Illegal sample rate",
            BASSError.BASS_ERROR_NOTFILE => "The stream is not a file stream",
            BASSError.BASS_ERROR_NOHW => "No hardware voices available",
            BASSError.BASS_ERROR_EMPTY => "The MOD music has no sequence data",
            BASSError.BASS_ERROR_CREATE => "Couldn't create the file",
            BASSError.BASS_ERROR_NOFX => "Effects are not available",
            BASSError.BASS_ERROR_PLAYING => "The channel is playing",
            BASSError.BASS_ERROR_DECODE => "The channel is a 'decoding channel'",
            BASSError.BASS_ERROR_DX => "A sufficient DirectX version is not installed",
            BASSError.BASS_ERROR_VERSION => "Invalid BASS version (used by add-ons)",
            BASSError.BASS_ERROR_ENDED => "The channel/file has ended",
            BASSError.BASS_ERROR_BUSY => "The device is busy (eg. in exclusive use by another process)",
            BASSError.BASS_ERROR_SERVER_CERT => "missing/invalid certificate",
            BASSError.BASS_ERROR_MP4_NOSTREAM => "BASS_AAC: non-streamable due to MP4 atom order ('mdat' before 'moov')",
            _ => "Unkown Error",
        };
    }

    public static string GetFileSize(int byteCount)
    {
        var size = "0 Bytes";
        if (byteCount >= 1073741824.0) { size = string.Format("{0:##.##}", byteCount / 1073741824.0) + " GB"; }
        else if (byteCount >= 1048576.0) { size = string.Format("{0:##.##}", byteCount / 1048576.0) + " MB"; }
        else if (byteCount >= 1024.0) { size = string.Format("{0:##.##}", byteCount / 1024.0) + " KB"; }
        else if (byteCount > 0 && byteCount < 1024.0) { size = byteCount.ToString() + " Bytes"; }
        return size;
    }

    public static void SetAutoStart(string appName, string assemblyLocation)
    {
        var key = Registry.CurrentUser.CreateSubKey(runLocation);
        key.SetValue(appName, assemblyLocation);
    }

    public static bool IsAllLower(string input)
    {
        for (var i = 0; i < input.Length; i++)
        {
            if (char.IsLetter(input[i]) && !char.IsLower(input[i])) { return false; }
        }
        return true;
    }

    public static bool IsAutoStartEnabled(string appName, string assemblyLocation)
    {
        var key = Registry.CurrentUser.OpenSubKey(runLocation);
        if (key == null) { return false; }

        var value = (string?)key.GetValue(appName);
        if (value == null) { return false; }
        else if (Debugger.IsAttached) { return true; } // run by Visual Studio
        else { return value == assemblyLocation; }
    }

    public static bool IsInnoSetupValid(string assemblyLocation)
    {
        var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\NetRadio_is1");
        if (key == null) { return false; }
        var value = (string?)key.GetValue("UninstallString");
        if (value == null) { return false; }
        else if (Debugger.IsAttached) { return true; } // run by Visual Studio
        else { return assemblyLocation.Equals(RemoveFromEnd(value.Trim('"'), "\\unins000.exe")); } // "C:\Program Files\NetRadio\unins000.exe"
    }

    public static void UnSetAutoStart(string appName)
    {
        var key = Registry.CurrentUser.CreateSubKey(runLocation);
        key.DeleteValue(appName);
    }

    public static DateTime GetBuildDate()
    { //s. <SourceRevisionId>build$([System.DateTime]::UtcNow.ToString("yyyyMMddHHmmss"))</SourceRevisionId> in ClipMenu.csproj
        const string BuildVersionMetadataPrefix = "+build";
        var attribute = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        if (attribute?.InformationalVersion != null)
        {
            var value = attribute.InformationalVersion;
            var index = value.IndexOf(BuildVersionMetadataPrefix);
            if (index > 0)
            {
                value = value[(index + BuildVersionMetadataPrefix.Length)..];
                if (DateTime.TryParseExact(value, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var result)) { return result; }
            }
        }
        return default;
    }


    public static string RemoveFromEnd(string str, string toRemove)
    {
        return str.EndsWith(toRemove) ? str[..^toRemove.Length] : str;
    }

    //public static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
    //{
    //    bool isFirstTime = true;
    //    for (int i = 0; i < itemsCount; i++)
    //    {
    //        if (!isColumnNeeded(i)) { continue; }

    //        if (!isFirstTime) { result.Append(";"); }
    //        isFirstTime = false;
    //        result.Append(string.Format("\"{0}\"", columnValue(i)));
    //    }
    //    result.AppendLine();
    //}

    //public static void ListView2CsvFile(string filePath, ListView historyListView)
    //{
    //    using StreamWriter sw = new(filePath, false, Encoding.UTF8);
    //    for (int i = 0; i < historyListView.Columns.Count; i++) // Spaltenüberschriften
    //    {
    //        sw.Write($"\"{historyListView.Columns[i].Text}\"");
    //        if (i < historyListView.Columns.Count - 1) { sw.Write(";"); }
    //    }
    //    sw.WriteLine();
    //    foreach (ListViewItem item in historyListView.Items) // Daten aus jedem ListViewItem
    //    {
    //        for (int i = 0; i < item.SubItems.Count; i++)
    //        {
    //            if (i == 0) { sw.Write($"\"{DateTime.ParseExact(item.Tag.ToString(), "s", CultureInfo.InvariantCulture):yyyyMMdd-HH:mm:ss}\""); }
    //            else { sw.Write($"\"{item.SubItems[i].Text}\""); }
    //            if (i < item.SubItems.Count - 1) { sw.Write(";"); }
    //        }
    //        sw.WriteLine();
    //    }
    //}

    public static void SortHistoryNormal(ListView historyListView, CListViewItemComparer lviComparer, string[] lvSortOrderArray)
    {
        historyListView.ListViewItemSorter = lviComparer;
        lviComparer.SortColumn = 0;
        lviComparer.Order = SortOrder.Descending;
        historyListView.Refresh(); // Arrows auf anderen ColumnHeader-Buttons werden entfernt
        historyListView.Sort(); // MessageBox.Show(historyListView.TopItem.Tag.ToString()); // 2023-04-14T12:59:06.3463796Z
        lvSortOrderArray[0] = "Descending";
    }

    public static bool IsDGVEmpty(DataGridView gridView)
    {
        var isEmpty = true;
        for (var row = 0; row < gridView.RowCount - 1; row++)
        {
            for (var col = 0; col < gridView.Columns.Count; col++)
            {
                if (gridView.Rows[row].Cells[col].Value != null && !string.IsNullOrEmpty(gridView.Rows[row].Cells[col].Value.ToString())) { isEmpty = false; break; }
            }
        }
        return isEmpty;
    }

    public static bool IsDGVRowEmpty(DataGridViewRow row)
    {
        for (var i = 0; i < row.Cells.Count; i++)
        {
            if (row.Cells[i].Value != null)
            {
                if (!string.IsNullOrWhiteSpace(row.Cells[i].Value.ToString())) { return false; }
            }
        }
        return true;
    }

    public static void ResizeColumns(ListView lv, bool bBlockUIUpdate)
    {// KeePass\UI\UIUtil.cs
        if (lv == null) { return; }
        var nColumns = 0;
        foreach (ColumnHeader ch in lv.Columns)
        {
            if (ch.Width > 0) { ++nColumns; }
        }
        if (nColumns == 0) { return; }
        var cx = (lv.ClientSize.Width - 1) / nColumns;
        var cx0 = lv.ClientSize.Width - 1 - cx * (nColumns - 1);
        if (cx0 <= 0 || cx <= 0) { return; }
        if (bBlockUIUpdate) { lv.BeginUpdate(); }

        var bFirst = true;
        foreach (ColumnHeader ch in lv.Columns)
        {
            var nCurWidth = ch.Width;
            if (nCurWidth == 0) { continue; }
            if (bFirst && nCurWidth == cx0) { bFirst = false; continue; }
            if (!bFirst && nCurWidth == cx) { continue; }
            ch.Width = bFirst ? cx0 : cx;
            bFirst = false;
        }
        if (bBlockUIUpdate) { lv.EndUpdate(); }
    }

}
//public class VolumeEventArgs(int value) : EventArgs { public int Delta { get; set; } = value; }
