import os
import shutil
import re

# Caminho para o Desktop
DESKTOP_PATH = os.path.join(os.path.expanduser("~"), "Desktop")

# Caminho para o diretório do Love2D (onde estão love.exe e as DLLs)
LOVE_DIR = r"C:\Program Files\LOVE"  # Ajuste para o diretório correto
LOVE_EXECUTABLE = os.path.join(LOVE_DIR, "love.exe")  # Caminho completo para love.exe

# Lista de arquivos adicionais do Love2D necessários para o executável funcionar
LOVE_DLLS = [
    "love.dll",  # Essencial para o funcionamento do executável
    "SDL2.dll",
    "lua51.dll",
    "OpenAL32.dll",
    "mpg123.dll",
    "msvcp120.dll",
    "msvcr120.dll",
    "license.txt"  # Opcional
]

def create_love_executable(mygame_path, output_dir):
    """
    Converte a pasta mygame (contendo main.lua, imagens e sons) em um executável Love2D
    e salva dentro de uma pasta 'build' ao lado da pasta mygame.
    """
    try:
        # Criar a pasta 'build' ao lado da pasta mygame, se não existir
        build_dir = os.path.join(output_dir, "build")
        os.makedirs(build_dir, exist_ok=True)

        # Criar um arquivo .zip temporário a partir da pasta mygame, incluindo main.lua, imagens e sons
        temp_zip_file = os.path.join(build_dir, "game_temp")
        shutil.make_archive(temp_zip_file, 'zip', mygame_path)  # Zipa toda a pasta mygame
        print(f"Arquivos de {mygame_path} (main.lua, imagens, sons) zipados em {temp_zip_file}.zip")

        # Renomear o arquivo .zip para .love
        temp_love_file = os.path.join(build_dir, "game.love")
        os.rename(temp_zip_file + ".zip", temp_love_file)
        print(f"Arquivo .zip renomeado para .love: {temp_love_file}")

        # Nome do executável final
        exe_name = "game.exe"
        output_exe_path = os.path.join(build_dir, exe_name)

        # Combinar love.exe com o arquivo .love para criar o executável
        with open(output_exe_path, 'wb') as exe_file:
            with open(LOVE_EXECUTABLE, 'rb') as love_exe:
                exe_file.write(love_exe.read())  # Copia o love.exe
            with open(temp_love_file, 'rb') as love_file:
                exe_file.write(love_file.read())  # Adiciona o .love com main.lua, imagens e sons

        # Remover o arquivo .love temporário
        os.remove(temp_love_file)

        # Copiar as DLLs necessárias para a pasta build
        for dll in LOVE_DLLS:
            dll_src = os.path.join(LOVE_DIR, dll)
            dll_dst = os.path.join(build_dir, dll)
            if os.path.exists(dll_src):
                shutil.copy2(dll_src, dll_dst)
                print(f"    Copiado {dll} para {build_dir}")
            else:
                print(f"    Aviso: {dll} não encontrado em {LOVE_DIR}")

        print(f"Executável criado com sucesso em: {output_exe_path}")
        return True

    except Exception as e:
        print(f"Erro ao criar o executável em {build_dir}: {e}")
        return False

def process_workshop_folders():
    """
    Processa todas as pastas workshopBackup-* no Desktop.
    """
    # Encontrar todas as pastas workshopBackup-*
    workshop_paths = [
        os.path.join(DESKTOP_PATH, folder)
        for folder in os.listdir(DESKTOP_PATH)
        if folder.startswith("workshopBackup-") and os.path.isdir(os.path.join(DESKTOP_PATH, folder))
    ]

    if not workshop_paths:
        print("Nenhuma pasta 'workshopBackup-*' encontrada no Desktop!")
        return

    # Verificar se o executável do Love2D existe
    if not os.path.exists(LOVE_EXECUTABLE):
        print(f"Executável do Love2D não encontrado em: {LOVE_EXECUTABLE}. Verifique o caminho!")
        return

    # Padrão para verificar datas no formato _DD-MM-YYYY
    date_pattern = re.compile(r"_\d{2}-\d{2}-\d{4}$")

    # Processar cada pasta workshopBackup-*
    for workshop_path in workshop_paths:
        print(f"Processando: {workshop_path}")

        # Iterar pelas subpastas com nome_DD-MM-YYYY
        for folder_name in os.listdir(workshop_path):
            user_folder_path = os.path.join(workshop_path, folder_name)
            if os.path.isdir(user_folder_path) and date_pattern.search(folder_name):
                print(f"  Analisando pasta: {folder_name}")

                # Caminho para a pasta mygame
                mygame_path = os.path.join(user_folder_path, "mygame")
                if not os.path.exists(mygame_path):
                    print(f"    Pasta 'mygame' não encontrada em {folder_name}")
                    continue

                # Diretório de saída é o mesmo que contém mygame (onde a pasta build será criada)
                output_dir = user_folder_path

                # Criar o executável dentro da pasta build ao lado da pasta mygame
                if create_love_executable(mygame_path, output_dir):
                    print(f"    Build concluído para {folder_name}")
                else:
                    print(f"    Falha ao criar build para {folder_name}")

def main():
    process_workshop_folders()

if __name__ == "__main__":
    main()