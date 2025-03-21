using System;
using System.Diagnostics;
using Newtonsoft.Json;
using Sharprompt;
using System.Reflection;
using System.Runtime.InteropServices;

namespace workshopCli
{
    public class OpenFileL2DAction : IAction
    {
        string stepId;
        public int Delay;
        Process autoHotkeyProcess;

        public OpenFileL2DAction(string stepId, int delay)
        {
            this.stepId = stepId;
            Delay = delay;
            CloseAHKProcess();
        }

        public void Execute()
        {
            var VsCode = new OpenVSCode();
            Console.ForegroundColor = ConsoleColor.Black;
            var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "repoWorkshop");
            var txtFilePath = Path.Combine(GuideCli.ResourcesPath, "session.txt");
            var sessionData = File.ReadAllText(txtFilePath);
            var session = JsonConvert.DeserializeObject<Session>(sessionData);

            var username = session.Name?.Replace(" ", "-") ?? "default-user";
            var dateStamp = DateTime.Now.ToString("dd-MM-yyyy");
            var userFolderPath = Path.Combine(desktopPath, $"{username}_{dateStamp}");
            var folderPath = Path.Combine(userFolderPath, "mygame"); 
            var filePath = Path.Combine(folderPath, "main.lua"); 
            var userDataFilePath = Path.Combine(userFolderPath, "user_data.txt"); 

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath); 
            }

            
            if (!File.Exists(userDataFilePath))
            {
                using (var sw = File.CreateText(userDataFilePath))
                {
                    sw.WriteLine($"Dados do Usuário:");
                    sw.WriteLine($"Nome: {session.Name ?? "N/A"}");
                    sw.WriteLine($"Idade: {session.Age ?? "N/A"}");
                    sw.WriteLine($"Email: {session.Email ?? "N/A"}");
                }
            }

            if (!File.Exists(filePath))
            {
                using (var sw = File.CreateText(filePath))
                {
                    sw.WriteLine("--É aqui onde começa a tua aventura");
                }
            }
            try
            {

                VsCode.Open();
                Thread.Sleep(1000);

                // Open CLI using AutoHotKey
                var startAhkR = new ProcessStartInfo
                {
                    FileName = Path.Combine(GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "AutoHotkeyU64.exe"),
                    Arguments = Path.Combine(GuideCli.ResourcesPath, "vsc.ahk"),
                    WorkingDirectory = @"C:\",
                    Verb = "runas"
                };
                autoHotkeyProcess = Process.Start(startAhkR);

                var settingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Code",
            "User",
            "settings.json");

        if (!File.Exists(settingsPath))
        {
            string sourceFile = Path.Combine(GuideCli.ResourcesPath, "settings.json");
            string destinationFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Code",
                "User",
                "settings.json");
            File.Copy(sourceFile, destinationFolder, true);
        }

        var jsonset = File.ReadAllText(settingsPath);

        // Verifica se a configuração "files.autoSave" já está presente no JSON
        if (!jsonset.Contains("\"files.autoSave\":"))
        {
            // Adiciona a configuração "files.autoSave" com o valor "afterDelay"
            jsonset = jsonset.TrimEnd();
            if (jsonset.EndsWith("}"))
            {
                jsonset = jsonset.Substring(0, jsonset.Length - 1);
            }

            jsonset += ",\n  \"files.autoSave\": \"afterDelay\"\n}";
        }
        else
        {
            // Substitui o valor de "files.autoSave" para "afterDelay" se já existir
            jsonset = jsonset.Replace("\"files.autoSave\": \"off\"", "\"files.autoSave\": \"afterDelay\"");
        }

        // Adiciona ou atualiza a configuração para autoformatação e Lua
        if (!jsonset.Contains("\"editor.formatOnSave\":"))
        {
            jsonset = jsonset.TrimEnd();
            if (jsonset.EndsWith("}"))
            {
                jsonset = jsonset.Substring(0, jsonset.Length - 1);
            }

            jsonset += ",\n  \"editor.formatOnSave\": true\n}";
        }
        else
        {
            jsonset = jsonset.Replace("\"editor.formatOnSave\": false", "\"editor.formatOnSave\": true");
        }

        // Adiciona ou atualiza a configuração para formatOnPaste
        if (!jsonset.Contains("\"editor.formatOnPaste\":"))
        {
            jsonset = jsonset.TrimEnd();
            if (jsonset.EndsWith("}"))
            {
                jsonset = jsonset.Substring(0, jsonset.Length - 1);
            }

            jsonset += ",\n  \"editor.formatOnPaste\": true\n}";
        }
        else
        {
            jsonset = jsonset.Replace("\"editor.formatOnPaste\": false", "\"editor.formatOnPaste\": true");
        }

        // Adiciona ou atualiza a configuração para formatOnType
        if (!jsonset.Contains("\"editor.formatOnType\":"))
        {
            jsonset = jsonset.TrimEnd();
            if (jsonset.EndsWith("}"))
            {
                jsonset = jsonset.Substring(0, jsonset.Length - 1);
            }

            jsonset += ",\n  \"editor.formatOnType\": true\n}";
        }
        else
        {
            jsonset = jsonset.Replace("\"editor.formatOnType\": false", "\"editor.formatOnType\": true");
        }

        if (!jsonset.Contains("\"[lua]\":"))
        {
            jsonset = jsonset.TrimEnd();
            if (jsonset.EndsWith("}"))
            {
                jsonset = jsonset.Substring(0, jsonset.Length - 1);
            }

            jsonset += ",\n  \"[lua]\": {\n    \"editor.defaultFormatter\": \"sumneko.lua\"\n  }\n}";
        }
        else
        {
            // Atualiza o formatador padrão para arquivos Lua se já existir
            if (jsonset.Contains("\"[lua]\":"))
            {
                int luaIndex = jsonset.IndexOf("\"[lua]\":");
                int luaEndIndex = jsonset.IndexOf("}", luaIndex);
                string luaSettings = jsonset.Substring(luaIndex, luaEndIndex - luaIndex + 1);

                if (!luaSettings.Contains("\"editor.defaultFormatter\": \"sumneko.lua\""))
                {
                    luaSettings = luaSettings.TrimEnd();
                    if (luaSettings.EndsWith("}"))
                    {
                        luaSettings = luaSettings.Substring(0, luaSettings.Length - 1);
                    }

                    luaSettings += ",\n    \"editor.defaultFormatter\": \"sumneko.lua\"\n  }";

                    jsonset = jsonset.Substring(0, luaIndex) + luaSettings + jsonset.Substring(luaEndIndex + 1);
                }
            }
        }

        File.WriteAllText(settingsPath, jsonset);

        Console.WriteLine("Configurações atualizadas com sucesso.");
    

                KeyboardShortcut.AddKeyboardShortcut();

                var startFileInfo = new ProcessStartInfo
                {
                    FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/../Local/Programs/Microsoft VS Code/Code.exe",
                    Arguments = $"\"{filePath}\" --disable-workspace-trust",
                    WorkingDirectory = @"C:\",
                    Verb = "runas"
                };

                // Process.Start(startFileInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error opening file: " + ex.Message);
            }


            Thread.Sleep(500);

            // Console.Clear();

            var MDPath = $"{stepId}.md";
            var assembly = Assembly.GetExecutingAssembly();

            var resourceStream = assembly.GetManifestResourceStream($"workshop_cli.Guide.{MDPath}");
            if (resourceStream != null)
            {
                using (var reader = new StreamReader(resourceStream))
                {
                    var fileContents = reader.ReadToEnd();
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(fileContents);
                    Console.ForegroundColor = ConsoleColor.Black;
                }
            }
            
            Thread.Sleep(Delay);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            ExerciseHelper.PromptAnswerAndConfirm("Verifica o código e escreve 'proximo' ou 'p' para continuar ou para retroceder escreve 'anterior' (ou 'ajuda')\n");
        }

        // Add event to close AutoHotKeys
        public void CloseAHKProcess()
        {
            if (autoHotkeyProcess != null && !autoHotkeyProcess.HasExited)
            {
                autoHotkeyProcess.Kill();
                autoHotkeyProcess.WaitForExit();
                autoHotkeyProcess.Dispose();
            }
        }
    }
}
