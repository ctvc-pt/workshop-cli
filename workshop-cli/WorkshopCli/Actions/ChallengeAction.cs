using Sharprompt;

namespace workshopCli;

public class ChallengeAction : IAction
{
    public void Execute()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm("Quando completares o desafio escreve 'proximo' para avançar\n");
    }
}