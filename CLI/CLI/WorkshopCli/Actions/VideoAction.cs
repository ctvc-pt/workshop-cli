using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace workshopCli
{
    public class VideoAction : IAction
    {
        private int currentIndex;
        private string extensionId = "pixelbyte-studios.pixelbyte-love2d";
        private GuideCli cli; 

        public VideoAction(int currentIndex, GuideCli cli)
        {
            this.currentIndex = currentIndex;
            this.cli = cli; 
        }

        public void Execute()
        {
            Console.ForegroundColor = ConsoleColor.Black;
            var step = cli.guide.Steps[currentIndex]; 
            var path = Path.Combine(GuideCli.ResourcesPath, step.Message);

            try
            {
                // Verificar se o arquivo de vídeo existe
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException($"Vídeo não encontrado em: {path}");
                }

                var vlcProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = Path.Combine(GuideCli.ResourcesPath, "VLCPortable", "VLCPortable.exe"),
                        Arguments = $"\"{path}\"", 
                        UseShellExecute = false,
                        RedirectStandardError = true 
                    },
                    EnableRaisingEvents = true
                };
                vlcProcess.Start();

                // Inicia o processo AutoHotkey
                var startAhkR = new ProcessStartInfo
                {
                    FileName = Path.Combine(GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "AutoHotkeyU64.exe"),
                    Arguments = Path.Combine(GuideCli.ResourcesPath, "win-rigth.ahk"),
                    WorkingDirectory = @"C:\",
                    Verb = "runas",
                    UseShellExecute = false
                };
                Process ahkProcess = Process.Start(startAhkR);

                Thread.Sleep(2000); 

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Quando o vídeo acabar, feche-o para continuar...");
                
                SetForegroundWindow(GetConsoleWindow());
                
                vlcProcess.WaitForExit();
                
                string error = vlcProcess.StandardError.ReadToEnd();
                if (!string.IsNullOrEmpty(error))
                {
                    Console.WriteLine($"Erro do VLC: {error}");
                }

                
                if (!ahkProcess.HasExited)
                {
                    ahkProcess.Kill();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Erro ao reproduzir o vídeo: {ex.Message}");
                if (ex is FileNotFoundException)
                {
                    Console.WriteLine("Verifique se o arquivo .mp4 está na pasta Resources.");
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
    }
}