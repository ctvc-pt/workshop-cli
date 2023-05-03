using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli;

public class VideoAction: IAction
{
    int currentIndex;

    public VideoAction( int currentIndex )
    {
        this.currentIndex = currentIndex;
    }

    public void Execute()
    {
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
            Console.WriteLine( $"Playing video: {step.Message}" );
            // Call the method to play the video using the path in step.Message
            var path = Path.Combine( GuideCli.ResourcesPath, step.Message );
           
            Process.Start( $"{GuideCli.ResourcesPath}/VLCPortable/VLCPortable.exe",path );

        
        
        Prompt.Confirm("Verifica o código e clica ENTER para continuar\n", false);
    }
}