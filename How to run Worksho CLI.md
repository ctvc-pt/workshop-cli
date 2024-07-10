# How to run Workshop CLI

1 - Clone the repo

2 - Download the Resources folder from the CPDS Drive and put it inside project folder

3 - Install .NET 8.0 - Resources/dotnet-runtime-8.0.4-win-x64.exe

4 - Run Install.bat with admin permissions in the project folder

PS: if it doesn't work, try to install each installer in the Resources folder
PS2: if you want to restart the CLI, don't forget the delete session.txt from Resources folder or run the script delete_session.bat

# How to create game builds

1 - Create a folder called "Workshops" with all game folders

2 - Run the script "tools/build_game.py"

3 - It creates a zip file with the game code and a exe file 
