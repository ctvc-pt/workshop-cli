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
        var txtFilePath = Path.Combine(AppContext.BaseDirectory,"Resources", "session.txt");
        Console.WriteLine("Bem-vindo!/n");

        if (File.Exists(txtFilePath))
        {
            session = JsonConvert.DeserializeObject<Session>(File.ReadAllText(txtFilePath));
            Console.WriteLine($"Bem-vindo outra vez {session.Name}!");
        }

        foreach (var step in guide.Steps.Skip(session.StepId))
        {
            Console.WriteLine(step.Message);
            session.StepId++;

            var filePath =  Path.Combine(AppContext.BaseDirectory,"Resources","Guide",$"{step.Id}.md");
            if (File.Exists(filePath))
            {
                string fileContents = File.ReadAllText(filePath);
                Console.WriteLine(fileContents);
            }
            

            switch (step.Type)
            {
                case "ask-name":
                    PromptAnswerAndPrint("name", ref session.Name);
                    break;

                case "ask-age":
                    PromptAnswerAndPrint("age", ref session.Age);
                    break;

                case "ask-email":
                    PromptAnswerAndPrint("email", ref session.Email);
                    break;

                case "information":
                    Prompt.Confirm("Quando completares a taréfa Avança para a frente", false);
                    Console.WriteLine("");
                    break;

                case "exercise":
                    while (!PromptAnswerAndConfirm("exercise")) ;
                    Console.WriteLine("Great job!");
                    break;
            }

            sessionWriter.AddSession(session.Name, session.Age, session.Email, session.StepId);
            File.WriteAllText(txtFilePath, JsonConvert.SerializeObject(session));
        }
    }

    private static bool PromptAnswerAndConfirm(string prompt)
    {
        Prompt.Confirm("Did you complete the " + prompt + "?", false);
        return Prompt.Input<string>("Type 'finish' to move to the next exercise", "").ToLower() == "finish";
    }

    private static void PromptAnswerAndPrint(string prompt, ref string sessionValue)
    {
        sessionValue = Prompt.Input<string>(prompt + ":");
        Console.WriteLine($"You entered: {sessionValue}");
    }
}