using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using NetRadio.cls;

namespace NetRadio;

public partial class FrmBrowser : Form
{
    // Typisierte Klasse für die Server-Antwort (Json)
    public class RadioServer
    {
        [JsonPropertyName("name")]
        public string? Name
        {
            get; set;
        }
    }

    // Typisiertes Objekt für die interne Liste (statt string[])
    // Erleichtert den Zugriff in PropertiesToolStripMenuItem_Click erheblich.
    private record StationItem(
        string Name,
        string Url,
        string Homepage,
        string Favicon,
        string CountryCode,
        string Language,
        string Votes,
        string Codec,
        string Bitrate
    );

    private string? radioStation, radioURL;
    internal string? SelectedStation => radioStation;
    internal string? SelectedURL => radioURL;

    private readonly Version? curVersion;
    private bool resizing = false;
    private readonly string _searchText;

    // Typisierte Liste
    private readonly List<StationItem> stationList = [];

    public FrmBrowser(string searchText, Point point, Version version)
    {
        InitializeComponent();
        Text = "Search results";
        _searchText = searchText;
        Location = point;
        curVersion = version;
    }

    private async void FrmBrowser_Load(object sender, EventArgs e)
    {
        await LoadStationsAsync();
        Top += 25;
        Left += 50;
        toolStripStatusLabel.Text = $"{listView.Items.Count} {(listView.Items.Count < 2 ? "item" : "items")} found. Press <Alt+Enter> or <F4> for details.";
    }

    private async Task LoadStationsAsync()
    {
        using FrmWait f2 = new(new Point(Location.X, Location.Y));
        f2.Show();
        f2.Update();

        try
        {
            // 1. Liste potenzieller Server erstellen
            // Zuerst den per Ping ermittelten "besten", dann harte Fallbacks als Reserve.
            var bestServer = await SelectBestServerAsync();

            List<string> serverQueue = [];
            if (!string.IsNullOrEmpty(bestServer)) { serverQueue.Add(bestServer); }

            // Fallbacks hinzufügen (Deutschland, Österreich, Niederlande sind meist stabil)
            serverQueue.Add("de1.api.radio-browser.info");
            serverQueue.Add("at1.api.radio-browser.info");
            serverQueue.Add("nl1.api.radio-browser.info");

            // Duplikate entfernen (falls bestServer == fallback ist)
            var distinctServers = serverQueue.Distinct().ToList();

            List<StationItem>? stations = null;
            Exception? lastException = null;

            // 2. Retry-Schleife: Versuche Server nacheinander
            foreach (var server in distinctServers)
            {
                try
                {
                    f2.LabelText = $"Connecting to: {server}";

                    // URL bauen
                    var searchUrl = $"https://{server}/xml/stations/search?name={Uri.EscapeDataString(_searchText)}";

                    var httpClient = NetHttpClient.Instance;

                    // Timeout für diesen Versuch etwas kürzer setzen, damit wir nicht ewig warten, 
                    // falls ein Server hängt (optional, über CancellationTokenSource möglich).
                    // Hier nutzen wir einfach den Standard-Call.

                    using (var response = await httpClient.GetAsync(searchUrl))
                    {
                        response.EnsureSuccessStatusCode(); // Hier krachte es vorher (502)

                        await using var stream = await response.Content.ReadAsStreamAsync();
                        var xdoc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

                        stations = [.. xdoc.Descendants("station")
                            .Select(s => new StationItem(
                                Name: (string?)s.Attribute("name") ?? string.Empty,
                                Url: (string?)s.Attribute("url") ?? string.Empty,
                                Homepage: (string?)s.Attribute("homepage") ?? string.Empty,
                                Favicon: (string?)s.Attribute("favicon") ?? string.Empty,
                                CountryCode: (string?)s.Attribute("countrycode") ?? string.Empty,
                                Language: (string?)s.Attribute("language") ?? string.Empty,
                                Votes: (string?)s.Attribute("votes") ?? string.Empty,
                                Codec: (string?)s.Attribute("codec") ?? string.Empty,
                                Bitrate: (string?)s.Attribute("bitrate") ?? string.Empty
                            ))
                            .Where(s => !string.IsNullOrEmpty(s.Name))];
                    }

                    // Wenn wir hier ankommen, war es erfolgreich -> Schleife abbrechen
                    lastException = null;
                    break;
                }
                catch (HttpRequestException ex)
                {
                    // 502, 503, 404 etc. -> Wir merken uns den Fehler und versuchen den nächsten Server
                    lastException = ex;
                    continue;
                }
                catch (Exception ex)
                {
                    // Andere Fehler (XML Parse Error etc.) -> Auch hier Retry versuchen
                    lastException = ex;
                    continue;
                }
            }

            // 3. Ergebnis prüfen
            if (stations == null || stations.Count == 0)
            {
                f2.Hide();
                if (lastException != null)
                {
                    // Wenn alle Server fehlschlugen, zeigen wir den letzten Fehler an
                    Utilities.ErrTaskDialog(this, lastException);
                }
                else { Utilities.MsgTaskDialog(this, "No results found!", $"No stations matching '{_searchText}' were found.", TaskDialogIcon.ShieldWarningYellowBar); }

                DialogResult = DialogResult.Abort;
                Close();
                return;
            }

            // 4. UI Update (nur wenn wir Daten haben)
            listView.BeginUpdate();
            stationList.Clear();

            foreach (var station in stations)
            {
                var item = new ListViewItem(station.Name);
                item.SubItems.Add(station.Url);
                listView.Items.Add(item);
                stationList.Add(station);
            }

            Utilities.ResizeColumns(listView, false);
            if (listView.Items.Count > 0)
            {
                listView.Items[0].Selected = true;
            }
            listView.EndUpdate();

        }
        catch (Exception ex)
        {
            // Fangnetz für völlig unerwartete Fehler außerhalb der Schleife
            f2.Hide();
            Utilities.ErrTaskDialog(this, ex);
            DialogResult = DialogResult.Abort;
            Close();
        }
        finally
        {
            f2.Hide();
        }
    }

