# Things to do manually

`Install.bat` handles .NET 8, Python, Git, Ollama (+ the `qwen2.5:3b` model), Visual Studio Code and the required VS Code extensions automatically. Only the step below still needs to be done by hand:

1. Right click on the `cli` shortcut on the Desktop, click on `Properties`, go to `Compatibility`, and select the `Run this program as an administrator` option. (The `Install.bat` shortcut logic only sets the working directory, not the elevation flag.)

## If `Install.bat` could not install Ollama

If `Install.bat` logs *"Ollama is still not detected after install"* — usually a Device Guard / WDAC block on a managed machine — install Ollama manually (e.g. from <https://ollama.com/download>), then re-run `Install.bat` so it pulls the `qwen2.5:3b` model and starts the server.

## No issues

If the student closes the CLI prompt, just double-click the `cli` shortcut on the Desktop again.
