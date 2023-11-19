using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NetRadio
{
    internal static class NativeMethods
    {
        public const int WM_KEYDOWN = 0x100; //  WM_SYSKEYDOWN = 0x104;
        public const int WM_KEYUP = 0x101; //  WM_SYSKEYUP = 0x105;
        public const int KEY_PRESSED = 0x8000;
        public const int WM_NCHITTEST = 0x84;
        public const int HTCLIENT = 0x1;
        public const int HTCAPTION = 0x2;
        public const int WM_HOTKEY = 0x312;
        public const int HOTKEY_ID = 0x0312; // 0; // 42;
        public const int WM_QUERYENDSESSION = 0x0011;
        public const int WM_SETCURSOR = 0x0020;
        public const int WM_CLOSE = 0x10;
        public const int WS_VSCROLL = 0x00200000; // WS_HSCROLL = 0x00100000;
        public const int GWL_STYLE = -16;
        public const int LVM_FIRST = 0x1000;
        public const int LVM_SCROLL = LVM_FIRST + 20;
        public const int HWND_BROADCAST = 0xffff;
        public const int VK_CONTROL = 0x11;
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int WM_NCLBUTTONDBLCLK = 0x00A3;
        public const int SW_SHOWNOACTIVATE = 4; // similar to SW_SHOWNORMAL, except that the window is not activated.
        public const int HT_CAPTION = 0x2;
        public const int WM_COPYDATA = 0x004A;

        public const int WM_SYSCOMMAND = 0x112;
        public const int MF_SEPARATOR = 0x800;
        public const int MF_BYPOSITION = 0x400;
        public const int MF_STRING = 0x0;
        public const int IDM_CUSTOMITEM1 = 1000; //public const Int32 IDM_CUSTOMITEM2 = 1001;

        public static readonly int WM_SHOWNETRADIO = RegisterWindowMessage("WM_SHOWNETRADIO");
        private delegate bool CallBackPtr(int hwnd, int lParam);
        private static CallBackPtr callBackPtr;
        public static List<IntPtr> enumedwindowPtrs = new();
        public static List<Rectangle> enumedwindowRects = new();
        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        //public enum MessageFilterInfo : uint { None = 0, AlreadyAllowed = 1, AlreadyDisAllowed = 2, AllowedHigher = 3 }
        //public enum ChangeWindowMessageFilterExAction : uint { Reset = 0, Allow = 1, DisAllow = 2 }
        private enum KeyStates { None = 0, Down = 1, Toggled = 2 }

        private static KeyStates GetKeyState(Keys key)
        {
            KeyStates state = KeyStates.None;
            short retVal = GetKeyState((int)key);
            if ((retVal & 0x8000) == 0x8000) { state |= KeyStates.Down; } //If the high-order bit is 1, the key is down otherwise, it is up.
            if ((retVal & 1) == 1) { state |= KeyStates.Toggled; } //If the low-order bit is 1, the key is toggled.
            return state;
        }

        public static bool HitTest(Rectangle ctrlRect, IntPtr ctrlHandle, Point p) //  IntPtr ExcludeWindow
        {
            enumedwindowPtrs.Clear(); // clear results
            enumedwindowRects.Clear();
            callBackPtr = new CallBackPtr(EnumCallBack); // Create callback and start enumeration
            EnumDesktopWindows(IntPtr.Zero, callBackPtr, 0);
            Region r = new(ctrlRect); // Go from last to first window, and substract them from the ctrlRect area
            bool StartClipping = false;
            for (int i = enumedwindowRects.Count - 1; i >= 0; i--)
            {
                if (StartClipping) { r.Exclude(enumedwindowRects[i]); } //  && enumedwindowPtrs[i] != ExcludeWindow
                if (enumedwindowPtrs[i] == ctrlHandle) StartClipping = true;
            }
            return r.IsVisible(p); // return boolean indicating if point is visible to clipped (truly visible) window
        }

        private static bool EnumCallBack(int hwnd, int lParam)
        {
            if (IsWindow((IntPtr)hwnd) && IsWindowVisible((IntPtr)hwnd) && !IsIconic((IntPtr)hwnd)) // If window is visible and not minimized (isiconic)
            {
                enumedwindowPtrs.Add((IntPtr)hwnd); // add the handle and windowrect to "found windows" collection
                if (GetWindowRect((IntPtr)hwnd, out RECT rct)) { enumedwindowRects.Add(new Rectangle(rct.Left, rct.Top, rct.Right - rct.Left, rct.Bottom - rct.Top)); } // add rect to list
                else { enumedwindowRects.Add(new Rectangle(0, 0, 0, 0)); } // invalid, make empty rectangle
            }
            return true;
        }

        public static IEnumerable<IntPtr> EnumerateWinHandles(int processId)
        {
            List<IntPtr> handles = new();
            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
            {
                EnumThreadWindows(thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            }
            return handles;
        }

        //[DllImport("User32.dll")]
        //public static extern bool GetAsyncKeyState(Keys ArrowKeys);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern bool AppendMenu(IntPtr hMenu, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);

        [DllImport("user32.dll")]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        public static bool IsKeyDown(Keys key) { return KeyStates.Down == (GetKeyState(key) & KeyStates.Down); } // IsKeyToggled(Keys key) { return KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled); }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetKeyState(int key);

        //[DllImport("user32.dll", SetLastError = true)]
        //public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, ChangeWindowMessageFilterExAction action, ref CHANGEFILTERSTRUCT changeInfo);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string textSubAppName, string textSubIdList);

        public enum Modifiers : uint { Alt = 0x0001, Control = 0x0002, Shift = 0x0004, Win = 0x0008 }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        public static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int MessageBoxTimeout(IntPtr hWnd, String lpText, String lpCaption, uint uType, Int16 wLanguageId, Int32 dwMilliseconds);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
        internal static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken = default);

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32", CharSet = CharSet.Unicode)]
        public static extern int RegisterWindowMessage(string message);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        public static bool VerticalScrollbarVisible(Control ctl)
        {
            int wndStyle = GetWindowLong(ctl.Handle, GWL_STYLE);
            return (wndStyle & WS_VSCROLL) != 0;
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int EnumDesktopWindows(IntPtr hDesktop, CallBackPtr callPtr, int lPar);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;        // x position of upper-left corner
            public int Top;         // y position of upper-left corner
            public int Right;       // x position of lower-right corner
            public int Bottom;      // y position of lower-right corner
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData; // The data to be passed to the receiving application. This member can be IntPtr.Zero.
            public int cbData; // The size, in bytes, of the data pointed to by the lpData member.
            public IntPtr lpData; // The data to be passed to the receiving application. This member can be IntPtr.Zero.
        }

        //[StructLayout(LayoutKind.Sequential)]
        //public struct CHANGEFILTERSTRUCT
        //{
        //    public uint size;
        //    public MessageFilterInfo info;
        //}
    }
}
