using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading; // Mutex
using System.Windows.Forms;
using NetRadio.cls;
using Un4seen.Bass;

namespace NetRadio;

internal static partial class Program
{
    static partial void RegisterBass();

    private static Mutex? _singleMutex; // hält die Single-Instance-Sperre bis zum Prozessende (das OS räumt das Handle beim Beenden ab)

    /// <summary>Gibt den Single-Instance-Mutex frei — nötig vor Application.Restart(), denn die neue
    /// Instanz startet, bevor der alte Prozess (und damit der Mutex) verschwunden ist; sie würde sich
    /// sonst für eine Zweitinstanz halten und sofort wieder beenden.</summary>
    internal static void ReleaseSingleInstanceMutex()
    {
        _singleMutex?.Dispose();
        _singleMutex = null;
    }

    [STAThread]
    private static void Main()
    {
        _singleMutex = new Mutex(true, "{8F4J0AC4-WH29-57GD-A8CF-72F04E6BDE8F}", out var isNewInstance);
        if (isNewInstance)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\bass.dll";
            try
            {
                if (File.Exists(dllPath))
                {
                    RegisterBass();  // BassNet.Registration("abc@xyz.com", "01234567890");
                    if (Utils.HighWord(Bass.BASS_GetVersion()) < Bass.BASSVERSION) { Utilities.MsgTaskDialog(null, "Wrong Bass Version!"); }
                    Application.Run(new FrmMain());
                }
                else
                {
                    Utilities.MsgTaskDialog(null, dllPath, "The required BASS library file is missing from the application folder." + Environment.NewLine + "Please reinstall NetRadio to fix this issue.", TaskDialogIcon.Error);
                }
            }
            catch (ArgumentException ex) { Utilities.ErrTaskDialog(null, ex); }
            catch (DllNotFoundException ex) { Utilities.ErrTaskDialog(null, ex); }
            catch (BadImageFormatException ex) { Utilities.ErrTaskDialog(null, ex); }
        }
        else
        {
            var args = Environment.GetCommandLineArgs().Skip(1).ToArray();
            if (args.Length > 0)
            {
                var ptrCopyData = IntPtr.Zero;
                var arguments = string.Join('|', args);
                try
                {
                    NativeMethods.COPYDATASTRUCT copyData = new()
                    {
                        dwData = new IntPtr(2),
                        cbData = (arguments.Length + 1) * 2,
                        lpData = Marshal.StringToHGlobalUni(arguments)
                    };
                    ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData));
                    Marshal.StructureToPtr(copyData, ptrCopyData, false);

                    var entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        var otherProcess = Process.GetProcessesByName(entryAssembly.GetName().Name).FirstOrDefault(p => p.Id != Environment.ProcessId);
                        if (otherProcess != null)
                        {
                            foreach (var handle in NativeMethods.EnumerateWinHandles(otherProcess.Id)) { NativeMethods.SendMessage(handle, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData); }
                        }
                    }
                }
                catch { }
                finally
                {
                    if (ptrCopyData != IntPtr.Zero) { Marshal.FreeCoTaskMem(ptrCopyData); }
                }
            }
            else { NativeMethods.PostMessage(NativeMethods.HWND_BROADCAST, NativeMethods.WM_SHOWNETRADIO, IntPtr.Zero, IntPtr.Zero); }
        }
    }
}
