using System.Drawing;
using System.Windows.Forms;

namespace NetRadio;

public partial class FrmWait : Form
{
    public string LabelText
    {
        set => labelPleaseWait.Text = value;
    }
    public FrmWait(Point point)
    {
        InitializeComponent();
        Location = new Point(point.X + 50, point.Y + 25);
    }

    protected override bool ShowWithoutActivation => true;

    private const int WS_EX_TOPMOST = 0x00000008;
    protected override CreateParams CreateParams
    {
        get
        {
            var createParams = base.CreateParams;
            createParams.ExStyle |= WS_EX_TOPMOST;
            return createParams;
        }
    }

    //protected override void WndProc(ref Message m)
    //{
    //    base.WndProc(ref m);
    //    if (m.Msg == NativeMethods.WM_NCHITTEST && (int)m.Result == NativeMethods.HTCLIENT) { m.Result = (IntPtr)NativeMethods.HTCAPTION; } // allows form to be draggable still
    //    else if (m.Msg == NativeMethods.WM_NCACTIVATE && m.WParam.ToInt32() == 0) // wurde durch Umstellung auf nicht modal und Deactivate-Event überflüssig
    //    {
    //        if (!RectangleToScreen(DisplayRectangle).Contains(Cursor.Position)) { DialogResult = DialogResult.Cancel; } // Close by click outside form cientarea
    //    }
    //}

}
