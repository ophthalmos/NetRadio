using Microsoft.Win32;
using NetRadio.cls;
using System;
using System.Windows.Forms;

namespace NetRadio;

public partial class FrmTask : Form
{
    internal RadioButton RBtnPlay
    {
        set => rbPlay = value;
        get => rbPlay;
    }
    internal RadioButton RBtnStop
    {
        set => rbStop = value;
        get => rbStop;
    }
    internal RadioButton RBtnRecord
    {
        set => rbRecord = value;
        get => rbRecord;
    }
    internal RadioButton RBtnRecStop
    {
        set => rbRecStop = value;
        get => rbRecStop;
    }
    internal RadioButton RBtnShutdown
    {
        set => rbShutdown = value;
        get => rbShutdown;
    }
    internal RadioButton RBtnSleep
    {
        set => rbSleep = value;
        get => rbSleep;
    }
    internal RadioButton RBtnHibernate
    {
        set => rbHibernate = value;
        get => rbHibernate;
    }
    internal ComboBox CmBxStations
    {
        set => cmBxStations = value;
        get => cmBxStations;
    }
    internal NumericUpDown00 NUDHH
    {
        set => nudHH = value;
        get => nudHH;
    }
    internal NumericUpDown00 NUDMM
    {
        set => nudMM = value;
        get => nudMM;
    }

    private string station = string.Empty;
    private bool selectHHAllDone = false;
    private bool selectMMAllDone = false;

    public FrmTask()
    {
        InitializeComponent();
        rbPlay.Text = Utilities.TaskNames[0];
        rbStop.Text = Utilities.TaskNames[1];
        rbRecord.Text = Utilities.TaskNames[2];
        rbRecStop.Text = Utilities.TaskNames[3];
        rbSleep.Text = Utilities.TaskNames[4];
        rbHibernate.Text = Utilities.TaskNames[5];
        rbShutdown.Text = Utilities.TaskNames[6];
        station = cmBxStations.Text;
        var hibernateNew = Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabledDefault", -1)); // -1 if name does not exist
        var hibernateOld = Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Power", "HibernateEnabled", -1)); // -1 if name does not exist
        rbHibernate.Enabled = hibernateNew > 0 || hibernateOld > 0;  // From version 1809 the registry setting “HibernateEnabled” was renamed “HibernateEnabledDefault”
    }

    protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
    {
        switch (keyData)
        {
            case Keys.Escape:
                {
                    Close();
                    return true;
                }
        }
        return base.ProcessCmdKey(ref msg, keyData);
    }

    private void RbPlay_CheckedChanged(object sender, System.EventArgs e)
    {
        if (rbPlay.Checked)
        {
            cmBxStations.Enabled = true;
            cmBxStations.Text = cmBxStations.Text == string.Empty ? station : cmBxStations.Text;
        }
    }

    private void RbRecord_CheckedChanged(object sender, System.EventArgs e)
    {
        if (rbRecord.Checked)
        {
            cmBxStations.Enabled = true;
            cmBxStations.Text = cmBxStations.Text == string.Empty ? station : cmBxStations.Text;
        }
    }

    private void RbStop_CheckedChanged(object sender, System.EventArgs e)
    {
        if (rbStop.Checked)
        {
            cmBxStations.Enabled = false;
            station = CmBxStations.Text;
            cmBxStations.Text = string.Empty;
        }
    }

    private void RbRecStop_CheckedChanged(object sender, System.EventArgs e)
    {
        if (rbRecStop.Checked)
        {
            cmBxStations.Enabled = false;
            station = CmBxStations.Text;
            cmBxStations.Text = string.Empty;
        }
    }

    private void RbSleep_CheckedChanged(object sender, EventArgs e)
    {
        if (rbSleep.Checked)
        {
            cmBxStations.Enabled = false;
            station = CmBxStations.Text;
            cmBxStations.Text = string.Empty;
        }
    }

    private void RbHibernate_CheckedChanged(object sender, EventArgs e)
    {
        if (rbHibernate.Checked)
        {
            cmBxStations.Enabled = false;
            station = CmBxStations.Text;
            cmBxStations.Text = string.Empty;
        }
    }

    private void RbShutdown_CheckedChanged(object sender, System.EventArgs e)
    {
        if (rbShutdown.Checked)
        {
            cmBxStations.Enabled = false;
            station = CmBxStations.Text;
            cmBxStations.Text = string.Empty;
        }
    }

    private void NudMM_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) { { DialogResult = DialogResult.OK; } } // Formular schließen
    }

    private void NudHH_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter) { { DialogResult = DialogResult.OK; } } // Formular schließen
    }

    private void NudHH_Enter(object sender, EventArgs e)
    {
        if (MouseButtons == MouseButtons.None)
        {
            nudHH.Select(0, nudHH.Text.Length);
            selectHHAllDone = true;
        }
    }
    private void NudMM_Enter(object sender, EventArgs e)
    {
        if (MouseButtons == MouseButtons.None)
        {
            nudMM.Select(0, nudMM.Text.Length);
            selectMMAllDone = true;
        }
    }

    private void NudHH_MouseClick(object sender, MouseEventArgs e)
    {
        if (!selectHHAllDone) { nudHH.Select(0, nudHH.Text.Length); selectHHAllDone = true; }
    }
    private void NudMM_MouseClick(object sender, MouseEventArgs e)
    {
        if (!selectMMAllDone) { nudMM.Select(0, nudMM.Text.Length); selectMMAllDone = true; }
    }

    private void NudHH_Leave(object sender, EventArgs e)
    {
        selectHHAllDone = false;
    }
    private void NudMM_Leave(object sender, EventArgs e)
    {
        selectMMAllDone = false;
    }

}