    private static async Task<string?> SelectBestServerAsync()
    {
        try
        {
            var httpClient = NetHttpClient.Instance;

            // JSON der Serverliste abrufen
            // GetFromJsonAsync ist eine praktische Abkürzung in .NET
            var servers = await httpClient.GetFromJsonAsync<List<RadioServer>>("https://all.api.radio-browser.info/json/servers");

            if (servers == null || servers.Count == 0) { return null; }

            // Pings parallel ausführen
            var pingTasks = servers
                .Where(s => !string.IsNullOrEmpty(s.Name))
                .Select(async server =>
                {
                    try
                    {
                        using var ping = new Ping();
                        // SendPingAsync ist der moderne Weg
                        var reply = await ping.SendPingAsync(server.Name!, 2000);

                        if (reply.Status == IPStatus.Success)
                        {
                            return new { ServerName = server.Name, Latency = reply.RoundtripTime };
                        }
                    }
                    catch (PingException) { /* Ignore unreachable hosts */ }
                    return null;
                });

            var results = await Task.WhenAll(pingTasks);

            // Den schnellsten Server finden (MinBy ignoriert null nicht automatisch, daher Filter davor)
            var bestResult = results.Where(r => r != null).MinBy(r => r!.Latency);

            return bestResult?.ServerName;
        }
        catch (Exception) // catch-all für Netzwerk/Json Fehler ist hier okay, da Fallback greift
        {
            return null;
        }
    }

