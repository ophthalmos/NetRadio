using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NetRadio.cls;

internal static partial class NativeMethods
{
    public const int WM_SYSCOLORCHANGE = 0x0015;
    public const int WM_KEYDOWN = 0x100;
    public const int WM_KEYUP = 0x101;
    public const int WM_MOUSEWHEEL = 0x020A;
    public const int KEY_PRESSED = 0x8000;
    public const int WM_NCHITTEST = 0x84;
    public const int HTCLIENT = 0x1;
    public const int HTCAPTION = 0x2;
    public const int WM_HOTKEY = 0x312;
    public const int HOTKEY_ID = 0x002A;
    public const int VK_SHIFT = 0x10;
    public const int WM_QUERYENDSESSION = 0x0011;
    public const int WM_SETCURSOR = 0x0020;
    public const int WM_CLOSE = 0x10;
    public const int WS_VSCROLL = 0x00200000;
    public const int GWL_STYLE = -16;
    public const int LVM_FIRST = 0x1000;
    public const int LVM_SCROLL = LVM_FIRST + 20;
    public const int HWND_BROADCAST = 0xffff;
    public const int VK_CONTROL = 0x11;
    public const uint WM_NCLBUTTONDOWN = 0x00A1;
    public const nint HT_CAPTION = 2; // nint, da wParam in SendMessage ein Pointer-Typ ist
    public const int WM_NCLBUTTONDBLCLK = 0x00A3;
    public const int SW_SHOWNOACTIVATE = 4;
    public const int WM_COPYDATA = 0x004A;
    public const int WM_SYSCOMMAND = 0x112;
    public const int MF_SEPARATOR = 0x800;
    public const int MF_BYPOSITION = 0x400;
    public const int MF_STRING = 0x0;
    public const int IDM_CUSTOMITEM1 = 1000;

    private static nint _hookIDKeyboard = nint.Zero;
    private const int WH_KEYBOARD_LL = 13;
    public static event KeyEventHandler? KeyDown;

    public static readonly uint WM_SHOWNETRADIO = RegisterWindowMessage("WM_SHOWNETRADIO");
    private delegate bool CallBackPtr(int hwnd, int lParam);
    public static List<nint> enumedwindowPtrs = [];
    public static List<Rectangle> enumedwindowRects = [];
    private delegate bool EnumThreadDelegate(nint hWnd, nint lParam);

    private enum KeyStates
    {
        None = 0, Down = 1, Toggled = 2
    }

    private static KeyStates GetKeyState(Keys key)
    {
        var state = KeyStates.None;
        var retVal = GetKeyState((int)key);
        if ((retVal & 0x8000) == 0x8000) { state |= KeyStates.Down; }
        if ((retVal & 1) == 1) { state |= KeyStates.Toggled; }
        return state;
    }

    public static unsafe bool HitTest(Rectangle ctrlRect, nint ctrlHandle, Point p)
    {
        enumedwindowPtrs.Clear();
        enumedwindowRects.Clear();
        _ = EnumDesktopWindows(nint.Zero, &EnumCallBack, nint.Zero);  // Direkte Übergabe des Methoden-Pointers mit dem Adressoperator &
        var r = new Region(ctrlRect);
        var startClipping = false;
        for (var i = enumedwindowRects.Count - 1; i >= 0; i--)
        {
            if (startClipping) { r.Exclude(enumedwindowRects[i]); }
            if (enumedwindowPtrs[i] == ctrlHandle) { startClipping = true; }
        }
        return r.IsVisible(p);
    }

    [UnmanagedCallersOnly]
    private static int EnumCallBack(nint hwnd, nint lParam)
    {
        if (IsWindow(hwnd) && IsWindowVisible(hwnd) && !IsIconic(hwnd))
        {
            enumedwindowPtrs.Add(hwnd);
            if (GetWindowRect(hwnd, out var rct)) { enumedwindowRects.Add(new Rectangle(rct.Left, rct.Top, rct.Right - rct.Left, rct.Bottom - rct.Top)); }
            else { enumedwindowRects.Add(new Rectangle(0, 0, 0, 0)); }
        }
        return 1; // 1 = Win32 TRUE (weiter enumerieren)
    }

    public static unsafe IEnumerable<nint> EnumerateWinHandles(int processId)
    {
        var handles = new List<nint>();
        var gch = GCHandle.Alloc(handles); // Liste vor dem Garbage Collector verstecken
        try
        {
            foreach (ProcessThread thread in Process.GetProcessById(processId).Threads) { EnumThreadWindows(thread.Id, &EnumThreadWindowCallback, GCHandle.ToIntPtr(gch)); }
        }
        finally { gch.Free(); }  // Speicherleck verhindern
        return handles;
    }

    [UnmanagedCallersOnly]
    private static int EnumThreadWindowCallback(nint hWnd, nint lParam)
    {
        var gch = GCHandle.FromIntPtr(lParam);
        if (gch.Target is List<nint> list) { list.Add(hWnd); }
        return 1; // 1 = Win32 TRUE
    }

    [LibraryImport("user32.dll")]
    public static partial nint GetSystemMenu(nint hWnd, [MarshalAs(UnmanagedType.Bool)] bool bRevert);

