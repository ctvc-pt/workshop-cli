namespace workshopCli;

public class CreateBranchAction : IAction
{
    public void Execute()
    {
       
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm( "Escreve 'proximo' ou 'p' para avançar (ou 'ajuda')" );
   
        var repoManager = new GitHubManager();
        repoManager.CreateBranch();
        
    }
}