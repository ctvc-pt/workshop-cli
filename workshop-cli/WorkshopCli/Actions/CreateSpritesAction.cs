using System.Diagnostics;
using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli;

public class CreateSpritesAction: IAction
{
    public int Delay;
    public CreateSpritesAction(int delay)
    {
        Delay = delay;
    }
    public void Execute()
    {
        var sourceFolderPath = Path.Combine( GuideCli.ResourcesPath,"Images");
        var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "repoWorkshop");
        var txtFilePath =Path.Combine( GuideCli.ResourcesPath,"session.txt" );
        
        if ( !File.Exists( txtFilePath ) )
        {
            return;
        }
      
        var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText( txtFilePath ));
        var username = session.Name;
        if(username != null) username = username.Replace(" ", "-");
        
        var folderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.ToString("dd-MM-yyyy")}", "mygame");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string[] files = Directory.GetFiles(sourceFolderPath);
        
        foreach (string file in files)
        {
            string fileName = Path.GetFileName(file);
            string destPath = Path.Combine(folderPath, fileName);
            File.Copy(file, destPath, true);
        }
        
        Thread.Sleep(Delay);
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm( "Escreve 'proximo' ou 'p' para avançar ou para retroceder escreve 'anterior' (ou 'ajuda')" );
    }
}