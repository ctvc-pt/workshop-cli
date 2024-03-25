@echo off

set DOTNET_INSTALLER=%USERPROFILE%\Desktop\workshop-cli\Resources\dotnet-runtime-6.0.19-win-x64.exe
set TARGET_PROGRAM=%USERPROFILE%\Desktop\workshop-cli\workshop-cli\WorkshopCli\bin\Debug\net6.0\WorkshopCli.exe
set SHORTCUT_NAME=%USERPROFILE%\Desktop\workshop-cli\cli.lnk

REM Install .NET 6
echo Installing .NET 6...
start /wait %DOTNET_INSTALLER% /quiet

REM Create the shortcut
echo Creating shortcut...
powershell -Command "Start-Process -FilePath '%TARGET_PROGRAM%' -Verb RunAs; $shell = New-Object -ComObject WScript.Shell; $shortcut = $shell.CreateShortcut('%SHORTCUT_NAME%'); $shortcut.TargetPath = '%TARGET_PROGRAM%'; $shortcut.Save()"

REM Set the shortcut to run as administrator
echo Setting shortcut to run as administrator...
powershell -Command "$shell = New-Object -ComObject WScript.Shell; $shortcut = $shell.CreateShortcut('%SHORTCUT_NAME%'); $shortcut.WorkingDirectory = (Split-Path -Path '%TARGET_PROGRAM%'); $shortcut.Save()"

REM Open the shortcut
echo Opening shortcut...
start "" "%SHORTCUT_NAME%"

@REM REM Delete the batch script
@REM echo Deleting script...
@REM del "%~f0"

echo .NET 6 (64-bit) installed successfully, shortcut created, shortcut opened, and script deleted.
exit
