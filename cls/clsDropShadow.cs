using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NetRadio.cls;

public class DropShadow
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public struct MARGINS
    {
        public int leftWidth;
        public int rightWidth;
        public int topHeight;
        public int bottomHeight;
    }

    [DllImport("dwmapi.dll")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static extern int DwmExtendFrameIntoClientArea(nint hWnd, ref MARGINS pMarInset);

    [DllImport("dwmapi.dll")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static extern int DwmSetWindowAttribute(nint hwnd, int attr, ref int attrValue, int attrSize);

    [DllImport("dwmapi.dll")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    private static extern int DwmIsCompositionEnabled(ref int pfEnabled);

    public static void ApplyShadows(Form form)
    {
        var v = 2;
        _ = DwmSetWindowAttribute(form.Handle, 2, ref v, 4);
        var margins = new MARGINS() { bottomHeight = 1, leftWidth = 0, rightWidth = 0, topHeight = 0 };
        _ = DwmExtendFrameIntoClientArea(form.Handle, ref margins);
    }

}
