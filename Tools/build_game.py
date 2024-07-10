import os
import shutil
import zipfile

def create_love_file(folder_path):
    love_file_path = f"{folder_path}.love"
    with zipfile.ZipFile(love_file_path, 'w', zipfile.ZIP_DEFLATED) as love_zip:
        for root, dirs, files in os.walk(folder_path):
            for file in files:
                file_path = os.path.join(root, file)
                arcname = os.path.relpath(file_path, folder_path)
                love_zip.write(file_path, arcname)
    return love_file_path

def create_executable(love_file_path, love_exe_path, dll_paths):
    executable_name = os.path.splitext(love_file_path)[0] + '.exe'
    output_dir = os.path.dirname(executable_name)

    # Cria o executável combinando love.exe e o arquivo .love
    with open(love_exe_path, 'rb') as f:
        love_exe_data = f.read()
    with open(executable_name, 'wb') as f:
        f.write(love_exe_data)
        with open(love_file_path, 'rb') as love_file:
            shutil.copyfileobj(love_file, f)

    # Copia os arquivos DLL para o diretório de saída
    for dll_path in dll_paths:
        shutil.copy(dll_path, output_dir)

    return executable_name, output_dir

def zip_directory(folder_path, zip_name):
    with zipfile.ZipFile(zip_name, 'w', zipfile.ZIP_DEFLATED) as zipf:
        for root, dirs, files in os.walk(folder_path):
            for file in files:
                file_path = os.path.join(root, file)
                arcname = os.path.relpath(file_path, folder_path)
                zipf.write(file_path, arcname)

def main():
    workshops_dir = "Workshops"
    love_exe_path = "C:\\Program Files\\LOVE\\love.exe"  # Mude para o caminho correto do executável Love2D
    dll_paths = [
        "C:\\Program Files\\LOVE\\SDL2.dll",
        "C:\\Program Files\\LOVE\\love.dll",
        "C:\\Program Files\\LOVE\\lua51.dll",
        "C:\\Program Files\\LOVE\\mpg123.dll",
        "C:\\Program Files\\LOVE\\msvcp120.dll",
        "C:\\Program Files\\LOVE\\msvcr120.dll",
        "C:\\Program Files\\LOVE\\OpenAL32.dll",
    ]

    for root, dirs, files in os.walk(workshops_dir):
        for dir in dirs:
            sub_dir_path = os.path.join(root, dir)
            for sub_root, sub_dirs, sub_files in os.walk(sub_dir_path):
                for sub_dir in sub_dirs:
                    if sub_dir == "mygame":
                        mygame_path = os.path.join(sub_root, sub_dir)
                        love_file_path = create_love_file(mygame_path)
                        executable_path, output_dir = create_executable(love_file_path, love_exe_path, dll_paths)
                        print(f"Executable created: {executable_path}")
                        
                        zip_name = f"{output_dir}.zip"
                        zip_directory(output_dir, zip_name)
                        print(f"Zipped {output_dir} into {zip_name}")
                break  # Evita descer mais um nível no os.walk

if __name__ == "__main__":
    main()
