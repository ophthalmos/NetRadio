using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NetRadio.cls;

internal static class NativeMethods
{
    public const int WM_SYSCOLORCHANGE = 0x0015;
    public const int WM_KEYDOWN = 0x100; //  WM_SYSKEYDOWN = 0x104;
    public const int WM_KEYUP = 0x101; //  WM_SYSKEYUP = 0x105;
    public const int WM_MOUSEWHEEL = 0x020A;
    public const int KEY_PRESSED = 0x8000;
    public const int WM_NCHITTEST = 0x84;
    public const int HTCLIENT = 0x1;
    public const int HTCAPTION = 0x2;
    public const int WM_HOTKEY = 0x312;
    public const int HOTKEY_ID = 0x002A; // 42;
    public const int VK_SHIFT = 0x10;
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
    //public const int SC_MINIMIZE = 0xF020;
    public const int WM_SYSCOMMAND = 0x112;
    public const int MF_SEPARATOR = 0x800;
    public const int MF_BYPOSITION = 0x400;
    public const int MF_STRING = 0x0;
    public const int IDM_CUSTOMITEM1 = 1000; //public const Int32 IDM_CUSTOMITEM2 = 1001;

    private static nint _hookIDKeyboard = nint.Zero;
    private const int WH_KEYBOARD_LL = 13;
    internal static event KeyEventHandler KeyDown;
    //internal static event KeyEventHandler KeyUp;

    public static readonly uint WM_SHOWNETRADIO = RegisterWindowMessage("WM_SHOWNETRADIO");
    private delegate bool CallBackPtr(int hwnd, int lParam);
    private static CallBackPtr callBackPtr;
    public static List<nint> enumedwindowPtrs = [];
    public static List<Rectangle> enumedwindowRects = [];
    private delegate bool EnumThreadDelegate(nint hWnd, nint lParam);

    //public enum MessageFilterInfo : uint { None = 0, AlreadyAllowed = 1, AlreadyDisAllowed = 2, AllowedHigher = 3 }
    //public enum ChangeWindowMessageFilterExAction : uint { Reset = 0, Allow = 1, DisAllow = 2 }
    private enum KeyStates
    {
        None = 0, Down = 1, Toggled = 2
    }

    private static KeyStates GetKeyState(Keys key)
    {
        var state = KeyStates.None;
        var retVal = GetKeyState((int)key);
        if ((retVal & 0x8000) == 0x8000) { state |= KeyStates.Down; } //If the high-order bit is 1, the key is down otherwise, it is up.
        if ((retVal & 1) == 1) { state |= KeyStates.Toggled; } //If the low-order bit is 1, the key is toggled.
        return state;
    }

    public static bool HitTest(Rectangle ctrlRect, nint ctrlHandle, Point p) //  IntPtr ExcludeWindow
    {
        enumedwindowPtrs.Clear(); // clear results
        enumedwindowRects.Clear();
        callBackPtr = new CallBackPtr(EnumCallBack); // Create callback and start enumeration
        _ = EnumDesktopWindows(nint.Zero, callBackPtr, 0);
        Region r = new(ctrlRect); // Go from last to first window, and substract them from the ctrlRect area
        var StartClipping = false;
        for (var i = enumedwindowRects.Count - 1; i >= 0; i--)
        {
            if (StartClipping) { r.Exclude(enumedwindowRects[i]); } //  && enumedwindowPtrs[i] != ExcludeWindow
            if (enumedwindowPtrs[i] == ctrlHandle) { StartClipping = true; }
        }
        return r.IsVisible(p); // return boolean indicating if point is visible to clipped (truly visible) window
    }

    private static bool EnumCallBack(int hwnd, int lParam)
    {
        if (IsWindow(hwnd) && IsWindowVisible(hwnd) && !IsIconic(hwnd)) // If window is visible and not minimized (isiconic)
        {
            enumedwindowPtrs.Add(hwnd); // add the handle and windowrect to "found windows" collection
            if (GetWindowRect(hwnd, out var rct)) { enumedwindowRects.Add(new Rectangle(rct.Left, rct.Top, rct.Right - rct.Left, rct.Bottom - rct.Top)); } // add rect to list
            else { enumedwindowRects.Add(new Rectangle(0, 0, 0, 0)); } // invalid, make empty rectangle
        }
        return true;
    }

    public static IEnumerable<nint> EnumerateWinHandles(int processId)
    {
        List<nint> handles = [];
        foreach (ProcessThread thread in Process.GetProcessById(processId).Threads)
        {
            EnumThreadWindows(thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, nint.Zero);
        }
        return handles;
    }

    //[DllImport("User32.dll")]
    //public static extern bool GetAsyncKeyState(Keys ArrowKeys);

    [DllImport("user32.dll")]
    public static extern nint GetSystemMenu(nint hWnd, bool bRevert);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool AppendMenu(nint hMenu, int wFlags, int wIDNewItem, string lpNewItem);

