using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using workshopCli;

Console.TreatControlCAsInput = true;
var assembly = Assembly.GetExecutingAssembly();
var steps = new List<Guide.Step>();

using ( var stream = assembly.GetManifestResourceStream( "workshop_cli.Guide.Steps.json" ) )
using ( var reader = new StreamReader( stream ) )
{
    var json = reader.ReadToEnd();
    steps = JsonConvert.DeserializeObject<List<Guide.Step>>( json );
}

var guide = new Guide { Steps = steps };
var guideCli = new GuideCli( guide );
var startAhkL = new ProcessStartInfo
{
    FileName = Path.Combine( GuideCli.ResourcesPath, "AutoHotkey", "v1.1.36.02", "AutoHotkeyU64.exe" ),
    Arguments = Path.Combine( GuideCli.ResourcesPath, "win-left.ahk" ),
    WorkingDirectory = @"C:\",
    Verb = "runas"
};

Process.Start( startAhkL );
var processes = Process.GetProcessesByName("Code");
foreach (var process in processes)
{
    process.Kill();
}

guideCli.Run();