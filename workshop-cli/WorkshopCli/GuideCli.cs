﻿using System.Reflection;
using Newtonsoft.Json;

namespace workshopCli;

public class GuideCli
{
    private Session session;
    private readonly Guide guide;
    CsvSessionWriter sessionWriter = new CsvSessionWriter();

    public GuideCli( Guide guide )
    {
        this.guide = guide;
        session = new Session();
    }

    public void Run()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var txtFilePath = Path.Combine( Environment.CurrentDirectory, "..", "..", "..", "..", "..", "Resources",
            "session.txt" );


        if ( File.Exists( txtFilePath ) )
        {
            session = JsonConvert.DeserializeObject<Session>( File.ReadAllText( txtFilePath ) );
            Console.WriteLine( $"Bem-vindo outra vez {session.Name}!" );
        }
        else
        {
            Console.WriteLine( "Bem-vindo ao Workshop de Luv2D!" );
        }

        var actions = new Dictionary<string, IAction>()
        {
            { "ask-name", new AskNameAction( session ) },
            { "ask-age", new AskAgeAction( session ) },
            { "ask-email", new AskEmailAction( session ) },
            { "information", new InformationAction() },
            { "challenge", new ChallengeAction() },
            { "exercise", new ExerciseAction() },
            { "install", new InstallAction() },
            { "open-file", new OpenFileAction() },
        };


        var startIndex = guide.Steps.FindIndex( step => step.Id == session.StepId );
        if ( startIndex == -1 ) // Step ID not found
        {
            startIndex = 0;
        }

        for ( var i = startIndex; i < guide.Steps.Count; i++ )
            {
                var step = guide.Steps[ i ];
                Console.WriteLine( step.Message );
                session.StepId = step.Id;

                var filePath = $"{step.Id}.md";
                using ( var resourceStream = assembly.GetManifestResourceStream( $"workshop_cli.Guide.{filePath}" ) )
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

                if ( actions.TryGetValue( step.Type, out var action ) )
                {
                    action.Execute();
                }
                else
                {
                    Console.WriteLine( $"Unknown action type: {step.Type}" );
                }
                
                
                sessionWriter.AddSession( session.Name, session.Age, session.Email, session.StepId );
                File.WriteAllText( txtFilePath, JsonConvert.SerializeObject( session ) );
            }
        
    }
}

