@echo off
setlocal

REM Receptor dos jogos dos miudos durante o workshop + dashboard de monitorizacao.
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

REM Instalar dependencias do receiver se ainda nao estiverem.
python -m pip show flask >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Flask nao encontrado. A instalar...
    python -m pip install -r requirements.txt
    if %ERRORLEVEL% NEQ 0 (
        echo ERRO: Falhou a instalacao das dependencias do receiver.
        pause
        exit /b 1
    )
)

REM === Dashboard ===
REM Arranca o dashboard React em paralelo, numa janela separada. Se o Node
REM nao estiver instalado, segue em frente sem dashboard ^(o receiver e o
REM essencial; o dashboard e opcional^).
where node >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo AVISO: Node nao encontrado. Dashboard nao vai arrancar.
    echo Para o dashboard, instala Node 18+ em https://nodejs.org
    echo.
) else (
    if not exist "..\..\Dashboard\node_modules" (
        echo Primeira vez: a instalar dependencias do dashboard...
        pushd "..\..\Dashboard"
        call npm install
        popd
    )
    echo A arrancar o dashboard numa janela separada...
    start "Workshop Dashboard" cmd /k "cd /d %~dp0..\..\Dashboard && npm run dev"

    REM Da tempo ao Vite para arrancar antes de abrir o browser.
    timeout /t 4 /nobreak >nul
    start http://localhost:5173
)

REM Mostrar ao monitor o IP local, para ele poder mete-lo no setup dos PCs dos miudos.
echo.
echo IPs locais deste portatil ^(usa um destes no setup dos PCs^):
for /f "tokens=2 delims=:" %%a in ('ipconfig ^| findstr /C:"IPv4"') do echo   %%a
echo.

python workshop_receiver.py

pause
