using System;
using System.IO;
using System.Threading; // Mutex
using System.Windows.Forms;
using System.Reflection;
using Un4seen.Bass;

namespace NetRadio
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static readonly Mutex mutex = new(true, "{8F4J0AC4-WH29-57GD-A8CF-72F04E6BDE8F}");
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
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
                mutex.ReleaseMutex();
            }
            else { NativeMethods.PostMessage((IntPtr)NativeMethods.HWND_BROADCAST, NativeMethods.WM_SHOWME, IntPtr.Zero, IntPtr.Zero); } // send our Win32 message to make the currently running instance; jump on top of all the other windows
        }
    }
}
