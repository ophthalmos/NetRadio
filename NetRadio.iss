#define MyAppName "NetRadio"
#define MyAppVersion "2.6.0"

[Setup]
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
; Admin bewusst beibehalten: Ohne installierte .NET-10-Desktop-Runtime sind ohnehin
; einmalig Adminrechte nötig, eine Per-User-Variante brächte daher keinen echten Vorteil.
PrivilegesRequired=admin
; HKCU/{userdesktop} werden bewusst verwendet - Compiler-Warnung dazu unterdrücken
UsedUserAreasWarning=no
AppPublisher=Wilhelm Happe
VersionInfoCopyright=(C) 2026, W. Happe
AppPublisherURL=https://www.netradio.info/
AppSupportURL=https://www.netradio.info/
AppUpdatesURL=https://www.netradio.info/
DefaultDirName={autopf}\{#MyAppName}
DisableWelcomePage=yes
DisableDirPage=no
DisableReadyPage=yes
CloseApplications=yes
WizardStyle=modern
WizardSizePercent=100
SetupIconFile=img\NetRadio.ico
UninstallDisplayIcon={app}\NetRadio.exe
DefaultGroupName=NetRadio
AppId=NetRadio
TimeStampsInUTC=yes
OutputDir=.
OutputBaseFilename={#MyAppName}Setup
Compression=lzma2/max
SolidCompression=yes
DirExistsWarning=no
; Windows 10 Version 1607 (Build 14393) = Minimum der .NET-10-Runtime
MinVersion=10.0.14393
SetupMutex=NetRadioSetupMutex

[Files]
Source: "bin\Release\net10.0-windows7.0\NetRadio.exe"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\{#MyAppName}.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\{#MyAppName}.runtimeconfig.json"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\bass.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\bassflac.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\bassopus.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\basshls.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\Bass.Net.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\de\NetRadio.resources.dll"; DestDir: "{app}\de"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\es\NetRadio.resources.dll"; DestDir: "{app}\es"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net10.0-windows7.0\fr\NetRadio.resources.dll"; DestDir: "{app}\fr"; Permissions: users-modify; Flags: ignoreversion
Source: "Lizenzvereinbarung.txt"; DestDir: "{app}"; Permissions: users-modify;
Source: "LicenseAgreement.txt"; DestDir: "{app}"; Permissions: users-modify;
Source: "NetRadio.pdf"; DestDir: "{app}"; Permissions: users-modify;
Source: "img\isdonate.bmp"; Flags: dontcopy

[Icons]
Name: "{userdesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppName}.exe"
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppName}.exe"

[InstallDelete]
Type: filesandordirs; Name: "{group}"
Type: files; Name: "{app}\{#MyAppName}.exe.config"
Type: files; Name: "{app}\bass_aac.dll"

[Languages]
; Die Namen sind bewusst die Kultur-Codes der Programmsprachen: {language} wird unten
; unverändert in die Registry geschrieben und beim ersten Programmstart übernommen.
Name: "en"; MessagesFile: "compiler:Default.isl"; LicenseFile: "LicenseAgreement.txt"
Name: "de"; MessagesFile: "compiler:Languages\German.isl"; LicenseFile: "Lizenzvereinbarung.txt"
Name: "es"; MessagesFile: "compiler:Languages\Spanish.isl"; LicenseFile: "LicenseAgreement.txt"
Name: "fr"; MessagesFile: "compiler:Languages\French.isl"; LicenseFile: "LicenseAgreement.txt"

[Registry]
Root: HKCU; Subkey: "Software\{#MyAppName}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "NetRadio"; Flags: dontcreatekey uninsdeletevalue

[Run]
; /l übergibt die Setup-Sprachauswahl ({language} = en/de/es/fr, siehe [Languages]-Namen) an den Programmstart
Filename: "{app}\{#MyAppName}.exe"; Parameters: "/l {language}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: postinstall nowait skipifsilent runasoriginaluser
Filename: "{app}\{#MyAppName}.pdf"; Description: "{cm:ViewFaq}"; Flags: postinstall shellexec runasoriginaluser

[Messages]
BeveledLabel=
en.WinVersionTooLowError=This program requires Windows 10 (version 1607) or higher.
en.ConfirmUninstall=Are you sure you want to remove %1 and all of its components? Uninstallation is not necessary before an update.
de.WinVersionTooLowError=Dieses Programm benötigt Windows 10 (Version 1607) oder höher.
de.ConfirmUninstall=Soll %1 mit allen Komponenten wirklich entfernt werden? Vor einem Update ist keine Deinstallation nötig.
es.WinVersionTooLowError=Este programa requiere Windows 10 (versión 1607) o superior.
es.ConfirmUninstall=¿Seguro que quieres eliminar %1 y todos sus componentes? No es necesario desinstalar antes de una actualización.
fr.WinVersionTooLowError=Ce programme nécessite Windows 10 (version 1607) ou ultérieur.
fr.ConfirmUninstall=Voulez-vous vraiment supprimer %1 et tous ses composants ? Une désinstallation n'est pas nécessaire avant une mise à jour.

[CustomMessages]
en.RemoveSettings=Do you also want to remove the NetRadio settings, station lists and history of all user accounts on this computer?%nThis deletes the "AppData\Roaming\NetRadio" folder in every user profile.
en.IsDonateHint=Support NetRadio - Thank you!
en.ViewFaq=View Frequently Asked Questions (PDF)
de.RemoveSettings=Sollen auch die NetRadio-Einstellungen, Senderlisten und Verläufe aller Benutzerkonten dieses Computers entfernt werden?%nDabei wird in jedem Benutzerprofil der Ordner "AppData\Roaming\NetRadio" gelöscht.
de.IsDonateHint=Unterstütze NetRadio - Danke!
de.ViewFaq=Häufig gestellte Fragen anzeigen (PDF)
es.RemoveSettings=¿Quieres eliminar también los ajustes, las listas de emisoras y el historial de NetRadio de todas las cuentas de usuario de este equipo?%nSe borrará la carpeta "AppData\Roaming\NetRadio" de cada perfil de usuario.
es.IsDonateHint=Apoya a NetRadio - ¡Gracias!
es.ViewFaq=Ver preguntas frecuentes (PDF)
fr.RemoveSettings=Voulez-vous aussi supprimer les réglages, les listes de stations et l'historique de NetRadio pour tous les comptes de cet ordinateur ?%nLe dossier "AppData\Roaming\NetRadio" sera supprimé dans chaque profil utilisateur.
fr.IsDonateHint=Soutenez NetRadio - Merci !
fr.ViewFaq=Afficher la FAQ (PDF)

[Code]
procedure DonateImageOnClick(Sender: TObject);
var
  ErrorCode: Integer;
begin
  ShellExecAsOriginalUser('open', 'https://www.paypal.com/donate/?hosted_button_id=3HRQZCUW37BQ6', '', '', SW_SHOWNORMAL, ewNoWait, ErrorCode);
end;

procedure InitializeWizard;
var
  ImageFileName: String;
  DonateImage: TBitmapImage;
  BevelTop: Integer;
begin
  ImageFileName := ExpandConstant('{tmp}\isdonate.bmp');
  ExtractTemporaryFile(ExtractFileName(ImageFileName));
  DonateImage := TBitmapImage.Create(WizardForm);
  DonateImage.AutoSize := True;
  DonateImage.Bitmap.LoadFromFile(ImageFileName);
  DonateImage.Hint := CustomMessage('IsDonateHint');
  DonateImage.ShowHint := True;
  DonateImage.Anchors := [akLeft, akBottom];
  BevelTop := WizardForm.Bevel.Top;
  DonateImage.Top := BevelTop + (WizardForm.ClientHeight - BevelTop - DonateImage.Bitmap.Height) div 2;
  DonateImage.Left := DonateImage.Top - BevelTop;
  DonateImage.Cursor := crHand;
  DonateImage.OnClick := @DonateImageOnClick;
  DonateImage.Parent := WizardForm;
end;

procedure DeleteDataDir(const DataDir: String);
begin
  if DirExists(DataDir) then
  begin
    if not DelTree(DataDir, True, True, True) then
    begin
      MsgBox('The folder "' + DataDir + '" could not be removed completely.', mbError, MB_OK);
    end;
  end;
end;

// Admin-Modus: NetRadio-AppData-Ordner in allen Benutzerprofilen löschen.
// Profilpfade aus der ProfileList-Registry; Systemprofile (%systemroot%\...) fallen
// durch den DirExists-Test in DeleteDataDir automatisch heraus.
const
  ProfileList = 'SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList';
procedure DeleteAllUsersData;
var
  Names: TArrayOfString;
  ProfilePath: String;
  I: Integer;
begin
  if RegGetSubkeyNames(HKLM, ProfileList, Names) then
  begin
    for I := 0 to GetArrayLength(Names) - 1 do
    begin
      if RegQueryStringValue(HKLM, ProfileList + '\' + Names[I], 'ProfileImagePath', ProfilePath) then
      begin
        DeleteDataDir(ProfilePath + '\AppData\Roaming\{#MyAppName}');
      end;
    end;
  end;
end;

procedure CurUninstallStepChanged(CurUninstallStep: TUninstallStep);
begin
  // Bei stiller Deinstallation (/SILENT, /VERYSILENT) keine Rückfrage - Daten bleiben erhalten.
  if (CurUninstallStep = usUninstall) and not UninstallSilent then
  begin
    if MsgBox(CustomMessage('RemoveSettings'), mbConfirmation, MB_YESNO or MB_DEFBUTTON2) = IDYES then
    begin
      DeleteAllUsersData;
    end;
  end;
end;