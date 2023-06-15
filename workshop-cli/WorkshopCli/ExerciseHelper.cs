using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli;

public class ExerciseHelper
{
   
   
    public static string PromptAnswerAndPrint()
    {
        var sessionValue = Prompt.Input<string>("Resposta");
        Console.ForegroundColor = ConsoleColor.Yellow;
        return sessionValue;
    }
    
    public static bool PromptAnswerAndConfirm(string prompt)
    {
        var txtFilePath = Path.Combine( GuideCli.ResourcesPath,"session.txt" );
        var session = JsonConvert.DeserializeObject<Session>( File.ReadAllText( txtFilePath ) );
        
        var chatGptClient = new ChatGptClient();
        while (true)
        {
            string wrappedString = GuideCli.WrapString(prompt, 50);
            Console.WriteLine(wrappedString);
            var answer = Prompt.Input<string>("Resposta ");   
            if (answer == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Resposta inválida. Insere 'proximo' ou 'p'.");
                continue;
            }

            if ( answer.ToLower() == "ajuda" || answer.ToLower() == "a" )
            {
                CsvHelpRequest.printHelp( false, true );
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine( "Qual é o problema?" );
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write( $"{session.Name}: " ); //pergunta
                var userMessage = Console.ReadLine();
                var response = chatGptClient.AskGPT( userMessage ).Result; //resposta
                var typewriter = new TypewriterEffect(100); // Create an instance of TypewriterEffect with a delay of 100ms
                typewriter.Type(response, ConsoleColor.Cyan);
                //Console.WriteLine( $"Assistant: {response}" );

                Console.WriteLine( "\nconseguiste resolver? (sim ou não)" );
                while ( true )
                {
                    var input = Console.ReadLine().ToLower();
                    if ( input is "sim" or "s" )
                    {
                        CsvHelpRequest.printHelp( false, false );
                        return PromptAnswerAndConfirm( prompt );
                    }

                    if ( input is "não" or "nao" or "n" )
                    {
                        CsvHelpRequest.printHelp( true, false );
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine( "Fizeste um pedido de ajuda, espera um bocado que alguém vem ter contigo" );
                        Console.WriteLine( "ATENÇÃO: se saires desta mensagem o teu pedido de ajuda desaparece" );
                        Console.WriteLine( "Escreve 'continuar' ou 'done' para continuar o workshop." );
                        Console.ResetColor();

                        while ( true )
                        {
                            var inputHelp = Console.ReadLine().ToLower();
                            if ( inputHelp is "continuar" or "done" )
                            {
                                CsvHelpRequest.printHelp( false, false );
                                return PromptAnswerAndConfirm( prompt );
                            }
                            else
                            {
                                Console.WriteLine( "Resposta inválida. Escreve 'continuar' ou 'done'." );
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine( "Resposta inválida. Escreve 'sim' ou 'não'." );
                    }
                }
            }
            else if (answer.ToLower() is "admin")
            {
                Console.ForegroundColor = ConsoleColor.Black;
                var input = Console.ReadLine().ToLower();
                string[] inputs = new string[] { };
                
                if ( input.Contains( " " ) )
                {
                    inputs = input.Split( " " );
                }
                if ( inputs[0] is "id")
                {
                    GuideCli.adminInput = Int32.Parse(inputs[1]);
                }
                else
                {
                    Console.WriteLine( "Resposta inválida.");
                }
            }
            else
            { 
                if ( answer is "anterior" or "b" )
                {
                    GuideCli.adminInput = -1;
                } 
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