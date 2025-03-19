using System;
using System.IO;
using Newtonsoft.Json;

namespace workshopCli
{
    public class AskParticipationAction : IAction
    {
        public GuideCli Cli;

        public AskParticipationAction(GuideCli cli)
        {
            Cli = cli;
        }

        public void Execute()
        {
            var txtFilePath = Path.Combine(GuideCli.ResourcesPath, "session.txt");
            
            if (Cli.session != null && Cli.session.StepId != null && !string.IsNullOrEmpty(Cli.session.StepId))
            {
                return; 
            }
            
            int cursorTop = Console.CursorTop;

            Console.WriteLine("Já participaste em algum dos nossos Workshops (sim ou nao)?\n");

            string resposta;
            bool respostaValida;

            do
            {
                resposta = ExerciseHelper.PromptAnswerAndPrint()?.Trim().ToLower()
                    .Replace("ã", "a")
                    .Replace("á", "a")
                    .Replace("õ", "o");

                respostaValida = resposta == "sim" || resposta == "nao";

                if (!respostaValida)
                {
                    Console.WriteLine("Resposta inválida. Por favor, responda 'sim' ou 'nao'.");
                }
            } while (!respostaValida);
            
            Console.SetCursorPosition(0, cursorTop);

            for (int i = 0; i < 3; i++) 
            {
                Console.Write(new string(' ', Console.WindowWidth)); 
                Console.SetCursorPosition(0, Console.CursorTop + 1); 
            }

            Console.SetCursorPosition(0, cursorTop); 

            Cli.session.Participation = resposta;
        }

    }
}