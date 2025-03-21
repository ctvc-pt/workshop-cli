using Sharprompt;

namespace workshopCli;

public class ChallengeAction : IAction
{
    public int Delay;
    public string SessionName;

    public ChallengeAction( int delay, string sessionName )
    {
        Delay = delay;
        SessionName = sessionName;
    }
    public void Execute()
    {
        Thread.Sleep(Delay);
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm("Quando completares o desafio, escreve 'proximo' ou 'p' para avançar ou para retroceder escreve 'anterior' (ou 'ajuda')\n");
        
        /*var repoManager = new GitHubManager();
        repoManager.Commit(SessionName);*/
    }
}