namespace workshopCli;

public class AskAgeAction: IAction
{
    public GuideCli Cli;

    public AskAgeAction( GuideCli cli )
    {
        Cli = cli;
    }
    public void Execute()
    {
        Cli.session.Age = ExerciseHelper.PromptAnswerAndPrint();
    }
}