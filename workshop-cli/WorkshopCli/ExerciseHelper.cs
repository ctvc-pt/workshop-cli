using Sharprompt;

namespace workshopCli;

public class ExerciseHelper
{
    public static void PromptAnswerAndPrint(string prompt, ref string sessionValue)
    {
        sessionValue = Prompt.Input<string>(prompt + ":");
        Console.WriteLine($"You entered: {sessionValue}");
    }
    
    public static bool PromptAnswerAndConfirm(string prompt)
    {
        Console.Write(prompt);
        string answer = Console.ReadLine();
        Console.WriteLine($"Your answer was: {answer}. Is this correct?");
        while (true)
        {
            Console.Write("Enter 'yes' or 'no': ");
            string confirmation = Console.ReadLine().ToLower();
            if (confirmation == "yes")
            {
                return true;
            }
            else if (confirmation == "no")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
            }
        }
    }
}
