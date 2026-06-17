@echo off
set DOTNET_ROLL_FORWARD=Major
cd /d "%~dp0CLI\CLI\WorkshopCli\bin\Debug\net6.0"
"WorkshopCli.exe"
