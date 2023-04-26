using Sharprompt;

namespace workshopCli;

public class InformationAction : IAction
{
    
    public void Execute()
    {
        Prompt.Input<string>("Clica Enter para continuar");
    }
}