    [DllImport("user32.dll")]
    public static extern int GetWindowTextLength(nint hWnd);

    [DllImport("user32.dll")]
    private static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, nint lParam);

    [DllImport("user32.dll")]
    internal static extern bool ShowWindow(nint hWnd, int nCmdShow);

    public static bool IsKeyDown(Keys key)
    {
        return KeyStates.Down == (GetKeyState(key) & KeyStates.Down);
    } // IsKeyToggled(Keys key) { return KeyStates.Toggled == (GetKeyState(key) & KeyStates.Toggled); }


    [DllImport("user32.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall)]
    internal static extern short GetKeyState(int nVirtKey);


    //[DllImport("user32.dll", SetLastError = true)]
    //public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, ChangeWindowMessageFilterExAction action, ref CHANGEFILTERSTRUCT changeInfo);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    public static extern nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

    [DllImport("user32.dll")]
    public static extern int SendMessage(nint hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    public static extern bool ReleaseCapture();

    [DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
    public static extern int SetWindowTheme(nint hWnd, string textSubAppName, string textSubIdList);

    public enum Modifiers : uint
    {
        Alt = 0x0001, Control = 0x0002, Shift = 0x0004, Win = 0x0008
    }

    [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
    public static extern nint CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern nint SetFocus(nint hWnd);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern int MessageBoxTimeout(nint hWnd, string lpText, string lpCaption, uint uType, short wLanguageId, int dwMilliseconds);

    [DllImport("wininet.dll", SetLastError = true)]
    public static extern bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern int GetWindowLong(nint hWnd, int nIndex);

    [DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
    internal static extern string SHGetKnownFolderPath([MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, nint hToken = default);

    [DllImport("user32")]
    public static extern bool PostMessage(nint hwnd, uint msg, nint wparam, nint lparam);

    [DllImport("user32", CharSet = CharSet.Unicode)]
    public static extern uint RegisterWindowMessage(string message);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool RegisterHotKey(nint hWnd, int id, uint fsModifiers, uint vk);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnregisterHotKey(nint hWnd, int id);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
    public static extern nint GetForegroundWindow();

    public static bool VerticalScrollbarVisible(Control ctl)
    {
        var wndStyle = GetWindowLong(ctl.Handle, GWL_STYLE);
        return (wndStyle & WS_VSCROLL) != 0;
    }

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindowVisible(nint hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWindow(nint hWnd);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsIconic(nint hWnd);

    [DllImport("user32.dll")]
    private static extern int EnumDesktopWindows(nint hDesktop, CallBackPtr callPtr, int lPar);

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(nint hWnd, out RECT lpRect);

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
        public nint dwData; // The data to be passed to the receiving application. This member can be IntPtr.Zero.
        public int cbData; // The size, in bytes, of the data pointed to by the lpData member.
        public nint lpData; // The data to be passed to the receiving application. This member can be IntPtr.Zero.
    }

    [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern nint SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, nint hInstance, int threadId);

    [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern bool UnhookWindowsHookEx(nint idHook);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);

    private delegate int LowLevelKeyboardProc(int nCode, nint wParam, nint lParam);

    [StructLayout(LayoutKind.Sequential)]
    private struct KBDLLHOOKSTRUCT
    {
        internal uint vkCode;
        internal uint scanCode;
        internal uint flags;
        internal uint time;
        internal nuint dwExtraInfo;
    }

    private static int LowLevelKeyboardHookProc(int nCode, nint wParam, nint lParam)
    {
        if (nCode >= 0 && wParam == WM_KEYDOWN) // || wParam == WM_SYSKEYUP)) WM_SYSKEYUP is necessary to trap Alt-key combinations
        {
            var kbStruct = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));
            var keys = (Keys)kbStruct.vkCode;
            switch (keys)
            {
                case Keys.MediaPlayPause:
                case Keys.MediaNextTrack:
                case Keys.MediaPreviousTrack:
                case Keys.MediaStop:
                    KeyDown(Application.OpenForms[0], new KeyEventArgs(keys));
                    return 1; // nur diese Anwendung verarbeitet MediaKeys
            }
        }
        else if (nCode >= 0 && wParam == WM_KEYUP)
        {
            switch ((Keys)((KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT))).vkCode)
            {
                case Keys.MediaPlayPause:
                case Keys.MediaNextTrack:
                case Keys.MediaPreviousTrack:
                case Keys.MediaStop:
                    return 1;
            }
        }
        return CallNextHookEx(_hookIDKeyboard, nCode, wParam, lParam);
    }

    internal static nint RegisterMediaKeys()
    {
        return _hookIDKeyboard = SetWindowsHookEx(WH_KEYBOARD_LL, LowLevelKeyboardHookProc, nint.Zero, 0);
    }
    internal static void UnregisterMediaKeys()
    {
        UnhookWindowsHookEx(_hookIDKeyboard);
    }

    [DllImport("shell32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static extern bool IsUserAnAdmin(); // LogEvent

}
