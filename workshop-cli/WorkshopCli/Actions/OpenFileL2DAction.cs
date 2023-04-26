using System.Diagnostics;
using System.Text.Json;
using Newtonsoft.Json;

namespace workshopCli;

public class OpenFileL2DAction : IAction
{

    public OpenFileL2DAction()
    {
    }
    public void Execute()
    {
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var txtFilePath =Path.Combine( Environment.CurrentDirectory, "..", "..", "..", "..", "..", "Resources",
            "session.txt" );
        if ( !File.Exists( txtFilePath ) )
        {
            return;
        }
        // Parse session file contents into a Session struct
        var session = JsonConvert.DeserializeObject<Session>(File.ReadAllText( txtFilePath ));

        // Get username from Session struct
        var username = session.Name;
        
        
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
                Arguments = folderPath,
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            Process.Start(startFolderInfo);
        
            var startFileInfo = new ProcessStartInfo {
                FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/../Local/Programs/Microsoft VS Code/Code.exe",
                Arguments = filePath,
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            Process.Start(startFileInfo);
        } catch (Exception ex) {
            Console.WriteLine("Error opening file: " + ex.Message);
        }
    }
}