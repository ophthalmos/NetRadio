using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace NetRadio;

public partial class FrmStationInfo : Form
{
    public FrmStationInfo(int index, int total, string name, string url, string homepage, string favicon, string country, string language, string votes, string codec, string bitrate)
    {
        InitializeComponent();
        Text = index.ToString() + "/" + total.ToString();
        lblName.Text = name.Length > 0 ? name : "N.N.";
        tbxURL.Text = url;
        linkLblHomepage.Text = homepage;
        lblCountry.Text = country;
        lblLanguage.Text = language;
        lblVotes.Text = votes;
        lblCodec.Text = codec.Length > 0 ? codec : "unknown";
        lblBitrate.Text = bitrate.Equals("0") ? "unknown" : bitrate;
        if (favicon.Length > 0)
        {
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.ImageLocation = favicon;
        }
        else
        {
            pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox.Image = pictureBox.ErrorImage;
        }

        if (total == 1) { btnPrevious.Enabled = btnNext.Enabled = false; }
        else if (index == 1) { btnPrevious.Enabled = false; }
        else if (index == total) { btnNext.Enabled = false; }

        tbxURL.SelectionStart = tbxURL.TextLength;
        tbxURL.ScrollToCaret();
        tbxURL.HideSelection = false; //Keep the selection highlighted, even after textbox loses focus.
    }

    private void LinkLblHomepage_MouseClick(object sender, MouseEventArgs e)
    {// LinkClicked-Event wird hier nicht verwendet, weil sonst Enter-Taste den Link startet!
        try
        {
            ProcessStartInfo psi = new(linkLblHomepage.Text) { UseShellExecute = true };
            Process.Start(psi);
        }
        catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Enter: // case Keys.Return:
            case Keys.Escape: { DialogResult = DialogResult.Cancel; return false; }
            case Keys.PageUp:
            case Keys.F3: { DialogResult = DialogResult.No; return true; }
            case Keys.PageDown:
            case Keys.F4: { DialogResult = DialogResult.Yes; return true; }
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

}
