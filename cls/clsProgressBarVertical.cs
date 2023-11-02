using System.Windows.Forms;

namespace NetRadio
{
    public class VerticalProgressBar : ProgressBar
    {
        public VerticalProgressBar() => NativeMethods.SetWindowTheme(Handle, string.Empty, string.Empty);
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x04;
                return cp;
            }
        }
    }
}
