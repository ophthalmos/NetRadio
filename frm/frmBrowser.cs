using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net; // WebClient
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection; // Assembly
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml; // XmlTextReader

namespace NetRadio
{
    public partial class FrmBrowser : Form
    {
        private string radioStation, radioURL;
        private readonly List<string[]> stationList = new();
        internal string SelectedStation { get { return radioStation; } }
        internal string SelectedURL { get { return radioURL; } }

        private readonly Version curVersion = Assembly.GetExecutingAssembly().GetName().Version;
        private bool resizing = false;

        public FrmBrowser(string seachText, Point point)
        {
            InitializeComponent();
            Text = "Search results";
            bool success = false;

            using FrmWait f2 = new(new Point(point.X, point.Y));
            f2.Show(); // Please wait...
            f2.Update();
            const string baseUrl = @"all.api.radio-browser.info";
            string searchUrl = @"de1.api.radio-browser.info";
            try
            {
                IPAddress[] ips = Dns.GetHostAddresses(baseUrl);
                long lastRoundTripTime = long.MaxValue;
                string tempUrl = string.Empty;
                foreach (IPAddress ipAddress in ips)
                {
                    tempUrl = Dns.GetHostEntry(ipAddress.ToString()).HostName;
                    if (!string.IsNullOrEmpty(tempUrl))
                    {
                        try
                        {
                            PingReply reply = new Ping().Send(tempUrl);
                            if (reply.Status == IPStatus.Success && reply.RoundtripTime < lastRoundTripTime)
                            {
                                lastRoundTripTime = reply.RoundtripTime;
                                searchUrl = tempUrl;
                            }
                        }
                        catch (Exception ex) when (ex is InvalidOperationException || ex is PingException) { continue; }
                    }
                }
                f2.LabelText = searchUrl; // MessageBox.Show(searchUrl);
            }
            catch (Exception ex) when (ex is SocketException || ex is InvalidOperationException || ex is ArgumentException)
            {
                f2.Hide();
                MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            HttpClient httpClient = FrmMain.MainHttpClient;
            httpClient ??= new HttpClient();

            try
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("user-agent", Application.ProductName + "/" + new Regex(@"^\d+\.\d+").Match(curVersion.ToString()).Value);
                Task<HttpResponseMessage> responseTask = httpClient.GetAsync(@"https://" + searchUrl + "/xml/stations/search?name=" + Regex.Replace(seachText, @"\s+", " "));
                responseTask.Wait();
                HttpResponseMessage result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    Task<Stream> readTask = result.Content.ReadAsStreamAsync();
                    readTask.Wait();
                    using Stream stream = readTask.Result;

                    if (stream != null)
                    { //stream.ReadTimeout = 3000; // erzeugt Fehlermeldung: "Timeouts are not supported on this stream"
                        using XmlTextReader xtReader = new(stream);
                        xtReader.WhitespaceHandling = WhitespaceHandling.None; // Return no Whitespace and no SignificantWhitespace nodes.
                        try
                        {
                            string radioName = string.Empty; // = "prog" + i.ToString();
                            while (xtReader.Read())
                            {
                                if (xtReader.NodeType == XmlNodeType.Element && xtReader.LocalName == "station")
                                {
                                    xtReader.MoveToAttribute("name");
                                    radioName = xtReader.Value;
                                    if (radioName.Length > 0)
                                    {
                                        success = true;
                                        string[] array = new string[9];
                                        array[0] = radioName;

                                        xtReader.MoveToAttribute("url");
                                        ListViewItem item = new(radioName);
                                        item.SubItems.Add(xtReader.Value); listView.Items.Add(item);
                                        array[1] = xtReader.Value;

                                        xtReader.MoveToAttribute("homepage"); // string, URL (HTTP/HTTPS)
                                        array[2] = xtReader.Value;

                                        xtReader.MoveToAttribute("favicon"); // string, URL (HTTP/HTTPS)
                                        array[3] = xtReader.Value;

                                        xtReader.MoveToAttribute("countrycode"); // Do NOT use the "country" fields anymore! Use "countrycode" instead, which is standardized.
                                        array[4] = xtReader.Value;

                                        xtReader.MoveToAttribute("language");
                                        array[5] = xtReader.Value;

                                        xtReader.MoveToAttribute("votes"); // number, integer
                                        array[6] = xtReader.Value.ToString();

                                        xtReader.MoveToAttribute("codec");
                                        array[7] = xtReader.Value;

                                        xtReader.MoveToAttribute("bitrate"); // number, integer, bps
                                        array[8] = xtReader.Value.ToString();
                                        stationList.Add(array);
                                    }
                                }
                            }
                            Utilities.ResizeColumns(listView, false);
                        }
                        catch (XmlException ex)
                        {
                            f2.Hide();
                            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.DialogResult = DialogResult.Abort;
                            Load += (s, e) => Close();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is InvalidOperationException || ex is WebException || ex is SecurityException || ex is ArgumentException)
            {
                f2.Hide();
                DialogResult dialogResult = MessageBox.Show(ex.Message + "\n\nWould you rather do a simple Google search?", Application.ProductName, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    try { Process.Start("https://www.google.com/search?q=" + Regex.Replace(seachText + " stream url", @"[^a-zA-Z0-9äöüÄÖÜßé'-]+", " ").Trim().Replace(" ", "+")); } // [^...] Matches any single character that is not in the class.  
                    catch (InvalidOperationException ioe) { if (ioe != null) { radioStation = string.Empty; } } // Fake-Code wg. Codeüberprüfung
                }
                this.DialogResult = DialogResult.Abort;
                Load += (s, e) => Close();
                return;
            }
            if (!success)
            {
                f2.Hide();
                MessageBox.Show("No results found!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.Abort;
                Load += (s, e) => Close();
                return;
            }
            else { listView.Items[0].Selected = true; }
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            //int selected = listView.FocusedItem.Index; // gets the index of the CURRENT ListViewItem (Not the one you see highlighted after arrow key movement)

            if (e.KeyCode == Keys.F2 && listView.SelectedItems.Count > 0) { listView.SelectedItems[0].BeginEdit(); }
            else if (e.KeyCode == Keys.Escape) { DialogResult = DialogResult.Abort; }
            else if (e.KeyCode == Keys.F4) { PropertiesToolStripMenuItem_Click(null, null); }
            else if (e.Alt && e.KeyCode == Keys.Enter) // (e.KeyCode == Keys.Enter && e.Modifiers == Keys.Alt)
            {
                e.SuppressKeyPress = true;
                e.Handled = true; // Stop the character from being entered into the control 
                timer.Enabled = true; // Workaround to supress beep!
            }
            else if (e.KeyCode == Keys.Enter) { SaveAndLeave(); } // nach Alt-Enter!
            else if (e.Control && e.KeyCode == Keys.F) { DialogResult = DialogResult.Yes; } // erneut frmSearch anzeigen
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Enabled = false;
            PropertiesToolStripMenuItem_Click(null, null);
        }

        private void SaveAndLeave()
        {
            if (listView.SelectedItems != null)
            {
                radioStation = listView.SelectedItems[0].SubItems[0].Text;
                radioURL = listView.SelectedItems[0].SubItems[1].Text;
            }
            DialogResult = DialogResult.OK;
        }

        private void ListView_DoubleClick(object sender, EventArgs e) { SaveAndLeave(); }

        private void BtnAccept_Click(object sender, EventArgs e) { SaveAndLeave(); }

        private void FrmBrowser_Load(object sender, EventArgs e)
        {
            Top += 25;
            Left += 50;
            toolStripStatusLabel.Text = listView.Items.Count.ToString() + (listView.Items.Count < 2 ? " item " : " items ") + "found. Press <Alt+Enter> or <F4> for details."; // Double-click to accept an entry.";
        } //Double-click list entry to copy to station list.

        private void FrmBrowser_Resize(object sender, EventArgs e)
        {// Spaltenbreite automatisch anpassen, wenn die Größe des Hauptfensters verändert wird.
            Utilities.ResizeColumns(listView, true);
        }


        private void EditToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0) { listView.SelectedItems[0].BeginEdit(); }
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                int i = listView.Items.IndexOf(listView.SelectedItems[0]);
                string[] key = stationList[i]; //.ElementAt(i);
                string name = key[0]; //.ElementAt(0);
                string url = key[1]; //.ElementAt(1);
                string homepage = key[2]; //.ElementAt(2);
                string favicon = key[3]; //.ElementAt(3);
                string country = key[4]; //.ElementAt(4);
                string language = key[5]; //.ElementAt(5);
                string votes = key[6]; //.ElementAt(6);
                string codec = key[7]; //.ElementAt(7);
                string bitrate = key[8]; //.ElementAt(8);
                using Form frmStationInfo = new FrmStationInfo(i + 1, listView.Items.Count, name, url, homepage, favicon, country, language, votes, codec, bitrate);
                DialogResult result = frmStationInfo.ShowDialog();
                if (result == DialogResult.Yes || result == DialogResult.No)
                {
                    int selectedIndex = listView.SelectedIndices[0];
                    bool nextItem = false;
                    if (result == DialogResult.Yes)
                    {
                        selectedIndex++;
                        if (selectedIndex < listView.Items.Count) { nextItem = true; }
                    }
                    else if (result == DialogResult.No)
                    {
                        selectedIndex--;
                        if (selectedIndex >= 0) { nextItem = true; }
                    }
                    if (nextItem)
                    {
                        listView.Items[selectedIndex].Selected = true;
                        listView.Items[selectedIndex].Focused = true;
                        listView.Items[selectedIndex].EnsureVisible();
                        PropertiesToolStripMenuItem_Click(null, null);
                    }
                }
            }
        }

