"""
Depois do workshop: corre no portatil do monitor (em casa, com net decente)
para enviar os emails aos miudos com o jogo deles.

Le as pastas em C:\\WorkshopReceiver\\* (cada uma com game.zip + user_data.txt
criados pelo workshop_receiver.py durante o workshop), faz upload do zip para
a Google Drive e envia email com o link.

Reutiliza 95% da logica do ToolsAfterWorkshop/send_email.py original; a unica
diferenca real e que nao precisa de zipar nada porque o zip ja vem pronto.
"""
import json
import os
import re
import smtplib
from dotenv import load_dotenv
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from googleapiclient.discovery import build
from google.oauth2 import service_account

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))
BASE = r"C:\WorkshopReceiver"
EMAIL_TEMPLATE = os.path.join(SCRIPT_DIR, "..", "emailText.txt")
SERVICE_ACCOUNT_FILE = os.path.join(SCRIPT_DIR, "..", "service_account.json")
SCOPES = ["https://www.googleapis.com/auth/drive"]
PARENT_FOLDER_ID = "1mWhVNfZhQrnFaheAouTY83_ltbD2sIAg"

load_dotenv(os.path.join(SCRIPT_DIR, "..", ".env"))
config = json.loads(os.getenv("CONFIG_JSON", "{}"))
SMTP_SERVER = config.get("SMTP_SERVER")
SMTP_PORT = config.get("SMTP_PORT")
EMAIL_SENDER = config.get("EMAIL_SENDER")
EMAIL_PASSWORD = config.get("EMAIL_PASSWORD")


def is_valid_email(email):
    return bool(re.match(r"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", email))


def read_user_data(path):
    info = {"Nome": None, "Email": None}
    with open(path, "r", encoding="utf-8") as f:
        for line in f:
            if line.startswith("Nome:"):
                info["Nome"] = line.split("Nome:", 1)[1].strip()
            elif line.startswith("Email:"):
                info["Email"] = line.split("Email:", 1)[1].strip()
    if not info["Email"] or not is_valid_email(info["Email"]):
        return None
    return info


def upload_to_drive(file_path):
    creds = service_account.Credentials.from_service_account_file(SERVICE_ACCOUNT_FILE, scopes=SCOPES)
    service = build("drive", "v3", credentials=creds)
    metadata = {"name": os.path.basename(file_path), "parents": [PARENT_FOLDER_ID]}
    file = service.files().create(body=metadata, media_body=file_path, fields="id").execute()
    fid = file.get("id")
    return f"https://drive.google.com/uc?id={fid}&export=download" if fid else None


def get_email_body(name, link):
    with open(EMAIL_TEMPLATE, "r", encoding="utf-8") as f:
        return f.read().replace("{NOME}", name).replace("{LINK}", link)


def send_email(to_email, subject, body):
    msg = MIMEMultipart()
    msg["From"] = EMAIL_SENDER
    msg["To"] = to_email
    msg["Subject"] = subject
    msg.attach(MIMEText(body, "plain"))
    with smtplib.SMTP_SSL(SMTP_SERVER, SMTP_PORT) as server:
        server.login(EMAIL_SENDER, EMAIL_PASSWORD)
        server.send_message(msg)


def process_folder(folder):
    user_data_file = os.path.join(folder, "user_data.txt")
    zip_file = os.path.join(folder, "game.zip")

    if not os.path.exists(user_data_file) or not os.path.exists(zip_file):
        print(f"[skip] {folder}: faltam user_data.txt ou game.zip")
        return False

    info = read_user_data(user_data_file)
    if info is None:
        print(f"[skip] {folder}: email invalido ou em falta")
        return False

    name, email = info["Nome"], info["Email"]
    print(f"[...] {name} <{email}>")

    link = upload_to_drive(zip_file)
    if not link:
        print(f"[erro] falhou upload para {name}")
        return False

    body = get_email_body(name, link)
    try:
        send_email(email, "Oficinas de Programacao - 2025", body)
    except Exception as e:
        print(f"[erro] falhou envio para {email}: {e}")
        return False

    print(f"[ok]  enviado para {email}  ({link})")
    return True


def main():
    if not os.path.exists(BASE):
        print(f"Pasta {BASE} nao encontrada. Nada a enviar.")
        return

    if not all([SMTP_SERVER, SMTP_PORT, EMAIL_SENDER, EMAIL_PASSWORD]):
        print("ERRO: credenciais SMTP em falta no .env (CONFIG_JSON).")
        return

    if not os.path.exists(SERVICE_ACCOUNT_FILE):
        print(f"ERRO: service_account.json nao encontrado em {SERVICE_ACCOUNT_FILE}")
        return

    seen = set()
    total, sent = 0, 0
    for name in os.listdir(BASE):
        folder = os.path.join(BASE, name)
        if not os.path.isdir(folder):
            continue
        total += 1

        info = read_user_data(os.path.join(folder, "user_data.txt")) if os.path.exists(os.path.join(folder, "user_data.txt")) else None
        if info and info["Email"] in seen:
            print(f"[skip] {folder}: email {info['Email']} ja processado")
            continue

        if process_folder(folder):
            sent += 1
            if info:
                seen.add(info["Email"])

    print(f"\nConcluido: {sent}/{total} emails enviados.")


if __name__ == "__main__":
    main()
