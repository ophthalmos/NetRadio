using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace NetRadio.cls;

public sealed class SpectrumDisplay : Control
{
    private const int Bands = 20;
    private const float MinDb = -60f;             // untere Grenze der Skala
    private const float AttackFactor = 0.55f;     // Glättung aufwärts (1 = ungebremst) - höher = reaktiver
    private const float ReleaseFactor = 0.18f;    // Glättung abwärts - niedriger = ruhigeres Abfallen
    private const float PeakFallDbPerTick = 0.4f; // Peak-Striche fallen langsam (bei 47 ms Timerintervall ~8,5 dB/s)
    private const int PeakHoldTicks = 15;         // Haltezeit der Peaks in Timer-Ticks (bei 47 ms = ~700 ms)
    private const float GainDb = 7.2f;            // verschiebt Anzeige (nicht Messung), damit 0dB erreichbar, alle Werte erlaubt (z.B. 7.5f)
    private const int ScaleWidth = 20;            // reservierter Bereich rechts für die dB-Beschriftung
    private const int FreqHeight = 14;            // reservierter Bereich unten für die Frequenzbeschriftung

    private static readonly Color BarTopColor = Color.CornflowerBlue;     // Balkenverlauf oben (hell) FromArgb(0x2B, 0x6C, 0xC4)
    private static readonly Color BarBottomColor = Color.DarkBlue;        // Balkenverlauf unten (dunkel) FromArgb(0x02, 0x08, 0x18)
    private static readonly Color PeakColor = Color.DimGray;              // Peak-Hold-Striche
    private static readonly Color GridColor = Color.Gainsboro;            // Gitterlinien
    private static readonly Color DbLabelColor = Color.Black;             // dB-Beschriftung rechts
    private static readonly Color FreqLabelColor = Color.MidnightBlue;    // Frequenzbeschriftung unten FromArgb(64, 64, 64)
    private static readonly Color ScaleBackTopColor = Color.GhostWhite;   // Hintergrund der Skalenbereiche: Verlauf oben ...
    private static readonly Color ScaleBackBottomColor = Color.Gainsboro; // ... nach unten; beide gleich setzen = einfarbig

    private readonly float[] _bandDb = new float[Bands];
    private readonly float[] _peakDb = new float[Bands];
    private readonly int[] _peakHold = new int[Bands];
    private readonly float[] _bandUpperFreq = new float[Bands]; // obere Grenzfrequenz je Band
    private readonly string[] _bandLabel = new string[Bands];   // Beschriftung (50, 69, ..., 20K)

