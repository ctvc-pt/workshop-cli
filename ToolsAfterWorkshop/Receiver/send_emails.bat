@echo off
cd /d "%~dp0"

where python >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERRO: Python nao encontrado.
    pause
    exit /b 1
)

python -m pip show requests >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo A instalar dependencias...
    python -m pip install requests
)

python send_emails.py
pause
