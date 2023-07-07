using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli;

public class VideoAction: IAction
{
    int currentIndex;
    string extensionId = "pixelbyte-studios.pixelbyte-love2d"; // Replace with the desired extension ID
    
    public VideoAction( int currentIndex )
    {
        this.currentIndex = currentIndex;
    }
    
    public void Execute()
    {
        Console.ForegroundColor = ConsoleColor.Black;
        var assembly = Assembly.GetExecutingAssembly();
        var steps = new List<Guide.Step>();

        using ( var stream = assembly.GetManifestResourceStream( "workshop_cli.Guide.Steps.json" ) )
        using ( var reader = new StreamReader( stream ) )
        {
            var json = reader.ReadToEnd();
            steps = JsonConvert.DeserializeObject<List<Guide.Step>>( json );
        }
        
        var guide = new Guide { Steps = steps };

        var step = guide.Steps[currentIndex];
            //Console.WriteLine( $"Playing video: {step.Message}" );
            // Call the method to play the video using the path in step.Message
            var path = Path.Combine( GuideCli.ResourcesPath, step.Message );
           
            var vlcProcess = new Process();
            vlcProcess.StartInfo.FileName = Path.Combine(GuideCli.ResourcesPath, "VLCPortable", "VLCPortable.exe");
            vlcProcess.StartInfo.Arguments = path;
            vlcProcess.EnableRaisingEvents = true;
            vlcProcess.Start();

        
            Thread.Sleep(2000); 
            var startAhkR = new ProcessStartInfo
            {
                FileName = Path.Combine( GuideCli.ResourcesPath,"AutoHotkey","v1.1.36.02","AutoHotkeyU64.exe"),
                Arguments = Path.Combine( GuideCli.ResourcesPath,"win-rigth.ahk"),
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            Process.Start(startAhkR);
            
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            var installer = new PythonInstaller();
            installer.InstallPython();
            
            if (!IsVSCodeInstalled())
            {
                Console.WriteLine("--------33%-------");
                InstallVSCode();
            }
            
            InstallVSCodeExtension(extensionId);
            
            if (!IsGitInstalled())
            {
                Console.WriteLine("--------66%-------");
                InstallGit();
            }
            
            Console.WriteLine("Quando o video acabar, fecha-o para continuar...");

            //dar focus na CLI
            [DllImport( "user32.dll" )]
            static extern bool SetForegroundWindow( IntPtr hWnd );
            [DllImport( "kernel32.dll" )]
            static extern IntPtr GetConsoleWindow();
            IntPtr consoleWindowHandle = GetConsoleWindow();
            SetForegroundWindow( consoleWindowHandle );
            
            //---------
            // Wait for the VLC process to exit
            vlcProcess.WaitForExit();

    }

    private bool IsVSCodeInstalled()
    {
        string installDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/../Local/Programs/Microsoft VS Code/Code.exe";
        return File.Exists(installDirectory);
    }

    private void InstallVSCode()
    {
        
        string installerPath = Path.Combine(GuideCli.ResourcesPath, "VSCodeSetup.exe");

        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = installerPath,
            Arguments = "/verysilent /mergetasks=!runcode",
            WorkingDirectory = GuideCli.ResourcesPath,
            CreateNoWindow = true,
            UseShellExecute = false
        };

        Process installProcess = Process.Start(startInfo);
        installProcess.WaitForExit();
        
    }
    
    public static void InstallVSCodeExtension(string extensionId)
    {
        ProcessStartInfo processStartInfo = new ProcessStartInfo
        {
            FileName = "cmd",
            Arguments = $"/c code --install-extension {extensionId}",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        Process process = new Process
        {
            StartInfo = processStartInfo
        };
        process.Start();
        process.WaitForExit();
    }

    public static void InstallGit()
    {
        string installerPath = Path.Combine(GuideCli.ResourcesPath, "Git-2.41.0-64-bit.exe");

        Process gitInstaller = new Process();
        gitInstaller.StartInfo.FileName = installerPath;
        gitInstaller.StartInfo.Arguments = "/SILENT";
        gitInstaller.Start();
        gitInstaller.WaitForExit();
    }

    public static bool IsGitInstalled()
    {
        Process gitProcess = new Process();
        gitProcess.StartInfo.FileName = "git";
        gitProcess.StartInfo.Arguments = "--version";
        gitProcess.StartInfo.RedirectStandardOutput = true;
        gitProcess.StartInfo.RedirectStandardError = true;
        gitProcess.StartInfo.UseShellExecute = false;
        gitProcess.StartInfo.CreateNoWindow = true;

        gitProcess.Start();
        gitProcess.WaitForExit();

        string output = gitProcess.StandardOutput.ReadToEnd();
        string error = gitProcess.StandardError.ReadToEnd();

        if (!string.IsNullOrEmpty(output))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}