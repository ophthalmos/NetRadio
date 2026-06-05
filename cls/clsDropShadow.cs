using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NetRadio.cls;

public static partial class DropShadow
{
    private struct MARGINS
    {
        public int leftWidth;
        public int rightWidth;
        public int topHeight;
        public int bottomHeight;
    }

    // Modernes P/Invoke via Source Generator in .NET
    [LibraryImport("dwmapi.dll")]
    private static partial int DwmExtendFrameIntoClientArea(nint hWnd, ref MARGINS pMarInset);

    [LibraryImport("dwmapi.dll")]
    private static partial int DwmSetWindowAttribute(nint hwnd, int attr, ref int attrValue, int attrSize);

    [LibraryImport("dwmapi.dll")]
    private static partial int DwmIsCompositionEnabled(out int pfEnabled);

    // Konstanten für bessere Lesbarkeit statt magischer Zahlen
    private const int DWMWA_NCRENDERING_POLICY = 2;
    private const int DWMNCRP_ENABLED = 2;

    public static void ApplyShadows(Form form)
    {
        // Zur Sicherheit prüfen, ob die Desktop-Fenstermanager-Komposition aktiv ist
        _ = DwmIsCompositionEnabled(out var isEnabled);

        if (isEnabled != 1)
        {
            return;
        }

        var policy = DWMNCRP_ENABLED;
        _ = DwmSetWindowAttribute(form.Handle, DWMWA_NCRENDERING_POLICY, ref policy, sizeof(int));

        var margins = new MARGINS { bottomHeight = 1, leftWidth = 0, rightWidth = 0, topHeight = 0 };
        _ = DwmExtendFrameIntoClientArea(form.Handle, ref margins);
    }
}