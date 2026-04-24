@echo off
cd /d "%~dp0"

where python >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo ERRO: Python nao encontrado.
    pause
    exit /b 1
)

python -m pip show google-api-python-client >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo A instalar dependencias...
    python -m pip install python-dotenv google-api-python-client google-auth
)

python send_emails.py
pause
