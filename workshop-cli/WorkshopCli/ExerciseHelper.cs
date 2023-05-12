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
            if (answer == null)
            {
                Console.WriteLine("Resposta inválida. Insere 'proximo' ou 'p'.");
                continue;
            }
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
                        Console.WriteLine("Resposta inválida. Escreve 'continuar' ou 'done'.");
                    }
                }
            }
            else
            { 
                if (answer == "proximo" || answer == "p" )
                {
                    return true;
                }
                else if (answer == "s")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Resposta inválida. Insere 'proximo' ou 'p'.");
                }
            }
        }
    }

   
}
