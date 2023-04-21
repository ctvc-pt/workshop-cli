namespace workshopCli;

public class AskNameAction : IAction
{
    private Session session;

    public AskNameAction(Session session)
    {
        this.session = session;
    }
    public void Execute()
    {
        ExerciseHelper.PromptAnswerAndPrint("name", ref session.Name);
    }
}