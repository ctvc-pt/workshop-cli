using Sharprompt;
using System.Diagnostics;

namespace workshopCli;

public class InformationAction : IAction
{
    public GuideCli Cli;
    public int Delay;

    public InformationAction(GuideCli cli,int delay)
    {
        Cli = cli;
        Delay = delay;
    }
    public void Execute()
    {
        
        Thread.Sleep(Delay);
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm( "Escreve 'proximo' ou 'p' para avançar ou para retroceder escreve 'anterior' ou 'a'. Se precisares de ajuda escreve 'ajuda'. Se por alguma razão desejares voltar ao codigo anterior, escreve 'reset'" );
    }
}