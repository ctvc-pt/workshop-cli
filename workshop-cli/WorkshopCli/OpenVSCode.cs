using System.Diagnostics;

namespace workshopCli;

public class OpenVSCode
{
    public void Open()
    {
        string vsCodeExecutable = "code.exe";
            Process vsCodeProcess = GetProcessByName(vsCodeExecutable);
                
                if (vsCodeProcess != null)
            {
                Console.WriteLine("Visual Studio Code is open.");
            }
            else
            {
                var startFolderInfo = new ProcessStartInfo
                {
                    FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/../Local/Programs/Microsoft VS Code/Code.exe",
                    Arguments = "--disable-workspace-trust",
                    WorkingDirectory = @"C:\", 
                    Verb = "runas"
                };
                Process.Start(startFolderInfo);
                
                Thread.Sleep( 2000 );
                var startAhkR = new ProcessStartInfo
                {
                    FileName = Path.Combine( GuideCli.ResourcesPath,"AutoHotkey","v1.1.36.02","AutoHotkeyU64.exe"),
                    Arguments = Path.Combine( GuideCli.ResourcesPath,"win-rigth.ahk"),
                    WorkingDirectory = @"C:\",
                    Verb = "runas"
                };
                Process.Start(startAhkR);
            }
    }
    static Process GetProcessByName(string processName)
    {
        Process[] processes = Process.GetProcesses();
        
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