# winget-release.ps1
# Reicht ein neues NetRadio-Release an das WinGet-Community-Repository weiter.
# Voraussetzung: wingetcreate ist installiert (winget install wingetcreate)
# Voraussetzung: Die Setup-Datei ist bereits unter der URL öffentlich erreichbar.

$url = "https://www.netradio.info/download/NetRadioSetup.exe"

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

Write-Host ""
Write-Host "Starte wingetcreate fuer Version $version ..."
Write-Host ""

wingetcreate update WilhelmHappe.NetRadio `
    --version $version `
    --urls "$url|x64" `
    --submit

Write-Host
Write-Host "Fertig!`nDrücken Sie eine beliebige Taste..."
$x = $host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
