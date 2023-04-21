namespace workshopCli;

public class AskAgeAction : IAction
{
    private Session session;

    public AskAgeAction(Session session)
    {
        this.session = session;
    }
    public void Execute()
    {
        ExerciseHelper.PromptAnswerAndPrint("age", ref session.Age);
    }
}