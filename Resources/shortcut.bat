@echo off
:: Define the target executable and shortcut path
set TARGET_PROGRAM=%~dp0..\workshop-cli\WorkshopCli\bin\Debug\net6.0\WorkshopCli.exe
set SHORTCUT_NAME=%USERPROFILE%\Desktop\WorkshopCli.lnk

:: Create the shortcut
echo Creating shortcut...
powershell -Command "$shell = New-Object -ComObject WScript.Shell; $shortcut = $shell.CreateShortcut('%SHORTCUT_NAME%'); $shortcut.TargetPath = '%TARGET_PROGRAM%'; $shortcut.Save()"

:: Set the shortcut to run as administrator
echo Setting shortcut to run as administrator...
powershell -Command "$shell = New-Object -ComObject WScript.Shell; $shortcut = $shell.CreateShortcut('%SHORTCUT_NAME%'); $shortcut.WorkingDirectory = (Split-Path -Path '%TARGET_PROGRAM%'); $shortcut.Save()"

echo Shortcut created successfully.
exit
