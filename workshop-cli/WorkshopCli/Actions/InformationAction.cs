using Sharprompt;

namespace workshopCli;

public class InformationAction : IAction
{
    
    public void Execute()
    {
        ExerciseHelper.PromptAnswerAndConfirm( "escreve 'proximo' para avançar" );
        
    }
}