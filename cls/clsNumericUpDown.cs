using System.Windows.Forms;

namespace NetRadio.cls;

public class NumericUpDown00 : NumericUpDown
{
    protected override void UpdateEditText() => Text = Value.ToString("00");
}
