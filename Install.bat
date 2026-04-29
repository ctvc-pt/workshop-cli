@echo off

REM Require admin — otherwise the silent installers fail in silence and the user has no idea why.
net session >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo *** This script must be run as Administrator. ***
    echo Right-click Install.bat and choose "Run as administrator".
    echo.
    pause
    exit /b 1
)

set DOTNET_INSTALLER=%~dp0Resources\dotnet-sdk-8.0.420-win-x64.exe
set PYTHON_INSTALLER=%~dp0Resources\python-installer.exe
set GIT_INSTALLER=%~dp0Resources\Git-2.46.2-64-bit.exe
set VSCODE_INSTALLER=%~dp0Resources\VSCodeSetup.exe
set OLLAMA_INSTALLER=%~dp0Resources\OllamaSetup.exe
set TARGET_PROGRAM=%~dp0CLI\CLI\WorkshopCli\bin\Debug\net8.0\WorkshopCli.exe
set "DESKTOP_DIR="
for /f "usebackq delims=" %%i in (`powershell -NoProfile -ExecutionPolicy Bypass -Command "[Environment]::GetFolderPath('Desktop')"`) do set "DESKTOP_DIR=%%i"
if not defined DESKTOP_DIR set "DESKTOP_DIR=%USERPROFILE%\Desktop"
set "SHORTCUT_NAME=%DESKTOP_DIR%\cli.lnk"
set SESSION_FILE=%~dp0Resources\session.txt
set RECEIVER_FILE=%~dp0Resources\receiver.json

REM Check and install .NET 8 SDK (a .NET 6 SDK alone is not enough)
echo Checking .NET 8 SDK...
where dotnet >nul 2>&1
if %ERRORLEVEL% NEQ 0 goto install_dotnet8
dotnet --list-sdks > "%TEMP%\dotnet_sdks.txt" 2>&1
findstr /B /C:"8." "%TEMP%\dotnet_sdks.txt" >nul 2>&1
set "HAS_DOTNET8=%ERRORLEVEL%"
del "%TEMP%\dotnet_sdks.txt" 2>nul
if "%HAS_DOTNET8%"=="0" (
    echo .NET 8 SDK already installed.
    goto dotnet8_done
)
:install_dotnet8
echo .NET 8 SDK not found. Installing .NET 8 (this may take 1-2 min)...
"%DOTNET_INSTALLER%" /install /quiet /norestart
set "DOTNET_INSTALL_EXITCODE=%ERRORLEVEL%"
set "PATH=%PATH%;C:\Program Files\dotnet"
dotnet --list-sdks > "%TEMP%\dotnet_sdks.txt" 2>&1
findstr /B /C:"8." "%TEMP%\dotnet_sdks.txt" >nul 2>&1
set "DOTNET8_VERIFY=%ERRORLEVEL%"
del "%TEMP%\dotnet_sdks.txt" 2>nul
if not "%DOTNET8_VERIFY%"=="0" (
    echo.
    echo *** ERROR: .NET 8 SDK install did not succeed ^(installer exit code %DOTNET_INSTALL_EXITCODE%^).
    echo *** Try running "%DOTNET_INSTALLER%" manually to see the error.
    echo.
    pause
    exit /b 1
)
echo .NET 8 SDK installed successfully.
:dotnet8_done

REM Check and install Python
echo Checking Python...
where python >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Python not found. Installing Python...
    start /wait "" "%PYTHON_INSTALLER%" /quiet
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
    start /wait "" "%GIT_INSTALLER%" /VERYSILENT /NORESTART
) else (
    echo Git already installed.
)

REM Check and install Ollama (provides the local LLM for the 'ajuda' helper)
echo.
echo === Checking Ollama ===
where ollama >nul 2>&1
if %ERRORLEVEL% EQU 0 goto ollama_present

REM Primary path: winget. It's Microsoft-signed and is often whitelisted on managed
REM machines where direct installers get blocked by Device Guard / WDAC policies.
where winget >nul 2>&1
if %ERRORLEVEL% NEQ 0 goto ollama_try_local

echo Ollama not found. Installing via winget.
echo This usually takes 1-3 minutes. Please do not close this window...
winget install --id Ollama.Ollama --exact --silent --accept-source-agreements --accept-package-agreements
REM winget installs Ollama per-user into %LocalAppData%\Programs\Ollama but does not
REM refresh PATH in this shell; append the expected install dir before re-checking.
set "PATH=%PATH%;%LocalAppData%\Programs\Ollama"
where ollama >nul 2>&1
if %ERRORLEVEL% EQU 0 goto ollama_installed
echo winget install did not produce a working 'ollama' command. Falling back to local installer.

:ollama_try_local
if not exist "%OLLAMA_INSTALLER%" (
    echo Local installer missing at %OLLAMA_INSTALLER%.
    echo Skipping Ollama install, model pull and server start.
    goto ollama_done
)