    private void ListView_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F2 && listView.SelectedItems.Count > 0)
        {
            listView.SelectedItems[0].BeginEdit();
        }
        else if (e.KeyCode == Keys.Escape)
        {
            DialogResult = DialogResult.Abort;
        }
        else if (e.KeyCode == Keys.F4)
        {
            PropertiesToolStripMenuItem_Click(null!, null!);
        }
        else if (e.Alt && e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
            timer.Enabled = true;
        }
        else if (e.KeyCode == Keys.Enter)
        {
            SaveAndLeave();
        }
        else if (e.Control && e.KeyCode == Keys.F)
        {
            DialogResult = DialogResult.Yes;
        }
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        timer.Enabled = false;
        PropertiesToolStripMenuItem_Click(null!, null!);
    }

    private void SaveAndLeave()
    {
        if (listView.SelectedItems.Count > 0)
        {
            radioStation = listView.SelectedItems[0].Text; // Name ist Text (SubItem 0)
            radioStation = listView.SelectedItems[0].SubItems[0].Text;
            radioURL = listView.SelectedItems[0].SubItems[1].Text;
        }
        DialogResult = DialogResult.OK;
    }

    private void ListView_DoubleClick(object sender, EventArgs e)
    {
        SaveAndLeave();
    }

    private void BtnAccept_Click(object sender, EventArgs e)
    {
        SaveAndLeave();
    }

    private void FrmBrowser_Resize(object sender, EventArgs e)
    {
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
            var index = listView.SelectedIndices[0];

            // HIER IST DER VORTEIL DES RECORDS:
            // Kein index-basiertes Raten mehr (key[0], key[1]...), sondern klare Namen:
            var station = stationList[index];

            using Form frmStationInfo = new FrmStationInfo(
                index + 1,
                listView.Items.Count,
                station.Name,
                station.Url,
                station.Homepage,
                station.Favicon,
                station.CountryCode,
                station.Language,
                station.Votes,
                station.Codec,
                station.Bitrate
            );

            var result = frmStationInfo.ShowDialog();

            // Logik für "Next/Previous" Navigation im Info-Fenster
            if (result == DialogResult.Yes || result == DialogResult.No)
            {
                var newIndex = index;
                var nextItem = false;

                if (result == DialogResult.Yes) // Next
                {
                    newIndex++;
                    if (newIndex < listView.Items.Count) { nextItem = true; }
                }
                else if (result == DialogResult.No) // Previous
                {
                    newIndex--;
                    if (newIndex >= 0) { nextItem = true; }
                }

                if (nextItem)
                {
                    listView.Items[index].Selected = false; // Alten deselektieren
                    listView.Items[newIndex].Selected = true;
                    listView.Items[newIndex].Focused = true;
                    listView.Items[newIndex].EnsureVisible();

                    // Rekursiver Aufruf für das neue Item
                    PropertiesToolStripMenuItem_Click(null!, null!);
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

    private void BtnDetail_Click(object sender, EventArgs e)
    {
        PropertiesToolStripMenuItem_Click(null!, null!);
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        EditToolStripMenuItem_Click(null!, null!);
    }

    private void ListView_SelectedIndexChanged(object sender, EventArgs e)
    {
        var hasSelection = listView.SelectedItems.Count > 0;
        btnEdit.Enabled = hasSelection;
        btnAccept.Enabled = hasSelection;
        btnDetail.Enabled = hasSelection;
    }

    private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        var hasSelection = listView.SelectedItems.Count > 0;
        editToolStripMenuItem.Enabled = hasSelection;
        acceptToolStripMenuItem.Enabled = hasSelection;
        propertiesToolStripMenuItem.Enabled = hasSelection;
    }

    private void ListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
    {
        if (!resizing)
        {
            resizing = true;
            var vScrollWidth = 0;
            if (NativeMethods.VerticalScrollbarVisible(listView))
            {
                vScrollWidth = SystemInformation.VerticalScrollBarWidth;
            }

            // Sichere Prüfung auf Spaltenanzahl
            if (listView.Columns.Count >= 2)
            {
                listView.Columns[0].Width = 200;
                listView.Columns[1].Width = listView.Width - 200 - vScrollWidth - 4;
            }
            resizing = false;
        }
    }
}