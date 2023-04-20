using Sharprompt;

namespace workshopCli;

public class ChallengeAction : IAction
{
    public void Execute()
    {
        Prompt.Confirm("Quando completares o desafio avança para a frente\n", false);
    }
}