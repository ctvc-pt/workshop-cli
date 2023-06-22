using Sharprompt;
using System.Diagnostics;

namespace workshopCli;

public class InformationAction : IAction
{
    public GuideCli Cli;
    public int Delay;

    public InformationAction(GuideCli cli,int delay)
    {
        Cli = cli;
        Delay = delay;
    }
    public void Execute()
    {
        
        Process[] processes = Process.GetProcessesByName("vlc");

        if (processes.Length > 0)
        {
            foreach (Process process in processes)
            {
                process.CloseMainWindow();
                process.WaitForExit();
            }
            //Console.WriteLine("VLC media player closed successfully.");
        }
        Thread.Sleep(Delay);
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm( "Escreve 'proximo' ou 'p' para avançar ou para retroceder escreve 'anterior' (ou 'ajuda')" );
    }
}