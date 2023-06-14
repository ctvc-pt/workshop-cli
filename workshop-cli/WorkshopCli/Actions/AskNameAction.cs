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
        
        Guid guid = Guid.NewGuid();
        string guidString = guid.ToString();
        Cli.session.NameId = Cli.session.Name.ToLower().Replace(" ", "") + "-" + guidString.Substring(guidString.Length - 4);
        //Console.WriteLine(Cli.session.NameId);
        var repoManager = new GitHubManager();
        repoManager.CloneRepo();
        
    }
}