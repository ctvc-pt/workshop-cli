import json
import os
import shutil
from dotenv import load_dotenv
import smtplib
from email.mime.multipart import MIMEMultipart
from email.mime.text import MIMEText
from googleapiclient.discovery import build
from google.oauth2 import service_account
import re

# Load .env file
load_dotenv()

# Load JSON config from environment
config_json = os.getenv('CONFIG_JSON')
if config_json is None:
    print("Erro: CONFIG_JSON não encontrado no .env")
    exit(1)
config = json.loads(config_json)

# Email configurations
SMTP_SERVER = config["SMTP_SERVER"]
SMTP_PORT = config["SMTP_PORT"]
EMAIL_SENDER = config["EMAIL_SENDER"]
EMAIL_PASSWORD = config["EMAIL_PASSWORD"]


SCOPES = ['https://www.googleapis.com/auth/drive']
SERVICE_ACCOUNT_FILE = 'service_account.json'
PARENT_FOLDER_ID = "1mWhVNfZhQrnFaheAouTY83_ltbD2sIAg"


# Function to validate email format and check if it's a valid one
def is_valid_email(email):
    # Use regex to validate email format
    regex = r'^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$'
    if re.match(regex, email):
        return True
    return False


def authenticate():
    creds = service_account.Credentials.from_service_account_file(SERVICE_ACCOUNT_FILE, scopes=SCOPES )
    return creds


# Function to upload file to Google Drive
def upload_to_drive(file_path):
    creds = authenticate()
    service = build('drive', 'v3', credentials=creds)

    file_name = os.path.basename(file_path)

    file_metadata = {
        'name' : file_name,
        'parents' : [PARENT_FOLDER_ID]
    }

    file = service.files().create(
        body = file_metadata,
        media_body=file_path,
        fields='id'
    ).execute()

    file_id = file.get("id")
    if file_id:
        return f"https://drive.google.com/uc?id={file_id}&export=download"  # Link direto para download
    return None

# Function to read user data
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

        # Validate email
        if not user_info["Email"] or not is_valid_email(user_info["Email"]):
            return None
        
        return user_info
    except Exception as e:
        return None

# Function to create a ZIP file
def zip_folder(folder_path, output_zip):
    try:
        shutil.make_archive(output_zip, 'zip', folder_path)
        zip_file = output_zip + ".zip"
        return zip_file
    except Exception as e:
        return None

# Function to read email template
def get_email_body(template_path, name, download_link):
    try:
        with open(template_path, "r", encoding="utf-8") as f:
            content = f.read()
        email_body = content.replace("{NOME}", name).replace("{LINK}", download_link)
        return email_body
    except Exception as e:
        return None

# Function to send email
def send_email(to_email, subject, body):
    try:
        msg = MIMEMultipart()
        msg["From"] = EMAIL_SENDER
        msg["To"] = to_email
        msg["Subject"] = subject
        msg.attach(MIMEText(body, "plain"))

        with smtplib.SMTP_SSL(SMTP_SERVER, SMTP_PORT) as server:
            server.login(EMAIL_SENDER, EMAIL_PASSWORD)
            server.send_message(msg)
        print(f"Email enviado com sucesso")
        return True
    except Exception as e:
        print(f"Erro ao enviar o email: {e}")
        return False

# Main function
def main():
    script_dir = os.path.dirname(os.path.abspath(__file__))
    desktop_path = os.path.join(os.path.expanduser("~"), "Desktop")

    # Find all "workshopBackup" folders
    repo_paths = [os.path.join(desktop_path, folder) for folder in os.listdir(desktop_path) if folder.startswith("workshopBackup") and os.path.isdir(os.path.join(desktop_path, folder))]
    
    email_template_path = os.path.join(script_dir, "emailText.txt")

    if not repo_paths:
        print("Nenhuma pasta 'workshopBackup' encontrada.")
        return

    if not os.path.exists(email_template_path):
        print(f"Template de email não encontrado em: {email_template_path}")
        return

    # Use a set to track emails already processed
    processed_emails = set()

    for repo_path in repo_paths:
        print(f"Verificando pasta: {repo_path}")
        for folder_name in os.listdir(repo_path):
            user_folder_path = os.path.join(repo_path, folder_name)
            if os.path.isdir(user_folder_path):

                # Get user data
                user_data_file = os.path.join(user_folder_path, "user_data.txt")
                if not os.path.exists(user_data_file):
                    continue

                user_info = get_user_data(user_data_file)
                if not user_info or not user_info["Email"]:
                    continue

                name = user_info["Nome"]
                email = user_info["Email"]

                # Skip if email already processed
                if email in processed_emails:
                    print(f"Email {email} já foi processado, pulando.")
                    continue

                print(f"Processando: {name} - {email}")

                # Paths
                mygame_path = os.path.join(user_folder_path, "mygame")
                build_path = os.path.join(user_folder_path, "build")
                if not os.path.exists(mygame_path) or not os.path.exists(build_path):
                    continue

                # Create temporary combined folder
                temp_combined_path = os.path.join(user_folder_path, f"temp_combined_{name.replace(' ', '-')}")
                os.makedirs(temp_combined_path, exist_ok=True)

                shutil.copytree(mygame_path, os.path.join(temp_combined_path, "mygame"))
                shutil.copytree(build_path, os.path.join(temp_combined_path, "build"))

                # Zip the folder
                combined_zip_path = os.path.join(user_folder_path, f"game_files_{name.replace(' ', '-')}")
                combined_zip_file = zip_folder(temp_combined_path, combined_zip_path)
                if not combined_zip_file:
                    shutil.rmtree(temp_combined_path)
                    continue

                # Upload to Google Drive
                download_link = upload_to_drive(combined_zip_file)
                if not download_link:
                    shutil.rmtree(temp_combined_path)
                    os.remove(combined_zip_file)
                    continue

                # Prepare email body
                email_body = get_email_body(email_template_path, name, download_link)
                if not email_body:
                    shutil.rmtree(temp_combined_path)
                    os.remove(combined_zip_file)
                    continue

                print(f"Download link: {download_link} for {name} - {email}")

                # Send email
                subject = f"Oficinas de Programação - 2025"
                if send_email(email, subject, email_body):
                    print(f"Email enviado para {email}")
                    processed_emails.add(email)  # Mark email as processed

                # Cleanup
                shutil.rmtree(temp_combined_path)
                os.remove(combined_zip_file)

if __name__ == "__main__":
    main()
