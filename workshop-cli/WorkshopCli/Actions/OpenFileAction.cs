using System.Diagnostics;

namespace workshopCli;

public class OpenFileAction : IAction
{

    public OpenFileAction()
    {
    }
    public void Execute()
    {
        string filename = "main.lua";
        var folderPath = Path.Combine( Environment.CurrentDirectory, "..", "..", "..", "..", "..", "Resources","mygame");
        var filePath = Path.Combine( Environment.CurrentDirectory, "..", "..", "..", "..", "..", "Resources","mygame", filename);
        if (!File.Exists(filePath)) 
        {
            File.Create(filePath);
        }
        try {
            ProcessStartInfo startFolderInfo = new ProcessStartInfo {
                FileName = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/../Local/Programs/Microsoft VS Code/Code.exe",
                Arguments = folderPath,
                WorkingDirectory = @"C:\",
                Verb = "runas"
            };
            Process.Start(startFolderInfo);
    
            ProcessStartInfo startFileInfo = new ProcessStartInfo {
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