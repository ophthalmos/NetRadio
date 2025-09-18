using System.Windows.Forms;

namespace NetRadio.cls;

public class AutoEllipsisToolStripRenderer : ToolStripSystemRenderer
{
    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        if (e.Item is not ToolStripStatusLabel label)
        {
            base.OnRenderItemText(e);
            return;
        }
        if ((e.Item as ToolStripStatusLabel).IsLink) { base.OnRenderItemText(e); }
        else { TextRenderer.DrawText(e.Graphics, label.Text, label.Font, e.TextRectangle, label.ForeColor, TextFormatFlags.EndEllipsis); }
    }
}
