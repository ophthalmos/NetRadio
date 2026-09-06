# Prüft die Übersetzungs-Konsistenz des Lng-Systems (ENGLISCHER Text = resx-Schlüssel):
# LNG001: Ein übersetzbarer englischer Text (Lng.T im Code, Text/ToolTipText/HeaderText-Zuweisungen,
#         SetToolTip-Aufrufe, Combo-Listen, Texte aus den Form-resx) hat in einer Sprachdatei keinen Eintrag — vermutlich
#         wurde englischer Text geändert, ohne die Schlüssel nachzuziehen.
# LNG002: Ein resx-Schlüssel kommt im Code nicht mehr vor — Altlast nach einer Umformulierung.
# Besonderheit: Mehrzeilige Texte sind als Schlüssel erlaubt — Lng.T normalisiert Zeilenumbrüche
# zu einem sichtbaren "\n" (so stehen sie auch in den resx-Schlüsseln).
# Grenzen: String-Konstanten sieht der Scanner nicht (siehe Ignorierliste). Ausgabe im
# MSBuild-Warnungsformat; läuft als Build-Target nach jedem Build (s. NetRadio.csproj) und jederzeit
# manuell:  powershell -ExecutionPolicy Bypass -File check-lng.ps1

# -Strict (Release-Build): Funde als Fehler statt Warnungen melden und mit Exit-Code 1 abbrechen
param([switch]$Strict)
$severity = if ($Strict) { "error" } else { "warning" }
$root = $PSScriptRoot
$languages = "de", "fr", "es"

# Texte, die absichtlich in keiner Sprachdatei stehen (Markennamen, sprachneutrale Angaben,
# Designer-Platzhalter, die zur Laufzeit sofort überschrieben werden)
$ignore = @(
    "NetRadio", "OK", "URL", "GNU", "Homepage", "Miniplayer", "MiniPlayer (Esc)",
    "Deutsch", "English", "Español", "Français", # Eigennamen der Sprachen — erscheinen bewusst in der jeweiligen Sprache
    "Wilhelm Happe, Kiel, Germany", "github.com/ophthalmos/NetRadio", "radio-browser.info",
    "www.netradio.info/app", "www.radio42.com/bass", "www.un4seen.com",
    "frmBrowser", "frmStationInfo", "label1", "statusStrip", "WaitWindow", "name", "row" # Designer-Platzhalter
)

# ---------------------------------------------------------------- Schlüssel der Sprachdateien
$langKeys = @{}
foreach ($code in $languages) {
    $file = Join-Path $root "Languages\lng.$code.resx"
    [xml]$xml = Get-Content $file -Raw -Encoding UTF8
    $keys = New-Object 'System.Collections.Generic.HashSet[string]'
    foreach ($data in $xml.root.data) { [void]$keys.Add([string]$data.name) }
    $langKeys[$code] = $keys
}

# ---------------------------------------------------------------- verwendete Schlüssel sammeln
# Zeilenumbruch-Escapes bleiben als sichtbares "\n" erhalten — wie Lng.T sie normalisiert
function ConvertFrom-CSharpLiteral([string]$s) {
    [regex]::Replace($s, '\\(.)', { param($m)
        switch ($m.Groups[1].Value) { 'n' { '\n' } 'r' { '' } 't' { "`t" } default { $m.Groups[1].Value } } })
}

