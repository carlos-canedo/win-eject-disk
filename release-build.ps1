# --- Configuration ---
$APP_NAME = "WinEjectDisk"
$APP_PROJ_DIR = "./WinEjectDisk/Src/App"
$OUTPUT_DIR = "./dist"
$BUILD_DIR = "./dist/app"
$VERSION = "1.0.0"
$INNO_SCRIPT = "./installer.iss"
$ISCC_PATH = "C:\Program Files (x86)\Inno Setup 6\ISCC.exe"
$EXE_PROJ_NAME = "WinEjectDisk.Src.App.exe"
$EXE_PROD_NAME = "WinEjectDisk.exe" # FIXME: this should be passed to the installer

# 1. Clean up previous builds
if (Test-Path $OUTPUT_DIR) { 
    Write-Host "Cleaning old build files..." -ForegroundColor Cyan
    Remove-Item -Recurse -Force $OUTPUT_DIR 
}
New-Item -ItemType Directory -Path $OUTPUT_DIR | Out-Null

# 2. Build and Publish the .NET App
Write-Host "Publishing $APP_NAME (Release)..." -ForegroundColor Magenta
dotnet publish $APP_PROJ_DIR `
    -c Release `
    -r win-x64 `
    --self-contained false `
    -p:PublishSingleFile=true `
    -o "$OUTPUT_DIR/app"

# 3. Compile the Inno Setup Installer
if (Test-Path $ISCC_PATH) {
    Write-Host "Compiling Installer..." -ForegroundColor Green
    # We pass the Version directly to Inno Setup as a variable
    & $ISCC_PATH "$INNO_SCRIPT" "/DAppVersion=$VERSION"
} else {
    Write-Error "Inno Setup (ISCC.exe) not found at $ISCC_PATH. Please check the path."
}

Write-Host "Build Complete! Check the $OUTPUT_DIR folder." -ForegroundColor Yellow
