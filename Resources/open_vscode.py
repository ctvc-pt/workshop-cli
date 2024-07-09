import os
import subprocess
import json
from datetime import datetime

def open_vscode(folder_path):
    try:
        # Caminho para o executável do VS Code pode variar, então verifique o caminho.
        code_path = os.path.join(os.path.expanduser("~"), "AppData/Local/Programs/Microsoft VS Code/Code.exe")
        if not os.path.exists(code_path):
            print("VS Code não encontrado em", code_path)
            return
        subprocess.Popen([code_path, '--add', '--disable-workspace-trust', folder_path])
    except OSError as e:
        print("Erro ao abrir o VS Code:", e)

current_directory = os.path.dirname(os.path.abspath(__file__))
name_file_path = os.path.join(current_directory, "session.txt")

# Verifica se o arquivo de nome existe antes de tentar abrir.
if not os.path.exists(name_file_path):
    print("Arquivo de sessão não encontrado:", name_file_path)
    exit(1)

with open(name_file_path, "r") as name_file:
    name_data = json.load(name_file)
    name = name_data.get("Name", "default-user").replace(' ', '-')

# Assegura que o nome do diretório é sempre válido.
if not name:
    name = "default-user"

desktop_folder = os.path.expanduser("~/Desktop/repoWorkshop")         
folder_name = f"{name}_{datetime.now().strftime('%d-%m-%Y')}"
full_path = os.path.join(desktop_folder, folder_name, "mygame")

# Verifica se o diretório existe antes de tentar abri-lo.
if not os.path.exists(full_path):
    print("Diretório não encontrado:", full_path)
    exit(1)

open_vscode(full_path)
