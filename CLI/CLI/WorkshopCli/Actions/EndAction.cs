using System.Diagnostics;

namespace workshopCli;

public class EndAction : IAction
{
    public void Execute()
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = "cmd",
            Arguments = $"/c start https://docs.google.com/forms/d/e/1FAIpQLSfvFGl453XXBXot80CdAKoo6yCp0JbwuS1qmlgiRC3p1ebnfw/viewform?usp=sf_link",
            WindowStyle = ProcessWindowStyle.Hidden
        });
        
        
        var exePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "AutoHotkey", "v1.1.36.02", "AutoHotkeyU64.exe");
        var processName = Path.GetFileNameWithoutExtension(exePath);
        var processes = Process.GetProcessesByName(processName);

        foreach (var process in processes)
        {
            process.Kill();
        }

    }
}