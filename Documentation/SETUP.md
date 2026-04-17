# Workshop CLI — Setup Guide

This guide describes how to prepare a fresh Windows PC to run the Workshop CLI. It is aimed at whoever prepares the machines for a workshop (mentors, organizers), not at the students using it.

## Prerequisites

- Windows 10 or 11, 64-bit.
- Administrator privileges on the target machine.
- Internet connection (for Ollama model pulls and, if missing locally, extension downloads).

## Quick setup

1. Copy (or clone) the repository to the user's Desktop, so the final path is:
   ```
   %USERPROFILE%\Desktop\workshop-cli
   ```
   The `Install.bat` script assumes this exact location.
2. Open `Install.bat` **as administrator** (right-click → *Run as administrator*).
3. Wait for it to finish. A `cli.lnk` shortcut is created on the Desktop and the workshop CLI opens automatically on the first run.

On subsequent sessions, launching the workshop is just a double-click on `cli.lnk`.

## What `Install.bat` does

The script is idempotent: each step checks whether the component is already present and only installs it if missing.

| Component                              | Source in `Resources/`                     | Purpose                                  |
|----------------------------------------|--------------------------------------------|------------------------------------------|
| .NET SDK                               | `dotnet-sdk-6.0.425-win-x64.exe`           | Required to run the CLI executable       |
| Python 3                               | `python-installer.exe`                     | Used by helper scripts (e.g. `open_vscode.py`) |
| PyGithub                               | installed via `pip`                        | Git operations from Python helpers       |
| Git                                    | `Git-2.46.2-64-bit.exe`                    | Used by the Git-based workshop workflow  |
| Ollama (runtime)                       | not bundled — must be installed manually if missing | Hosts the local LLM the CLI talks to |
| `llama3.2:1b` model                    | pulled via `ollama pull`                   | Provides on-device help responses        |
| Visual Studio Code                     | `VSCodeSetup.exe` (User Setup)             | The editor students use                  |
| VS Code extension `sumneko.lua`        | installed via `code --install-extension`   | Lua language support                     |
| VS Code extension `pixelbyte-studios.pixelbyte-love2d` | installed via `code --install-extension`   | Provides the Alt+L run command the guides reference |

The script also deletes any pre-existing `Resources\session.txt` (so the first launch is a clean session), creates the `cli.lnk` shortcut on the Desktop, and launches it once.

## After installation

- Students launch the workshop via the `cli.lnk` Desktop shortcut.
- The shortcut targets `CLI\CLI\WorkshopCli\bin\Debug\net6.0\WorkshopCli.exe` and runs it as administrator.
- Inside the editor, **Alt + L** runs the current Love2D game (provided by the `pixelbyte-love2d` extension).

## Building a distributable installer

If you need a single-file installer instead of running `Install.bat` directly on each machine, see the "How to Make an Installer" section in [`HowToRunWorkshopCLI.md`](./HowToRunWorkshopCLI.md). It uses Inno Setup against `Installer/workhopCLI.iss`, which bundles all the installers from `Resources/` and runs them during setup.

## Troubleshooting

- **`Install.bat` cannot find an installer** (e.g. `VSCodeSetup.exe` missing). The `Resources/` folder on disk may be incomplete. Copy the missing installer into `Resources/` and re-run `Install.bat`. The bundled binaries are tracked via Git LFS, so make sure LFS is installed and `git lfs pull` has run.
- **VS Code is installed but `code` is not found on PATH.** Restart the terminal after installation — the User Setup adds `code` to the PATH of the current user, but the update is only visible to new shells.
- **Alt+L does nothing inside VS Code.** Confirm the `pixelbyte-studios.pixelbyte-love2d` extension is installed (`code --list-extensions`). If it is, check `%APPDATA%\Code\User\keybindings.json` for a stale binding of `pixelbyte.love2d.run` to a different key.
- **Workshop asks for the name again even though the student already started.** The session file was deleted. If that was not intentional, restore `Resources/session.txt` from backup; otherwise just proceed.
- **Student wants to restart the workshop from scratch.** Delete `Resources/session.txt` (or run `delete_session.bat` if present).
