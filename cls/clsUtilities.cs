using Microsoft.Win32; // Registry
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Un4seen.Bass;

namespace NetRadio
{
    class Utilities // internal is standard
    {
        private const string runLocation = @"Software\Microsoft\Windows\CurrentVersion\Run";

        public static readonly List<string> TaskNames = ["Start playing", "Stop playing", "Start recording", "Stop recording", "Put PC to sleep", "Hibernate PC", "Shut down PC"];

        internal static void ErrorMsgTaskDlg(IntPtr hwnd, string message, string caption, TaskDialogIcon taskDialogIcon = null)
        {
            taskDialogIcon ??= TaskDialogIcon.Error;
            TaskDialog.ShowDialog(hwnd, new TaskDialogPage() { Caption = caption, SizeToContent = true, Text = message, Icon = taskDialogIcon, AllowCancel = true, Buttons = { TaskDialogButton.OK } });
        }

        public static bool PingGoogleSuccess(int timeout)
        { // InternetGetConnectedState: This code only checks if the network cable is plugged in
            try { return NativeMethods.InternetGetConnectedState(out _, 0) && new Ping().Send(new IPAddress(new byte[] { 8, 8, 8, 8 }), timeout).Status == IPStatus.Success; }
            catch { return false; } // erforderlich //  && System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()
        }

        public static void SetClipboardUnicodeText(string text)
        {
            Thread thread = new(() => Clipboard.SetText(text, TextDataFormat.UnicodeText));
            thread.SetApartmentState(ApartmentState.STA); //Set the thread to STA
            thread.Start();
            thread.Join();
        }

        public static StringFormat GetStringFormat(HorizontalAlignment ha)
        {
            StringAlignment align = ha switch
            {
                HorizontalAlignment.Right => StringAlignment.Far,
                HorizontalAlignment.Center => StringAlignment.Center,
                _ => StringAlignment.Near,
            };
            return new StringFormat() { Alignment = align, LineAlignment = StringAlignment.Center };
        }

        //public static void TaskDialogMessage(string text, string heading, string caption, TaskDialogIcon icon)
        //{
        //    TaskDialog.ShowDialog(Form.ActiveForm, new TaskDialogPage() { Text = text, Heading = heading, Caption = caption, Buttons = { TaskDialogButton.OK, }, Icon = icon});
        //}

        public static string StationLong(string caption)
        {
            caption = Regex.Replace(caption, @"[\[{].*\||[\[{](.*)[]}]", "$1");
            caption = caption.Replace("[", string.Empty).Replace("]", string.Empty);
            caption = caption.Replace("{", string.Empty).Replace("}", string.Empty);
            return Regex.Replace(caption, @"\s+", " "); // doppelte Leerzeichen entfernen
        }

        public static string StationShort(string s)
        {
            s = Regex.Replace(s, @"[\[{]([^\]|}]*)\|.*[]}]", "$1"); // innerhalb geschweifter Klammern wird Part1 genommen
            s = Regex.Replace(s, @"[\[{][^\]|}]*[]}]", string.Empty); // Text innerhalb eckiger Klammern wird entfernt, Zwischebereich darf keine schließende Klammer enthalten [^\]]*; [ muss maskiert werden, ] nicht
            s = Regex.Replace(s, @"\s+", " "); // doppelte Leerzeichen entfernen
            Match m = Regex.Match(s, @"(.+? .+?) ");
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
                BASSError.BASS_ERROR_FORMAT => "The sample format is not supported.",
                BASSError.BASS_ERROR_SPEAKER => "The specified SPEAKER flags are invalid.",
                BASSError.BASS_ERROR_MEM => "There is insufficient memory.",
                BASSError.BASS_ERROR_NO3D => "Could not initialize 3D support",
                BASSError.BASS_ERROR_NONET => "No internet connection could be opened.",
                BASSError.BASS_ERROR_TIMEOUT => "The server did not respond to the request.",// within the timeout period.";
                BASSError.BASS_ERROR_ILLPARAM => "Illegal Parameter. Url is not a valid URL.",
                BASSError.BASS_ERROR_UNKNOWN => "Unkown Error!",
                BASSError.BASS_ERROR_DRIVER => "Can't find a free/valid driver",
                BASSError.BASS_ERROR_BUFLOST => "The sample buffer was lost",
                BASSError.BASS_ERROR_HANDLE => "Invalid handle",
                BASSError.BASS_ERROR_POSITION => "Invalid playback position",
                BASSError.BASS_ERROR_START => "BASS_Start has not been successfully called",
                BASSError.BASS_ERROR_REINIT => "device needs to be reinitialized",
                BASSError.BASS_ERROR_ALREADY => "Already initialized/paused/whatever",
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
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0) { size = string.Format("{0:##.##}", byteCount / 1073741824.0) + " GB"; }
            else if (byteCount >= 1048576.0) { size = string.Format("{0:##.##}", byteCount / 1048576.0) + " MB"; }
            else if (byteCount >= 1024.0) { size = string.Format("{0:##.##}", byteCount / 1024.0) + " KB"; }
            else if (byteCount > 0 && byteCount < 1024.0) { size = byteCount.ToString() + " Bytes"; }
            return size;
        }

