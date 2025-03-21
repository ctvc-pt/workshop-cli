# How to send Email

Before proceeding, make sure to follow the steps in [this file](./HowToCopyRepoWorkshopToUsb.md) .

## Steps to Send an Email

1. Take the `backupWorkshop-(table number)` folder from the USB drive (as described in the previous guide) and drag it to the desktop.
2. Open the `workshop-cli` folder.
3. Open the `ToolsAfterWorkshop` folder.
4. Locate the file named `send_email.bat` and double click on it.

This will zip the `mygame` folder for each participant and send an email with their respective `mygame.zip` file.

`Note:` This script does not send emails to `@hotmail` domains.

## How to Edit the Email Text

If you want to modify the email content:

1. Open the `workshop-cli` folder.
2. Open the `ToolsAfterWorkshop` folder.
3. Locate the file named `emailText.txt` and open it.
4. Edit the text as needed.

`Note:` Do not change or remove the `{NOME}` paramater,as it the name of the participant in the workshop.