using System.Diagnostics;
using Newtonsoft.Json;
using Sharprompt;
using System.Reflection;
using System.Runtime.InteropServices;

namespace workshopCli;

public class OpenFileL2DAction : IAction
{
    string stepId;
    public int Delay;
   
    public OpenFileL2DAction(string stepId, int delay)
    {
        this.stepId = stepId;
        Delay = delay;
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
        var folderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.ToString("dd-MM-yyyy")}", "mygame");
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
            
            //Open VS Code
            var startFolderInfo = new ProcessStartInfo {
                FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/../Local/Programs/Microsoft VS Code/Code.exe",
                Arguments = $"\"{ folderPath }\" --disable-workspace-trust",
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            Process.Start(startFolderInfo);
            Thread.Sleep(1000);
            
            //Open CLI using AutoHotKey
            var startAhkR = new ProcessStartInfo
            {
                FileName = Path.Combine( GuideCli.ResourcesPath,"AutoHotkey","v1.1.36.02","AutoHotkeyU64.exe"),
                Arguments = Path.Combine( GuideCli.ResourcesPath,"win-rigth.ahk"),
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            Process.Start(startAhkR);
            
            //Install extension on VS Code
            var startCommandInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = $"/C code --install-extension pixelbyte-studios.pixelbyte-love2d",
                WorkingDirectory = folderPath,
                Verb = "runas"
            };
            Process.Start(startCommandInfo);
            
            var settingsPath = Path.Combine(
                Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ),
                "Code",
                "User",
                "settings.json" );

            if (!File.Exists( settingsPath ))
            {
                string sourceFile = Path.Combine( GuideCli.ResourcesPath, "settings.json" );
                string destinationFolder = Path.Combine( 
                    Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ),
                    "Code",
                    "User",
                    "settings.json");
                File.Copy( sourceFile , destinationFolder, true );
            }

            var jsonset = File.ReadAllText( settingsPath );
            
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
                Arguments = $"\"{ filePath }\" --disable-workspace-trust",
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            
            //Process.Start(startFileInfo);
        } catch (Exception ex) {
            Console.WriteLine("Error opening file: " + ex.Message);
        }

        
        Thread.Sleep(500); 
        
        Console.Clear();
        
        var MDPath = $"{stepId}.md";
        var assembly = Assembly.GetExecutingAssembly();

        var resourceStream = assembly.GetManifestResourceStream( $"workshop_cli.Guide.{MDPath}" );
        if ( resourceStream != null )
        {
            using ( var reader = new StreamReader( resourceStream ) )
            {
                var fileContents = reader.ReadToEnd();
                Console.WriteLine( fileContents );
            }
        }
        
        Thread.Sleep(Delay);
        Console.ForegroundColor = ConsoleColor.Yellow;

        ExerciseHelper.PromptAnswerAndConfirm( "Verifica o código e escreve 'proximo' ou 'p' para continuar (ou 'ajuda')\n" );
    }
}