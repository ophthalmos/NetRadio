using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace NetRadio.cls;

[Flags]
internal enum THUMBBUTTONMASK : uint
{
    THB_BITMAP = 0x1,
    THB_ICON = 0x2,
    THB_TOOLTIP = 0x4,
    THB_FLAGS = 0x8
}

[Flags]
internal enum THUMBBUTTONFLAGS : uint
{
    THBF_ENABLED = 0x0,
    THBF_DISABLED = 0x1,
    THBF_DISMISSONCLICK = 0x2,
    THBF_NOBACKGROUND = 0x4,
    THBF_HIDDEN = 0x8,
    THBF_NONINTERACTIVE = 0x10
}

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
internal struct THUMBBUTTON
{
    public THUMBBUTTONMASK dwMask;
    public uint iId;
    public uint iBitmap;
    public nint hIcon;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
    public string szTip;
    public THUMBBUTTONFLAGS dwFlags;
}

#pragma warning disable SYSLIB1096 // Konvertieren Sie in GeneratedComInterface
[ComImport]
[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal interface ITaskbarList3
{
    void HrInit();
    void AddTab(nint hwnd);
    void DeleteTab(nint hwnd);
    void ActivateTab(nint hwnd);
    void SetActiveAlt(nint hwnd);
    void MarkFullscreenWindow(nint hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);
    void SetProgressValue(nint hwnd, ulong ullCompleted, ulong ullTotal);
    void SetProgressState(nint hwnd, int tbpFlags);
    void RegisterTab(nint hwndTab, nint hwndMDI);
    void UnregisterTab(nint hwndTab);
    void SetTabOrder(nint hwndTab, nint hwndInsertBefore);
    void SetTabActive(nint hwndTab, nint hwndMDI, uint dwReserved);

    [PreserveSig]
    int ThumbBarAddButtons(nint hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButton);

    [PreserveSig]
    int ThumbBarUpdateButtons(nint hwnd, uint cButtons, [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)] THUMBBUTTON[] pButton);

    void ThumbBarSetImageList(nint hwnd, nint himl);
    void SetOverlayIcon(nint hwnd, nint hIcon, [MarshalAs(UnmanagedType.LPWStr)] string pszDescription);
    void SetThumbnailTooltip(nint hwnd, [MarshalAs(UnmanagedType.LPWStr)] string pszTip);
    void SetThumbnailClip(nint hwnd, ref Rectangle prcClip);
}
#pragma warning restore SYSLIB1096
