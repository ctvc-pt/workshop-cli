namespace workshopCli;

public class AskTableAction: IAction
{
    public GuideCli Cli;
    public AskTableAction(GuideCli cli)
    {
        Cli = cli;
    }

    public void Execute()
    {
        Cli.session.Mesa = ExerciseHelper.PromptAnswerAndPrint();
        
        
    }
}