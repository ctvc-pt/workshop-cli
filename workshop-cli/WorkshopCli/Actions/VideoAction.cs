using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Newtonsoft.Json;

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

            var vlcProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(GuideCli.ResourcesPath, "VLCPortable", "VLCPortable.exe"),
                    Arguments = path
                },
                EnableRaisingEvents = true
            };
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

            if (currentIndex == 0) // Only install software during the first video
            {
                var installer = new SoftwareInstaller();
                installer.InstallSoftware();
            }

            Console.WriteLine("Quando o vídeo acabar, feche-o para continuar...");

            //CLI Focus
            SetForegroundWindow(GetConsoleWindow());

            // Wait for VLC to close
            vlcProcess.WaitForExit();

            // If VLC closed, close win-right.ahk
            if (!ahkProcess.HasExited)
            {
                ahkProcess.Kill();
            }
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
    }
}
