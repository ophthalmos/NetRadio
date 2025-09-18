using System;
using System.Drawing;
using System.Windows.Forms;

namespace NetRadio;

public partial class SplashForm : Form
{
    public event EventHandler SplashActivated;
    protected virtual void OnFSplashActivated(EventArgs e) { SplashActivated?.Invoke(this, e); }

    public SplashForm()
    {
        InitializeComponent();
        var parentForm = Application.OpenForms[0];
        Location = new Point(PointToScreen(new Point(parentForm.Left, parentForm.Top)).X + 51, PointToScreen(new Point(parentForm.Left, parentForm.Top)).Y + 22);
    }

    private void SplashForm_Activated(object sender, EventArgs e)
    {
        Application.OpenForms[0].Activate();
        OnFSplashActivated(null);
    }
}