        private void AcceptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0) { SaveAndLeave(); }
        }

        private void CancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void BtnDetail_Click(object sender, EventArgs e) { PropertiesToolStripMenuItem_Click(null, null); }

        private void BtnEdit_Click(object sender, EventArgs e) { EditToolStripMenuItem_Click(null, null); }

        private void ListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                btnEdit.Enabled = true;
                btnAccept.Enabled = true;
                btnDetail.Enabled = true;
            }
            else
            {
                btnEdit.Enabled = false;
                btnAccept.Enabled = false;
                btnDetail.Enabled = false;
            }
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                editToolStripMenuItem.Enabled = true;
                acceptToolStripMenuItem.Enabled = true;
                propertiesToolStripMenuItem.Enabled = true;
            }
            else
            {
                editToolStripMenuItem.Enabled = false;
                acceptToolStripMenuItem.Enabled = false;
                propertiesToolStripMenuItem.Enabled = false;
            }
        }

        private void ListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (!resizing) // Don't allow overlapping of SizeChanged calls
            {
                resizing = true; // Set the resizing flag
                int vScrollWidth = 0;
                if (NativeMethods.VerticalScrollbarVisible(listView)) { vScrollWidth = SystemInformation.VerticalScrollBarWidth; }
                listView.Columns[0].Width = 200;
                listView.Columns[1].Width = listView.Width - 200 - vScrollWidth - 4;
                resizing = false; // Clear the resizing flag
            }
        }

    }
}
