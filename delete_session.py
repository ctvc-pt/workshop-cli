import os

# Caminho do ficheiro que será eliminado, baseado no diretório atual
current_dir = os.path.dirname(os.path.abspath(__file__))
file_path = os.path.join(current_dir, 'Resources', 'session.txt')

try:
    # Verifica se o ficheiro existe
    if os.path.exists(file_path):
        os.remove(file_path)
        print(f"Ficheiro {file_path} eliminado com sucesso.")
    else:
        print(f"O ficheiro {file_path} não existe.")
except Exception as e:
    print(f"Ocorreu um erro ao tentar eliminar o ficheiro: {e}")
