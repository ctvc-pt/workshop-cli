import json
import os
import shutil
from dotenv import load_dotenv
import smtplib
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from email.mime.base import MIMEBase
from email import encoders

# Carregar o arquivo .env
load_dotenv()

# Obter a string JSON da variável de ambiente
config_json = os.getenv('CONFIG_JSON')

# Verificar se a variável foi carregada corretamente
if config_json is None:
    print("Erro: A variável de ambiente CONFIG_JSON não foi encontrada no .env")
    exit(1)

# Converter a string JSON em um dicionário Python
config = json.loads(config_json)

# Configurações de e-mail extraídas do arquivo JSON
SMTP_SERVER = config["SMTP_SERVER"]
SMTP_PORT = config["SMTP_PORT"]
EMAIL_SENDER = config["EMAIL_SENDER"]
EMAIL_PASSWORD = config["EMAIL_PASSWORD"]

# Função para ler o user_data.txt e extrair nome e e-mail
def get_user_data(user_data_file):
    user_info = {"Nome": None, "Email": None}
    try:
        with open(user_data_file, "r", encoding="utf-8") as f:
            lines = f.readlines()
            for line in lines:
                if line.startswith("Nome:"):
                    user_info["Nome"] = line.split("Nome:")[1].strip()
                elif line.startswith("Email:"):
                    user_info["Email"] = line.split("Email:")[1].strip()
        return user_info
    except Exception as e:
        print(f"Erro ao ler o arquivo {user_data_file}: {e}")
        return None

# Função para zipar a pasta mygame
def zip_mygame_folder(folder_path, output_zip):
    try:
        shutil.make_archive(output_zip, 'zip', folder_path)
        return output_zip + ".zip"
    except Exception as e:
        print(f"Erro ao zipar a pasta: {e}")
        return None

# Função para ler o conteúdo do emailText.txt e substituir {NOME}
def get_email_body(template_path, name):
    try:
        with open(template_path, "r", encoding="utf-8") as f:
            content = f.read()
        return content.replace("{NOME}", name)
    except Exception as e:
        print(f"Erro ao ler o arquivo {template_path}: {e}")
        return None

# Função para enviar e-mail com anexo
def send_email(to_email, subject, body, attachment_path):
    try:
        msg = MIMEMultipart()
        msg["From"] = EMAIL_SENDER
        msg["To"] = to_email
        msg["Subject"] = subject

        msg.attach(MIMEText(body, "plain"))

        # Anexar o arquivo zipado
        with open(attachment_path, "rb") as f:
            part = MIMEBase("application", "octet-stream")
            part.set_payload(f.read())
            encoders.encode_base64(part)
            part.add_header("Content-Disposition", f"attachment; filename={os.path.basename(attachment_path)}")
            msg.attach(part)

        # Conectar ao servidor SMTP e enviar
        with smtplib.SMTP_SSL(SMTP_SERVER, SMTP_PORT) as server:
            server.login(EMAIL_SENDER, EMAIL_PASSWORD)
            server.send_message(msg)
        return True
    except Exception as e:
        print(f"Erro ao enviar e-mail: {e}")
        return False

# Função principal
def main():
    script_dir = os.path.dirname(os.path.abspath(__file__))
    desktop_path = os.path.join(os.path.expanduser("~"), "Desktop")

    # Encontrar todas as pastas que começam com "workshopBackup"
    repo_paths = [os.path.join(desktop_path, folder) for folder in os.listdir(desktop_path) if folder.startswith("workshopBackup") and os.path.isdir(os.path.join(desktop_path, folder))]
    
    email_template_path = os.path.join(script_dir, "emailText.txt")

    if not repo_paths:
        print("Nenhuma pasta 'workshopBackup' encontrada no Desktop!")
        return

    if not os.path.exists(email_template_path):
        print(f"Arquivo de template de e-mail não encontrado: {email_template_path}")
        return

    # Iterar pelas pastas workshopBackup-*
    for repo_path in repo_paths:
        print(f"Processando: {repo_path}")

        # Iterar pelas subpastas dentro de cada workshopBackup-*
        for folder_name in os.listdir(repo_path):
            user_folder_path = os.path.join(repo_path, folder_name)
            if os.path.isdir(user_folder_path):
                # Caminho para o user_data.txt
                user_data_file = os.path.join(user_folder_path, "user_data.txt")
                if not os.path.exists(user_data_file):
                    print(f"Arquivo 'user_data.txt' não encontrado em {folder_name}")
                    continue

                # Extrair nome e e-mail do user_data.txt
                user_info = get_user_data(user_data_file)
                if not user_info or not user_info["Email"]:
                    print(f"Nome ou e-mail não encontrado em {user_data_file}")
                    continue

                name = user_info["Nome"]
                email = user_info["Email"]

                # Caminho para a pasta mygame
                mygame_path = os.path.join(user_folder_path, "mygame")
                if not os.path.exists(mygame_path):
                    print(f"Pasta 'mygame' não encontrada em {folder_name}")
                    continue

                # Zipar a pasta mygame
                zip_path = os.path.join(user_folder_path, f"mygame_{name.replace(' ', '-')}")
                zip_file = zip_mygame_folder(mygame_path, zip_path)
                if not zip_file:
                    continue

                # Obter o corpo do e-mail do arquivo de texto
                email_body = get_email_body(email_template_path, name)
                if not email_body:
                    print("Falha ao carregar o corpo do e-mail")
                    continue

                # Enviar e-mail
                subject = f"Oficinas de Programação - 2025"
                if send_email(email, subject, email_body, zip_file):
                    print(f"E-mail enviado para {email} com sucesso!")
                else:
                    print(f"Falha ao enviar e-mail para {email}")

                # Remover o arquivo zip após o envio
                try:
                    os.remove(zip_file)
                except Exception as e:
                    print(f"Erro ao remover o arquivo zip: {e}")

if __name__ == "__main__":
    main()
