@echo off
pip install -r "%~dp0requirements.txt"
python "%~dp0send_email.py"
pause