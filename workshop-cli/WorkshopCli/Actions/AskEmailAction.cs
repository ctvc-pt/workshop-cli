namespace workshopCli;

public class AskEmailAction: IAction
{
    public GuideCli Cli;

    public AskEmailAction( GuideCli cli )
    {
        Cli = cli;
    }
    public void Execute()
    {
        Cli.session.Email = ExerciseHelper.PromptAnswerAndPrint();
    }
}
