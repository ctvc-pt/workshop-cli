using Sharprompt;

namespace workshopCli;

public class ChallengeAction : IAction
{
    public int Delay;

    public ChallengeAction( int delay )
    {
        Delay = delay;
    }
    public void Execute()
    {
        Thread.Sleep(Delay);
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm("Quando completares o desafio, escreve 'proximo' ou 'p' para avançar (ou 'ajuda')\n");
    }
}