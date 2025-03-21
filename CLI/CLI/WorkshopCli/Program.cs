using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using workshopCli;

Console.TreatControlCAsInput = true;
var assembly = Assembly.GetExecutingAssembly();
var steps = new List<Guide.Step>();

using (var stream = assembly.GetManifestResourceStream("workshop_cli.Guide.Steps.json"))
{
    if (stream == null)
    {
        throw new FileNotFoundException("Resource 'workshop_cli.Guide.Steps.json' not found in the assembly. Check the resource names.");
    }
    using (var reader = new StreamReader(stream))
    {
        var json = reader.ReadToEnd();
        steps = JsonConvert.DeserializeObject<List<Guide.Step>>(json) ?? new List<Guide.Step>();
    }
}

var guide = new Guide(steps);
var guideCli = new GuideCli(guide);
var startAhkL = new ProcessStartInfo
{
    FileName = Path.Combine(GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "AutoHotkeyU64.exe"),
    Arguments = Path.Combine(GuideCli.ResourcesPath, "win-left.ahk"),
    WorkingDirectory = @"C:\",
    Verb = "runas"
};

Process ahkProcess = Process.Start(startAhkL);
ahkProcess.WaitForExit();

var processes = Process.GetProcessesByName("Code");
foreach (var process in processes)
{
    process.Kill();
}

ahkProcess.Kill();
ahkProcess.Dispose();

guideCli.Run();