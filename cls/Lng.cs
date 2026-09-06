using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Resources;
using System.Windows.Forms;

namespace NetRadio.cls;

/// <summary>Mehrsprachigkeit (Englisch/Deutsch/Spanisch/Französisch, erweiterbar): Englisch ist einkompiliert und
/// bleibt der Rückfall für jeden fehlenden Eintrag; andere Sprachen liegen als Languages\lng.&lt;kultur&gt;.resx daneben,
/// wobei der SCHLÜSSEL jedes Eintrags der englische Text selbst ist. Ändert sich ein englischer Text,
/// verfällt seine Übersetzung dadurch automatisch, bis die resx nachgezogen ist — bis dahin erscheint
/// der Text auf Englisch, es bricht also nichts. (Gleiches Muster wie in PDFlight/ScanView.)</summary>
internal static class Lng
{
    private static readonly ResourceManager resources = new("NetRadio.Languages.lng", typeof(Lng).Assembly);
    private static CultureInfo? culture; // null = Englisch (keine Übersetzung nötig)

    /// <summary>Der gewählte Kultur-Code ("en", "de", …).</summary>
    public static string CultureCode { get; private set; } = "en";

    public static void Initialize(string cultureCode)
    {
        CultureCode = string.IsNullOrEmpty(cultureCode) ? "en" : cultureCode;
        try { culture = CultureCode == "en" ? null : CultureInfo.GetCultureInfo(CultureCode); }
        catch (CultureNotFoundException) { culture = null; CultureCode = "en"; }
    }

    /// <summary>Übersetzt einen englischen Text; ohne Eintrag (oder auf Englisch) kommt er unverändert zurück.
    /// Zeilenumbrüche werden für den Schlüssel zu einem sichtbaren "\n" normalisiert, denn echte Umbrüche
    /// taugen nicht als resx-Schlüssel (XML-Attribut-Normalisierung) — so laufen auch die mehrzeiligen
    /// Designer-Texte automatisch über Apply.</summary>
    [return: NotNullIfNotNull(nameof(english))]
    public static string? T(string? english)
    {
        if (culture == null || string.IsNullOrEmpty(english)) { return english; }
        var key = english.Contains('\n') ? english.Replace("\r\n", "\\n").Replace("\n", "\\n") : english;
        try { return resources.GetString(key, culture) ?? english; }
        catch (MissingManifestResourceException) { return english; }
    }

    /// <summary>Nachschlag über einen expliziten Schlüssel (für Texte, die sich als Schlüssel nicht eignen).</summary>
    public static string T(string key, string english)
    {
        if (culture == null) { return english; }
        try { return resources.GetString(key, culture) ?? english; }
        catch (MissingManifestResourceException) { return english; }
    }

    /// <summary>Übersetzt alle Texte eines Formulars samt Menüs, Spaltenköpfen und (wenn übergeben) den im
    /// Designer gesetzten WinForms-ToolTips — einmal direkt nach InitializeComponent bzw. nach dem Laden
    /// der Einstellungen aufrufen.</summary>
    public static void Apply(Control root, ToolTip? toolTip = null)
    {
        if (culture == null) { return; }
        root.Text = T(root.Text);
        TranslateChildren(root, toolTip);
    }

    /// <summary>Übersetzt ein einzelnes Menü (Kontextmenüs hängen nicht im Control-Baum des Formulars).</summary>
    public static void Apply(ToolStrip strip)
    {
        if (culture == null) { return; }
        foreach (ToolStripItem item in strip.Items) { TranslateItem(item); }
    }

    private static void TranslateChildren(Control parent, ToolTip? toolTip)
    {
        foreach (Control child in parent.Controls)
        {
            child.Text = T(child.Text);
            if (toolTip != null)
            {
                var tip = toolTip.GetToolTip(child);
                if (!string.IsNullOrEmpty(tip)) { toolTip.SetToolTip(child, T(tip)); }
            }
            switch (child)
            {
                case ToolStrip strip: // deckt auch StatusStrip ab
                    foreach (ToolStripItem item in strip.Items) { TranslateItem(item); }
                    break;
                case DataGridView dgv:
                    foreach (DataGridViewColumn col in dgv.Columns) { col.HeaderText = T(col.HeaderText); }
                    break;
                case ListView lv:
                    foreach (ColumnHeader col in lv.Columns) { col.Text = T(col.Text); }
                    break;
                case TabControl tc: // TabPage.Text läuft über die Rekursion, ToolTipText nicht
                    foreach (TabPage tp in tc.TabPages) { tp.ToolTipText = T(tp.ToolTipText); }
                    break;
            }
            TranslateChildren(child, toolTip);
        }
    }

    private static void TranslateItem(ToolStripItem item)
    {
        item.Text = T(item.Text);
        item.ToolTipText = T(item.ToolTipText);
        if (item is ToolStripDropDownItem dropDown)
        {
            foreach (ToolStripItem child in dropDown.DropDownItems) { TranslateItem(child); }
        }
    }
}
