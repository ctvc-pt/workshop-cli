"""
Servidor HTTP que recebe os jogos dos miúdos durante o workshop.
Corre no portátil do monitor, na mesma rede WiFi que os PCs dos miúdos.

Endpoints:
  GET  /ping    -> "ok"  (usado pelo Install.bat para validar o IP)
  POST /upload  -> recebe zip + metadata, guarda em C:\\WorkshopReceiver
                   e dispara em background o POST para o webhook do n8n
                   (que trata de enviar o email com o zip em anexo).
"""
import os
import re
import sys
import threading
import datetime
import requests
from flask import Flask, request, jsonify

BASE = r"C:\WorkshopReceiver"
PORT = 5000
MAX_MB = 50
N8N_WEBHOOK_URL = "https://n8n.ctvc.pt/webhook/Workshops2026"
WEBHOOK_TIMEOUT = 60

app = Flask(__name__)
app.config["MAX_CONTENT_LENGTH"] = MAX_MB * 1024 * 1024


def safe_name(value: str) -> str:
    # Evita path traversal e caracteres inválidos em nomes de pastas do Windows.
    value = value.strip()
    value = re.sub(r"[^\w\-]", "-", value)
    return value[:80] or "sem-nome"


def post_to_n8n(nome: str, email: str, mesa: str, zip_path: str):
    # Em background: o miudo nao espera pelo email para o seu PC libertar.
    # Se falhar, o ficheiro fica em disco e o send_emails.py reenvia depois.
    ts = datetime.datetime.now().strftime("%H:%M:%S")
    try:
        with open(zip_path, "rb") as f:
            r = requests.post(
                N8N_WEBHOOK_URL,
                data={"nome": nome, "email": email, "mesa": mesa},
                files={"zip": (os.path.basename(zip_path), f, "application/zip")},
                timeout=WEBHOOK_TIMEOUT,
            )
        if r.ok:
            print(f"[n8n] {ts}  email pedido para {email}")
        else:
            print(f"[n8n] {ts}  ERRO {r.status_code} para {email}: {r.text[:200]}")
    except Exception as e:
        print(f"[n8n] {ts}  EXCEPCAO para {email}: {e}")


@app.route("/ping", methods=["GET"])
def ping():
    return "ok", 200


@app.route("/upload", methods=["POST"])
def upload():
    email = request.form.get("email", "").strip()
    nome = safe_name(request.form.get("nome", ""))
    mesa = safe_name(request.form.get("mesa", "sem-mesa"))
    zipf = request.files.get("zip")

    if not email or not zipf:
        return jsonify(error="email e zip sao obrigatorios"), 400

    date = datetime.datetime.now().strftime("%d-%m-%Y")
    folder = os.path.join(BASE, f"{nome}_mesa{mesa}_{date}")
    os.makedirs(folder, exist_ok=True)

    zip_path = os.path.join(folder, "game.zip")
    zipf.save(zip_path)

    # Mantemos user_data.txt para o send_emails.py poder reenviar a partir do disco.
    user_data = os.path.join(folder, "user_data.txt")
    nome_legivel = nome.replace("-", " ")
    with open(user_data, "w", encoding="utf-8") as f:
        f.write("Dados do Usuario:\n")
        f.write(f"Nome: {nome_legivel}\n")
        f.write(f"Email: {email}\n")

    size_kb = os.path.getsize(zip_path) // 1024
    print(f"[OK] {datetime.datetime.now().strftime('%H:%M:%S')}  {nome}  <{email}>  {size_kb} KB  -> {folder}")

    threading.Thread(
        target=post_to_n8n,
        args=(nome_legivel, email, mesa, zip_path),
        daemon=True,
    ).start()

    return jsonify(status="ok", folder=folder), 200


if __name__ == "__main__":
    os.makedirs(BASE, exist_ok=True)
    print(f"Receptor a escutar em 0.0.0.0:{PORT}")
    print(f"Pasta de destino: {BASE}")
    print(f"Webhook n8n:     {N8N_WEBHOOK_URL}")
    print("Ctrl+C para parar.\n")
    # debug=False para nao ter o reloader (queremos que o monitor veja os logs limpos).
    app.run(host="0.0.0.0", port=PORT, debug=False)
