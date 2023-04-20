using System.Reflection;
using Newtonsoft.Json;
using Sharprompt;


namespace workshopCli;

public class GuideCli
{
    private Session session;
    private readonly Guide guide;
    CsvSessionWriter sessionWriter = new CsvSessionWriter();

    public GuideCli(Guide guide)
    {
        this.guide = guide;
        session = new Session();
    }

    public void Run()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var txtFilePath = Path.Combine( Environment.CurrentDirectory, "..", "..", "..", "..", "..", "Resources",
            "session.txt" );
        Console.WriteLine( "Bem-vindo!/n" );

        if ( File.Exists( txtFilePath ) )
        {
            session = JsonConvert.DeserializeObject<Session>( File.ReadAllText( txtFilePath ) );
            Console.WriteLine( $"Bem-vindo outra vez {session.Name}!" );
        }

        var actions = new Dictionary<string, IAction>()
        {
            { "ask-name", new AskNameAction(session) },
            { "ask-age", new AskAgeAction(session) },
            { "ask-email", new AskEmailAction(session) },
            { "information", new InformationAction() },
            { "challenge", new ChallengeAction() },
            { "exercise", new ExerciseAction() }
        };

        foreach ( var step in guide.Steps.Skip( session.StepId ) )
        {
            Console.WriteLine( step.Message );
            session.StepId++;

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

