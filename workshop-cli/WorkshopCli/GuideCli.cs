using Newtonsoft.Json;
using Sharprompt;

namespace workshop_cli;

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
        const string filePath = @"D:\CPDS\workshop-cli\workshop-cli\session.txt"; // session file
        var csv = "D:/CPDS/workshop-cli/workshop-cli/sessions.csv";

        Console.WriteLine("Welcome to the programming workshop!/n");

        if (File.Exists(filePath))
        {
            var jsonText = File.ReadAllText(filePath);

            session = JsonConvert.DeserializeObject<Session>(jsonText);

            Console.WriteLine($"Bem-vindo outra vez {session.Name}!");
        }
        
        foreach (var step in guide.Steps.Skip(session.StepId))
        {
            Console.WriteLine(step.Message);

            switch (step.Type)
            {
                // 
                case "question": // TODO: Can be improved
                    var answer = Prompt.Input<string>(":");
                    Console.WriteLine($"You entered: {answer}");
                    
                    switch (step.Id)
                    {
                        case "ask-name":
                            session.Name = answer;
                            break;
                        case "ask-age":
                            session.Age = answer;
                            break;
                        case "ask-email":
                            session.Email = answer;
                            break;
                    }

                    session.StepId++;

                    break;

                case "information":
                    Prompt.Confirm("Did you complete the task?", false);
                    Console.WriteLine("lets work!");
                    session.StepId++;
                    break;

                case "exercise":
                    while (true)
                    {
                        Prompt.Confirm("Did you complete the exercise?", false);
                        var input = Prompt.Input<string>("Type 'finish' to move to the next exercise", "");
                        if (input.ToLower() == "finish")
                        {
                            Console.WriteLine("Great job!");
                            session.StepId++;
                            break;
                        }
                    }

                    break;
            }

            var json = JsonConvert.SerializeObject(session);
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine(json);
            }

            Console.WriteLine();
        }

        Console.WriteLine("Congratulations, you have completed the workshop!");
    }
}