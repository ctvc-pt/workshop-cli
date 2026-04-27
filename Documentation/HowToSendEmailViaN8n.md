# How to Send Emails via n8n

This document describes the **post-2026 email pipeline**: when a kid finishes their game, the receiver uploads it to a shared n8n webhook that takes care of storing the zip on Google Drive and emailing the kid a link.

For the old SMTP+Drive flow (`ToolsAfterWorkshop/send_email.py` / `Receiver/send_emails.py`'s previous version), see git history before the n8n migration.

## Why n8n

The CTVC Google Workspace blocks `.zip` attachments by policy ("Mail Delivery Subsystem — For security reasons, CTVC Mail does not allow you to use this type of file…"). Going through Drive + a shared link sidesteps that, and n8n removes the need to ship SMTP credentials and `service_account.json` to every monitor's laptop.

## Architecture

```
Kid's PC                 Monitor's laptop           n8n.ctvc.pt              Google
-------------            ------------------         --------------------     ----------
WorkshopCli  ──upload──> workshop_receiver.py ────> Webhook (POST)
                              │                         │
                              └─ saves locally          ├─> Drive: Upload file
                                 C:\WorkshopReceiver    │
                                                        └─> Gmail: send link
                                                                          ──> Kid's email
```

Two reasons the receiver still saves locally:

1. If the workshop venue's internet flaps, the POST to n8n may fail. The local copy lets us re-run `send_emails.py` later from a stable connection.
2. The same local copy is what the existing post-workshop tooling already expects.

## Webhook

- **Production URL**: `https://n8n.ctvc.pt/webhook/Workshops2026`
- **Method**: `POST`
- **Body**: `multipart/form-data` with these fields:

  | Field  | Type   | Notes                                 |
  |--------|--------|---------------------------------------|
  | `nome` | string | kid's name (already cleaned by receiver) |
  | `email`| string | recipient                             |
  | `mesa` | string | optional, used only in the local folder name |
  | `zip`  | file   | the game zip, `application/zip`       |

The n8n workflow is set to **Respond: Immediately**, so the receiver doesn't wait for Drive upload + email.

## n8n Workflow

The workflow has 3 nodes in series:

```
Webhook ──> Upload file (Google Drive) ──> Send a message (Gmail)
```

### 1. Webhook node

| Setting | Value |
|---|---|
| HTTP Method | `POST` |
| Path | `Workshops2026` |
| Authentication | `None` |
| Respond | `Immediately` |
| Options → Field Name for Binary Data | `zip` |

> n8n appends an index to the binary field name on multipart uploads, so the binary actually arrives as `zip0` (1 file) — that's the name to use later in the Drive node.

### 2. Upload file (Google Drive)

| Setting | Value |
|---|---|
| Resource | `File` |
| Operation | `Upload` |
| Input Binary Field | `zip0` |
| File Name | `={{ $json.body.nome }}.zip` |
| Parent Folder | (the shared workshop folder) |

**Important — share the parent folder once, manually**:
1. Open the parent folder in Google Drive
2. Share → "Anyone with the link" → "Viewer"

This way every uploaded file inherits public-read permission and the workflow doesn't need a separate Share node.

### 3. Send a message (Gmail)

All three text fields **must be in expression mode** (the `fx` icon, expression highlighted in green). After the Drive node, `$json` refers to the Drive output, so to read the original webhook payload we have to reference the Webhook node by name.

| Field | Value |
|---|---|
| Credential | `ines acc - ctvc` |
| Resource | `Message` |
| Operation | `Send` |
| To | `{{ $('Webhook').item.json.body.email }}` |
| Subject | `Oficinas de Programação 2026 — {{ $('Webhook').item.json.body.nome }}` |
| Email Type | `HTML` |

**Message** (HTML):
```
Olá {{ $('Webhook').item.json.body.nome }},<br><br>Descarrega o teu jogo aqui:<br><a href="{{ $('Upload file').item.json.webViewLink }}">{{ $('Upload file').item.json.webViewLink }}</a><br><br>Abraço,<br>CTVC
```

**Do not** add an Attachments option. The zip is on Drive — the email carries only the link.

## Code Side

### Real-time path: `Receiver/workshop_receiver.py`

When a kid uploads via `/upload`:
1. Saves `game.zip` and `user_data.txt` under `C:\WorkshopReceiver\<nome>_mesa<x>_<date>\`
2. Spawns a **daemon thread** that POSTs the same data to the webhook (60s timeout)
3. Returns `200 OK` to the kid's PC immediately — the email send doesn't block them

If the POST fails, it's logged in the receiver's console but no retry happens — that's what the fallback script is for.

The webhook URL is hardcoded at the top of the file:
```python
N8N_WEBHOOK_URL = "https://n8n.ctvc.pt/webhook/Workshops2026"
```

### Fallback path: `Receiver/send_emails.py`

Run after the workshop, from a stable connection, to retry any folder where the real-time POST failed. It walks `C:\WorkshopReceiver\*`, reads each `user_data.txt`, and POSTs the same payload to the webhook. The n8n workflow is the same — there's no "skip Drive" path; re-running just creates another file in Drive and sends another email.

To invoke it, double-click `Receiver/send_emails.bat`.

## Size Limits

| Layer | Limit | Where to bump |
|---|---|---|
| Receiver Flask | 50 MB | `workshop_receiver.py:17` (`MAX_MB`) |
| n8n payload | **16 MB** (default) | `N8N_PAYLOAD_SIZE_MAX` env var on the n8n container |
| Google Drive | 5 TB | n/a |
| Gmail | n/a (link only) | n/a |

The effective limit is the **n8n default of ~16 MB**. To raise, both layers need to grow together.

Typical workshop game zips are 2–8 MB, so the default is comfortable.

## Testing the workflow

1. In the n8n editor, make sure the workflow is **Active** (toggle top-right). Until it's active, the production URL returns 404.
2. Start the receiver: `Receiver/start_receiver.bat`
3. From any other PC on the same WiFi, run a kid's CLI session through to the upload step.
4. Watch the receiver console — you should see:
   ```
   [OK]  10:30:00  jorge-b  <…@gmail.com>  4310 KB  -> C:\WorkshopReceiver\jorge-b_mesa1_27-04-2026
   [n8n] 10:30:02  email pedido para …@gmail.com
   ```
5. Confirm the kid's inbox (and check spam — the message comes from `ivaz@ctvc.pt`).
6. In the n8n editor under **Executions**, the run should show all 3 nodes green.

To test without activating the workflow, click **Listen for test event** in the Webhook node and temporarily change `N8N_WEBHOOK_URL` to the test URL (`/webhook-test/Workshops2026`). The test listener captures one event then stops.

## Troubleshooting

### `The item has no binary field 'zip0'`
The Gmail node's Attachment Field Name (or the Drive node's Input Binary Field) doesn't match what n8n named the upload. Open the Webhook node's **OUTPUT → Binary** tab — the name shown there (e.g. `zip0`, `zip00`) is the source of truth. Mirror it in downstream nodes.

### `Invalid email address - The email address '' in the 'To' field isn't valid`
The Gmail node is reading `$json.body.email`, but `$json` after the Drive node is the Drive output (no `body`). Reference the original webhook explicitly: `{{ $('Webhook').item.json.body.email }}`. Same for Subject and Message.

### `{{ $('Webhook').item.json.body.X }}` shows up literally in the email
The field is in text mode, not expression mode. Click `fx` next to the field and re-paste the expression. When it's in expression mode the resolved value shows in green under the field.

### `For security reasons, CTVC Mail does not allow you to use this type of file…`
A `.zip` (or `.exe`, etc.) is being attached. With this pipeline that should never happen — make sure no Attachments option is configured on the Gmail node.

### The kid clicks the link and gets 403
The parent folder isn't shared as "Anyone with the link can view". Open the folder in Drive and fix the share setting once — existing file permissions update automatically.

### Receiver logs `[n8n] EXCEPCAO …`
Network failed during the workshop. The local files in `C:\WorkshopReceiver` are intact — run `send_emails.bat` after the workshop from a stable connection to retry.

## Changing the email text

The HTML body lives in the Gmail node's **Message** field inside n8n — edit it there, save the workflow. There is **no** `emailText.txt` template in this pipeline.

The `nome` and link expressions to keep:
- `{{ $('Webhook').item.json.body.nome }}` — the kid's name
- `{{ $('Upload file').item.json.webViewLink }}` — the Drive link
