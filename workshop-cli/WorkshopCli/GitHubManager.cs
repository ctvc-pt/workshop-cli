using System.Diagnostics;
using System.IO.Compression;

namespace workshopCli;

public class GitHubManager
{
    public void CreateBranch()
    {
        //Console.WriteLine("create branch");
        
        var pythonScriptPath = $"{GuideCli.ResourcesPath}/git_workflow.py"; // Replace with the actual path to your Python script
        
        var processStartInfo = new ProcessStartInfo
        {
            FileName = "python",
            Arguments = pythonScriptPath,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        var process = new Process { StartInfo = processStartInfo };
        process.Start();
        process.WaitForExit();
    }

    public void Commit(string name)
    {
        Console.WriteLine("commit");
        var username = name;

        if (username != null)
        {
            username = username.Replace(" ", "-");
            //Console.WriteLine(username);
        }
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var sourceFolderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.ToString("dd-MM-yyyy")}");

        var folderPath = Path.Combine(GuideCli.ResourcesPath,"Workshop2023");

        var files = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var relativePath = file.Replace(sourceFolderPath, "").TrimStart(Path.DirectorySeparatorChar);
            var destPath = Path.Combine(folderPath, relativePath);
            var destDir = Path.GetDirectoryName(destPath);

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
                //Console.WriteLine($"Created folder: {destDir}");
            }

            File.Copy(file, destPath, true);
            //Console.WriteLine($"Copied file: {relativePath}");
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

        var process = new Process { StartInfo = processStartInfo };
        process.Start();
        process.WaitForExit();
    }

   
}