# How to Send Emails via n8n

This document describes the **post-2026 email pipeline**: when a kid finishes their game, the receiver uploads it to a shared n8n webhook. n8n stores the zip on Google Drive, calls a Google Apps Script Web App that builds a personalised PDF certificate, archives that certificate in a dedicated Drive folder, and emails the kid the game-zip Drive link plus the certificate as an attachment.

For the old SMTP+Drive flow (`ToolsAfterWorkshop/send_email.py` / `Receiver/send_emails.py`'s previous version), see git history before the n8n migration.

## Why n8n

The CTVC Google Workspace blocks `.zip` attachments by policy ("Mail Delivery Subsystem — For security reasons, CTVC Mail does not allow you to use this type of file…"). Going through Drive + a shared link sidesteps that, and n8n removes the need to ship SMTP credentials and `service_account.json` to every monitor's laptop.

## Architecture

```
Kid's PC                 Monitor's laptop           n8n.ctvc.pt              Google
-------------            ------------------         --------------------     ----------
WorkshopCli  ──upload──> workshop_receiver.py ────> Webhook (POST)
                              │                         │
                              └─ saves locally          ├─> Drive: Upload zip       (game zips folder)
                                 C:\WorkshopReceiver    │
                                                        ├─> Apps Script Web App ──> personalised PDF certificate
                                                        │     (fills {{nome}} in
                                                        │      a Google Doc template)
                                                        │
                                                        ├─> Drive: Upload PDF        (certificates archive folder)
                                                        │
                                                        └─> Gmail: link + PDF attachment
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

The workflow has 6 nodes. The first 4 run in series; after **Move Base64 to File** the chain branches into two parallel sinks (Drive archive and Gmail send) so both consume the same binary `data`.

```
Webhook ──> Upload file ──> Generate Certificate ──> Move Base64 to File ─┬─> Upload Certificate (Drive archive)
            (Drive zip)     (HTTP Request)           (binary conversion)  │
                                                                          └─> Send a message (Gmail)
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

### 3. Generate Certificate (HTTP Request)

Calls the `Workshop CLI 2026` Apps Script Web App with the kid's name. The web app copies the certificate template, replaces `{{nome}}`, exports to PDF and returns the bytes as base64. See **Apps Script: certificate generator** below for what lives on the Google side.

| Setting | Value |
|---|---|
| Method | `POST` |
| URL | the Web App `/exec` URL (see Apps Script section) |
| Send Body | ON |
| Body Content Type | `JSON` |
| Specify Body | `Using JSON` |
| JSON | `{ "nome": "{{ $('Webhook').item.json.body.nome }}" }` |
| Options → Redirects → Follow Redirects | ON |

The `Follow Redirects` toggle matters — Apps Script returns the payload behind a 302 to `googleusercontent.com`.

The response shape is:
```json
{
  "filename": "Certificado-<nome>.pdf",
  "content":  "<base64-encoded PDF bytes>",
  "mimeType": "application/pdf"
}
```

### 4. Move Base64 String to File

Converts the `content` string from the previous node into an n8n binary field that downstream nodes can consume as a file.

| Setting | Value |
|---|---|
| Operation | `Move Base64 String to File` |
| Base64 Input Field | `content` |
| Put Output File in Field | `data` |
| Options → File Name (expression) | `={{ $json.filename }}` |
| Options → MIME Type (fixed) | `application/pdf` |

After this node the item carries a binary field named `data` containing the PDF. The output of this node feeds **two** parallel sinks — the Drive archive (node 5) and the Gmail send (node 6) — by dragging two connections out of it. Don't put them in series: a Google Drive Upload node replaces the binary with the new file's metadata, so a Gmail node placed after it would have no `data` to attach.

### 5. Upload Certificate (Google Drive)

Archives a copy of every generated certificate in a dedicated Drive folder, separate from the game-zips folder used by node 2. Useful for reprints, registry, and as a fallback if Gmail delivery fails.

| Setting | Value |
|---|---|
| Credential | `Drive Ines CTVC` |
| Resource | `File` |
| Operation | `Upload` |
| Input Data Field Name | `data` |
| File Name (expression) | `={{ $json.filename }}` |
| Parent Drive | `My Drive` |
| Parent Folder | `By URL` → URL of the certificates folder |

The folder is private — the script runs under the coordinator account and the PDF doesn't need to be world-readable (the kid receives it as an email attachment, not as a link).

### 6. Send a message (Gmail)

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

**Attachments option** (the only attachment is the certificate PDF):

| Field | Value |
|---|---|
| Add Option → Attachments → Attachment Field Name | `data` |

The zip itself is **not** attached — it lives on Drive and the email carries only the link to it. The PDF certificate is the sole attachment, picked up from the `data` binary produced by node 4.

## Apps Script: certificate generator

The Apps Script project `Workshop CLI 2026` is a Google Apps Script Web App that personalises the certificate template with the kid's name and returns the PDF bytes as base64. It sits between n8n's Drive upload and the Gmail send (node 3 in the workflow above).

### Why Apps Script

- The certificate is a Google Doc, and only Apps Script (`DocumentApp`) can do native text replacement on Docs.
- Running it as a Web App means n8n can call it over plain HTTP without dragging Google credentials around.

### Template setup

The certificate is a **Google Doc** (a native Doc, not a `.docx`) with the literal placeholder `{{nome}}` where the kid's name should appear.

If the source artwork is a `.docx`:

1. Upload it to Drive
2. Right-click → **Open with → Google Docs**
3. Verify the layout (gold border, signature image, position of `{{nome}}`) survived the conversion — `.docx` → Doc occasionally shifts elements
4. The new Google Doc is the template; copy its file ID from the URL (`/document/d/<ID>/edit`) and paste it into the script's `TEMPLATE_ID`

The placeholder must be a single uninterrupted run of text. If part of `{{nome}}` is bold and part isn't, `replaceText` will skip it — re-type the placeholder cleanly in the Doc.

### Code

`Código.gs`:

```javascript
const TEMPLATE_ID = '<google-doc-file-id>';

function doPost(e) {
  const { nome } = JSON.parse(e.postData.contents);

  // Copy the template
  const copy = DriveApp.getFileById(TEMPLATE_ID).makeCopy(`Certificado - ${nome}`);
  const copyId = copy.getId();

  // Replace {{nome}}
  const doc = DocumentApp.openById(copyId);
  doc.getBody().replaceText('\\{\\{nome\\}\\}', nome);
  doc.saveAndClose();

  // Export as PDF
  const pdfBlob = DriveApp.getFileById(copyId).getAs('application/pdf');
  const pdfBase64 = Utilities.base64Encode(pdfBlob.getBytes());

  // Discard the temporary copy
  DriveApp.getFileById(copyId).setTrashed(true);

  return ContentService
    .createTextOutput(JSON.stringify({
      filename: `Certificado-${nome}.pdf`,
      content: pdfBase64,
      mimeType: 'application/pdf'
    }))
    .setMimeType(ContentService.MimeType.JSON);
}
```

`replaceText` takes a regex, so `{` and `}` need escaping — that's why the placeholder is `'\\{\\{nome\\}\\}'` and not `'{{nome}}'`.

### Deploy as Web App

1. **Implementar → Nova implementação**
2. Tipo: **Aplicação Web**
3. Executar como: **Eu** (the workshop coordinator's account, so the script has access to the template Doc)
4. Quem tem acesso: **Qualquer pessoa** — the most permissive option. It must NOT be "Qualquer pessoa com conta Google", or n8n's anonymous POST will get 401
5. Authorise the script when prompted (it needs Drive + Docs scopes; click through the "Google hasn't verified this app" warning)
6. Copy the Web App URL ending in `/exec` and paste it into the n8n HTTP Request node (node 3)

After every code change you must do **Implementar → Gerir implementações → editar → Nova versão**, otherwise n8n keeps hitting the cached old code on the same URL.

### Smoke-test from the command line

```powershell
$r = Invoke-RestMethod -Uri "<exec-url>" -Method Post -ContentType "application/json" -Body '{"nome":"Teste Silva"}'
$r.filename                                    # Certificado-Teste Silva.pdf
$r.mimeType                                    # application/pdf
$r.content.Substring(0,6)                      # JVBERi  (= "%PDF-" in base64 — sanity check)

[IO.File]::WriteAllBytes("$HOME\Desktop\teste.pdf", [Convert]::FromBase64String($r.content))
Invoke-Item "$HOME\Desktop\teste.pdf"
```

If the response is 401, the deploy access setting is wrong (see step 4). If the PDF opens but `{{nome}}` is still literal in the output, the placeholder in the template has mixed formatting or the wrong characters.

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
A `.zip` (or `.exe`, etc.) is being attached. With this pipeline only the certificate PDF should be attached — make sure the Gmail node's `Attachment Field Name` is `data` (the binary produced by **Move Base64 String to File**), and that nothing else is feeding a binary into the Gmail node.

### Email arrives without the certificate attached
Either the **Move Base64 String to File** node didn't run, or its `Put Output File in Field` doesn't match the Gmail node's `Attachment Field Name`. Both must be the same string (default `data`). Open the Move Base64 node's output and confirm a **Binary** tab shows up with the PDF.

Also check the topology: **Upload Certificate** and **Send a message** must both branch out of `Move Base64` in parallel — not in series. If you wired them as `Move Base64 → Upload Certificate → Send a message`, the Drive Upload swallows the binary and the Gmail node has nothing to attach.

### Certificate doesn't appear in the archive folder
The **Upload Certificate** node's `Parent Folder` URL is wrong, the credential doesn't have access to that folder, or the node is wired in the wrong place in the chain (must hang off `Move Base64`, parallel to the Gmail node — see above). Look in n8n **Executions** for that node's status and error message.

### Apps Script returns 401 Unauthorized
The Web App's access setting is too restrictive. **Implementar → Gerir implementações → edit → Quem tem acesso: Qualquer pessoa** (not "Qualquer pessoa com conta Google"). If editing the existing deployment doesn't take effect, do **Nova implementação** instead — sometimes the deploy cache is sticky. The URL changes when you do that, so update the n8n HTTP Request node accordingly.

### Certificate PDF shows `{{nome}}` literally
The placeholder in the Google Doc has stray formatting (e.g. only part of it is bold or in a different font), so `replaceText` can't match it. Re-type `{{nome}}` cleanly in one uninterrupted run. Also confirm the template is a native **Google Doc**, not still a `.docx` (Apps Script's `DocumentApp` only works on native Docs).

### Apps Script changes don't show up
After editing `Código.gs` you must do **Implementar → Gerir implementações → editar → Nova versão**. Just saving the script file is not enough — n8n keeps calling the previously deployed version on the same `/exec` URL.

### The kid clicks the link and gets 403
The parent folder isn't shared as "Anyone with the link can view". Open the folder in Drive and fix the share setting once — existing file permissions update automatically.

### Receiver logs `[n8n] EXCEPCAO …`
Network failed during the workshop. The local files in `C:\WorkshopReceiver` are intact — run `send_emails.bat` after the workshop from a stable connection to retry.

## Changing the email text

The HTML body lives in the Gmail node's **Message** field inside n8n — edit it there, save the workflow. There is **no** `emailText.txt` template in this pipeline.

The `nome` and link expressions to keep:
- `{{ $('Webhook').item.json.body.nome }}` — the kid's name
- `{{ $('Upload file').item.json.webViewLink }}` — the Drive link