$literal = '"((?:[^"\\]|\\.)*)"'
$verbatim = '@"((?:[^"]|"")*)"'
$tokens = '@"(?<v>(?:[^"]|"")*)"|"(?<s>(?:[^"\\]|\\.)*)"|' + "'" + '(?:\\.|[^' + "'" + '\\])' + "'" + '|//[^\r\n]*|/\*[\s\S]*?\*/'
$used = New-Object 'System.Collections.Generic.HashSet[string]'
$sources = Get-ChildItem $root -Recurse -Include *.cs -File | Where-Object { $_.FullName -notmatch '\\(obj|bin|\.claude)\\' }
foreach ($file in $sources) {
    $text = Get-Content $file.FullName -Raw -Encoding UTF8
    # 1) direkte Lng.T-Aufrufe (auch innerhalb interpolierter Strings; Verbatim-Variante mit @"…")
    foreach ($m in [regex]::Matches($text, "Lng\.T\(\s*$literal")) {
        [void]$used.Add((ConvertFrom-CSharpLiteral $m.Groups[1].Value))
    }
    foreach ($m in [regex]::Matches($text, "Lng\.T\(\s*$verbatim")) {
        [void]$used.Add($m.Groups[1].Value.Replace('""', '"'))
    }
    # 2) Ternary-Argumente: Lng.T(bedingung ? "A" : "B") — beide Zweige sind Schlüssel
    foreach ($m in [regex]::Matches($text, "Lng\.T\([^`"()]*\?\s*$literal\s*:\s*$literal\s*\)")) {
        [void]$used.Add((ConvertFrom-CSharpLiteral $m.Groups[1].Value))
        [void]$used.Add((ConvertFrom-CSharpLiteral $m.Groups[2].Value))
    }
    # 3) übersetzte Eigenschafts-Zuweisungen (Designer wie Code; nur reine Literale bis zum Semikolon)
    foreach ($m in [regex]::Matches($text, "\b(?:Text|ToolTipText|ShortcutKeyDisplayString|HeaderText)\s*=\s*$literal\s*;")) {
        [void]$used.Add((ConvertFrom-CSharpLiteral $m.Groups[1].Value))
    }
    # 4) Designer-Tooltips über die ToolTip-Komponente (Lng.Apply übersetzt sie zur Laufzeit)
    foreach ($m in [regex]::Matches($text, "\.SetToolTip\([^,]+,\s*$literal\s*\)")) {
        [void]$used.Add((ConvertFrom-CSharpLiteral $m.Groups[1].Value))
    }
    # 5) Combo-Listen des Designers und Dictionary-Werte — übersetzt die Anwendung zur Laufzeit
    foreach ($m in [regex]::Matches($text, "Items\.AddRange\(new object\[\]\s*\{([^}]*)\}")) {
        foreach ($s in [regex]::Matches($m.Groups[1].Value, $literal)) { [void]$used.Add((ConvertFrom-CSharpLiteral $s.Groups[1].Value)) }
    }
    foreach ($m in [regex]::Matches($text, "\]\s*=\s*$literal\s*,?\s*(?:\r?\n|\})")) {
        [void]$used.Add((ConvertFrom-CSharpLiteral $m.Groups[1].Value))
    }
    # 6) Rettungsregel gegen Fehlalarme: Jedes Literal, das exakt einem vorhandenen resx-Schlüssel
    #    entspricht, gilt als verwendet — deckt Felder, switch-Ausdrücke, Dialog-Filter usw. ab,
    #    ohne LNG001 aufzuweichen (es zählen nur Texte, die bereits übersetzt sind). Tokenisiert
    #    Strings, Kommentare und Zeichenliterale gemeinsam, damit Anführungszeichen in Kommentaren
    #    die Paarung nicht verschieben.
    foreach ($m in [regex]::Matches($text, $tokens)) {
        $value = $null
        if ($m.Groups['v'].Success) { $value = $m.Groups['v'].Value.Replace('""', '"') }
        elseif ($m.Groups['s'].Success) { $value = ConvertFrom-CSharpLiteral $m.Groups['s'].Value }
        if ($value -and ($languages | Where-Object { $langKeys[$_].Contains($value) })) { [void]$used.Add($value) }
    }
}

# 7) Texte, die der Designer statt in die .Designer.cs in die Form-resx ausgelagert hat (z. B. splashLabel.Text
#    in frmSplash.resx) — Lng.Apply übersetzt sie zur Laufzeit genauso; Zeilenumbrüche wie in Lng.T zu "\n"
foreach ($file in Get-ChildItem $root -Recurse -Include *.resx -File | Where-Object { $_.FullName -notmatch '\\(obj|bin|\.claude|Languages)\\' }) {
    [xml]$xml = Get-Content $file.FullName -Raw -Encoding UTF8
    foreach ($data in $xml.root.data) {
        if ([string]$data.name -notmatch '\.(Text|ToolTipText|HeaderText)$') { continue }
        $value = ([string]$data.value) -replace "`r`n", '\n' -replace "`n", '\n'
        if ($value) { [void]$used.Add($value) }
    }
}

# ---------------------------------------------------------------- LNG001: fehlende Übersetzungen
$findings = 0
foreach ($key in $used | Sort-Object) {
    if ($key -notmatch '\p{L}') { continue }     # ohne Buchstaben (z.B. "0/0") gibt es nichts zu übersetzen
    if ($key -match '\{\D') { continue }         # Fragment eines interpolierten Strings, kein Schlüssel ({0} bleibt erlaubt)
    if ($key -match '^(F\d+|Alt\+.+|Ctrl\+.+|Strg\+.+|[A-Z]|Del|Ins|Enter|Esc|Shift\+Esc|DoubleClick|\(Shift \+\) Ctrl \+ Win \+)$') { continue } # sprachneutrale Kürzel und Hotkey-Tasten
    if ($ignore -contains $key) { continue }
    foreach ($code in ($languages | Where-Object { -not $langKeys[$_].Contains($key) })) {
        Write-Output "Languages\lng.$code.resx : $severity LNG001: Übersetzung fehlt für Schlüssel: `"$key`""
        $findings++
    }
}

# ---------------------------------------------------------------- LNG002: verwaiste resx-Schlüssel
foreach ($code in $languages) {
    foreach ($key in $langKeys[$code] | Sort-Object) {
        if ($ignore -contains $key) { continue }
        if (-not $used.Contains($key)) {
            Write-Output "Languages\lng.$code.resx : $severity LNG002: Verwaister Schlüssel (im Code nicht gefunden): `"$key`""
            $findings++
        }
    }
}

if ($findings -eq 0) { Write-Output "check-lng: Alle Übersetzungen konsistent ($($used.Count) Schlüssel geprüft)." }
if ($Strict -and $findings -gt 0) { exit 1 }
exit 0
