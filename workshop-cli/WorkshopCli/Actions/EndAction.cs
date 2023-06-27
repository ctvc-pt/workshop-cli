using System.Diagnostics;

namespace workshopCli;

public class EndAction : IAction
{
    public void Execute()
    {
        var ahkProcesses = Process.GetProcessesByName("AutoHotkey");
        if (ahkProcesses.Length > 0)
        {
            // Kill all running AutoHotkey processes
            foreach (var process in ahkProcesses)
            {
                process.Kill();
            }
        }
    }
}