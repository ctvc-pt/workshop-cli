using Newtonsoft.Json;
using Sharprompt;
using System;
using System.Diagnostics;
using System.IO;

namespace workshopCli
{
    public class ExerciseHelper
    {
        static ProcessLuv processLuv = new ProcessLuv();

        public static string PromptAnswerAndPrint()
        {
            string sessionValue = null;

            while ( sessionValue == null )
            {
                sessionValue = Prompt.Input<string>( "Resposta" );

                if ( string.IsNullOrWhiteSpace( sessionValue ) )
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    ClearLines( 5 );
                    Console.WriteLine( "A resposta não pode estar vazia" );
                    Console.ResetColor();
                    sessionValue = null;
                }
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            return sessionValue;
        }

        public static bool PromptAnswerAndConfirm( string prompt )
        {
            var txtFilePath = Path.Combine( GuideCli.ResourcesPath, "session.txt" );
            var session = JsonConvert.DeserializeObject<Session>( File.ReadAllText( txtFilePath ) );

            var chatGptClient = new ChatGptClient();
            while ( true )
            {
                var wrappedString = GuideCli.WrapString( prompt, 50 );
                Console.ForegroundColor = ConsoleColor.Yellow; // Set the text color to yellow for the entire prompt
                Console.WriteLine( wrappedString );
                Console.ResetColor(); // Reset text color to default

                var answer = Prompt.Input<string>( "Resposta " );

                if ( string.IsNullOrWhiteSpace( answer ) )
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    ClearLines( 6 );
                    Console.WriteLine( "Resposta inválida. Insere 'proximo' ou 'p'." );
                    Console.ResetColor();
                    continue;
                }

                switch ( answer.ToLower() )
                {
                    case "ajuda" or "h":
                        HandleHelp( session, chatGptClient );
                        break;
                    case "admin":
                        HandleAdmin();
                        break;
                    case "anterior" or "a":
                        GuideCli.adminInput = -1;
                        return true;
                    case "proximo" or "p":
                        processLuv.CloseLovecProcess();
                        return true;
                    case "reset":
                        ResetToLastStep( session.Name );
                        break;
                    case "s":
                        return false;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        ClearLines( 6 );
                        Console.WriteLine( "Resposta inválida. Insere 'proximo' ou 'p'." );
                        Console.ResetColor();
                        break;
                }
            }
        }

        static void HandleHelp( Session session, ChatGptClient chatGptClient )
        {
            try
            {
                CsvHelpRequest.printHelp( false, true );
            }
            catch ( Exception e )
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine( e );
            }

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine( "Qual é o problema?" );
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write( $"{session.Name}: " );
            var userMessage = Prompt.Input<string>( "" );
            var urgent = false;
            var GPTfailed = false;

            try
            {
                var response = chatGptClient.AskGPT( userMessage, GuideCli.stepMessage ).Result;
                var typewriter = new TypewriterEffect( 50 );
                typewriter.Type( response, ConsoleColor.Cyan );
            }
            catch ( Exception ex )
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine( "Não foi possível pedir ajuda" );
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                GPTfailed = true;
            }

            if ( !GPTfailed )
            {
                Console.WriteLine( "\nConseguiste resolver? (sim ou não)" );
                var input = Prompt.Input<string>( "" ).ToLower();

                if ( input == "sim" || input == "s" )
                {
                    try
                    {
                        CsvHelpRequest.printHelp( true, false );
                    }
                    catch ( Exception e )
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine( e );
                    }
                }
                else if ( input == "não" || input == "nao" || input == "n" )
                {
                    HandleHelpRequest();
                }
                else
                {
                    Console.WriteLine( "Resposta inválida. Escreve 'sim' ou 'não'." );
                }
            }
            else
            {
                HandleHelpRequest();
            }
        }

        static void HandleHelpRequest()
        {
            try
            {
                CsvHelpRequest.printHelp( true, false );
            }
            catch ( Exception e )
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine( e );
            }

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine( "Fizeste um pedido de ajuda, espera por um professor." );
            Console.WriteLine( "ATENÇÃO: se saires desta mensagem o teu pedido de ajuda desaparece" );
            Console.WriteLine( "Escreve 'continuar' ou 'done' para continuar o workshop." );
            Console.ResetColor();
            var urgent = true;

            while ( urgent )
            {
                var inputHelp = Prompt.Input<string>( "" ).ToLower();
                if ( inputHelp == "continuar" || inputHelp == "done" )
                {
                    try
                    {
                        CsvHelpRequest.printHelp( true, false );
                    }
                    catch ( Exception e )
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine( e );
                    }

                    urgent = false;
                }
                else
                {
                    Console.WriteLine( "Resposta inválida. Escreve 'continuar' ou 'done'." );
                }
            }
        }

        static void HandleAdmin()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            var input = Prompt.Input<string>( "" ).ToLower();
            var inputs = input.Split( " " );

            if ( inputs.Length > 1 && inputs[ 0 ] == "id" )
            {
                GuideCli.adminInput = int.Parse( inputs[ 1 ] );
            }
            else
            {
                Console.WriteLine( "Resposta inválida." );
            }
        }

        static void ResetToLastStep( string username )
        {
            var desktopPath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.DesktopDirectory ),
                "repoWorkshop" );
            var folderPath = Path.Combine( desktopPath, $"{username}_{DateTime.Now.ToString( "dd-MM-yyyy" )}",
                "mygame" );
            string destinationFilePath = Path.Combine( folderPath, "main.lua" );
            string sourceFilePath = $"{GuideCli.ResourcesPath}/backup.lua";

            try
            {
                string content = File.ReadAllText( sourceFilePath );

                File.WriteAllText( destinationFilePath, content );

                Console.WriteLine( "O código foi restaurado." );
            }
            catch ( Exception ex )
            {
                Console.WriteLine( "Ocorreu um erro ao fazer backup: " + ex.Message );
            }
        }

        static void ClearLines( int numberLines )
        {
            int currentLineCursor = Console.CursorTop;
            int startLine = currentLineCursor - numberLines;

            for ( int i = 0; i < numberLines; i++ )
            {
                Console.SetCursorPosition( 0, startLine + i );
                Console.Write( new string( ' ', Console.WindowWidth ) );
            }

            Console.SetCursorPosition( 0, startLine );
        }
    }
}