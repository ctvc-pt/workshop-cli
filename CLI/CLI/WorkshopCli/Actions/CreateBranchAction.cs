namespace workshopCli;

public class CreateBranchAction : IAction
{
    public void Execute()
    {

        var repoManager = new GitHubManager();
        repoManager.CreateBranch();
        
    }
}