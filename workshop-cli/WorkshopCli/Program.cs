using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using workshopCli;

class Program
{
    static void Main(string[] args)
    {
        // Add event handler for Console.CancelKeyPress
        Console.CancelKeyPress += Console_CancelKeyPress;

        var assembly = Assembly.GetExecutingAssembly();
        var steps = new List<Guide.Step>();

        using (var stream = assembly.GetManifestResourceStream("workshop_cli.Guide.Steps.json"))
        using (var reader = new StreamReader(stream))
        {
            var json = reader.ReadToEnd();
            steps = JsonConvert.DeserializeObject<List<Guide.Step>>(json);
        }

        var guide = new Guide { Steps = steps };
        var guideCli = new GuideCli(guide);
        var startAhkL = new ProcessStartInfo
        {
            FileName = Path.Combine(GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "AutoHotkeyU64.exe"),
            Arguments = Path.Combine(GuideCli.ResourcesPath, "win-left.ahk"),
            WorkingDirectory = @"C:\",
            Verb = "runas"
        };

        if (guideCli.verificaIndex == 5)
        {
            // Open VS Code
            var startFolderInfo = new ProcessStartInfo
            {
                FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/../Local/Programs/Microsoft VS Code/Code.exe",
                Arguments = "--disable-workspace-trust",
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            Process.Start(startFolderInfo);
        }

        Process.Start(startAhkL);
        guideCli.Run();
    }

    private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        // Prevent the application from closing when Ctrl+C is pressed
        e.Cancel = true;
    }
}