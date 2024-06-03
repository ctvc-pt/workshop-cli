using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;
using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli
{
    public class VideoAction : IAction
    {
        private int currentIndex;
        private string extensionId = "pixelbyte-studios.pixelbyte-love2d";

        public VideoAction(int currentIndex)
        {
            this.currentIndex = currentIndex;
        }

        public void Execute()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            var assembly = Assembly.GetExecutingAssembly();
            var steps = new List<Guide.Step>();

            using (var stream = assembly.GetManifestResourceStream("workshop_cli.Guide.Steps.json"))
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
                steps = JsonConvert.DeserializeObject<List<Guide.Step>>(json);
            }

            var guide = new Guide { Steps = steps };

            var step = guide.Steps[currentIndex];
            var path = Path.Combine(GuideCli.ResourcesPath, step.Message);

            var vlcProcess = new Process();
            vlcProcess.StartInfo.FileName = Path.Combine(GuideCli.ResourcesPath, "VLCPortable", "VLCPortable.exe");
            vlcProcess.StartInfo.Arguments = path;
            vlcProcess.EnableRaisingEvents = true;
            vlcProcess.Start();

            // Starts win-right.ahk process
            var startAhkR = new ProcessStartInfo
            {
                FileName = Path.Combine(GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "AutoHotkeyU64.exe"),
                Arguments = Path.Combine(GuideCli.ResourcesPath, "win-rigth.ahk"),
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            Process ahkProcess = Process.Start(startAhkR);

            Thread.Sleep(2000);

            Console.ForegroundColor = ConsoleColor.Yellow;

            var installer = new SoftwareInstaller();
            installer.InstallPython();

            // Install Git if not already installed
            if (!IsGitInstalled())
            {
                Console.WriteLine("--------33%-------");
                InstallGit();
            }
            else
            {
                Console.WriteLine("Git is already installed.");
            }

            // Install VS Code if not already installed
            if (!IsVSCodeInstalled())
            {
                Console.WriteLine("--------66%-------");
                InstallVSCode();
            }
            else
            {
                Console.WriteLine("Visual Studio Code is already installed.");
            }

            InstallVSCodeExtension(extensionId);

            Console.WriteLine("Quando o vídeo acabar, feche-o para continuar...");

            //CLI FOcus
            SetForegroundWindow(GetConsoleWindow());

            // Wait VLC to close
            vlcProcess.WaitForExit();

            // If VLC closed,close win-right.ahk
            if (!ahkProcess.HasExited)
            {
                ahkProcess.Kill();
            }
        }

        private bool IsVSCodeInstalled()
        {
            string installDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "../Local/Programs/Microsoft VS Code/Code.exe");
            return File.Exists(installDirectory);
        }

        private void InstallVSCode()
        {
            string installerPath = Path.Combine(GuideCli.ResourcesPath, "VSCodeSetup.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = installerPath,
                Arguments = "/verysilent /mergetasks=!runcode",
                WorkingDirectory = GuideCli.ResourcesPath,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process installProcess = Process.Start(startInfo);
            installProcess.WaitForExit();
        }

        public static void InstallVSCodeExtension(string extensionId)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd",
                Arguments = $"/c code --install-extension {extensionId}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            Process process = new Process
            {
                StartInfo = processStartInfo
            };
            process.Start();
            process.WaitForExit();
        }

        public static void InstallGit()
        {
            string installerPath = Path.Combine(GuideCli.ResourcesPath, "Git-2.45.0-64-bit.exe");

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = installerPath,
                Arguments = "/SILENT",
                WorkingDirectory = GuideCli.ResourcesPath,
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process gitInstaller = Process.Start(startInfo);
            gitInstaller.WaitForExit();
        }


        public static bool IsGitInstalled()
        {
            const string gitKey = @"SOFTWARE\GitForWindows";
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(gitKey))
            {
                return key != null;
            }
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
    }
}
