using System.Windows.Forms;

namespace NetRadio.cls;

public class NumericUpDown00 : NumericUpDown
{
    public override string Text
    {
        get => base.Text;
        set
        {
            if (value.Length < 2) { value = "0" + value; }
            base.Text = value;
        }
    }
}
