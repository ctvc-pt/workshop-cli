using System.Diagnostics;
using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli;

public class CreateSpritesAction: IAction
{
    public CreateSpritesAction()
    {
    }

    public void Execute()
    {
        var sourceFolderPath = Path.Combine( GuideCli.ResourcesPath,"Images");
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var txtFilePath =Path.Combine( GuideCli.ResourcesPath,"session.txt" );
        
        if ( !File.Exists( txtFilePath ) )
        {
            return;
        }
      
        var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText( txtFilePath ));
        var username = session.Name;
        if(username != null) username = username.Replace(" ", "-");
        
        var folderPath = Path.Combine(desktopPath,$"{username}_{DateTime.Now.Year}","mygame","Images");

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
        
        Console.ForegroundColor = ConsoleColor.Yellow;
        ExerciseHelper.PromptAnswerAndConfirm( "Escreve 'proximo' ou 'p' para avançar" );
        //Prompt.Confirm("Quando completares o desafio avança para a frente\n", false);
    }
}