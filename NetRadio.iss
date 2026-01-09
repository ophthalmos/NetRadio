#define MyAppName "NetRadio"
#define MyAppVersion "2.5.10"

[Setup]
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppVerName={#MyAppName} {#MyAppVersion}
VersionInfoVersion={#MyAppVersion}
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64
PrivilegesRequired=admin
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
MinVersion=0,10.0
SetupMutex=NetRadioSetupMutex

[Files]
Source: "bin\Release\net8.0-windows8.0\NetRadio.exe"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net8.0-windows8.0\{#MyAppName}.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net8.0-windows8.0\{#MyAppName}.runtimeconfig.json"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net8.0-windows8.0\bass.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net8.0-windows8.0\bassflac.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net8.0-windows8.0\bassopus.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net8.0-windows8.0\basshls.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
Source: "bin\Release\net8.0-windows8.0\Bass.Net.dll"; DestDir: "{app}"; Permissions: users-modify; Flags: ignoreversion
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

[UnInstallDelete]
Type: filesandordirs; Name: {userappdata}\{#MyAppName}\{#MyAppName}*
Type: dirifempty; Name: {userappdata}\{#MyAppName}

[Languages]
Name: "en"; MessagesFile: "compiler:Default.isl"; LicenseFile: "LicenseAgreement.txt"

[Registry]
Root: HKCU; Subkey: "Software\{#MyAppName}"; Flags: uninsdeletekey
Root: HKCU; Subkey: "Software\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "NetRadio"; Flags: dontcreatekey uninsdeletevalue

[Run]
Filename: "{app}\{#MyAppName}.exe"; Description: "Launch {#MyAppName}"; Flags: postinstall nowait skipifsilent runasoriginaluser
Filename: "{app}\{#MyAppName}.pdf"; Description: "View Frequently Asked Questions (PDF)"; Flags: postinstall shellexec runasoriginaluser

[Messages]
BeveledLabel=
en.WinVersionTooLowError=This program requires Windows 2000 or higher.
en.ConfirmUninstall=Are you sure you want to remove %1 and all of its components? Uninstallation is not necessary before an update.

[CustomMessages]
RemoveSettings=Do you want to remove all custom settings set for NetRadio?
IsDonateHint=Support NetRadio - Thank you!

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
  // WizardForm.LicenseAcceptedRadio.Checked := True;
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
