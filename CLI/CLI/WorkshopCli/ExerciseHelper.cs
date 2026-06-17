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
        static Session currentSession;

        public static void SetCurrentSession( Session session )
        {
            currentSession = session;
        }

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

        public static bool PromptAnswerAndConfirm(string prompt)
        {
            var txtFilePath = Path.Combine(GuideCli.ResourcesPath, "session.txt");
            var session = currentSession ?? JsonConvert.DeserializeObject<Session>(File.ReadAllText(txtFilePath));

            while (true)
            {
                var wrappedString = GuideCli.WrapString(prompt, 50);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(wrappedString);
                Console.ResetColor();

                var answer = Prompt.Input<string>("Resposta ");

                if (string.IsNullOrWhiteSpace(answer))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    ClearLines(-1);
                    Console.WriteLine("Resposta inválida. Insere 'proximo' ou 'p'.");
                    Console.ResetColor();
                    continue;
                }

                switch ( answer.ToLower() )
                {
                    case "admin":
                        HandleAdmin();
                        break;
                    case "anterior" or "a":
                        GuideCli.adminInput = -1;
                        ClearConsole();
                        return true;
                    case "proximo" or "p":
                        processLuv.CloseLovecProcess();
                        ShowSpinnerAnimation().GetAwaiter().GetResult();
                        ClearConsole();
                        return true;
                    case "reset":
                        ResetToLastStep( session );
                        break;
                    case "s":
                        return false;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        ClearLines( -1 );
                        Console.WriteLine( "Resposta inválida. Insere 'proximo' ou 'p'." );
                        Console.ResetColor();
                        break;
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

        public static string GetStudentGameFolder( string username )
        {
            if ( !string.IsNullOrWhiteSpace( username ) )
            {
                username = username.Replace( " ", "-" );
            }

            var desktopPath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.DesktopDirectory ),
                "repoWorkshop" );

            return Path.Combine( desktopPath, $"{username}_{DateTime.Now.ToString( "dd-MM-yyyy" )}", "mygame" );
        }

        public static void SaveStepBackup( Session session )
        {
            if ( session == null || string.IsNullOrWhiteSpace( session.Name ) || string.IsNullOrWhiteSpace( session.StepId ) )
            {
                return;
            }

            try
            {
                var folderPath = GetStudentGameFolder( session.Name );
                var sourceFilePath = Path.Combine( folderPath, "main.lua" );

                if ( !File.Exists( sourceFilePath ) )
                {
                    return;
                }

                var backupFolder = Path.Combine( folderPath, ".workshop-backups" );
                Directory.CreateDirectory( backupFolder );

                var backupFilePath = Path.Combine( backupFolder, $"{session.StepId}.lua" );
                if ( !File.Exists( backupFilePath ) )
                {
                    File.Copy( sourceFilePath, backupFilePath );
                }
            }
            catch
            {
            }
        }

        static void ResetToLastStep( Session session )
        {
            try
            {
                var folderPath = GetStudentGameFolder( session?.Name );
                string destinationFilePath = Path.Combine( folderPath, "main.lua" );
                string sourceFilePath = Path.Combine( folderPath, ".workshop-backups", $"{session?.StepId}.lua" );

                if ( string.IsNullOrWhiteSpace( session?.StepId ) || !File.Exists( sourceFilePath ) )
                {
                    Console.WriteLine( "Ainda nao existe uma copia guardada deste desafio." );
                    Console.WriteLine( "Chama um professor antes de usar reset aqui." );
                    return;
                }

                File.Copy( sourceFilePath, destinationFilePath, true );

                Console.WriteLine( $"O codigo voltou ao inicio do desafio {session.StepId}." );
            }
            catch ( Exception ex )
            {
                Console.WriteLine( "Ocorreu um erro ao fazer reset: " + ex.Message );
            }
        }

        public static void ClearConsole()
        {
            Console.ResetColor();

            try
            {
                Console.Clear();
            }
            catch
            {
            }

            try
            {
                Console.SetCursorPosition(0, 0);
                if (Console.BufferHeight > Console.WindowHeight)
                {
                    Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
                }
            }
            catch
            {
            }

            Console.Write("\u001b[2J\u001b[3J\u001b[H");
            Console.Out.Flush();
        }

        static void ClearLines( int numberLines )
        {
            if (numberLines <= 0)
            {
                ClearConsole();
                return;
            }

            int currentLineCursor = Console.CursorTop;
            int startLine = Math.Max(0, currentLineCursor - numberLines);

            for ( int i = 0; i < numberLines; i++ )
            {
                Console.SetCursorPosition( 0, startLine + i );
                Console.Write( new string( ' ', Console.WindowWidth ) );
            }

            Console.SetCursorPosition( 0, startLine );
        }
        
        static async Task ShowSpinnerAnimation(int durationMs = 2000)
        {
            string[] frames = { "[ *   ]", "[  *  ]", "[   * ]", "[  *  ]" };
            int frameCount = frames.Length;
            int delay = 100;
            int iterations = durationMs / delay;

            ClearConsole();
            Console.CursorVisible = false;
            Console.SetCursorPosition(0, 0);

            for (int i = 0; i < iterations; i++)
            {
                string frame = $"Loading {frames[i % frameCount]}";
                Console.SetCursorPosition(0, 0);
                Console.Write(frame.PadRight(Console.WindowWidth));
                await Task.Delay(delay);
            }

            Console.SetCursorPosition(0, 0);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.CursorVisible = true;
            ClearConsole();
        }
    }
}
