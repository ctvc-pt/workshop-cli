using System.Diagnostics;
using System.Net;

namespace workshopCli;

public class InstallAction : IAction
{

    public InstallAction()
    {
    }
    public void Execute()
    {
        Process[] processes = Process.GetProcessesByName("vlc");

        if (processes.Length > 0)
        {
            foreach (Process process in processes)
            {
                process.CloseMainWindow();
                process.WaitForExit();
            }
            Console.WriteLine("VLC media player closed successfully.");
        }
        else
        {
            Console.WriteLine("No VLC media player process found.");
        }
        
        var exePath = Path.Combine( "C:\\Program Files\\LOVE\\love.exe" );
        var lovePath = Path.Combine( GuideCli.ResourcesPath,"love-11.4-win64.exe" );
        Process loveProcess = new Process();
        loveProcess.StartInfo.FileName = lovePath;
        loveProcess.StartInfo.Arguments = $"\"{lovePath}\" /SILENT /NORESTART /SP-";
        loveProcess.StartInfo.Verb = "runas";
        loveProcess.Start();
        loveProcess.WaitForExit();
        if ( !File.Exists(exePath) )
        {
            Execute();
        }
    }
}