        public static void SetAutoStart(string appName, string assemblyLocation)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(runLocation);
            key.SetValue(appName, assemblyLocation);
        }

        public static bool IsAllLower(string input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsLetter(input[i]) && !char.IsLower(input[i])) { return false; }
            }
            return true;
        }

        public static bool IsAutoStartEnabled(string appName, string assemblyLocation)
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(runLocation);
            if (key == null) return false;
            string value = (string)key.GetValue(appName);
            if (value == null) return false;
            else if (Debugger.IsAttached) { return true; } // run by Visual Studio
            else { return value == assemblyLocation; }
        }

        public static bool IsInnoSetupValid(string assemblyLocation)
        {
            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\NetRadio_is1");
            if (key == null) return false;
            string value = (string)key.GetValue("UninstallString");
            if (value == null) return false;
            else if (Debugger.IsAttached) { return true; } // run by Visual Studio
            else { return assemblyLocation.Equals(RemoveFromEnd(value.Trim('"'), "\\unins000.exe")); } // "C:\Program Files\NetRadio\unins000.exe"
        }

        public static void UnSetAutoStart(string appName)
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(runLocation);
            key.DeleteValue(appName);
        }

        public static string RemoveFromEnd(string str, string toRemove)
        {
            return str.EndsWith(toRemove) ? str[..^toRemove.Length] : str;
        }

        //public static string GetDescription()
        //{
        //    Type clsType = typeof(FrmMain);
        //    Assembly assy = clsType.Assembly;
        //    AssemblyDescriptionAttribute adAttr = (AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assy, typeof(AssemblyDescriptionAttribute));
        //    if (adAttr == null) { return string.Empty; }
        //    return adAttr.Description;
        //}

        public static void WriteCSVRow(StringBuilder result, int itemsCount, Func<int, bool> isColumnNeeded, Func<int, string> columnValue)
        {
            bool isFirstTime = true;
            for (int i = 0; i < itemsCount; i++)
            {
                if (!isColumnNeeded(i)) { continue; }

                if (!isFirstTime) { result.Append(";"); }
                isFirstTime = false;
                result.Append(string.Format("\"{0}\"", columnValue(i)));
            }
            result.AppendLine();
        }

        public static bool IsDGVEmpty(DataGridView gridView)
        {
            bool isEmpty = true;
            for (int row = 0; row < gridView.RowCount - 1; row++)
            {
                for (int col = 0; col < gridView.Columns.Count; col++)
                {
                    if (gridView.Rows[row].Cells[col].Value != null && !String.IsNullOrEmpty(gridView.Rows[row].Cells[col].Value.ToString())) { isEmpty = false; break; }
                }
            }
            return isEmpty;
        }

        public static bool IsDGVRowEmpty(DataGridViewRow row)
        {
            for (int i = 0; i < row.Cells.Count; i++)
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
            int nColumns = 0;
            foreach (ColumnHeader ch in lv.Columns) { if (ch.Width > 0) ++nColumns; }
            if (nColumns == 0) return;
            int cx = (lv.ClientSize.Width - 1) / nColumns;
            int cx0 = (lv.ClientSize.Width - 1) - (cx * (nColumns - 1));
            if ((cx0 <= 0) || (cx <= 0)) return;
            if (bBlockUIUpdate) lv.BeginUpdate();
            bool bFirst = true;
            foreach (ColumnHeader ch in lv.Columns)
            {
                int nCurWidth = ch.Width;
                if (nCurWidth == 0) continue;
                if (bFirst && (nCurWidth == cx0)) { bFirst = false; continue; }
                if (!bFirst && (nCurWidth == cx)) continue;
                ch.Width = (bFirst ? cx0 : cx);
                bFirst = false;
            }
            if (bBlockUIUpdate) lv.EndUpdate();
        }

    }
    //public class VolumeEventArgs(int value) : EventArgs { public int Delta { get; set; } = value; }

}
