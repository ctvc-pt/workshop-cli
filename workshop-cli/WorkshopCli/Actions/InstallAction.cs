using System.Diagnostics;

namespace workshopCli;

public class InstallAction : IAction
{

    public InstallAction()
    {
    }
    public void Execute()
    {
        var lovePath = Path.Combine( Environment.CurrentDirectory, "..", "..", "..", "..", "..", "Resources",
            "love-11.4-win64.exe" );
        Process loveProcess = new Process();
        loveProcess.StartInfo.FileName = lovePath;
        loveProcess.StartInfo.Arguments = $"\"{lovePath}\" /SILENT /NORESTART /SP-";
        loveProcess.StartInfo.Verb = "runas";
        loveProcess.Start();
        loveProcess.WaitForExit();
    }
}