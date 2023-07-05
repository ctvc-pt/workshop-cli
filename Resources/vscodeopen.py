import subprocess

def open_vscode():
    try:
        # Replace the path below with the actual path to the 'code' executable of VS Code on your system
        subprocess.Popen(['C:\Users\Francisco Costa\AppData\Local\Programs\Microsoft VS Code', '.'])
        print("VS Code has been opened successfully.")
    except OSError as e:
        print("Error opening VS Code:", e)

open_vscode()