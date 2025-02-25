using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli;

public class CodeAction : IAction
{
    int currentIndex;

    public CodeAction( int currentIndex )
    {
        this.currentIndex = currentIndex;
    }

    public void Execute()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var steps = new List<Guide.Step>();

        var txtFilePath = Path.Combine( GuideCli.ResourcesPath,"session.txt" );

        if ( !File.Exists( txtFilePath ) )
        {
            return;
        }

        var session = JsonConvert.DeserializeObject<Session>( File.ReadAllText( txtFilePath ) );
        var username = session.Name;
        if(username != null) username = username.Replace(" ", "-");

        using ( var stream = assembly.GetManifestResourceStream( "workshop_cli.Guide.Steps.json" ) )
        using ( var reader = new StreamReader( stream ) )
        {
            var json = reader.ReadToEnd();
            steps = JsonConvert.DeserializeObject<List<Guide.Step>>( json );
        }

        var guide = new Guide { Steps = steps };

        var step = guide.Steps[currentIndex];
        var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "repoWorkshop");

        var folderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.ToString("dd-MM-yyyy")}", "mygame");
        
        var mdFilePath =$"{step.Id}.md";
        using var resourceStream = assembly.GetManifestResourceStream( $"workshop_cli.Guide.{mdFilePath}" );
        if ( resourceStream != null )
        {
            var mdFileContents = new StreamReader( resourceStream ).ReadToEnd();
            var mainLuaFilePath = Path.Combine( folderPath, "main.lua" );
            File.WriteAllText( mainLuaFilePath, mdFileContents );
            
        }
        else
        {
            Console.WriteLine( $"Could not find resource file: {mdFilePath}" );
        }

        
        
        var startFolderInfo = new ProcessStartInfo {
            FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/../Local/Programs/Microsoft VS Code/Code.exe",
            Arguments = $"\"{ folderPath }\" --disable-workspace-trust",
            WorkingDirectory = @"C:\",
            Verb = "runas",
            Environment =
            {
                { "VSCODE_LOG_LEVEL", "error" },
                { "VSCODE_DISABLE_REDACTED_EXTENSIONS", "true" }
            }
        };
        Process.Start(startFolderInfo);
        //Prompt.Confirm("Verifica o código e clica ENTER para continuar\n", false);
    }
}