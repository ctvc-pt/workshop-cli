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
        ExerciseHelper.PromptAnswerAndConfirm( "Escreve 'proximo' ou 'p' para avancar. Para retroceder escreve 'anterior' ou 'a'. Para voltar ao codigo anterior deste desafio, escreve 'reset'." );
    }
}
