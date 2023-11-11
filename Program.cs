using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading; // Mutex
using System.Windows.Forms;
using Un4seen.Bass;

namespace NetRadio
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            using Mutex singleMutex = new(true, "{8F4J0AC4-WH29-57GD-A8CF-72F04E6BDE8F}", out bool isNewInstance);
            if (isNewInstance)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                string dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\bass.dll";
                try
                {
                    if (File.Exists(dllPath))
                    {
                        BassNet.Registration("happe.kiel@web.de", "2X313517322323");
                        if (Utils.HighWord(Bass.BASS_GetVersion()) != Bass.BASSVERSION) { MessageBox.Show("Wrong Bass Version!", "NetRadio", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                        Application.Run(new FrmMain());
                    }
                    else { MessageBox.Show("Missing \"" + dllPath + "\"!", "NetRadio", MessageBoxButtons.OK, MessageBoxIcon.Error); }
                }
                catch (ArgumentException ex) { MessageBox.Show(ex.Message + Environment.NewLine + "NetRadio will exit for security reasons.", "NetRadio", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else
            {
                string[] args = Environment.GetCommandLineArgs().Skip(1).ToArray();
                if (args.Length > 0)
                {
                    IntPtr ptrCopyData = IntPtr.Zero;
                    string arguments = string.Join('|', args);
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
                        foreach (IntPtr handle in NativeMethods.EnumerateWinHandles(Process.GetProcessesByName(Assembly.GetEntryAssembly().GetName().Name)
                            .Where(p => p.Id != Environment.ProcessId).FirstOrDefault().Id)) // process.MainWindowHandle funktioniert nicht wenn Hidden
                        { //(Assembly.GetCallingAssembly().GetName().Name + " " + new Regex(@"^\d+\.\d+").Match(_curVersion.ToString()).Value).Length = 12
                            if (NativeMethods.GetWindowTextLength(handle) == 12) { NativeMethods.SendMessage(handle, NativeMethods.WM_COPYDATA, IntPtr.Zero, ptrCopyData); }
                        } //Prüfung auf != IntPtr.Zero bringt nichts
                    }
                    catch { } //{ MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error); }
                    finally
                    {
                        if (ptrCopyData != IntPtr.Zero) { Marshal.FreeCoTaskMem(ptrCopyData); } // Free the allocated memory after the control has been returned
                    }
                }
                else { NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, NativeMethods.WM_SHOWNETRADIO, IntPtr.Zero, IntPtr.Zero); } // make the currently running instance jump on top of all the other windows
            }
        }
    }
}
