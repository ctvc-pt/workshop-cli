using System.Diagnostics;
using System.IO.Compression;

namespace workshopCli;

public class GitHubManager
{
    public void CloneRepo()
    {
        var pythonScriptPath = $"{GuideCli.ResourcesPath}/github_clone.py"; // Replace with the actual path to your Python script
        
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

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        if (!string.IsNullOrEmpty(output))
            Console.WriteLine(output);

        if (!string.IsNullOrEmpty(error))
            Console.WriteLine(error);
    
    }
    public void CreateBranch()
    {
        Console.WriteLine("hrllo");
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

        var process = new Process { StartInfo = processStartInfo };
        process.Start();
        process.WaitForExit();

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        if (!string.IsNullOrEmpty(output))
            Console.WriteLine(output);

        if (!string.IsNullOrEmpty(error))
            Console.WriteLine(error);
    }

    public void Commit(string name)
    {
        var username = name;

        if (username != null)
        {
            username = username.Replace(" ", "-");
            Console.WriteLine(username);
        }
        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var sourceFolderPath = Path.Combine(desktopPath, $"{username}_{DateTime.Now.Year}");

        var folderPath = Path.Combine(desktopPath, "repoWorkshop", $"{username}_{DateTime.Now.Year}");

        var files = Directory.GetFiles(sourceFolderPath, "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var relativePath = file.Replace(sourceFolderPath, "").TrimStart(Path.DirectorySeparatorChar);
            var destPath = Path.Combine(folderPath, relativePath);
            var destDir = Path.GetDirectoryName(destPath);

            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
                Console.WriteLine($"Created folder: {destDir}");
            }

            File.Copy(file, destPath, true);
            Console.WriteLine($"Copied file: {relativePath}");
        }

       
        
        var pythonScriptPath = $"{GuideCli.ResourcesPath}/github_commit.py"; // Replace with the actual path to your Python script
        
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

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        if (!string.IsNullOrEmpty(output))
            Console.WriteLine(output);

        if (!string.IsNullOrEmpty(error))
            Console.WriteLine(error);
        
        
    }

   
}