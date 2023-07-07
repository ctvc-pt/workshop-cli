namespace workshopCli;

public class AskNameAction: IAction
{
    public GuideCli Cli;
    public AskNameAction(GuideCli cli)
    {
        Cli = cli;
    }

    public void Execute()
    {
        Cli.session.Name = ExerciseHelper.PromptAnswerAndPrint();
        
        
    }
}