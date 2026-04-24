@echo off
setlocal

REM Receptor dos jogos dos miudos durante o workshop.
REM Corre no portatil do monitor, na mesma rede WiFi que os PCs dos miudos.

cd /d "%~dp0"

echo === Receptor do Workshop ===
echo.

where python >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERRO: Python nao encontrado. Instala Python primeiro.
    pause
    exit /b 1
)

REM Instalar Flask se ainda nao estiver.
python -m pip show flask >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Flask nao encontrado. A instalar...
    python -m pip install -r requirements.txt
    if %ERRORLEVEL% NEQ 0 (
        echo ERRO: Falhou a instalacao do Flask.
        pause
        exit /b 1
    )
)

REM Mostrar ao monitor o IP local, para ele poder metelo no setup dos PCs dos miudos.
echo IPs locais deste portatil ^(usa um destes no setup dos PCs^):
for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr /C:"IPv4"') do echo   %%a
echo.

python workshop_receiver.py

pause
