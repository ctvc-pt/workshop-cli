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
            vsCodeProcess = GetProcessByName("Code");

            if (vsCodeProcess != null)
            {
                Console.WriteLine("Visual Studio Code is already open.");
            }
            else
            {
                var txtFilePath = Path.Combine(GuideCli.ResourcesPath, "session.txt");
                var session = Newtonsoft.Json.JsonConvert.DeserializeObject<Session>(File.ReadAllText(txtFilePath));
                var username = session.Name?.Replace(" ", "-");
                var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "repoWorkshop");
                var folderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now:dd-MM-yyyy}", "mygame");

                VSCodeLauncher.Open(folderPath);

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
            }
        }

        private static Process GetProcessByName(string processName)
        {
            Process[] processes = Process.GetProcessesByName(processName);
            foreach (Process process in processes)
            {
                try
                {
                    var fileName = Path.GetFileNameWithoutExtension(process.MainModule.FileName);
                    if (fileName.Equals(processName, StringComparison.OrdinalIgnoreCase))
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
