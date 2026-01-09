using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading; // Mutex
using System.Windows.Forms;
using NetRadio.cls;
using Un4seen.Bass;

namespace NetRadio;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        using Mutex singleMutex = new(true, "{8F4J0AC4-WH29-57GD-A8CF-72F04E6BDE8F}", out var isNewInstance);
        if (isNewInstance)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\bass.dll";
            try
            {
                if (File.Exists(dllPath))
                {
                    BassNet.Registration("happe.kiel@web.de", "2X313517322323");
                    if (Utils.HighWord(Bass.BASS_GetVersion()) < Bass.BASSVERSION) {Utilities.MsgTaskDialog(null, "Wrong Bass Version!"); }
                    Application.Run(new FrmMain());
                }
                else { Utilities.MsgTaskDialog(null, dllPath, "The required BASS library file is missing from the application folder." + Environment.NewLine + "Please reinstall NetRadio to fix this issue.", TaskDialogIcon.Error); }
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
                        dwData = new IntPtr(2),    // Just a number to identify the data type
                        cbData = Encoding.Unicode.GetBytes(arguments).Length + 1,  // One extra byte for the \0 character
                        lpData = Marshal.StringToHGlobalUni(arguments) // Create the data structure and fill with data
                    };
                    ptrCopyData = Marshal.AllocCoTaskMem(Marshal.SizeOf(copyData)); // Allocate memory for the data and copy
                    Marshal.StructureToPtr(copyData, ptrCopyData, false);

                    var entryAssembly = Assembly.GetEntryAssembly();
                    if (entryAssembly != null)
                    {
                        var otherProcess = Process.GetProcessesByName(entryAssembly.GetName().Name).FirstOrDefault(p => p.Id != Environment.ProcessId);
                        if (otherProcess != null)
                        {
                            foreach (var handle in NativeMethods.EnumerateWinHandles(otherProcess.Id))
                            {
                                if (NativeMethods.GetWindowTextLength(handle) == 12) { NativeMethods.SendMessage(handle, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData); }
                            }
                        }
                    }
                }
                catch { } //{ MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                finally
                {
                    if (ptrCopyData != IntPtr.Zero) { Marshal.FreeCoTaskMem(ptrCopyData); } // Free the allocated memory after the control has been returned
                }
            }
            else { NativeMethods.PostMessage(NativeMethods.HWND_BROADCAST, NativeMethods.WM_SHOWNETRADIO, IntPtr.Zero, IntPtr.Zero); } // make the currently running instance jump on top of all the other windows
        }
    }
}
