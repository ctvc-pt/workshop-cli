# How to Run Workshop CLI

1. Clone the repository.
2. Download the API keys and add them to the `Resources` folder from the following [Google Drive link](https://drive.google.com/drive/folders/1BnHV73TDjjBz409cYiKzdfZMoD3cgLwp?usp=sharing).
3. Install .NET 8.0 by running `Resources/dotnet-runtime-8.0.4-win-x64.exe`.
4. Run `Install.bat` with administrator permissions in the project folder.

### Troubleshooting

1. If the installation doesn’t work, manually install each installer in the `Resources` folder.
2. To restart the CLI, delete the `session.txt` file from the `Resources` folder, or run the `delete_session.bat` script.
3. If the `Resources` folder is incomplete, check the files on [Google Drive](https://drive.google.com/drive/folders/1gVgpg3qHIyZw43Nk-zmTdFIFGjQzUcOD?usp=sharing).

# How to Create Game Builds

1. Create a folder and place all the game folders inside it.
2. Run the script `tools/build_game.py` and pass the folder with the games as an argument.
3. The script will create a zip file containing the game code and an `.exe` file inside the specified folder.

# How to Make an Installer

1. Install [Inno Setup](https://jrsoftware.org/isdl.php).
2. Open the script located in `Installer/workshopCLI.iss`.
3. Compile the script.
   ![alt text](image.png)
4. After compilation, the installation files will be in the `Installer/Output` folder.
5. Inside the `Output` folder, find the `mysetup.exe` file, and rename it to `WorkshopCliSetup.exe`.