echo Trying silent install from local installer...
REM OllamaSetup.exe is NSIS-based: /S (uppercase) is the silent-install flag.
start /wait "" "%OLLAMA_INSTALLER%" /S
set "PATH=%PATH%;%LocalAppData%\Programs\Ollama"
where ollama >nul 2>&1
if %ERRORLEVEL% EQU 0 goto ollama_installed

REM Silent install failed. Fall back to an interactive install so the student can click
REM through the wizard without leaving this window.
echo.
echo Silent install did not produce a working 'ollama' command.
echo Opening the Ollama installer wizard. Please click through Next/Install.
echo This window will wait until you finish.
echo.
start /wait "" "%OLLAMA_INSTALLER%"
set "PATH=%PATH%;%LocalAppData%\Programs\Ollama"
where ollama >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo *** WARNING: Ollama is still not detected after install.
    echo *** The CLI's 'ajuda' helper will not work until Ollama is available on PATH.
    echo *** If Device Guard / WDAC is blocking the installer, ask IT to whitelist it,
    echo *** or install Ollama manually on another machine.
    echo.
    goto ollama_done
)

:ollama_installed
echo Ollama installed successfully.
goto ollama_serve

:ollama_present
echo Ollama already installed.

REM Start the server BEFORE checking/pulling the model, because `ollama list` and
REM `ollama pull` both require the daemon to be listening on localhost:11434.
:ollama_serve
echo Checking Ollama server...
tasklist /FI "IMAGENAME eq ollama.exe" 2>nul | find /I "ollama.exe" >nul
if %ERRORLEVEL% EQU 0 (
    echo Ollama server already running.
    goto ollama_model
)
echo Starting Ollama server in the background...
start /B "" ollama serve >nul 2>&1 < nul
REM Give the server a few seconds to bind to localhost:11434 before we call ollama list/pull
timeout /t 5 /nobreak >nul

:ollama_model
echo Checking Qwen 2.5 3B model...
REM Redirect stdin from nul so any accidental prompt inside ollama cannot block.
ollama list < nul > "%TEMP%\ollama_list.txt" 2>&1
findstr /C:"qwen2.5:3b" "%TEMP%\ollama_list.txt" >nul 2>&1
set "HAS_MODEL=%ERRORLEVEL%"
del "%TEMP%\ollama_list.txt" 2>nul
if "%HAS_MODEL%"=="0" (
    echo Qwen 2.5 3B model already installed.
    goto ollama_done
)
echo Qwen 2.5 3B model not found. Pulling the model (~2 GB).
echo Depending on your connection this can take 2-8 minutes. Please wait...
REM Run inline (no start /wait) so progress streams to this window and no second
REM console can linger waiting for a keypress. < nul neutralises any stdin prompt.
ollama pull qwen2.5:3b < nul

:ollama_done

REM Check and install Visual Studio Code
echo Checking Visual Studio Code...
where code >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    if exist "%VSCODE_INSTALLER%" (
        echo Visual Studio Code not found. Installing...
        start /wait "" "%VSCODE_INSTALLER%" /VERYSILENT /NORESTART /MERGETASKS=!runcode,addcontextmenufiles,addcontextmenufolders,addtopath
    ) else (
        echo Visual Studio Code not found and installer missing at %VSCODE_INSTALLER%. Please install VS Code manually.
    )
) else (
    echo Visual Studio Code already installed.
)

REM Install VS Code extensions (re-check `code` command in case VS Code was just installed).
REM Prefer the canonical code.cmd install paths over `where code`: a stray/broken
REM code.cmd wrapper on PATH (seen in the wild pointing to C:\Code.exe) would otherwise
REM shadow the real one and make every --install-extension call fail.
set "CODE_CMD="
if exist "%LOCALAPPDATA%\Programs\Microsoft VS Code\bin\code.cmd" set "CODE_CMD=%LOCALAPPDATA%\Programs\Microsoft VS Code\bin\code.cmd"
if not defined CODE_CMD if exist "%ProgramFiles%\Microsoft VS Code\bin\code.cmd" set "CODE_CMD=%ProgramFiles%\Microsoft VS Code\bin\code.cmd"
if not defined CODE_CMD if exist "%ProgramFiles(x86)%\Microsoft VS Code\bin\code.cmd" set "CODE_CMD=%ProgramFiles(x86)%\Microsoft VS Code\bin\code.cmd"
if defined CODE_CMD goto code_cmd_found
where code >nul 2>&1
if %ERRORLEVEL% EQU 0 set "CODE_CMD=code"
:code_cmd_found

if not defined CODE_CMD goto skip_extensions

REM Sanity-check: call --version so a broken wrapper gets rejected rather than swallowed.
call "%CODE_CMD%" --version >nul 2>&1
if %ERRORLEVEL% EQU 0 goto code_cmd_ok
echo WARNING: "%CODE_CMD%" failed to run. Skipping extension install.
goto skip_extensions
:code_cmd_ok

REM sumneko.lua - Lua language support
echo Checking VS Code extension sumneko.lua...
call "%CODE_CMD%" --list-extensions | findstr /I "sumneko.lua" >nul
if %ERRORLEVEL% NEQ 0 (
    echo Extension not found. Installing sumneko.lua...
    call "%CODE_CMD%" --install-extension sumneko.lua
) else (
    echo Extension sumneko.lua already installed.
)

