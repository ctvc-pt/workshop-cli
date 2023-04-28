using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;
using Sharprompt;

namespace workshopCli;

public class OpenFileL2DAction : IAction
{

    public OpenFileL2DAction()
    {
    }
    public void Execute()
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var txtFilePath = Path.Combine( GuideCli.ResourcesPath,"session.txt" );
        if ( !File.Exists( txtFilePath ) )
        {
            return;
        }
        
        var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText( txtFilePath ));
        
        var username = session.Name;
        if ( username != null )
        {
            username = username.Replace(" ", "-");
        }

        var folderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.Year}", "mygame");
        var filePath = Path.Combine(folderPath, "main.lua");
           
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            using var sw = File.CreateText(filePath);
            sw.WriteLine("--É aqui onde começa a tua aventura");
            /*
                 * NOTE:
                 * we need to call '.Close()' on the returned stream from 'File.Create'
                 * to ensure that the file is closed properly before we try to open it with Visual Studio Code.
                 */
        }
        try {
            var startFolderInfo = new ProcessStartInfo {
                FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/../Local/Programs/Microsoft VS Code/Code.exe",
                Arguments = $"\"{ folderPath }\"",
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            Process.Start(startFolderInfo);
        
            var startFileInfo = new ProcessStartInfo {
                FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/../Local/Programs/Microsoft VS Code/Code.exe",
                Arguments = $"\"{ filePath }\"",
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            //Process.Start(startFileInfo);
        } catch (Exception ex) {
            Console.WriteLine("Error opening file: " + ex.Message);
        }
        
        Prompt.Confirm("Verifica o código e clica ENTER para continuar\n", false);
    }
}