    public SpectrumDisplay()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer |
                 ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        for (var i = 0; i < Bands; i++)
        {
            _bandDb[i] = _peakDb[i] = MinDb;
            _bandUpperFreq[i] = 50f * MathF.Pow(20000f / 50f, (i + 1) / (float)Bands); // logarithmische Bandgrenzen
            _bandLabel[i] = FormatFrequency(50f * MathF.Pow(20000f / 50f, i / (float)(Bands - 1)));
        }
    }

    private static string FormatFrequency(float hz) => hz < 1000f ? MathF.Round(hz).ToString() : hz < 9950f ? (hz / 1000f).ToString("0.0") + "K" : MathF.Round(hz / 1000f) + "K";

    public void SetFft(float[] fft, int sampleRate)
    {
        if (fft.Length < 1024 || sampleRate <= 0) { return; }
        var binWidth = sampleRate / 2f / 1024f; // Hz pro FFT-Bin
        var b0 = Math.Max(1, (int)(50f / binWidth)); // Bänder beginnen bei 50 Hz (Bin 0 = DC)
        for (var i = 0; i < Bands; i++)
        {
            var b1 = Math.Clamp((int)(_bandUpperFreq[i] / binWidth), b0 + 1, 1023);

            // Bandenergie (Leistungssumme) statt Spitzenwert: unterdrückt grobe Einzelausschläge und
            // gleicht die Bänder aus - breite (hohe) Bänder integrieren über mehr Bins und fallen
            // dadurch nicht mehr systematisch ab wie beim RMS-Mittelwert.
            var sum = 0f;
            for (; b0 < b1; b0++) { sum += fft[b0] * fft[b0]; }
            var db = sum > 0f ? Math.Clamp(10f * MathF.Log10(sum) + GainDb, MinDb, 0f) : MinDb;

            // Zeitliche Glättung: schneller Anstieg (Attack), gebremstes Abfallen (Release)
            _bandDb[i] += (db - _bandDb[i]) * (db > _bandDb[i] ? AttackFactor : ReleaseFactor);

            if (_bandDb[i] >= _peakDb[i]) { _peakDb[i] = _bandDb[i]; _peakHold[i] = PeakHoldTicks; } // neuer Peak: halten
            else if (_peakHold[i] > 0) { _peakHold[i]--; }                                           // Haltezeit läuft
            else { _peakDb[i] = Math.Max(MinDb, _peakDb[i] - PeakFallDbPerTick); }                   // langsam fallen
        }
        Invalidate();
    }

    public void ClearBars()  // Blendet nur die Balken aus - die Peak-Hold-Striche bleiben
    {
        for (var i = 0; i < Bands; i++) { _bandDb[i] = MinDb; }
        Invalidate();
    }

    public void Clear()  // Balken und Peaks zurück (z. B. bei Stopp/Reset/Senderwechsel)
    {
        for (var i = 0; i < Bands; i++)
        {
            _bandDb[i] = _peakDb[i] = MinDb;
            _peakHold[i] = 0;
        }
        Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        var g = e.Graphics;
        var plotW = ClientSize.Width - ScaleWidth; // Balkenbereich; rechts bleibt Platz für die Skala
        var plotH = ClientSize.Height - FreqHeight; // unten bleibt Platz für die Frequenzbeschriftung
        if (plotW < Bands * 2 || plotH < 30) { return; }
        g.Clear(BackColor);

        using (var scaleBrush = new LinearGradientBrush(new Rectangle(0, 0, ClientSize.Width, ClientSize.Height),
                   ScaleBackTopColor, ScaleBackBottomColor, LinearGradientMode.Vertical))
        {
            g.FillRectangle(scaleBrush, plotW, 0, ClientSize.Width - plotW, ClientSize.Height); // rechte Skalenspalte
            g.FillRectangle(scaleBrush, 0, plotH, plotW, ClientSize.Height - plotH);            // unterer Beschriftungsstreifen
        }

        using var labelFont = new Font("Segoe UI", 6.5f);

        using (var gridPen = new Pen(GridColor))
        using (var dbBrush = new SolidBrush(DbLabelColor))
        {
            for (var db = 0; db >= (int)MinDb; db -= 10)
            {
                var y = (int)(db / MinDb * (plotH - 1));
                if (db != 0) { g.DrawLine(gridPen, 0, y, plotW - 2, y); }
                var text = $"{-db}dB";
                var size = g.MeasureString(text, labelFont);
                g.DrawString(text, labelFont, dbBrush, ClientSize.Width - size.Width, Math.Clamp(y - size.Height / 2f, 0f, ClientSize.Height - size.Height));
            }
        }

        var pitch = (float)plotW / Bands;
        var barWidth = MathF.Max(2f, pitch - 2f);
        using var barBrush = new LinearGradientBrush(new Rectangle(0, 0, plotW, plotH), BarTopColor, BarBottomColor, LinearGradientMode.Vertical);
        using var peakPen = new Pen(PeakColor, 2f);
        using var freqBrush = new SolidBrush(FreqLabelColor);
        using var freqFormat = new StringFormat { Alignment = StringAlignment.Center };
        for (var i = 0; i < Bands; i++)
        {
            var x = i * pitch + 1f;
            var barH = (1f - _bandDb[i] / MinDb) * (plotH - 1);
            if (barH >= 1f) { g.FillRectangle(barBrush, x, plotH - barH, barWidth, barH); }

            var peakH = (1f - _peakDb[i] / MinDb) * (plotH - 1);
            if (peakH >= 1f) { g.DrawLine(peakPen, x, plotH - peakH, x + barWidth, plotH - peakH); } // Peak-Hold-Strich

            g.DrawString(_bandLabel[i], labelFont, freqBrush, x + barWidth / 2f, plotH + 1, freqFormat); // Frequenz unter dem Balken
        }
    }
}
