using Sharprompt;

namespace workshopCli;

public class InformationAction : IAction
{
    public GuideCli Cli;
    
    public InformationAction(GuideCli cli)
    {
        Cli = cli;
    }
    public void Execute()
    {
        Thread.Sleep(2000);
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm( "Escreve 'proximo' ou 'p' para avançar (ou 'ajuda')" );
    }
}