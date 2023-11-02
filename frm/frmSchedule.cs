using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NetRadio
{
    public partial class FrmSchedule : Form
    {
        internal List<string> StationsList { get { return stations; } set { stations = value; } }
        internal ListView ActionListView { get { return aLV; } set { aLV = value; } }
        internal CheckBox KeepActionsActive { get { return cbKeepActionsActive; } set { cbKeepActionsActive = value; } }

        private List<string> stations = new();
        private bool checkFromDoubleClick = false;

        public FrmSchedule()
        {
            InitializeComponent();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (aLV.SelectedItems.Count > 0)
            {
                using FrmTask frmTask = new();
                if (TopMost) { frmTask.TopMost = true; }

                string s = aLV.SelectedItems[0].SubItems[1].Text;
                if (frmTask.RBtnPlay.Text.Equals(s)) { frmTask.RBtnPlay.Checked = true; }
                else if (frmTask.RBtnStop.Text.Equals(s)) { frmTask.RBtnStop.Checked = true; }
                else if (frmTask.RBtnRecord.Text.Equals(s)) { frmTask.RBtnRecord.Checked = true; }
                else if (frmTask.RBtnRecStop.Text.Equals(s)) { frmTask.RBtnRecStop.Checked = true; }
                else if (frmTask.RBtnSleep.Text.Equals(s)) { frmTask.RBtnSleep.Checked = true; }
                else if (frmTask.RBtnHibernate.Text.Equals(s)) { frmTask.RBtnHibernate.Checked = true; }
                else if (frmTask.RBtnShutdown.Text.Equals(s)) { frmTask.RBtnShutdown.Checked = true; }

                foreach (string station in stations) { frmTask.CmBxStations.Items.Add(station); }
                if (!string.IsNullOrEmpty(aLV.SelectedItems[0].SubItems[2].Text))
                {
                    frmTask.CmBxStations.Text = aLV.SelectedItems[0].SubItems[2].Text;
                }
                else { frmTask.CmBxStations.Text = frmTask.RBtnPlay.Checked || frmTask.RBtnRecord.Checked ? stations[0] : string.Empty; }
                if (!string.IsNullOrEmpty(aLV.SelectedItems[0].SubItems[3].Text))
                {
                    frmTask.NUDHH.Text = aLV.SelectedItems[0].SubItems[3].Text.Split(':').FirstOrDefault();
                    frmTask.NUDMM.Text = aLV.SelectedItems[0].SubItems[3].Text.Split(':').LastOrDefault();
                }
                else
                {
                    frmTask.NUDHH.Text = DateTime.Now.Hour.ToString("D2");
                    frmTask.NUDMM.Text = DateTime.Now.AddMinutes(1).Minute.ToString("D2");
                }



                if (frmTask.ShowDialog() == DialogResult.OK)
                {
                    aLV.SelectedItems[0].Checked = true;
                    aLV.SelectedItems[0].SubItems[1].Text = frmTask.RBtnPlay.Checked ? frmTask.RBtnPlay.Text :
                        frmTask.RBtnStop.Checked ? frmTask.RBtnStop.Text :
                        frmTask.RBtnRecord.Checked ? frmTask.RBtnRecord.Text :
                        frmTask.RBtnRecStop.Checked ? frmTask.RBtnRecStop.Text :
                        frmTask.RBtnSleep.Checked ? frmTask.RBtnSleep.Text :
                        frmTask.RBtnHibernate.Checked ? frmTask.RBtnHibernate.Text :
                        frmTask.RBtnShutdown.Checked ? frmTask.RBtnShutdown.Text : string.Empty;

                    aLV.SelectedItems[0].SubItems[2].Text = frmTask.CmBxStations.Text; // short station text

                    aLV.SelectedItems[0].SubItems[3].Text = frmTask.NUDHH.Value.ToString().PadLeft(2, '0') + ":" + frmTask.NUDMM.Value.ToString().PadLeft(2, '0');

                }
            }
        }

        private void TimerListview_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (aLV.SelectedItems.Count > 0)
            {
                aLV.SelectedItems[0].Checked = false;
                BtnEdit_Click(null, null);
            }
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

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (aLV.SelectedItems.Count > 0)
            {
                int x = aLV.SelectedItems[0].Index;
                aLV.SelectedItems[0].Remove();
                aLV.Items.Add(new ListViewItem(new string[] { "", "", "", "" }));
                if (x >= 0 && !string.IsNullOrEmpty(aLV.Items[x].SubItems[1].Text)) { aLV.Items[x].Selected = true; }
                else if (x > 0 && !string.IsNullOrEmpty(aLV.Items[x - 1].SubItems[1].Text)) { aLV.Items[x - 1].Selected = true; }
            }
        }

        private void ALV_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (checkFromDoubleClick) // prevent listview item checked from double-click
            {
                e.NewValue = e.CurrentValue;
                checkFromDoubleClick = false;
            }
        }

        private void ALV_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left) && (e.Clicks > 1)) { checkFromDoubleClick = true; } // Is this a double-click?
        }

        private void CbKeepActionsActive_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != null && ActiveControl == cbKeepActionsActive && aLV.CheckedItems.Count.Equals(0)) { cbKeepActionsActive.Checked = false; }
        }

        private void ALV_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ActiveControl != null && ActiveControl == aLV && aLV.CheckedItems.Count.Equals(0)) { cbKeepActionsActive.Checked = false; }
        }
    }
}