    [LibraryImport("user32.dll", EntryPoint = "AppendMenuW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool AppendMenu(nint hMenu, int wFlags, int wIDNewItem, string lpNewItem);

    [LibraryImport("user32.dll")]
    public static partial int GetWindowTextLength(nint hWnd);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static unsafe partial bool EnumThreadWindows(int dwThreadId, delegate* unmanaged<nint, nint, int> lpfn, nint lParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ShowWindow(nint hWnd, int nCmdShow);

    public static bool IsKeyDown(Keys key) => KeyStates.Down == (GetKeyState(key) & KeyStates.Down);

    [LibraryImport("user32.dll")]
    internal static partial short GetKeyState(int nVirtKey);

    [LibraryImport("user32.dll", EntryPoint = "SendMessageW", StringMarshalling = StringMarshalling.Utf16)]
    public static partial nint SendMessage(nint hWnd, uint Msg, nint wParam, nint lParam);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool ReleaseCapture();

    [LibraryImport("uxtheme.dll", StringMarshalling = StringMarshalling.Utf16)]
    public static partial int SetWindowTheme(nint hWnd, string textSubAppName, string textSubIdList);

    public enum Modifiers : uint
    {
        Alt = 0x0001, Control = 0x0002, Shift = 0x0004, Win = 0x0008
    }

    [LibraryImport("Gdi32.dll")]
    public static partial nint CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

    [LibraryImport("user32.dll", SetLastError = true)]
    public static partial nint SetFocus(nint hWnd);

    // MessageBoxTimeoutW explizit angegeben, da es eine undokumentierte Funktion ist
    [LibraryImport("user32.dll", EntryPoint = "MessageBoxTimeoutW", StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
    public static partial int MessageBoxTimeout(nint hWnd, string lpText, string lpCaption, uint uType, short wLanguageId, int dwMilliseconds);

    [LibraryImport("wininet.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool InternetGetConnectedState(out int lpdwFlags, int dwReserved);

    [LibraryImport("user32.dll", EntryPoint = "GetWindowLongW", SetLastError = true)]
    public static partial int GetWindowLong(nint hWnd, int nIndex);

    [LibraryImport("shell32.dll")]
    internal static partial int SHGetKnownFolderPath(in Guid rfid, uint dwFlags, nint hToken, out nint ppszPath);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool PostMessage(nint hwnd, uint msg, nint wparam, nint lparam);

    [LibraryImport("user32.dll", EntryPoint = "RegisterWindowMessageW", StringMarshalling = StringMarshalling.Utf16)]
    public static partial uint RegisterWindowMessage(string lpString);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool RegisterHotKey(nint hWnd, int id, uint fsModifiers, uint vk);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static partial bool UnregisterHotKey(nint hWnd, int id);

    [LibraryImport("user32.dll")]
    public static partial nint GetForegroundWindow();

    public static bool VerticalScrollbarVisible(Control ctl)
    {
        var wndStyle = GetWindowLong(ctl.Handle, GWL_STYLE);
        return (wndStyle & WS_VSCROLL) != 0;
    }

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsWindowVisible(nint hWnd);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsWindow(nint hWnd);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool IsIconic(nint hWnd);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static unsafe partial bool EnumDesktopWindows(nint hDesktop, delegate* unmanaged<nint, nint, int> callPtr, nint lPar);

    [LibraryImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool GetWindowRect(nint hWnd, out RECT lpRect);

    [StructLayout(LayoutKind.Sequential)]
    private struct RECT
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct COPYDATASTRUCT
    {
        public nint dwData;
        public int cbData;
        public nint lpData;
    }

    [LibraryImport("user32.dll", EntryPoint = "SetWindowsHookExW", SetLastError = true)]
    private static unsafe partial nint SetWindowsHookEx(int idHook, delegate* unmanaged<int, nint, nint, nint> lpfn, nint hInstance, int threadId);

    [LibraryImport("user32.dll", SetLastError = true)]
    private static partial nint CallNextHookEx(nint hhk, int nCode, nint wParam, nint lParam);

    [LibraryImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool UnhookWindowsHookEx(nint idHook);

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

    [UnmanagedCallersOnly]
    private static nint LowLevelKeyboardHookProc(int nCode, nint wParam, nint lParam)
    {
        if (nCode >= 0 && wParam == WM_KEYDOWN)
        {
            var kbStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
            var keys = (Keys)kbStruct.vkCode;

            switch (keys)
            {
                case Keys.MediaPlayPause:
                case Keys.MediaNextTrack:
                case Keys.MediaPreviousTrack:
                case Keys.MediaStop:
                    if (Application.OpenForms.Count > 0 && KeyDown != null) { KeyDown(Application.OpenForms[0], new KeyEventArgs(keys)); }
                    return 1;
            }
        }
        else if (nCode >= 0 && wParam == WM_KEYUP)
        {
            var kbStruct = Marshal.PtrToStructure<KBDLLHOOKSTRUCT>(lParam);
            var keys = (Keys)kbStruct.vkCode;

            switch (keys)
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

    public static string GetKnownFolderPath(Guid folderGuid)
    {
        if (SHGetKnownFolderPath(folderGuid, 0, nint.Zero, out var pPath) == 0)
        {
            try { return Marshal.PtrToStringUni(pPath) ?? string.Empty; }
            finally { Marshal.FreeCoTaskMem(pPath); }  // WICHTIG: Speicher wieder freigeben 
        }
        return string.Empty;
    }

    internal static unsafe nint RegisterMediaKeys()
    {
        _hookIDKeyboard = SetWindowsHookEx(WH_KEYBOARD_LL, &LowLevelKeyboardHookProc, nint.Zero, 0);
        return _hookIDKeyboard;
    }

    internal static void UnregisterMediaKeys() => UnhookWindowsHookEx(_hookIDKeyboard);

    [LibraryImport("shell32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool IsUserAnAdmin();
}