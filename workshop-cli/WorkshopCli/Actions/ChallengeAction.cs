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
        int totalSeconds = Delay;
        for (int i = totalSeconds; i > 0; i--)
        {
            //Console.Write($"\rTime remaining: {i} seconds");
            Console.Out.Flush();
            Thread.Sleep(1000);
            KeyPress.SimulateKeyPress();
        }
        KeyPress.SimulateKeyPress();
        //Thread.Sleep(Delay);
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm("Quando completares o desafio, escreve 'proximo' ou 'p' para avançar ou para retroceder escreve 'anterior' (ou 'ajuda')\n");
    }
}