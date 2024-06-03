using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace workshopCli
{
    public class OpenVSCode
    {
        private Process vsCodeProcess;
        private Process autoHotkeyProcess;

        public void Open()
        {
            string vsCodeExecutable = "code.exe";
            vsCodeProcess = GetProcessByName(vsCodeExecutable);

            if (vsCodeProcess != null)
            {
                Console.WriteLine("Visual Studio Code is already open.");
            }
            else
            {
                // Iniciar o VS Code
                var pythonScriptPath = $"{GuideCli.ResourcesPath}/open_vscode.py";

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

                Thread.Sleep(2000);

                // Iniciar o AutoHotkey
                var ahkScriptPath = Path.Combine(GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "vsc.ahk");
                var ahkProcessStartInfo = new ProcessStartInfo
                {
                    FileName = Path.Combine(GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "AutoHotkeyU64.exe"),
                    Arguments = ahkScriptPath,
                    WorkingDirectory = @"C:\",
                    Verb = "runas"
                };
                autoHotkeyProcess = Process.Start(ahkProcessStartInfo);

                // Add event ot close AutoHotKeys
                AppDomain.CurrentDomain.ProcessExit += (sender, e) =>
                {
                    if (autoHotkeyProcess != null && !autoHotkeyProcess.HasExited)
                    {
                        Console.WriteLine("Closing AutoHotkey...");
                        autoHotkeyProcess.Kill();
                        autoHotkeyProcess.WaitForExit();
                        Console.WriteLine("AutoHotkey has been closed.");
                    }
                };
            }
        }

        private static Process GetProcessByName(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                try
                {
                    if (process.MainModule.FileName.EndsWith(processName, StringComparison.OrdinalIgnoreCase))
                    {
                        return process;
                    }
                }
                catch (Exception)
                {
                    // Ignore any process that throws an exception when accessing MainModule
                }
            }
            return null;
        }
    }
}
