using System.Diagnostics;
using Newtonsoft.Json;
using Sharprompt;
using System.Runtime.InteropServices;

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
            
            var startCommandInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = $"/C code --install-extension pixelbyte-studios.pixelbyte-love2d",
                WorkingDirectory = folderPath,
                Verb = "runas"
            };
            Process.Start(startCommandInfo);
            
            string settingsPath = Path.Combine(
                Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ),
                "Code",
                "User",
                "settings.json" );

            string jsonset = File.ReadAllText( settingsPath );
            Console.WriteLine("2");
            // Check if the "files.autoSave" setting is already present in the JSON
            if ( !jsonset.Contains( "\"files.autoSave\":" ) )
            {
                // Add the "files.autoSave" setting with the value "afterDelay"
                jsonset = jsonset.TrimEnd();
                if ( jsonset.EndsWith( "}" ) )
                {
                    jsonset = jsonset.Substring( 0, jsonset.Length - 1 );
                }

                jsonset += ",\n  \"files.autoSave\": \"afterDelay\"\n}";
            }
            else
            {
                // Replace the "files.autoSave" value with "afterDelay" if it exists
                jsonset = jsonset.Replace( "\"files.autoSave\": \"off\"", "\"files.autoSave\": \"afterDelay\"" );
            }

            File.WriteAllText( settingsPath, jsonset );
            
            KeyboardShortcut.AddKeyboardShortcut();
            

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

        ExerciseHelper.PromptAnswerAndConfirm( "Verifica o código e clica ENTER para continuar\n" );
        //Prompt.Confirm("Verifica o código e clica ENTER para continuar\n", false);
    }
}