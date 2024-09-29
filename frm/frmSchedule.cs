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
        internal CheckBox RepeatActionsDaily { get { return cbRepeatActionsDaily; } set { cbRepeatActionsDaily = value; } }

        private List<string> stations = new();
        private bool checkFromDoubleClick = false;
        //private readonly string helpText = "Unfortunately, it is currently not possible to set daily repeating\ntasks. This is to prevent a PC from shutting down accidentally.\nIf you want a task from the previous day to repeat, you have\nto manually activate its checkbox every day.";

        public FrmSchedule()
        {
            InitializeComponent();
        }

        private void FrmSchedule_Load(object sender, EventArgs e)
        {
            ListViewItem[] items = new ListViewItem[aLV.Items.Count]; // Liste der ListViewItems erstellen, um die Sortierung durchzuführen
            aLV.Items.CopyTo(items, 0);
            Array.Sort(items, (x, y) =>  // Elemente nach der dritten Spalte (Time) sortieren, leere Einträge am Ende
            {
                if (!string.IsNullOrEmpty(x.SubItems[3].Text) && !string.IsNullOrEmpty(y.SubItems[3].Text)) { return string.Compare(x.SubItems[3].Text, y.SubItems[3].Text, StringComparison.OrdinalIgnoreCase); }
                else if (!string.IsNullOrEmpty(x.SubItems[3].Text)) { return -1; } // Wenn nur eines der Elemente Text hat, soll das mit Text zuerst kommen
                else if (!string.IsNullOrEmpty(y.SubItems[3].Text)) { return 1; }
                else { return 0; } // Beide Elemente haben keinen Text, beliebige Reihenfolge
            });
            aLV.Items.Clear();
            aLV.Items.AddRange(items); // Sortierte Elemente in das ListView übertragen
            lblRepeat.Visible = cbRepeatActionsDaily.Checked && ContainsTextInCheckedItems(aLV) > 0;
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
                aLV.Items.Add(new ListViewItem(["", "", "", ""]));
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

        private void CbRepeatActionsDaily_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != null && ActiveControl == cbRepeatActionsDaily && aLV.CheckedItems.Count.Equals(0)) { cbRepeatActionsDaily.Checked = false; }
            else
            {
                lblRepeat.Visible = ActiveControl != null && ActiveControl == cbRepeatActionsDaily && cbRepeatActionsDaily.Checked && ContainsTextInCheckedItems(aLV) > 0;
            }
        }

        private int ContainsTextInCheckedItems(ListView listView)
        {
            int returnValue = 0;
            foreach (ListViewItem item in listView.CheckedItems)
            {
                if (item.SubItems[1].Text == Utilities.TaskNames[6])
                {
                    lblRepeat.Text = "Caution: The computer is shut down every day!";
                    returnValue = 2;
                }
                else if (item.SubItems[1].Text == Utilities.TaskNames[5])
                {
                    lblRepeat.Text = "Caution: The computer hibernates every day!";
                    returnValue = returnValue == 0 ? 1 : returnValue;
                }
            }
            return returnValue;
        }


        private void ALV_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (ActiveControl != null && ActiveControl == aLV && aLV.CheckedItems.Count.Equals(0)) { cbRepeatActionsDaily.Checked = false; }
            else
            {
                lblRepeat.Visible = ActiveControl != null && ActiveControl == aLV && cbRepeatActionsDaily.Checked && ContainsTextInCheckedItems(aLV) > 0;
            }
        }

        //private void FrmSchedule_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //    Utilities.ErrorMsgTaskDlg(Handle, helpText, Application.ProductName, TaskDialogIcon.None);
        //}

        //private void FrmSchedule_HelpRequested(object sender, HelpEventArgs hlpevent)
        //{
        //    hlpevent.Handled = true;
        //    Utilities.ErrorMsgTaskDlg(Handle, helpText, Application.ProductName, TaskDialogIcon.None);
        //}

    }
}
