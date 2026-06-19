"""
Re-envio manual: corre depois do workshop se algum email falhou em tempo real.

Le as pastas em C:\\WorkshopReceiver\\* (cada uma com game.zip + user_data.txt)
e faz POST ao webhook do n8n para cada miudo. A logica de Drive/email vive
toda no n8n; este script so se preocupa em ler os ficheiros e fazer o POST.
"""
import os
import re
import datetime
import requests

BASE = r"C:\WorkshopReceiver"
N8N_WEBHOOK_URL = "https://n8n.ctvc.pt/webhook/Workshops2026"
WEBHOOK_TIMEOUT = 60


def is_valid_email(email: str) -> bool:
    return bool(re.match(r"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", email))


def read_user_data(path: str):
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


def process_folder(folder: str) -> bool:
    user_data_file = os.path.join(folder, "user_data.txt")
    zip_file = os.path.join(folder, "game.zip")

    if not os.path.exists(user_data_file) or not os.path.exists(zip_file):
        print(f"[skip] {folder}: faltam user_data.txt ou game.zip")
        return False

    info = read_user_data(user_data_file)
    if info is None:
        print(f"[skip] {folder}: email invalido ou em falta")
        return False

    nome, email = info["Nome"], info["Email"]
    print(f"[...] {nome} <{email}>")

    try:
        with open(zip_file, "rb") as f:
            r = requests.post(
                N8N_WEBHOOK_URL,
                data={"nome": nome, "email": email, "mesa": ""},
                files={"zip": (os.path.basename(zip_file), f, "application/zip")},
                timeout=WEBHOOK_TIMEOUT,
            )
        if not r.ok:
            print(f"[erro] {email}: HTTP {r.status_code} - {r.text[:200]}")
            return False
    except Exception as e:
        print(f"[erro] {email}: {e}")
        return False

    print(f"[ok]  pedido para {email}")
    return True


def main():
    if not os.path.exists(BASE):
        print(f"Pasta {BASE} nao encontrada. Nada a enviar.")
        return

    seen = set()
    total, sent = 0, 0
    for name in os.listdir(BASE):
        folder = os.path.join(BASE, name)
        if not os.path.isdir(folder):
            continue
        total += 1

        info_path = os.path.join(folder, "user_data.txt")
        info = read_user_data(info_path) if os.path.exists(info_path) else None
        if info and info["Email"] in seen:
            print(f"[skip] {folder}: email {info['Email']} ja processado")
            continue

        if process_folder(folder):
            sent += 1
            if info:
                seen.add(info["Email"])

    print(f"\nConcluido: {sent}/{total} envios para o n8n.")


if __name__ == "__main__":
    main()
