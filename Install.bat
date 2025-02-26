@echo off

set DOTNET_INSTALLER=C:\Projects\workshop-cli\Resources\dotnet-sdk-6.0.425-win-x64.exe
set PYTHON_INSTALLER=C:\Projects\workshop-cli\Resources\python-installer.exe
set GIT_INSTALLER=C:\Projects\workshop-cli\Resources\Git-2.46.2-64-bit.exe
set VSCODE_INSTALLER=C:\Projects\workshop-cli\Resources\VSCodeSetup.exe
set TARGET_PROGRAM=C:\Projects\workshop-cli\workshop-cli\WorkshopCli\bin\Debug\net6.0\WorkshopCli.exe
set SHORTCUT_NAME=C:\Projects\cli.lnk
set SESSION_FILE=C:\Projects\workshop-cli\Resources\session.txt

REM Install .NET 6
echo Installing .NET 6...
start /wait "" %DOTNET_INSTALLER% /quiet

REM Install Python
echo Installing Python...
start /wait "" %PYTHON_INSTALLER% /quiet

REM Install PyGithub
echo Installing PyGithub...
python -m pip install PyGithub

REM Install Git
echo Installing Git...
start /wait "" %GIT_INSTALLER% /VERYSILENT /NORESTART

REM Install Visual Studio Code
echo Installing Visual Studio Code...
start /wait "" %VSCODE_INSTALLER% /VERYSILENT /NORESTART

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

@REM REM Delete the batch script
@REM echo Deleting script...
@REM del "%~f0"

echo Installations completed successfully, shortcut created, and script finished.
exit
