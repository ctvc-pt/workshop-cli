using Sharprompt;

namespace workshop_cli;

public class GuideCli
{
    private readonly Guide guide;
    
    public GuideCli(Guide guide)
    {
        this.guide = guide;
    }

    public void Run()
    {
        Console.WriteLine("Welcome to the programming workshop!");
        Console.WriteLine();

        foreach (var step in guide.Steps)
        {
            Console.WriteLine(step.Message);

            switch (step.Type)
            {
                case "question":
                    var answer = Prompt.Input<string>(":");
                    Console.WriteLine($"You entered: {answer}");
                    break;
                
                case "information":
                    Prompt.Confirm("Did you complete the task?", false);
                    Console.WriteLine("lets work!");
                    break;

              
                case "exercise":
                    while (true)
                    {
                        Prompt.Confirm("Did you complete the exercise?", false);
                        var input = Prompt.Input<string>("Type 'finish' to move to the next exercise", "");
                        if (input.ToLower() == "finish")
                        {
                            Console.WriteLine("Great job!");
                            break;
                        }
                    }

                    break;
            }

            Console.WriteLine();
        }

        Console.WriteLine("Congratulations, you have completed the workshop!");
    }
}