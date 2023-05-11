using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli;

public class GuideCli
{
    
    
    public static string ResourcesPath =
        Path.Combine( Environment.CurrentDirectory, "..", "..", "..", "..", "..", "Resources" );

    public Session session;
    private readonly Guide guide;
    CsvSessionWriter sessionWriter = new CsvSessionWriter();
    CsvHelpRequest helpRequest = new CsvHelpRequest();

    public GuideCli( Guide guide )
    {
        this.guide = guide;
        session = new Session();
    }

    public void Run()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var txtFilePath = Path.Combine( ResourcesPath,"session.txt" );

        if ( File.Exists( txtFilePath ) )
        {
            session = JsonConvert.DeserializeObject<Session>( File.ReadAllText( txtFilePath ) );
            Console.WriteLine( $"Bem-vindo outra vez {session.Name}!" );
        }
        else
        {
            Console.WriteLine( "Bem-vindo ao Workshop de Luv2D!" );
        }

        var startIndex = guide.Steps.FindIndex( step => step.Id == session.StepId );
        if ( startIndex == -1 ) // Step ID not found
        {
            startIndex = 0;
        }
        else
        {
            startIndex += 1;
        }

        int currentIndex = 0;

        for ( var i = startIndex; i < guide.Steps.Count; i++ )
        {
            var step = guide.Steps[ i ];
            Console.WriteLine( step.Message );
            session.StepId = step.Id;
            currentIndex = i;
            if ( step.Type != "code" )
            {
                var filePath = $"{step.Id}.md";
                var resourceStream = assembly.GetManifestResourceStream( $"workshop_cli.Guide.{filePath}" );
                {
                    if ( resourceStream != null )
                    {
                        using ( var reader = new StreamReader( resourceStream ) )
                        {
                            var fileContents = reader.ReadToEnd();
                            Console.WriteLine( fileContents );
                        }
                    }
                }
            }

            var actions = new Dictionary<string, IAction>()
            {
                { "information", new InformationAction() },
                { "challenge", new ChallengeAction() },
                { "exercise", new ExerciseAction() },
                { "install", new InstallAction() },
                { "open-file", new OpenFileL2DAction() },
                { "CreateSprites", new CreateSpritesAction() },
                { "code", new CodeAction( currentIndex ) },
                { "ask-name", new AskNameAction( this ) },
                { "ask-age", new AskAgeAction( this ) },
                { "ask-email", new AskEmailAction( this ) },
                { "video", new VideoAction( currentIndex ) }
            };

            if ( actions.TryGetValue( step.Type, out var action ) )
            {
                action.Execute();
            }
            else
            {
                Console.WriteLine( $"Unknown action type: {step.Type}" );
            }
            
            

            sessionWriter.AddSession( session.Name, session.Age, session.Email, session.StepId );
            helpRequest.GetHelp(session.Name,session.StepId);
            File.WriteAllText( txtFilePath, JsonConvert.SerializeObject( session ) );
            Console.Clear();
        }
        Thread.Sleep(2000);
        Environment.Exit(0);
    }
}