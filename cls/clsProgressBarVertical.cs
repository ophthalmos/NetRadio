using System.Windows.Forms;

namespace NetRadio.cls;

public class VerticalProgressBar : ProgressBar
{
    public VerticalProgressBar()
    {
        _ = NativeMethods.SetWindowTheme(Handle, string.Empty, string.Empty);
    }

    protected override CreateParams CreateParams
    {
        get
        {
            var cp = base.CreateParams;
            cp.Style |= 0x04;
            return cp;
        }
    }
}
