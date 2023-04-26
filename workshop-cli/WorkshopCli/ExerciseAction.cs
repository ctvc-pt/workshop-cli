namespace workshopCli;

public class ExerciseAction : IAction
{
    public void Execute()
    {
        while (!ExerciseHelper.PromptAnswerAndConfirm("Responde"));
      
        Console.WriteLine("Bom trabalho!");
    }
}