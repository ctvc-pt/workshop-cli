using Sharprompt;

namespace workshopCli;

public class ChallengeAction : IAction
{
    public void Execute()
    {
        ExerciseHelper.PromptAnswerAndConfirm("Quando completares o desafio avança para a frente\n");
        //Prompt.Confirm("Quando completares o desafio avança para a frente\n", false);
    }
}