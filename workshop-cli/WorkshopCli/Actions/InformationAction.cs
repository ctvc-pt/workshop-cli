using Sharprompt;

namespace workshopCli;

public class InformationAction : IAction
{
    
    public void Execute()
    {
        Prompt.Confirm("Quando completares a taréfa Avança para a frente\n", false);
    }
}