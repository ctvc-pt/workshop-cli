using Sharprompt;

namespace workshopCli;

public class ExerciseHelper
{
    public static void PromptAnswerAndPrint(string prompt, ref string sessionValue)
    {
        sessionValue = Prompt.Input<string>(prompt + ":");
        Console.WriteLine($"Inseris-te: {sessionValue}");
    }
    
    public static bool PromptAnswerAndConfirm(string prompt)
    {
        Console.Write(prompt);
        string answer = Console.ReadLine();
        Console.WriteLine($"A tua resposta foi: {answer}. Está correto?");
        while (true)
        {
            Console.Write("Insere 'sim' ou 'não': ");
            string confirmation = Console.ReadLine().ToLower();
            if (confirmation == "sim" || confirmation == "s" || confirmation == "yes" || confirmation == "y" )
            {
                return true;
            }
            else if (confirmation == "nao" || confirmation == "n" || confirmation == "no" || confirmation == "não" )
            {
                return false;
            }
            else
            {
                Console.WriteLine("Resposta inválida. Insere 'sim' ou 'não'.");
            }
        }
    }
}
