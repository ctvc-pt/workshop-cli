using Sharprompt;

namespace workshopCli;

public class ExerciseHelper
{
   
   
    public static string PromptAnswerAndPrint()
    {
        var sessionValue = Prompt.Input<string>("Resposta");
        Console.WriteLine($"Inseris-te: {sessionValue}");
        return sessionValue;
    }
    
    public static bool PromptAnswerAndConfirm(string prompt)
    {
        while (true)
        {
            var answer = Prompt.Input<string>($"{prompt} Resposta (escreve 'ajuda' para chamar alguém):");        
            if (answer.ToLower() == "ajuda")
            {
                CsvHelpRequest.printHelp(true);
                Console.WriteLine("Fizeste um pedido de ajuda, espera um bocado que alguém vem ter contigo\n");
                Console.WriteLine("ATENÇÃO: se saires desta mensagem o teu pedido de ajuda desaparece");
                Console.WriteLine("Escreve 'continuar' ou 'done' para continuar o workshop.");
                while (true)
                {
                    var input = Console.ReadLine().ToLower();
                    if (input == "continuar" || input == "done")
                    {
                        CsvHelpRequest.printHelp(false);
                        return PromptAnswerAndConfirm( prompt );
                    }
                    else
                    {
                        Console.WriteLine("Resposta inválida. Escreve 'continue' ou 'done'.");
                    }
                }
            }
            else
            {
                while (true)
                {
                    if (answer == "sim" || answer == "s" || answer == "yes" || answer == "y")
                    {
                        return true;
                    }
                    else if (answer == "nao" || answer == "n" || answer == "no" || answer == "não")
                    {
                        return false;
                    }
                    else
                    {
                        Console.WriteLine("Resposta inválida. Insere 'sim' ou 'não'.");
                        return false;
                    }
                }
            }
        }
    }

   
}
