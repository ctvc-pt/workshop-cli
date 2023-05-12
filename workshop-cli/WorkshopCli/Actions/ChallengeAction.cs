using Sharprompt;

namespace workshopCli;

public class ChallengeAction : IAction
{
    public void Execute()
    {
        ExerciseHelper.PromptAnswerAndConfirm("Quando completares o desafio escreve 'proximo' para avançar\n");
        
    }
}