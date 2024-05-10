@echo off

set DOTNET_INSTALLER=%USERPROFILE%\Desktop\workshop-cli\Resources\dotnet-runtime-8.0.4-win-x64.exe
set TARGET_PROGRAM=%USERPROFILE%\Desktop\workshop-cli\workshop-cli\WorkshopCli\bin\Debug\net6.0\WorkshopCli.exe
set SHORTCUT_NAME=%USERPROFILE%\Desktop\cli.lnk
set SESSION_FILE=C:\Users\info\Desktop\workshop-cli\Resources\session.txt

REM Install .NET 8
echo Installing .NET 8...
start /wait "" runas %DOTNET_INSTALLER% /quiet

REM Delete session file if exists
if exist "%SESSION_FILE%" (
    echo Deleting session file...
    del "%SESSION_FILE%"
)

REM Create the shortcut
echo Creating shortcut...
powershell -Command "Start-Process -FilePath '%TARGET_PROGRAM%' -Verb RunAs; $shell = New-Object -ComObject WScript.Shell; $shortcut = $shell.CreateShortcut('%SHORTCUT_NAME%'); $shortcut.TargetPath = '%TARGET_PROGRAM%'; $shortcut.Save()"

REM Set the shortcut to run as administrator
echo Setting shortcut to run as administrator...
powershell -Command "$shell = New-Object -ComObject WScript.Shell; $shortcut = $shell.CreateShortcut('%SHORTCUT_NAME%'); $shortcut.WorkingDirectory = (Split-Path -Path '%TARGET_PROGRAM%'); $shortcut.Save()"

REM Open the shortcut
echo Opening shortcut...
start "" "%SHORTCUT_NAME%"

echo .NET 6 (64-bit) installed successfully, shortcut created, shortcut opened, and script completed.
exit
