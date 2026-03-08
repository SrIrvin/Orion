; Script de Inno Setup para Orión - Versión 1.0.0
#define MyAppName "Orión - Gestor de Mantenimiento"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "sr_irvin"
#define MyAppExeName "Orión.DesktopUI.exe"
#define MyPublishDir "Publish"

[Setup]
AppId={{ORION-MAINTENANCE-GESTOR-2026}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=.
OutputBaseFilename=Instalador_Orion_v1.0.0
Compression=lzma
SolidCompression=yes
WizardStyle=modern
SetupIconFile=Orión.DesktopUI\Assets\Icono.ico

[Languages]
Name: "spanish"; MessagesFile: "compiler:Languages\Spanish.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "{#MyPublishDir}\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
function InitializeSetup(): Boolean;
var
  ResultCode: Integer;
begin
  Result := True;
  // Verificar si el motor de Access está instalado (Chequeo de Registro OLEDB)
  if not RegKeyExists(HKLM, 'SOFTWARE\Microsoft\Office\ClickToRun\Registry\Machine\Software\Classes\CLSID\{3BE786A0-03D9-4f11-9431-2FD89721E2B3}') and
     not RegKeyExists(HKLM, 'SOFTWARE\Classes\Microsoft.ACE.OLEDB.12.0') then
  begin
    if MsgBox('Aviso: Para que el sistema de base de datos funcione correctamente, se requiere instalar el "Microsoft Access Database Engine".' + #13#10#13#10 +
              '¿Desea continuar con la instalación de todos modos? Se recomienda instalarlo después de finalizar este proceso.', mbInformation, MB_YESNO) = IDNO then
    begin
      Result := False;
    end;
  end;
end;
