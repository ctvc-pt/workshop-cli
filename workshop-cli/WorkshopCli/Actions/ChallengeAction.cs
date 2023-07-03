using Sharprompt;

namespace workshopCli;

public class ChallengeAction : IAction
{
    public int Delay;
    private Timers timers;

    public ChallengeAction( int delay )
    {
        Delay = delay;
        timers = new Timers();
    }
    public void Execute()
    {
        Thread.Sleep(Delay);
        timers.StartTimer();
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm("Quando completares o desafio, escreve 'proximo' ou 'p' para avançar ou para retroceder escreve 'anterior' (ou 'ajuda')\n");
        timers.CancelTimer();
    }
}