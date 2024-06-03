import os
import subprocess
import json
from datetime import datetime

def open_vscode(folder_path):
    try:
        file_name = os.path.join(os.path.expanduser("~"), "AppData/Local/Programs/Microsoft VS Code/Code.exe")
        subprocess.Popen([file_name, '--add','--disable-workspace-trust', folder_path])
    except OSError as e:
        print(e)

current_directory = os.path.dirname(os.path.abspath(__file__))
name_file_path = os.path.join(current_directory, "session.txt")

# Read the name from the name file
with open(name_file_path, "r") as name_file:
    name_data = json.load(name_file)
    name = name_data.get("Name", "")

desktop_folder = os.path.expanduser("~/Desktop/repoWorkshop")         
folder_name = f"{name.replace(' ', '-')}_{datetime.now().strftime('%d-%m-%Y')}"
open_vscode(os.path.join(desktop_folder, folder_name,"mygame"))
