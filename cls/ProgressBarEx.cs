using System.Drawing;
using System.Windows.Forms;

namespace NetRadio.cls;

internal class ProgressBarEx : ProgressBar
{
    public ProgressBarEx() { SetStyle(ControlStyles.UserPaint, true); }

    protected override CreateParams CreateParams
    { // remove flickering in this custom ProgressBar class?
        get
        {
            var result = base.CreateParams;
            result.ExStyle |= 0x02000000; // WS_EX_COMPOSITED 
            return result;
        }
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        SolidBrush brush;
        Rectangle rec = new(0, 0, Width, Height);
        if (ProgressBarRenderer.IsSupported) { ProgressBarRenderer.DrawHorizontalBar(e.Graphics, rec); }
        rec.Width = (int)(rec.Width * ((double)Value / Maximum)) - 4;
        rec.Height -= 4;
        using (brush = new SolidBrush(ForeColor)) { e.Graphics.FillRectangle(brush, 2, 2, rec.Width, rec.Height); }
    }
}