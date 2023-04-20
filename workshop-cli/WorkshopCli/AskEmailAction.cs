namespace workshopCli;

public class AskEmailAction : IAction
{
    private Session session;

    public AskEmailAction(Session session)
    {
        this.session = session;
    }
    public void Execute()
    {
        ExerciseHelper.PromptAnswerAndPrint("email", ref session.Email);
    }
}