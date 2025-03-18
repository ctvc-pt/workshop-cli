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

            // Verificar se há uma sessão salva com StepId
            if (Cli.session != null && Cli.session.StepId != null && !string.IsNullOrEmpty(Cli.session.StepId))
            {
                return; 
            }

            // Apenas exibe a pergunta se não houver StepId ou sessão
            Console.WriteLine("Já participaste em um Workshop (sim ou nao)?");
            while (true)
            {
                Cli.session.Participation = Console.ReadLine();

                string resposta = Cli.session.Participation?.Trim().ToLower().Replace("ã", "a").Replace("á", "a").Replace("õ", "o");

                if (resposta == "sim")
                {
                    break; 
                }
                else if (resposta == "nao")
                {
                    break; 
                }
                else
                {
                    Console.WriteLine("Resposta inválida. Por favor, responda 'sim' ou 'nao'.");
                }
            }
        }
    }
}