using System.Windows.Forms;

namespace NetRadio
{
    public class NumericUpDown00 : NumericUpDown
    {
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (value.Length < 2) { value = "0" + value; }
                base.Text = value;
            }
        }
    }
}
