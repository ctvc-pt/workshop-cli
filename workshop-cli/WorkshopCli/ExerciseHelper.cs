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
        var chatGptClient = new ChatGptClient();
        while (true)
        {
            var answer = Prompt.Input<string>($"{prompt} Resposta (escreve 'ajuda' para chamar alguém):");   
            if (answer == null)
            {
                Console.WriteLine("Resposta inválida. Insere 'proximo' ou 'p'.");
                continue;
            }

            if ( answer.ToLower() == "ajuda" )
            {
                CsvHelpRequest.printHelp( false, true );
                Console.WriteLine( "Qual é o problema?" );

                Console.Write( "User: " ); //pergunra
                var userMessage = Console.ReadLine();
                var response = chatGptClient.AskGPT( userMessage ).Result; //resposta
                Console.WriteLine( $"Assistant: {response}" );

                Console.WriteLine( "conseguiste resolver? (sim ou não)" );
                var input = Console.ReadLine().ToLower();
                if ( input is "sim" or "s")
                {
                    CsvHelpRequest.printHelp( false, false );
                    return PromptAnswerAndConfirm( prompt );
                }

                if ( input is "não" or "nao" or "n" )
                {
                    CsvHelpRequest.printHelp(true,false);
                    Console.WriteLine( "Fizeste um pedido de ajuda, espera um bocado que alguém vem ter contigo\n" );
                    Console.WriteLine( "ATENÇÃO: se saires desta mensagem o teu pedido de ajuda desaparece" );
                    Console.WriteLine( "Escreve 'continuar' ou 'done' para continuar o workshop." );
                    while (true)
                    {
                        var inputHelp = Console.ReadLine().ToLower();
                        if (inputHelp is "continuar" or "done")
                        {
                            CsvHelpRequest.printHelp(false,false);
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
                    Console.WriteLine( "Resposta inválida. Escreve 'continuar' ou 'done'." );
                }
            }
            else
            { 
                if (answer is "proximo" or "p" )
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