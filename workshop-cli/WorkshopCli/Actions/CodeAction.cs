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

        var txtFilePath = Path.Combine( Environment.CurrentDirectory, "..", "..", "..", "..", "..", "Resources",
            "session.txt" );

        if ( !File.Exists( txtFilePath ) )
        {
            return;
        }

        var session = JsonConvert.DeserializeObject<Session>( File.ReadAllText( txtFilePath ) );
        var username = session.Name;

        using ( var stream = assembly.GetManifestResourceStream( "workshop_cli.Guide.Steps.json" ) )
        using ( var reader = new StreamReader( stream ) )
        {
            var json = reader.ReadToEnd();
            steps = JsonConvert.DeserializeObject<List<Guide.Step>>( json );
        }

        var guide = new Guide { Steps = steps };

        var step = guide.Steps[currentIndex];
        var desktopPath = Environment.GetFolderPath( Environment.SpecialFolder.DesktopDirectory );
        var folderPath = Path.Combine( desktopPath, $"{username}_{DateTime.Now.Year}", "mygame" );
        
        var mdFilePath =$"{step.Id}.md";
        using var resourceStream = assembly.GetManifestResourceStream( $"workshop_cli.Guide.012-Teste-Lua.md" );
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
           
        Prompt.Confirm("Quando completares o desafio avança para a frente\n", false);
    }
}