REM pixelbyte-studios.pixelbyte-love2d - Love2D run command (Alt+L)
echo Checking VS Code extension pixelbyte-studios.pixelbyte-love2d...
call "%CODE_CMD%" --list-extensions | findstr /I "pixelbyte-studios.pixelbyte-love2d" >nul
if %ERRORLEVEL% NEQ 0 (
    echo Extension not found. Installing pixelbyte-studios.pixelbyte-love2d...
    call "%CODE_CMD%" --install-extension pixelbyte-studios.pixelbyte-love2d
) else (
    echo Extension pixelbyte-studios.pixelbyte-love2d already installed.
)
goto extensions_done

:skip_extensions
echo WARNING: VS Code "code" command not found in PATH or expected install locations.
echo Skipping extension installation. Install manually after reopening a shell:
echo   code --install-extension sumneko.lua
echo   code --install-extension pixelbyte-studios.pixelbyte-love2d

:extensions_done

REM Build the CLI if the executable is missing (first run after a fresh install)
if not exist "%TARGET_PROGRAM%" (
    echo Building WorkshopCli...
    REM Refresh PATH so a freshly installed dotnet is picked up without reopening the shell
    set "PATH=%PATH%;C:\Program Files\dotnet"
    pushd "%~dp0CLI\CLI\WorkshopCli"
    dotnet build
    popd
)

REM Delete session file if it exists
if exist "%SESSION_FILE%" (
    echo Deleting session file...
    del "%SESSION_FILE%"
)

REM Configure receiver URL (monitor's laptop IP) for end-of-guide game upload.
REM This is asked ONCE per PC during setup. Kids never see it at runtime.
REM
REM We use PowerShell's Read-Host instead of "set /p" because "set /p" silently
REM skips the prompt on some Windows setups — typically after earlier commands
REM in this script redirect stdin (e.g. "ollama pull < nul"). Read-Host reopens
REM the console handle and is immune to that.
echo.
echo === Configuracao do receptor dos jogos ===
if not exist "%RECEIVER_FILE%" goto receiver_ask
echo Receptor ja configurado em %RECEIVER_FILE%:
type "%RECEIVER_FILE%"
echo.
choice /c SN /n /m "Reconfigurar? [S/N]: "
if errorlevel 2 goto receiver_done

:receiver_ask
echo Qual o IP (ou hostname) do portatil do monitor que vai receber os jogos?
echo Exemplos: 192.168.1.42  ou  workshop.local
echo Deixa em branco e carrega Enter para saltar (o envio automatico fica desativado).
set "RECEIVER_HOST="
for /f "usebackq delims=" %%i in (`powershell -NoProfile -Command "Read-Host 'IP/hostname'"`) do set "RECEIVER_HOST=%%i"
if "%RECEIVER_HOST%"=="" (
    echo Receptor nao configurado. O envio automatico do jogo fica desativado neste PC.
    goto receiver_done
)
set "RECEIVER_PORT="
for /f "usebackq delims=" %%i in (`powershell -NoProfile -Command "$p = Read-Host 'Porta [5000]'; if ($p) { $p } else { '5000' }"`) do set "RECEIVER_PORT=%%i"
if "%RECEIVER_PORT%"=="" set "RECEIVER_PORT=5000"
set "RECEIVER_URL=http://%RECEIVER_HOST%:%RECEIVER_PORT%/upload"
echo { "ReceiverUrl": "%RECEIVER_URL%" } > "%RECEIVER_FILE%"
echo Receptor gravado: %RECEIVER_URL%

REM Ping para validar (se o monitor ja tiver o receptor a correr).
powershell -NoProfile -Command "try { $r = Invoke-WebRequest -Uri 'http://%RECEIVER_HOST%:%RECEIVER_PORT%/ping' -UseBasicParsing -TimeoutSec 3; if ($r.StatusCode -eq 200) { Write-Host 'Receptor respondeu OK.' } else { Write-Host ('Receptor devolveu status ' + $r.StatusCode) } } catch { Write-Host 'Aviso: receptor nao respondeu (pode nao estar ligado agora, esta bem).' }"
:receiver_done

REM Create the shortcut. Do NOT also Start-Process the exe directly here:
REM doing both spawns two CLI instances, each opens its own VS Code window.
echo Creating shortcut...
powershell -Command "$shell = New-Object -ComObject WScript.Shell; $shortcut = $shell.CreateShortcut('%SHORTCUT_NAME%'); $shortcut.TargetPath = '%TARGET_PROGRAM%'; $shortcut.WorkingDirectory = (Split-Path -Path '%TARGET_PROGRAM%'); $shortcut.WindowStyle = 3; $shortcut.Save()"

REM Open the shortcut (single launch — same path students will use afterwards)
echo Opening shortcut...
start "" "%SHORTCUT_NAME%"

REM REM Delete the script (uncomment if desired)
REM echo Deleting script...
REM del "%~f0"

echo Installations completed successfully, shortcut created, and script finished.
pause