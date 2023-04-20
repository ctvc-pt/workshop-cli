using Newtonsoft.Json;
using Sharprompt;


namespace workshopCli;

public class GuideCli
{
    private Session session;
    private readonly Guide guide;

    public GuideCli(Guide guide)
    {
        this.guide = guide;
        session = new Session();
    }

    public void Run()
    {
        var txtFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "session.txt");
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
                    Prompt.Confirm("Did you complete the task?", false);
                    Console.WriteLine("lets work!");
                    break;

                case "exercise":
                    while (!PromptAnswerAndConfirm("exercise")) ;
                    Console.WriteLine("Great job!");
                    break;
            }

            AddSessionToCsv(session.Name, session.Age, session.Email, session.StepId);
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

    public void AddSessionToCsv(string name, string age, string email, int stepId)
    {
        var csvFilePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "sessions.csv");
        var lines = File.ReadAllLines(csvFilePath).ToList();

        for (var i = 0; i < lines.Count; i++)
        {
            var values = lines[i].Split(';');
            if (values[0] != name) continue;
            values[1] = age;
            values[2] = email;
            values[3] = stepId.ToString();
            lines[i] = string.Join(";", values);
            File.WriteAllLines(csvFilePath, lines);
            return;
        }

        lines.Add($"{name};{age};{email};{stepId}");
        File.WriteAllLines(csvFilePath, lines);
    }
}