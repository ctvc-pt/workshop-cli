using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace workshopCli
{
    public class OpenVSCode
    {
        private Process vsCodeProcess;
        private Process autoHotkeyProcess;

        public bool Open()
        {
            vsCodeProcess = Process.GetProcessesByName("Code").FirstOrDefault();

            if (vsCodeProcess != null)
            {
                Console.WriteLine("Visual Studio Code is already open.");
                return false;
            }
            else
            {
                // Start VS Code
                var pythonScriptPath = $"{GuideCli.ResourcesPath}/open_vscode.py"; // Script Path

                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "python",
                    Arguments = pythonScriptPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                vsCodeProcess = new Process { StartInfo = processStartInfo };
                vsCodeProcess.Start();

                // Wait VS Code to open
                Thread.Sleep(2000);

                // Start AutoHotKeys
                var ahkScriptPath = Path.Combine(GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "vsc.ahk");
                var ahkProcessStartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "AutoHotkeyU64.exe"),
                    Arguments = ahkScriptPath,
                    WorkingDirectory = @"C:\",
                    Verb = "runas"
                };
                autoHotkeyProcess = Process.Start(ahkProcessStartInfo);

                // Add event to close AutoHotKeys
                AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
                {
                    if (autoHotkeyProcess != null && !autoHotkeyProcess.HasExited)
                    {
                        Console.WriteLine("Closing AutoHotkey...");
                        autoHotkeyProcess.Kill();
                        autoHotkeyProcess.WaitForExit();
                        autoHotkeyProcess.Dispose();
                        Console.WriteLine("AutoHotkey has been closed.");
                    }
                };
                return true;
            }
        }

    }
}
