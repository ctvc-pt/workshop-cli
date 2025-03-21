@echo off

set DOTNET_INSTALLER=%USERPROFILE%\Desktop\workshop-cli\Resources\dotnet-sdk-6.0.425-win-x64.exe
set PYTHON_INSTALLER=%USERPROFILE%\Desktop\workshop-cli\Resources\python-installer.exe
set GIT_INSTALLER=%USERPROFILE%\Desktop\workshop-cli\Resources\Git-2.46.2-64-bit.exe
set TARGET_PROGRAM=%USERPROFILE%\Desktop\workshop-cli\CLI\CLI\WorkshopCli\bin\Debug\net6.0\WorkshopCli.exe
set SHORTCUT_NAME=%USERPROFILE%\Desktop\cli.lnk
set SESSION_FILE=%USERPROFILE%\Desktop\workshop-cli\Resources\session.txt

REM Check and install .NET 6
echo Checking .NET 6...
dotnet --version 2>nul | findstr "6.0" >nul
if %ERRORLEVEL% NEQ 0 (
    echo .NET 6 not found. Installing .NET 6...
    start /wait "" %DOTNET_INSTALLER% /quiet
) else (
    echo .NET 6 already installed.
)

REM Check and install Python
echo Checking Python...
python --version 2>nul | findstr "3." >nul
if %ERRORLEVEL% NEQ 0 (
    echo Python not found. Installing Python...
    start /wait "" %PYTHON_INSTALLER% /quiet
) else (
    echo Python already installed.
)

REM Check and install PyGithub
echo Checking PyGithub...
python -m pip show PyGithub >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo PyGithub not found. Installing PyGithub...
    python -m pip install PyGithub
) else (
    echo PyGithub already installed.
)

REM Check and install Git
echo Checking Git...
git --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Git not found. Installing Git...
    start /wait "" %GIT_INSTALLER% /VERYSILENT /NORESTART
) else (
    echo Git already installed.
)

REM Check Ollama
echo Checking Ollama...
ollama --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Ollama not found. Please install Ollama manually if needed.
) else (
    echo Ollama already installed.
)

REM Check and pull the Llama 3.2 1B model
echo Checking Llama 3.2 1B model...
ollama list | findstr "llama3.2:1b" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Llama 3.2 1B model not found. Pulling the model...
    start /wait "" ollama pull llama3.2:1b
) else (
    echo Llama 3.2 1B model already installed.
)

REM Start the Ollama server in the background if not running
echo Checking Ollama server...
tasklist | findstr "ollama" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Starting Ollama server in the background...
    start /B "" ollama serve >nul 2>&1
    timeout /t 5 /nobreak >nul
) else (
    echo Ollama server already running.
)

REM Check Visual Studio Code
echo Checking Visual Studio Code...
where code >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Visual Studio Code not found. Please install VS Code manually if needed.
) else (
    echo Visual Studio Code already installed.
)

REM Delete session file if it exists
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

REM REM Delete the script (uncomment if desired)
REM echo Deleting script...
REM del "%~f0"

echo Installations completed successfully, shortcut created, and script finished.
exit