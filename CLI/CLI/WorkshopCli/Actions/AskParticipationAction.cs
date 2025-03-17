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
            while (true)
            {
                Cli.session.Participation = ExerciseHelper.PromptAnswerAndPrint();
                
                string resposta = Cli.session.Participation.Trim().ToLower()
                    .Replace("ã", "a").Replace("á", "a").Replace("õ", "o");
                
                if (resposta == "sim")
                {
                    GuideCli.adminInput = -2; 
                    break; 
                }
                else if (resposta == "nao")
                {
                    GuideCli.adminInput = 0;
                    break; 
                }
                else
                {
                    Console.WriteLine("Resposta inválida. Por favor, responda 'Sim' ou 'Nao'.");
                }
            }
        }
    }
}