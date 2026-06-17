using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace workshopCli
{
    public static class VSCodeLauncher
    {
        public static string UserDataDirectory => Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WorkshopCli",
            "vscode-user-data");

        public static bool Open(string targetPath, bool runAs = false)
        {
            PrepareWorkshopWorkspace(targetPath);

            if (IsAlreadyOpen())
            {
                Console.WriteLine("Visual Studio Code ja esta aberto.");
                return true;
            }

            foreach (var executablePath in GetCandidatePaths())
            {
                if (!File.Exists(executablePath))
                {
                    continue;
                }

                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = executablePath,
                        Arguments = $"--disable-extensions --user-data-dir \"{UserDataDirectory}\" --disable-workspace-trust \"{targetPath}\"",
                        WorkingDirectory = @"C:\",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };

                    if (runAs)
                    {
                        startInfo.Verb = "runas";
                    }

                    var process = new Process { StartInfo = startInfo };
                    process.OutputDataReceived += (_, _) => { };
                    process.ErrorDataReceived += (_, _) => { };
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    return true;
                }
                catch
                {
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Nao consegui abrir o Visual Studio Code automaticamente.");
            Console.WriteLine($"Abre esta pasta manualmente: {targetPath}");
            Console.ResetColor();
            return false;
        }

        private static bool IsAlreadyOpen()
        {
            foreach (var process in Process.GetProcessesByName("Code"))
            {
                if (process.MainWindowHandle != IntPtr.Zero)
                {
                    return true;
                }
            }

            return false;
        }

        private static string[] GetCandidatePaths()
        {
            var vsCodeFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Programs",
                "Microsoft VS Code");

            return new[]
            {
                Path.Combine(vsCodeFolder, "Code.exe"),
                Path.Combine(vsCodeFolder, "bin", "code.cmd")
            };
        }

        public static void PrepareWorkshopWorkspace(string targetPath)
        {
            Directory.CreateDirectory(targetPath);
            Directory.CreateDirectory(Path.Combine(targetPath, ".vscode"));
            Directory.CreateDirectory(Path.Combine(UserDataDirectory, "User"));

            WriteRunLoveTask(targetPath);
            KeyboardShortcut.AddKeyboardShortcut();
            WriteWorkshopSettings();
        }

        private static void WriteRunLoveTask(string targetPath)
        {
            var tasksPath = Path.Combine(targetPath, ".vscode", "tasks.json");
            var loveExe = @"C:\Program Files\LOVE\love.exe";

            var json = new StringBuilder();
            json.AppendLine("{");
            json.AppendLine("  \"version\": \"2.0.0\",");
            json.AppendLine("  \"tasks\": [");
            json.AppendLine("    {");
            json.AppendLine("      \"label\": \"Run Love2D\",");
            json.AppendLine("      \"type\": \"process\",");
            json.AppendLine($"      \"command\": \"{EscapeJson(loveExe)}\",");
            json.AppendLine("      \"args\": [");
            json.AppendLine("        \"${workspaceFolder}\"");
            json.AppendLine("      ],");
            json.AppendLine("      \"group\": {");
            json.AppendLine("        \"kind\": \"build\",");
            json.AppendLine("        \"isDefault\": true");
            json.AppendLine("      },");
            json.AppendLine("      \"problemMatcher\": [],");
            json.AppendLine("      \"presentation\": {");
            json.AppendLine("        \"reveal\": \"silent\",");
            json.AppendLine("        \"panel\": \"dedicated\",");
            json.AppendLine("        \"clear\": true");
            json.AppendLine("      }");
            json.AppendLine("    }");
            json.AppendLine("  ]");
            json.AppendLine("}");

            File.WriteAllText(tasksPath, json.ToString());
        }

        private static void WriteWorkshopSettings()
        {
            var settingsPath = Path.Combine(UserDataDirectory, "User", "settings.json");
            File.WriteAllText(settingsPath,
                "{\n" +
                "  \"files.autoSave\": \"afterDelay\",\n" +
                "  \"editor.formatOnSave\": true,\n" +
                "  \"editor.formatOnPaste\": true,\n" +
                "  \"editor.formatOnType\": true,\n" +
                "  \"security.workspace.trust.enabled\": false\n" +
                "}\n");
        }

        private static string EscapeJson(string value)
        {
            return value.Replace("\\", "\\\\").Replace("\"", "\\\"");
        }
    }
}
