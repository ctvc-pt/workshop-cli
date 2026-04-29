using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using workshopCli;

Console.TreatControlCAsInput = true;

// Try to grow the console to a comfortable width for the guide markdown.
// This works on the legacy Windows console but throws on Windows Terminal —
// in that case we silently fall back to dynamic re-wrapping (HtmlConsoleRenderer
// reads Console.WindowWidth at render time). If the window is still too narrow
// we ask the kid to maximize before continuing.
try
{
    if (Console.WindowWidth < 100)
    {
        Console.SetWindowSize(Math.Min(110, Console.LargestWindowWidth), Math.Max(Console.WindowHeight, 30));
        Console.SetBufferSize(Math.Max(Console.BufferWidth, 110), Console.BufferHeight);
    }
}
catch
{
    // Windows Terminal can't be resized programmatically — ignore.
}

while (Console.WindowWidth < 80)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine();
    Console.WriteLine("A janela é demasiado estreita para ler o guia.");
    Console.WriteLine("Maximiza a janela e carrega ENTER para continuar.");
    Console.ResetColor();
    Console.ReadLine();
}

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

// Watch for terminal resizes and reflow the current step. Without this, what
// was already written to the screen buffer keeps the old wrap and the new
// text wraps to the new width, producing a chaotic mix.
_ = System.Threading.Tasks.Task.Run(() =>
{
    int lastWidth = SafeWidth();
    int lastHeight = SafeHeight();
    while (true)
    {
        System.Threading.Thread.Sleep(200);
        int w = SafeWidth();
        int h = SafeHeight();
        if (w == lastWidth && h == lastHeight) continue;
        lastWidth = w;
        lastHeight = h;
        try
        {
            lock (RenderState.RenderLock)
            {
                if (string.IsNullOrEmpty(RenderState.CurrentMarkdown)) continue;
                Console.Clear();
                HtmlConsoleRenderer.RenderInternal(RenderState.CurrentMarkdown);
                if (!string.IsNullOrEmpty(RenderState.CurrentTrailingMessage))
                {
                    Console.ForegroundColor = RenderState.CurrentTrailingColor;
                    Console.WriteLine(RenderState.CurrentTrailingMessage);
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        catch
        {
            // The console can throw if the user is dragging the resize edge —
            // try again on the next tick.
        }
    }
});

static int SafeWidth()
{
    try { return Console.WindowWidth; } catch { return 0; }
}
static int SafeHeight()
{
    try { return Console.WindowHeight; } catch { return 0; }
}
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