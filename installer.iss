; App
#define APP_NAME "WinEjectDisk"
#define APP_ID "6C62EB1A-6F38-4C1F-AD79-F08F0AB09FA1"
#define APP_VERSION "1.0.0"

; Files
#define EXE_NAME "WinEjectDisk.exe"
#define APP_DIR ".\dist"
#define OUT_DIR ".\installer"
#define OUT_FILENAME "WinEjectDiskSetup"

[Setup]
AppName={#APP_NAME}
AppId={#APP_ID}
AppVersion={#APP_VERSION}
; Install storage settings
DefaultDirName={autopf}\{#APP_NAME}
DefaultGroupName={#APP_NAME}
; Output settings
OutputDir={#OUT_DIR}
OutputBaseFilename={#OUT_FILENAME}
; Build setting
Compression=lzma
SolidCompression=yes
; OS
PrivilegesRequired=admin

[Files]
Source: "{#APP_DIR}\{#EXE_NAME}"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
; Start menu shorcuts
Name: "{group}\{#APP_NAME}"; Filename: "{app}\{#EXE_NAME}"

[Run]
; Creates the task when the app is installed
Filename: "schtasks"; \
  Parameters: "/Create /TN ""{#APP_NAME}Task"" /TR ""'{app}\{#EXE_NAME}'"" /SC ONLOGON /RL HIGHEST /f"; \
  Flags: runhidden

[UninstallRun]
; Deletes the task when the app is uninstalled
Filename: "schtasks"; \
  Parameters: "/Delete /TN ""{#APP_NAME}Task"" /F"; \
  Flags: runhidden; RunOnceId: "RemoveAdminStartupTask"
