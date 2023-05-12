using Sharprompt;

namespace workshopCli;

public class InformationAction : IAction
{
    
    public void Execute()
    {
        ExerciseHelper.PromptAnswerAndConfirm( "Clica Enter para continuar" );
        //Prompt.Input<string>("Clica Enter para continuar");
    }
}