using System;
using System.Drawing;
using System.Windows.Forms;
using NetRadio.cls;

namespace NetRadio;

public partial class SplashForm : Form
{
    public event EventHandler? SplashActivated;

    private readonly Form _ownerForm;

    public SplashForm(Form ownerForm)
    {
        InitializeComponent();
        Lng.Apply(this); // übersetzt alle Designer-Texte, falls nicht Englisch eingestellt ist
        _ownerForm = ownerForm; // Speichern der Referenz
        Location = new Point(_ownerForm.Left + 51, _ownerForm.Top + 22);
    }

    private void SplashForm_Activated(object sender, EventArgs e)
    {
        _ownerForm.Activate();
        OnFSplashActivated(EventArgs.Empty);
    }

    protected virtual void OnFSplashActivated(EventArgs e)
    {
        SplashActivated?.Invoke(this, e);
    }
}