using Sharprompt;

namespace workshopCli;

public class ChallengeAction : IAction
{
    public void Execute()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm("Quando completares o desafio, escreve 'proximo' ou 'p' para avançar (ou 'ajuda')\n");
    }
}