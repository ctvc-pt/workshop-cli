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
        
        int totalSeconds = Delay;
        for (int i = totalSeconds; i > 0; i--)
        {
            //Console.Write($"\rTime remaining: {i} seconds");
            Console.Out.Flush();
            Thread.Sleep(1000);
            KeyPress.SimulateKeyPress();
        }
        KeyPress.SimulateKeyPress();
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm( "Escreve 'proximo' ou 'p' para avançar ou para retroceder escreve 'anterior' (ou 'ajuda')" );
    }
}