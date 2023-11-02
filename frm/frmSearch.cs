using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace NetRadio
{
    public partial class FrmSearch : Form
    {
        internal TextBox TbString { get { return tbSearch; } }

        public FrmSearch(string row, string name)
        {
            InitializeComponent();
            lblName.Text = name;
            if (string.IsNullOrEmpty(name)) { lblMsg2.Text = string.Empty; }
            lblRow.Text = row;
        }

        private void FrmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) { { Close(); } } // Formular schließen
        }

        private void LinkLblRadioBrowserInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                ProcessStartInfo psi = new("https://www.radio-browser.info/") { UseShellExecute = true };
                Process.Start(psi);
            }
            catch (Exception ex) when (ex is Win32Exception || ex is InvalidOperationException) { MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        }

        //protected override void OnHandleCreated(EventArgs e)
        //{
        //    base.OnHandleCreated(e);
        //    NativeMethods.SendMessage(tbSearch.Handle, 0xD3, 1, 6); // EM_SETMARGINS = 0xD3, EC_LEFTMARGIN = 1, lParam = Wert
        //    NativeMethods.SendMessage(tbSearch.Handle, 0xD3, 2, (3 << 16)); // EM_SETMARGINS = 0xD3, EC_RIGHTMARGIN = 2; lParam = Wert
        //}

    }
}
