namespace workshopCli;

public class ExerciseAction : IAction
{
    public void Execute()
    {
        while (!ExerciseHelper.PromptAnswerAndConfirm("exercise"));
      
        Console.WriteLine("Great job!");
    }
}