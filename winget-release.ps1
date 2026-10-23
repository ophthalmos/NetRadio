# winget-release.ps1
# Reicht ein neues NetRadio-Release an das WinGet-Community-Repository weiter.
# Voraussetzung: wingetcreate ist installiert (winget install wingetcreate)
# Voraussetzung: Die Setup-Datei ist bereits unter der URL öffentlich erreichbar.
#
# Ablauf: wingetcreate erzeugt die Manifeste zuerst lokal (--out). Dort werden die Adressfelder
# auf www.netradio.info gesetzt, denn "wingetcreate update" ersetzt nur die Installer-URL und
# uebernimmt alle uebrigen Felder aus dem zuletzt veroeffentlichten Manifest (bis 2.5.6.0 noch
# mit der alten Domain ophthalmostar.de). Erst nach Bestaetigung folgt "wingetcreate submit".

$url = "https://www.netradio.info/download/NetRadioSetup.exe"

# Feste Werte fuer das Gebietsschema-Manifest (werden bei jedem Release gesetzt)
$localeFields = [ordered]@{
    'PublisherUrl' = 'https://www.netradio.info/'
    'PrivacyUrl'   = 'https://www.netradio.info/web-privacy-policy/'
    'PackageUrl'   = 'https://www.netradio.info/app/'
    'DocumentUrl'  = 'https://www.netradio.info/download/NetRadio.pdf'   # FAQ
}

Write-Host ""
Write-Host "WinGet-Release fuer WilhelmHappe.NetRadio"
Write-Host "------------------------------------------"
Write-Host "Format: x.y.z.0  (Beispiel: 2.6.0.0)"
Write-Host "Die letzte Stelle ist fast immer 0."
Write-Host ""

do {
    $version = (Read-Host "Versionsnummer eingeben").Trim()
    $valid = $version -match '^\d+\.\d+\.\d+\.\d+$'
    if (-not $valid) {
        Write-Warning "Ungueltig. Bitte im Format x.y.z.0 eingeben (z. B. 2.6.0.0)."
    }
} while (-not $valid)

$out = Join-Path $env:TEMP "NetRadio-winget-$version"
if (Test-Path $out) { Remove-Item $out -Recurse -Force }

Write-Host ""
Write-Host "Erzeuge die Manifeste fuer Version $version lokal ..."
Write-Host ""

wingetcreate update WilhelmHappe.NetRadio `
    --version $version `
    --urls "$url|x64" `
    --out $out

$manifests = @(Get-ChildItem $out -Recurse -Filter *.yaml -ErrorAction SilentlyContinue)
if ($LASTEXITCODE -ne 0 -or $manifests.Count -eq 0) {
    Write-Warning "wingetcreate hat keine Manifeste erzeugt - Abbruch, nichts eingereicht."
    $x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

# Adressfelder im Gebietsschema-Manifest setzen (zeilenweise, Einrueckung bleibt erhalten)
$utf8 = New-Object System.Text.UTF8Encoding($false)
foreach ($file in $manifests | Where-Object { $_.Name -like '*.locale.*.yaml' }) {
    $text = [IO.File]::ReadAllText($file.FullName)
    foreach ($key in $localeFields.Keys) {
        $text = [regex]::Replace($text, "(?m)^(\s*(?:-\s+)?$key):.*$", "`$1: $($localeFields[$key])")
    }
    [IO.File]::WriteAllText($file.FullName, $text, $utf8)
}

# Sicherheitsnetz: Steht die alte Domain noch irgendwo, wird nicht eingereicht
$old = @($manifests | Select-String -Pattern 'ophthalmostar' -SimpleMatch)
if ($old.Count -gt 0) {
    Write-Warning "Die alte Domain steht noch in den Manifesten - Abbruch, nichts eingereicht:"
    $old | ForEach-Object { Write-Host "  $($_.Filename):$($_.LineNumber): $($_.Line.Trim())" }
    $x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 1
}

Write-Host ""
Write-Host "Erzeugte Manifeste ($($manifests[0].DirectoryName)):"
foreach ($file in $manifests) {
    Write-Host ""
    Write-Host "----- $($file.Name)" -ForegroundColor Cyan
    Get-Content $file.FullName -Encoding UTF8 | Where-Object { $_ -notmatch '^#' -and $_.Trim() -ne '' } | ForEach-Object { Write-Host "  $_" }
}

Write-Host ""
$answer = (Read-Host "Jetzt bei WinGet einreichen? (j/n)").Trim()
if ($answer -notmatch '^[jJyY]') {
    Write-Host "Nicht eingereicht. Die Manifeste liegen in $out."
    $x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
    exit 0
}

wingetcreate submit $manifests[0].DirectoryName `
    --prtitle "New version: WilhelmHappe.NetRadio version $version"

Write-Host
Write-Host "Fertig!`nDrücken Sie eine beliebige Taste..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
