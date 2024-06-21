using System.Diagnostics;
using System.IO;

namespace workshopCli;

public class GitHubManager
{
    public void CreateBranch()
    {
        var pythonScriptPath = $"{GuideCli.ResourcesPath}/github_branch.py"; // Replace with the actual path to your Python script
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = pythonScriptPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        
        try
        {
            var process = new Process { StartInfo = processStartInfo };
            process.Start();
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing Python script: {ex.Message}");
        }
    }

    public void Commit(string name)
    {
        var username = name;

        if (username != null)
        {
            username = username.Replace(" ", "-");
        }
        var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "repoWorkshop");

        var sourceFolderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now:dd-MM-yyyy}");

        var folderPath = Path.Combine(GuideCli.ResourcesPath, "Workshop2023");

        var files = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var relativePath = file.Replace(sourceFolderPath, "").TrimStart(Path.DirectorySeparatorChar);
            var destPath = Path.Combine(folderPath, relativePath);
            var destDir = Path.GetDirectoryName(destPath);

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            File.Copy(file, destPath, true);
        }

        var pythonScriptPath = $"{GuideCli.ResourcesPath}/github_commit.py";

        var processStartInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = pythonScriptPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        Process process = null;
        try
        {
            process = new Process { StartInfo = processStartInfo };

            // Start the process
            process.Start();

            // Begin asynchronous read operations for output and error streams
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            // Wait for the process to exit
            process.WaitForExit();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error executing Python script: {ex.Message}");
        }
        BackupScript(username);
    }

    public void BackupScript(string username)
    {
        var desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "repoWorkshop");
        var folderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.ToString("dd-MM-yyyy")}", "mygame");
        string sourceFilePath = Path.Combine(folderPath, "main.lua");
        string destinationFilePath =  $"{GuideCli.ResourcesPath}/backup.lua";

        try
        {
            string content = File.ReadAllText(sourceFilePath);

            File.WriteAllText(destinationFilePath, content);

            Console.WriteLine("Conteúdo copiado com sucesso.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ocorreu um erro ao fazer backup: " + ex.Message);
        }
    }
}
