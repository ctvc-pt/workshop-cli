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
        var sourceFolderPath = Path.Combine( GuideCli.ResourcesPath,"Sprites");
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var txtFilePath =Path.Combine( GuideCli.ResourcesPath,"session.txt" );
        
        if ( !File.Exists( txtFilePath ) )
        {
            return;
        }
      
        var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText( txtFilePath ));
        var username = session.Name;
        username = username.Replace(" ", "-");
        
        var folderPath = Path.Combine(desktopPath,$"{username}_{DateTime.Now.Year}","mygame","Sprites");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        File.Copy(Path.Combine(sourceFolderPath, "parrot.png"), Path.Combine(folderPath, "parrot.png"), true);
        File.Copy(Path.Combine(sourceFolderPath, "background.png"), Path.Combine(folderPath, "background.png"), true);

        Prompt.Confirm("Quando completares o desafio avança para a frente\n", false);
    }
}