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
        Console.WriteLine("What is your name?");
        var input = Console.ReadLine();
        session.Name = input;
    }
}