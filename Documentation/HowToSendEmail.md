# How to send Email

Before proceeding, make sure to follow the steps in [this file](./HowToCopyRepoWorkshopToUsb.md) .

## Steps to Send an Email

1. Take the `backupWorkshop-(table number)` folder from the USB drive (as described in the previous guide) and drag it to the desktop.
2. Open the `workshop-cli` folder.
3. Open the `ToolsAfterWorkshop` folder.
4. Locate the file named `send_email.bat` and double click on it.

This will zip the folder for each participant and send an email with Google Drive Link containing their respective `.zip` file.

`Note:` Emails may end up in the Spam folder.

## How to Edit the Email Text

If you want to modify the email content:

1. Open the `workshop-cli` folder.
2. Open the `ToolsAfterWorkshop` folder.
3. Locate the file named `emailText.txt` and open it.
4. Edit the text as needed.

`Note:` Do not change or remove the `{NOME}` and `{LINK}` paramater,as it the name of the participant in the workshop, and the Link to google drive folder.